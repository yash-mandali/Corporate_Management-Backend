using Corporate_Management.Models;
using Corporate_Management.Models.Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Corporate_Management.Repositories.IRepositories.Repositories;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> AddTimesheetEntry(Timesheet timesheet)
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
    }
}
