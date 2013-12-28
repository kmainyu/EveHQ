using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveHQ.EveApi
{
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using EveHQ.Caching;
    using EveHQ.Common;
    using EveHQ.Common.Extensions;

    public class ServerClient : BaseApiClient
    {
        /// <summary>The request prefix.</summary>
        private const string RequestPrefix = "/server";

        /// <summary>Initializes a new instance of the EveClient class.</summary>
        /// <param name="eveServiceLocation">location of the eve API web service</param>
        /// <param name="cacheProvider">root folder used for caching.</param>
        /// <param name="requestProvider">Request provider to use for this instance.</param>
        internal ServerClient(string eveServiceLocation, ICacheProvider cacheProvider, IHttpRequestProvider requestProvider)
            : base(eveServiceLocation, cacheProvider, requestProvider)
        {
        }

        public EveServiceResponse<ServerStatus> ServerStatus()
        {
          return RunAsyncMethod(ServerStatusAsync);
        }

        public Task<EveServiceResponse<ServerStatus>> ServerStatusAsync()
        {

            const string MethodPath = "{0}/ServerStatus.xml.aspx";
            const string CacheKeyFormat = "ServerStatus";

            string cacheKey = CacheKeyFormat.FormatInvariant();

            IDictionary<string, string> apiParams = new Dictionary<string, string>();

            return GetServiceResponseAsync(null, null, 0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, ParseServerStatusResponse);
        }


        private static ServerStatus ParseServerStatusResponse(XElement result)
        {
            if (result == null)
            {
                return null; // return null... no data.
            }

            var online = result.Element("serverOpen").Value.ToBoolean();
            var players = result.Element("onlinePlayers").Value.ToInt32();

            return new ServerStatus() { OnlinePlayers = players, IsServerOpen = online };
        }

    }
}
