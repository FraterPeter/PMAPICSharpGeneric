using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using Newtonsoft.Json;

namespace SuT.PMAPI.Generic
{
    public class Client
    {
        const string PMAPI_SERVER = "https://api.sign-up.to";
        const string IDENT = ".NET generic library";
        const int VERSION = 1;

        public string Server { get; private set; }

        private RestRequest _request;
        private RestClient _restClient;
        private IAuthenticator _authenticator { get; set; }


        public Client(IAuthenticator auth)
        {
            Init(auth, PMAPI_SERVER);
        }

        private void Init(IAuthenticator auth, string serverURL)
        {
            Server = serverURL;
            _authenticator = auth;
            _restClient = new RestClient(Server);
            _restClient.Authenticator = auth;
        }

        protected string getResource(string endpoint)
        {
            return "v" + VERSION + "/" + endpoint;
        }

        private Response makeRequest(string endpoint, Hashtable properties, RestSharp.Method method)
        {
            _request = new RestRequest();

            _request.AddHeader("Accept", "application/json");
            _request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            _request.AddHeader("Library", IDENT);
            _request.Resource = getResource(endpoint);
            _request.Method = method;

            // Go through the properties and add them to the request
            foreach (DictionaryEntry pair in properties)
            {
                _request.AddParameter(pair.Key.ToString(), pair.Value.ToString());
            }

            // Make the request
            IRestResponse restResponse = _restClient.Execute(_request);

            // This is quite dirty, but we deserialize the JSON into 2 dictionaries for use later
            Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(restResponse.Content);
            Dictionary<string, object> values2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(values["response"].ToString());            

            // Define the response object    
            Response returnResponse = new Response();
            returnResponse.Status = values["status"].ToString();

            // Check for errors
            if (values["status"].ToString() == "error")
            {
                returnResponse.setError(values2["message"].ToString(), values2["code"].ToString(), values2["subcode"] == null ? null : values2["subcode"].ToString());
                return returnResponse;
            }

            // There are no errors, so continue as planned            
            returnResponse.Count = Convert.ToInt32(values2["count"]);

            if (values2["next"] == null)
            {
                returnResponse.Next = null;
            }
            else
            {
                returnResponse.Next = values2["next"].ToString();
            }

            // Data is a list of dictionary objects. This is used to store the returned rows.
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

            try
            {
                data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(values2["data"].ToString());
                returnResponse.Data = data;
            }
            catch (Newtonsoft.Json.JsonSerializationException e)
            {
                // This exception occurs when the returned data is not a list but is 1 item. We therefore create a list and add the 
                // single item to the list. This keeps the results set consistent.
                Dictionary<string, object> item = JsonConvert.DeserializeObject<Dictionary<string, object>>(values2["data"].ToString());
                data = new List<Dictionary<string, object>>();
                data.Add(item);
                returnResponse.Data = data;
            }

            return returnResponse;
        }

        public Response get(string endpoint, Hashtable properties)
        {
            return makeRequest(endpoint, properties, RestSharp.Method.GET);
        }

        public Response post(string endpoint, Hashtable properties)
        {
            return makeRequest(endpoint, properties, RestSharp.Method.POST);
        }

        public Response put(string endpoint, Hashtable properties)
        {
            return makeRequest(endpoint, properties, RestSharp.Method.PUT);
        }

        public Response delete(string endpoint, Hashtable properties)
        {
            return makeRequest(endpoint, properties, RestSharp.Method.DELETE);
        }
    }
}