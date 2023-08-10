using NCRFiscalManager.Core.Entities;

namespace NCRFiscalManager.Core.Interfaces.Repositories
{
    public interface IPointOnSaleRepository : IRepositoryAsync<PointOnSale>
    {
        public PointOnSale GetStoreByKey(string storeKey, string ResolutionSerial);
    }
}
