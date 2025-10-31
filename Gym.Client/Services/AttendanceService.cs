using Blazored.LocalStorage;
using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using System.Net.Http.Headers;

namespace Gym.Client.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly HttpClient _http;
        public AttendanceService(HttpClient http)
        {
            _http = http;
          
        }
        public async Task<List<AttendanceLogDTO>> GetCurrentAttendanceAsync()
        {  
            var response = await _http.GetFromJsonAsync<List<AttendanceLogDTO>>("api/Attendance/today")
                            ?? new List<AttendanceLogDTO>();
            return response;
        }
    }
}
