using NCRFiscalManager.Core.Entities;

namespace NCRFiscalManager.Core.Interfaces;

public interface IInvoiceTransactionPersistenceService
{
    public bool SaveInvoiceTransaction(InvoiceTransaction invoiceTransaction, string storeKey, string ResolutionSerial);
    public InvoiceTransaction GetInvoiceTransactionByCufeAndPointOfSaleId(long pointOnSaleId, string cufe);
    public InvoiceTransaction GetInvoiceTransactionByCufe(string cufe);
    public InvoiceTransaction GetInvoiceTransactionByStoreKeyAndCorrelative(string storeKey, string correlativoFiscal, int tipoDocumentoFiscal, string ResolutionSerial);
}