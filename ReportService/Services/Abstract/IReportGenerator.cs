using ReportService.Models;
using System.Collections.Generic;

namespace ReportService.Services.Abstract
{
    public interface IReportGenerator
    {
        string Generate(int month, int year, List<EmployeeWithSalary> employeeWithSalary);
    }
}