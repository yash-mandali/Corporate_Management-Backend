using Corporate_Management.DTOs;
using Corporate_Management.Models;

namespace Corporate_Management.Repositories.IRepositories
{
    public interface ILeaveRepository
    {
        Task<int> CreateLeave(Leave leavemodel);
        Task<IEnumerable<LeaveListDto>> GetLeaveByIUserId(int id);
        Task<IEnumerable<LeaveListDto>> GetLeaveByLeaveId(int id);
        Task<IEnumerable<LeaveListDto>> GetAllLeaveAsync();
        Task<bool> WithdrawLeave(int id);
        Task<bool> UpdateLeave(int leaveId, updateLeaveDto leavemodel);
        Task<IEnumerable<LeaveListDto>> GetAllPendingLeaves();
        Task<bool> ApproveLeave(int leaveId);
        Task<bool> RejectLeave(int leaveId);

    }
}
