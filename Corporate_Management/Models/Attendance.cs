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

        public string? Note { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
