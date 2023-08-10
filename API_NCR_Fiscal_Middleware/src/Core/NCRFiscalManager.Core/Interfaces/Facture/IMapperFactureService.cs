using NCRFiscalManager.Core.DTO.Facture.Request;
using NCRFiscalManager.Core.DTO.NCR;

namespace NCRFiscalManager.Core.Interfaces.Facture;

public interface IMapperFactureService
{
    public Factura GetFactureClientDTO(DocumentDTO documentDto);
    public NotaCredito GetNotaCredito(DocumentDTO documentDto);
}