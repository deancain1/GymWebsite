using Gym.Client.DTOs;
using Gym.Client.Interfaces;
using Gym.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Reflection.Emit;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gym.Client.Components.Pages.Admin_Pages
{
    public class AdminHomeBase : ComponentBase
    {
        [Inject] protected IMembershipService _membershipService { get; set; } = default!;
        [Inject] protected IUserService _userService { get; set; } = default!;
        [Inject] protected IAttendanceService _attendanceService { get; set; } = default!;
        [Inject] protected  HttpClient _http { get; set; } = default!;

        protected int TotalUsers;
        protected int TotalMemberships;
        protected int TotalAdmins;
        protected int Attendees;

        protected int _index = -1;
        protected AxisChartOptions _axisChartOptions = new AxisChartOptions();
        protected List<ChartSeries>? _barSeries;
        protected string[] _xAxisLabels = Array.Empty<string>();

        protected string[] labels = Array.Empty<string>();
        protected double[] data = Array.Empty<double>();


        protected SortMode _sortMode = SortMode.Multiple;
        protected List<AttendanceLogDTO> attendees = new();

        protected DateTime CurrentMonth = DateTime.Today;
        protected DateTime SelectedDate = DateTime.Today;
        protected string[] DayNames = new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        protected List<DateTime> CalendarDates = new();

        protected override async Task OnInitializedAsync()
        {
            
            TotalMemberships = await _membershipService.GetTotalMembershipsAsync();
            TotalUsers = await _userService.GetTotalUserAsync();
            TotalAdmins = await _userService.GetTotalAdminsAsync();
            attendees = await _attendanceService.GetCurrentAttendanceAsync(); 
            Attendees = attendees.Count(a => a.ScanTime.Date == DateTime.Today);

         
            var monthlyCounts = await _membershipService.GetMembershipsPerMonthAsync();
            var monthlyExpired = await _membershipService.GetExpiredMembershipsPerMonthAsync();

            if (monthlyCounts is not null && monthlyExpired is not null)
            {
                _xAxisLabels = monthlyCounts
                    .Select(x => x.Month)
                    .Union(monthlyExpired.Select(x => x.Month))
                    .Distinct()
                    .OrderBy(x => DateTime.ParseExact(x, "MMM yyyy", null))
                    .ToArray();

                var activeData = _xAxisLabels.Select(m => monthlyCounts.FirstOrDefault(x => x.Month == m)?.Count ?? 0).Select(c => (double)c).ToArray();
                var expiredData = _xAxisLabels.Select(m => monthlyExpired.FirstOrDefault(x => x.Month == m)?.Count ?? 0).Select(c => (double)c).ToArray();

                _barSeries = new List<ChartSeries>
            {
                new ChartSeries
                {
                    Name = "Active Memberships",
                    Data = activeData
                },
                new ChartSeries
                {
                    Name = "Expired Memberships",
                    Data = expiredData
                }
            };
            }


            var planCounts = await _membershipService.GetMembershipPlanCountsAsync();
            if (planCounts is not null && planCounts.Count > 0)
            {
                labels = planCounts.Select(p => p.MembershipPlan).ToArray();
                data = planCounts.Select(p => (double)p.Count).ToArray();
            }
            GenerateCalendar(CurrentMonth);

        }
        private void GenerateCalendar(DateTime month)
        {
            CalendarDates.Clear();
            var firstDay = new DateTime(month.Year, month.Month, 1);
            var start = firstDay.AddDays(-(int)firstDay.DayOfWeek);

            for (int i = 0; i < 42; i++)
            {
                CalendarDates.Add(start.AddDays(i));
            }
        }
        protected string GetDateStyle(DateTime date)
        {
            if (date.Month != CurrentMonth.Month)
                return "color:gray";

            if (date.Date == SelectedDate.Date)
                return "background-color:#B0332E; color:white; border-radius:50%;";

            return "";
        }


    }
}
