using NCRFiscalManager.Core.Interfaces;
using NCRFiscalManager.Core.Interfaces.Repositories;

namespace NCRFiscalManager.Core.Services;

public class BasicAuthUserService : IBasicAuthUserService
{
    private readonly IBasicAuthUserRepository _basicAuthUserRepository;

    public BasicAuthUserService(IBasicAuthUserRepository basicAuthUserRepository)
    {
        _basicAuthUserRepository = basicAuthUserRepository ?? throw new ArgumentNullException(nameof(basicAuthUserRepository));
    }
    public async Task<bool> AuthenticateAsync(string username, string password)
    {
        try
        {
            bool authenticated = false;
            if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var user = await _basicAuthUserRepository.GetByFilter(u => u.Username == username && u.Password == password);

                if (user.Count() > 0) return true;
            }
            return authenticated;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}