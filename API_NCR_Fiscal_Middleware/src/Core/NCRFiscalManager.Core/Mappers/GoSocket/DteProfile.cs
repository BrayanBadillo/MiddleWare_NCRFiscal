using AutoMapper;
using NCRFiscalManager.Core.Constants;
using NCRFiscalManager.Core.DTO.GoSocket;
using NCRFiscalManager.Core.DTO.NCR;

namespace NCRFiscalManager.Core.Mappers.GoSocket;

public class DteProfile : Profile
{
    
    public DteProfile()
    {
        try
        {
            CreateMap<DocumentDTO, IdDocDTO>()
                // Se envía 10 por defecto de acuerdo a catálogo para factura electrónica
                .ForMember(dest => dest.TipoServicio, opt => opt.MapFrom(src => "10"))
                // Se valida el tipo documento a generar Factura Electrónica o Nota Crédito
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => "01"))
                .ForMember(dest => dest.Serie, opt => opt.MapFrom(src => src.NumeroSerie))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => long.Parse(src.CorrelativoFiscal)))
                .ForMember(dest => dest.NumeroInterno, opt => opt.MapFrom(src => src.CheckNumber))
                .ForMember(dest => dest.FechaEmis, opt => opt.MapFrom(src => src.FechaEmision))
                .ForMember(dest => dest.IDPago, opt => opt.MapFrom(src => src.Pagos[0].IdentificadorPago))
                .ForMember(dest => dest.TipoNegociacion, opt => opt.MapFrom(src => "1"));

            CreateMap<DocumentDTO, EmisorDTO>()
                .ForMember(dest => dest.TipoContribuyente,
                    opt => opt.MapFrom(src => Constant.PersonaJuridica))
                .ForMember(dest => dest.RegimenContable,
                    opt => opt.MapFrom(src => Constant.RegimenJuridico))
                .ForMember(dest => dest.IDEmisor, opt => opt.MapFrom(src => GetEmisorId(src.EmisorId)))
                .ForMember(dest => dest.NombreEmisor,
                    opt => opt.MapFrom(src => new NombreEmisorDTO() { PrimerNombre = "Papa Johns" }))
                .ForMember(dest => dest.CodigoEmisor,
                    opt => opt.MapFrom(src => GetCodigoEmisor()))
                .ForMember(dest => dest.DomFiscal, opt => opt.MapFrom(src => GetDomicilioFiscal(src.CodigoDane, null)))
                .ForMember(dest => dest.LugarExped, opt => opt.MapFrom(src => GetDomicilioFiscal(src.CodigoDane, null)));

            CreateMap<DocumentDTO, ReceptorDTO>()
                .ForMember(dest => dest.TipoContribuyenteR, opt => opt.MapFrom(src => src.Cliente.TipoIdentificacion == "NATURAL" ? "2" : "1"))
                .ForMember(dest => dest.RegimenContableR, opt => opt.MapFrom(src => src.Cliente.Regimen == "NO_RESPONSABLE_IVA" ? "49" : "48"))
                .ForMember(dest => dest.DocRecep,
                    opt => opt.MapFrom(src => new DocRecepDTO() { TipoDocRecep =  Constant.GetKindDocument()[src.Cliente.TipoPersona] , NroDocRecep = src.Cliente.ClienteId }))
                .ForMember(dest => dest.NmbRecep,
                    opt => opt.MapFrom(src =>  src.Cliente.Nombre))
                .ForMember(dest => dest.NombreRecep, opt => opt.MapFrom(src => new NombreEmisorDTO(){PrimerNombre = src.Cliente.Nombre}))
                .ForMember(dest => dest.CodigoReceptor,
                    opt => opt.MapFrom(src => GetCodigoReceptor(src.Cliente.ResponsabilidadFiscal)))
                .ForMember(dest => dest.DomFiscalRcp, opt => opt.MapFrom(src => GetDomicilioFiscal(src.CodigoDane, src.Cliente.Direccion)))
                .ForMember(dest => dest.LugarRecep, opt => opt.MapFrom(src => GetDomicilioFiscal(src.CodigoDane, src.Cliente.Direccion)))
                .ForMember(dest => dest.ContactoReceptor, opt => opt.MapFrom(src => GetContactoReceptor(src.Cliente)));

        }
        catch (Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// Método para obtener el número de identificación del emisor sin número de verificación.
    /// </summary>
    /// <param name="strEmisorId">Número de identificación del emisor completo</param>
    /// <returns>Retorna string obtenido</returns>
    private string GetEmisorId(string strEmisorId)
    {
        string strEmisor = "";
        if (strEmisorId != null)
        {
            strEmisor = strEmisorId.Split("-")[0];
        }

        return strEmisor;
    }

    /// <summary>
    /// Método para organizar los datos de la responsabilidad fiscal del emisor.
    /// </summary>
    /// <returns>Objeto CodigoEmisorDTO con los datos organizados del emisor</returns>
    private CodigoEmisorDTO GetCodigoEmisor()
    {
        CodigoEmisorDTO codigoEmisorDto = new CodigoEmisorDTO()
        {
            CdgIntEmisor = "O-13", TpoCdgIntEmisor = "TpoObl"
        };
        
        return codigoEmisorDto;
    }

    /// <summary>
    /// Método para organizar la responsabilidad fiscal del receptor
    /// </summary>
    /// <param name="strResponsabilidadFiscal">Código de responsabilidad fiscal del receptor</param>
    /// <returns>Retorna objeto CodigoReceptorDTO con los datos organizados</returns>
    private CodigoReceptorDTO GetCodigoReceptor(string strResponsabilidadFiscal)
    {
        CodigoReceptorDTO codigoReceptorDto = new CodigoReceptorDTO()
        {
            CdgIntRecep = strResponsabilidadFiscal.Replace("0", "O"),
            TpoCdgIntRecep = "TpoObl"
        };
        
        return codigoReceptorDto;
    }

    /// <summary>
    /// Método para obtener los datos de localización.
    /// </summary>
    /// <param name="codigoDane">Código DANE de la ciudad, municipio</param>
    /// <param name="direccion">Dirección</param>
    /// <returns>Retorna objeto PlaceDTO con los datos organizados</returns>
    private PlaceDTO GetDomicilioFiscal(string codigoDane, string direccion)
    {
        return new PlaceDTO()
        {
            Calle = direccion,
            Departamento = codigoDane.Substring(0,2),
            Ciudad = codigoDane,
            Pais = "CO",
            //CodigoPostal = "110111"
        };
    }

    /// <summary>
    /// Método para obtener los datos de contacto del cliente.
    /// </summary>
    /// <param name="customerDto">Objeto CustomerDTO con los datos del cliente</param>
    /// <returns>Retorna objeto ContactoEmisorDTO con los datos organizados</returns>
    private ContactoEmisorDTO GetContactoReceptor(CustomerDTO customerDto)
    {
        return new ContactoEmisorDTO()
        {
            Tipo = "1",
            Nombre = customerDto.Nombre,
            Descripcion = "Cliente",
            eMail = customerDto.Emails[0],
            Telefono = customerDto.Telefono
        };
    }
}