using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAC.Common.Sage;
using PAC.DataLayer.API;
using PAC.DataLayer.Sage;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PAC.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PODController : ControllerBase
    {
        // GET: api/<PODController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PODController>/5
        [HttpGet("{awb}")]
        public ActionResult<PODItem> Get(string awb)
        {
            try
            {
                List<PODItem> list = PODAPI.GetPODInformation(awb);
                return Ok(list);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("{custNo}/{minDate}/{maxDate}")]
        public ActionResult<PODHistoryItem> GetPODsByCustInvNo(string custNo, DateTime minDate, DateTime maxDate)
        {
            try
            {
                List<PODHistoryItem> list = PODAPI.GetPODHistory(custNo, minDate, maxDate);
                return Ok(list);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
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
