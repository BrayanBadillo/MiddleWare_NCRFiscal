using NCRFiscalManager.Core.Entities;

namespace NCRFiscalManager.Core.Interfaces.Repositories
{
    public interface IInvoiceTransactionRepository : IRepositoryAsync<InvoiceTransaction>
    {
        public InvoiceTransaction GetInvoiceTransactionByCufeAndPointOfSaleId(long pointOnSaleId, string cufe);
        public InvoiceTransaction GetInvoiceTransactionByCufe(string cufe);
        public InvoiceTransaction GetInvoiceTransactionByPointOnSaleAndCorrelative(long pointOnSaleId, string correlativoFiscal, int tipoDocumentoFiscal);
    }
}
