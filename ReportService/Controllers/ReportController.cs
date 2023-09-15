using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportService.Domain;
using ReportService.Features.Report;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{year}/{month}")]
        public async Task<IActionResult> Download(int year, int month, CancellationToken cancellationToken)
        {
            if (!DateHepler.IsValidDate(year, month))
            {
                return BadRequest();
            }

            var file = await _mediator.Send(new Report.Command(year, month));

            return File(Encoding.UTF8.GetBytes(file),
             "application/octet-stream",
              string.Format("report.txt"));
        }
    }
}