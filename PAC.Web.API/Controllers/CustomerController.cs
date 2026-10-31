using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAC.Common.Sage;
using Input = PAC.Common.Tracking.Input;
using PAC.DataLayer.API;
using PAC.DataLayer.Sage;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PAC.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Cargo.Read")]
    public class CustomerController(ILogger<CustomerController> logger) : ControllerBase
    {
        // GET: api/<CustomerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CustomerController>/5
        [HttpGet("{custNum}")]
        public ActionResult<Customer> Get(string custNum)
        {
            if (!Input.IsKey(custNum)) return BadRequest("customer number");
            try
            {
                Customer item = CustomerAPI.GetCustomer(custNum);
                return Ok(item);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Customer Get failed for {Path}", Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, "The process encountered an error");
            }
        }

        // GET api/<CustomerController>/summary/5
        [HttpGet("summary/{custNum}")]
        public ActionResult<CustomerSummary> GetSummary(string custNum)
        {
            if (!Input.IsKey(custNum)) return BadRequest("customer number");
            try
            {
                CustomerSummary item = CustomerAPI.GetCustomerSummary(custNum);
                if (item == null) return NotFound();
                return Ok(item);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Customer GetSummary failed for {Path}", Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, "The process encountered an error");
            }
        }

        // GET api/<CustomerController>/search/abc
        [HttpGet("search/{text}")]
        public ActionResult<List<CustomerSummary>> Search(string text)
        {
            if (!Input.IsSearch(text)) return BadRequest("search text");
            try
            {
                List<CustomerSummary> list = CustomerAPI.SearchCustomers(text);
                return Ok(list);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Customer Search failed for {Path}", Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, "The process encountered an error");
            }
        }

        // POST api/<CustomerController>
        [HttpPost]
        [Authorize(Policy = "Customer.Write")]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Customer.Write")]
        public ActionResult<string> Put(int id, [FromBody] Customer value)
        {
            try
            {
                CustomerAPI.UpdateCustomer(value);
                return Ok("Operation Completed Successfully");
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Customer Put failed for {Path}", Request.Path);
                return StatusCode(StatusCodes.Status500InternalServerError, "The process encountered an error");
            }
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Customer.Write")]
        public void Delete(int id)
        {
        }
    }
}
