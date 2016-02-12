using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PatientCareWebApi.Results
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class AuthenticationFailureResult : IHttpActionResult
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }
        public HttpRequestMessage Request { get; set; }

        public string ReasonPhrase { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            response.ReasonPhrase = ReasonPhrase;
            return null;
        }
    }
}