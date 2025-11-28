namespace Gym.Client.DTOs
{
    public class TokenResponse
    {
        public string Token { get; set; }          
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
