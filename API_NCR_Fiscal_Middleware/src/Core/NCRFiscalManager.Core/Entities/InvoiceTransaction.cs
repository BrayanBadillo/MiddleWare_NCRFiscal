using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NCRFiscalManager.Core.Entities
{
    public class InvoiceTransaction : BaseEntity
    {
        [Required]
        public long PointOnSaleId { get; set; }
        [ForeignKey("PointOnSaleId")]
        public PointOnSale PointOnSale { get; set; }

        [Required]
        [MaxLength(100)]
        public string CUFE { get; set; }

        [Required]
        [MaxLength(100)]
        public string FiscalCorrelative { get; set; }

        [Required]
        public string Response { get; set; }

        [Required]
        public string Request { get; set; }
        
        public int TipoDocumentoFiscal { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
