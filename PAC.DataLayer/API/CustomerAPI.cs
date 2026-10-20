using Newtonsoft.Json;
using PAC.Common.Sage;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Reflection;
using System.Text;

namespace PAC.DataLayer.API
{
    public class CustomerAPI : BaseDL
    {
        #region GetCustomer
        public static Customer GetCustomer(string custNum)
        {
            Customer item = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Customer/{1}", BaseAPIUrl, custNum));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

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

                item = response.Content;
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
