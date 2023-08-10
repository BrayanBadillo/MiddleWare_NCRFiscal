using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.Facture.Response;

public class ResponseDocumentFactureDTO
{
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }
    
    [JsonPropertyName("isDuplicateDocumentRequest")]
    public bool IsDuplicateDocumentRequest { get; set; }
    
    [JsonPropertyName("requestId")]
    public string RequestId { get; set; }
    
    public string UUID { get; set; }
    
    [JsonPropertyName("DocumentNumber")]
    public string DocumentNumber { get; set; }
    
    public string LDF { get; set; }
    public string URL { get; set; }

    [JsonPropertyName("validatingErrorMessages")]
    public string ValidatingErrorMessages { get; set; }
}