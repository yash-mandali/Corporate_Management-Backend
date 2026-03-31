using Corporate_Management.Models;

namespace Corporate_Management.Repositories.IRepositories
{
    public interface IPayrollRepository
    {
        Task<int> CreateSalaryStructure(SalaryStructure model);
        Task<bool> UpdateSalaryStructure(updateSalaryStructure model);
        Task<GetDataSalaryStructure> GetSalaryStructureByUserId(int userId);
        Task<IEnumerable<GetDataSalaryStructure>> GetAllSalaryStructure();
        Task<int> GeneratePayroll(GeneratePayroll model);
    }
}
