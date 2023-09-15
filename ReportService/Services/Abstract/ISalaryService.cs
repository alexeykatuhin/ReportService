using System.Threading;
using System.Threading.Tasks;

namespace ReportService.Services.Abstract
{
    public interface ISalaryService
    {
        Task<int> GetSalary(string inn, CancellationToken cancellationToken);
    }
}