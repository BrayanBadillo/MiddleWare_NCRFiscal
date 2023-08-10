namespace NCRFiscalManager.Core.Interfaces;

public interface IBasicAuthUserService
{
    public Task<bool> AuthenticateAsync(string username, string password);
}