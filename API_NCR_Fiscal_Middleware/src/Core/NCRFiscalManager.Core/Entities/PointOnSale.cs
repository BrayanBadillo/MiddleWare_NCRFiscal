using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NCRFiscalManager.Core.Entities
{
    public class PointOnSale : BaseEntity
    {
        [Required]
        public long EmitterInVoiceId { get; set; }
        
        [ForeignKey("EmitterInVoiceId")]
        public  EmitterInVoice EmitterInVoice { get; set; }

        public List<InvoiceTransaction> InvoiceTransactions { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string StoreKey { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string InitInvoiceNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string FinalInvoiceNumber { get; set; }

        [Required]
        public bool IsProduction { get; set; }

        [Required]
        [MaxLength(50)]
        public string DateOfResolution { get; set; }

        [Required]
        [MaxLength(50)]
        public string Plazo { get; set; }

        // Llave técnica para generar Factura electrónica
        [MaxLength(100)]
        public string? LlaveTecnica { get; set; }
        
        [MaxLength(50)]
        public string ResolutionSerial { get; set; }
    }
}

