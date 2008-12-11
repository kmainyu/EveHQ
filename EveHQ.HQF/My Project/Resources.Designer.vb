﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.3053
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("EveHQ.HQF.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 10001,Radius,Radius,0,m,1
        '''10002,Mass,Mass,0,kg,1
        '''10003,Volume,Volume,0,m3,1
        '''10004,Capacity,Capacity,0,m3,1
        '''10005,Drone Control,Drone Control,0,,0
        '''10006,Fighter Control,Fighter Control,0,,0
        '''10007,Drone Control Range,Drone Control Range,0,,0
        '''10008,Fighter Control Range,Fighter Control Range,0,,0
        '''10009,Warp Core Strength,Warp Core Strength,0,,6
        '''10010,Thrust Addition,Thrust Addition,0,,6
        '''10011,energyTurretROF,Rate Of Fire,0,s,9
        '''10012,hybridTurretROF,Rate Of Fire,0,s,9
        '''10013,projectileTurretROF,Rate  [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Attributes() As String
            Get
                Return ResourceManager.GetString("Attributes", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property Clipboard1() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Clipboard1", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to #HQF Attribute Mapping Table,,,,,,,,,,
        '''#Affecting Attribute,Affecting Type,Affecting ID,Affected Attribute,Affected Type,Affected ID,Stack Nerf,Per Level,Calc Type,State,Description
        '''# SKILL ATTRIBUTE MAPPING,,,,,,,,,,
        '''# DRONES,,,,,,,,,,
        '''#,1,24613,,,,0,1,0,0,Advanced Drone Interfacing
        '''292,1,12484,114,5,12484,0,1,0,0,Amarr Drone Specialization - EM
        '''292,1,12484,116,5,12484,0,1,0,0,Amarr Drone Specialization - Explosive
        '''292,1,12484,117,5,12484,0,1,0,0,Amarr Drone Specialization - Kinetic
        '''292,1,12484,118 [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Effects() As String
            Get
                Return ResourceManager.GetString("Effects", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to #HQF Fleet Attribute Mapping Table,,,,,,,,,,,
        '''#Skill ID,Affecting Type,Affecting ID,Affected Attribute,Affected Type,Affected ID,Stack Nerf,Per Level,Calc Type,Value,State,Description
        '''20494,1,20494,265,3,6,0,1,0,2,0,Armored Warfare
        '''11569,1,11569,833,1,20069;20409;22227,0,1,0,100,0,Armored Warfare Specialist
        '''#24764,1,24764,,,,0,1,0,,0,Fleet Command
        '''20495,1,20495,76,3,6,0,1,0,2,0,Information Warfare
        '''3352,1,3352,833,1,20405;11052,0,1,0,100,0,Information Warfare Specialist
        '''3352,1,3352,1320,1,20406,0,1,0, [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property FleetEffects() As String
            Get
                Return ResourceManager.GetString("FleetEffects", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property imgArmor() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgArmor", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgCalibration() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgCalibration", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgCap() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgCap", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgCapAverage() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgCapAverage", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgCapBal() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgCapBal", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgCapPeak() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgCapPeak", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgCargo() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgCargo", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgCPU() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgCPU", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgDoomsday() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgDoomsday", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgDrone() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgDrone", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgDroneBay() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgDroneBay", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgDroneControl() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgDroneControl", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgEMResist() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgEMResist", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgExplosiveResist() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgExplosiveResist", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgHiSlot() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgHiSlot", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgInertia() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgInertia", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgInfo1() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgInfo1", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgInfo2() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgInfo2", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgKineticResist() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgKineticResist", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgLauncherSlots() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgLauncherSlots", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgLog() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgLog", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgLowSlot() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgLowSlot", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgMaxTargets() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgMaxTargets", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgMidSlot() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgMidSlot", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgMining() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgMining", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgPG() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgPG", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgPrice() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgPrice", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgRigSlot() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgRigSlot", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgScanResolution() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgScanResolution", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgSensorStregthL() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgSensorStregthL", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgSensorStrengthG() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgSensorStrengthG", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgSensorStrengthM() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgSensorStrengthM", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgSensorStrengthR() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgSensorStrengthR", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgShield() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgShield", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgSigRadius() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgSigRadius", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgSmartbomb() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgSmartbomb", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgSpeed() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgSpeed", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgStructure() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgStructure", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgTargetRange() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgTargetRange", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgThermalResist() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgThermalResist", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgTimer() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgTimer", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgTurretSlots() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgTurretSlots", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgWarpAlign() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgWarpAlign", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property imgWarpSpeed() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("imgWarpSpeed", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to #HQF Implant Mapping Table,,,,,,,,,,
        '''#Affecting Attribute,Affecting Type,Affecting ID,Affected Attribute,Affected Type,Affected ID,Calc Type,State,IsGang,Group,Implant Name
        '''1083,1,21606,265,3,6,0,0,0,Armor,Akemon&apos;s Modified &apos;Noble&apos; ZET5000
        '''1083,1,13209,265,3,6,0,0,1,Armor,Armored Warfare Mindlink
        '''884,1,13209,833,1,20069;20409;22227,0,0,1,Gang,Armored Warfare Mindlink
        '''548,1,20121,68,2,40,0,0,0,Shield;Learning,Crystal Alpha
        '''548,1,20157,68,2,40,0,0,0,Shield;Learning,Crystal Beta
        '''548,1,20159,68,2,40,0,0, [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property ImplantEffects() As String
            Get
                Return ResourceManager.GetString("ImplantEffects", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 4579,1050,1
        '''4621,1051,1
        '''10210,0,0
        '''10211,0,0
        '''10214,0,0
        '''10215,0,0
        '''10218,0,0
        '''10219,0,0
        '''10223,0,0
        '''10224,0,0
        '''10227,0,0
        '''20371,627,0
        '''20443,624,0
        '''20700,624,0
        '''21606,627,0
        '''22715,620,0
        '''22760,618,0
        '''24663,625,0
        '''24669,624,0
        '''25867,627,0
        '''25868,626,0
        '''25922,0,0
        '''26336,0,0
        '''27080,627,0
        '''27091,623,0
        '''27204,623,0
        '''27205,623,0
        '''27206,623,0
        '''27213,623,0
        '''27214,623,0
        '''27215,623,0
        '''27216,623,0
        '''27217,623,0
        '''27218,623,0
        '''27219,624,0
        '''27220,624,0
        '''27221,624,0
        '''28262,837,0
        '''28264,837,0
        '''28266,839,0
        '''28268,839,0
        '''2 [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property newItemMarketGroup() As String
            Get
                Return ResourceManager.GetString("newItemMarketGroup", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property noitem() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("noitem", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property plugin_icon() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("plugin_icon", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to #HQF Ship Bonus Mapping Table,,,,,,,,,,,
        '''#Ship ID,Affecting Type,Affecting ID,Affected Attribute,Affected Type,Affected ID,Stack Nerf,Per Level,Calc Type,Value,Status,Description
        '''# Abaddon,,,,,,,,,,,
        '''24692,1,3339,10014,4,569;573,0,1,0,5,15,Large Energy Turret Damage
        '''24692,1,3339,267,1,24692,0,1,2,5,15,Armor EM Resistance
        '''24692,1,3339,268,1,24692,0,1,2,5,15,Armor Explosive Resistance
        '''24692,1,3339,269,1,24692,0,1,2,5,15,Armor Kinetic Resistance
        '''24692,1,3339,270,1,24692,0,1,2,5,15,Armor Thermal Resistan [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property ShipEffects() As String
            Get
                Return ResourceManager.GetString("ShipEffects", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property Skills0() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Skills0", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Skills1() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Skills1", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Status_green() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Status_green", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Status_red() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Status_red", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Status_yellow() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Status_yellow", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
    End Module
End Namespace
