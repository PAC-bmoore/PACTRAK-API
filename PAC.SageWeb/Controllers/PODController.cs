using Microsoft.AspNetCore.Mvc;
using PAC.Common.Sage;
using Input = PAC.Common.Tracking.Input;
using PAC.DataLayer.Sage;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PAC.SageWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PODController : ControllerBase
    {
        // GET api/<PODController>/5
        [HttpGet("{awb}")]
        public ActionResult<PODItem> Get(string awb)
        {
            if (!Input.IsSono(awb)) return BadRequest("waybill number");
            try
            {
                List<PODItem> list = PODDL.GetPODInformation(awb);
                return Ok(list);
            }

            catch (Exception ex)
            {
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "The operations database did not answer.");
            }
        }

        [HttpGet("{custNo}/{minDate}/{maxDate}")]
        public ActionResult<PODHistoryItem> GetPODsByCustInvNo(string custNo, DateTime minDate, DateTime maxDate)
        {
            if (!Input.IsKey(custNo)) return BadRequest("customer number");
            try
            {
                List<PODHistoryItem> list = PODDL.GetPODHistory(custNo, minDate, maxDate);
                return Ok(list);
            }

            catch (Exception ex)
            {
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "The operations database did not answer.");
            }
        }

        // POST api/<PODController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PODController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PODController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
