using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PAC.Common.Sage;
using Input = PAC.Common.Tracking.Input;
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
            if (!Input.IsKey(custNum)) return BadRequest("customer number");
            try
            {
                Customer item = CustomerDL.GetCustomer(custNum);
                return Ok(item);
            }

            catch (Exception ex)
            {
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "The operations database did not answer.");
            }
        }

        // GET api/<CustomerController>/summary/5
        [HttpGet("summary/{custNum}")]
        [AllowAnonymous]
        public ActionResult<CustomerSummary> GetSummary(string custNum)
        {
            if (!Input.IsKey(custNum)) return BadRequest("customer number");
            try
            {
                CustomerSummary item = CustomerDL.GetCustomerSummary(custNum);
                if (item == null) return NotFound();
                return Ok(item);
            }

            catch (Exception ex)
            {
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "The operations database did not answer.");
            }
        }

        // GET api/<CustomerController>/search/abc
        [HttpGet("search/{text}")]
        [AllowAnonymous]
        public ActionResult<List<CustomerSummary>> Search(string text)
        {
            if (!Input.IsSearch(text)) return BadRequest("search text");
            try
            {
                List<CustomerSummary> list = CustomerDL.SearchCustomers(text);
                return Ok(list);
            }

            catch (Exception ex)
            {
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "The operations database did not answer.");
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
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "The operations database did not answer.");
            }
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
