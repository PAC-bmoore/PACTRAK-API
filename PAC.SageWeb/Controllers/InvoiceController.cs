using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PAC.Common.Sage;
using PAC.DataLayer.Sage;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PAC.SageWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                Invoice item = InvoiceDL.GetTrackingInvoiceBySONO(id.ToString());

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
                List<InvoiceItem> list = InvoiceDL.GetInvoiceTransactions(id.ToString(), custNo);

                return Ok(list);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("{custNo}/{minDate}/{maxDate}")]
        public ActionResult<List<Invoice>> GetInvoiceByCustDateRange(string custNo, DateTime minDate, DateTime maxDate)
        {
            try
            {
                List<Invoice> list = InvoiceDL.GetInvoicesByCustDateRange(custNo, minDate, maxDate);

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
