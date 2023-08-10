using Microsoft.EntityFrameworkCore;
using NCRFiscalManager.Core.Constants;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces.Repositories;
using NCRFiscalManager.Infraestructure.Repositories.Context;
using Serilog;

namespace NCRFiscalManager.Infraestructure.Repositories
{
    public class InvoiceTransactionRepository : RepositoryAsync<InvoiceTransaction>, IInvoiceTransactionRepository
    {
        private readonly NCRFiscalContext _ncrFiscalContext;
        public InvoiceTransactionRepository(NCRFiscalContext ncrFiscalContext) : base(ncrFiscalContext)
        {
            _ncrFiscalContext = ncrFiscalContext;
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
                InvoiceTransaction invoiceTransaction = null;
                var lstInvoiceTransactions =  _ncrFiscalContext.InvoiceTransactions
                    .Include(invoice => invoice.PointOnSale)
                    .Where(transaction => transaction.PointOnSaleId == pointOnSaleId && transaction.CUFE == cufe)
                    .ToList();

                if (lstInvoiceTransactions.Count > 0)
                {
                    invoiceTransaction = lstInvoiceTransactions[0];
                }

                return invoiceTransaction;
            }
            catch (Exception e)
            {
                Serilog.Log.Error($"Error GetInvoiceTransactionByCufe {e}");
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
                InvoiceTransaction invoiceTransaction = null;
                var lstInvoiceTransactions = _ncrFiscalContext.InvoiceTransactions
                    .Include(invoice => invoice.PointOnSale)
                    .Where(transaction => transaction.CUFE == cufe)
                    .ToList();

                if (lstInvoiceTransactions.Count > 0)
                {
                    invoiceTransaction = lstInvoiceTransactions[0];
                }

                return invoiceTransaction;
            }
            catch (Exception e)
            {
                Serilog.Log.Error($"Error GetInvoiceTransactionByCufe {e}");
                return null;
            }
        }

        /// <summary>
        /// Método para obtener un registro de InvoiceTransaction
        /// </summary>
        /// <param name="pointOnSaleId">Identificador del punto de venta</param>
        /// <param name="correlativoFiscal">Correlativo fiscal asociado, número de facturación</param>
        /// <param name="tipoDocumentoFiscal">Tipo de documento fiscal. Factura o Nota Crédito</param>
        /// <returns>Retorna entity InvoiceTransaction con los datos obtenidos</returns>
        public InvoiceTransaction GetInvoiceTransactionByPointOnSaleAndCorrelative(long pointOnSaleId,
            string correlativoFiscal, int tipoDocumentoFiscal)
        {
            try
            {
                InvoiceTransaction invoiceTransaction = null;
                var lstInvoiceTransactions =  _ncrFiscalContext.InvoiceTransactions
                    .Include(invoice => invoice.PointOnSale)
                    .Where(transaction => transaction.PointOnSaleId == pointOnSaleId && transaction.FiscalCorrelative == correlativoFiscal 
                        && transaction.CUFE != Constant.CufeNoGenerado && transaction.TipoDocumentoFiscal == tipoDocumentoFiscal)
                    .ToList();

                if (lstInvoiceTransactions.Count > 0)
                {
                    invoiceTransaction = lstInvoiceTransactions[0];
                }

                return invoiceTransaction;
            }
            catch (Exception e)
            {
                Log.Error($"Error GetInvoiceTransactionByPointOnSaleAndCorrelative {e}");
                return null;
            }
        }
    }
}
