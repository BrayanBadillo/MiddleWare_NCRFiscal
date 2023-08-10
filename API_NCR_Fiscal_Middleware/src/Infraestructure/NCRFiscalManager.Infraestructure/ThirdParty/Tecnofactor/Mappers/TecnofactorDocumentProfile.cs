using AutoMapper;
using NCRFiscalManager.Core.DTO.NCR;
using NCRFiscalManager.Infraestructure.ThirdParty.Tecnofactor.Services.Models;

namespace NCRFiscalManager.Infraestructure.ThirdParty.Tecnofactor.Mappers
{
    public class TecnofactorDocumentProfile : Profile
    {
        public TecnofactorDocumentProfile()
        {
            CreateMap<DocumentDTO, Document>()
                .ForMember(dest => dest.emailAdquiriente, opt => opt.MapFrom(src => src.Cliente.Emails.FirstOrDefault()));
            CreateMap<Document, DocumentDTO>();
        }
    }
}
