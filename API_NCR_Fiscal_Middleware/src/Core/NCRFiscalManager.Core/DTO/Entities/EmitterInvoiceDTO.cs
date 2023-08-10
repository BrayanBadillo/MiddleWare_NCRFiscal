namespace NCRFiscalManager.Core.DTO.Entities;

public class EmitterInvoiceDTO
{
    public string IdentificationNumber { get; set; }
    public bool IgnoresAllPriceZeroItems { get; set; }

    public bool UsesBlackList { get; set; }
}
