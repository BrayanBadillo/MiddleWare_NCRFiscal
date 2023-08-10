using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.TecnoFactor
{    public class CargosDescuentosDTO
    {
        [Required]
        [JsonPropertyName("tipo")]
        public bool Tipo { get; set; }

        [JsonPropertyName("porcentaje")]
        public float Porcentaje { get; set; }
        
        [JsonPropertyName("valor")]
        public float Valor { get; set; }
    }
}