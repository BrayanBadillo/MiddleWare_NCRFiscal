using AutoMapper;
using NCRFiscalManager.Core.Constants;
using NCRFiscalManager.Core.DTO.Facture.Request;
using NCRFiscalManager.Core.DTO.NCR;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces;
using NCRFiscalManager.Core.Interfaces.Facture;
using NCRFiscalManager.Core.Interfaces.Repositories;
using System.Globalization;
using DocumentoReferenciaDTO = NCRFiscalManager.Core.DTO.NCR.DocumentoReferenciaDTO;

namespace NCRFiscalManager.Core.Services.Facture;

public class MapperFactureService : IMapperFactureService
{
    private readonly IMapper _mapper;
    private readonly IPointOnSaleRepository _pointOnSaleRepository;
    private readonly IPaymentMethodsService _paymentMethodsService;
    
    private PointOnSale _pointOnSale;

    public MapperFactureService( IMapper mapper, IPointOnSaleRepository pointOnSaleRepository, IPaymentMethodsService paymentMethodsService)
    {
        _mapper = mapper;
        _pointOnSaleRepository = pointOnSaleRepository ?? throw new ArgumentNullException(nameof(pointOnSaleRepository));
        _paymentMethodsService = paymentMethodsService ?? throw new ArgumentNullException(nameof(paymentMethodsService));
    }
    
    /// <summary>
    /// Método para obtener los datos asociados a una factura
    /// </summary>
    /// <param name="documentDto">Objeto DocumentoDTO con los datos a organizar y enviar al operador tecnológico</param>
    /// <returns>Retorna objeto Factura con los datos organizados a enviar al operador tecnológico</returns>
    /// <exception cref="Exception"></exception>
    public Factura GetFactureClientDTO(DocumentDTO documentDto)
    {
        try
        {
            float advancePaymentValue = 0.0f;
            var chargesAndTips = documentDto.RecargoPorServicio + documentDto.RecargoModoPedido;
            var advancePaymentMethod = documentDto.Pagos.FirstOrDefault(x => x.PagoDescripcion == Constant.AdvancePaymentMethodName);

            // Revisar ese campo debido a que debe ser el store key
            _pointOnSale = _pointOnSaleRepository.GetStoreByKey(documentDto.EmisorIdExterno, documentDto.NumeroResolucion + "-" + documentDto.NumeroSerie);

            if (advancePaymentMethod is not null)
                advancePaymentValue = advancePaymentMethod.MontoPagado;

            Factura factureInvoice = new Factura()
            {
                Cabecera = _mapper.Map<CabeceraDTO>(documentDto),
                NumeracionDIAN = GetNumeracionDIANFactureMapped(documentDto.NumeroResolucion, documentDto.NumeroSerie),
                MediosDePago = GetMediosDePagoFactureMapped(_pointOnSale.EmitterInVoiceId,documentDto.Pagos),
                Linea = GetProductsLines(documentDto.Articulos, documentDto.Descuentos),
                Anticipos = GetAnticiposFromPaymentMethods(documentDto.Pagos),
                DescuentosOCargos = GetPropinasDeServicio(documentDto),
                // Extensiones = new ExtensionesDTO() { POS = GetPOSPropertiesFactureMapped() },
                Emisor = GetEmisor(documentDto.EmisorId),
                Cliente = _mapper.Map<ClienteDTO>(documentDto),
                Notificacion = _mapper.Map<NotificacionDTO>(documentDto.Cliente),
                Impuestos = GetImpuestos(documentDto.Articulos),
                Totales = GetTotal(documentDto.Articulos, chargesAndTips, advancePaymentValue)
            };

            factureInvoice.SetLineNumbers();
            factureInvoice.Cabecera.Observaciones = GetPaymentMethodNamesAndDiscountsForHeaderObservations(_pointOnSale.EmitterInVoiceId, documentDto.Pagos, documentDto.Descuentos, documentDto.RecargoModoPedido, documentDto.RecargoPorServicio);
            factureInvoice.Cabecera.Ambiente = (_pointOnSale.IsProduction) ? 1 : 2;

            return factureInvoice;
        }
        catch(Exception ex)
        {
            throw new Exception($"An error ocurred while mapping the facture fields! [{ex}] ");
        }
    }
    
    /// <summary>
    /// Método para obtener los datos asociados a una nota crédito
    /// </summary>
    /// <param name="documentDto">Objeto DocumentoDTO con los datos a organizar y enviar al operador tecnológico</param>
    /// <returns>Retorna objeto NotaCredito con los datos organizados para enviar al operador tecnológico.</returns>
    /// <exception cref="Exception"></exception>
    public NotaCredito GetNotaCredito(DocumentDTO documentDto)
    {
        try
        {
            var chargesAndTips = documentDto.RecargoPorServicio + documentDto.RecargoModoPedido;
            _pointOnSale = _pointOnSaleRepository.GetStoreByKey(documentDto.EmisorIdExterno, documentDto.NumeroResolucion + "-" + documentDto.NumeroSerie);

            NotaCredito creditNote = new NotaCredito()
            {
                Cabecera = _mapper.Map<CabeceraCrediNoteDTO>(documentDto),
                ReferenciasNotas = GetReferenciasNotasFactureMapped($"{documentDto.DocumentoReferencia.NumeroSerie}{documentDto.DocumentoReferencia.CorrelativoFiscal}"),
                FacturasRelacionadas = GetFacturasRelacionadas(documentDto.DocumentoReferencia),
                Linea = GetProductsLines(documentDto.Articulos, documentDto.Descuentos),
                DescuentosOCargos = GetPropinasDeServicio(documentDto),
                // Extensiones = new ExtensionesDTO() { POS = GetPOSPropertiesFactureMapped() },
                Emisor = GetEmisor(documentDto.EmisorId),
                Cliente = _mapper.Map<ClienteDTO>(documentDto),
                Notificacion = _mapper.Map<NotificacionDTO>(documentDto.Cliente),
                Impuestos = GetImpuestos(documentDto.Articulos),
                Totales = GetTotal(documentDto.Articulos, chargesAndTips)
            };

            if (documentDto.Pagos.Count > 0)
            {
                creditNote.MediosDePago = GetMediosDePagoFactureMapped(_pointOnSale.EmitterInVoiceId,documentDto.Pagos);
            }
            
            creditNote.SetLineNumbers();
            creditNote.Cabecera.Observaciones = GetPaymentMethodNamesAndDiscountsForHeaderObservations(_pointOnSale.EmitterInVoiceId, documentDto.Pagos, documentDto.Descuentos, documentDto.RecargoModoPedido);
            creditNote.Cabecera.Ambiente = (_pointOnSale.IsProduction) ? 1 : 2;
            return creditNote;
        }
        catch (Exception ex)
        {
            throw new Exception($"An error ocurred while mapping the facture fields! [{ex}] ");
        }
    }


    /// <summary>
    /// Obtener los cargos o propinas en la factura.
    /// </summary>
    /// <param name="document">DocumentDTO con la información de la factura.</param>
    /// <returns>Objeto DescuentosOCargosDTO con los recargos.</returns>
    private DescuentosOCargosDTO GetPropinasDeServicio(DocumentDTO document)
    {
        var chargesAndTips = 
            new List<float> { document.RecargoPorServicio, document.RecargoModoPedido }
            .Where(charge => charge > 0)
            .Select(charge => new DescuentoOCargoDTO
            {
                ID = 1,
                Indicador = true,
                Justificacion = "Cargo por servicio, propinas o costo por domicilio.",
                Porcentaje = decimal.Truncate((decimal)charge / (decimal)document.MontoGravado * 100).ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
                Valor = charge.ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
                ValorBase = document.MontoGravado.ToString(Constant.DefaultRound, CultureInfo.InvariantCulture)
            })
            .ToList();
       
        return new DescuentosOCargosDTO()
        {
            DescuentoOCargo = chargesAndTips
        };
    }


    /// <summary>
    /// Obtener el anticipo desde los medios de pago.
    /// </summary>
    /// <param name="advancePaymentMethod">Lista de los métodos de pago.</param>
    /// <returns>Objeto AnticiposDTO con la información del anticipo completa.</returns>
    private AnticiposDTO GetAnticiposFromPaymentMethods(List<PaymentDTO> advancePaymentMethod)
    {
        var advancePayment = advancePaymentMethod.FirstOrDefault(x => x.PagoDescripcion == Constant.AdvancePaymentMethodName);

        if (advancePayment is null)
            return new AnticiposDTO();

        return new AnticiposDTO()
        {
            Anticipo = new List<AnticipoDTO>
            {
                new AnticipoDTO
                {
                    IDPago = advancePayment.PagoId,
                    MonedaAnticipo = Constant.MonedaCop,
                    ValorPagoAnticipo = advancePayment.MontoPagado,
                    FechaRecepcion = DateTime.Today.Date.ToString("yyyy-MM-dd"),

                }
            }
        };
    }

    /// <summary>
    /// Obtener observaciones de la FE con los metodos de pago, descuentos y cargos de modo pedido.
    /// </summary>
    /// <param name="emitterInvoiceId">Identificador de la tabla EmitterInvoice</param>
    /// <param name="paymentMethods">Listado de métodos de pago enviados por AFM.</param>
    /// <param name="discounts">Listado de descuentos enviados por AFM.</param>
    /// <param name="orderModeCharge">Valor del recargo por modo de pedido.</param>
    /// <returns>Cadena con los métodos de pago usados en la compra.</returns>
    private string GetPaymentMethodNamesAndDiscountsForHeaderObservations(long emitterInvoiceId, List<PaymentDTO> paymentMethods, List<DescuentosDTO> discounts = null, float orderModeCharge = 0, float serviceCharge = 0)
    {
        string observations = "Métodos de pago: ";
        foreach (PaymentDTO paymentMethod in paymentMethods)
        {
            var paymentMethodName = _paymentMethodsService.GetPaymentMethod(emitterInvoiceId, int.Parse(paymentMethod.PagoId));
            observations += string.Join( "/ " ,$"{paymentMethodName}: {paymentMethod.MontoPagado.ToString("C", CultureInfo.CurrentCulture)}/ ");
        }
        observations = observations.TrimEnd('/', ' ') + "." + "\n";
        
        if(discounts.Count > 0)
        {
            observations += "Descuentos: "; 
            foreach (var discount in discounts)
            {
                observations += $" / {discount.NombreDescuento}";
            }
            observations += "\n";
        }

        if(orderModeCharge != 0)
        {
            observations += $"Cargo por domicilio: {orderModeCharge.ToString("C", CultureInfo.CurrentCulture)}";
            observations += "\n";
        }

        if(serviceCharge != 0)
        {
            observations += $"Cargo por valor {serviceCharge.ToString("C", CultureInfo.CurrentCulture)} es equivalente a propinas.";
        }

        return observations;
    }

    /// <summary>
    /// Método para obtener los datos relacionados a la factura a realizar nota crédito.
    /// </summary>
    /// <param name="documentReference">Objeto DocumentoReferenciaDTO con los datos de la factura a hacer la nota crédito</param>
    /// <returns>Retorna objeto FacturasRelacionadasDTO con los datos organizados</returns>
    private FacturasRelacionadasDTO GetFacturasRelacionadas(DocumentoReferenciaDTO documentReference)
    {
        return new FacturasRelacionadasDTO()
        {
            FacturaRelacionada = new FacturaRelacionadaDTO()
            {
                Numero = $"{documentReference.NumeroSerie}{documentReference.CorrelativoFiscal}",
                Cufe = documentReference.CufeReference,
                FechaEmisionFA = documentReference.FechaEmision.ToString("yyyy-MM-dd")
            }
        };
    }

    /// <summary>
    /// Obtener la lista de 'lineas' o productos mapeados a facture.
    /// </summary>
    /// <param name="listOfItems">Lista de items</param>
    /// <returns>Lista de 'Lineas' de facture</returns>
    private List<LineaDTO> GetProductsLines(List<ItemDTO> listOfItems, List<DescuentosDTO> discounts)
    {
        List<LineaDTO> totalLines = new();
        foreach(ItemDTO item in listOfItems)
        {
            LineaDTO linea = new LineaDTO()
            {
                Detalle = _mapper.Map<DetalleDTO>(item),
                Impuestos = GetTaxesByLine(item),
                DescuentosOCargos = GetDiscountByLine(item, discounts),
                CodificacionesEstandar = new CodificacionesEstandarDTO()
                {
                    CodificacionEstandar = new CodificacionEstandarDTO()
                    {
                        CodigoEstandar = Constant.EstandarCodificacionArticulo,
                        CodigoArticulo = item.CodigoArticulo
                    }
                }
            };
            if (item.Total == 0)
            {
                linea.PrecioReferencia = new PrecioReferenciaDTO()
                {
                    ValorArticulo = "1.00",
                    CodigoTipoPrecio = "01",
                    MonedaPrecioReferencia = Constant.MonedaCop
                };
            }
            totalLines.Add(linea);
        }

        return totalLines;
    }

    /// <summary>
    /// Obtener los descuentos por linea.
    /// </summary>
    /// <param name="item">Item a buscar los descuentos.</param>
    /// <param name="discounts">Descuentos aplicados en la factura.</param>
    /// <returns>Objeto DescuentosOCargosDTO con la descripción del descuento por linea.</returns>
    private DescuentosOCargosDTO GetDiscountByLine(ItemDTO item, List<DescuentosDTO> discounts)
    {
        if (item.Descuento == 0.0) return new DescuentosOCargosDTO();

        return new DescuentosOCargosDTO
        {
            DescuentoOCargo = new List<DescuentoOCargoDTO>()
                    {
                        new DescuentoOCargoDTO
                        {
                            ID = 1,
                            Indicador = false,
                            Justificacion = discounts.FirstOrDefault().NombreDescuento,
                            Porcentaje = decimal.Truncate((decimal)Math.Abs(item.Descuento) / (decimal)(item.PrecioUnitario * item.Cantidad) * 100).ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
                            Valor = Math.Abs(item.Descuento).ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
                            ValorBase = (item.PrecioUnitario * item.Cantidad).ToString(Constant.DefaultRound, CultureInfo.InvariantCulture)

                        }
                    }
        };
    }

    /// <summary>
    /// Obtener el impuesto de cada linea.
    /// </summary>
    /// <param name="item">Objeto ItemDTO</param>
    /// <returns>Objeto con los impuestos de la linea.</returns>
    private ImpuestosLineaDTO GetTaxesByLine(ItemDTO item)
    {
        if (item.DescripcionIVA == "") item.DescripcionIVA = "No Tax";


        return new ImpuestosLineaDTO()
        {
            Impuesto = new ImpuestoLineaDTO()
            {
                Tipo = Constant.GetTaxType()[item.DescripcionIVA],
                Nombre = item.DescripcionIVA.Contains("IVA") ? "IVA" : "INC",
                Valor = Math.Round(item.MontoGravado * item.TasaIva).ToString(Constant.DefaultRound,  CultureInfo.InvariantCulture),
                // Redondeo = Constant.defaultRounding,
                Subtotal = new SubtotalLineaDTO()
                {
                    ValorBase = item.MontoGravado.ToString(Constant.DefaultRound,  CultureInfo.InvariantCulture),
                    Porcentaje = Constant.GetTaxValue()[item.TasaIva].ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
                    CodigoUnidMedidaBase = Constant.UnitOfMeasurement,
                    Valor = Math.Round(item.MontoGravado * item.TasaIva).ToString(Constant.DefaultRound, CultureInfo.InvariantCulture)
                }
            }
        };
    }
    

    /// <summary>
    /// Mapear el campo de numeracíon DIAN para facture
    /// </summary>
    /// <param name="numeroResolucion"></param>
    /// <param name="numeroSerie"></param>
    /// <returns>Objeto numeracionDIAN mapeado.</returns>
    private NumeracionDianDTO GetNumeracionDIANFactureMapped(string numeroResolucion, string numeroSerie)
    {
        return new NumeracionDianDTO()
        {
            NumeroResolucion = long.Parse(numeroResolucion),
            FechaInicio = _pointOnSale.DateOfResolution,
            FechaFin = _pointOnSale.Plazo,
            PrefijoNumeracion = numeroSerie,
            ConsecutivoInicial = _pointOnSale.InitInvoiceNumber,
            ConsecutivoFinal = _pointOnSale.FinalInvoiceNumber
        };
    }


    /// <summary>
    /// Obtener referencias de facturas para hacer nota crédito.
    /// </summary>
    /// <returns>Objeto ReferenciasNotas mapeado</returns>
    private ReferenciasNotasDTO GetReferenciasNotasFactureMapped(string facturaAsociada)
    {
        // TODO validar como mapear estos campos según el JSON de NCR.
        return new ReferenciasNotasDTO()
        {
            ReferenciaNota = new ReferenciaNotaDTO()
            {
                CodigoNota = 2,
                FacturaAsociada = facturaAsociada,
                DescripcionNota = "Anulación de factura electrónica"
            }
        };
    }

    /// <summary>
    /// Obtener los medios de pago mapeados a facture.
    /// </summary>
    /// <param name="emitterInvoiceId">Identificador de la tabla EmitterInvoice</param>
    /// <param name="paymentMethods">Metodos de pago que envía AFM.</param>
    /// <returns>Retorna objeto MediosDePagoDTO con los datos organizados</returns>
    private List<MediosDePagoDTO> GetMediosDePagoFactureMapped(long emitterInvoiceId, List<PaymentDTO> paymentMethods)
    {
        List<MediosDePagoDTO> paymentMethodsfacture = new();
        foreach (PaymentDTO paymentMethod in paymentMethods)
        {
            paymentMethodsfacture.Add(new MediosDePagoDTO()
            {
                CodigoMedioPago = GetMedioDePagoFacture(_paymentMethodsService.GetPaymentMethod(emitterInvoiceId, int.Parse(paymentMethod.PagoId))),
                FormaDePago = Constant.PaymentMethodFacture
            });
        }
        return paymentMethodsfacture;
    }

    private string GetMedioDePagoFacture(string strMedioPago)
    {
        // TODO Se debe validar una forma genérica para convertir el medio de pago de acuerdo a la tienda
        if (strMedioPago == null) return strMedioPago;
        string medioPago = strMedioPago.Trim().ToLower();
        switch (medioPago)
        {
            case "credicard":
            case "tarjet d/c":
                return "48";
            case "bonos":
                return "71";
            case "efectivo":
                return "10";
            case "50000":
                return "10";
            case "100000":
                return "10";
            case "e. exacto":
                return "10";
            case "gift card":
                return "71";
            case "g.card emp":
                return "71";
            case "preorden":
                return "34";
            case "debito":
                return "14";
            case "ticket":
                return "ZZZ";
            case "otro":
                return "ZZZ";
            case "cheque":
                return "20";
            case "visa":
                return "48";
            case "mastercard":
                return "48";
            case "amex":
                return "48";
            case "éxito":
                return "48";
            case "compensar":
                return "48";
            case "anticipo":
                return "42";
            default:
                return "1";
        }
    }

    /// <summary>
    /// Método para obtener los datos del emisor.
    /// </summary>
    /// <param name="strEmisorId">Número de NIT de la empresa emisora de la factura</param>
    /// <returns>Retorna objeto EmisorDTO con los datos organizados del emisor</returns>
    private EmisorDTO GetEmisor(string strEmisorId)
    {
        try
        {
            return new EmisorDTO()
            {
                TipoPersona = _pointOnSale.EmitterInVoice.PersonType.ToString(),
                TipoRegimen = _pointOnSale.EmitterInVoice.RegimeType,
                TipoIdentificacion = _pointOnSale.EmitterInVoice.IdentificationType,
                NumeroIdentificacion = _pointOnSale.EmitterInVoice.IdentificationNumber.Split("-")[0],
                DV = _pointOnSale.EmitterInVoice.IdentificationNumber.Split("-").Length > 1 ? _pointOnSale.EmitterInVoice.IdentificationNumber.Split("-")[1] : "",
                RazonSocial = _pointOnSale.EmitterInVoice.BusinessName,
                ObligacionesEmisor = new List<CodigoObligacionDTO>() { new CodigoObligacionDTO() { CodigoObligacion = _pointOnSale.EmitterInVoice.ObligationCode } },
                TributoEmisor = new TributoEmisorDTO() { 
                    CodigoTributo = _pointOnSale.EmitterInVoice.TributeCode, 
                    NombreTributo = _pointOnSale.EmitterInVoice.TributeName},
                Contacto = new ContactoDTO { Email = _pointOnSale.EmitterInVoice.Email}
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Método para obtener el consolidado del valor de impuestos
    /// </summary>
    /// <param name="lstItems">Lista de artículos a facturar</param>
    /// <returns>Retorna objeto ImpuestoDTO con los datos obtenidos y organizados de los impuestos</returns>
    private ImpuestosDTO GetImpuestos(List<ItemDTO> lstItems)
    {
        var lstGroups = lstItems.GroupBy(item => (item.DescripcionIVA,item.TasaIva)).ToList();
        List<ImpuestoDTO> lstImpuesto = new List<ImpuestoDTO>();
        foreach (var groupItems in lstGroups)
        {
            ImpuestoDTO impuesto = new ImpuestoDTO()
            {
                Nombre = groupItems.Key.DescripcionIVA.Contains("IVA") ? "IVA" : "INC",
                // Redondeo = groupItems.Key.TasaIva.ToString(Constant.DefaultRound),
                Valor = groupItems.Sum(item => Math.Round(item.MontoGravado * item.TasaIva)).ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
                Tipo = Constant.GetTaxType()[groupItems.Key.DescripcionIVA],
                Subtotal = GetSubtotalImpuesto(groupItems.GetEnumerator())
            };
            lstImpuesto.Add(impuesto);
        }

        return new ImpuestosDTO()
        {
            Impuesto = lstImpuesto
        };
    }

    /// <summary>
    /// Método para obtener el subtotal por impuesto
    /// </summary>
    /// <param name="e">Lista de objetos ItemDTO organizada con IEnumerator</param>
    /// <returns>Retorna objeto SubtotalDTO con los datos organizados</returns>
    private SubtotalDTO GetSubtotalImpuesto(IEnumerator<ItemDTO> e)
    {
        List<ItemDTO> lstItems = new List<ItemDTO>();
        while (e.MoveNext())
        {
            lstItems.Add(e.Current);
        }

        return new SubtotalDTO()
        {
            ValorBase = lstItems.Sum(item => item.MontoGravado).ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
            Porcentaje = Constant.GetTaxValue()[lstItems[0].TasaIva].ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
            Valor = lstItems.Sum(item => Math.Round(item.MontoGravado * item.TasaIva)).ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
            CodigoUnidMedidaBase = Constant.UnitOfMeasurement
        };
    }

    /// <summary>
    /// Método para obtener el valor total de la factura
    /// </summary>
    /// <param name="lstItems">Lista de objetos ItemDTO con los datos de los artículos a facturar.</param>
    /// <param name="chargesAndTips">Recargos y propinas.</param>
    /// <param name="advance">Anticipos.</param>
    /// <returns>Retorna objeto TotalesDTO con los datos consolidados.</returns>
    private TotalesDTO GetTotal(List<ItemDTO> lstItems, float chargesAndTips, float advance = 0.0f)
    {
        var discounts = lstItems.Sum(item => Math.Abs(item.Descuento));
        return new TotalesDTO()
        {
            Bruto = lstItems.Sum(item => item.MontoGravado).ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
            BaseImponible = lstItems.Sum(item => item.MontoGravado).ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
            BrutoMasImpuestos = lstItems.Sum(item => item.Total).ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
            Impuestos = lstItems.Sum(item => Math.Round(item.MontoGravado * item.TasaIva)).ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
            Anticipo = advance.ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
            Cargos = chargesAndTips.ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
            General = (lstItems.Sum(item => item.Total) + chargesAndTips)
            .ToString(Constant.DefaultRound, CultureInfo.InvariantCulture),
            TotalDescuentosLineas = discounts.ToString(Constant.DefaultRound, CultureInfo.InvariantCulture)
        };
    }
}