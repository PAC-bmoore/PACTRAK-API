using Newtonsoft.Json;
using PAC.Common.Sage;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.DataLayer.API
{
    public class AirWaybillAPI : BaseDL
    {
        #region GetRecentAirbills
        public static List<AirWaybillItem> GetRecentAirbills(string custNo)
        {

            List<AirWaybillItem> list = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/AirWaybill/recent/{1}/{2}", BaseAPIUrl, custNo));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                list = JsonConvert.DeserializeObject<List<AirWaybillItem>>(response.Content);
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
