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
        Task<IEnumerable<LeaveListDto>> GetTeamAllLeaveRequests(int managerId);
        Task<bool> WithdrawLeave(int leaveRequestId);
        Task<bool> UpdateLeave(int leaveId, updateLeaveDto leavemodel);
        Task<IEnumerable<LeaveListDto>> GetAllPendingLeaves();
        Task<IEnumerable<LeaveListDto>> GetAllRejectedLeaves();
        Task<IEnumerable<LeaveListDto>> GetAllApprovedLeaves();
        Task<IEnumerable<LeaveListDto>> GetAllWithdrawnLeaves();
        Task<int> AutoRejectLeave();

        //----------------------------------Manager-------------------------------
        Task<IEnumerable<LeaveListDto>> GetManagerTeamPendingLeaves(int managerId);
        Task<IEnumerable<LeaveListDto>> GetManagerApprovedLeaves();
        Task<bool> ManagerApproveLeave(int leaveId);
        Task<bool> ManagerRejectLeave(int leaveId);


        //----------------------------------HR------------------------------------
        Task<bool> HrApproveLeave(int leaveId);
        Task<bool> HrRejectLeave(int leaveId);
        Task<bool> InitilizeUsersLeaveBalance();
        Task<IEnumerable<UserLeaveBalanceDto>> getUserLeaveBalance(int userId);

    }
}
