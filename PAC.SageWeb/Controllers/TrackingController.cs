using Microsoft.AspNetCore.Mvc;
using PAC.Common.Sage;
using PAC.DataLayer.Sage;
using System.Net;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PAC.SageWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        // GET: api/<TrackingController>
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "value";
        }

        // GET api/<TrackingController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("recent/{custNo}/{count}")]
        public ActionResult<List<AirWaybillItem>> GetMinTariffs(string custNo, int count)
        {
            try
            {
                List<AirWaybillItem> list = AirWaybillDL.GetRecentAirbills(custNo, count);

                return Ok(list);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // POST api/<TrackingController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TrackingController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TrackingController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
