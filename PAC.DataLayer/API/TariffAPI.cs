using Newtonsoft.Json;
using PAC.Common.Sage;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PAC.DataLayer.API
{
    public class TariffAPI
    {
        public static List<Tariff> GetMinTariffs(string custNo)
        {
            List<Tariff> item = null;

            try
            {
                //AppSetting setting = SettingsDL.GetAppSetting("Sage Data URL");
                string APIURL = "http://10.0.0.6/PACTRAK";

//                var client = new RestClient(string.Format("{0}/api/Customer/{1}", setting.SettingValue, custNo));
                var client = new RestClient(string.Format("{0}/api/min/{1}", APIURL, custNo));
                var request = new RestRequest();
                request.Method = Method.Get;
                RestResponse response = client.Execute(request);

                item = JsonConvert.DeserializeObject<List<Tariff>>(response.Content);
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            return item;
        }
    }
}
