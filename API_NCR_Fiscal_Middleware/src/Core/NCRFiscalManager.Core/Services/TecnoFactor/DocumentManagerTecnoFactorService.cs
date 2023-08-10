using Serilog;
using System.Net;
using System.Text.Json;
using NCRFiscalManager.Core.Constants;
using NCRFiscalManager.Core.DTO.NCR;
using NCRFiscalManager.Core.DTO.TecnoFactor;
using NCRFiscalManager.Core.Interfaces;

namespace NCRFiscalManager.Core.Services
{
    public class DocumentManagerTecnoFactorService : IDocumentManagerTecnoFactorService
    {
        private readonly ISendDocumentTecnoFactorService _sendDocument;
        private readonly IMapperTecnoFactorService _mapperTecnoFactorService;
        private readonly ILogger _log;

        public DocumentManagerTecnoFactorService(ISendDocumentTecnoFactorService sendDocumentTecnoFactorService,
            IMapperTecnoFactorService mapperTecnoFactorService, ILogger log)
        {
            _sendDocument = sendDocumentTecnoFactorService;
            _mapperTecnoFactorService = mapperTecnoFactorService;
            _log = log;
        }
        
        /// <summary>
        /// Método para reportar el documento al operador tecnológico TecnoFactor.
        /// </summary>
        /// <param name="document">Objeto de tipo DocumentDTO con los datos a enviar al Operador Tecnológico</param>
        /// <returns>Retorna objeto ResponseDTO con el resultado de la transacción</returns>
        public ResponseDTO ProcessDocumentTecnofactor(DocumentDTO document)
        {
            ResponseDTO responseDto = new ResponseDTO();
            responseDto.AuhorizedDate = DateTime.Now.ToString("yyyyMMdd");

            try
            {
                // Mapper
                _log.Information("Initialize Mapper");
                RequestDocumentDTO requestDocumentoDto = _mapperTecnoFactorService.GetTecnoFactorClientDTO(document);

                // Get Token
                ResponseTokenDTO responseTokenDto = _sendDocument.GenerateAuthenticationToken();
                if (Convert.ToInt16(responseTokenDto.Estado.Codigo) != (int)HttpStatusCode.OK)
                {
                    responseDto.StatusCode = HttpStatusCode.Forbidden;
                    responseDto.Message = responseTokenDto.Estado.Descripcion;
                    return responseDto;
                }

                RequestConsultarDocumentoDTO requestConsultarDocumentoDto = new RequestConsultarDocumentoDTO()
                {
                    Prefijo = document.NumeroSerie,
                    NumeroDocumento = document.CorrelativoFiscal,
                    TipoDocumento = "VENTA"
                };
                ResponseConsultarDocumentoDTO responseConsultarDocumentoDto =
                    _sendDocument.ConsultDocumentTecnofactor(requestConsultarDocumentoDto, responseTokenDto.Token);
                if (responseConsultarDocumentoDto.Data != null && responseConsultarDocumentoDto.Data.Estado && validateJson(responseConsultarDocumentoDto.Data.Descripcion))
                {
                    return validateEstado(responseConsultarDocumentoDto.Data.Descripcion);
                }
                ResponseDocumentDTO responseDocumentDto = _sendDocument.SendDocumentToTecnofactor(requestDocumentoDto, responseTokenDto.Token);
                if (Convert.ToInt16(responseDocumentDto.Estado.Codigo) == (int)HttpStatusCode.OK)
                {
                    Thread.Sleep(1000);
                    responseConsultarDocumentoDto =
                        _sendDocument.ConsultDocumentTecnofactor(requestConsultarDocumentoDto, responseTokenDto.Token);
                    if (responseConsultarDocumentoDto.Data.Estado && responseConsultarDocumentoDto.Data.Descripcion != null)
                    {
                        return validateEstado(responseConsultarDocumentoDto.Data.Descripcion);
                    }
                }
                responseDto.StatusCode = HttpStatusCode.InternalServerError;
                responseDto.Message = responseDocumentDto.Estado.Descripcion;
            }
            catch (Exception e)
            {
                _log.Error($"Error send TecnoFactor document {e.Message}");
                responseDto.StatusCode = HttpStatusCode.InternalServerError;
                responseDto.Message = e.Message;
            }

            return responseDto;
        }
        
        /// <summary>
            /// Método para validar si la estructura del JSON es correcta.
            /// </summary>
            /// <param name="strDescripcion">String de JSON a validar</param>
            /// <returns>Retorna resultado de la validación. True el JSON es correcto, False en caso contrario</returns>
            private bool validateJson(string strDescripcion)
            {
                try
                {
                    JsonSerializer.Deserialize<ResponseDocumentDTO>(strDescripcion);
                    return true;
                }
                catch (Exception e)
                {
                    _log.Error($"{Constant.ErrorMessageServiceLog}: {e}");
                    return false;
                }
            }

            /// <summary>
            /// Método para validar la respuesta del estado del documento en Tecnofactor.
            /// </summary>
            /// <param name="strDescripcion">JSON con la respuesta del estado</param>
            /// <returns>Retorna objeto ResponseDTO con el resultado de la transacción</returns>
            /// <exception cref="Exception"></exception>
            private ResponseDTO validateEstado(string strDescripcion)
            {
                try
                {
                    ResponseDTO responseDto = new ResponseDTO();
                    ResponseDocumentDTO responseDescription =
                        JsonSerializer.Deserialize<ResponseDocumentDTO>(strDescripcion);
                    
                    if (responseDescription.Estado.Codigo == "EXITOSO")
                    {
                        responseDto.StatusCode = HttpStatusCode.OK;
                        responseDto.Cufe = responseDescription.Cufe;
                    }
                    else
                    {
                        responseDto.StatusCode = HttpStatusCode.InternalServerError;
                    }

                    responseDto.Message = responseDescription.Estado.Descripcion;
                    responseDto.AuhorizedDate = DateTime.Now.ToString("yyyyMMdd");
                    return responseDto;
                }
                catch (Exception e)
                {
                    _log.Error($"{Constant.ErrorMessageServiceLog}: {e}");
                    throw e;
                }
            }
    }
}

