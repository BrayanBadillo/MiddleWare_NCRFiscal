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
    public class TechOperatorRepository : RepositoryAsync<TechOperator>, ITechOperatorRepository
    {
        private readonly NCRFiscalContext _ncrFiscalContext;

        public TechOperatorRepository(NCRFiscalContext ncrFiscalContext) : base(ncrFiscalContext)
        {
        }
    }
}
