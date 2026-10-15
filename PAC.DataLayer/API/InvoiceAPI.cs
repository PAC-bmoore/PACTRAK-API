using Newtonsoft.Json;
using PAC.Common.Sage;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.DataLayer.API
{
    public class InvoiceAPI : BaseDL
    {
        #region GetInvoicesByCustDateRange
        public static List<Invoice> GetInvoicesByCustDateRange(string custNo, DateTime minDate, DateTime maxDate)
        {
            List<Invoice> item = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Invoice/{1}/{2}/{3}", BaseAPIUrl, custNo, minDate.ToString("yyyy-MM-dd"), maxDate.ToString("yyyy-MM-dd")));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                item = JsonConvert.DeserializeObject<List<Invoice>>(response.Content);
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return item;
        }
        #endregion

        #region GetInvoiceLineItems
        public static List<InvoiceItem> GetInvoiceLineItems(string invNo, string custNo)
        {

            List<InvoiceItem> list = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Invoice/Detail/{1}/{2}", BaseAPIUrl, invNo, custNo));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                list = JsonConvert.DeserializeObject<List<InvoiceItem>>(response.Content);
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return list;
        }
        #endregion

        #region GetTrackingInvoiceBySono
        public static Invoice GetTrackingInvoiceBySono(string sono)
        {

            Invoice item = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Invoice/{1}", BaseAPIUrl, sono));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                item = JsonConvert.DeserializeObject<Invoice>(response.Content);
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return item;
        }
        #endregion

    }
}
