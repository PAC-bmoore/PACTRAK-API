using Newtonsoft.Json;
using PAC.Common.Sage;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PAC.DataLayer.API
{
    public class TariffAPI : BaseDL
    {
        #region GetMinTariffs
        public static List<Tariff> GetMinTariffs(string custNo)
        {
            
            List<Tariff> item = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Tariff/min/{1}", BaseAPIUrl, custNo));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                if (!response.IsSuccessful)
                    throw new Exception(string.Format("Inner service returned {0}", (int)response.StatusCode));

                item = JsonConvert.DeserializeObject<List<Tariff>>(response.Content);
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return item;
        }
        #endregion

        #region GetTariffsByOriginDest
        public static List<Tariff> GetTariffsByOriginDest(string custNo, string origin, string dest)
        {

            List<Tariff> item = null;

            try
            {
                var client = new RestClient(string.Format("{0}/api/Tariff/{1}/{2}/{3}", BaseAPIUrl, custNo, origin, dest));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                if (!response.IsSuccessful)
                    throw new Exception(string.Format("Inner service returned {0}", (int)response.StatusCode));

                item = JsonConvert.DeserializeObject<List<Tariff>>(response.Content);
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
