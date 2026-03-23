using Corporate_Management.Models;
using Corporate_Management.Models.Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Corporate_Management.Repositories.IRepositories.Repositories;
using Corporate_Management.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Corporate_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : ControllerBase
    {
        private readonly ITimesheetRepository _timesheetrepository;

        public TimesheetController(ITimesheetRepository timesheetrepository)
        {
            _timesheetrepository = timesheetrepository;
        }

        [HttpPost("AddTimesheetEntry")]
        public async Task<IActionResult> AddTimesheetEntry(AddTimesheet timesheet)
        {
            try
            {
                var data = await _timesheetrepository.AddTimesheetEntry(timesheet);

                return Ok(new { message = "Timesheet Added",timesheetId= data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Timesheet Error", error = ex.Message });
            }
        }

        [HttpPut("updateTimesheetEntry")]
        public async Task<IActionResult> UpdateLeave(updateTimesheet timesheet)
        {
            try
            {
                var result = await _timesheetrepository.UpdateTimesheetEntry(timesheet);

                if (!result)
                {
                    return BadRequest(new
                    {
                        message = "Timesheet cannot be updated."
                    });
                }

                return Ok(new { message = "Timesheet updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to update Timesheet", error = ex.Message });
            }
        }

        [HttpGet("getTimesheetEntryById")]
        public async Task<IActionResult> getTimesheetEntryById(int sheetId)
        {
            try
            {
                var timesheet = await _timesheetrepository.getTimesheetEntryById(sheetId);

                if (timesheet == null)
                    return NotFound(new { message = "timesheet not found" });

                return Ok(timesheet);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving timesheet", error = ex.Message });
            }
        }

        [HttpGet("getTimesheetEntryByUserId")]
        public async Task<IActionResult> getTimesheetEntryByUserId(int userId)
        {
            try
            {
                var timesheet = await _timesheetrepository.getTimesheetEntryByUserId(userId);

                if (timesheet == null)
                    return NotFound(new { message = "timesheet not found" });

                return Ok(timesheet);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error retrieving timesheet", error = ex.Message });
            }
        }

        [HttpGet("GetAlltimesheets")]
        public async Task<IActionResult> getAllTimesheetEntry()
        {
            try
            {
                var timesheets = await _timesheetrepository.getAllTimesheetEntry();

                if (timesheets == null)
                    return NotFound(new { message = "timesheets not found" });

                return Ok(timesheets);

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to fetch timesheets", error = ex.Message });
            }
        }

        [HttpGet("manager-GetAlltimesheets")]
        public async Task<IActionResult> GetManagerTeamTimesheets(int managerId)
        {
            try 
            {
                var result = await _timesheetrepository.GetManagerTeamTimesheets(managerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to fetch timesheets", error = ex.Message });
            }
        }

        [HttpDelete("DeleteTimesheet")]
        public async Task<IActionResult> DeleteUser(int sheetId)
        {
            try
            {
                var userId = await _timesheetrepository.deleteTimesheetEntry(sheetId);
                return Ok(new { message = "Timesheet deleted Succesfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Timesheet delete error", error = ex.Message });
            }
        }

        [HttpPost("submitTimesheet")]
        public async Task<IActionResult> submitTimesheet(int sheetId)
        {
            try
            {
                var userId = await _timesheetrepository.SubmitTimesheetEntry(sheetId);
                return Ok(new { message = "Timesheet submited Succesfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Timesheet submit error", error = ex.Message });
            }
        }

        [HttpPut("ManagerApproveT")]
        public async Task<IActionResult> ManagerApprove(int sheetId)
        {
            try
            {
                var userId = await _timesheetrepository.ApproveByManager(sheetId);
                if (userId == null)
                    return NotFound(new { message = "Id not found" });

                return Ok(new { message = "Timesheet Approved " });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Timesheet Approved error", error = ex.Message });
            }
        }

        [HttpPut("ManagerRejectT")]
        public async Task<IActionResult> ManagerReject(int sheetId, string reason)
        {
            try
            {
                var result = await _timesheetrepository.RejectByManager(sheetId, reason);
                if (result == null)
                    return NotFound(new { message = "data not found" });



                return Ok(new { message = "Timesheet Rejected" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Timesheet Rejected error", error = ex.Message });
            }
        }

        [HttpGet("getByStatus(manager)")]
        public async Task<IActionResult> getTimesheetByStatus(string status)
        {
            try
            {
                var timesheet = await _timesheetrepository.getTimesheetByStatus(status);

                if (timesheet == null)
                    return NotFound(new { message = "status not found" });

                return Ok(timesheet); ;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "failed to fetch Timesheet", error = ex.Message });
            }
        }
    }
}
