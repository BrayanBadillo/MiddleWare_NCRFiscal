using NCRFiscalManager.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NCRFiscalManager.Core.DTO.Entities;

public class BlackListItemsDTO
{
    public long Id { get; set; }
    public long EmitterInVoiceId { get; set; }
    public string AlohaItemCode { get; set; }
    public string AlohaItemName { get; set; }
}
