using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PAC.Common.Sage;
using PAC.DataLayer.Sage;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PAC.SageWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        // GET: api/<CustomerController>
        [HttpGet]
        public ActionResult Get()
        {
            return BadRequest();
        }

        // GET api/<CustomerController>/5
        [HttpGet("{custNum}")]
        public ActionResult<Customer> Get(string custNum)
        {
            try
            {
                Customer item = CustomerDL.GetCustomer(custNum);
                return Ok(item);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // POST api/<CustomerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, [FromBody] Customer value)
        {
            try
            {
                CustomerDL.UpdateCustomer(value);
                return Ok("Operation Completed Successfully");
            }

            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
