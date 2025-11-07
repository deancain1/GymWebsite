namespace Gym.Client.DTOs
{
    public class AttendanceLogDTO
    {
        public int AttendanceID { get; set; }
        public int MemberID { get; set; }
        public string FullName { get; set; }
        public string UserID { get; set; }
        public DateTime ScanTime { get; set; }
        public string Status { get; set; }
      



    }
}
