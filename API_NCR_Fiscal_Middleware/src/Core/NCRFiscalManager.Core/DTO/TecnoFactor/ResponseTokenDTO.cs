using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.TecnoFactor
{
    public class ResponseTokenDTO
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("estado")]
        public EstadoDTO Estado { get; set; }
    }
}