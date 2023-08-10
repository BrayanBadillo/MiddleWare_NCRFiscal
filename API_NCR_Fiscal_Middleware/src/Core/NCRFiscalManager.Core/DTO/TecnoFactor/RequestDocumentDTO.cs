using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NCRFiscalManager.Core.DTO.TecnoFactor
{
    public class RequestDocumentDTO
    {
        [Required]
        [JsonPropertyName("emisorId")]
        public string EmisorId { get; set; }

        [Required]
        [JsonPropertyName("tipoDocumentoElectronico")]
        public string TipoDocumentoElectronico { get; set; }

        [Required]
        [JsonPropertyName("dsPrefijo")]
        public string DsPrefijo { get; set; }

        [JsonPropertyName("dsNumeroFactura")]
        public string DsNumeroFactura { get; set; }

        [Required]
        [JsonPropertyName("dsResolucionDian")]
        public string DsResolucionDian { get; set; }

        [JsonPropertyName("moneda")]
        public string Moneda { get; set; }

        [Required]
        [JsonPropertyName("snConsecutivoAutomatico")]
        public bool SnConsecutivoAutomatico { get; set; }

        [JsonPropertyName("fechaEmision")]
        public string FechaEmision { get; set; }

        /*[JsonPropertyName("fechaVencimiento")]
        public DateTime FechaVencimiento { get; set; }*/

        [Required]
        [JsonPropertyName("nombresAdquiriente")]
        public string NombresAdquiriente { get; set; }

        [JsonPropertyName("segundoNombre")]
        public string SegundoNombre { get; set; }

        [JsonPropertyName("primerApellido")]
        public string PrimerApellido { get; set; }

        [JsonPropertyName("segundoApellido")]
        public string SegundoApellido { get; set; }

        [Required]
        [JsonPropertyName("emailAdquiriente")]
        public string EmailAdquiriente { get; set; }

        [Required]
        [JsonPropertyName("telefonoAdquiriente")]
        public string TelefonoAdquiriente { get; set; }

        [Required]
        [JsonPropertyName("direccionAdquiriente")]
        public string DireccionAdquiriente { get; set; }

        [Required]
        [JsonPropertyName("identificacionAdquiriente")]
        public string IdentificacionAdquiriente { get; set; }

        [Required]
        [JsonPropertyName("tipoIdentificacionAdquiriente")]
        public string TipoIdentificacionAdquiriente { get; set; }

        [Required]
        [JsonPropertyName("regimenAdquirente")]
        public string RegimenAdquiriente { get; set; }

        [Required]
        [JsonPropertyName("responsabilidadesFiscales")]
        public string ResponsabilidadesFiscales { get; set; }

        [Required]
        [JsonPropertyName("tipoPersonaAdquiriente")]
        public string TipoPersonaAdquiriente { get; set; }

        [JsonPropertyName("codigoPostalAdquiriente")]
        public string CodigoPostalAdquiriente { get; set; }

        [Required]
        [JsonPropertyName("adquirenteResponsable")]
        public bool AdquirienteResponsable { get; set; }

        [JsonPropertyName("totalCopago")]
        public float TotalCopago { get; set; }

        [JsonPropertyName("totalModeradora")]
        public float TotalModeradora { get; set; }

        [JsonPropertyName("dsObservaciones")]
        public string DsObservaciones { get; set; }

        [JsonPropertyName("facturasReferencia")]
        public List<FacturasReferenciaDTO> FacturasReferencia { get; set; }

        [Required]
        [JsonPropertyName("ciudadAdquiriente")]
        public CiudadAdquirienteDTO CiudadAdquiriente { get; set; }

        [Required]
        [JsonPropertyName("pago")]
        public PagoDTO Pago { get; set; }

        [JsonPropertyName("impuestosRetenciones")]
        public List<ImpuestosRetencionesDTO> ImpuestosRetenciones { get; set; }

        [Required]
        [JsonPropertyName("detalles")]
        public List<DetalleDTO> Detalles { get; set; }
    }
}