using NCRFiscalManager.Core.Entities;

namespace NCRFiscalManager.Core.Interfaces.Repositories
{
    public interface ITechOperatorEmitterInVoiceRepository : IRepositoryAsync<TechOperatorEmitterInVoice>
    {
        public TechOperatorEmitterInVoice GetByEmitterInvoiceNit(string nit);

    }
}
