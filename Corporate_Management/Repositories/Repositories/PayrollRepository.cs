using Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Corporate_Management.Repositories.Repositories
{
    public class PayrollRepository:IPayrollRepository
    {
        private readonly string _connectionString;
        public PayrollRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("dbconnection");
        }

        public async Task<int> CreateSalaryStructure(SalaryStructure model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameter = new DynamicParameters();
                parameter.Add("@UserId", model.UserId);
                parameter.Add("@BasicSalary", model.BasicSalary);
                parameter.Add("@HRA", model.HRA);
                parameter.Add("@OtherAllowance", model.OtherAllowance);
                parameter.Add("@PF", model.PF);
                parameter.Add("@Tax", model.Tax);
                parameter.Add("@newId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await connection.ExecuteAsync("sp_CreateSalaryStructure", parameter, commandType: CommandType.StoredProcedure);
                return parameter.Get<int>("@newId");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateSalaryStructure(updateSalaryStructure model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameter = new DynamicParameters();
                parameter.Add("@SalaryStructureId", model.SalaryStructureId);
                parameter.Add("@BasicSalary", model.BasicSalary);
                parameter.Add("@HRA", model.HRA);
                parameter.Add("@OtherAllowance", model.OtherAllowance);
                parameter.Add("@PF", model.PF);
                parameter.Add("@Tax", model.Tax);

                var result = await connection.QuerySingleAsync<int>("sp_UpdateSalaryStructure",parameter,commandType: CommandType.StoredProcedure);
                return result == 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GetDataSalaryStructure> GetSalaryStructureByUserId(int userId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                var parameter = new DynamicParameters();
                parameter.Add("@UserId", userId);

                var data= await connection.QueryFirstOrDefaultAsync<GetDataSalaryStructure>("sp_GetSalaryStructureByUserId",parameter,commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<GetDataSalaryStructure>> GetAllSalaryStructure()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                return await connection.QueryAsync<GetDataSalaryStructure>("sp_GetAllSalaryStructure",commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GeneratePayroll(GeneratePayroll model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                var parameter = new DynamicParameters();
                parameter.Add("@UserId", model.UserId);
                parameter.Add("@Month", model.Month);
                parameter.Add("@Year", model.Year);
                parameter.Add("@newId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "sp_GeneratePayroll",
                    parameter,
                    commandType: CommandType.StoredProcedure
                );

                return parameter.Get<int>("@newId");
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
