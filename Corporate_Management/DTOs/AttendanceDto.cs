namespace Corporate_Management.DTOs
{
    public class AttendanceDto
    {
        public int AId { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public string? Day { get; set; }   // Computed column from SQL

        public TimeSpan? CheckIn { get; set; }

        public TimeSpan? CheckOut { get; set; }

        public string? Hours { get; set; }   // Computed column

        public bool IsCheckIn { get; set; }
        public bool IsCheckOut { get; set; }
        public string Status { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }
    }
}
