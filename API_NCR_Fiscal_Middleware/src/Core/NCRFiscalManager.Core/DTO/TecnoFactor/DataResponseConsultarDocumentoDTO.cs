using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.TecnoFactor
{    public class DataResponseConsultarDocumentoDTO
    {
        [JsonPropertyName("prefijo")]
        public string Prefijo { get; set; }

        [JsonPropertyName("numDoc")]
        public string NumeroDocumento { get; set; }

        [JsonPropertyName("tipoDoc")]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("estado")]
        public bool Estado { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }
    }
}