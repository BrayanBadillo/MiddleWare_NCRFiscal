using AutoMapper;
using NCRFiscalManager.Core.Constants;
using NCRFiscalManager.Core.DTO.GoSocket;
using NCRFiscalManager.Core.DTO.NCR;
using NCRFiscalManager.Core.Utilities;

namespace NCRFiscalManager.Core.Mappers.GoSocket;

public class DetalleGoSocketProfile : Profile
{
    public DetalleGoSocketProfile()
    {
        CreateMap<ItemDTO, DetalleGoSocketDTO>()
            .ForMember(dest => dest.CdgItem, opt => opt.MapFrom(src => new CdgItemDTO() { TpoCodigo = "999", VlrCodigo = src.CodigoArticulo }))
            .ForMember(dest => dest.NroLinDet, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.DscItem, opt => opt.MapFrom(src => src.DescripcionArticulo))
            .ForMember(dest => dest.QtyItem, opt => opt.MapFrom(src => (double)src.Cantidad))
            // Unidad de medida unidad-->94
            .ForMember(dest => dest.UnmdItem, opt => opt.MapFrom(src => "94"))
            .ForMember(dest => dest.PrcBrutoItem, opt => opt.MapFrom(src => src.MontoGravado))
            .ForMember(dest => dest.PrcNetoItem, opt => opt.MapFrom(src => src.PrecioUnitario))
            .ForMember(dest => dest.ImpuestosDet, opt => opt.MapFrom(src => GetImpuestoDetalle(src)))
            .ForMember(dest => dest.MontoTotalItem, opt => opt.MapFrom(src => src.MontoGravado))
            .ForMember(dest => dest.SubMonto, opt => opt.MapFrom(src => GetSubMontoDTO(src.PrecioUnitario)));
    }

   

    /// <summary>
    /// Método para obtener el detalle del impuesto por artículo
    /// </summary>
    /// <param name="itemDto">Objeto ItemDTO con los datos del artículo asociado a la orden</param>
    /// <returns>Retorna objeto ImpuestoDetalleDTO con los datos organizados</returns>
    private ImpuestosDetalleDTO GetImpuestoDetalle(ItemDTO itemDto)
    {
        ImpuestosDetalleDTO ImpuestosDetalleDTO = new ImpuestosDetalleDTO()
        {
            TipoImp = Constant.GetTaxType()[itemDto.DescripcionIVA],
            TasaImp = (decimal)DecimalUtilities.FormaterDecimal(itemDto.TasaIva * 100),
            MontoImp = (decimal)DecimalUtilities.FormaterDecimal(itemDto.MontoGravado * itemDto.TasaIva),
            MontoBaseImp = (decimal)DecimalUtilities.FormaterDecimal(itemDto.MontoGravado)
        };
        return ImpuestosDetalleDTO;
    }

    /// <summary>
    /// Método para validar y organizar los datos si el precio de un artículo es de 0.
    /// </summary>
    /// <param name="precioUnitario">Precio unitario del artículo</param>
    /// <returns>Retorna objeto SubMontoDTO con los datos organizados</returns>
    private SubMontoDTO GetSubMontoDTO(float precioUnitario)
    {
        if (precioUnitario == 0)
        {
            return new SubMontoDTO()
            {
                Tipo = "FREE",
                CodTipoMonto = "01",
                MontoConcepto = 1f
            };
        }

        return null;
    }
}