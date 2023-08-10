using Serilog;
using System.Net;
using System.Text.Json;
using NCRFiscalManager.Core.Constants;
using NCRFiscalManager.Core.DTO.NCR;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Enums;
using NCRFiscalManager.Core.Interfaces;
using NCRFiscalManager.Core.Interfaces.Facture;
using NCRFiscalManager.Core.Interfaces.GoSocket;
using NCRFiscalManager.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using NCRFiscalManager.Core.DTO.Facture.Request;

namespace NCRFiscalManager.Core.Services
{
    public class DocumentManagerService : IDocumentManagerServices
    {
        private readonly IDocumentManagerTecnoFactorService _documentManagerTecnoFactor;
        private readonly IDocumentManagerGoSocketService _documentManagerGoSocket;
        private readonly IDocumentManagerFactureService _documentManagerFacture;
        private readonly IInvoiceTransactionPersistenceService _invoiceTransactionPersistenceService;
        private readonly IEmitterInvoiceService _emitterInvoiceService;
        private readonly IBlackListedItemsService _blackListedItemsService;
        private readonly IConfiguration _configuration;

        public DocumentManagerService( IDocumentManagerTecnoFactorService documentManagerTecnoFactorService, IDocumentManagerGoSocketService documentManagerGoSocket,
            IDocumentManagerFactureService documentManagerFacture, IInvoiceTransactionPersistenceService invoiceTransactionPersistenceService, IEmitterInvoiceService emitterInvoiceService, IBlackListedItemsService blackListedItemsService, IConfiguration configuration )
        {
            _documentManagerTecnoFactor = documentManagerTecnoFactorService;
            _documentManagerGoSocket = documentManagerGoSocket;
            _documentManagerFacture = documentManagerFacture;
            _invoiceTransactionPersistenceService = invoiceTransactionPersistenceService;
            _emitterInvoiceService = emitterInvoiceService;
            _blackListedItemsService = blackListedItemsService;
            _configuration = configuration;
        }

        /// <summary>
        /// Servicio encargado de recibir un documento, procesarlo y enviarlo al operador tecnológico.
        /// </summary>
        /// <param name="document">Objeto de tipo DocumentDTO con los datos a enviar al Operador Tecnológico</param>
        /// <param name="idOperadorTecnologico">Indica el operador tecnológico</param>
        /// <returns>Retorna objeto ResponseDTO con el resultado de la transacción</returns>
        public ResponseDTO ProcessDocument( DocumentDTO document, int idOperadorTecnologico )
        {
            try
            {
                Log.Information($"Request from AFM Fiscal document: {JsonSerializer.Serialize(document)}");

                ResponseDTO responseDto = GetValidateCufe(document);
                if ( responseDto != null )
                {
                    return responseDto;
                }

                GetDeliveryItemToCharges(document);
                GetTipsFromChuckECheese(document);
                GetEmitterInvoiceItemsConfigurations(document);

                switch ( idOperadorTecnologico )
                {
                    case (int) Constant.ProveedorTecnologico.TECNOFACTOR:
                        return _documentManagerTecnoFactor.ProcessDocumentTecnofactor(document);
                    case (int) Constant.ProveedorTecnologico.GOSOCKET:
                        return _documentManagerGoSocket.ProcessDocumentGoSocket(document);
                    case (int) Constant.ProveedorTecnologico.FACTURE:
                        return _documentManagerFacture.ProcessDocumentFacture(document);
                    default:
                        return new ResponseDTO()
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            Message = "No existe el operador tecnológico",
                            AuhorizedDate = DateTime.Now.ToString("yyyyMMdd")
                        };
                }
            }
            catch ( Exception e )
            {
                Log.Error($"{Constant.ErrorMessageServiceLog}: {e}");
                return new ResponseDTO()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = e.Message,
                    AuhorizedDate = DateTime.Now.ToString("yyyyMMdd")
                };
            }
        }

        public ResponseDTO ProcessResendDocumentFacture( Factura factura, string storeKey )
        {
            try
            {
                //                Log.Information($"Request from AFM Fiscal document: {JsonSerializer.Serialize(document)}");
                return _documentManagerFacture.ProcessResendDocumentFacture(factura, storeKey);

            }

            catch ( Exception e )
            {
                Log.Error($"{Constant.ErrorMessageServiceLog}: {e}");
                return new ResponseDTO()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = e.Message,
                    AuhorizedDate = DateTime.Now.ToString("yyyyMMdd")


                };
            }
        }

        /// <summary>
        /// Obtener el monto del item Domicilio y agregarlo a los cargos, elmininando el articulo de la lista.
        /// </summary>
        /// <param name="document"></param>
        private void GetDeliveryItemToCharges( DocumentDTO document )
                {
                    // Solo se hace para PJ.
                    var papaJohnsNit = _configuration.GetSection("NITs:PJ").Value;
                    if ( document.EmisorId != papaJohnsNit )
                        return;

                    var deliveryItemsId = _configuration.GetSection("DeliveryItemsPapaJohns").Get<List<string>>();
                    var deliveryItemInDocument = document.Articulos.FirstOrDefault(x => deliveryItemsId.Contains(x.CodigoArticulo));

                    if ( deliveryItemInDocument != null )
                    {
                        document.RecargoModoPedido = deliveryItemInDocument.Total;
                        document.Articulos.Remove(deliveryItemInDocument);

                        return;
                    }

                    return;
                }

                /// <summary>
                /// Obtener el monto del item PROPINA y reemplazarlo al valor del recargo por servicio, eliminando el articulo propina de la lista de articulos.
                /// </summary>
                /// <param name="document"></param>
                private void GetTipsFromChuckECheese( DocumentDTO document )
                {
                    var tipItemAlohaId = _configuration.GetSection("TipsChuckECheese:TipId").Value;
                    var tipsItem = document.Articulos.FirstOrDefault(x => x.CodigoArticulo == tipItemAlohaId);

                    if ( tipsItem != null )
                    {
                        document.RecargoPorServicio = tipsItem.Total;
                        document.Articulos.Remove(tipsItem);

                        return;
                    }

                    return;
                }

                /// <summary>
                /// Verificar las configuraciones del emisor para el tratamiento de los productos a ignorar en la factura.
                /// </summary>
                /// <param name="document">Objeto DocumentDTO con la información de la transacción</param>
                private void GetEmitterInvoiceItemsConfigurations( DocumentDTO document )
                {
                    var itemZeroConfiguration = _emitterInvoiceService.GetZeroPriceItemConfigurationOfEmitterInvoice(document.EmisorId);

                    if ( itemZeroConfiguration.IgnoresAllPriceZeroItems )
                    {
                        document.Articulos.RemoveAll(x => x.Total == 0);
                        return;
                    }

                    if ( itemZeroConfiguration.UsesBlackList )
                    {
                        var blackListedItemsCodes = _blackListedItemsService.GetBlackListedItemsCodesByEmitterInvoice(document.EmisorId);
                        document.Articulos.RemoveAll(x => blackListedItemsCodes.Any(n => x.CodigoArticulo.Equals(n)));

                        return;
                    }

                    return;
                }

                /// <summary>
                /// Método para validar si la operación ha sido enviada con anterioridad a la DIAN.
                /// </summary>
                /// <param name="documentDto">Objeto de tipo DocumentDTO con los datos a validar</param>
                /// <returns>Retorna objeto ResponseDTO con los datos obtenidos. Si retorna null es porque no hay transacciones previas</returns>
                private ResponseDTO GetValidateCufe( DocumentDTO documentDto )
                {
                    try
                    {
                        ResponseDTO responseDto = null;
                        string correlativoFiscal = documentDto.CorrelativoFiscal;
                        int tipoDocumentoFiscal = (int) TipoDocumentoFiscalEnum.FacturaElectronica;

                        if ( documentDto.DocumentoReferencia != null )
                        {
                            correlativoFiscal = documentDto.DocumentoReferencia.CorrelativoFiscal;
                            tipoDocumentoFiscal = (int) TipoDocumentoFiscalEnum.NotaCredito;
                        }

                        InvoiceTransaction invoiceTransaction =
                            _invoiceTransactionPersistenceService.GetInvoiceTransactionByStoreKeyAndCorrelative(
                                documentDto.EmisorIdExterno, correlativoFiscal, tipoDocumentoFiscal, documentDto.NumeroResolucion + "-" + documentDto.NumeroSerie);
                        if ( invoiceTransaction != null )
                        {
                            responseDto = new ResponseDTO()
                            {
                                Cufe = invoiceTransaction.CUFE,
                                Message = "La Factura o Nota Crédito ya ha sido procesada con anterioridad",
                                StatusCode = HttpStatusCode.OK,
                                AuhorizedDate = invoiceTransaction.CreatedAt.ToString("yyyyMMdd")
                            };
                        }

                        return responseDto;
                    }
                    catch ( Exception e )
                    {
                        Log.Error($"Error GetValidateCufe {e}");
                        return null;
                    }
                }
            }
        }
