using AutoMapper;
using Microsoft.Extensions.Configuration;
using NCRFiscalManager.Core.Constants;
using NCRFiscalManager.Core.DTO.NCR;
using NCRFiscalManager.Core.DTO.TecnoFactor;
using NCRFiscalManager.Core.Interfaces;

namespace NCRFiscalManager.Core.Services;

public class MapperTecnoFactorService : IMapperTecnoFactorService
{
    private readonly IDBFManager _dbfManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public MapperTecnoFactorService(IDBFManager dbfManager, IConfiguration configuration, IMapper mapper)
    {
        _dbfManager = dbfManager;
        _configuration = configuration;
        _mapper = mapper;
    }
    
    /// <summary>
        /// Servicio para mapear los datos entre AFM Fiscal y TecnoFactor.
        /// </summary>
        /// <param name="documentDto">Objeto de tipo DocumentDTO con los datos a enviar al Operador Tecnológico</param>
        /// <returns>Retorna objeto RequestDocumentDTO con la estructura a enviar al Operador Tecnológico</returns>
        public RequestDocumentDTO GetTecnoFactorClientDTO(DocumentDTO documentDto)
        {
            try
            {
                RequestDocumentDTO requestDocumentoDto = _mapper.Map<RequestDocumentDTO>(documentDto);
                if (documentDto.Pagos != null && documentDto.Pagos.Count > 0)
                {
                    requestDocumentoDto.Pago = getPago(documentDto.Pagos[0].PagoId);
                    DetalleDTO dPropina = getPropina(documentDto.Pagos[0].Propina);
                    if (dPropina != null)
                    {
                        requestDocumentoDto.Detalles.Add(dPropina);
                    }
                }

                if (documentDto.DocumentoReferencia != null)
                {
                    requestDocumentoDto.Pago = null;
                    List<FacturasReferenciaDTO> lstFacturasReferencia = new List<FacturasReferenciaDTO>();
                    FacturasReferenciaDTO facturasReferenciaDto = new FacturasReferenciaDTO();
                    facturasReferenciaDto.Prefijo = documentDto.DocumentoReferencia.NumeroSerie;
                    facturasReferenciaDto.Numero = documentDto.DocumentoReferencia.CorrelativoFiscal;
                    facturasReferenciaDto.ConceptoNotaCredito = documentDto.DocumentoReferencia.ConceptoNotaCredito;
                    lstFacturasReferencia.Add(facturasReferenciaDto);
                    requestDocumentoDto.FacturasReferencia = lstFacturasReferencia;
                }
                
                requestDocumentoDto.FacturasReferencia = new List<FacturasReferenciaDTO>();
                requestDocumentoDto.ImpuestosRetenciones = new List<ImpuestosRetencionesDTO>();
                return requestDocumentoDto;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Método para organizar los datos de pago para TecnoFactor.
        /// </summary>
        /// <param name="strIdPago">Identificador del medio de pago</param>
        /// <returns>Retorna objeto PagoDTO con los datos organizados</returns>
        /// <exception cref="Exception"></exception>
        private PagoDTO getPago(string strIdPago)
        {
            try
            {
                string strMedioPago = getMedioPagoDbf(strIdPago);
                return new PagoDTO()
                {
                    MedioPago = getMedioPagoTecnoFactor(strMedioPago),
                    FormaPago = "CONTADO"
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string getMedioPagoDbf(string strIdPago)
        {
            try
            {
                string filePath = Path.Combine(_configuration.GetValue<string>("DBF:BasePath"), "TDR.DBF");
                Dictionary<int, string> dtrTdr = _dbfManager.ReadDictionaryFromDBFWithIntKey(filePath, "ID", "NAME");
                return dtrTdr.Where(tdr => tdr.Key == Convert.ToInt32(strIdPago)).FirstOrDefault().Value;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Método para parametrizar el método de pago a TecnoFactor.
        /// </summary>
        /// <param name="strMedioPago">String con el medio de pago a parametrizar</param>
        /// <returns>Retorna string con el valor parametrizado</returns>
        private string getMedioPagoTecnoFactor(string strMedioPago)
        {
            if (strMedioPago == null) return strMedioPago;
            string medioPago = strMedioPago.Trim();
            switch (medioPago)
            {
                case "Tarjet D/C":
                    return "TARJETA_CREDITO";
                case "BonosC&W": 
                case "BonoVirC&W":
                    return "BONOS";
                case "EFECTIVO":
                case "Cheque":
                    return strMedioPago.ToUpper().Trim();
                default:
                    return "OTRO";
            }
        }

        /// <summary>
        /// Método para organizar la propina como un detalle en la factura.
        /// </summary>
        /// <param name="fPropina">Valor de la propina</param>
        /// <returns>Retorna objeto DetalleDTO con los datos organizados de propina</returns>
        /// <exception cref="Exception"></exception>
        private DetalleDTO getPropina(float fPropina)
        {
            try
            {
                DetalleDTO detalleDto = null;
                if (fPropina > 0)
                {
                    detalleDto = new DetalleDTO()
                    {
                        PrecioUnitario = fPropina,
                        Cantidad = 1,
                        UnidadMedida = Constant.Unidad,
                        DescripcionArticulo = "Propina",
                        CodigoArticulo = "99999",
                        ImpuestosRetenciones = new List<ImpuestosRetencionesDTO>()
                        {
                            new ImpuestosRetencionesDTO()
                            {
                                Tributo = "IVA",
                                Porcentaje = 0
                            }
                        },
                    };
                }

                return detalleDto;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
}