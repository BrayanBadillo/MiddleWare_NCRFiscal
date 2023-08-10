using AutoMapper;
using NCRFiscalManager.Core.DTO.Entities;
using NCRFiscalManager.Core.Entities;

namespace NCRFiscalManager.Core.Mappers.Entities;

public class BlackListItemsMapperProfile : Profile
{
	public BlackListItemsMapperProfile()
	{
		CreateMap<BlackListItemsDTO, BlackListItems>()
			.ReverseMap();
		CreateMap<UpsertBlackListItemDTO, BlackListItems>()
			.ReverseMap();
	}
}
