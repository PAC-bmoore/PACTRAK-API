using Microsoft.AspNetCore.Mvc;
using PAC.Common.Sage;
using Input = PAC.Common.Tracking.Input;
using PAC.DataLayer.Sage;
using PAC.DataLayer.API;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PAC.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TariffController(ILogger<TariffController> logger) : ControllerBase
    {
        // GET: api/<TariffController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TariffController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("min/{custNo}")]
        public ActionResult<List<Tariff>> GetMinTariffs(string custNo)
        {
            if (!Input.IsKey(custNo)) return BadRequest("customer number");
            try
            {
                List<Tariff> list = TariffAPI.GetMinTariffs(custNo);

                return Ok(list);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "GetMinTariffs failed for {Path}", Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, "The process encountered an error");
            }
        }

        [HttpGet("{custNo}/{origin}/{dest}")]
        public ActionResult<List<Tariff>> GetTariffsByOriginDest(string custNo, string origin, string dest)
        {
            if (!Input.IsKey(custNo)) return BadRequest("customer number");
            if (!Input.IsKey(origin)) return BadRequest("origin");
            if (!Input.IsKey(dest)) return BadRequest("destination");
            try
            {
                List<Tariff> list = TariffAPI.GetTariffsByOriginDest(custNo, origin, dest);

                return Ok(list);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "GetTariffsByOriginDest failed for {Path}", Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, "The process encountered an error");
            }
        }


        // POST api/<TariffController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TariffController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TariffController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
