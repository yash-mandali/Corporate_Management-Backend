using ClosedXML.Excel;
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
        }  //manager

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

        //-----------------------------------timesheetexport report-------------------------------

        [HttpGet("ExportTimesheetReport")]
        public async Task<IActionResult> ExportTimesheetReport([FromQuery] TimesheetReportParameters parameters)
        {
            try
            {
                var data = await _timesheetrepository.GetTimesheetReportdata(parameters);

                if (data == null || !data.Any())
                    return NotFound(new { message = "No records found for report." });

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Timesheet Report");

                // Header
                worksheet.Cell(1, 1).Value = "Employee Name";
                worksheet.Cell(1, 2).Value = "Department";
                worksheet.Cell(1, 3).Value = "Work Date";
                worksheet.Cell(1, 4).Value = "Project";
                worksheet.Cell(1, 5).Value = "Task";
                worksheet.Cell(1, 6).Value = "Start Time";
                worksheet.Cell(1, 7).Value = "End Time";
                worksheet.Cell(1, 8).Value = "Total Hours";
                worksheet.Cell(1, 9).Value = "Status";
                worksheet.Cell(1, 10).Value = "Work Type";

                var headerRange = worksheet.Range("A1:J1");

                headerRange.Style.Font.Bold = true;
                headerRange.Style.Font.FontColor = XLColor.RichBlack;
                headerRange.Style.Fill.BackgroundColor = XLColor.AshGrey;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                int row = 2;

                foreach (var item in data)
                {
                    worksheet.Cell(row, 1).Value = item.EmployeeName;
                    worksheet.Cell(row, 2).Value = item.Department;
                    worksheet.Cell(row, 3).Value = item.WorkDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 4).Value = item.ProjectName;
                    worksheet.Cell(row, 5).Value = item.TaskDescription;
                    worksheet.Cell(row, 6).Value = item.StartTime.ToString(@"hh\:mm");
                    worksheet.Cell(row, 7).Value = item.EndTime.ToString(@"hh\:mm");
                    worksheet.Cell(row, 8).Value = item.TotalHours;
                    worksheet.Cell(row, 9).Value = item.Status;
                    worksheet.Cell(row, 10).Value = item.WorkType;

                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(
                    stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Timesheet_Report_{DateTime.Now:yyyyMMdd}.xlsx"
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to generate report", error = ex.Message });
            }
        }
    }
}
