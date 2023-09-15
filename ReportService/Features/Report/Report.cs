using MediatR;
using Microsoft.Extensions.Options;
using ReportService.Data;
using ReportService.Domain;
using ReportService.Models;
using ReportService.Services.Abstract;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReportService.Features.Report
{
    public class Report
    {
        public record Command(int Year, int Month) : IRequest<string>;

        public class Handler : IRequestHandler<Command, string>
        {
            private readonly IRepository _repository;
            private readonly ISalaryService _salaryService;
            private readonly Settings _settings;
            private readonly IReportGenerator _reportGenerator;

            public Handler(IRepository repository, ISalaryService salaryService, IOptions<Settings> options, IReportGenerator reportGenerator)
            {
                _repository = repository;
                _salaryService = salaryService;
                _settings = options.Value;
                _reportGenerator = reportGenerator;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                var employees = await _repository.GetEmployees(cancellationToken);
                var employeesWithSalary = employees.Select(x => new EmployeeWithSalary(x));

                Parallel.ForEach(
                    employeesWithSalary,
                    new ParallelOptions { MaxDegreeOfParallelism = _settings.DegreeOfParallelism },
                    async item => item.Salary = await _salaryService.GetSalary(item.Employee.Inn, cancellationToken));

                var file = _reportGenerator.Generate(request.Month, request.Year, employeesWithSalary.ToList());

                return file;
            }
        }
    }
}