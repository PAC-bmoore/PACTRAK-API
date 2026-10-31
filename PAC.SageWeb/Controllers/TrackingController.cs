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
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController(ILogger<TrackingController> logger) : ControllerBase
    {
        [HttpGet("awb/{custno}/open")]
        public ActionResult<List<AirWaybillItem>> GetOpenAWBs(string custno)
        {
            if (!Input.IsKey(custno)) return BadRequest("customer number");
            try
            {

                List<AirWaybillItem> list = AirWaybillDL.GetOpenAirWaybills(custno);

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
            if (!Input.IsSono(sono)) return BadRequest("waybill number");
            try
            {
                
                List<ManifestItem> list = ManifestDL.GetManifestItems(sono);

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
            if (!Input.IsKey(custNo)) return BadRequest("customer number");
            if (count < 1)
                count = 1;
            if (count > 200)
                count = 200;
            try
            {
                List<AirWaybillItem> list = AirWaybillDL.GetRecentAirbills(custNo, count);

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
            if (!Input.IsSono(sono)) return BadRequest("waybill number");
            try
            {
                List<ShipmentItem> list = AirWaybillDL.GetShipmentDetails(sono);

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
