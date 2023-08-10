using System.ComponentModel.DataAnnotations;

namespace NCRFiscalManager.Core.Entities
{
    public class EmitterInVoice : BaseEntity
    {
        public List<PointOnSale> PointOnSale { get; set; }

        [Required]
        public byte PersonType { get; set; }

        [Required]
        [MaxLength(15)]
        public string RegimeType { get; set; }

        [Required]
        [MaxLength(50)]
        public string IdentificationType { get; set; }

        [Required]
        [MaxLength(20)]
        public string IdentificationNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string BusinessName { get; set; }

        [Required]
        [MaxLength(20)]
        public string CommercialRegistrationNum { get; set; }

        [Required]
        [MaxLength(100)]
        public string TradeName { get; set; }

        [Required]
        [MaxLength(20)]
        public string ObligationCode { get; set; }

        [Required]
        [MaxLength(20)]
        public string TributeName { get; set; }

        [Required]
        [MaxLength(20)]
        public string TributeCode { get; set; }

        [Required]
        [MaxLength(80)]
        public string Email { get; set; }

        public bool IgnoresAllPriceZeroItems { get; set; }

        public bool UsesBlackList { get; set; }
    }
}
