using Newtonsoft.Json;
using PAC.Common.Sage;
using PAC.Common.Tracking;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.DataLayer.API
{
    public class TrackingAPI : BaseDL
    {
        #region GetOpenAWBs
        public static List<AirWaybillItem> GetOpenAWBs(string custNo)
        {

            List<AirWaybillItem> item = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Tracking/awb/{1}/open", BaseAPIUrl, custNo));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                item = JsonConvert.DeserializeObject<List<AirWaybillItem>>(response.Content);
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return item;
        }
        #endregion

        #region GetManifestItems
        public static List<ManifestItem> GetManifestItems(string sono)
        {

            List<ManifestItem> item = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Tracking/awb/{1}/manifest", BaseAPIUrl, sono));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                item = JsonConvert.DeserializeObject<List<ManifestItem>>(response.Content);
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return item;
        }
        #endregion

        #region GetRecentAWBs
        public static List<AirWaybillItem> GetRecentAWBs(string custNo, int count)
        {

            List<AirWaybillItem> item = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Tracking/awb/recent/{1}/{2}", BaseAPIUrl, custNo, count));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                item = JsonConvert.DeserializeObject<List<AirWaybillItem>>(response.Content);
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
