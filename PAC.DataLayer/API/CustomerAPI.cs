using Newtonsoft.Json;
using PAC.Common.Sage;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Net;
using System.Reflection;
using System.Text;

namespace PAC.DataLayer.API
{
    public class CustomerAPI : BaseDL
    {
        #region GetCustomer
        public static Customer GetCustomer(string custNum)
        {
            Customer? item = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Customer/{1}", BaseAPIUrl, custNum));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                if (!response.IsSuccessful)
                    throw new Exception(string.Format("Inner service returned {0}", (int)response.StatusCode));

                item = JsonConvert.DeserializeObject<Customer>(response.Content);
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return item;
        }
        #endregion

        #region UpdateCustomer
        public static string UpdateCustomer(Customer cust)
        {
            string item = string.Empty;
            try
            {
                var client = new RestClient(string.Format("{0}/api/Customer", BaseAPIUrl));
                var request = new RestRequest();
                request.Method = Method.Put;
                request.AddObject<Customer>(cust);
                RestResponse response = client.Execute(request);

                if (!response.IsSuccessful)
                    throw new Exception(string.Format("Inner service returned {0}", (int)response.StatusCode));

                item = response.Content;
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return item;
        }
        #endregion

        #region GetCustomerSummary
        public static CustomerSummary GetCustomerSummary(string custNum)
        {
            CustomerSummary? item = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Customer/summary/{1}", BaseAPIUrl, custNum));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                if (response.IsSuccessful)
                    item = JsonConvert.DeserializeObject<CustomerSummary>(response.Content);
                else if (response.StatusCode != HttpStatusCode.NotFound)
                    throw new Exception(string.Format("Inner service returned {0}", (int)response.StatusCode));
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return item;
        }
        #endregion

        #region SearchCustomers
        public static List<CustomerSummary> SearchCustomers(string text)
        {
            List<CustomerSummary>? list = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Customer/search/{1}", BaseAPIUrl, Uri.EscapeDataString(text)));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                if (!response.IsSuccessful)
                    throw new Exception(string.Format("Inner service returned {0}", (int)response.StatusCode));

                list = JsonConvert.DeserializeObject<List<CustomerSummary>>(response.Content);
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return list;
        }
        #endregion
    }
}
