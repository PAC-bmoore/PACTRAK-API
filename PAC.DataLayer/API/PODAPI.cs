using Newtonsoft.Json;
using PAC.Common.Sage;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.DataLayer.API
{
    public class PODAPI : BaseDL
    {
        #region GetPODHistory
        public static List<PODHistoryItem> GetPODHistory(string custNo, DateTime minDate, DateTime maxDate)
        {

            List<PODHistoryItem> item = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/POD/{1}/{2}/{3}", BaseAPIUrl, custNo, minDate.ToString("yyyy-MM-dd"), maxDate.ToString("yyyy-MM-dd")));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                item = JsonConvert.DeserializeObject<List<PODHistoryItem>>(response.Content);
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return item;
        }
        #endregion

        #region GetPODInformation
        public static List<PODItem> GetPODInformation(string awb)
        {

            List<PODItem> item = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/POD/{1}", BaseAPIUrl, awb));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                item = JsonConvert.DeserializeObject<List<PODItem>>(response.Content);
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
