using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using NCRFiscalManager.Core.Interfaces;

namespace NCRFiscalManager.API.Middleware;

public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IBasicAuthUserService _basicAuthUserService;

	public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
		ILoggerFactory logger,
		UrlEncoder encoder,
		ISystemClock clock,
		IBasicAuthUserService basicAuthUserService) : base(options, logger, encoder, clock)
	{
		_basicAuthUserService = basicAuthUserService ?? throw new ArgumentNullException(nameof(basicAuthUserService));
	}

	/// <summary>
	/// Middleware de autenticación de la API con BasicAuth.
	/// Verifica que el header de autenticación tenga credenciales correctas.
	/// </summary>
	/// <returns>token de acceso o código 401 Unauthorized</returns>
	protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		if (!Request.Headers.ContainsKey("Authorization"))
			return AuthenticateResult.Fail("Invalid authentication header");

		bool authResult = false;
		string username;
		string password;
        try
		{
			var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
			var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
			var credentials = Encoding.UTF8.GetString(credentialBytes).Split( ":", 2);
			username = credentials[0];
			password = credentials[1];
			Console.WriteLine($"User: {username}, Password: {password}");

			authResult = await _basicAuthUserService.AuthenticateAsync(username, password);
			if (authResult != null)
			{
				Console.WriteLine($"authResult: {authResult}");
			}
		}
		catch
		{
			return AuthenticateResult.Fail("Invalid credentials");
        }

		if(!authResult)
			return AuthenticateResult.Fail("Invalid username or password");

        var claim = new Claim[]{ new Claim(ClaimTypes.NameIdentifier, username) };
		var identity = new ClaimsIdentity(claim, Scheme.Name);
		var principal = new ClaimsPrincipal(identity);
		var ticket = new AuthenticationTicket(principal, Scheme.Name);

		return AuthenticateResult.Success(ticket);
	}

	/// <summary>
	/// Método para sobrescribir la respuesta cuando se devuelve un código 401
	/// </summary>
	/// <param name="properties"></param>
	/// <returns></returns>
	protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
	{
		string result;
		Response.StatusCode = 401;
        result = JsonSerializer.Serialize(new { error = "Invalid authentication credentials!" });
        Context.Response.ContentType = "application/json";
        await Context.Response.WriteAsync(result);
    }
}