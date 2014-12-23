// ==============================================================================
// 
// EveHQ - An Eve-Online™ character assistance application
// Copyright © 2005-2014  EveHQ Development Team
//   
// This file is part of EveHQ.
//  
// The source code for EveHQ is free and you may redistribute 
// it and/or modify it under the terms of the MIT License. 
// 
// Refer to the NOTICES file in the root folder of EVEHQ source
// project for details of 3rd party components that are covered
// under their own, separate licenses.
// 
// EveHQ is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
// license below for details.
// 
// ------------------------------------------------------------------------------
// 
// The MIT License (MIT)
// 
// Copyright © 2005-2014  EveHQ Development Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ==============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using EveHQ.Caching;
using EveHQ.Common;
using EveHQ.Common.Extensions;

namespace EveHQ.EveApi
{
    /// <summary>
    ///     Client object for interacting with the Character Data methods on the Eve Web Service.
    /// </summary>
    public sealed class CharacterClient : CorpCharBaseClient
    {
        /// <summary>The request prefix.</summary>
        private const string RequestPrefix = "/char";

        /// <summary>Initializes a new instance of the CharacterClient class.</summary>
        /// <param name="eveServiceLocation">location of the eve API web service</param>
        /// <param name="cacheProvider">root folder used for caching.</param>
        /// <param name="requestProvider">Request provider to use for this instance.</param>
        internal CharacterClient(string eveServiceLocation, ICacheProvider cacheProvider,
            IHttpRequestProvider requestProvider)
            : base(eveServiceLocation, cacheProvider, requestProvider, RequestPrefix)
        {
        }

        /// <summary>Calls the CalendarEventAttendees method on the Eve web service.</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="eventId">The eventID to query</param>
        /// <param name="responseMode"></param>
        /// <returns>A collection of attendees for the event.</returns>
        public Task<EveServiceResponse<IEnumerable<CalendarEventAttendee>>> CalendarEventAttendees(string keyId,
            string vCode, int characterId, int eventId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);

            const string MethodPath = "{0}/CalendarEventAttendees.xml.aspx";
            const string CacheKeyFormat = "Calendar_EventAttendees{0}_{1}_{2}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId, eventId);

            IDictionary<string, string> apiParams = new Dictionary<string, string>();
            apiParams[ApiConstants.EventId] = eventId.ToInvariantString();

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), apiParams,
                cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ParseCalendarEventAttendeesResponse);
        }

        /// <summary>Retrieves the Character Sheet data from the Eve web service.</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The given character's data.</returns>
        public EveServiceResponse<CharacterData> CharacterSheet(string keyId, string vCode, int characterId,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(CharacterSheetAsync, keyId, vCode, characterId, responseMode);
        }

        /// <summary>Retrieves the Character Sheet data from the Eve web service.</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The given character's data.</returns>
        public Task<EveServiceResponse<CharacterData>> CharacterSheetAsync(string keyId, string vCode, int characterId,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);

            const string MethodPath = "/char/CharacterSheet.xml.aspx";
            const string CacheKeyFormat = "CharacterSheet{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null,
                cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ParseCharacterSheetResponse);
        }

        /// <summary>Gets a collection of mailing lists the user belongs to.</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>a collection of mailing lists</returns>
        public EveServiceResponse<IEnumerable<MailingList>> MailingLists(string keyId, string vCode, int characterId,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(MailingListsAsync, keyId, vCode, characterId, responseMode);
        }

        /// <summary>Gets a collection of mailing lists the user belongs to.</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>a collection of mailing lists</returns>
        public Task<EveServiceResponse<IEnumerable<MailingList>>> MailingListsAsync(string keyId, string vCode,
            int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);

            const string MethodPath = "{0}/mailinglists.xml.aspx";
            const string CacheKeyFormat = "CharacterMailingList{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null,
                cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ParseMailingListsResponse);
        }

        /// <summary>Retrieves the mail bodies for given IDs</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="ids">The id values to get the mail bodies for</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>a collection of mail bodies</returns>
        public EveServiceResponse<IEnumerable<MailBody>> MailBodies(string keyId, string vCode, int characterId,
            IEnumerable<int> ids, ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(MailBodiesAsync, keyId, vCode, characterId, ids, responseMode);
        }

        /// <summary>Retrieves the mail bodies for given IDs</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="ids">The id values to get the mail bodies for</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>a collection of mail bodies</returns>
        public Task<EveServiceResponse<IEnumerable<MailBody>>> MailBodiesAsync(string keyId, string vCode,
            int characterId, IEnumerable<int> ids, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);
            Guard.Ensure(ids != null);
            Guard.Ensure(ids.Any());

            const string MethodPath = "{0}/MailBodies.xml.aspx";
            const string CacheKeyFormat = "CharacterMailBodies{0}_{1}_{2}";

            // TODO: switch to using a hash value for making a unique key or a long batch of mail ids can cause a filename length issue.
            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId,
                string.Join("_", ids.Select(id => id.ToInvariantString())).GetHashCode());

            IDictionary<string, string> apiParams = new Dictionary<string, string>();
            apiParams[ApiConstants.Ids] = string.Join(",", ids.Select(id => id.ToInvariantString()));

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), apiParams,
                cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ProcessMailBodyResponse);
        }

        /// <summary>Retrieves the collection of mail message headers from the eve web service</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>A collection of mail headers </returns>
        public EveServiceResponse<IEnumerable<MailHeader>> MailMessages(string keyId, string vCode, int characterId,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(MailMessagesAsync, keyId, vCode, characterId, responseMode);
        }

        /// <summary>Retrieves the collection of mail message headers from the eve web service</summary>
        /// <param name="keyId">API Key ID to query</param>
        /// <param name="vCode">The Verification Code for this ID</param>
        /// <param name="characterId">Character to query.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>A collection of mail headers </returns>
        public Task<EveServiceResponse<IEnumerable<MailHeader>>> MailMessagesAsync(string keyId, string vCode,
            int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);

            const string MethodPath = "{0}/MailMessages.xml.aspx";
            const string CacheKeyFormat = "CharacterMailMessages{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null,
                cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ProcessMailMessagesResponse);
        }

        /// <summary>Gets the collection of notification for the user.</summary>
        /// <param name="keyId"></param>
        /// <param name="vCode"></param>
        /// <param name="characterId"></param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task" />.</returns>
        public EveServiceResponse<IEnumerable<Notification>> Notifications(string keyId, string vCode, int characterId,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(NotificationsAsync, keyId, vCode, characterId, responseMode);
        }

        /// <summary>Gets the collection of notification for the user.</summary>
        /// <param name="keyId"></param>
        /// <param name="vCode"></param>
        /// <param name="characterId"></param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task" />.</returns>
        public Task<EveServiceResponse<IEnumerable<Notification>>> NotificationsAsync(string keyId, string vCode,
            int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);

            const string MethodPath = "{0}/Notifications.xml.aspx";
            const string CacheKeyFormat = "CharacterNotifications{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null,
                cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ProcessNotificationsResponse);
        }

        /// <summary>The notification texts.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="notificationIds">The notification ids.</param>
        /// <param name="responseMode">The response mode.</param>
        /// <returns></returns>
        public EveServiceResponse<IEnumerable<NotificationText>> NotificationTexts(string keyId, string vCode,
            int characterId, IEnumerable<long> notificationIds, ResponseMode responseMode = ResponseMode.Normal)
        {
            return RunAsyncMethod(NotificationTextsAsync, keyId, vCode, characterId, notificationIds, responseMode);
        }

        /// <summary>The notification texts.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="notificationIds">The notification ids.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task" />.</returns>
        public Task<EveServiceResponse<IEnumerable<NotificationText>>> NotificationTextsAsync(
            string keyId,
            string vCode,
            int characterId,
            IEnumerable<long> notificationIds,
            ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);

            const string MethodPath = "{0}/NotificationTexts.xml.aspx";
            const string CacheKeyFormat = "NotificationTexts{0}_{1}_{2}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId, notificationIds.GetHashCode());
            IDictionary<string, string> apiParams = new Dictionary<string, string>();
            apiParams[ApiConstants.Ids] = string.Join(",", notificationIds.Select(id => id.ToInvariantString()));

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), apiParams,
                cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ProcessNotificationTextsResponse);
        }

        /// <summary>The upcoming calendar events.</summary>
        /// <param name="keyId">The key id.</param>
        /// <param name="vCode">The v code.</param>
        /// <param name="characterId">The character id.</param>
        /// <param name="responseMode">The response Mode.</param>
        /// <returns>The <see cref="Task" />.</returns>
        public Task<EveServiceResponse<IEnumerable<UpcomingCalendarEvent>>> UpcomingCalendarEvents(string keyId,
            string vCode, int characterId, ResponseMode responseMode = ResponseMode.Normal)
        {
            Guard.Ensure(!keyId.IsNullOrWhiteSpace());
            Guard.Ensure(!vCode.IsNullOrWhiteSpace());
            Guard.Ensure(characterId > 0);

            const string MethodPath = "{0}/UpcomingCalendarEvents.xml.aspx";
            const string CacheKeyFormat = "UpcomingCalendarEvents{0}_{1}";

            string cacheKey = CacheKeyFormat.FormatInvariant(keyId, characterId);

            return GetServiceResponseAsync(keyId, vCode, characterId, MethodPath.FormatInvariant(PathPrefix), null,
                cacheKey, ApiConstants.SixtyMinuteCache, responseMode, ProcessUpcomingCalendarEventsResponse);
        }

        /// <summary>Parses the Eve web service response for the CharacterSheet method.</summary>
        /// <param name="results">xml result element</param>
        /// <returns>A strongly typed Character object.</returns>
        public static CharacterData ParseCharacterSheetResponse(XElement results)
        {
            if (results == null)
            {
                return null; // return null... no data.
            }

            // ReSharper disable PossibleNullReferenceException
            // disabled null checks on resharper, because the xml contract expects these not to be null, and if they are an exception should be fired (and caught in base class processing)
            int characterId = results.Element(ApiConstants.CharacterId).Value.ToInt32();
            string name = results.Element(ApiConstants.Name).Value;
            int homeStationId = results.Element("homeStationID").Value.ToInt32();
            DateTimeOffset birthDate = results.Element("DoB").Value.ToDateTimeOffset(0);
            string race = results.Element("race").Value;
            string bloodline = results.Element("bloodLine").Value;
            string ancestry = results.Element("ancestry").Value;
            string gender = results.Element("gender").Value;
            string corporationName = results.Element(ApiConstants.CorporationName).Value;
            int corporationId = results.Element(ApiConstants.CorporationId).Value.ToInt32();
            XElement alliance;
            string allianceName = (alliance = results.Element("allianceName")) != null ? alliance.Value : string.Empty;
            XElement allianceIdNode;
            int allianceId = (allianceIdNode = results.Element("allianceID")) != null
                ? allianceIdNode.Value.ToInt32()
                : 0;
            int freeSkillPoints = results.Element("freeSkillPoints").Value.ToInt32();
            int freeRespecs = results.Element("freeRespecs").Value.ToInt32();
            DateTimeOffset cloneJumpDate = results.Element("cloneJumpDate").Value.ToDateTimeOffset(0);
            DateTimeOffset lastRespecDate = results.Element("lastRespecDate").Value.ToDateTimeOffset(0);
            DateTimeOffset lastTimedRespec = results.Element("lastTimedRespec").Value.ToDateTimeOffset(0);
            DateTimeOffset remoteStationDate = results.Element("remoteStationDate").Value.ToDateTimeOffset(0);
            DateTimeOffset jumpActivation = results.Element("jumpActivation").Value.ToDateTimeOffset(0);
            DateTimeOffset jumpFatigue = results.Element("jumpFatigue").Value.ToDateTimeOffset(0);
            DateTimeOffset jumpLastUpdate = results.Element("jumpLastUpdate").Value.ToDateTimeOffset(0);
            double balance = results.Element("balance").Value.ToDouble();
            XElement attributes = results.Element("attributes");
            int intelligence = attributes.Element("intelligence").Value.ToInt32();
            int willpower = attributes.Element("willpower").Value.ToInt32();
            int perception = attributes.Element("perception").Value.ToInt32();
            int memory = attributes.Element("memory").Value.ToInt32();
            int charisma = attributes.Element("charisma").Value.ToInt32();

            // ReSharper restore PossibleNullReferenceException

            // Build the jump clones
            IEnumerable<JumpClone> jumpClones =
                results.Elements(ApiConstants.Rowset)
                    .First(set => set.Attribute(ApiConstants.Name).Value == "jumpClones")
                    .Elements(ApiConstants.Row)
                    .Select(row => new JumpClone
                                       {
                                           JumpCloneId = long.Parse(row.Attribute("jumpCloneID").Value),
                                           CloneName = row.Attribute("cloneName").Value,
                                           TypeId = int.Parse(row.Attribute("typeID").Value),
                                           LocationId = long.Parse(row.Attribute("locationID").Value),
                                       });

            // Build the jump clone Implants
            IEnumerable<Implant> jumpCloneImplants =
                results.Elements(ApiConstants.Rowset)
                    .First(set => set.Attribute(ApiConstants.Name).Value == "jumpCloneImplants")
                    .Elements(ApiConstants.Row)
                    .Select(row => new Implant
                                       {
                                           JumpCloneId = long.Parse(row.Attribute("jumpCloneID").Value),
                                           TypeId = int.Parse(row.Attribute("typeID").Value),
                                           TypeName = row.Attribute("typeName").Value
                                       });

            // Add the current clone implants to the implant collection
            IEnumerable<Implant> implants =
               results.Elements(ApiConstants.Rowset)
                   .First(set => set.Attribute(ApiConstants.Name).Value == "implants")
                   .Elements(ApiConstants.Row)
                   .Select(row => new Implant
                   {
                       JumpCloneId = 0,
                       TypeId = int.Parse(row.Attribute("typeID").Value),
                       TypeName = row.Attribute("typeName").Value
                   });
           
            // build the enumerable collection for the skills.
            IEnumerable<CharacterSkillRecord> skills =
                results.Elements(ApiConstants.Rowset)
                    .First(set => set.Attribute(ApiConstants.Name).Value == "skills")
                    .Elements(ApiConstants.Row)
                    .Select(
                        row =>
                            new CharacterSkillRecord
                            {
                                SkillId = int.Parse(row.Attribute("typeID").Value),
                                SkillPoints = int.Parse(row.Attribute("skillpoints").Value),
                                Level = int.Parse(row.Attribute("level").Value),
                                Published = int.Parse(row.Attribute("published").Value) == 1
                            });

            // certificates
            IEnumerable<int> certificates =
                results.Elements(ApiConstants.Rowset)
                    .First(set => set.Attribute(ApiConstants.Name).Value == "certificates")
                    .Elements(ApiConstants.Row)
                    .Select(row => int.Parse(row.Attribute("certificateID").Value));

            // Corp Roles collections
            IEnumerable<CharacterCorporationRoles> corpRoles = ParseCorpRoles(results, "corporationRoles");
            IEnumerable<CharacterCorporationRoles> corpRolesAtHq = ParseCorpRoles(results, "corporationRolesAtHQ");
            IEnumerable<CharacterCorporationRoles> corpRolesAtBase = ParseCorpRoles(results, "corporationRolesAtBase");
            IEnumerable<CharacterCorporationRoles> corpRolesAtOther = ParseCorpRoles(results, "corporationRolesAtOther");

            // titles
            IEnumerable<CharacterCorporationTitles> corpTitles =
                results.Elements(ApiConstants.Rowset)
                    .First(set => set.Attribute(ApiConstants.Name).Value == "corporationTitles")
                    .Elements(ApiConstants.Row)
                    .Select(
                        row =>
                            new CharacterCorporationTitles
                            {
                                TitleId = int.Parse(row.Attribute("titleID").Value),
                                TitleName = row.Attribute("titleName").Value
                            });

            // pack it into to the entity type and return.
            return new CharacterData
            {
                CharacterId = characterId,
                Name = name,
                HomeStationId = homeStationId,
                BirthDate = birthDate,
                Race = race,
                BloodLine = bloodline,
                Ancestry = ancestry,
                Gender = gender,
                CorporationName = corporationName,
                CorporationId = corporationId,
                AllianceName = allianceName,
                AllianceId = allianceId,
                FreeSkillPoints = freeSkillPoints,
                FreeRespecs = freeRespecs,
                CloneJumpDate = cloneJumpDate,
                LastRespecDate = lastRespecDate,
                LastTimedRespec = lastTimedRespec,
                RemoteStationDate = remoteStationDate,
                JumpActivation = jumpActivation,
                JumpFatigue = jumpFatigue,
                JumpLastUpdate = jumpLastUpdate,
                Balance = balance,
                Intelligence = intelligence,
                Memory = memory,
                Charisma = charisma,
                Perception = perception,
                Willpower = willpower,
                Skills = skills,
                JumpClones = jumpClones,
                Implants = implants,
                JumpCloneImplants = jumpCloneImplants,
                Certificates = certificates,
                CorporationRoles = corpRoles,
                CorporationRolesAtHq = corpRolesAtHq,
                CorporationRolesAtBase = corpRolesAtBase,
                CorporationRolesAtOthers = corpRolesAtOther,
                CorporationTitles = corpTitles
            };
        }

        /// <summary>The parse corp roles.</summary>
        /// <param name="results">The results.</param>
        /// <param name="corpRoleName">The corp role name.</param>
        /// <returns>The collection of <see cref="CharacterCorporationRoles" />.</returns>
        private static IEnumerable<CharacterCorporationRoles> ParseCorpRoles(XElement results, string corpRoleName)
        {
            int temp;
            return
                results.Elements(ApiConstants.Rowset)
                    .First(set => set.Attribute(ApiConstants.Name).Value == corpRoleName)
                    .Elements(ApiConstants.Row)
                    .Select(
                        row =>
                            new CharacterCorporationRoles
                            {
                                RoleId = int.TryParse(row.Attribute("roleID").Value, out temp) ? temp : 0,
                                RoleName = row.Attribute("roleName").Value
                            });
        }

        /// <summary>The process upcoming calendar events response.</summary>
        /// <param name="result">The result.</param>
        /// <returns>The collection up coming calendar events..</returns>
        private static IEnumerable<UpcomingCalendarEvent> ProcessUpcomingCalendarEventsResponse(XElement result)
        {
            if (result == null)
            {
                return new UpcomingCalendarEvent[0]; // empty collection
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                from row in rowset.Elements(ApiConstants.Row)
                let eventId = row.Attribute("eventID").Value.ToInt64()
                let ownerId = row.Attribute("ownerID").Value.ToInt64()
                let ownerName = row.Attribute("ownerName").Value
                let eventDate = row.Attribute("eventDate").Value.ToDateTimeOffset(0)
                let eventTitle = row.Attribute("eventTitle").Value
                let duration = TimeSpan.FromMinutes(row.Attribute("duration").Value.ToInt32())
                let importance = row.Attribute("importance").Value.ToBoolean()
                let eventText = row.Attribute("eventText").Value
                let response = row.Attribute("response").Value
                select
                    new UpcomingCalendarEvent
                    {
                        Duration = duration,
                        EventDate = eventDate,
                        EventId = eventId,
                        EventText = eventText,
                        EventTitle = eventTitle,
                        IsImportant = importance,
                        OwnerId = ownerId,
                        OwnerName = ownerName,
                        Response = response
                    };
        }

        /// <summary>The process notification texts response.</summary>
        /// <param name="result">The result.</param>
        /// <returns>The collection of notification bodies.</returns>
        private static IEnumerable<NotificationText> ProcessNotificationTextsResponse(XElement result)
        {
            if (result == null)
            {
                return new NotificationText[0]; // empty collection
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                from row in rowset.Elements(ApiConstants.Row)
                let id = row.Attribute("notificationID").Value.ToInt32()
                let text = row.Value
                select new NotificationText {NotificationId = id, Text = text};
        }

        /// <summary>Processes the notifications response.</summary>
        /// <param name="result"></param>
        /// <returns>The collection of notifications.</returns>
        private static IEnumerable<Notification> ProcessNotificationsResponse(XElement result)
        {
            if (result == null)
            {
                return new Notification[0]; // empty collection
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                from row in rowset.Elements(ApiConstants.Row)
                let notifcationId = row.Attribute("notificationID").Value.ToInt32()
                let type = row.Attribute("typeID").Value.ToEnum<NotificationType>()
                let sender = row.Attribute("senderID").Value.ToInt32()
                let sentDate = row.Attribute("sentDate").Value.ToDateTimeOffset(0)
                let isRead = row.Attribute("read").Value.ToBoolean()
                select
                    new Notification
                    {
                        IsRead = isRead,
                        NotificationId = notifcationId,
                        SenderId = sender,
                        SentDate = sentDate,
                        TypeId = type
                    };
        }

        /// <summary>processes the mail message payload into objects</summary>
        /// <param name="result">xml data to process</param>
        /// <returns>a collection of mail headers</returns>
        private static IEnumerable<MailHeader> ProcessMailMessagesResponse(XElement result)
        {
            if (result == null)
            {
                return new MailHeader[0]; // empty collection
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                from row in rowset.Elements(ApiConstants.Row)
                let messageId = row.Attribute("messageID").Value.ToInt32()
                let senderId = row.Attribute("senderID").Value.ToInt32()
                let sentDate = row.Attribute("sentDate").Value.ToDateTimeOffset(0)
                let title = row.Attribute("title").Value
                let toCorpOrAllianceId = row.Attribute("toCorpOrAllianceID").Value
                let toCharacterIds = row.Attribute("toCharacterIDs").Value.Split(',').Select(id => id.ToInt32())
                let toListIds = row.Attribute("toListID").Value.Split(',').Select(id => id.ToInt32())
                select
                    new MailHeader
                    {
                        MessageId = messageId,
                        SenderId = senderId,
                        SentDate = sentDate,
                        Title = title,
                        ToCorpOrAllianceId = toCorpOrAllianceId,
                        ToCharacterIds = toCharacterIds,
                        ToListListIds = toListIds
                    };
        }

        /// <summary>Processes the xml response into mail body object.</summary>
        /// <param name="result"></param>
        /// <returns>The collection of mail bodies.</returns>
        private static IEnumerable<MailBody> ProcessMailBodyResponse(XElement result)
        {
            if (result == null)
            {
                return new MailBody[0]; // empty collection
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                from row in rowset.Elements(ApiConstants.Row)
                let messageId = row.Attribute("messageID").Value.ToInt32()
                let message = row.Value
                select new MailBody {MessageId = messageId, Body = message};
        }

        /// <summary>processes the xml for the mailing list method.</summary>
        /// <param name="result">xml to process.</param>
        /// <returns>a collection of mailing lists</returns>
        private static IEnumerable<MailingList> ParseMailingListsResponse(XElement result)
        {
            if (result == null)
            {
                return new MailingList[0]; // empty collection
            }

            return from rowset in result.Elements(ApiConstants.Rowset)
                from row in rowset.Elements(ApiConstants.Row)
                let listId = row.Attribute("listID").Value.ToInt32()
                let displayName = row.Attribute("displayName").Value
                select new MailingList {ListId = listId, DisplayName = displayName};
        }

        /// <summary>The parser for processing the response from the CalendarEventAttendees Method</summary>
        /// <param name="results">The XML results from the EveAPI</param>
        /// <returns>A collection of attendees.</returns>
        private static IEnumerable<CalendarEventAttendee> ParseCalendarEventAttendeesResponse(XElement results)
        {
            XElement rowset = results.Element(ApiConstants.Rowset);

            if (rowset == null)
            {
                return new CalendarEventAttendee[0]; // return empty collection
            }

            return rowset.Elements(ApiConstants.Row).Select(
                row =>
                {
                    XAttribute idAttribute = row.Attribute(ApiConstants.CharacterId);
                    XAttribute nameAttribute = row.Attribute(ApiConstants.CharacterName);
                    XAttribute responseAttribute = row.Attribute(ApiConstants.Response);

                    int charId;
                    AttendeeResponseType response;

                    charId = int.TryParse(idAttribute.Value, out charId) ? charId : 0;
                    string charName = nameAttribute != null ? nameAttribute.Value : string.Empty;
                    response = Enum.TryParse(responseAttribute.Value, out response)
                        ? response
                        : AttendeeResponseType.Invalid;

                    return new CalendarEventAttendee
                    {
                        CharacterId = charId,
                        CharacterName = charName,
                        Response = response
                    };
                });
        }
    }
}