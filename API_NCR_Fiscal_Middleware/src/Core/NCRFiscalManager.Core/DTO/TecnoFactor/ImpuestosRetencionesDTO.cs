using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.TecnoFactor
{    public class ImpuestosRetencionesDTO
    {
        [Required]
        [JsonPropertyName("tributo")]
        public string Tributo { get; set; }

        [JsonPropertyName("valorImpuestoRetencion")]
        public Nullable<float> ValorImpuestoRetencion { get; set; }

        [JsonPropertyName("porcentaje")]
        public float Porcentaje { get; set; }
    }
}