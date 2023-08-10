using NCRFiscalManager.Core.DTO.GoSocket;
using NCRFiscalManager.Core.DTO.NCR;

namespace NCRFiscalManager.Core.Interfaces.GoSocket;

public interface IMapperGoSocketService
{
    public DTE GetGoSocketClientDTO(DocumentDTO documentDto, string claveTcNotaCredito);
}