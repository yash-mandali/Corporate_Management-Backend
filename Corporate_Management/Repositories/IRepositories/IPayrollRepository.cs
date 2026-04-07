using Corporate_Management.Models;

namespace Corporate_Management.Repositories.IRepositories
{
    public interface IPayrollRepository
    {
        Task<int> CreateSalaryStructure(createSalaryStructure model);
        Task<bool> UpdateSalaryStructure(updateSalaryStructure model);
        Task<int> DeleteSalaryStructure(int SalaryStructureId);
        Task<GetDataSalaryStructure> GetSalaryStructureByUserId(int userId);
        Task<IEnumerable<GetDataSalaryStructure>> GetAllSalaryStructure();
        Task<int> GeneratePayroll(GeneratePayroll model);
        Task<getPayrollData> getPayrollbyUserId(int userId);
        Task<getPayrollData> getPayrollbyPayrollId(int PayrollId);
        Task<int> deletePayroll(int PayrollId);
        Task<bool> GeneratePayrollForAll(int month, int year);
        Task<IEnumerable<getPayrollDataDto>> GetAllPayrollByMonth(int month);
        Task<int> markAsPaid(int PayrollId);
    }
}
