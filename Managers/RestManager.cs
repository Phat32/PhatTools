using PhatRestHelper.Helpers;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhatRestHelper
{
    public class RestManager
    {
        public RestManager(string uri)
        {
            _uri = uri;
        }

        private string _uri;

        private RestClient _client;

        private RestClient Client
        {
            get
            {
                if (_client == null)
                    _client = new RestClient(_uri);
                return _client;
            }
        }

        public Task<object> Get(string endPoint, IEnumerable<KeyValuePair<string, string>> args = null)
        {
            var test = new KeyValuePair<string, string> ( "test", "test 2" );

            var request = new RestRequest(endPoint, Method.Get);
            if (args?.Any() == true)
            {
                foreach (var arg in args)
                {
                    request.AddParameter(arg.Key, arg.Value);
                }
            }

            return RestHelper.ExecuteAsync<object>(Client, request);
        }

        public async Task<object> Post(string endPoint, object obj, IEnumerable<KeyValuePair<string, string>> args = null)
        {
            var request = new RestRequest(endPoint, Method.Post);
            if (args?.Any() == true)
            {
                foreach (var arg in args)
                {
                    request.AddParameter(arg.Key, arg.Value);
                }
            }
            request.AddJsonBody(obj);

            return await RestHelper.ExecuteAsync<object>(Client, request).ConfigureAwait(false);
        }

        public async Task<object> Put(string endPoint, object obj)
        {
            var request = new RestRequest(endPoint, Method.Put);
            request.AddJsonBody(obj);

            return await RestHelper.ExecuteAsync<object>(Client, request).ConfigureAwait(false);
        }

        public async Task<object> DeleteFromBody(string endPoint, object obj)
        {
            var request = new RestRequest(endPoint, Method.Delete);
            request.AddJsonBody(obj);

            return await RestHelper.ExecuteAsync<object>(Client, request).ConfigureAwait(false);
        }

        public async Task<object> DeleteFromParam(string endPoint, KeyValuePair<string, string> args)
        {
            var request = new RestRequest(endPoint, Method.Delete);
            request.AddParameter(args.Key, args.Value);

            return await RestHelper.ExecuteAsync<object>(Client, request).ConfigureAwait(false);
        }
    }
}
