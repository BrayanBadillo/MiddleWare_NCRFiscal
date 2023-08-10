using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO
{    public class CiudadAdquirienteDTO
    {
        [Required]
        [JsonPropertyName("cdDane")]
        public string CdDane { get; set; }
    }
}