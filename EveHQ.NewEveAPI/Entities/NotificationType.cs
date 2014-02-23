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

namespace EveHQ.EveApi
{
    /// <summary>The notification type.</summary>
    public enum NotificationType
    {
        /// <summary>The unknown.</summary>
        Unknown,

        /// <summary>The legacy.</summary>
        Legacy,

        /// <summary>The character deleted.</summary>
        CharacterDeleted,

        /// <summary>The give medal to character.</summary>
        GiveMedalToCharacter,

        /// <summary>The alliance Maintenance bill.</summary>
        AllianceMaintenanceBill,

        /// <summary>The alliance war declared.</summary>
        AllianceWarDeclared,

        /// <summary>The alliance war surrender.</summary>
        AllianceWarSurrender,

        /// <summary>The alliance war retracted.</summary>
        AllianceWarRetracted,

        /// <summary>The alliance war invalidated by concord.</summary>
        AllianceWarInvalidatedByConcord,

        /// <summary>The bill issued to a character.</summary>
        BillIssuedToACharacter,

        /// <summary>The bill issued to corp or alliance.</summary>
        BillIssuedToCorpOrAlliance,

        /// <summary>The bill not paid due to insufficient funds.</summary>
        BillNotPaidDueToInsufficientFunds,

        /// <summary>The bill issued by character paid.</summary>
        BillIssuedByCharacterPaid,

        /// <summary>The bill issued by corp or alliance paid.</summary>
        BillIssuedByCorpOrAlliancePaid,

        /// <summary>The bounty claimed.</summary>
        BountyClaimed,

        /// <summary>The clone activated.</summary>
        CloneActivated,

        /// <summary>The new corp member application.</summary>
        NewCorpMemberApplication,

        /// <summary>The corp application rejected.</summary>
        CorpApplicationRejected,

        /// <summary>The corp application accepted.</summary>
        CorpApplicationAccepted,

        /// <summary>The corp tax rate changed.</summary>
        CorpTaxRateChanged,

        /// <summary>The corp news report.</summary>
        CorpNewsReport,

        /// <summary>The player left corp.</summary>
        PlayerLeftCorp,

        /// <summary>The new corp CEO.</summary>
        NewCorpCEO,

        /// <summary>The corp dividend liquidation.</summary>
        CorpDividendLiquidation,

        /// <summary>The corp dividend payout.</summary>
        CorpDividendPayout,

        /// <summary>The corp vote created.</summary>
        CorpVoteCreated,

        /// <summary>The corp CEO vote revoked.</summary>
        CorpCeoVoteRevoked,

        /// <summary>The corp declares war.</summary>
        CorpDeclaresWar,

        /// <summary>The corp war started.</summary>
        CorpWarStarted,

        /// <summary>The corp surrenders war.</summary>
        CorpSurrendersWar,

        /// <summary>The corp retracts war.</summary>
        CorpRetractsWar,

        /// <summary>The corp war invalidated by concord.</summary>
        CorpWarInvalidatedByConcord,

        /// <summary>The container password retrieved.</summary>
        ContainerPasswordRetrieved,

        /// <summary>The contraband confiscated.</summary>
        ContrabandConfiscated,

        /// <summary>The first ship insurance.</summary>
        FirstShipInsurance,

        /// <summary>The insurance paid.</summary>
        InsurancePaid,

        /// <summary>The insurance contract expired invalidated.</summary>
        InsuranceContractExpiredInvalidated,

        /// <summary>The SOV claim fails alliance.</summary>
        SovClaimFailsAlliance,

        /// <summary>The SOV claim fails corp.</summary>
        SovClaimFailsCorp,

        /// <summary>The SOV bill late alliance.</summary>
        SovBillLateAlliance,

        /// <summary>The SOV bill late corp.</summary>
        SovBillLateCorp,

        /// <summary>The SOV claim lost alliance.</summary>
        SovClaimLostAlliance,

        /// <summary>The SOV claim lost corp.</summary>
        SovClaimLostCorp,

        /// <summary>The SOV claim acquired alliance.</summary>
        SovClaimAcquiredAlliance,

        /// <summary>The SOV claim acquired corp.</summary>
        SovClaimAcquiredCorp,

        /// <summary>The alliance anchoring alert.</summary>
        AllianceAnchoringAlert,

        /// <summary>The alliance structure vulnerable.</summary>
        AllianceStructureVulnerable,

        /// <summary>The alliance structure invulnerable.</summary>
        AllianceStructureInvulnerable,

        /// <summary>The sovereignty disruptor anchor.</summary>
        SovereigntyDisruptorAnchor,

        /// <summary>The structure disruptor won lost.</summary>
        StructureDisruptorWonLost,

        /// <summary>The corp office lease expired.</summary>
        CorpOfficeLeaseExpired,

        /// <summary>The clone contract revoked by station manager.</summary>
        CloneContractRevokedByStnManager,

        /// <summary>The corp member clones moved stations.</summary>
        CorpMemberClonesMovedStations,

        /// <summary>The clone contract revoked by station manager 2.</summary>
        CloneContractRevokedByStnManager2,

        /// <summary>The insurance contract expired.</summary>
        InsuranceContractExpired,

        /// <summary>The insurance contract issued.</summary>
        InsuranceContractIssued,

        /// <summary>The jump clone destroyed.</summary>
        JumpCloneDestroyed,

        /// <summary>The jump clone destroyed 2.</summary>
        JumpCloneDestroyed2,

        /// <summary>The corp join faction war.</summary>
        CorpJoinFacWar,

        /// <summary>The corp leave faction war.</summary>
        CorpLeaveFacWar,

        /// <summary>The corp kicked from faction war bad standings.</summary>
        CorpKickedFromFacWarBadStandings,

        /// <summary>The character kicked from faction war bad standings.</summary>
        CharacterKickedFromFacWarBadStandings,

        /// <summary>The corp warned faction war bad standings.</summary>
        CorpWarnedFacWarBadStandings,

        /// <summary>The character warned faction war bad standings.</summary>
        CharacterWarnedFacWarBadStandings,

        /// <summary>The agent moved.</summary>
        AgentMoved,

        /// <summary>The mass transaction reversed.</summary>
        MassTransactionReversed,

        /// <summary>The reimbursement.</summary>
        Reimbursement,

        /// <summary>The agent located character.</summary>
        AgentLocatedCharacter,

        /// <summary>The research mission available.</summary>
        ResearchMissionAvailable,

        /// <summary>The agent mission offer expires.</summary>
        AgentMissionOfferExpires,

        /// <summary>The agent mission timeout.</summary>
        AgentMissionTimeout,

        /// <summary>The agent storyline offer.</summary>
        AgentStorylineOffer,

        /// <summary>The tutorial message.</summary>
        TutorialMessage,

        /// <summary>The tower alert.</summary>
        TowerAlert,

        /// <summary>The tower resource alert.</summary>
        TowerResourceAlert,

        /// <summary>The station aggression.</summary>
        StationAggression,

        /// <summary>The station state change.</summary>
        StationStateChange,

        /// <summary>The station conquered.</summary>
        StationConquered,

        /// <summary>The station aggression 2.</summary>
        StationAggression2,

        /// <summary>The corp request join faction war.</summary>
        CorpRequestJoinFacWar,

        /// <summary>The corp request leave faction war.</summary>
        CorpRequestLeaveFacWar,

        /// <summary>The corp faction war join request withdrawn.</summary>
        CorpFacWarJoinRequestWithdrawn,

        /// <summary>The corp faction war leave request withdrawn.</summary>
        CorpFacWarLeaveRequestWithdrawn,

        /// <summary>The corporation liquidation.</summary>
        CorporationLiquidation,

        /// <summary>The TCU under attack.</summary>
        TcuUnderAttack,

        /// <summary>The SBU under attack.</summary>
        SbuUnderAttack,

        /// <summary>The IHUB under attack.</summary>
        IhubUnderAttack,

        /// <summary>The contact add.</summary>
        ContactAdd,

        /// <summary>The contact edit.</summary>
        ContactEdit,

        /// <summary>The incursion completed.</summary>
        IncursionCompleted,

        /// <summary>The corp kicked.</summary>
        CorpKicked,

        /// <summary>The custom office attacked.</summary>
        CustomOfficeAttacked,

        /// <summary>The custom office reinforced.</summary>
        CustomOfficeReinforced,

        /// <summary>The custom office transferred.</summary>
        CustomOfficeTransferred,

        /// <summary>The faction war alliance warning.</summary>
        FacWarAllianceWarning,

        /// <summary>The faction war alliance kick.</summary>
        FacWarAllianceKick,

        /// <summary>The all war corp joined.</summary>
        AllWarCorpJoined,

        /// <summary>The ally joined defender.</summary>
        AllyJoinedDefender,

        /// <summary>The ally joined aggressor.</summary>
        AllyJoinedAggressor,

        /// <summary>The ally joined ally.</summary>
        AllyJoinedAlly,

        /// <summary>The entity offers assistance.</summary>
        EntityOffersAssistance,

        /// <summary>The war surrender offered.</summary>
        WarSurrenderOffered,

        /// <summary>The war surrender declined.</summary>
        WarSurrenderDeclined,

        /// <summary>The faction war LP payout kill.</summary>
        FacWarLpPayoutKill,

        /// <summary>The faction war LP payout event.</summary>
        FacWarLpPayoutEvent,

        /// <summary>The faction war LP disqualified event.</summary>
        FacWarLpDisqualifiedEvent,

        /// <summary>The faction war LP disqualified kill.</summary>
        FacWarLpDisqualifiedKill
    }
}