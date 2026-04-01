using Corporate_Management.Models;
using Corporate_Management.Repositories.IRepositories;
using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.EMMA;
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

        public async Task<int> CreateSalaryStructure(createSalaryStructure model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameter = new DynamicParameters();
                parameter.Add("@UserId", model.UserId);
                parameter.Add("@BasicSalary", model.BasicSalary);     
                parameter.Add("@OtherAllowance", model.OtherAllowance);            
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
                parameter.Add("@OtherAllowance", model.OtherAllowance);
                var result = await connection.ExecuteAsync("sp_UpdateSalaryStructure", parameter,commandType: CommandType.StoredProcedure);
                return result > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteSalaryStructure(int SalaryStructureId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@SalaryStructureId", SalaryStructureId);

                var rows = await connection.ExecuteAsync(
                    "sp_DeleteSalaryStructure",
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
                parameter.Add("@TaxDeduction ", model.TaxDeduction);
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

        public async Task<getPayrollData> getPayrollbyUserId(int userId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                var parameter = new DynamicParameters();
                parameter.Add("@UserId", userId);

                var data = await connection.QueryFirstOrDefaultAsync<getPayrollData>("sp_getPayrollByUserId", parameter, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<getPayrollData> getPayrollbyPayrollId(int PayrollId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                var parameter = new DynamicParameters();
                parameter.Add("@PayrollId", PayrollId);

                var data = await connection.QueryFirstOrDefaultAsync<getPayrollData>("sp_getPayrollByPayrollId", parameter, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> deletePayroll(int PayrollId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@PayrollId", PayrollId);

                var rows = await connection.ExecuteAsync(
                    "sp_DeletePayroll",
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

        public async Task<bool> GeneratePayrollForAll(int month,int year)
        {   
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameter = new DynamicParameters();
                parameter.Add("@Month", month);
                parameter.Add("@Year", year);         

                var data=await connection.QuerySingleAsync<int>("sp_generatePayrollForAllUsers",parameter,commandType: CommandType.StoredProcedure);
                return data == 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<getPayrollDataDto>> GetAllPayrollByMonth(int month)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameter = new DynamicParameters();
                parameter.Add("@Month", month);

                return await connection.QueryAsync<getPayrollDataDto>("sp_GetAllPayrollByMonth", parameter, commandType: CommandType.StoredProcedure);
              
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
