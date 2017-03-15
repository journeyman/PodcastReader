using Pr.Ui.Core.OAuth;

namespace Pr.Ui.Core.Feedly
{
    public class FeedlySandboxSettings
    {
        public static Settings OAuthSettings =>
            new Settings
            {
                ClientId = "sandbox",
                RedirectUri = "OE12J47X2W5PEF7CKPGZ",
            };

        public static string Endpoint => "http://sandbox.feedly.com";
    }
}
