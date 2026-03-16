namespace Corporate_Management.DTOs
{
    using System;

    namespace Corporate_Management.Models
    {
        public class TimesheetDto
        {
            public int TimesheetId { get; set; }

            public int UserId { get; set; }

            public DateTime WorkDate { get; set; }

            public string? ProjectName { get; set; }

            public string? TaskDescription { get; set; }

            public TimeSpan StartTime { get; set; }

            public TimeSpan EndTime { get; set; }

            public string? TotalHours { get; set; }

            public string? Status { get; set; }

            public string? WorkType { get; set; }

            public DateTime CreatedAt { get; set; }

            public DateTime? UpdatedAt { get; set; }
        }
    }
}
