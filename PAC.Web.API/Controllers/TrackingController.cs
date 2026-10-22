using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAC.Common.Tracking;
using PAC.Web.API.Services;

namespace PAC.Web.API.Controllers
{
    /// <summary>
    /// Tracking reads for the PAC Track portal. Each route here mirrors the
    /// same route on PAC.SageWeb and forwards to it unchanged: the route
    /// values are checked, the query string goes through as given (the
    /// inner service validates the dates), and the answer comes back byte
    /// for byte. Dates are ISO (yyyy-MM-dd).
    /// </summary>
    [ApiController]
    [Authorize]
    public class TrackingController(InnerApi inner) : ControllerBase
    {
        /// <summary>Orders dated in the range; custno narrows to one customer.</summary>
        [HttpGet("api/awbs")]
        public Task<IActionResult> AwbsByDateRange(CancellationToken ct)
            => Forward("api/awbs", ct);

        /// <summary>A customer's open orders.</summary>
        [HttpGet("api/customers/{custno}/awbs/open")]
        public Task<IActionResult> OpenAwbs(string custno, CancellationToken ct)
            => Input.IsKey(custno) ? Forward($"api/customers/{custno}/awbs/open", ct) : Rejected("custno");

        [HttpGet("api/awbs/{sono}/manifest")]
        public Task<IActionResult> Manifest(string sono, CancellationToken ct)
            => Input.IsKey(sono) ? Forward($"api/awbs/{sono}/manifest", ct) : Rejected("sono");

        [HttpGet("api/awbs/{sono}/pod")]
        public Task<IActionResult> PodForAwb(string sono, CancellationToken ct)
            => Input.IsKey(sono) ? Forward($"api/awbs/{sono}/pod", ct) : Rejected("sono");

        [HttpGet("api/pod")]
        public Task<IActionResult> PodByDateRange(CancellationToken ct)
            => Forward("api/pod", ct);

        [HttpGet("api/invoices")]
        public Task<IActionResult> InvoicesByDateRange(CancellationToken ct)
            => Forward("api/invoices", ct);

        private async Task<IActionResult> Forward(string path, CancellationToken ct)
        {
            var answer = await inner.GetAsync(path + Request.QueryString.Value, ct);
            return new ContentResult
            {
                StatusCode = answer.Status,
                ContentType = answer.ContentType,
                Content = Encoding.UTF8.GetString(answer.Body),
            };
        }

        private Task<IActionResult> Rejected(string what)
            => Task.FromResult<IActionResult>(BadRequest(what));
    }
}
