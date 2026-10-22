using Microsoft.AspNetCore.Mvc;
using PAC.Common.Sage;
using PAC.DataLayer.Sage;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PAC.SageWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TariffController : ControllerBase
    {
        // GET: api/<TariffController>
        [HttpGet]
        public ActionResult Get()
        {
            return BadRequest();
        }

        // GET api/<TariffController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return BadRequest();
        }

        [HttpGet("min/{custNo}")]
        public ActionResult<List<Tariff>> GetMinTariffs(string custNo)
        {
            try
            {
                List<Tariff> list = TariffDL.GetMinTariffs(custNo);

                return Ok(list);
            }

            catch (Exception ex)
            {
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "The operations database did not answer.");
            }
        }

        [HttpGet("{custNo}/{origin}/{dest}")]
        public ActionResult<List<Tariff>> GetTariffsByOriginDest(string custNo, string origin, string dest)
        {
            try
            {
                List<Tariff> list = TariffDL.GetTariffsByOriginDest(custNo, origin, dest);

                return Ok(list);
            }

            catch (Exception ex)
            {
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "The operations database did not answer.");
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
