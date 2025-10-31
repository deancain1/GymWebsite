namespace Gym.Client.DTOs
{
    public class UserDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; } = new();
    }
}
