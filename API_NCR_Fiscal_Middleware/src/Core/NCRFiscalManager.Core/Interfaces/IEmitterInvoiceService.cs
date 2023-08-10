using NCRFiscalManager.Core.DTO.Entities;

namespace NCRFiscalManager.Core.Interfaces;

public interface IEmitterInvoiceService
{
    EmitterInvoiceDTO GetZeroPriceItemConfigurationOfEmitterInvoice(string identificationNumber);

}
