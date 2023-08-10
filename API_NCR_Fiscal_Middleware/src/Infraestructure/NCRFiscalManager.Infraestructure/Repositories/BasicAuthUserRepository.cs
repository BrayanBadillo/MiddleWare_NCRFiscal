using NCRFiscalManager.Core.Entities;
using NCRFiscalManager.Core.Interfaces.Repositories;
using NCRFiscalManager.Infraestructure.Repositories.Context;

namespace NCRFiscalManager.Infraestructure.Repositories;

public class BasicAuthUserRepository : RepositoryAsync<BasicAuthUser>, IBasicAuthUserRepository
{
    private readonly NCRFiscalContext _ncrFiscalContext;

    public BasicAuthUserRepository(NCRFiscalContext ncrFiscalContext) : base(ncrFiscalContext)
    {
        
    }
}