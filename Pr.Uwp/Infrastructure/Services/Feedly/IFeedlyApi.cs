using System.Threading.Tasks;
using Refit;

namespace Pr.Uwp.Infrastructure.Services.Feedly
{
    public class TokenResponse
    {
        [AliasAs("expires_in")] public string ExpiresIn { get; set; }
        [AliasAs("token_type")] public string TokenType { get; set; }
        [AliasAs("refresh_token")] public string RefreshToken { get; set; }
        [AliasAs("id")] public string Id { get; set; }
        [AliasAs("state")] public string State { get; set; }
        [AliasAs("plan")] public string Plan { get; set; }
        [AliasAs("access_token")] public string AccessToken { get; set; }
    }

    public class TokenRequest
    {
        [AliasAs("code")] public string Code { get; set; }
        [AliasAs("client_id")] public string ClientId { get; set; }
        [AliasAs("client_secret")] public string ClientSecret { get; set; }
        [AliasAs("redirect_uri")] public string RedirectUri { get; set; }
        [AliasAs("state")] public string State { get; set; }
        [AliasAs("grant_type")] public string GrantType { get; set; }
        [AliasAs("refresh_token")] public string RefreshToken { get; set; }
    }

    public interface IFeedlyApi
    {
        [Get("/v3/auth/token")]
        Task<TokenResponse> Token([Body] TokenRequest request);
    }
}
