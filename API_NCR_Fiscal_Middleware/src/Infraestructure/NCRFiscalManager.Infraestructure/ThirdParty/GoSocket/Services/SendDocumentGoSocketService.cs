using NCRFiscalManager.Core.DTO.GoSocket;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces.GoSocket;
using NCRFiscalManager.Core.Utilities;
using RestSharp;
using RestSharp.Authenticators;
using Serilog;
using System.Text.Json;

namespace NCRFiscalManager.Infraestructure.ThirdParty.GoSocket.Services
{
    public class SendDocumentGoSocketService : ISendDocumentGoSocketService
    {
        private readonly ILogger _log;

        public SendDocumentGoSocketService(ILogger log)
        {
            _log = log;
        }

        /// <summary>
        /// Método para enviar los datos al operador tecnológico GoSocket para generar la factura electrónica.
        /// </summary>
        /// <param name="requestDocumentGoSocket">Objeto DTE con los datos organizados a enviar</param>
        /// <param name="techOperatorData">Entity TechOperatorEmitterInVoice con los datos de conexión a GoSocket</param>
        /// <returns>Retorna objeto ResponseDocumentGoSocketDTO con la respuesta de la transacción</returns>
        /// <exception cref="Exception"></exception>
        public ResponseDocumentGoSocketDTO SendDocumentToGoSocket(DTE requestDocumentGoSocket, TechOperatorEmitterInVoice techOperatorData)
        {
            try
            {
                string urlService = techOperatorData.Url;
                var client = new RestClient(urlService)
                {
                    Authenticator = new HttpBasicAuthenticator(techOperatorData.User, techOperatorData.Password)
                };
                ConnectionDataGoSocketDTO connectionData =
                    JsonSerializer.Deserialize<ConnectionDataGoSocketDTO>(techOperatorData.ConnectionData);
                                 
                string xmlFileContent = XmlSerializerUtility.GetXml(requestDocumentGoSocket);
                xmlFileContent = xmlFileContent.Replace("<campoString>", "<campoString name=\"NombreLocal\">");
                 
                var request = new RestRequest(new Uri(urlService), Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("mapping", connectionData.Mapping);
                request.AddParameter("sign", true);
                request.AddParameter("defaultCertificate", true);
                request.AddParameter("async", false);
                request.AddParameter("fileContent", xmlFileContent);

                _log.Information($"Request SendDocumentToGoSocket: {xmlFileContent}");

                var Json = JsonSerializer.Serialize(xmlFileContent);

                RestResponse response = client.Execute(request);

                string Resultado = response.Content;

                _log.Information($"Response SendDocumentToGoSocket: {response.Content}");
                return JsonSerializer.Deserialize<ResponseDocumentGoSocketDTO>(response.Content);
            }
            catch (Exception e)
            {
                _log.Error($"Error SendDocumentToGoSocket {e}");
                return new ResponseDocumentGoSocketDTO()
                {
                    Success = false,
                    Messages = new List<string>() { e.Message },
                    Description = e.Message
                };
            }
        }
    }
}
