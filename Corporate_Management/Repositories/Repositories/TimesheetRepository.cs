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

        public async Task<int> AddTimesheetEntry(Timesheet timesheet)
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

        public async Task<Timesheet> getTimesheetEntryByUserId(int userId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                var rows = await connection.QueryFirstOrDefaultAsync<Timesheet>(
                    "sp_getTimesheetEntryByUserId",
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

    }
}
