using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.TecnoFactor
{    public class PagoDTO
    {
        [Required]
        [JsonPropertyName("formaPago")]
        public string FormaPago { get; set; }

        [Required]
        [JsonPropertyName("medioPago")]
        public string MedioPago { get; set; }

        [JsonPropertyName("fechaVencimientoPago")]
        public DateTime FechaVencimientoPago { get; set; }
    }
}