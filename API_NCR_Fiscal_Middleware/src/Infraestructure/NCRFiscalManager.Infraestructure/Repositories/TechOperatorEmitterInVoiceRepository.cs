using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces.Repositories;
using NCRFiscalManager.Infraestructure.Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NCRFiscalManager.Infraestructure.Repositories
{
    public class TechOperatorEmitterInVoiceRepository : RepositoryAsync<TechOperatorEmitterInVoice>, ITechOperatorEmitterInVoiceRepository
    {
        private readonly NCRFiscalContext _ncrFiscalContext;

        public TechOperatorEmitterInVoiceRepository(NCRFiscalContext ncrFiscalContext) : base(ncrFiscalContext)
        {
            _ncrFiscalContext =  ncrFiscalContext;
        }

        /// <summary>
        /// Método para consultar los datos de conexión por cliente, emisor de factura de acuerdo al operador tecnológico asociado.
        /// </summary>
        /// <param name="nit">Número de identificación del emisor de la factura</param>
        /// <returns>Retorna entity TechOperatorEmitterInVoice con los datos obtenidos de conexión</returns>
        public TechOperatorEmitterInVoice GetByEmitterInvoiceNit(string nit)
        {
            try
            {
                return _ncrFiscalContext.TechOperatorEmitterInVoice
                    .Include(optec => optec.EmitterInVoice)
                    .Where(optec => optec.EmitterInVoice.IdentificationNumber == nit)
                    .ToList()
                    .FirstOrDefault(new TechOperatorEmitterInVoice());
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
