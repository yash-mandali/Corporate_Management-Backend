using Corporate_Management.Models;
using Corporate_Management.Models.Corporate_Management.Models;

namespace Corporate_Management.Repositories.IRepositories
{
    public interface ITimesheetRepository
    {
        Task<int> AddTimesheetEntry(AddTimesheet timesheet);
        Task<bool> UpdateTimesheetEntry(updateTimesheet timesheet);
        Task<Timesheet> getTimesheetEntryById(int sheetId);
        Task<Timesheet> getTimesheetEntryByUserId(int userId);
        Task<IEnumerable<Timesheet>> getAllTimesheetEntry();
        Task<int> deleteTimesheetEntry(int sheetId);
        Task<int> SubmitTimesheetEntry(int sheetId);
        Task<int> ApproveTimesheetEntry(int sheetId);
        Task<int> RejectTimesheetEntry(int sheetId, string reason);
        Task<Timesheet> getTimesheetByStatus(string status);
    }
}
