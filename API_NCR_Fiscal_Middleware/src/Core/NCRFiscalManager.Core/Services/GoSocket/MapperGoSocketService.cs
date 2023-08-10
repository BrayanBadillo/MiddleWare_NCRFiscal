using AutoMapper;
using NCRFiscalManager.Core.Constants;
using NCRFiscalManager.Core.DTO.GoSocket;
using NCRFiscalManager.Core.DTO.NCR;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces;
using NCRFiscalManager.Core.Interfaces.GoSocket;
using NCRFiscalManager.Core.Interfaces.Repositories;
using NCRFiscalManager.Core.Utilities;
using Serilog;
using System.Text.Json;

namespace NCRFiscalManager.Core.Services;

public class MapperGoSocketService : IMapperGoSocketService
{
    private readonly IMapper _mapper;
    private readonly IPointOnSaleRepository _pointOnSaleRepository;
    private readonly IPaymentMethodsService _paymentMethodsService;
    private readonly IInvoiceTransactionPersistenceService _invoiceTransactionPersistenceService;

    private PointOnSale _pointOnSale;

    public MapperGoSocketService(IMapper mapper, IPointOnSaleRepository pointOnSaleRepository, IPaymentMethodsService paymentMethodsService, IInvoiceTransactionPersistenceService invoiceTransactionPersistenceService)
    {
        _mapper = mapper;
        _pointOnSaleRepository = pointOnSaleRepository;
        _paymentMethodsService = paymentMethodsService;
        _invoiceTransactionPersistenceService = invoiceTransactionPersistenceService;
    }


    /// <summary>
    /// Método para organizar los datos para enviar a GoSocket
    /// </summary>
    /// <param name="documentDto">Objeto DocumentoDTO con los datos a organizar y enviar al operador tecnológico</param>
    /// <param name="claveTcNotaCredito"> Clave técnica para generar Nota Crédito</param>
    /// <returns>Retorna objeto DTE con los datos organizados a enviar</returns>
    /// <exception cref="Exception"></exception>
    public DTE GetGoSocketClientDTO(DocumentDTO documentDto, string claveTcNotaCredito)
    {
        try
        {
            _pointOnSale = _pointOnSaleRepository.GetStoreByKey(documentDto.EmisorIdExterno, documentDto.NumeroResolucion + "-" + documentDto.NumeroSerie);
            DTE dteDto = new DTE();

            dteDto.Documento = new DocumentDteDTO()
            {
                Encabezado = new EncabezadoDTO()
                {
                    Emisor = _mapper.Map<EmisorDTO>(documentDto),
                    Receptor = _mapper.Map<ReceptorDTO>(documentDto)
                },
                Detalle = DetalleGoSocketMap(documentDto.Articulos)
            };

            dteDto.Personalizados = new Personalizados()
            {
                DocPersonalizado = new DocPersonalizado() { campoString = _pointOnSale.Name }
            };

            string claveTc = String.Empty;

            // Validacion para identificar si es una nota crédito
            if (documentDto.DocumentoReferencia != null)
            {
                dteDto.Documento.Encabezado.IdDoc = GetDataCrediNote(documentDto);
                dteDto.Documento.Encabezado.IdDoc.MedioPago = GetMedioPagoNotaCredito(_pointOnSale.Id, documentDto.DocumentoReferencia.CufeReference);
                dteDto.Documento.Referencia = GetReferenciaCreditNote(documentDto.DocumentoReferencia);
                claveTc = claveTcNotaCredito;
            }
            else
            {
                dteDto.Documento.Encabezado.IdDoc = _mapper.Map<IdDocDTO>(documentDto);
                dteDto.Documento.Encabezado.IdDoc.MedioPago = GetMedioPagoGoSocket(_paymentMethodsService.GetPaymentMethod(_pointOnSale.EmitterInVoiceId, int.Parse(documentDto.Pagos[0].PagoId)));
                claveTc = _pointOnSale.LlaveTecnica;
            }

            dteDto.Documento.CAE = GetCaeDatos(documentDto.NumeroResolucion, documentDto.NumeroSerie, claveTc);

            dteDto.ID = "11";
            dteDto.Documento.Encabezado.Emisor.CodigoEmisor.CdgIntEmisor = _pointOnSale.EmitterInVoice.ObligationCode;
            dteDto.Documento.Encabezado.Emisor.NmbEmisor = _pointOnSale.EmitterInVoice.TradeName;
            dteDto.Documento.Encabezado.Emisor.NombreEmisor = new NombreEmisorDTO()
            { PrimerNombre = _pointOnSale.EmitterInVoice.BusinessName };
            dteDto.Documento.Encabezado.Emisor.ContactoEmisor = GetContactoEmisor();

            dteDto.Documento.Encabezado.Emisor.DomFiscal.Calle = _pointOnSale.Address;
            dteDto.Documento.Encabezado.Emisor.LugarExped.Calle = _pointOnSale.Address;

            // dteDto.Documento.Encabezado.Impuestos = GetImpuestosGoSocket(documentDto.Articulos);
            dteDto.Documento.Encabezado.Impuestos = GetImpuestosGoSocketRecalculate(dteDto.Documento.Detalle, documentDto.Articulos);

            if (documentDto.RecargoPorServicio > 0)
            {
                dteDto.Documento.RecargosGlobalesDto = GetRecargosGlobalesGoSocket(
                    documentDto.Pagos,
                    dteDto.Documento.Encabezado.Impuestos,
                    (decimal)documentDto.RecargoPorServicio);
            }

            //dteDto.Documento.Encabezado.Totales = GetTotalesGoSocket(documentDto.Articulos, documentDto.OtrosRecargos);
            dteDto.Documento.Encabezado.Totales = GetTotalesGoSocketRecalculate(dteDto.Documento.Encabezado.Impuestos, dteDto.Documento.RecargosGlobalesDto);

            dteDto.Documento.Encabezado.IdDoc.Ambiente = _pointOnSale.IsProduction
                ? Constant.ProductionEnvironment.ToString()
                : Constant.TestEnvironment.ToString();

            return dteDto;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// Método para obtener los datos asociados a la facturación electrónica asignada.
    /// </summary>
    /// <param name="strNumeroResolucion">Número de resolución asignada por la DIAN</param>
    /// <param name="strNumeroSerie">Número de serie asignado por la DIAN</param>
    /// <param name="claveTC">Clave para la transacción con el operador tecnológico</param>
    /// <returns>Retorna objeto CaeDTO con los datos asociados a la facturación electrónica</returns>
    private CaeDTO GetCaeDatos(string strNumeroResolucion, string strNumeroSerie, string claveTC)
    {
        return new CaeDTO()
        {
            Tipo = 1,
            Serie = strNumeroSerie,
            NroResolucion = long.Parse(strNumeroResolucion),
            NumeroInicial = long.Parse(_pointOnSale.InitInvoiceNumber),
            NumeroFinal = long.Parse(_pointOnSale.FinalInvoiceNumber),
            FechaResolucion = _pointOnSale.DateOfResolution,
            ClaveTC = claveTC, // TODO Definir si se guarda en el POS o en el Emitter
            Plazo = _pointOnSale.Plazo
        };
    }

    /// <summary>
    /// Método para parametrizar el método de pago a GoSocket.
    /// </summary>
    /// <param name="strMedioPago">String con el medio de pago a parametrizar</param>
    /// <returns>Retorna string con el valor parametrizado</returns>
    private string GetMedioPagoGoSocket(string strMedioPago)
    {
        // TODO Se debe validar una forma genérica para convertir el medio de pago de acuerdo a la tienda
        if (strMedioPago == null) return strMedioPago;
        string medioPago = strMedioPago.Trim().ToLower();
        switch (medioPago)
        {
            case "credicard":
            case "tarjet d/c":
                return "48";
            case "amex":
                return "48";
            case "visa":
                return "48";
            case "diners":
                return "48";
            case "falabella":
                return "48";
            case "mastercard":
                return "48";
            case "bono pub":
                return "71";
            case "b sodexo":
                return "71";
            case "b big pass":
                return "48";
            case "bonos":
                return "71";
            case "t sodexo":
                return "48";
            case "wompi":
                return "ZZZ";
            case "rappi":
                return "ZZZ";
            case "cxc":
                return "ZZZ";
            case "payu":
                return "ZZZ";
            case "didifood":
                return "ZZZ";
            case "efectivo":
                return "10";
            case "cheque":
                return "20";
            default:
                return "1";
        }
    }

    /// <summary>
    /// Método para obtener los datos de contacto del emisor de la factura.
    /// </summary>
    /// <returns>Retorna objeto ContactoEmisorDTO con los datos obtenidos</returns>
    private ContactoEmisorDTO GetContactoEmisor()
    {
        return new ContactoEmisorDTO()
        {
            Tipo = "1",
            Descripcion = "Datos emisor factura",
            eMail = _pointOnSale.EmitterInVoice.Email
        };
    }

    private TotalesDTO GetTotalesGoSocketRecalculate(List<ImpuestosDTO> Impuestos, RecargosGlobalesDto RecargosTotales)
    {
        try
        {
            TotalesDTO totalesDto = new TotalesDTO();
            totalesDto.Moneda = Constant.MonedaCop;
            totalesDto.SubTotal = (Impuestos.Sum(x => x.MontoBAseImp));
            totalesDto.MntBase = (Impuestos.Sum(x => x.MontoBAseImp));
            totalesDto.MntImp = (Impuestos.Sum(x => x.MontoImp));

            if (RecargosTotales != null)
            {
                if ((float)RecargosTotales.ValorDR > 0)
                {
                    totalesDto.VlrPagar = (totalesDto.SubTotal + totalesDto.MntImp + RecargosTotales.ValorDR);

                    // ---  Marcos --- totalesDto.MntRcgo = totalesDto.VlrPagar - (totalesDto.MntBase + totalesDto.MntImp);
                    totalesDto.MntRcgo = (decimal)RecargosTotales.ValorDR;
                }
                else
                {
                    totalesDto.VlrPagar = (totalesDto.SubTotal + totalesDto.MntImp);
                }
            }
            else
            {
                totalesDto.VlrPagar = (totalesDto.SubTotal + totalesDto.MntImp);
            }
            return totalesDto;

        }
        catch (Exception error)
        {
            return null;
        }
    }

    /// <summary>
    /// Validar el listado de productos para reemplazar los valores invertidos de Bono Regalo.
    /// </summary>
    /// <param name="lstItems">Lista de artículos.</param>
    /// <returns>Lista de artículos con el ajuste en el bono de </returns>
    private List<ItemDTO> ValidateItemListForGiftItems(List<ItemDTO> lstItems)
    {
        //List<ItemDTO> newList = new();

        return lstItems.Select(x =>
        {
            if (x.MontoGravado == 0.0)
            {
                x.MontoGravado = x.MontoIVA;
                x.MontoIVA = 0.0f;
            }
            return x;

        }).ToList();
    }

    private List<ImpuestosDTO> GetImpuestosGoSocketRecalculate(List<DetalleGoSocketDTO> lstItems, List<ItemDTO> ListArticulos)
    {
        try
        {
            // Obtengo agrego el valor de la descripcion del iva que no lo tenia 
            foreach (var item in lstItems)
            {
                var obj = ListArticulos.FirstOrDefault(x => x.CodigoArticulo == item.CdgItem.VlrCodigo);
                if (obj != null)
                {
                    if (item.ImpuestosDet != null)
                    {
                        item.ImpuestosDet.DescripcioTax = obj.DescripcionIVA;
                    }
                }
            }

            List<ImpuestosDTO> lstImpuestos = new List<ImpuestosDTO>();

            var groupByTipoImpuesto = lstItems.Where(x => x.ImpuestosDet != null)
            .GroupBy(item => new { item.ImpuestosDet.DescripcioTax, item.ImpuestosDet.TasaImp });

            // decimal xx = 0;

            foreach (var item in groupByTipoImpuesto)
            {
                ImpuestosDTO impuestosDto = new ImpuestosDTO();

                var TapImpuestos = item.FirstOrDefault(x => x.ImpuestosDet != null);

                //var cc = item.Where(x => x.ImpuestosDet != null).ToList();

                //foreach (var obj in cc)
                //{
                //    var jjjj = obj.ImpuestosDet.MontoBaseImp;
                //    xx = xx + (decimal)jjjj;
                //}

                if (TapImpuestos != null)
                {
                    if (item.Key.DescripcioTax == "" || item.Key.DescripcioTax == null)
                    { impuestosDto.TipoImp = Constant.GetTaxType()["No Tax"]; }

                    else
                    { impuestosDto.TipoImp = Constant.GetTaxType()[item.Key.DescripcioTax]; }

                    impuestosDto.TasaImp = (item.Key.TasaImp);

                    // Sumatoria montos base tasados con  la misma tasa(caso 1, caso 2) -- Precio sin impuesto x cant
                    impuestosDto.MontoBAseImp = item.Sum(i => (i.ImpuestosDet.MontoBaseImp));

                    // sumatoria de los imp tasados con la misma tasa(caso 1, caso 2) -- Monto Base (Precio x cant ) * tasa
                    impuestosDto.MontoImp = (item.Sum(i => (i.ImpuestosDet.MontoImp)));
                }
                lstImpuestos.Add(impuestosDto);
            }

            foreach (var item in lstItems)
            {
                if (item.ImpuestosDet != null)
                {
                    item.ImpuestosDet.DescripcioTax = null;
                }
            }

            return lstImpuestos;
        }
        catch (Exception error)
        {
            return null;
        }
    }

    private RecargosGlobalesDto GetRecargosGlobalesGoSocket(List<PaymentDTO> lstItems, List<ImpuestosDTO> ListImpuestos, decimal Propinas)
    {

        decimal MontoBaseTotal = (decimal)(ListImpuestos.Sum(x => x.MontoBAseImp));

        try
        {
            RecargosGlobalesDto RecargosGlobalesDto = new RecargosGlobalesDto();

            RecargosGlobalesDto.TpoMov = "R";
            RecargosGlobalesDto.GlosaDR = "Propinas";

            RecargosGlobalesDto.ValorDR = (decimal)Propinas;

            if (Propinas > 0)
            {
                decimal Numerador = (Propinas * 100);
                decimal Denominador = (decimal)MontoBaseTotal;
                RecargosGlobalesDto.PctDR = DecimalUtilities.FormaterDecimal(((float)Numerador / (float)Denominador)).ToString().Replace(",", ".");
            }
            else
            {
                RecargosGlobalesDto.PctDR = "0";
            }
            return RecargosGlobalesDto;
        }
        catch (Exception e)
        {
            return null;
        }
    }


    private List<DetalleGoSocketDTO> DetalleGoSocketMap(List<ItemDTO> list)
    {
        List<DetalleGoSocketDTO> listDetail = new List<DetalleGoSocketDTO>();

        foreach (var item in list)
        {
            DetalleGoSocketDTO DetalleGoSocketDTO = new DetalleGoSocketDTO();

            DetalleGoSocketDTO.CdgItem = new CdgItemDTO() { TpoCodigo = "999", VlrCodigo = item.CodigoArticulo };
            DetalleGoSocketDTO.NroLinDet = 1;
            DetalleGoSocketDTO.DscItem = item.DescripcionArticulo;
            DetalleGoSocketDTO.QtyItem = (float)item.Cantidad;
            // Unidad de medida unidad-->94
            DetalleGoSocketDTO.UnmdItem = "94";


            if (item.Descuento == 0 && item.Total > 0)
            {
                // (Precio Unitario sin impuestos )
                DetalleGoSocketDTO.PrcBrutoItem = DecimalUtilities.FormaterDecimal(item.PrecioUnitario / (1 + item.TasaIva));

                // Valor producto con impuestos
                DetalleGoSocketDTO.PrcNetoItem = DecimalUtilities.FormaterDecimal(item.PrecioUnitario);

                //  -- (precio unitario sin impuesto) * (cantidad)
                DetalleGoSocketDTO.MontoTotalItem = (DetalleGoSocketDTO.PrcBrutoItem * item.Cantidad);

                DetalleGoSocketDTO.SubMonto = GetSubMontoDTO(item.PrecioUnitario, item.CodigoArticulo);

                if (DetalleGoSocketDTO.SubMonto == null)
                {
                    DetalleGoSocketDTO.ImpuestosDet = GetImpuestoDetalle(item);
                }

                listDetail.Add(DetalleGoSocketDTO);
            }
            else
            {
                // Descuentos = se resta el descuento al valor total del articulo

                // (Precio Unitario sin impuestos )
                float PrecioUnitarioDesc = item.PrecioUnitario - item.Descuento;
                DetalleGoSocketDTO.PrcBrutoItem = DecimalUtilities.FormaterDecimal(PrecioUnitarioDesc / (1 + item.TasaIva));

                // Valor producto con impuestos
                DetalleGoSocketDTO.PrcNetoItem = DecimalUtilities.FormaterDecimal(PrecioUnitarioDesc);

                //  -- (precio unitario sin impuesto) * (cantidad)
                DetalleGoSocketDTO.MontoTotalItem = (DetalleGoSocketDTO.PrcBrutoItem * item.Cantidad);

                DetalleGoSocketDTO.SubMonto = GetSubMontoDTO(item.PrecioUnitario, item.CodigoArticulo);

                if (DetalleGoSocketDTO.SubMonto == null)
                {
                    DetalleGoSocketDTO.ImpuestosDet = GetImpuestoDetalle(item);
                }

                listDetail.Add(DetalleGoSocketDTO);
            }
        }

        return listDetail;
    }

    private SubMontoDTO GetSubMontoDTO(float precioUnitario, string CodigoArticulo)
    {
        if ((precioUnitario == 0) || (Constant.GetExcludeItemTax().Contains(CodigoArticulo)))
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

    private ImpuestosDetalleDTO GetImpuestoDetalle(ItemDTO itemDto)
    {
        ImpuestosDetalleDTO ImpuestosDetalleDTO = new ImpuestosDetalleDTO();

        //  -- (precio unitario sin impuesto) * (cantidad)

        ImpuestosDetalleDTO.MontoBaseImp = (decimal)DecimalUtilities.FormaterDecimal((itemDto.PrecioUnitario / (1 + itemDto.TasaIva)) * itemDto.Cantidad);

        if (itemDto.DescripcionIVA == "" || itemDto.DescripcionIVA == null)
        {
            ImpuestosDetalleDTO.TipoImp = "";
        }
        else
        {
            ImpuestosDetalleDTO.TipoImp = Constant.GetTaxType()[itemDto.DescripcionIVA];
        }

        if (itemDto.TasaIva == 0)
        {
            ImpuestosDetalleDTO.MontoImp = 0;
            ImpuestosDetalleDTO.TasaImp = 0;
        }
        else
        {
            // impuesto detalle = (precio sin impuestos *cantidad ) + tasa
            ImpuestosDetalleDTO.MontoImp = (decimal)DecimalUtilities.FormaterDecimal((float)ImpuestosDetalleDTO.MontoBaseImp * (float)itemDto.TasaIva);
            ImpuestosDetalleDTO.TasaImp = (decimal)itemDto.TasaIva * 100;
        }

        return ImpuestosDetalleDTO;
    }

    /// <summary>
    /// Método para organizar los datos para un Nota Crédito
    /// </summary>
    /// <param name="documentDto">Objeto DocumentoDTO con los datos a organizar y enviar al operador tecnológico</param>
    /// <returns>Retorna objeto IdDocDTO con los dato organizados</returns>
    private IdDocDTO GetDataCrediNote(DocumentDTO documentDto)
    {
        try
        {
            return new IdDocDTO()
            {
                // De acuerdo al catálogo de integración 91 es para tipo de operación Nota Crédito
                Tipo = "91",
                TipoServicio = "20",
                TipoNegociacion = "1",
                Serie = documentDto.DocumentoReferencia.NumeroSerie,
                Numero = long.Parse(documentDto.CorrelativoFiscal),
                NumeroInterno = documentDto.DocumentoReferencia.CorrelativoFiscal,
                FechaEmis = Convert.ToDateTime(documentDto.FechaEmision)
            };
        }
        catch (Exception e)
        {
            Log.Error($"Error GetDataCrediNote {e}");
            throw;
        }
    }

    /// <summary>
    /// Método para organizar los datos de referencia para una Nota Crédito.
    /// </summary>
    /// <param name="documentoReferenciaDto">Objeto DocumentoReferenciaDTO con los datos de la factura a hacer Nota Crédito</param>
    /// <returns>Retorna lista de objetos ReferenciaDTO con los datos de referencia</returns>
    private List<ReferenciaDTO> GetReferenciaCreditNote(DocumentoReferenciaDTO documentoReferenciaDto)
    {
        return new List<ReferenciaDTO>()
        {
            new ReferenciaDTO()
            {
                NroLinRef = 1,
                // Valor de acuerdo al catálogo de integración
                TpoDocRef = "01",
                SerieRef = documentoReferenciaDto.NumeroSerie,
                NumeroRef = documentoReferenciaDto.CorrelativoFiscal,
                FechaRef = documentoReferenciaDto.FechaEmision.ToString("yyyy-MM-dd"),
                // Valor de acuerdo al catálogo de integración
                CodRef = 2,
                RazonRef = "Anulación de factura electrónica",
                Ecb01 = documentoReferenciaDto.CufeReference
            }
        };
    }

    /// <summary>
    /// Método para obtener el medio de pago enviado en la factura a realizar nota crédito.
    /// </summary>
    /// <param name="pointOnSaleId">Identificador del punto de venta</param>
    /// <param name="cufe">CUFE generado desde la DIAN</param>
    /// <returns>Retorna el medio de pago parametrizado</returns>
    private string GetMedioPagoNotaCredito(long pointOnSaleId, string cufe)
    {
        // Por defecto se coloca el código 10 que pertenece a efectivo
        string medioPago = "10";
        try
        {
            InvoiceTransaction invoiceTransaction = _invoiceTransactionPersistenceService.GetInvoiceTransactionByCufeAndPointOfSaleId(pointOnSaleId, cufe);
            if (invoiceTransaction != null)
            {
                DTE documentDte = JsonSerializer.Deserialize<DTE>(invoiceTransaction.Request);
                medioPago = documentDte.Documento.Encabezado.IdDoc.MedioPago;
            }
        }
        catch (Exception e)
        {
            Log.Error($"Error GetMedioPagoNotaCredito {e}");
        }
        return medioPago;
    }
}