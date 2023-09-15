using ReportService.Data.Models;

namespace ReportService.Models
{
    public class EmployeeWithSalary
    {
        public EmployeeWithSalary(Employee employee)
        { Employee = employee; }

        public Employee Employee { get; set; }

        public int Salary { get; set; }
    }
}