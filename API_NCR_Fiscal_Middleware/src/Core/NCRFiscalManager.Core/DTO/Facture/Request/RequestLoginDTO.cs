using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Request;
public class RequestLoginDTO
{
    [JsonPropertyName("u")]
    public string User { get; set; }
    
    [JsonPropertyName("p")]
    public string Password { get; set; }
    
    [JsonPropertyName("t")]
    public string Tenant { get; set; }
}

