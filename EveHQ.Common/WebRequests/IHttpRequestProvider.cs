namespace EveHQ.Common
{
    using System;
    using System.Collections.Specialized;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public interface IHttpRequestProvider
    {
        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<HttpResponseMessage> GetAsync(Uri target, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth);

        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<HttpResponseMessage> GetAsync(Uri target, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth, string acceptContentType);

        /// <summary>Executes an HTTP GET Request to the provided URL.</summary>
        /// <param name="target">The target URL.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<HttpResponseMessage> GetAsync(Uri target, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth, string acceptContentType, HttpCompletionOption completionOption);

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postContent">The string content to send as the payload.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<WebResponse> PostAsync(Uri target, string postContent);

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postContent">The string content to send as the payload.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<WebResponse> PostAsync(Uri target, string postContent, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth);

        /// <summary>Executes an HTTP POST request to the provided url.</summary>
        /// <param name="target">The target URL.</param>
        /// <param name="postData">A name/value collection to send as the form data.</param>
        /// <returns>The asynchronouse task instance</returns>
        Task<WebResponse> PostAsync(Uri target, NameValueCollection postData, Uri proxyServerAddress, bool useDefaultCredential, string proxyUserName, string proxyPassword, bool useBasicAuth);
    }
}