using System.ComponentModel.DataAnnotations;

namespace NCRFiscalManager.Core.Entities
{
    public class TechOperator : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
