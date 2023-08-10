using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.TecnoFactor
{    public class ObsequioDTO
    {
        [Required]
        [JsonPropertyName("precioReferencia")]
        public float PrecioReferencia { get; set; }

        [Required]
        [JsonPropertyName("tipoPrecioReferencia")]
        public string TipoPrecioReferencia { get; set; }
    }
}