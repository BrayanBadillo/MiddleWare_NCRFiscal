using AutoMapper;
using NCRFiscalManager.Core.Constants;
using NCRFiscalManager.Core.DTO.Facture.Request;
using NCRFiscalManager.Core.DTO.NCR;

namespace NCRFiscalManager.Core.Mappers.Facture;

public class FactureProfile : Profile
{
    public FactureProfile()
    {
	    try
	    {
            CreateMap<DocumentDTO, CabeceraDTO>()
		    .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => $"{src.NumeroSerie}{src.CorrelativoFiscal}"))
		    .ForMember(dest => dest.FechaEmision, opt => opt.MapFrom(src => SplitDateAndTime(src.FechaEmision, 0)))
		    .ForMember(dest => dest.HoraEmision, opt => opt.MapFrom(src => SplitDateAndTime(src.FechaEmision, 1)))
		    .ForMember(dest => dest.MonedaFactura, opt => opt.MapFrom(src => Constant.MonedaCop))
		    .ForMember(dest => dest.TipoFactura, opt => opt.MapFrom(src => "01")) // Por validar.
		    .ForMember(dest => dest.FormaDePago, opt => opt.MapFrom(src => Constant.PaymentMethodFacture))
		    .ForMember(dest => dest.LineasDeFactura, opt => opt.MapFrom(src => src.Articulos.Count))
		    .ForMember(dest => dest.TipoOperacion, opt => opt.MapFrom(src => Constant.PayMethod))
		    .ForMember(dest => dest.OrdenCompra, opt => opt.MapFrom(src => src.CheckNumber));
            
            CreateMap<DocumentDTO, CabeceraCrediNoteDTO>()
	            .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => $"{src.NumeroSerie}{src.CorrelativoFiscal}"))
	            .ForMember(dest => dest.Prefijo, opt => opt.MapFrom(src => src.NumeroSerie))
	            .ForMember(dest => dest.FechaEmision, opt => opt.MapFrom(src => SplitDateAndTime(src.FechaEmision, 0)))
	            .ForMember(dest => dest.HoraEmision, opt => opt.MapFrom(src => SplitDateAndTime(src.FechaEmision, 1)))
	            .ForMember(dest => dest.MonedaNota, opt => opt.MapFrom(src => Constant.MonedaCop))
	            .ForMember(dest => dest.FormaDePago, opt => opt.MapFrom(src => Constant.PaymentMethodFacture))
	            .ForMember(dest => dest.LineasDeNota, opt => opt.MapFrom(src => src.Articulos.Count))
	            // Tipo de operación para nota crédito factura electrónica es 20
	            .ForMember(dest => dest.TipoOperacion, opt => opt.MapFrom(src => "20"))
	            .ForMember(dest => dest.OrdenCompra, opt => opt.MapFrom(src => src.CheckNumber));

            CreateMap<ItemDTO, DetalleDTO>()
	            .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => (float)src.Cantidad))
	            .ForMember(dest => dest.UnidadMedida, opt => opt.MapFrom(src => Constant.UnitOfMeasurement))
	            .ForMember(dest => dest.SubTotalLinea, opt => opt.MapFrom(src => src.MontoGravado))
	            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.DescripcionArticulo.Trim() == "" ? "Artículo" : src.DescripcionArticulo))
	            .ForMember(dest => dest.CantidadBase, opt => opt.MapFrom(src => (float)src.Cantidad))
	            .ForMember(dest => dest.UnidadCantidadBase, opt => opt.MapFrom(src => Constant.UnitOfMeasurement))
	            .ForMember(dest => dest.PrecioUnitario, opt => opt.MapFrom(src => src.PrecioUnitario))
	            .ForMember(dest => dest.ValorTotalItem, opt => opt.MapFrom(src => src.Total));
            //.ForMember(dest => dest.IdentificadorUnico, opt => opt.MapFrom(src => src.CodigoArticulo));


            CreateMap<DocumentDTO, ClienteDTO>()
                .ForMember(dest => dest.TipoPersona, opt => opt.MapFrom(src => Constant.GetKindPerson()[src.Cliente.TipoIdentificacion]))
                .ForMember(dest => dest.TipoRegimen, opt => opt.MapFrom(src => Constant.GetTaxResponsability()[src.Cliente.Regimen]))
                .ForMember(dest => dest.TipoIdentificacion, opt => opt.MapFrom(src => Constant.GetKindDocument()[src.Cliente.TipoPersona]))
                .ForMember(dest => dest.NumeroIdentificacion, opt => opt.MapFrom(src => src.Cliente.ClienteId))
                .ForMember(dest => dest.NombreComercial, opt => opt.MapFrom(src => src.Cliente.Nombre))
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.Cliente.Nombre))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => GetDireccion(src.CodigoDane, src.Cliente.Direccion)))
                .ForMember(dest => dest.ObligacionesCliente, opt => opt.MapFrom(src => GetListaObligaciones(src.Cliente.ResponsabilidadFiscal)))
                .ForMember(dest => dest.TributoCliente, opt => opt.MapFrom(src => GetTributoCliente(src.Cliente.ResponsabilidadFiscal)))
                .ForMember(dest => dest.Contacto, opt => opt.MapFrom(src => GetContacto(src.Cliente)));


            CreateMap<CustomerDTO, NotificacionDTO>()
	            .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => "Mail"))
	            .ForMember(dest => dest.Para, opt => opt.MapFrom(src => GetEmailsNotification(src.Emails)));
	    }
	    catch (Exception e)
	    {
		    Console.WriteLine(e);
		    throw;
	    }
	    
    }
    
    /// <summary>
    /// Dividir el campo de FechaEmision de DocumentDTO en fecha = 0 y hora = 1
    /// </summary>
    /// <param name="fullDate">Cadena con la fecha</param>
    /// <param name="index">indice de la cadena a retornar (fecha = 0 y hora = 1)</param>
    /// <returns>Fecha u hora dependiendo del indice que se pasa por parametro.</returns>
    private string SplitDateAndTime(string fullDate, int index)
    {	
	    return fullDate.Split(" ")[index];
    }

    /// <summary>
    /// Método para obtener la dirección del cliente
    /// </summary>
    /// <param name="strCodigoDane">Código DANE</param>
    /// <param name="strDireccionCliente">Dirección del cliente</param>
    /// <returns>Retorna objeto DireccionDTO con los datos organizados del cliente</returns>
    private DireccionDTO GetDireccion(string strCodigoDane, string strDireccionCliente)
    {
        return new DireccionDTO()
        {
            Direccion = strDireccionCliente,
            CodigoMunicipio = strCodigoDane,
            CodigoDepartamento = strCodigoDane.Substring(0,2),
            CodigoPais = "CO",
            NombrePais = "Colombia",
            IdiomaPais = "es"
        };
    }

    /// <summary>
    /// Obtener la información del cliente
    /// </summary>
    /// <param name="customer">Objeto CustomerDTO con la información del cliente</param>
    /// <returns>Objeto ContactoDTO con la información del cliente en formato de Facture</returns>
    private ContactoDTO GetContacto(CustomerDTO customer)
    {
        return new ContactoDTO()
        {
            Nombre = customer.Nombre,
            Telefono = customer.Telefono,
            Email = customer.Emails[0]
        };
    }

    /// <summary>
    /// Método para obtener la listas de obligaciones del cliente
    /// </summary>
    /// <param name="strResponsabilidadFiscal">Indica el tipo de responsabilidad fiscal del cliente</param>
    /// <returns>Retorna lista de objetos CodigoObligacionDTO con las responsabilidades del cliente</returns>
    private List<CodigoObligacionDTO> GetListaObligaciones(string strResponsabilidadFiscal)
    {
        List<CodigoObligacionDTO> lstCodigoObligacion = new List<CodigoObligacionDTO>();
        CodigoObligacionDTO codigoObligacionDto = new CodigoObligacionDTO()
        {
            CodigoObligacion = strResponsabilidadFiscal.Replace("0", "O")
        };
        lstCodigoObligacion.Add(codigoObligacionDto);
        return lstCodigoObligacion;
    }

    /// <summary>
    /// Método para obtener el tipo de tributo de cliente
    /// </summary>
    /// <param name="strResponsabilidadFiscal">Indica el tipo de responsabilidad fiscal del cliente</param>
    /// <returns>Retorna objeto TributoClienteDTO con los datos organizados</returns>
    private TributoClienteDTO GetTributoCliente(string strResponsabilidadFiscal)
    {
        return new TributoClienteDTO()
        {
            CodigoTributo = strResponsabilidadFiscal.Equals(Constant.NoResponsableFiscal) ? "ZZ" : "01",
            NombreTributo = strResponsabilidadFiscal.Equals(Constant.NoResponsableFiscal) ? "No Aplica" : "IVA"
        };
    }

    /// <summary>
    /// Método para obtener los email a notificar la factura electrónica.
    /// </summary>
    /// <param name="lstEmails">Lista de string con los emails a notificar.</param>
    /// <returns>Retorna lista de objetos ParaDTO con los emails organizados a notificar</returns>
    private List<ParaDTO> GetEmailsNotification(List<string> lstEmails)
    {
	    List<ParaDTO> lstPara = new List<ParaDTO>();
	    lstEmails.ForEach(email => lstPara.Add(new ParaDTO(){Para = email}));
	    return lstPara;
    }

}