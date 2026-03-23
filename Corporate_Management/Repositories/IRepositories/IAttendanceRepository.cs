using Corporate_Management.DTOs;

namespace Corporate_Management.Repositories.IRepositories
{
    public interface IAttendanceRepository
    {
        Task<int> userCheckIn(int Id);
        Task<int> userCheckOut(int AId);
        Task<int> AutoCheckout();
        Task<IEnumerable<AttendanceDto>> getByUserId(int Id);
        Task<IEnumerable<AttendanceDto>> getByAttendanceId(int Id);
        Task<IEnumerable<AttendanceDto>> GetAllAttendanceAsync();
        Task<IEnumerable<AttendanceDto>> GetTeamAllAttendance(int managerId); 
    }
}
