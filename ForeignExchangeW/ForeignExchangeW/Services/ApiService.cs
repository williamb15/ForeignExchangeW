
namespace ForeignExchangeW
{
    using ForeignExchangeW.Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    public class ApiService
    {
        public async Task<Response> CheckConnection ()
        {
            if(!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Check your Internet Settings",
                };
            }

            var responce = await CrossConnectivity.Current.IsRemoteReachable("google.com");

            if(!responce)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Check your Internet Connection",
                };

            }

            return new Response
            {
                IsSuccess = true,
            };
        }

        public async Task<Response> GetList<T>(string urlBase, string controller)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }
                var List = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response

                {
                    IsSuccess = true,
                    Result= List,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
