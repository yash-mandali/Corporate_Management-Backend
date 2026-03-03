using Corporate_Management.DTOs;
using Corporate_Management.Models;

namespace Corporate_Management.Repositories.IRepositories
{
    public interface ILeaveRepository
    {
        Task<int> CreateLeave(Leave leavemodel);
    }
}
