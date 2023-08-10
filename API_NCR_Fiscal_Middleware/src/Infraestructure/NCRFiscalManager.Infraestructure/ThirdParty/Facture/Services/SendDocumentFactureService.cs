using Serilog;
using RestSharp;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using NCRFiscalManager.Core.Constants;
using NCRFiscalManager.Core.DTO.Facture.Request;
using NCRFiscalManager.Core.DTO.Facture.Response;
using NCRFiscalManager.Core.Interfaces.Facture;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces.Repositories;
using NCRFiscalManager.Core.Utilities;

namespace NCRFiscalManager.Infraestructure.ThirdParty.Facture.Services;

public class SendDocumentFactureService : ISendDocumentFactureService
{
    private readonly ILogger _log;
    
    // Campos para accessToken
    private static Dictionary<string, TokenDTO> _accessToken = new Dictionary<string, TokenDTO>();
    
    private readonly ITechOperatorEmitterInVoiceRepository _techOperatorEmitterInVoiceRepository;
    private TechOperatorEmitterInVoice _techOperatorEmitterInVoice;

    public SendDocumentFactureService(ILogger log, ITechOperatorEmitterInVoiceRepository techOperatorEmitterInVoiceRepository)
    {
        _log = log;
        _techOperatorEmitterInVoiceRepository = techOperatorEmitterInVoiceRepository ?? throw new ArgumentNullException(nameof(techOperatorEmitterInVoiceRepository));
    }

    /// <summary>
    /// Método para enviar la factura al operador tecnológico Facture.
    /// </summary>
    /// <param name="factureInvoice">Objeto Factura con los datos consolidados a reportar</param>
    /// <returns>Retorna objeto ResponseDocumentFactureDTO con el resultado de la transacción</returns>
    /// <exception cref="Exception"></exception>
    public ResponseDocumentFactureDTO SendFactureInvoice(Factura factureInvoice, string technicalKey)
    {
        try
        {
            var requestId = Guid.NewGuid();
            string accessToken = GetAccesToken($"{factureInvoice.Emisor.NumeroIdentificacion}-{factureInvoice.Emisor.DV}");
            ConnectionDataFactureDTO connectionDataDTO = JsonSerializer.Deserialize<ConnectionDataFactureDTO>(_techOperatorEmitterInVoice.ConnectionData);
            connectionDataDTO.XKeyControl = technicalKey;
            string serviceUrl = $"{_techOperatorEmitterInVoice.Url}/Issue/XML3";
            var client = new RestClient(serviceUrl);
            var request = new RestRequest(serviceUrl, Method.Post)
                .AddHeader("Content-Type", "application/xml")
                .AddHeader("Accept", "application/xml")
 
                .AddHeader("X-Who", $"{connectionDataDTO.Xwho}")
                .AddHeader("Authorization", $"Bearer {accessToken}")
                .AddHeader("REQUEST-ID", requestId)
                .AddHeader("X-REF-DOCUMENTTYPE", $"{connectionDataDTO.DocumentTypeInvoice}")
                .AddHeader("X-KEYCONTROL", $"{connectionDataDTO.XKeyControl}")
                .AddHeader("X-ASYNC", false)
                .AddXmlBody(factureInvoice);

            _log.Information($"Request SendFactureInvoice: [{XmlSerializerUtility.GetXml(factureInvoice)}]");
            RestResponse response = client.Execute(request);
            _log.Information($"Response SendFactureInvoice: [{response.Content}]");

            return JsonSerializer.Deserialize<ResponseDocumentFactureDTO>(response.Content);

        }
        catch (Exception e)
        {
            _log.Error($"{Constant.ErrorMessageServiceLog}::Send Factura to Facture {e}");
            //_log.Error($"Xml: {GetXml()}")
            throw e;
        }
    }
    
    /// <summary>
    /// Método para enviar una nota crédito al operador tecnológico Facture.
    /// </summary>
    /// <param name="creditNote">Objeto NotaCredito con los datos obtenidos a reportar</param>
    /// <returns>Retorna objeto ResponseDocumentFactureDTO con el resultado de la transacción</returns>
    /// <exception cref="Exception"></exception>
    public ResponseDocumentFactureDTO SendFactureCreditNote(NotaCredito creditNote, string technicalKey)
    {
        try
        {

            var requestId = Guid.NewGuid();
            string accessToken = GetAccesToken($"{creditNote.Emisor.NumeroIdentificacion}-{creditNote.Emisor.DV}");
            ConnectionDataFactureDTO connectionDataDTO = JsonSerializer.Deserialize<ConnectionDataFactureDTO>(_techOperatorEmitterInVoice.ConnectionData);
            connectionDataDTO.XKeyControl = technicalKey;
            string serviceUrl = $"{_techOperatorEmitterInVoice.Url}/Issue/XML3";
            var client = new RestClient(serviceUrl);
            var request = new RestRequest(serviceUrl, Method.Post)
                .AddHeader("Content-Type", "application/xml")
                .AddHeader("Accept", "application/xml")
                .AddHeader("X-Who", $"{connectionDataDTO.Xwho}")
                .AddHeader("Authorization", $"Bearer {accessToken}")
                .AddHeader("REQUEST-ID", requestId)
                .AddHeader("X-REF-DOCUMENTTYPE",$"{connectionDataDTO.DocumentTypeCreditNote}")
                .AddHeader("X-KEYCONTROL", $"{connectionDataDTO.XKeyControl}")
                .AddHeader("X-ASYNC", false)
                .AddXmlBody(creditNote);

            _log.Information($"Request SendCreditNote: [{XmlSerializerUtility.GetXml(creditNote)}]");
            RestResponse response = client.Execute(request);
            _log.Information($"Response SendCreditNote: [{response.Content}]");

            return JsonSerializer.Deserialize<ResponseDocumentFactureDTO>(response.Content);

        }
        catch (Exception e)
        {
            _log.Error($"{Constant.ErrorMessageServiceLog}::Send NotaCredito to Facture {e}");
            throw e;
        }
    }

    /// <summary>
    /// Método para obtener el token del operador tecnológico Facture.
    /// </summary>
    /// <returns>Retorna token generado.</returns>
    /// <exception cref="Exception"></exception>
    private string GetAccesToken(string numberIdentification)
    {
        try
        {
            _techOperatorEmitterInVoice = _techOperatorEmitterInVoiceRepository.GetByEmitterInvoiceNit(numberIdentification);
            ConnectionDataFactureDTO connectionDataDTO = JsonSerializer.Deserialize<ConnectionDataFactureDTO>(_techOperatorEmitterInVoice.ConnectionData);

            if (ValidateToken(_techOperatorEmitterInVoice.User))
            {
                RequestLoginDTO requestLoginDto = new()
                {
                    User = _techOperatorEmitterInVoice.User,
                    Password = _techOperatorEmitterInVoice.Password,
                    Tenant = connectionDataDTO.Tenant
                };

                string urlAuthorization = _techOperatorEmitterInVoice.Url + "/Auth/Login";
                var clientAuth = new RestClient(urlAuthorization);
                var requestAuth = new RestRequest(new Uri(urlAuthorization), Method.Post);

                requestAuth.AddHeader(Constant.ContentTypeHeader, Constant.ApplicationJsonHeader);
                requestAuth.AddHeader("X-Who", $"{connectionDataDTO.Xwho}");
                requestAuth.AddJsonBody(requestLoginDto);
                _log.Information("Request GenerateToken: " + JsonSerializer.Serialize(requestLoginDto));

                RestResponse response = clientAuth.Execute(requestAuth);
                _log.Information("Response GenerateToken: " + response.Content);
                ResponseLoginDTO responseLoginDto = JsonSerializer.Deserialize<ResponseLoginDTO>(response.Content);
                if (responseLoginDto.AccesToken != null)
                {
                    _accessToken[_techOperatorEmitterInVoice.User] = new TokenDTO() { Token = responseLoginDto.AccesToken, TokenDate = DateTime.Now };
                }
            }

            return _accessToken[_techOperatorEmitterInVoice.User].Token;
        }
        catch (Exception e)
        {
            _log.Error($"{Constant.ErrorMessageServiceLog}::GenerateAuthenticationToken Facture {e}");
            throw e;
        }
    }

    /// <summary>
    /// Método para validar si es necesario solicitar un token.
    /// </summary>
    /// <param name="user">Usuario a solicitar token</param>
    /// <returns>Retorna bool con el resultado de la validación. True se solicita token, False en caso contrario</returns>
    private bool ValidateToken(string user)
    {
        if (!_accessToken.ContainsKey(user)) return true;
        // Se suman 90 días ya que el tiempo de vigencia del token es de 3 meses.
        DateTime dateToken = _accessToken[user].TokenDate.AddDays(90);
        if (dateToken.Ticks < DateTime.Now.Ticks)
        {
            return true;
        }

        return false;
    }
}