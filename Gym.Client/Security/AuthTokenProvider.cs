namespace Gym.Client.Security
{
    public class AuthTokenProvider
    {
        public string AccessToken { get; private set; } = string.Empty;
        public string RefreshToken { get; private set; } = string.Empty;
        public DateTime AccessTokenExpiry { get; private set; } = DateTime.MinValue;

        public void SetTokens(string accessToken, string refreshToken, DateTime expiry)
        {
            AccessToken = accessToken ?? string.Empty;
            RefreshToken = refreshToken ?? string.Empty;
            AccessTokenExpiry = expiry;
        }

        public void ClearTokens()
        {
            AccessToken = string.Empty;
            RefreshToken = string.Empty;
            AccessTokenExpiry = DateTime.MinValue;
        }

        public bool IsAccessTokenExpiringSoon(int secondsThreshold = 60)
        {
            return DateTime.Now.AddSeconds(secondsThreshold) >= AccessTokenExpiry;
        }
    }
}

