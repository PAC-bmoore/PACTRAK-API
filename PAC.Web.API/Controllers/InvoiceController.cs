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
    public class InvoiceController : ControllerBase
    {
        // GET: api/<InvoiceController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<InvoiceController>/5
        [HttpGet("{id}")]
        public ActionResult<Invoice> Get(int id)
        {
            try
            {
                Invoice item = InvoiceAPI.GetTrackingInvoiceBySono(id.ToString());

                return Ok(item);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("Detail/{id}/{custNo}")]
        public ActionResult<List<InvoiceItem>> GetInvoiceLineItems(int id, string custNo)
        {
            try
            {
                List<InvoiceItem> list = InvoiceAPI.GetInvoiceLineItems(id.ToString(), custNo);

                return Ok(list);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }


        [HttpGet("{custNo}/{minDate}/{maxDate}")]
        public ActionResult<List<Invoice>> GetInvoiceByCustInvNo(string custNo, DateTime minDate, DateTime maxDate)
        {
            try
            {
                List<Invoice> list = InvoiceAPI.GetInvoicesByCustDateRange(custNo, minDate, maxDate);

                return Ok(list);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }


        // POST api/<InvoiceController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<InvoiceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InvoiceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
