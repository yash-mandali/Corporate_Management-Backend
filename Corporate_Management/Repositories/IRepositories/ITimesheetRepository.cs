using Corporate_Management.Models;
using Corporate_Management.Models.Corporate_Management.Models;

namespace Corporate_Management.Repositories.IRepositories
{
    public interface ITimesheetRepository
    {
        Task<int> AddTimesheetEntry(AddTimesheet timesheet);
        Task<bool> UpdateTimesheetEntry(updateTimesheet timesheet);
        Task<Timesheet> getTimesheetEntryById(int sheetId);
        Task<IEnumerable<Timesheet>> getTimesheetEntryByUserId(int userId);
        Task<IEnumerable<Timesheet>> getAllTimesheetEntry();
        Task<int> deleteTimesheetEntry(int sheetId);
        Task<int> SubmitTimesheetEntry(int sheetId);
       
        
        Task<IEnumerable<Timesheet>> getTimesheetByStatus(string status);
        //----------------------------------Manager-------------------------------

        Task<int> ApproveByManager(int sheetId);
        Task<int> RejectByManager(int sheetId, string reason);
        Task<IEnumerable<Timesheet>> GetManagerTeamTimesheets(int managerId);

      
    }
}
