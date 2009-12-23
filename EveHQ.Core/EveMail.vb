Public Class EveMail



End Class

Public Class EveMailMessage
    Public MessageKey As String
    Public MessageID As Long
    Public OriginatorID As Long
    Public SenderID As Long
    Public MessageDate As Date
    Public MessageTitle As String
    Public ToCorpAllianceIDs As String
    Public ToCharacterIDs As String
    Public ToListIDs As String
    Public ReadFlag As Boolean
End Class

Public Class EveNotification
    Public MessageKey As String
    Public MessageID As Long
    Public OriginatorID As Long
    Public TypeID As Long
    Public SenderID As Long
    Public MessageDate As Date
    Public ReadFlag As Boolean
End Class

Public Enum EveNotificationTypes As Integer
    Character_Deleted = 2
    Give_Medal_To_Character = 3
    Alliance_Maintenance_Bill = 4
    Alliance_War_Declared = 5
    Alliance_War_Surrender = 6
    Alliance_War_Retracted = 7
    Alliance_War_Invalidated_By_Concord = 8
    Bill_Issued_To_A_Character = 9
    Bill_Issued_To_Corporation_Or_Alliance = 10
    Bill_Not_Paid_Because_Not_Enough_ISK_Available = 11
    Bill_Issued_By_A_Character_Paid = 12
    Bill_Issued_By_A_Corporation_Or_Alliance_Paid = 13
    Bounty_Claimed = 14
    Clone_Activated = 15
    New_Corp_Member_Application = 16
    Corp_Application_Rejected = 17
    Corp_Application_Accepted = 18
    Corp_Tax_Rate_Changed = 19
    Corp_News_Report_Typically_For_Shareholders = 20
    Player_Leaves_Corp = 21
    Corp_News_New_CEO = 22
    Corp_Dividend_Liquidation_Sent_To_Shareholders = 23
    Corp_Dividend_Payout_Sent_To_Shareholders = 24
    Corp_Vote_Created = 25
    Corp_CEO_Votes_Revoked_During_Voting = 26
    Corp_Declares_War = 27
    Corp_War_Has_Started = 28
    Corp_Surrenders_War = 29
    Corp_Retracts_War = 30
    Corp_War_Invalidated_By_Concord = 31
    Container_Password_Retrieval = 32
    Contraband_Or_Low_Standings_Cause_An_Attack_Or_Items_Being_Confiscated = 33
    First_Ship_Insurance = 34
    Ship_Destroyed_Insurance_Payed = 35
    Insurance_Contract_Invalidated_Runs_Out = 36
    Sovereignty_Claim_Fails_Alliance = 37
    Sovereignty_Claim_Fails_Corporation = 38
    Sovereignty_Bill_Late_Alliance = 39
    Sovereignty_Bill_Late_Corporation = 40
    Sovereignty_Claim_Lost_Alliance = 41
    Sovereignty_Claim_Lost_Corporation = 42
    Sovereignty_Claim_Aquired_Alliance = 43
    Sovereignty_Claim_Aquired_Corporation = 44
    Alliance_Anchoring_Alert = 45
    Alliance_Structure_Turns_Vulnerable = 46
    Alliance_Structure_Turns_Invulnerable = 47
    Sovereignty_Disruptor_Anchored = 48
    Structure_Won_Lost = 49
    Corp_Office_Lease_Expiration_Notice = 50
    Clone_Contract_Revoked_By_Station_Manager = 51
    Corp_Member_Clones_Moved_Between_Stations = 52
    Clone_Contract_Revoked_By_Station_Manager2 = 53
    Insurance_Contract_Expired = 54
    Insurance_Contract_Issued = 55
    Jump_Clone_Destroyed = 56
    Jump_Clone_Destroyed2 = 57
    Corporation_Joining_Factional_Warfare = 58
    Corporation_Leaving_Factional_Warfare = 59
    Corporation_Kicked_From_Factional_Warfare_On_Startup_Because_Of_Too_Low_Standing_To_The_Faction = 60
    Character_Kicked_From_Factional_Warfare_On_Startup_Because_Of_Too_Low_Standing_To_The_Faction = 61
    Corporation_In_Factional_Warfare_Warned_On_Startup_Because_Of_Too_Low_Standing_To_The_Faction = 62
    Character_In_Factional_Warfare_Warned_On_Startup_Because_Of_Too_Low_Standing_To_The_Faction = 63
    Character_Loses_Factional_Warfare_Rank = 64
    Character_Gains_Factional_Warfare_Rank = 65
    Agent_Has_Moved = 66
    Mass_Transaction_Reversal_Message = 67
    Reimbursement_Message = 68
    Agent_Locates_A_Character = 69
    Research_Mission_Becomes_Available_From_An_Agent = 70
    Agent_Mission_Offer_Expires = 71
    Agent_Mission_Times_Out = 72
    Agent_Offers_A_Storyline_Mission = 73
    Tutorial_Message_Sent_On_Character_Creation = 74
    Tower_Alert = 75
    Tower_Resource_Alert = 76
    Station_Aggression_Message = 77
    Station_State_Change_Message = 78
    Station_Conquered_Message = 79
    Station_Aggression_Message2 = 80
    Corporation_Requests_Joining_Factional_Warfare = 81
    Corporation_Requests_Leaving_Factional_Warfare = 82
    Corporation_Withdrawing_A_Request_To_Join_Factional_Warfare = 83
    Corporation_Withdrawing_A_Request_To_Leave_Factional_Warfare = 84
End Enum
