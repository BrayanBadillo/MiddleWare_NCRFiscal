using NCRFiscalManager.Core.Entities;

namespace NCRFiscalManager.Core.Interfaces.Repositories;

public interface IBlackListItemsRepository : IRepositoryAsync<BlackListItems>
{
    IEnumerable<BlackListItems> GetAllBlackListedItemsByEmitterInvoiceId(long emitterInvoiceId);
}
