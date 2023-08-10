using NCRFiscalManager.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NCRFiscalManager.Core.DTO.Entities;

public class PointOfSaleDTO
{
    public long Id { get; set; }
    public long EmitterInVoiceId { get; set; }
    public EmitterInVoice EmitterInVoice { get; set; }
    public List<InvoiceTransaction> InvoiceTransactions { get; set; }
    public string Name { get; set; }
    public string StoreKey { get; set; }
    public string Address { get; set; }
    public string InitInvoiceNumber { get; set; }
    public string FinalInvoiceNumber { get; set; }
    public bool IsProduction { get; set; }
    public string DateOfResolution { get; set; }
    public string Plazo { get; set; }
    public string? LlaveTecnica { get; set; }
    public string ResolutionSerial { get; set; }
}
