using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.TecnoFactor
{
    public class ResponseDocumentDTO
    {
        [JsonPropertyName("prefijo")]
        public string Prefijo { get; set; }
        [JsonPropertyName("numero")]
        public string Numero { get; set; }
        [JsonPropertyName("estado")]
        public EstadoDTO Estado { get; set; }
        
        [JsonPropertyName("cufe")]
        public string Cufe { get; set; }

        [JsonPropertyName("detalle")]
        public DateTime Detalle { get; set; }
    }
}