namespace Corporate_Management.Models
{
    using System;

    namespace Corporate_Management.Models
    {
        public class Timesheet        {
            public int TimesheetId { get; set; }
            public int UserId { get; set; }
            public int ManagerId { get; set; }
            public string UserName { get; set; }
            public DateTime WorkDate { get; set; }
            public string? ProjectName { get; set; }
            public string? TaskDescription { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public string? TotalHours { get; set; } 
            public string? Status { get; set; }
            public string? WorkType { get; set; }
            public string RejectReason { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }

        public class AddTimesheet
        {
            public int TimesheetId { get; set; }
            public int UserId { get; set; }
            public DateTime WorkDate { get; set; }
            public string? ProjectName { get; set; }
            public string? TaskDescription { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public string? WorkType { get; set; }
        }

        public class updateTimesheet 
        {
            public int TimesheetId { get; set; }
            public string? ProjectName { get; set; }
            public string? TaskDescription { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public string? WorkType { get; set; }
        }

        public class TimesheetReportParameters
        {
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public int? UserId { get; set; }
            public string? Department { get; set; }
            public string? Status { get; set; }
            public string? WorkType { get; set; }
        }

        public class TimesheetReportDto
        {
            public string EmployeeName { get; set; }
            public string Department { get; set; }
            public DateTime WorkDate { get; set; }
            public string ProjectName { get; set; }
            public string TaskDescription { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public string TotalHours { get; set; }
            public string Status { get; set; }
            public string WorkType { get; set; }
        }

    }
}
