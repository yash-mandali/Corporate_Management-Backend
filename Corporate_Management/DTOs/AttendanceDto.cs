namespace Corporate_Management.DTOs
{
    public class AttendanceCheckInDto
    {
        public int UserId { get; set; }
        public string? Note { get; set; }
    }
    public class AttendanceCheckOutDto
    {
        public int AttendenceId { get; set; }
        public int UserId { get; set; }
    }
}
