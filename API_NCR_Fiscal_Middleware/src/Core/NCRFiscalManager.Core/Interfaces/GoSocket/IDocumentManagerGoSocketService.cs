using NCRFiscalManager.Core.DTO.NCR;

namespace NCRFiscalManager.Core.Interfaces.GoSocket
{
    public interface IDocumentManagerGoSocketService
    {
        public ResponseDTO ProcessDocumentGoSocket(DocumentDTO document);
    }
}