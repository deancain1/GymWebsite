using Gym.Client.DTOs;

namespace Gym.Client.Interfaces
{
    public interface IAttendanceService
    {
        Task<List<AttendanceLogDTO>> GetAllAttendanceAsync();
        Task<List<AttendanceLogDTO>> GetCurrentAttendanceAsync();
        Task<List<AttendanceLogDTO>> GetAttendanceByTokenAsync();
    }
}
