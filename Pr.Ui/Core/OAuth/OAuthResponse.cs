namespace Pr.Ui.Core.OAuth
{
    public class OAuthResponse<T>
    {
        public T Result { get; set; }
        public string Error { get; set; }
    }

    public static class OAuthResponse
    {
        public static OAuthResponse<T> Success<T>(T result)
        {
            return new OAuthResponse<T> { Result = result };
        }

        public static OAuthResponse<T> Failed<T>(string error)
        {
            return new OAuthResponse<T> { Error = error };
        }
    }
}