using Microsoft.EntityFrameworkCore;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces.Repositories;
using NCRFiscalManager.Infraestructure.Repositories.Context;

namespace NCRFiscalManager.Infraestructure.Repositories;

public class BlackListItemsRepository : RepositoryAsync<BlackListItems>, IBlackListItemsRepository
{
    private readonly NCRFiscalContext _ncrFiscalContext;

    public BlackListItemsRepository(NCRFiscalContext ncrFiscalContext) : base(ncrFiscalContext)
    {
        _ncrFiscalContext = ncrFiscalContext;
    }

    public IEnumerable<BlackListItems> GetAllBlackListedItemsByEmitterInvoiceId(long emitterInvoiceId)
    {
        var blackListedItemsList = _ncrFiscalContext.BlackListedItems
            .Where(x => x.EmitterInVoiceId == emitterInvoiceId)
            .ToList();

        return blackListedItemsList;
    }
}
