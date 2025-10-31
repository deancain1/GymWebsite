using Gym.Client.DTOs;

namespace Gym.Client.Interfaces
{
    public interface IAttendanceService 
    {
        Task<List<AttendanceLogDTO>> GetCurrentAttendanceAsync();
    }
}
