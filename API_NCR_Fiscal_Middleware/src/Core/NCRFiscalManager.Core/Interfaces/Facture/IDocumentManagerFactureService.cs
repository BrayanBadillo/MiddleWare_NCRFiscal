using NCRFiscalManager.Core.DTO.Facture.Request;
using NCRFiscalManager.Core.DTO.NCR;

namespace NCRFiscalManager.Core.Interfaces.Facture;

public interface IDocumentManagerFactureService
{
    public ResponseDTO ProcessDocumentFacture(DocumentDTO document);

    public ResponseDTO ProcessResendDocumentFacture( Factura factura, string storeKey );
}