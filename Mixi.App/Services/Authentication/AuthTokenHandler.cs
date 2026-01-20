using System.Net.Http.Headers;

namespace Mixi.App.Services.Authentication;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly ISecureStorage _secureStorage;

    public AuthTokenHandler(ISecureStorage secureStorage)
    {
        _secureStorage = secureStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _secureStorage.GetAsync("auth_token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}