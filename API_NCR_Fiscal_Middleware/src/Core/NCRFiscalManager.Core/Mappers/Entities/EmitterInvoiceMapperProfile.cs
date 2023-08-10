using AutoMapper;
using NCRFiscalManager.Core.DTO.Entities;
using NCRFiscalManager.Core.Entities;

namespace NCRFiscalManager.Core.Mappers.Entities;

public class EmitterInvoiceMapperProfile : Profile
{
	public EmitterInvoiceMapperProfile()
	{
		CreateMap<EmitterInvoiceDTO, EmitterInVoice>();
		CreateMap<EmitterInVoice, EmitterInvoiceDTO>();
	}
}
