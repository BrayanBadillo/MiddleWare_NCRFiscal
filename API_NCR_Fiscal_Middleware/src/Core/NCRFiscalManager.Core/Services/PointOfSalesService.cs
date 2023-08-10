using AutoMapper;
using NCRFiscalManager.Core.DTO.Entities;
using NCRFiscalManager.Core.Interfaces;
using NCRFiscalManager.Core.Interfaces.Repositories;

namespace NCRFiscalManager.Core.Services;

public class PointOfSalesService : IPointOfSalesService
{
    private readonly IPointOnSaleRepository _pointOnSaleRepository;
    private readonly IMapper _mapper;

    public PointOfSalesService(IPointOnSaleRepository pointOnSaleRepository, IMapper mapper)
    {
        _pointOnSaleRepository = pointOnSaleRepository ?? throw new ArgumentNullException(nameof(pointOnSaleRepository));
        _mapper = mapper;
    }

    public PointOfSaleDTO GetPointOfSaleByResolutionSerial(string storeKey, string resolutionSerial)
    {
        var pointOfSale = _pointOnSaleRepository.GetStoreByKey(storeKey, resolutionSerial);

        return _mapper.Map<PointOfSaleDTO>(pointOfSale);


    }
}
