using Serilog;
using System.Net;
using System.Text.Json;
using NCRFiscalManager.Core.DTO.NCR;
using NCRFiscalManager.Core.DTO.Facture.Request;
using NCRFiscalManager.Core.DTO.Facture.Response;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Enums;
using NCRFiscalManager.Core.Interfaces;
using NCRFiscalManager.Core.Interfaces.Facture;
using System.Reflection.Metadata;
using System.Globalization;

namespace NCRFiscalManager.Core.Services.Facture;

public class DocumentManagerFactureService : IDocumentManagerFactureService
{
    private readonly ILogger _log;
    private readonly IMapperFactureService _mapperFactureService;
    private readonly ISendDocumentFactureService _sendDocumentFactureService;
    private readonly IInvoiceTransactionPersistenceService _invoiceTransactionPersistenceService;
    private readonly IPointOfSalesService _pointOfSalesService;

    public DocumentManagerFactureService( ILogger log, IMapperFactureService mapperFactureService, ISendDocumentFactureService sendDocumentFactureService, IInvoiceTransactionPersistenceService invoiceTransactionPersistenceService, IPointOfSalesService pointOfSalesService )
    {
        _log = log;
        _mapperFactureService = mapperFactureService;
        _sendDocumentFactureService = sendDocumentFactureService;
        _invoiceTransactionPersistenceService = invoiceTransactionPersistenceService;
        _pointOfSalesService = pointOfSalesService;
    }

    /// <summary>
    /// Método para reportar y los datos al operador tecnológico GoSocket para generar la factura electrónica.
    /// </summary>
    /// <param name="document">Objeto DocumentoDTO con los datos a organizar y enviar al operador tecnológico</param>
    /// <returns>Retorna objeto ResponseDTO con el resultado de la transacción</returns>
    public ResponseDTO ProcessDocumentFacture( DocumentDTO document )
    {
        ResponseDTO responseDto = new ResponseDTO();
        responseDto.AuhorizedDate = DateTime.Now.ToString("yyyyMMdd");
        try
        {
            ResponseDocumentFactureDTO responseFacture = null;
            string request = String.Empty;
            int tipoDocumentoFiscal = (int) TipoDocumentoFiscalEnum.FacturaElectronica;
            string correlativoFiscal = document.CorrelativoFiscal;
            // Obtiener la llave técnica del PDV dependiendo de la resolución.
            var pointOfSale = _pointOfSalesService.GetPointOfSaleByResolutionSerial(document.EmisorIdExterno, $"{document.NumeroResolucion}-{document.NumeroSerie}");

            if ( document.DocumentoReferencia == null )
            {
                Factura factureInvoice = _mapperFactureService.GetFactureClientDTO(document);
                request = JsonSerializer.Serialize(factureInvoice);
                responseFacture = _sendDocumentFactureService.SendFactureInvoice(factureInvoice, pointOfSale.LlaveTecnica);
            }
            else
            {
               var referencedInvoice = _invoiceTransactionPersistenceService.GetInvoiceTransactionByCufe(document.DocumentoReferencia.CufeReference);

                if(referencedInvoice != null)
                {
                    var referencedInvoiceRequest = JsonSerializer.Deserialize<Factura>(referencedInvoice.Request);

                    document.RecargoPorServicio = referencedInvoiceRequest.DescuentosOCargos.DescuentoOCargo.Count != 0 ? float.Parse(referencedInvoiceRequest.DescuentosOCargos.DescuentoOCargo.FirstOrDefault().Valor, CultureInfo.InvariantCulture) : 0.0f;
                }

                NotaCredito creditNote = _mapperFactureService.GetNotaCredito(document);
                request = JsonSerializer.Serialize(creditNote);
                tipoDocumentoFiscal = (int) TipoDocumentoFiscalEnum.NotaCredito;
                responseFacture = _sendDocumentFactureService.SendFactureCreditNote(creditNote, pointOfSale.LlaveTecnica);
                correlativoFiscal = document.DocumentoReferencia.CorrelativoFiscal;
            }

            if ( responseFacture != null && responseFacture.IsSuccess )
            {
                responseDto.Cufe = responseFacture.UUID;
                responseDto.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                responseDto.Cufe = Constants.Constant.CufeNoGenerado;
                responseDto.StatusCode = HttpStatusCode.InternalServerError;
                responseDto.Message = responseFacture.ValidatingErrorMessages;
            }

            InvoiceTransaction invoiceTransaction = new InvoiceTransaction()
            {
                Request = request,
                Response = JsonSerializer.Serialize(responseFacture),
                FiscalCorrelative = correlativoFiscal,
                CUFE = responseDto.Cufe,
                TipoDocumentoFiscal = tipoDocumentoFiscal
            };
            _invoiceTransactionPersistenceService.SaveInvoiceTransaction(invoiceTransaction, document.EmisorIdExterno, document.NumeroResolucion + "-" + document.NumeroSerie);

        }
        catch ( Exception e )
        {
            _log.Error($"Error send Facture document {e.Message}");
            responseDto.StatusCode = HttpStatusCode.InternalServerError;
            responseDto.Message = e.Message;
        }
        return responseDto;
    }

    public ResponseDTO ProcessResendDocumentFacture( Factura factura, string storeKey )
    {
        ResponseDTO responseDto = new ResponseDTO();
        responseDto.AuhorizedDate = DateTime.Now.ToString("yyyyMMdd");
        try
        {

            // Obtiener la llave técnica del PDV dependiendo de la resolución.

            _log.Information($"Resend Facture document: StoreKey: {storeKey}, {factura.Cabecera.Numero}");
            var pointOfSale = _pointOfSalesService.GetPointOfSaleByResolutionSerial(storeKey, factura.NumeracionDIAN.NumeroResolucion + "-" + factura.NumeracionDIAN.PrefijoNumeracion);

            var responseFacture = _sendDocumentFactureService.SendFactureInvoice(factura, pointOfSale.LlaveTecnica);


            if ( responseFacture != null && responseFacture.IsSuccess )
            {
                responseDto.Cufe = responseFacture.UUID;
                responseDto.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                responseDto.Cufe = Constants.Constant.CufeNoGenerado;
                responseDto.StatusCode = HttpStatusCode.InternalServerError;
                responseDto.Message = responseFacture.ValidatingErrorMessages;
            }

        }
        catch ( Exception e )
        {
            _log.Error($"Error send Facture document {e.Message}");
            responseDto.StatusCode = HttpStatusCode.InternalServerError;
            responseDto.Message = e.Message;
        }
        return responseDto;
    }
}