using ReportService.Domain;
using ReportService.Models;
using ReportService.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportService.Services.Implementations
{
    public class ReportGenerator : IReportGenerator
    {
        private const string SEPARATOR = "---";
        private const string SUM_BY_DEPARTMENT = "Всего по отделу";

        public string Generate(int month, int year, List<EmployeeWithSalary> employeeWithSalary)
        {
            var monthString = MonthNameResolver.MonthName.GetName(year, month).FirstCharToUpper();
            var sb = new StringBuilder($"{monthString} {year}");
            sb.AppendLine(Environment.NewLine);

            var departments = employeeWithSalary.GroupBy(x => x.Employee.Department.Name).Select(x => (x.Key, x.ToArray())).OrderBy(x => x.Key);

            foreach (var department in departments)
            {
                PrintDepartment(sb, department.Key, department.Item2);
            }

            sb.AppendLine(SEPARATOR);
            sb.AppendLine();
            var sum = employeeWithSalary.Sum(x => x.Salary);
            sb.Append($"Всего по предприятию\t{sum}р");

            return sb.ToString();
        }

        private void PrintDepartment(StringBuilder sb, string depatment, EmployeeWithSalary[] employees)
        {
            sb.AppendLine(SEPARATOR);
            sb.AppendLine(depatment);
            foreach (var employee in employees.OrderBy(x=>x.Employee.Name))
            {
                sb.AppendLine();
                sb.AppendLine($"{employee.Employee.Name}\t{employee.Salary}р");
            }
            sb.AppendLine();
            var sum = employees.Sum(x => x.Salary);
            sb.AppendLine($"{SUM_BY_DEPARTMENT}\t{sum}р");
            sb.AppendLine();
        }
    }
}