using Microsoft.AspNetCore.Mvc;
using PAC.Common.Sage;
using PAC.Common.Tracking;
using PAC.DataLayer.Sage;

namespace PAC.SageWeb.Controllers
{
    /// <summary>
    /// Tracking reads for the PAC Track portal, on the routes and in the
    /// shapes the portal client already uses (the same shapes the August
    /// PACTRAK service produced). Dates are ISO (yyyy-MM-dd).
    /// </summary>
    [ApiController]
    public class TrackingController(ILogger<TrackingController> logger) : ControllerBase
    {
        /// <summary>Orders dated in the range; custno narrows to one customer.</summary>
        [HttpGet("api/awbs")]
        public ActionResult<List<AwbHeader>> AwbsByDateRange([FromQuery] DateTime from, [FromQuery] DateTime to, [FromQuery] string? custno = null)
        {
            if (to < from) return BadRequest("to is before from");
            if (custno is not null && !Input.IsKey(custno)) return BadRequest("custno");
            return Try(() => TrackingDL.GetHeadersByDateRange(from, to, custno));
        }

        /// <summary>A customer's open orders.</summary>
        [HttpGet("api/customers/{custno}/awbs/open")]
        public ActionResult<List<AwbHeader>> OpenAwbs(string custno)
        {
            if (!Input.IsKey(custno)) return BadRequest("custno");
            return Try(() => TrackingDL.GetOpenHeaders(custno));
        }

        [HttpGet("api/awbs/{sono}/manifest")]
        public ActionResult<List<ManifestItem>> Manifest(string sono)
        {
            if (!Input.IsKey(sono)) return BadRequest("sono");
            return Try(() => TrackingDL.GetManifestItems(sono));
        }

        [HttpGet("api/awbs/{sono}/pod")]
        public ActionResult<List<PODHistoryItem>> PodForAwb(string sono)
        {
            if (!Input.IsKey(sono)) return BadRequest("sono");
            return Try(() => TrackingDL.GetPodsForAwb(sono));
        }

        [HttpGet("api/pod")]
        public ActionResult<List<PODHistoryItem>> PodByDateRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (to < from) return BadRequest("to is before from");
            return Try(() => TrackingDL.GetPodsByDateRange(from, to));
        }

        [HttpGet("api/invoices")]
        public ActionResult<List<InvoiceDetail>> InvoicesByDateRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (to < from) return BadRequest("to is before from");
            return Try(() => TrackingDL.GetInvoicesByDateRange(from, to));
        }

        private ActionResult<T> Try<T>(Func<T> read)
        {
            try
            {
                return Ok(read());
            }
            catch (Exception ex)
            {
                // the detail goes to the log; the caller gets a plain answer
                logger.LogError(ex, "Tracking read failed for {Path}", Request.Path);
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "The operations database did not answer.");
            }
        }
    }

    /// <summary>Route and query values that reach the data layer: customer
    /// numbers, sales order numbers and invoice numbers are letters,
    /// digits and dashes only.</summary>
    public static class Input
    {
        public static bool IsKey(string value)
            => !string.IsNullOrWhiteSpace(value) && value.Length <= 32 && value.All(c => char.IsLetterOrDigit(c) || c == '-');
    }
}
