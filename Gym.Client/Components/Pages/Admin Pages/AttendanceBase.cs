using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Gym.Client.Components.Pages.Admin_Pages
{
    public class AttendanceBase : ComponentBase
    {
        [Inject] IAttendanceService _attendanceService { get; set; } = default!;
        public List<AttendanceLogDTO> Attendances { get; set; } = new();
        public string searchText { get; set; } = "";
        public DateTime? selectedDate { get; set; } = null;
        protected override async Task OnInitializedAsync()
        {
            await LoadAttendance();
        }
        private async Task LoadAttendance()
        {
            Attendances = await _attendanceService.GetAllAttendanceAsync();
        }
        protected List<AttendanceLogDTO> filteredAttendances => Attendances
      .Where(a =>
          (string.IsNullOrWhiteSpace(searchText) |
           a.FullName.Contains(searchText, StringComparison.OrdinalIgnoreCase)) &&

          (!selectedDate.HasValue ||
           a.ScanTime.Date == selectedDate.Value.Date)
      )
      .ToList();

    }
}
