// ===========================================================================
// <copyright file="AccessMasks.cs" company="EveHQ Development Team">
//  EveHQ - An Eve-Online™ character assistance application
//  Copyright © 2005-2013  EveHQ Development Team
//  This file (AccessMasks.cs), is part of EveHQ.
//  EveHQ is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  EveHQ is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with EveHQ.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// ============================================================================
namespace EveHQ.EveApi
{
    using System;

    /// <summary>The character access masks.</summary>
    [Flags]
    public enum CharacterAccessMasks : long
    {
        /// <summary>The account balances.</summary>
        AccountBalances = 1 << 0,

        /// <summary>The asset list.</summary>
        AssetList = 1 << 1,

        /// <summary>The calendar event attendees.</summary>
        CalendarEventAttendees = 1 << 2,

        /// <summary>The character sheet.</summary>
        CharacterSheet = 1 << 3,

        /// <summary>The contact list.</summary>
        ContactList = 1 << 4,

        /// <summary>The contact notifications.</summary>
        ContactNotifications = 1 << 5,

        /// <summary>The fac war stats.</summary>
        FacWarStats = 1 << 6,

        /// <summary>The industry jobs.</summary>
        IndustryJobs = 1 << 7,

        /// <summary>The kill log.</summary>
        KillLog = 1 << 8,

        /// <summary>The mail bodies.</summary>
        MailBodies = 1 << 9,

        /// <summary>The mailing lists.</summary>
        MailingLists = 1 << 10,

        /// <summary>The mail messages.</summary>
        MailMessages = 1 << 11,

        /// <summary>The market orders.</summary>
        MarketOrders = 1 << 12,

        /// <summary>The medals.</summary>
        Medals = 1 << 13,

        /// <summary>The notifications.</summary>
        Notifications = 1 << 14,

        /// <summary>The notification text.</summary>
        NotificationText = 1 << 15,

        /// <summary>The research.</summary>
        Research = 1 << 16,

        /// <summary>The skill in training.</summary>
        SkillInTraining = 1 << 17,

        /// <summary>The skill queue.</summary>
        SkillQueue = 1 << 18,

        /// <summary>The standings.</summary>
        Standings = 1 << 19,

        /// <summary>The upcoming calendar events.</summary>
        UpcomingCalendarEvents = 1 << 20,

        /// <summary>The wallet journal.</summary>
        WalletJournal = 1 << 21,

        /// <summary>The wallet transactions.</summary>
        WalletTransactions = 1 << 22,

        /// <summary>The character info private.</summary>
        CharacterInfoPrivate = 1 << 23,

        /// <summary>The character info public.</summary>
        CharacterInfoPublic = 1 << 24,

        /// <summary>The account status.</summary>
        AccountStatus = 1 << 25,

        /// <summary>The contracts.</summary>
        Contracts = 1 << 26,
    }

    /// <summary>The corporate access masks.</summary>
    [Flags]
    public enum CorporateAccessMasks : long
    {
        /// <summary>The account balances.</summary>
        AccountBalances = 1 << 0,

        /// <summary>The asset list.</summary>
        AssetList = 1 << 1,

        /// <summary>The member medals.</summary>
        MemberMedals = 1 << 2,

        /// <summary>The corporation sheet.</summary>
        CorporationSheet = 1 << 3,

        /// <summary>The contact list.</summary>
        ContactList = 1 << 4,

        /// <summary>The container log.</summary>
        ContainerLog = 1 << 5,

        /// <summary>The fac war stats.</summary>
        FacWarStats = 1 << 6,

        /// <summary>The industry jobs.</summary>
        IndustryJobs = 1 << 7,

        /// <summary>The kill log.</summary>
        KillLog = 1 << 8,

        /// <summary>The member security.</summary>
        MemberSecurity = 1 << 9,

        /// <summary>The member security log.</summary>
        MemberSecurityLog = 1 << 10,

        /// <summary>The member tracking.</summary>
        MemberTracking = 1 << 11,

        /// <summary>The market orders.</summary>
        MarketOrders = 1 << 12,

        /// <summary>The medals.</summary>
        Medals = 1 << 13,

        /// <summary>The outpost list.</summary>
        OutpostList = 1 << 14,

        /// <summary>The outpost service list.</summary>
        OutpostServiceList = 1 << 15,

        /// <summary>The shareholders.</summary>
        Shareholders = 1 << 16,

        /// <summary>The starbase detail.</summary>
        StarbaseDetail = 1 << 17,

        /// <summary>The standings.</summary>
        Standings = 1 << 18,

        /// <summary>The starbase list.</summary>
        StarbaseList = 1 << 19,

        /// <summary>The wallet journal.</summary>
        WalletJournal = 1 << 20,

        /// <summary>The wallet transactions.</summary>
        WalletTransactions = 1 << 21,

        /// <summary>The titles.</summary>
        Titles = 1 << 22,

        /// <summary>The contracts.</summary>
        Contracts = 1 << 23
    }

    public static class AccessMasks
    {
        public static bool HasCorpPermissions(long accessMask, CorporateAccessMasks reqPermissions)
        {
            return ((CorporateAccessMasks)accessMask & reqPermissions) == reqPermissions;
        }

        public static bool HasCharacterPermissions(long accessMask, CharacterAccessMasks reqPermissions)
        {
            return ((CharacterAccessMasks)accessMask & reqPermissions) == reqPermissions;
        }
    }
}