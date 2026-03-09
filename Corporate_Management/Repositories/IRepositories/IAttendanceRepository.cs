using Corporate_Management.DTOs;

namespace Corporate_Management.Repositories.IRepositories
{
    public interface IAttendanceRepository
    {
        Task<int> userCheckIn(int Id);
        Task<int> userCheckOut(int Id);
    }
}
