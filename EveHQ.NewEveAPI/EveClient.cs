// -----------------------------------------------------------------------
// <copyright file="EveClient.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace EveHQ.EveApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using EveHQ.Caching;
    using EveHQ.Common;
    using EveHQ.Common.Extensions;

    /// <summary>
    /// Client for the general Eve information APIs
    /// </summary>
    public sealed class EveClient : BaseApiClient
    {
        /// <summary>The request prefix.</summary>
        private const string RequestPrefix = "/eve";

        /// <summary>
        /// Initializes a new instance of the AccountClient class.
        /// </summary>
        /// <param name="eveServiceLocation">location of the eve API web service</param>
        /// <param name="cacheProvider">root folder used for caching.</param>
        /// <param name="requestProvider">Request provider to use for this instance.</param>
        internal EveClient(string eveServiceLocation, ICacheProvider cacheProvider, IHttpRequestProvider requestProvider)
            : base(eveServiceLocation, cacheProvider, requestProvider)
        {
        }


        public EveServiceResponse<IEnumerable<CharacterName>> CharacterName(IEnumerable<long> ids)
        {
            var task = CharacterNameAsync(ids);
            task.Wait();
            return task.Result;
        }

        public Task<EveServiceResponse<IEnumerable<CharacterName>>> CharacterNameAsync(IEnumerable<long> ids)
        {
            System.Diagnostics.Contracts.Contract.Requires(ids != null);

            List<long> checkedIds =  ids.Distinct().ToList(); // remove duplicates

            if (checkedIds.Count > 250 || checkedIds.Count <1 )
            {
                throw new ArgumentException("ids must have a length of 250 or less and greater than 0.");
            }

            const string MethodPath = "{0}/CharacterName.xml.aspx";
            const string CacheKeyFormat = "Eve_CharacterName{0}";

            string cacheKey = CacheKeyFormat.FormatInvariant(ids.GetHashCode());

            IDictionary<string, string> apiParams = new Dictionary<string, string>();
            apiParams.Add(ApiConstants.Ids, string.Join(",",checkedIds.Select(id=>id.ToInvariantString())));

            return this.GetServiceResponseAsync(null,null,0, MethodPath.FormatInvariant(RequestPrefix), apiParams, cacheKey, ApiConstants.SixtyMinuteCache, ParseCharacterNameResult);
        }

        private static IEnumerable<CharacterName> ParseCharacterNameResult(XElement results)
        {
            if (results == null)
            {
                return null; // return null... no data.
            }

            return (from rowset in results.Elements(ApiConstants.Rowset)
                    from row in rowset.Elements(ApiConstants.Row)
                    let name = row.Attribute("name").Value
                    let id = row.Attribute("characterID").Value.ToInt64()
                    select new CharacterName() { Id = id, Name = name });
        }
    }
}
