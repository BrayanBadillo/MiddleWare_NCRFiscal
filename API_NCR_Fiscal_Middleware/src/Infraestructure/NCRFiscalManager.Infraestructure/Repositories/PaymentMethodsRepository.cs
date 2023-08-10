using Serilog;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces.Repositories;
using NCRFiscalManager.Infraestructure.Repositories.Context;

namespace NCRFiscalManager.Infraestructure.Repositories
{
    public class PaymentMethodsRepository : RepositoryAsync<PaymentMethods>, IPaymentMethodsRepository
    {
        private readonly NCRFiscalContext _ncrFiscalContext;

        public PaymentMethodsRepository(NCRFiscalContext ncrFiscalcontext) : base(ncrFiscalcontext)
        {
            _ncrFiscalContext = ncrFiscalcontext;
        }

        /// <summary>
        /// Método para consultar el método de pago por el emisor de la factura y el número del método de pago.
        /// </summary>
            /// <param name="emitterInvoiceId">Identificador de la tabla EmitterInvoice</param>
        /// <param name="userNumber">Identificador del tipo de pago</param>
        /// <returns>Retorna entity PaymentMethods</returns>
        public PaymentMethods GetPaymetMethodByUserNumber(long emitterInvoiceId, int userNumber)
        {
            try
            {
                PaymentMethods paymentMethods = new PaymentMethods();
                var lstPaymentMethods = _ncrFiscalContext.PaymentMethods
                    .Where(payMethode =>
                        payMethode.UserNumber == userNumber && payMethode.EmitterInVoiceId == emitterInvoiceId)
                    .ToList();

                if (lstPaymentMethods.Count > 0)
                {
                    paymentMethods = lstPaymentMethods[0];
                }

                return paymentMethods;
            }
            catch (Exception e)
            {
                Log.Error($"Error GetPaymetMethodeByUserNumber {e}");
                return new PaymentMethods();
            }
        }
    }
}
