using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.TecnoFactor
{
    public class RequestConsultarDocumentoDTO
    {
        [JsonPropertyName("emisorId")]
        public string EmisorId { get; set; }

        [JsonPropertyName("prefijo")]
        public string Prefijo { get; set; }

        [JsonPropertyName("numDoc")]
        public string NumeroDocumento { get; set; }

        [JsonPropertyName("tipoDoc")]
        public string TipoDocumento { get; set; }
    }
}