namespace Gym.Client.DTOs
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public byte[] ProfilePicture { get; set; } = null!;
        public string? ProfileImageBase64 { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
