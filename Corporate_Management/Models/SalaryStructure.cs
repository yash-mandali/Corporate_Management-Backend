namespace Corporate_Management.Models
{
    public class SalaryStructure
    {
        public int SalaryStructureId { get; set; }
        public int UserId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HRA { get; set; }
        public decimal OtherAllowance { get; set; }
        public decimal PF { get; set; }
        public decimal Tax { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class updateSalaryStructure
    {
        public int SalaryStructureId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HRA { get; set; }
        public decimal OtherAllowance { get; set; }
        public decimal PF { get; set; }
        public decimal Tax { get; set; }
    }

    public class GetDataSalaryStructure
    {
        public int SalaryStructureId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HRA { get; set; }
        public decimal OtherAllowance { get; set; }
        public decimal PF { get; set; }
        public decimal Tax { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class GeneratePayroll
    {
        public int UserId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
