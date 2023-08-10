namespace NCRFiscalManager.Core.Interfaces;

public interface IPaymentMethodsService
{
    public string GetPaymentMethod(long emitterInvoiceId, int userNumber);

}