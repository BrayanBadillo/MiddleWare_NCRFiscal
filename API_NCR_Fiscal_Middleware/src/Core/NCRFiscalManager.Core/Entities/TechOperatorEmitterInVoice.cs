using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NCRFiscalManager.Core.Entities
{
    public class TechOperatorEmitterInVoice : BaseEntity
    {
        [Required]
        public long EmitterInVoiceId { get; set; }
        [ForeignKey("EmitterInVoiceId")]
        public EmitterInVoice EmitterInVoice { get; set; }

        [Required]
        public long TechOperatorId { get; set; }
        [ForeignKey("TechOperatorId")]
        public TechOperator TechOperator  { get; set; }

        [Required]
        public string ConnectionData { get; set; }

        [Required]
        [MaxLength(100)]
        public string User { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        [MaxLength(200)]
        public string Url { get; set; }

    }
}
