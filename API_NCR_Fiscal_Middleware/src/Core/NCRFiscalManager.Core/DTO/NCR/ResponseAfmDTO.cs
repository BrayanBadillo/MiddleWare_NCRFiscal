using System.Net;
using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.NCR
{    public class ResponseDTO
    {
        [JsonPropertyName("StatusCode")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonPropertyName("CUFE")]
        public string Cufe { get; set; }
        [JsonPropertyName("AuthorizedDate")]
        public string AuhorizedDate { get; set; }
        [JsonPropertyName("Message")]
        public string Message { get; set; }
    }
}