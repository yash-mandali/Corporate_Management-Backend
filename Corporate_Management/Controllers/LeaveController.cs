using Corporate_Management.DTOs;
using Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Corporate_Management.Repositories.IRepositories.Repositories;
using Corporate_Management.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Corporate_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly ILeaveRepository _leaveRepositories;


        public LeaveController(ILeaveRepository leaveRepositories)
        {
            _leaveRepositories = leaveRepositories;
        }


        [HttpPost("ApplyLeave")]
        public async Task<IActionResult> ApplyLeave(Leave leave)
        {
            try
            {

                var userId = await _leaveRepositories.CreateLeave(leave);

                return Ok(new { message = "Leave Applied Succesfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Leave Error", error = ex.Message });
            }
        }

        [HttpPut("updateLeave/{id}")]
        public async Task<IActionResult> UpdateLeave(int id, updateLeaveDto leavemodel)
        {
            try
            {
                var result = await _leaveRepositories.UpdateLeave(id, leavemodel);

                if (!result)
                {
                    return BadRequest(new
                    {
                        message = "Leave cannot be updated. It may already be approved, rejected, or withdrawn."
                    });
                }

                return Ok(new { message = "Leave updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to update leaves", error = ex.Message });
            }
        }

        [HttpGet("getMyLeaves")]
        public async Task<IActionResult> getMyLeaves(int id)
        {
            try
            {
                var leave = await _leaveRepositories.GetLeaveByIUserId(id);

                if (leave == null || !leave.Any())
                    return NotFound(new { message = "Leave not found" });

                return Ok(leave);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving leave", error = ex.Message });
            }
        }

        [HttpGet("getLeaveById")]
        public async Task<IActionResult> getLeaveById(int id)
        {
            try
            {
                var leave = await _leaveRepositories.GetLeaveByLeaveId(id);

                if (leave == null || !leave.Any())
                    return NotFound(new { message = "Leave not found" });

                return Ok(leave);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving leave", error = ex.Message });
            }
        }

        [HttpGet("GetAllLeaves")]
        public async Task<IActionResult> getAllLeaves()
        {
            try
            {
                var leaves = await _leaveRepositories.GetAllLeaveAsync();

                if (leaves == null)
                    return NotFound(new { message = "Leaves not found" });

                return Ok(leaves);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to fetch leaves", error = ex.Message });
            }
        }

        [HttpGet("managerTeam-AllLeaves")]
        public async Task<IActionResult> GetTeamAllLeaveRequests(int managerId)
        {
            try
            {
                var leaves = await _leaveRepositories.GetTeamAllLeaveRequests(managerId);

                if (leaves == null)
                    return NotFound(new { message = "Leaves not found" });

                return Ok(leaves);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to fetch leaves", error = ex.Message });
            }
        }

        [HttpPut("withdrawLeave/{leaveRequestId}")]
        public async Task<IActionResult> WithdrawLeave(int leaveRequestId)
        {
            try
            {
                var result = await _leaveRepositories.WithdrawLeave(leaveRequestId);

                if (!result)
                {
                    return BadRequest(new
                    {
                        message = "Leave cannot be withdrawn. It may already be approved or rejected."
                    });
                }

                return Ok(new { message = "Leave withdrawn successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to withdrawn laeves", error = ex.Message });
            }
        }

        [HttpGet("GetAllPendingLeaves")]
        public async Task<IActionResult> GetPendingLeaves()
        {
            try
            {
                var leaves = await _leaveRepositories.GetAllPendingLeaves();

                return Ok(new
                {
                    message = "Pending leaves...",
                    TotalLeaves = leaves.Count(),
                    data = leaves
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get Pending leaves", error = ex.Message });
            }
        }
        [HttpGet("GetAllApprovedLeaves")]
        public async Task<IActionResult> GetApprovedLeaves()
        {
            try
            {
                var leaves = await _leaveRepositories.GetAllApprovedLeaves();

                return Ok(new
                {
                    message = "approved leaves...",
                    TotalLeaves = leaves.Count(),
                    data = leaves
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get approved leaves", error = ex.Message });
            }
        }
        [HttpGet("GetAllRejectedLeaves")]
        public async Task<IActionResult> GetAllRejectedLeaves()
        {
            try
            {
                var leaves = await _leaveRepositories.GetAllRejectedLeaves();

                return Ok(new
                {
                    message = "rejected leaves...",
                    TotalLeaves = leaves.Count(),
                    data = leaves
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get rejected leaves", error = ex.Message });
            }
        }
        [HttpGet("GetAllWithdrawnLeaves")]
        public async Task<IActionResult> GetAllWithdrawnLeaves()
        {
            try
            {
                var leaves = await _leaveRepositories.GetAllWithdrawnLeaves();

                return Ok(new
                {
                    message = "withdrawn leaves...",
                    TotalLeaves = leaves.Count(),
                    data = leaves
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get withdrawn leaves", error = ex.Message });
            }
        }

        [HttpGet("GetManagerApprovedLeaves")]
        public async Task<IActionResult> GetManagerApprovedLeaves()
        {
            try
            {
                var leaves = await _leaveRepositories.GetManagerApprovedLeaves();

                return Ok(new
                {
                    message = "ManahgerApproved leaves...",
                    TotalLeaves = leaves.Count(),
                    data = leaves
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get leaves", error = ex.Message });
            }
        }

        [HttpGet("managerteam-pendingleaves")]
        public async Task<IActionResult> GetManagerPendingLeaves(int managerId)
        {
            try
            {
                var result = await _leaveRepositories.GetManagerTeamPendingLeaves(managerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to get pending leaves", error = ex.Message });
            }
        }  //Manager

        [HttpPut("ManagerApproveLeave")]
        public async Task<IActionResult> ManagerApproveLeave(int id)
        {
            try
            {
                var result = await _leaveRepositories.ManagerApproveLeave(id);

                if (!result)
                {
                    return BadRequest(new
                    {
                        message = "Leave cannot be approved."
                    });
                }

                return Ok(new { message = "Leave approved" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to Approve leave", error = ex.Message });
            }
        }

        [HttpPut("HrApproveLeave")]
        public async Task<IActionResult> HrApproveLeave(int id)
        {
            try
            {
                var result = await _leaveRepositories.HrApproveLeave(id);

                if (!result)
                {
                    return BadRequest(new
                    {
                        message = "Leave cannot be approved."
                    });
                }

                return Ok(new { message = "Leave approved" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to Approve leave", error = ex.Message });
            }
        }
        [HttpPut("ManagerRejectLeave")]
        public async Task<IActionResult> ManagerRejectLeave(int id)
        {
            try
            {
                var result = await _leaveRepositories.ManagerRejectLeave(id);

                if (!result)
                {
                    return BadRequest(new
                    {
                        message = "Leave cannot be Rejected."
                    });
                }

                return Ok(new { message = "Leave Rejected" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to Reject leave", error = ex.Message });
            }
        }

        [HttpPut("HrRejectLeave")]
        public async Task<IActionResult> HrRejectLeave(int id)
        {
            try
            {
                var result = await _leaveRepositories.HrRejectLeave(id);

                if (!result)
                {
                    return BadRequest(new
                    {
                        message = "Leave cannot be Rejected."
                    });
                }

                return Ok(new { message = "Leave Rejected" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to Reject leave", error = ex.Message });
            }
        }

        [HttpPut("AutoRejectLeave")]
        public async Task<IActionResult> AutoCheckout()
        {
            try
            {
                var result = await _leaveRepositories.AutoRejectLeave();

                return Ok(new { message = "AutoLeave Rejected", rowsUpdated = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "AutoLeave Rejecte failed", error = ex.Message });
            }
        }

        [HttpPost("initializeUsersLeaveBalance")]
        public async Task<IActionResult> InitializeLeaveBalance()
        {
            try
            {
                var result = await _leaveRepositories.InitilizeUsersLeaveBalance();

                if (result)
                {
                    return Ok(new { message = "Leave balance initialized successfully." });
                }

                return BadRequest(new { message = "Initialization failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("getUserLeaveBalance")]
        public async Task<IActionResult> GetUserLeaveBalance(int userId)
        {
            try
            {
                var result = await _leaveRepositories.getUserLeaveBalance(userId);

                if (result == null || !result.Any())
                {
                    return NotFound("No leave balance found.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error retrieving leave balance",
                    error = ex.Message
                });
            }
        }

        [HttpPut("updateLeaveBalance")]
        public async Task<IActionResult> updateLeaveBalance(int leaveTypeId, decimal defaultBalance)
        {
            try
            {
                var result = await _leaveRepositories.UpdateLeaveBalance(leaveTypeId, defaultBalance);
                if (!result)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Leave balance update failed"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Leave balance updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "update leavebalance failed",
                    error = ex.Message
                });
            }
        }
    }
}
