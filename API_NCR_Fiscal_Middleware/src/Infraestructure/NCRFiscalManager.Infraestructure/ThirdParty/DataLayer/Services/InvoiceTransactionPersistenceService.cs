using NCRFiscalManager.Core.Constants;
using Serilog;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces;
using NCRFiscalManager.Core.Interfaces.Repositories;
using NCRFiscalManager.Infraestructure.Repositories.Context;

namespace NCRFiscalManager.Infraestructure.ThirdParty.DataLayer.Services;

public class InvoiceTransactionPersistenceService : IInvoiceTransactionPersistenceService
{
    private readonly NCRFiscalContext _context;
    private readonly IPointOnSaleRepository _pointOnSaleRepository;
    private readonly IInvoiceTransactionRepository _invoiceTransactionRepository;

    public InvoiceTransactionPersistenceService(NCRFiscalContext context,
        IPointOnSaleRepository pointOnSaleRepository, IInvoiceTransactionRepository invoiceTransactionRepository)
    {
        _context = context;
        _pointOnSaleRepository = pointOnSaleRepository;
        _invoiceTransactionRepository = invoiceTransactionRepository;
    }

    /// <summary>
    /// Método para guardar los datos de la transacción al generar una factura electrónica.
    /// </summary>
    /// <param name="invoiceTransaction">Objeto InvoiceTransaction con los datos a guardar</param>
    /// <param name="storeKey">Identificador del punto de venta</param>
    /// <returns>Retorna bandera indicando resultado de la operación</returns>
    public bool SaveInvoiceTransaction(InvoiceTransaction invoiceTransaction, string storeKey, string ResolutionSerial)
    {
        try
        {
            PointOnSale pointOnSale = _pointOnSaleRepository.GetStoreByKey(storeKey, ResolutionSerial);

            if (invoiceTransaction.CUFE != Constant.CufeNoGenerado && ValidateInvoiceTransaction(pointOnSale.Id, invoiceTransaction.CUFE))
            {
                invoiceTransaction.PointOnSaleId = pointOnSale.Id;
                invoiceTransaction.CreatedAt = DateTime.Now;
                _context.InvoiceTransactions.Add(invoiceTransaction);
                _context.SaveChanges();
            }

            if (invoiceTransaction.CUFE == Constant.CufeNoGenerado && ValidateInvoiceTransaction(pointOnSale.Id, invoiceTransaction.CUFE))
            {
                invoiceTransaction.PointOnSaleId = pointOnSale.Id;
                invoiceTransaction.CreatedAt = DateTime.Now;
                _context.InvoiceTransactions.Add(invoiceTransaction);
                _context.SaveChanges();
            }

            return true;
        }
        catch (Exception e)
        {
            Log.Error($"Error SaveInvoiceTransaction {e}");
            return false;
        }
    }

    /// <summary>
    /// Método para validar si el registro ya existe en la base de datos a través del CUFE
    /// </summary>
    /// <param name="pointOnSaleId">Identificador del punto de venta</param>
    /// <param name="cufe">CUFE generado desde la DIAN</param>
    /// <returns>Retorna tipo de dato bool con el resultado de la validación. True no existe registro previo, False en caso contrario</returns>
    private bool ValidateInvoiceTransaction(long pointOnSaleId, string cufe)
    {
        try
        {
            InvoiceTransaction invoiceTransaction = GetInvoiceTransactionByCufeAndPointOfSaleId(pointOnSaleId, cufe);
            return invoiceTransaction == null;
        }
        catch (Exception e)
        {
            Log.Error($"Error ValidateInvoiceTransaction {e}");
            return false;
        }
    }

    /// <summary>
    /// Método para consultar registro de una transacción enviada al operador tecnológico.
    /// </summary>
    /// <param name="pointOnSaleId">Identificador del punto de venta</param>
    /// <param name="cufe">CUFE generado desde la DIAN</param>
    /// <returns>Retorna entity InvoiceTransaction con los datos obtenidos</returns>
    public InvoiceTransaction GetInvoiceTransactionByCufeAndPointOfSaleId(long pointOnSaleId, string cufe)
    {
        try
        {
            return _invoiceTransactionRepository.GetInvoiceTransactionByCufeAndPointOfSaleId(pointOnSaleId, cufe);
        }
        catch (Exception e)
        {
            Log.Error($"Error GetInvoiceTransactionByCufe {e}");
            return null;
        }
    }

    /// <summary>
    /// Método para consultar registro de una transacción enviada al operador tecnológico.
    /// </summary>
    /// <param name="cufe">CUFE generado desde la DIAN</param>
    /// <returns>Retorna entity InvoiceTransaction con los datos obtenidos</returns>
    public InvoiceTransaction GetInvoiceTransactionByCufe(string cufe)
    {
        try
        {
            return _invoiceTransactionRepository.GetInvoiceTransactionByCufe(cufe);
        }
        catch (Exception e)
        {
            Log.Error($"Error GetInvoiceTransactionByCufe {e}");
            return null;
        }
    }

    /// <summary>
    /// Método para consultar
    /// </summary>
    /// <param name="storeKey">Identificador Aloha del punto de venta</param>
    /// <param name="correlativoFiscal">Correlativo fiscal asociado, número de facturación</param>
    /// <param name="tipoDocumentoFiscal">Tipo de documento fiscal. Factura o Nota Crédito</param>
    /// <returns>Retorna entity InvoiceTransaction con los datos obtenidos</returns>
    public InvoiceTransaction GetInvoiceTransactionByStoreKeyAndCorrelative(string storeKey, string correlativoFiscal, int tipoDocumentoFiscal, string ResolutionSerial)
    {
        try
        {
            PointOnSale pointOnSale = _pointOnSaleRepository.GetStoreByKey(storeKey, ResolutionSerial);
            return _invoiceTransactionRepository.GetInvoiceTransactionByPointOnSaleAndCorrelative(pointOnSale.Id, correlativoFiscal, tipoDocumentoFiscal);
        }
        catch (Exception e)
        {
            Log.Error($"Error GetInvoiceTransactionByStoreKeyAndCorrelative {e}");
            return null;
        }
    }
}