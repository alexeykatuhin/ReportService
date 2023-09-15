using Dapper;
using Microsoft.Data.SqlClient;
using ReportService.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Data
{
    public class Repository : IRepository
    {
        private readonly DbOptions _dbOptions;

        public Repository(DbOptions dbOptions) => _dbOptions = dbOptions;

        public async Task<IEnumerable<Employee>> GetEmployees(CancellationToken cancellationToken)
        {
            using var connection = _dbOptions.CreateConnection();
            {
                var sql = @"SELECT e.Name, e.Inn, d.Name
                FROM Employees e 
                LEFT JOIN Deps d ON e.Departmentid = d.Id";

                var employees = await connection.QueryAsync<Employee, Department, Employee>(sql, (employee, department) => {
                    employee.Department = department;
                    return employee;
                },
                splitOn: "CategoryID");

                return employees;
            }
        }
    }
}
