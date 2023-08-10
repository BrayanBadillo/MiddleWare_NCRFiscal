using NCRFiscalManager.Core.DTO.Entities;

namespace NCRFiscalManager.Core.Interfaces;

public interface IPointOfSalesService
{
    PointOfSaleDTO GetPointOfSaleByResolutionSerial(string storeKey, string resolutionSerial); 
}
