
using System.ComponentModel.DataAnnotations;

namespace NCRFiscalManager.Core.DTO.NCR
{
    public class DocumentDTO
    {
        [Required(ErrorMessage ="El campo emisorId es obligatorio")]
        public string EmisorId { get; set; }
        public string EmisorAlohaId { get; set; }
        public string EmisorIdExterno { get; set; }
        public string TerminalAlohaId { get; set; }
        public string TipoDocumento { get; set; }
        public string CheckNumber { get; set; }
        public string CorrelativoFiscal { get; set; }
        public string NumeroSerie { get; set; }
        public string NumeroResolucion { get; set; }
        public string CorrelativoFiscalconFormato { get; set; }
        public string Moneda { get; set; }
        public string FechaEmision { get; set; }
        public string CodigoDane { get; set; }
        public float TotalDescuento { get; set; }
        public float TotalIVA { get; set; }
        public float MontoGravado { get; set; }
        public float MontoExento { get; set; }
        public float MontoExonerado { get; set; }
        public float OtrosRecargos { get; set; }
        public float TotalVenta { get; set; }
        [Required(ErrorMessage = "El campo del cliente es obligatorio")]
        public CustomerDTO Cliente { get; set; }
        [Required(ErrorMessage ="Se debe reportar por lo menos un artículo")]
        public List<ItemDTO> Articulos { get; set; }
        public DocumentoReferenciaDTO DocumentoReferencia { get; set; }
        [Required(ErrorMessage ="El campo de pago es obligatorio")]
        public List<PaymentDTO> Pagos { get; set; }
        public float RecargoModoPedido { get; set; }
        public float RecargoPorServicio { get; set; }
        public List<DescuentosDTO> Descuentos { get; set; }
    }

}
