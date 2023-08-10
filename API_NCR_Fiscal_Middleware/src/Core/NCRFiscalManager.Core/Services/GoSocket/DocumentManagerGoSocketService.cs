using NCRFiscalManager.Core.DTO.GoSocket;
using NCRFiscalManager.Core.DTO.NCR;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Enums;
using NCRFiscalManager.Core.Interfaces;
using NCRFiscalManager.Core.Interfaces.GoSocket;
using NCRFiscalManager.Core.Interfaces.Repositories;
using Serilog;
using System.Net;
using System.Text.Json;

namespace NCRFiscalManager.Core.Services
{
    public class DocumentManagerGoSocketService : IDocumentManagerGoSocketService
    {
        private readonly ILogger _log;
        private readonly IMapperGoSocketService _mapperGoSocketService;
        private readonly ISendDocumentGoSocketService _goSocketService;
        private readonly ITechOperatorEmitterInVoiceRepository _techOperatorEmitterInVoiceRepository;
        private readonly IInvoiceTransactionPersistenceService _invoiceTransactionPersistenceService;

        public DocumentManagerGoSocketService(ILogger log, IMapperGoSocketService mapperGoSocketService, ISendDocumentGoSocketService goSocketService, ITechOperatorEmitterInVoiceRepository techOperatorEmitterInVoiceRepository, IInvoiceTransactionPersistenceService invoiceTransactionPersistenceService)
        {
            _log = log;
            _mapperGoSocketService = mapperGoSocketService;
            _goSocketService = goSocketService;
            _techOperatorEmitterInVoiceRepository = techOperatorEmitterInVoiceRepository;
            _invoiceTransactionPersistenceService = invoiceTransactionPersistenceService;
        }

        /// <summary>
        /// Método para reportar y los datos al operador tecnológico GoSocket para generar la factura electrónica.
        /// </summary>
        /// <param name="document">Objeto DocumentoDTO con los datos a organizar y enviar al operador tecnológico</param>
        /// <returns>Retorna objeto ResponseDTO con el resultado de la transacción</returns>
        public ResponseDTO ProcessDocumentGoSocket(DocumentDTO document)
        {
            ResponseDTO responseDto = new ResponseDTO();
            responseDto.AuhorizedDate = DateTime.Now.ToString("yyyyMMdd");

            try
            {
                TechOperatorEmitterInVoice techOperatorData =
                    _techOperatorEmitterInVoiceRepository.GetByEmitterInvoiceNit(document.EmisorId);
                ConnectionDataGoSocketDTO connectionData =
                    JsonSerializer.Deserialize<ConnectionDataGoSocketDTO>(techOperatorData.ConnectionData);
                DTE documentDte = _mapperGoSocketService.GetGoSocketClientDTO(document, connectionData.ClaveTcNotaCredito);

                int tipoDocumentoFiscal = (int)TipoDocumentoFiscalEnum.FacturaElectronica;
                string correlativoFiscal = document.CorrelativoFiscal;

                if (document.DocumentoReferencia != null)
                {
                    tipoDocumentoFiscal = (int)TipoDocumentoFiscalEnum.NotaCredito;
                    correlativoFiscal = document.DocumentoReferencia.CorrelativoFiscal;
                }

                ResponseDocumentGoSocketDTO response = _goSocketService.SendDocumentToGoSocket(documentDte, techOperatorData);

                if (response.Success)
                {
                    responseDto.StatusCode = HttpStatusCode.OK;
                    responseDto.Cufe = response.CountryDocumentId;
                }
                else
                {
                    responseDto.Cufe = Constants.Constant.CufeNoGenerado; ;
                    responseDto.StatusCode = HttpStatusCode.InternalServerError;
                }

                var JsonGoSo = JsonSerializer.Serialize(response);

                responseDto.Message = response.Description;

                InvoiceTransaction invoiceTransaction = new InvoiceTransaction()
                {
                    Request = JsonSerializer.Serialize(documentDte),
                    Response = JsonSerializer.Serialize(response),
                    FiscalCorrelative = correlativoFiscal,
                    CUFE = responseDto.Cufe,
                    TipoDocumentoFiscal = tipoDocumentoFiscal
                };
                _invoiceTransactionPersistenceService.SaveInvoiceTransaction(invoiceTransaction, document.EmisorIdExterno, document.NumeroResolucion + "-" + document.NumeroSerie);

            }
            catch (Exception e)
            {
                _log.Error($"Error send GoSocket document {e.Message}");
                responseDto.StatusCode = HttpStatusCode.InternalServerError;
                responseDto.Message = e.Message;
            }

            return responseDto;
        }
    }
}

