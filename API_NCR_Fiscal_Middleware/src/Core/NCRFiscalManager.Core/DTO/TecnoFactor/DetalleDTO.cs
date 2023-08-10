using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.TecnoFactor
{    public class DetalleDTO
    {
        [Required]
        [JsonPropertyName("codigoArticulo")]
        public string CodigoArticulo { get; set; }

        [Required]
        [JsonPropertyName("descripcionArticulo")]
        public string DescripcionArticulo { get; set; }

        [JsonPropertyName("unidadMedida")]
        public string UnidadMedida { get; set; }

        [JsonPropertyName("estandarProducto")]
        public string EstandarProducto { get; set; }

        [JsonPropertyName("nombreMarca")]
        public string NombreMarca { get; set; }

        [JsonPropertyName("nombreModelo")]
        public string NombreModelo { get; set; }

        [Required]
        [JsonPropertyName("cantidad")]
        public int Cantidad { get; set; }

        [Required]
        [JsonPropertyName("precioUnitario")]
        public float PrecioUnitario { get; set; }

        [JsonPropertyName("cargosDescuentos")]
        public List<CargosDescuentosDTO> CargosDescuentos { get; set; }

        [JsonPropertyName("impuestosRetenciones")]
        public List<ImpuestosRetencionesDTO> ImpuestosRetenciones { get; set; }

        [JsonPropertyName("obsequio")]
        public ObsequioDTO Obsequio { get; set; }
    }
}