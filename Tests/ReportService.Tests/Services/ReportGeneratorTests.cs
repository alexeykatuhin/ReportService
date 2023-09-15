using FluentAssertions;
using ReportService.Data.Models;
using ReportService.Models;
using ReportService.Services.Implementations;

namespace ReportService.Tests.Services
{
    public class ReportGeneratorTests
    {
        [TestCaseSource(nameof(TestCases))]
        public void GenerateReport_ShoudBeCorrect(int month, int year, List<EmployeeWithSalary> employeeWithSalary, string report)
        {
            // Arrange
            var rg = new ReportGenerator();

            // Act
            var result = rg.Generate(month, year, employeeWithSalary);

            // Assert
            result.Should().Be(report);
        }

        public static object[] TestCases =
        {
            new object[]
            {
                1,2001, new List<EmployeeWithSalary>(),
                "Январь 2001\r\n" +
                "\r\n" +
                "---\r\n" +
                "\r\n" +
                "Всего по предприятию\t0р"
            },
            new object[]
            {
                2,2001,
                new List<EmployeeWithSalary>
                {
                    new EmployeeWithSalary(new Employee{ Name = "Алексей", Department = new Department{ Name = "Закупки" } }){ Salary = 100 },
                    new EmployeeWithSalary(new Employee{ Name = "Иван", Department = new Department{ Name = "ИТ" } }){ Salary = 200 },
                    new EmployeeWithSalary(new Employee{ Name = "Сергей", Department = new Department{ Name = "ИТ" } }){ Salary = 300 }
                },
                "Февраль 2001\r\n" +
                "\r\n" +
                "---\r\n" +
                "Закупки\r\n" +
                "\r\n" +
                "Алексей\t100р\r\n" +
                "\r\n" +
                "Всего по отделу\t100р\r\n" +
                "\r\n" +
                "---\r\n" +
                "ИТ\r\n" +
                "\r\n" +
                "Иван\t200р\r\n" +
                "\r\n" +
                "Сергей\t300р\r\n" +
                "\r\n" +
                "Всего по отделу\t500р\r\n" +
                "\r\n" +
                "---\r\n" +
                "\r\n" +
                "Всего по предприятию\t600р"
            },
        };
    }
}