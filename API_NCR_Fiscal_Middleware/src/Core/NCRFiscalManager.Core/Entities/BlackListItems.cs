using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NCRFiscalManager.Core.Entities;

public class BlackListItems : BaseEntity
{
    [Required]
    public long EmitterInVoiceId { get; set; }
    [ForeignKey("EmitterInVoiceId")]
    public EmitterInVoice EmitterInVoice { get; set; }

    [Required]
    [MaxLength(50)]
    public string AlohaItemCode { get; set; }

    [MaxLength(100)]
    public string AlohaItemName { get; set; }
}
