namespace Gym.Client.DTOs
{
    public class MembershipDTO
    {
        public int MemberID { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Plan { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime AppliedDate { get; set; } = DateTime.Now;
        public int DurationMonths { get; set; } = 1;
        public DateTime ExpirationDate { get; set; }
        public string QRCode { get; set; } = string.Empty;
    }
}
