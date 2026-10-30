using Newtonsoft.Json;
using PAC.Common.Sage;
using PAC.Common.Tracking;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAC.DataLayer.API
{
    public class AirWaybillAPI : BaseDL
    {
        #region GetRecentAirbills
        public static List<AirWaybillItem> GetRecentAirbills(string custNo, int count)
        {

            List<AirWaybillItem>? list = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Tracking/awb/recent/{1}/{2}", BaseAPIUrl, custNo, count));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                if (!response.IsSuccessful)
                    throw new Exception(string.Format("Inner service returned {0}", (int)response.StatusCode));

                list = JsonConvert.DeserializeObject<List<AirWaybillItem>>(response.Content);
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return list;
        }
        #endregion

        #region GetOpenAirWaybills
        public static List<AirWaybillItem> GetOpenAirWaybills(string custNo)
        {

            List<AirWaybillItem>? list = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Tracking/awb/{1}/open", BaseAPIUrl, custNo));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                if (!response.IsSuccessful)
                    throw new Exception(string.Format("Inner service returned {0}", (int)response.StatusCode));

                list = JsonConvert.DeserializeObject<List<AirWaybillItem>>(response.Content);
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return list;
        }
        #endregion

        #region GetManifestItems
        public static List<ManifestItem> GetManifestItems(string sono)
        {

            List<ManifestItem>? list = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Tracking/awb/{1}/manifest", BaseAPIUrl, sono));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                if (!response.IsSuccessful)
                    throw new Exception(string.Format("Inner service returned {0}", (int)response.StatusCode));

                list = JsonConvert.DeserializeObject<List<ManifestItem>>(response.Content);
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return list;
        }
        #endregion

        #region GetShipmentDetails
        public static List<ShipmentItem> GetShipmentDetails(string sono)
        {

            List<ShipmentItem>? list = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Tracking/awb/shipment/{1}/details", BaseAPIUrl, sono));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                if (!response.IsSuccessful)
                    throw new Exception(string.Format("Inner service returned {0}", (int)response.StatusCode));

                list = JsonConvert.DeserializeObject<List<ShipmentItem>>(response.Content);
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
