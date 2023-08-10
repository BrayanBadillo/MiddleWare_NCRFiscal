using NCRFiscalManager.Core.DTO.GoSocket;
using NCRFiscalManager.Core.Entities;

namespace NCRFiscalManager.Core.Interfaces.GoSocket
{
    public interface ISendDocumentGoSocketService
    {
        public ResponseDocumentGoSocketDTO SendDocumentToGoSocket(DTE requestDocumentGoSocket, TechOperatorEmitterInVoice techOperatorData);
    }
}
