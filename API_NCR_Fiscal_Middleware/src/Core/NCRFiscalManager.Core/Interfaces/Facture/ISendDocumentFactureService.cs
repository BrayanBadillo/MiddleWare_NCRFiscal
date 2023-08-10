using NCRFiscalManager.Core.DTO.Facture.Request;
using NCRFiscalManager.Core.DTO.Facture.Response;

namespace NCRFiscalManager.Core.Interfaces.Facture;

public interface ISendDocumentFactureService
{
    public ResponseDocumentFactureDTO SendFactureInvoice(Factura factureInvoice, string technicalKey);
    public ResponseDocumentFactureDTO SendFactureCreditNote(NotaCredito creditNote, string technicalKey);
}