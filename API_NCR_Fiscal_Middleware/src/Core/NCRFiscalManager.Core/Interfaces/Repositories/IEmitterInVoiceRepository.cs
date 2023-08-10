using NCRFiscalManager.Core.Entities;

namespace NCRFiscalManager.Core.Interfaces.Repositories;

public interface IEmitterInVoiceRepository : IRepositoryAsync<EmitterInVoice>
{
    public EmitterInVoice GetEmitterInvoiceByIdentificationNumber(string identificationNumber);
}
