using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAC.Common.Sage;
using PAC.Common.Tracking;
using PAC.DataLayer.API;
using PAC.DataLayer.Sage;
using PAC.Web.API.Services;
using System.Text;

namespace PAC.Web.API.Controllers
{
    /// <summary>
    /// Tracking reads for the PAC Track portal. Each route here mirrors the
    /// same route on PAC.SageWeb and forwards to it unchanged: the route
    /// values are checked, the query string goes through as given (the
    /// inner service validates the dates), and the answer comes back byte
    /// for byte. Dates are ISO (yyyy-MM-dd).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TrackingController(ILogger<TrackingController> logger) : ControllerBase
    {
        [HttpGet("awb/{custno}/open")]
        public ActionResult<List<AirWaybillItem>> GetOpenAWBs(string custno)
        {
            try
            {
                List<AirWaybillItem> list = TrackingAPI.GetOpenAWBs(custno);

                return Ok(list);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "GetOpenAWBs failed for {Path}", Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, "The process encountered an error");
            }
        }

        [HttpGet("awb/{sono}/manifest")]
        public ActionResult<List<ManifestItem>> GetManifestItems(string sono)
        {
            try
            {

                List<ManifestItem> list = TrackingAPI.GetManifestItems(sono);

                return Ok(list);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "GetManifestItems failed for {Path}", Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, "The process encountered an error");
            }
        }

        [HttpGet("awb/recent/{custNo}/{count}")]
        public ActionResult<List<AirWaybillItem>> GetRecentAWBs(string custNo, int count)
        {
            try
            {
                List<AirWaybillItem> list = AirWaybillAPI.GetRecentAirbills(custNo, count);

                return Ok(list);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Tracking read failed for {Path}", Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, "The process encountered an error");
            }
        }

        [HttpGet("awb/shipment/{sono}/details")]
        public ActionResult<List<ShipmentItem>> GetShipmentDetails(string sono)
        {
            try
            {
                List<ShipmentItem> list = AirWaybillAPI.GetShipmentDetails(sono);

                return Ok(list);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Tracking read failed for {Path}", Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, "The process encountered an error");
            }
        }

        // POST api/<TrackingController>
        [HttpPost]
        public ActionResult<string> Post([FromBody] string value)
        {
            return BadRequest("Not implemented");
        }
    }
 }
