using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NCRFiscalManager.Core.Entities
{
    public class PaymentMethods : BaseEntity
    {

        [Required]
        public long EmitterInVoiceId { get; set; }
        [ForeignKey("EmitterInVoiceId")]
        public EmitterInVoice EmitterInVoice { get; set; }

        [Required]
        [MaxLength(100)]
        public string PaymentType { get; set; }

        [Required]
        public int UserNumber { get; set; }

    }
}
