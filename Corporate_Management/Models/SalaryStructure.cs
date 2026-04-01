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
        public DateTime CreatedDate { get; set; }
    }
    public class createSalaryStructure
    {
      
        public int UserId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal OtherAllowance { get; set; }
    
    }

    public class updateSalaryStructure
    {
        public int SalaryStructureId { get; set; }
        public decimal BasicSalary { get; set; }  
        public decimal OtherAllowance { get; set; }
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
        public DateTime CreatedDate { get; set; }
    }

    public class GeneratePayroll
    {
        public int UserId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TaxDeduction { get; set; }
    }

    public class getPayrollData
    {
        public int PayrollId { get; set; }
        public int UserId { get; set; }
        public int SalaryStructureId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal OtherAllowance { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public string Status { get; set; }
        public DateTime GeneratedDate { get; set; }
    }

    public class getPayrollDataDto
    {
        public int PayrollId { get; set; }
        public int UserId { get; set; }
        public int SalaryStructureId { get; set; }
        public string UserName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal OtherAllowance { get; set; }
        public decimal TaxDeduction { get; set; }
        public decimal OtherDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public string Status { get; set; }
        public DateTime GeneratedDate { get; set; }
    }

}
