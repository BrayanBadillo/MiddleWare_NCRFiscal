using AutoMapper;
using NCRFiscalManager.Core.DTO.Entities;
using NCRFiscalManager.Core.Entities;

namespace NCRFiscalManager.Core.Mappers.Entities;

public class PointOfSalesMapperProfile :Profile
{
    public PointOfSalesMapperProfile()
    {
        CreateMap<PointOfSaleDTO, PointOnSale>()
            .ReverseMap();
    }
}
