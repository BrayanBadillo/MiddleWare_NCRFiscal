using RestSharp;
using Serilog;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using NCRFiscalManager.Core.Constants;
using NCRFiscalManager.Core.Interfaces;
using NCRFiscalManager.Core.DTO.TecnoFactor;
using NCRFiscalManager.Core.Utilities;

namespace NCRFiscalManager.Infraestructure.ThirdParty.Tecnofactor.Services
{
    public class SendDocumentTecnofactorService : ISendDocumentTecnoFactorService
    {
        private readonly ILogger _log;
        private readonly IConfiguration _configuration;
        public const string TecnoFactorSettings = "TecnoFactorSettings";
        private const string UrlTecnofactorSetting = $"{TecnoFactorSettings}:Url";
        public SendDocumentTecnofactorService(ILogger log, IConfiguration configuration)
        {
            _log = log;
            _configuration = configuration;
        }

        /// <summary>
        /// Servicio de autenticación para TecnoFactor.
        /// </summary>
        /// <returns>Retorna objeto ResponseTokenDTO con los datos del token</returns>
        public ResponseTokenDTO GenerateAuthenticationToken()
        {
            try
            {
                RequestTokenDTO credentials = new()
                {
                    Usuario = DecodeUtilities.GetDecodeText(_configuration.GetValue<string>($"{TecnoFactorSettings}:User")),
                    Contrasena = DecodeUtilities.GetDecodeText(_configuration.GetValue<string>($"{TecnoFactorSettings}:Password"))
                };

                string urlAuthorization = _configuration.GetValue<string>(UrlTecnofactorSetting) + "/auth/login";
                var clientAuth = new RestClient(urlAuthorization);
                var requestAuth = new RestRequest(new Uri(urlAuthorization), Method.Post);

                requestAuth.AddHeader(Constant.ContentTypeHeader, Constant.ApplicationJsonHeader);
                requestAuth.AddJsonBody(credentials);

                RestResponse response = clientAuth.Execute(requestAuth);
                _log.Information("Response GenerateToken: " + response.Content);
                return JsonSerializer.Deserialize<ResponseTokenDTO>(response.Content);
            }
            catch (Exception e)
            {
                _log.Error($"{Constant.ErrorMessageServiceLog}::GenerateAuthenticationToken {e}");
                throw e;
            }
        }

        /// <summary>
        /// Servicio para generar la factura electrónica en TecnoFactor.
        /// </summary>
        /// <param name="document">Objeto de tipo DocumentDTO con los datos a enviar al Operador Tecnológico</param>
        /// <param name="authToken">String con el token autorizado para consumir el servicio</param>
        /// <returns>Retorna objeto ResponseDocumentDTO con el resultado de la transacción</returns>
        public ResponseDocumentDTO SendDocumentToTecnofactor(RequestDocumentDTO requestDocument, string authToken)
        {
            try
            {
                requestDocument.EmisorId = DecodeUtilities.GetDecodeText(_configuration.GetValue<string>($"{TecnoFactorSettings}:EmisorId"));

                _log.Information("RequestDocument JSON " + JsonSerializer.Serialize(requestDocument));

                // HttpClient POST
                string urlSendDocument = _configuration.GetValue<string>(UrlTecnofactorSetting) + "/documento/registrardocumento";
                var client = new RestClient(urlSendDocument);
                var request = new RestRequest(new Uri(urlSendDocument), Method.Post);

                request.AddHeader(Constant.ContentTypeHeader, Constant.ApplicationJsonHeader);
                request.AddParameter("Authorization", authToken, ParameterType.HttpHeader);
                request.AddJsonBody(requestDocument);

                RestResponse response = client.Execute(request);
                _log.Information("Response SendDocument: " + response.Content);
                return JsonSerializer.Deserialize<ResponseDocumentDTO>(response.Content);
            }
            catch (Exception e)
            {
                _log.Error($"{Constant.ErrorMessageServiceLog}::SendDocumentToTecnofactor {e}");
                throw e;
            }
        }

        /// <summary>
        /// Servicio para consultar el estado del documento en TecnoFactor.
        /// </summary>
        /// <param name="requestConsultarDocumento">Objeto de tipo RequestConsultarDocumentoDTO con los datos a consultar</param>
        /// <param name="authToken">String con el token autorizado para consumir el servicio</param>
        /// <returns>Retorna objeto ResponseConsultarDocumentoDTO con los datos de respuesta del servicio</returns>
        public ResponseConsultarDocumentoDTO ConsultDocumentTecnofactor(RequestConsultarDocumentoDTO requestConsultarDocumento, string authToken)
        {
            try
            {
                // HttpClient GET
                requestConsultarDocumento.EmisorId = DecodeUtilities.GetDecodeText(_configuration.GetValue<string>($"{TecnoFactorSettings}:EmisorId"));
                string urlConsultDocument = _configuration.GetValue<string>(UrlTecnofactorSetting) + "/documento/consultarestado";
                var client = new RestClient(urlConsultDocument);
                var request = new RestRequest(new Uri(urlConsultDocument), Method.Get);

                request.AddHeader(Constant.ContentTypeHeader, Constant.ApplicationJsonHeader);
                request.AddParameter("Authorization", authToken, ParameterType.HttpHeader);
                request.AddJsonBody(requestConsultarDocumento);

                RestResponse response = client.Execute(request);
                _log.Information("Response ConsultDocument: " + response.Content);
                return JsonSerializer.Deserialize<ResponseConsultarDocumentoDTO>(response.Content);
            }
            catch (Exception e)
            {
                _log.Error($"{Constant.ErrorMessageServiceLog}::ConsultDocumentTecnofactor {e}");
                throw e;
            }
        }
    }
}

