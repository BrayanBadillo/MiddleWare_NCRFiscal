using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.TecnoFactor
{    public class FacturasReferenciaDTO
    {
        [Required]
        [JsonPropertyName("prefijo")]
        public string Prefijo { get; set; }

        [Required]
        [JsonPropertyName("numero")]
        public string Numero { get; set; }

        [JsonPropertyName("conceptoNotaDebito")]
        public string ConceptoNotaDebito { get; set; }

        [JsonPropertyName("conceptoNotaCredito")]
        public string ConceptoNotaCredito { get; set; }
    }
}