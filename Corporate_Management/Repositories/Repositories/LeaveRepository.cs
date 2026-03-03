using Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Dapper;
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

        public async Task<int> CreateLeave(Leave leavemodel)
        {
            try 
            {
                using var connection = new SqlConnection(_connectionString);
                var parameter = new DynamicParameters();
                parameter.Add("@UserId", leavemodel.UserId);    
                parameter.Add("@RequestType", leavemodel.RequestType);
                parameter.Add("@FromDate", leavemodel.FromDate.ToDateTime(TimeOnly.MinValue));
                parameter.Add("@ToDate", leavemodel.ToDate.ToDateTime(TimeOnly.MinValue));
                parameter.Add("@TotalDays", leavemodel.TotalDays);
                parameter.Add("@Session", leavemodel.Session);
                parameter.Add("@Reason", leavemodel.Reason);
                parameter.Add("@HandoverTo", leavemodel.HandoverTo);
                parameter.Add("@Status", leavemodel.Status);
                parameter.Add("@newId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("sp_ApplyLeave", parameter, commandType: CommandType.StoredProcedure);
                return parameter.Get<int>("@newId");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
