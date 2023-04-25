using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PhatRestHelper.Helpers
{
    public static class RestHelper
    {
        public static async Task<bool> ExecuteAsync(RestClient client, RestRequest request)
        {
            try
            {
                var response = await client.ExecuteAsync(request).ConfigureAwait(false);
                return response.StatusCode == HttpStatusCode.OK ? true : LogError<bool>(response);
            }
            catch (Exception ex)
            {
                return LogError<bool>(ex);
            }
        }

        public static async Task<T> ExecuteAsync<T>(RestClient client, RestRequest request)
        {
            try
            {
                var response = await client.ExecuteAsync<T>(request).ConfigureAwait(false);
               return response.StatusCode == HttpStatusCode.OK ? response.Data : LogError<T>(response);
            }
            catch (Exception ex)
            {
                return LogError<T>(ex);
            }
        }

        public static async Task<string> ExecuteContentAsync(RestClient client, RestRequest request)
        {
            try
            {
                var response = await client.ExecuteAsync<string>(request).ConfigureAwait(false);
                return response.StatusCode == HttpStatusCode.OK ? response.Content : LogError<string>(response);
            }
            catch (Exception ex)
            {
                return LogError<string>(ex);
            }
        }

        public static T LogError<T>(RestResponse response)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                
            }            

            return default(T);
        }

        public static T LogError<T>(Exception ex)
        {
            try
            {
                

            }
            catch (Exception e)
            {
                
            }

            return default(T);
        }
    }
}
