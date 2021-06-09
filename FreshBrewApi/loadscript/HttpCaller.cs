using Newtonsoft.Json;
using Riverbed.Test.FreshBrew.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LoadScript
{
    public class HttpCaller 
    {
         HttpClient client = null;
     
        #region ctor
        public HttpCaller(string baseAddress)
        {     
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30); //set your own timeout.
            client.BaseAddress = new Uri(baseAddress);//"http://10.46.45.8";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        #endregion

        #region Http Get
        public string DoHttpGet(string urlPathWithNoBaseAddress)
        {
            return GetAllOrderAsync(urlPathWithNoBaseAddress).GetAwaiter().GetResult();
        }

        private async Task<string> GetAllOrderAsync(string urlPathWithNoBaseAddress)
        {
            string result = null;
            HttpResponseMessage response = await client.GetAsync(urlPathWithNoBaseAddress);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                // list = JsonConvert.DeserializeObject<List<OrderItem>>(jsonString);
                result = jsonString;
            }
            return result;
        }


        #endregion

        #region do post
        public string DoHttpPost(string pathAndQuery, string payload)
        {
            return CreateHttpPostAsync(pathAndQuery, payload).GetAwaiter().GetResult();
        }
        private async Task<string> CreateHttpPostAsync(string pathAndQuery, string payload)
        {
            string result = null;
            string json = payload; //JsonConvert.SerializeObject(newOrder);
            HttpResponseMessage response = await client.PostAsync(pathAndQuery, new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.StatusCode == HttpStatusCode.NoContent)
                return HttpStatusCode.NoContent.ToString();
            else if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                //result = JsonConvert.DeserializeObject<OrderItem>(jsonString);
                result = jsonString;
            }
            return result;
        }
        #endregion

        #region do put
        public string DoHttpPut(string pathAndQuery, string payload)
        {            
            return CreateHttpPutAsync(pathAndQuery, payload).GetAwaiter().GetResult();
        }
        private async Task<String> CreateHttpPutAsync(string path, string payload)
        {
            string json = payload, result=null;
            Console.WriteLine("CreateHttpPutAsync:" + path + " payload:" + payload);
            HttpResponseMessage response = await client.PutAsync(path,new StringContent(json, Encoding.UTF8, "application/json"));          
            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }
        #endregion

        #region do delete
        public HttpStatusCode DoHttpDelete(string pathAndQuery)
        {
            return CreateHttpDeleteAsync(pathAndQuery).GetAwaiter().GetResult();
        }
        public async Task<HttpStatusCode> CreateHttpDeleteAsync(string pathAndQuery)
        {
            HttpResponseMessage response = await client.DeleteAsync(pathAndQuery);
            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
        }
        #endregion

    }
}
