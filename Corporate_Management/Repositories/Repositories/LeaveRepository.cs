using Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Corporate_Management.Repositories.Repositories
{
    public class LeaveRepository: ILeaveRepository
    {
        private readonly string _connectionString;

        public LeaveRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("dbconnection");
        }

        //public async Task<int> CreateLeave(Leave leavemodel)
        //{
        //    try 
        //    {
        //        using var connection = new SqlConnection(_connectionString);
        //        var parameter = new DynamicParameters();
        //        parameter.Add("@UserId", leavemodel.UserId);    
        //        parameter.Add("@RequestType", leavemodel.RequestType);
        //        parameter.Add("@FromDate", leavemodel.FromDate.ToDateTime(TimeOnly.MinValue));
        //        parameter.Add("@ToDate", leavemodel.ToDate.ToDateTime(TimeOnly.MinValue));
        //        parameter.Add("@Session", leavemodel.Session);
        //        parameter.Add("@Reason", leavemodel.Reason);
        //        parameter.Add("@HandoverTo", leavemodel.HandoverTo);
        //        //parameter.Add("@Status", leavemodel.Status ?? "Pending" );
        //        parameter.Add("@newId", dbType: DbType.Int32, direction: ParameterDirection.Output);

        //        await connection.ExecuteAsync("sp_ApplyLeave", parameter, commandType: CommandType.StoredProcedure);
        //        return parameter.Get<int>("@newId");
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<int> CreateLeave(Leave leavemodel)
        {
            using var connection = new SqlConnection(_connectionString);

            // 🔹 Step 1: Apply Leave
            var parameter = new DynamicParameters();
            parameter.Add("@UserId", leavemodel.UserId);
            parameter.Add("@RequestType", leavemodel.RequestType);
            parameter.Add("@FromDate", leavemodel.FromDate.ToDateTime(TimeOnly.MinValue));
            parameter.Add("@ToDate", leavemodel.ToDate.ToDateTime(TimeOnly.MinValue));
            parameter.Add("@Session", leavemodel.Session);
            parameter.Add("@Reason", leavemodel.Reason);
            parameter.Add("@HandoverTo", leavemodel.HandoverTo);
            parameter.Add("@newId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "sp_ApplyLeave",
                parameter,
                commandType: CommandType.StoredProcedure
            );

            var leaveId = parameter.Get<int>("@newId");

            // 🔹 Step 2: Get Username
            var userName = await connection.QuerySingleAsync<string>(
                "SELECT Username FROM Users WHERE Id = @Id AND IsDeleted = 0",
                new { Id = leavemodel.UserId }
            );

            // 🔹 Step 3: Get Assigned ManagerId
            var managerId = await connection.QuerySingleOrDefaultAsync<int?>(
                "SELECT ManagerId FROM Users WHERE Id = @Id AND IsDeleted = 0",
                new { Id = leavemodel.UserId }
            );

            if (!managerId.HasValue)
                throw new Exception("Manager not assigned.");           

            // 🔹 Step 5: Create Notification
            var notificationId = await connection.QuerySingleAsync<int>(
                "sp_CreateNotification",
                new
                {
                    Title = "Leave Applied",
                    Message = $"{userName} applied for leave from {leavemodel.FromDate:dd MMM yyyy} to {leavemodel.ToDate:dd MMM yyyy}",
                    Type = "Leave"
                },
                commandType: CommandType.StoredProcedure
            );

            // 🔹 Step 6: Send to Assigned Manager
            await connection.ExecuteAsync(
                "sp_InsertUserNotification",
                new
                {
                    NotificationId = notificationId,
                    UserId = managerId.Value
                },
                commandType: CommandType.StoredProcedure
            );

            return leaveId;
        }
        public async Task<bool> UpdateLeave(int leaveId, updateLeaveDto leavemodel)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@LeaveRequestId", leaveId);
            parameters.Add("@RequestType", leavemodel.RequestType);
            parameters.Add("@FromDate", leavemodel.FromDate);
            parameters.Add("@ToDate", leavemodel.ToDate);
            parameters.Add("@Session", leavemodel.Session);
            parameters.Add("@Reason", leavemodel.Reason);
            parameters.Add("@HandoverTo", leavemodel.HandoverTo);

            var result = await connection.ExecuteAsync(
                "sp_UpdateLeave",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result > 0;
        }
        public async Task<IEnumerable<LeaveListDto>> GetLeaveByIUserId(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                var rows = await connection.QueryAsync<LeaveListDto>(
                    "sp_GetLeaveByUserId",
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
        public async Task<IEnumerable<LeaveListDto>> GetLeaveByLeaveId(int id)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@LeaveRequestId", id);

                var rows = await connection.QueryAsync<LeaveListDto>(
                    "sp_GetLeaveByLeaveId",
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
        public async Task<IEnumerable<LeaveListDto>> GetAllLeaveAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var Leaves = await connection.QueryAsync<LeaveListDto>("sp_GetAllLeaveRequests", commandType: CommandType.StoredProcedure);
                return Leaves;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<LeaveListDto>> GetTeamAllLeaveRequests(int managerId)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@ManagerId", managerId);

            var result = await connection.QueryAsync<LeaveListDto>(
                "sp_GetTeamAllLeaveRequests",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        public async Task<bool> WithdrawLeave(int leaveRequestId)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@LeaveRequestId", leaveRequestId);

            var result = await connection.ExecuteAsync(
                "sp_WithdrawLeave",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result > 0;
        }
        public async Task<IEnumerable<LeaveListDto>> GetAllPendingLeaves()
        {
            using var connection = new SqlConnection(_connectionString);

            var leaves = await connection.QueryAsync<LeaveListDto>(
                "sp_GetPendingLeaves",
                commandType: CommandType.StoredProcedure
            );

            return leaves;
        }
        public async Task<IEnumerable<LeaveListDto>> GetAllRejectedLeaves()
        {
            using var connection = new SqlConnection(_connectionString);

            var leaves = await connection.QueryAsync<LeaveListDto>(
                "sp_GetRejectedLeaves",
                commandType: CommandType.StoredProcedure
            );

            return leaves;
        }
        public async Task<IEnumerable<LeaveListDto>> GetAllApprovedLeaves()
        {
            using var connection = new SqlConnection(_connectionString);

            var leaves = await connection.QueryAsync<LeaveListDto>(
                "sp_GetApprovedLeaves",
                commandType: CommandType.StoredProcedure
            );

            return leaves;
        }
        public async Task<IEnumerable<LeaveListDto>> GetAllWithdrawnLeaves()
        {
            using var connection = new SqlConnection(_connectionString);

            var leaves = await connection.QueryAsync<LeaveListDto>(
                "sp_GetWithdrawnLeaves",
                commandType: CommandType.StoredProcedure
            );

            return leaves;
        }
        public async Task<int> AutoRejectLeave()
        {
            using var connection = new SqlConnection(_connectionString);

            var result = await connection.ExecuteAsync(
                "sp_AutoRejectLeaves",
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        //----------------------------------Manager-------------------------------
        public async Task<IEnumerable<LeaveListDto>> GetManagerTeamPendingLeaves(int managerId)
        {
            using var connection = new SqlConnection(_connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@ManagerId", managerId);

            var result = await connection.QueryAsync<LeaveListDto>(
                "sp_GetManagerTeamPendingLeaves",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        public async Task<IEnumerable<LeaveListDto>> GetManagerApprovedLeaves()
        {
            using var connection = new SqlConnection(_connectionString);

            var leaves = await connection.QueryAsync<LeaveListDto>(
                "sp_GetManagerApprovedLeaves",
                commandType: CommandType.StoredProcedure
            );

            return leaves;
        }

        //public async Task<bool> ManagerApproveLeave(int leaveId)
        //{
        //    using var connection = new SqlConnection(_connectionString);

        //    var parameters = new DynamicParameters();
        //    parameters.Add("@LeaveRequestId", leaveId);

        //    var result = await connection.ExecuteAsync(
        //        "sp_ManagerApproveLeave",
        //        parameters,
        //        commandType: CommandType.StoredProcedure
        //    );

        //    return result > 0;
        //}
         
        public async Task<bool> ManagerApproveLeave(int leaveId)
        {
            using var connection = new SqlConnection(_connectionString);

            // 🔹 Step 1: Approve Leave
            var parameters = new DynamicParameters();
            parameters.Add("@LeaveRequestId", leaveId);

            var result = await connection.ExecuteAsync(
                "sp_ManagerApproveLeave",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (result <= 0)
                return false;

            // 🔹 Step 2: Get Employee UserId
            var userId = await connection.QuerySingleOrDefaultAsync<int?>(
                "SELECT UserId FROM LeaveRequests WHERE LeaveRequestId = @LeaveId",
                new { LeaveId = leaveId }
            );

            if (!userId.HasValue)
                throw new Exception("Leave record not found.");

            // 🔹 Step 3: Get Username
            var userName = await connection.QuerySingleAsync<string>(
                "SELECT Username FROM Users WHERE Id = @Id AND IsDeleted = 0",
                new { Id = userId.Value }
            );

            // 🔹 Step 4: Get HR Users
            var hrIds = await connection.QueryAsync<int>(
                "SELECT Id FROM Users WHERE RoleId = 4 AND IsDeleted = 0"
            );

            // 🔹 Step 5: Create Notification
            var notificationId = await connection.QuerySingleAsync<int>(
                "sp_CreateNotification",
                new
                {
                    Title = "Leave Approved",
                    Message = $"{userName}'s leave has been approved by manager",
                    Type = "Leave"
                },
                commandType: CommandType.StoredProcedure
            );

            // 🔹 Step 6: Send to Employee
            await connection.ExecuteAsync(
                "sp_InsertUserNotification",
                new
                {
                    NotificationId = notificationId,
                    UserId = userId.Value
                },
                commandType: CommandType.StoredProcedure
            );

            // 🔹 Step 7: Send to HR
            foreach (var hrId in hrIds)
            {
                await connection.ExecuteAsync(
                    "sp_InsertUserNotification",
                    new
                    {
                        NotificationId = notificationId,
                        UserId = hrId
                    },
                    commandType: CommandType.StoredProcedure
                );
            }

            return true;
        }

        //public async Task<bool> ManagerRejectLeave(int leaveId)
        //{
        //    using var connection = new SqlConnection(_connectionString);

        //    var parameters = new DynamicParameters();
        //    parameters.Add("@LeaveRequestId", leaveId);

        //    var result = await connection.ExecuteAsync(
        //        "sp_ManagerRejectLeave",
        //        parameters,
        //        commandType: CommandType.StoredProcedure
        //    );

        //    return result > 0;
        //}

        public async Task<bool> ManagerRejectLeave(int leaveId)
        {
            using var connection = new SqlConnection(_connectionString);

            // 🔹 Step 1: Reject Leave
            var parameters = new DynamicParameters();
            parameters.Add("@LeaveRequestId", leaveId);

            var result = await connection.ExecuteAsync(
                "sp_ManagerRejectLeave",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (result <= 0)
                return false;

            // 🔹 Step 2: Get Employee UserId
            var userId = await connection.QuerySingleOrDefaultAsync<int?>(
                "SELECT UserId FROM LeaveRequests WHERE LeaveRequestId = @LeaveId",
                new { LeaveId = leaveId }
            );

            if (!userId.HasValue)
                throw new Exception("Leave record not found.");

            // 🔹 Step 3: Get Username
            var userName = await connection.QuerySingleAsync<string>(
                "SELECT Username FROM Users WHERE Id = @Id AND IsDeleted = 0",
                new { Id = userId.Value }
            );

            // 🔹 Step 5: Create Notification
            var notificationId = await connection.QuerySingleAsync<int>(
                "sp_CreateNotification",
                new
                {
                    Title = "Leave Rejected",
                    Message = $"{userName}'s leave has been rejected by manager",
                    Type = "Leave"
                },
                commandType: CommandType.StoredProcedure
            );

            // 🔹 Step 6: Send to Employee
            await connection.ExecuteAsync(
                "sp_InsertUserNotification",
                new
                {
                    NotificationId = notificationId,
                    UserId = userId.Value
                },
                commandType: CommandType.StoredProcedure
            );

            return true;
        }


        //----------------------------------HR------------------------------------


        //public async Task<bool> HrApproveLeave(int leaveId)
        //{
        //    try
        //    {
        //        using var connection = new SqlConnection(_connectionString);
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@LeaveRequestId", leaveId);

        //        var result = await connection.ExecuteAsync(
        //            "sp_HrApproveLeave",
        //            parameters,
        //            commandType: CommandType.StoredProcedure
        //        );

        //        return true;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<bool> HrApproveLeave(int leaveId)
        {
            using var connection = new SqlConnection(_connectionString);

            // 🔹 Step 1: Approve Leave
            var parameters = new DynamicParameters();
            parameters.Add("@LeaveRequestId", leaveId);

            var result = await connection.ExecuteAsync(
                "sp_HrApproveLeave",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (result == 0)
                return false;

            // 🔹 Step 2: Get Employee UserId
            var userId = await connection.QuerySingleOrDefaultAsync<int?>(
                "SELECT UserId FROM LeaveRequests WHERE LeaveRequestId = @LeaveId",
                new { LeaveId = leaveId }
            );

            if (!userId.HasValue)
                throw new Exception("Leave record not found.");

            // 🔹 Step 3: Get Username (optional)
            var userName = await connection.QuerySingleOrDefaultAsync<string>(
                "SELECT Username FROM Users WHERE Id = @Id AND IsDeleted = 0",
                new { Id = userId.Value }
            );

            userName ??= "User";

            // 🔹 Step 4: Create Notification
            var notificationId = await connection.QuerySingleAsync<int>(
                "sp_CreateNotification",
                new
                {
                    Title = "Leave Approved",
                    Message = $"Your leave request has been approved by HR",
                    Type = "Leave"
                },
                commandType: CommandType.StoredProcedure
            );

            // 🔹 Step 5: Send ONLY to employee
            await connection.ExecuteAsync(
                "sp_InsertUserNotification",
                new
                {
                    NotificationId = notificationId,
                    UserId = userId.Value
                },
                commandType: CommandType.StoredProcedure
            );

            return true;
        }


        //public async Task<bool> HrRejectLeave(int leaveId)
        //{
        //    using var connection = new SqlConnection(_connectionString);

        //    var parameters = new DynamicParameters();
        //    parameters.Add("@LeaveRequestId", leaveId);

        //    var result = await connection.ExecuteAsync(
        //        "sp_HrRejectLeave",
        //        parameters,
        //        commandType: CommandType.StoredProcedure
        //    );

        //    return result > 0;
        //}

        public async Task<bool> HrRejectLeave(int leaveId)
        {
            using var connection = new SqlConnection(_connectionString);

            // 🔹 Step 1: Reject Leave
            var parameters = new DynamicParameters();
            parameters.Add("@LeaveRequestId", leaveId);

            var result = await connection.ExecuteAsync(
                "sp_HrRejectLeave",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (result <= 0)
                return false;

            // 🔹 Step 2: Get Employee UserId
            var userId = await connection.QuerySingleOrDefaultAsync<int?>(
                "SELECT UserId FROM LeaveRequests WHERE LeaveRequestId = @LeaveId",
                new { LeaveId = leaveId }
            );

            if (!userId.HasValue)
                throw new Exception("Leave record not found.");

            // 🔹 Step 3: Create Notification
            var notificationId = await connection.QuerySingleAsync<int>(
                "sp_CreateNotification",
                new
                {
                    Title = "Leave Rejected",
                    Message = "Your leave request has been rejected by HR",
                    Type = "Leave"
                },
                commandType: CommandType.StoredProcedure
            );

            // 🔹 Step 4: Send ONLY to employee
            await connection.ExecuteAsync(
                "sp_InsertUserNotification",
                new
                {
                    NotificationId = notificationId,
                    UserId = userId.Value
                },
                commandType: CommandType.StoredProcedure
            );

            return true;
        }

        public async Task<bool> InitilizeUsersLeaveBalance()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.ExecuteAsync(
               "sp_InitializeUserLeaveBalance",
               commandType: CommandType.StoredProcedure
           );

                return true;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<UserLeaveBalanceDto>> getUserLeaveBalance(int userId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                var result = await connection.QueryAsync<UserLeaveBalanceDto>(
                    "sp_getUserLeaveBalance",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
