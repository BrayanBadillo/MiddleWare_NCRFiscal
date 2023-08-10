using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Response;

public class ResponseLoginDTO
{
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }
    
    [JsonPropertyName("accessToken")]
    public string AccesToken { get; set; }
    
    [JsonPropertyName("tenantId")]
    public string TenantId { get; set; }
}