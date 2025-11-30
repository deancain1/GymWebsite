using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Gym.Client.Components.Pages.Admin_Pages
{
    public class AttendanceBase : ComponentBase
    {
        [Inject] IAttendanceService _attendanceService { get; set; } = default!;
        [Inject] IJSRuntime JS { get; set; } = default!;
        protected List<AttendanceLogDTO> Attendances { get; set; } = new();
     
        protected string searchText = "";
        protected DateTime? selectedDate { get; set; } = null;
        protected TimeSpan? selectedTime { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            await LoadAttendance();
        }
        protected async Task LoadAttendance()
        {
            Attendances = await _attendanceService.GetAllAttendanceAsync();
        }
        protected List<AttendanceLogDTO> filteredAttendances => Attendances
        .Where(a =>
            (string.IsNullOrWhiteSpace(searchText) ||
             a.FullName.Contains(searchText, StringComparison.OrdinalIgnoreCase))

            &&

            (!selectedDate.HasValue ||
             a.ScanTime.Date == selectedDate.Value.Date)

            &&

            (!selectedTime.HasValue ||
                (a.ScanTime.Hour == selectedTime.Value.Hours &&
                 a.ScanTime.ToString("tt") == DateTime.Today.Add(selectedTime.Value).ToString("tt"))
            )
        )
        .ToList();
        protected async Task PrintPDF()
        {
            await JS.InvokeVoidAsync("printAttendance");
        }
    }
}
