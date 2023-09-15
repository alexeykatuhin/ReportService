using ReportService.Consts;
using ReportService.Services.Abstract;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ReportService.Services.Implementations
{
    public class SalaryService : ISalaryService
    {
        private readonly HttpClient _accountingHttpClient;
        private readonly HttpClient _salaryHttpClient;

        public SalaryService(IHttpClientFactory clientFactory)
        {
            _accountingHttpClient = clientFactory.CreateClient(Constants.ACCOUNTING_HTTP_CLIENT);
            _salaryHttpClient = clientFactory.CreateClient(Constants.SALARY_HTTP_CLIENT);
        }

        public async Task<int> GetSalary(string inn, CancellationToken cancellationToken)
        {
            var accountingCode = await _accountingHttpClient.GetStringAsync(inn, cancellationToken);
            var salaryResponse = await _salaryHttpClient.PostAsJsonAsync(inn, new { accountingCode }, cancellationToken);
            var salary = int.Parse(await salaryResponse.Content.ReadAsStringAsync());
            return salary;
        }
    }
}