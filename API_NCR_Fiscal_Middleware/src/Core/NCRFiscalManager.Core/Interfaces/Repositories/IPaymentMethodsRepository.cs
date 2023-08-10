using NCRFiscalManager.Core.Entities;

namespace NCRFiscalManager.Core.Interfaces.Repositories
{
    public interface IPaymentMethodsRepository : IRepositoryAsync<PaymentMethods>
    {
        public PaymentMethods GetPaymetMethodByUserNumber(long emitterInvoiceId, int userNumber);

    }
}
