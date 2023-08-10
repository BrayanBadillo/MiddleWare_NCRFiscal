using Microsoft.EntityFrameworkCore;
using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces.Repositories;
using NCRFiscalManager.Infraestructure.Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCRFiscalManager.Infraestructure.Repositories
{
    public class PointOnSaleRepository : RepositoryAsync<PointOnSale>, IPointOnSaleRepository
    {
        private readonly NCRFiscalContext _ncrFiscalContext;

        public PointOnSaleRepository(NCRFiscalContext ncrFiscalContext) : base(ncrFiscalContext)
        {
            _ncrFiscalContext = ncrFiscalContext;
        }

        /// <summary>
        /// Método para consultar los datos de un punto de venta por el storekey.
        /// </summary>
        /// <param name="storeKey">Identificador de Aloha del punto de venta</param>
        /// <returns>Retorna entity PointOnSale con los datos obtenidos</returns>
        /// <exception cref="Exception"></exception>
        public PointOnSale GetStoreByKey(string storeKey, string ResolutionSerial)
        {
            try
            {
                return _ncrFiscalContext.PointOnSales
                       .Include(p => p.EmitterInVoice)
                       .Where(x => x.StoreKey == storeKey && x.ResolutionSerial == ResolutionSerial).FirstOrDefault();
            }
            catch (Exception e)
            {

                throw new Exception($"An error has ocurred during consult PointOnSale table: {e.Message}");
            }
        }
    }
}
