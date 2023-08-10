using AutoMapper;
using System.Text.RegularExpressions;
using NCRFiscalManager.Core.Constants;
using NCRFiscalManager.Core.DTO.NCR;
using NCRFiscalManager.Core.DTO.TecnoFactor;

namespace NCRFiscalManager.Core.Mappers.TecnoFactor
{
    public class DetalleProfile : Profile
    {
        
        private readonly Dictionary<string,string> _tipoImpuesto = new Dictionary<string, string>()
        {
            {"IVA","IVA"},
            {"IMPOCONSUMO","INC"},
            {"NO", "NO_CAUSA"}
        };
        public DetalleProfile()
        {
            CreateMap<ItemDTO, DetalleDTO>()
                .ForMember(dest => dest.CodigoArticulo, opt => opt.MapFrom(src => src.CodigoArticulo))
                .ForMember(dest => dest.DescripcionArticulo, opt => opt.MapFrom(src => GetDescripcion(src.DescripcionArticulo)))
                .ForMember(dest => dest.UnidadMedida, opt => opt.MapFrom(src => src.UnidadMedida.Replace("Uni", Constant.Unidad)))
                .ForMember(dest => dest.Cantidad, opt => opt.MapFrom(src => src.Cantidad))
                .ForMember(dest => dest.PrecioUnitario, opt => opt.MapFrom(src => src.MontoGravado))
                .ForMember(dest => dest.CargosDescuentos, opt => opt.MapFrom(src => GetCargosDescuentos(src.Descuento)))
                .ForMember(dest => dest.Obsequio, opt => opt.MapFrom(src => GetObsequio(src.PrecioUnitario)))
                .ForMember(dest => dest.ImpuestosRetenciones, opt => opt.MapFrom(src => GetImpuestos(src.MontoIVA, src.TasaIva, src.DescripcionIVA)));
        }
        
        /// <summary>
        /// Método para quitar caracteres especiales de la descripcion de un producto.
        /// </summary>
        /// <param name="strDescripcion">Descripción a validar</param>
        /// <returns>Retorna descripción sin caracteres especiales</returns>
        private string GetDescripcion(string strDescripcion)
        {
            string strDescripcionArticulo = null;
            if (strDescripcion != null)
            {
                strDescripcionArticulo = Regex.Replace(strDescripcion, @"[^\w\s.!@$%^&*()\-\/]+", "");
                if (strDescripcionArticulo.Length < 5)
                {
                    strDescripcionArticulo = "Artículo " + strDescripcionArticulo;
                }
            }
            return strDescripcionArticulo;
        }

        /// <summary>
        /// Método para obtener los impuestos del producto.
        /// </summary>
        /// <param name="fPorcentaje">Valor del porcentaje del impuesto</param>
        /// <param name="strDescripcionImpuesto">Descripción del impuesto</param>
        /// <returns>Retorna lista de objetos ImpuestosRetencionesDTO con los datos obtenidos</returns>
        private List<ImpuestosRetencionesDTO> GetImpuestos(float fMontoIva, float fPorcentaje, string strDescripcionImpuesto)
        {
            try
            {
                List<ImpuestosRetencionesDTO> lstImpuestos = new List<ImpuestosRetencionesDTO>();
                if (fPorcentaje > 0 && strDescripcionImpuesto != null)
                {
                    float porcentaje = (float)(Math.Truncate(fPorcentaje * 100.00f));
                    string tipoImpuesto = strDescripcionImpuesto.Split(" ")[0].Trim();
                    lstImpuestos.Add(new ImpuestosRetencionesDTO(){Tributo = _tipoImpuesto[tipoImpuesto], Porcentaje = porcentaje, ValorImpuestoRetencion = fMontoIva});
                }

                return lstImpuestos;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Método para organizar los descuentos a reportar a TecnoFactor.
        /// </summary>
        /// <param name="fDescuento">Valor de descuento</param>
        /// <returns>Retorna lista de objetos CargosDescuentosDTO con los datos obtenidos</returns>
        private List<CargosDescuentosDTO> GetCargosDescuentos(float fDescuento)
        {
            try
            {
                List<CargosDescuentosDTO> lstCargosDescuento = new List<CargosDescuentosDTO>();
                if (fDescuento > 0)
                {
                    CargosDescuentosDTO cargosDescuentosDto = new CargosDescuentosDTO()
                    {
                        Tipo = false,
                        Valor = fDescuento
                    };
                    lstCargosDescuento.Add(cargosDescuentosDto);
                }

                return lstCargosDescuento;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Método para organizar los datos de obsequio a TecnoFactor.
        /// </summary>
        /// <param name="fPrecioUnitario">Valor de obsequio</param>
        /// <returns>Retorna objeto ObsequioDTO con los datos organizados</returns>
        private ObsequioDTO GetObsequio(float fPrecioUnitario)
        {
            try
            {
                ObsequioDTO obsequioDto = null;
                if (fPrecioUnitario == 0)
                {
                    obsequioDto = new ObsequioDTO()
                    {
                        PrecioReferencia = 1f,
                        TipoPrecioReferencia = "VALOR_INVENTARIOS"
                    };
                }

                return obsequioDto;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}