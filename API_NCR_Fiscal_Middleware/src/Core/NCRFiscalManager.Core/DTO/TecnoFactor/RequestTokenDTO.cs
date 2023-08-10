using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.TecnoFactor
{
    public class RequestTokenDTO
    {
        [JsonPropertyName("usuario")]
        public string Usuario { get; set; }

        [JsonPropertyName("contrasena")]
        public string Contrasena { get; set; }
    }
}