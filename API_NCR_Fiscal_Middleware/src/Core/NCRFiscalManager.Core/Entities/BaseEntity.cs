using System.ComponentModel.DataAnnotations;

namespace NCRFiscalManager.Core.Entities
{
    public class BaseEntity
    {
        [Key]
        [Required]
        public long Id { get; set; }
    }
}
