using Corporate_Management.DTOs;
using Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Corporate_Management.Repositories.Repositories
{
    public class AttendanceRepository: IAttendanceRepository
    {
        private readonly string _connectionString;
        public AttendanceRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("dbconnection");

        }

        #region old checkin/checkout repos

        //public async Task<int> userCheckIn(int Id)
        //{
        //    try 
        //    {
        //        using var connection = new SqlConnection(_connectionString);
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@UserId",Id);
        //        var checkindata =await connection.ExecuteScalarAsync<int>("sp_checkIn", parameters, commandType: CommandType.StoredProcedure);
        //        return checkindata;   
        //    }
        //    catch (SqlException ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }
        //}

        //public async Task<int> userCheckOut(int AId)
        //{
        //    try
        //    {
        //        using var connection = new SqlConnection(_connectionString);
        //        var parameters = new DynamicParameters();          
        //        parameters.Add("@AttendenceId",AId);
        //        var checkindata = await connection.ExecuteAsync("sp_checkOut", parameters, commandType: CommandType.StoredProcedure);
        //        return checkindata;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion

        //public async Task<int> userCheckIn(int Id)
        //{
        //    using var connection = new SqlConnection(_connectionString);

        //    var parameters = new DynamicParameters();
        //    parameters.Add("@UserId", Id);

        //    var attendanceId = await connection.ExecuteScalarAsync<int>(
        //        "sp_checkIn",
        //        parameters,
        //        commandType: CommandType.StoredProcedure
        //    );

        //    var userName = await connection.QuerySingleAsync<string>(
        //        "SELECT Username FROM Users WHERE Id = @Id AND IsDeleted = 0",
        //        new { Id }
        //    );

        //    var userIds = await connection.QueryAsync<int>(
        //        "SELECT Id FROM Users WHERE RoleId IN (1,3,4) AND IsDeleted = 0"
        //    );

        //    var notificationId = await connection.QuerySingleAsync<int>(
        //        "sp_CreateNotification",
        //        new
        //        {
        //            Title = "Check-In",
        //            Message = $"{userName} checked in",
        //            Type = "Attendance"
        //        },
        //        commandType: CommandType.StoredProcedure
        //    );

        //    foreach (var userId in userIds)
        //    {
        //        await connection.ExecuteAsync(
        //            "sp_InsertUserNotification",
        //            new { NotificationId = notificationId, UserId = userId },
        //            commandType: CommandType.StoredProcedure
        //        );
        //    }

        //    return attendanceId;
        //}

        public async Task<int> userCheckIn(int Id)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", Id);

            var attendanceId = await connection.ExecuteScalarAsync<int>(
                "sp_checkIn",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var user = await connection.QuerySingleAsync<dynamic>(
                "SELECT Username, ManagerId FROM Users WHERE Id = @Id AND IsDeleted = 0",
                new { Id }
            );

            string userName = user.Username;
            int? managerId = user.ManagerId;

            var adminHrIds = await connection.QueryAsync<int>(
                "SELECT Id FROM Users WHERE RoleId IN (1,4) AND IsDeleted = 0"
            );

            var notifyUserIds = new List<int>(adminHrIds);

            if (managerId.HasValue)
            {
                notifyUserIds.Add(managerId.Value);
            }

            var notificationId = await connection.QuerySingleAsync<int>(
                "sp_CreateNotification",
                new
                {
                    Title = "Check-In",
                    Message = $"{userName} checked in",
                    Type = "Attendance"
                },
                commandType: CommandType.StoredProcedure
            );

            foreach (var userId in notifyUserIds.Distinct())
            {
                await connection.ExecuteAsync(
                    "sp_InsertUserNotification",
                    new { NotificationId = notificationId, UserId = userId },
                    commandType: CommandType.StoredProcedure
                );
            }

            return attendanceId;
        }

        //public async Task<int> userCheckOut(int AId)
        //{
        //    using var connection = new SqlConnection(_connectionString);

        //    var parameters = new DynamicParameters();
        //    parameters.Add("@AttendenceId", AId);

        //    var attendanceId =await connection.ExecuteAsync(
        //        "sp_checkOut",
        //        parameters,
        //        commandType: CommandType.StoredProcedure
        //    );

        //    var userId = await connection.QuerySingleOrDefaultAsync<int>(
        //        "SELECT UserId FROM Attendance WHERE AId = @AId",
        //        new { AId }
        //    );

        //    var userName = await connection.QuerySingleOrDefaultAsync<string>(
        //        "SELECT Username FROM Users WHERE Id = @Id AND IsDeleted = 0",
        //        new { Id = userId }
        //    );

        //    var userIds = await connection.QueryAsync<int>(
        //        "SELECT Id FROM Users WHERE RoleId IN (1,3,4) AND IsDeleted = 0"
        //    );

        //    var notificationId = await connection.QuerySingleAsync<int>(
        //        "sp_CreateNotification",
        //        new
        //        {
        //            Title = "Check-Out",
        //            Message = $"{userName} checked out",
        //            Type = "Attendance"
        //        },
        //        commandType: CommandType.StoredProcedure
        //    );

        //    foreach (var id in userIds)
        //    {
        //        await connection.ExecuteAsync(
        //            "sp_InsertUserNotification",
        //            new { NotificationId = notificationId, UserId = id },
        //            commandType: CommandType.StoredProcedure
        //        );
        //    }

        //    return attendanceId;
        //}

        public async Task<int> userCheckOut(int AId)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@AttendenceId", AId);

            var attendanceId = await connection.ExecuteAsync(
                "sp_checkOut",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var userId = await connection.QuerySingleOrDefaultAsync<int>(
                "SELECT UserId FROM Attendance WHERE AId = @AId",
                new { AId }
            );

            var user = await connection.QuerySingleOrDefaultAsync<dynamic>(
                "SELECT Username, ManagerId FROM Users WHERE Id = @Id AND IsDeleted = 0",
                new { Id = userId }
            );

            string userName = user?.Username ?? "User";
            int? managerId = user?.ManagerId;

            var adminHrIds = await connection.QueryAsync<int>(
                "SELECT Id FROM Users WHERE RoleId IN (1,4) AND IsDeleted = 0"
            );

            var notifyUserIds = new List<int>(adminHrIds);

            if (managerId.HasValue)
            {
                notifyUserIds.Add(managerId.Value);
            }

            var notificationId = await connection.QuerySingleAsync<int>(
                "sp_CreateNotification",
                new
                {
                    Title = "Check-Out",
                    Message = $"{userName} checked out",
                    Type = "Attendance"
                },
                commandType: CommandType.StoredProcedure
            );

            foreach (var id in notifyUserIds.Distinct())
            {
                await connection.ExecuteAsync(
                    "sp_InsertUserNotification",
                    new { NotificationId = notificationId, UserId = id },
                    commandType: CommandType.StoredProcedure
                );
            }

            return attendanceId;
        }

        public async Task<int> AutoCheckout()
        {
            using var connection = new SqlConnection(_connectionString);

            var result = await connection.ExecuteAsync(
                "sp_AutoCheckout",
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<IEnumerable<AttendanceDto>> getByUserId(int Id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Id", Id);
                var userdata = await connection.QueryAsync<AttendanceDto>("sp_getAttendenceByUserId", parameters, commandType: CommandType.StoredProcedure);
                return userdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<AttendanceDto>> getByAttendanceId(int Id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Id", Id);
                var attendancedata = await connection.QueryAsync<AttendanceDto>("sp_getByAttendenceId", parameters, commandType: CommandType.StoredProcedure);
                return attendancedata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<AttendanceDto>> GetAllAttendanceAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var data = await connection.QueryAsync<AttendanceDto>("sp_getAllAttendance", commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AttendanceDto>> GetTeamAllAttendance(int managerId)
        {
            try 
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@ManagerId", managerId);

                var result = await connection.QueryAsync<AttendanceDto>("sp_getTeamAllAttendance", parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AttendanceReportDto>> GetAttendanceReport(AttendanceReportParameters param)
        {
            try 
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@FromDate", param.FromDate);
                parameters.Add("@ToDate", param.ToDate);
                parameters.Add("@UserId", param.UserId);
                parameters.Add("@Department", param.Department);

                var result = await connection.QueryAsync<AttendanceReportDto>(
                    "sp_GetAttendanceReport",
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
