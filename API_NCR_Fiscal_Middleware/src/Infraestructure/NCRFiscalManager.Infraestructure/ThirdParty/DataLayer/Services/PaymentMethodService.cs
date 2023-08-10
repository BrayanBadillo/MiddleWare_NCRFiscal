using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces;
using NCRFiscalManager.Core.Interfaces.Repositories;

namespace NCRFiscalManager.Infraestructure.ThirdParty.DataLayer.Services;

public class PaymentMethodService : IPaymentMethodsService
{
    public readonly IPaymentMethodsRepository _PaymentMethodsRepository;

    public PaymentMethodService(IPaymentMethodsRepository paymentMethodsRepository)
    {
        _PaymentMethodsRepository = paymentMethodsRepository;
    }
    
    /// <summary>
    /// Método para obtener el método de pago
    /// </summary>
    /// <param name="emitterInvoiceId">Identificador de la tabla EmitterInvoice</param>
    /// <param name="userNumber">Identificador del tipo de pago</param>
    /// <returns>Retorna el método de pago obtenido</returns>
    public string GetPaymentMethod(long emitterInvoiceId, int userNumber)
    {
        try
        {
            PaymentMethods paymentMethods = _PaymentMethodsRepository.GetPaymetMethodByUserNumber(emitterInvoiceId, userNumber);
            return paymentMethods.PaymentType;
        }
        catch (Exception e)
        {
            Serilog.Log.Error($"Error GetPaymentMethod {e}");
            return null;
        }
    }
        
}