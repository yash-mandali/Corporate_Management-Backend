using Corporate_Management.Models;
using Corporate_Management.Models.Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Corporate_Management.Repositories.Repositories
{
    public class TimesheetRepository:ITimesheetRepository
    {
        private readonly string _connectionString;

        public TimesheetRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("dbconnection");
        }

        public async Task<int> AddTimesheetEntry(AddTimesheet timesheet)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameter = new DynamicParameters();
                parameter.Add("@UserId", timesheet.UserId);
                parameter.Add("@WorkDate", timesheet.WorkDate);        
                parameter.Add("@ProjectName", timesheet.ProjectName);
                parameter.Add("@TaskDescription", timesheet.TaskDescription);
                parameter.Add("@StartTime", timesheet.StartTime);
                parameter.Add("@EndTime", timesheet.EndTime);
                parameter.Add("@WorkType", timesheet.WorkType);
                parameter.Add("@newId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("sp_AddTimesheetEntry", parameter, commandType: CommandType.StoredProcedure);
                return parameter.Get<int>("@newId");
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateTimesheetEntry( updateTimesheet timesheet)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameter = new DynamicParameters();
                parameter.Add("@TimesheetId", timesheet.TimesheetId);
                parameter.Add("@ProjectName", timesheet.ProjectName);
                parameter.Add("@TaskDescription", timesheet.TaskDescription);
                parameter.Add("@StartTime", timesheet.StartTime);
                parameter.Add("@EndTime", timesheet.EndTime);
                parameter.Add("@WorkType", timesheet.WorkType);

                var result = await connection.ExecuteAsync("sp_UpdateTimesheetEntry", parameter,commandType: CommandType.StoredProcedure);
                return result > 0;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Timesheet> getTimesheetEntryById(int sheetId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@sheetId", sheetId);

                var rows = await connection.QueryFirstOrDefaultAsync<Timesheet>(
                    "sp_getTimesheetEntryById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return rows;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Timesheet>> getTimesheetEntryByUserId(int userId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                var rows = await connection.QueryAsync<Timesheet>("sp_getTimesheetEntryByUserId", parameters,commandType: CommandType.StoredProcedure);
                return rows;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Timesheet>> getAllTimesheetEntry()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var timesheet = await connection.QueryAsync<Timesheet>("sp_getAllTimesheetEntry", commandType: CommandType.StoredProcedure);
                return timesheet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> deleteTimesheetEntry(int sheetId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@sheetId", sheetId);

                var rows = await connection.ExecuteAsync(
                    "sp_deleteTimesheetEntry",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return rows;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> SubmitTimesheetEntry(int sheetId)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@sheetId", sheetId);

            var rows = await connection.ExecuteAsync(
                "sp_SubmitTimesheet",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var userId = await connection.QuerySingleAsync<int>(
                "SELECT UserId FROM TimesheetEntries WHERE TimeSheetId = @sheetId",
                new { sheetId }
            );

            var userName = await connection.QuerySingleAsync<string>(
                "SELECT Username FROM Users WHERE Id = @Id AND IsDeleted = 0",
                new { Id = userId }
            );

            var managerId = await connection.QuerySingleOrDefaultAsync<int?>(
                "SELECT ManagerId FROM Users WHERE Id = @Id AND IsDeleted = 0",
                new { Id = userId }
            );
            if (managerId == null)
            {
                throw new Exception("Manager not assigned to this user.");
            }

            var notificationId = await connection.QuerySingleAsync<int>(
                "sp_CreateNotification",
                new
                {
                    Title = "Timesheet Submitted",
                    Message = $"{userName} submitted timesheet",
                    Type = "Timesheet"
                },
                commandType: CommandType.StoredProcedure
            );

            await connection.ExecuteAsync(
                "sp_InsertUserNotification",
                new { NotificationId = notificationId, UserId = managerId },
                commandType: CommandType.StoredProcedure
            );

            return rows;
        }

        public async Task<IEnumerable<Timesheet>> getTimesheetByStatus(string status)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Status", status);

                var rows = await connection.QueryAsync<Timesheet>(
                    "sp_GetTimesheetByStatus",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return rows;
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        //----------------------------------Manager-------------------------------

        public async Task<int> ApproveByManager(int sheetId)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@sheetId", sheetId);

            var rows = await connection.ExecuteAsync(
                "sp_ManagerApproveTimesheet",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var userId = await connection.QuerySingleOrDefaultAsync<int?>(
                "SELECT UserId FROM TimesheetEntries WHERE TimeSheetId = @sheetId",
                new { sheetId }
            );

            if (!userId.HasValue)
            {
                throw new Exception("Timesheet not found.");
            }

            var userName = await connection.QuerySingleAsync<string>(
                "SELECT Username FROM Users WHERE Id = @Id AND IsDeleted = 0",
                new { Id = userId.Value }
            );

            var notificationId = await connection.QuerySingleAsync<int>(
                "sp_CreateNotification",
                new
                {
                    Title = "Timesheet Approved",
                    Message = $"Timesheet was approved by manager.",
                    Type = "Timesheet"
                },
                commandType: CommandType.StoredProcedure
            );

            await connection.ExecuteAsync(
                "sp_InsertUserNotification",
                new
                {
                    NotificationId = notificationId,
                    UserId = userId.Value
                },
                commandType: CommandType.StoredProcedure
            );

            return rows;
        }

        public async Task<int> RejectByManager(int sheetId, string reason)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@sheetId", sheetId);
            parameters.Add("@RejectReason", reason);

            var rows = await connection.ExecuteAsync(
                "sp_ManagerRejectTimesheet",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var userId = await connection.QuerySingleOrDefaultAsync<int?>(
                "SELECT UserId FROM TimesheetEntries WHERE TimeSheetId = @sheetId",
                new { sheetId }
            );

            if (!userId.HasValue)
            {
                throw new Exception("Timesheet not found.");
            }

            var notificationId = await connection.QuerySingleAsync<int>(
                "sp_CreateNotification",
                new
                {
                    Title = "Timesheet Rejected",
                    Message = $"Timesheet was rejected by manager.",
                    Type = "Timesheet"
                },
                commandType: CommandType.StoredProcedure
            );

            await connection.ExecuteAsync(
                "sp_InsertUserNotification",
                new
                {
                    NotificationId = notificationId,
                    UserId = userId.Value
                },
                commandType: CommandType.StoredProcedure
            );

            return rows;
        }

        public async Task<IEnumerable<Timesheet>> GetManagerTeamTimesheets(int managerId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@ManagerId", managerId);

                var result = await connection.QueryAsync<Timesheet>(
                    "sp_managerTeamAllTimesheetEntry",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
