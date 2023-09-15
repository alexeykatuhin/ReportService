using ReportService.Data.Models;

namespace ReportService.Data
{
    public interface IRepository
    {
        Task<IEnumerable<Employee>> GetEmployees(CancellationToken cancellationToken);
    }
}