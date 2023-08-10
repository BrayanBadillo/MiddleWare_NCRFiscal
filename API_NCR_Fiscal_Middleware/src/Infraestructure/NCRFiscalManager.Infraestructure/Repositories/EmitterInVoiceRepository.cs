using Microsoft.EntityFrameworkCore;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces.Repositories;
using NCRFiscalManager.Infraestructure.Repositories.Context;

namespace NCRFiscalManager.Infraestructure.Repositories
{
    public class EmitterInVoiceRepository : RepositoryAsync<EmitterInVoice>, IEmitterInVoiceRepository
    {
        private readonly NCRFiscalContext _ncrFiscalContext;

        public EmitterInVoiceRepository(NCRFiscalContext ncrFiscalContext) : base(ncrFiscalContext)
        {
            _ncrFiscalContext =  ncrFiscalContext;
        }


        /// <summary>
        /// Buscar el emisor por el número de identificación.
        /// </summary>
        /// <param name="identificationNumber">Identificación del emisor (NIT).</param>
        /// <returns>Objeto EmitterInvoice.</returns>
        public EmitterInVoice GetEmitterInvoiceByIdentificationNumber(string identificationNumber)
        {
            var emitter = _ncrFiscalContext.EmitterInVoice
                .FirstOrDefault(x => x.IdentificationNumber == identificationNumber);

            return emitter;
        }
    }
}
