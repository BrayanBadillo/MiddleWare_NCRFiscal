using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.TecnoFactor
{
    public class ResponseConsultarDocumentoDTO
    {
        [JsonPropertyName("data")]
        public DataResponseConsultarDocumentoDTO Data { get; set; }
    }
}