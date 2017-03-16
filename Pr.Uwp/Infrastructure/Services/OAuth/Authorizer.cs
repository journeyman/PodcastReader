using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Pr.Core.Utils;

namespace Pr.Uwp.Infrastructure.Services.OAuth
{
    public class Authorizer
    {
        private const string REDIRECT = "http://localhost";
        private const string CODE_PATH = "/v3/auth/auth";
        private const string TOKEN_PATH = "/v3/auth/token";

        private readonly string _endpoint;
        private readonly Settings _settings;
        private readonly IBrowserPresenter _browser;

        public Authorizer(string endpoint, Settings settings, IBrowserPresenter browser)
        {
            _endpoint = endpoint;
            _settings = settings;
            _browser = browser;
        }

        public async Task<OAuthResponse<string>> GetCode(string scope)
        {
            var requestParams = new QueryParams
            {
                { "response_type" , "code" },
                { "client_id", _settings.ClientId },
                { "redirect_uri", _settings.RedirectUri },
                { "scope", scope },
            };

            var url = $"{_endpoint}{CODE_PATH}{requestParams.ToQueryString()}";

            var redirect = Observable
                .FromEvent<Uri>(handler => _browser.Navigating += handler, handler => _browser.Navigating -= handler)
                .FirstAsync(uri => uri.OriginalString.StartsWith(_settings.RedirectUri, StringComparison.OrdinalIgnoreCase));
         
            await _browser.Navigate(new Uri(url, UriKind.Absolute));

            var lastRedirect = await redirect;
            var query = lastRedirect.ParseQueryString();
            var code = query.TryGet("code");
            if (string.IsNullOrEmpty(code))
            {
                return OAuthResponse.Failed<string>(query.TryGet("error") ?? lastRedirect.OriginalString);
            }
            else
            {
                return OAuthResponse.Success(code);
            }
        }
    }
}