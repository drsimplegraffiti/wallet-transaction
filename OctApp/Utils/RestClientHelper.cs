
using RestSharp;

namespace OctApp.Utils
{
    public class RestClientHelper
    {
        private readonly RestClient _restClient;

        public RestClientHelper(string baseUrl)
        {
            _restClient = new RestClient(baseUrl);
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            try
            {
                var response = _restClient.Execute<T>(request);

                if (response.ErrorException != null)
                {
                    throw new Exception($"Error: {response.ErrorMessage}", response.ErrorException);
                }

                return response.Data ?? new T();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}