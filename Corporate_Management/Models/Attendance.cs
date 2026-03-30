namespace Corporate_Management.Models
{
    public class Attendance
    {
        public int AId { get; set; }
        public int UserId { get; set; }
        public DateOnly Date { get; set; }
        public string? Day { get; set; }   // Computed column from SQL
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public decimal? Hours { get; set; }   // Computed column
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
    }

    public class AttendanceReportDto
    {
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public string WorkingHours { get; set; }
        public string Status { get; set; }
    }

    public class AttendanceReportParameters
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? UserId { get; set; }
        public string? Department { get; set; }
    }
}
