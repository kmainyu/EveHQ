﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.1434
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
        '''  Looks up a localized string similar to #Affecting Attribute, Affecting Group, Affected Attribute, Affected Group, Per Level
        '''424,-3426,48,0,1
        '''309,-3428,76,0,1
        '''566,-3431,564,0,1
        '''1079,-3418,482,0,1
        '''314,-3417,55,0,1
        '''313,-3413,11,0,1
        '''337,-3419,263,0,1
        '''338,-3416,479,0,1
        '''335,-3394,265,0,1
        '''327,-3392,9,0,1
        '''151,-3452,70,0,1
        '''315,-3449,37,0,1
        '''151,-20342,70,547,1
        '''151,-20533,70,547,1
        '''151,-3327,70,0,1
        '''.
        '''</summary>
        Friend ReadOnly Property Affections() As String
            Get
                Return ResourceManager.GetString("Affections", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 20,Max Velocity,131,MICROWARPDRIVE_SPEED
        '''20,Max Velocity,542,AFTERBURNER_SPEED
        '''20,Max Velocity,683,MAX_VELOCITY
        '''20,Max Velocity,801,MAX_VELOCITY
        '''64,Damage Modifier,646,PROJECTILE_TURRET_DAMAGE
        '''64,Damage Modifier,647,ENERGY_TURRET_DAMAGE
        '''64,Damage Modifier,648,HYBRID_TURRET_DAMAGE
        '''67,Capacitor,0,CAPACITOR_CAPACITY_FIXED
        '''68,Shield Bonus,0,SHIELD_BOOSTER_BOOST
        '''72,Shield HP,0,SHIELD_HP_FIXED
        '''77,Mining Amount,0,MINING_AMOUNT
        '''83,Hull HP Repaired,0,HULL_TRANSFER_AMOUNT
        '''84,Armor HP Repaired,0,ARMOR_TRAN [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Attributes() As String
            Get
                Return ResourceManager.GetString("Attributes", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to ACCESS_DIFFICULTY,Access Difficulty,Electronics
        '''AFTERBURNER_CAPACITOR,Afterburner Capacitor,Navigation
        '''AFTERBURNER_DURATION,Afterburner Duration,Navigation
        '''AFTERBURNER_SPEED,Afterburner Speed,Navigation
        '''AFTERBURNER_THRUST,Afterburner Thrust,Navigation
        '''AGILITY,Agility,Navigation
        '''AGILITY_ASC,Agility Asc,Navigation
        '''AGILITY_CAP,Agility Cap,Navigation
        '''ARCHAEOLOGICAL_FIND,Archaeological Find,Electronics
        '''ARMOR_HARDENER_ACTIVE_EM_RESIST,Armor Hardener Active EM Resist,Armor
        '''ARMOR_HARDENER_ACTIVE_EXPLOSIVE [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Bonuses() As String
            Get
                Return ResourceManager.GetString("Bonuses", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Improved Blue Pill Booster,SHIELD_BOOSTER_BOOST,25,1
        '''Improved Blue Pill Booster,SHIELD_HP_PERCENT,-25,-1
        '''Improved Blue Pill Booster,TURRET_OPTIMAL_RANGE,-25,-1
        '''Improved Blue Pill Booster,MISSILE_EXPLOSION_VELOCITY,-25,-1
        '''Improved Blue Pill Booster,CAPACITOR_CAPACITY_PERCENT,-25,-1
        '''Improved Crash Booster,MISSILE_EXPLOSION_RADIUS,-25,1
        '''Improved Crash Booster,SHIELD_BOOSTER_BOOST,-25,-1
        '''Improved Crash Booster,MAX_VELOCITY,-25,-1
        '''Improved Crash Booster,MISSILE_VELOCITY,-25,-1
        '''Improved Crash Booster,ARM [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Boosters() As String
            Get
                Return ResourceManager.GetString("Boosters", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property Clipboard1() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Clipboard1", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
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
        '''  Looks up a localized string similar to Akemon&apos;s Modified &apos;Noble&apos; ZET5000,ARMOR_HP_PERCENT,8,Armor
        '''Armored Warfare Mindlink,ARMOR_HP_PERCENT,15,Armor
        '''Armored Warfare Mindlink,MINDLINK,50,Gang
        '''Crystal Alpha,SHIELD_BOOSTER_BOOST,1,Shield
        '''Crystal Beta,SHIELD_BOOSTER_BOOST,2,Shield
        '''Crystal Delta,SHIELD_BOOSTER_BOOST,4,Shield
        '''Crystal Epsilon,SHIELD_BOOSTER_BOOST,5,Shield
        '''Crystal Gamma,SHIELD_BOOSTER_BOOST,3,Shield
        '''Crystal Omega,IMPLANT_SET,50,Science
        '''Cybernetic Subprocessor - Advanced,INTELLIGENCE,6,Learning
        '''Cybernetic Subprocessor - Basic,I [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Implants() As String
            Get
                Return ResourceManager.GetString("Implants", resourceCulture)
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
        '''  Looks up a localized string similar to Armor,MAX_VELOCITY
        '''Astronautic,ARMOR_HP_PERCENT
        '''Drone,CPU
        '''Electronics,NO
        '''Electronics Superiority,SHIELD_HP_PERCENT
        '''Energy Grid,NO
        '''Energy Weapon,ENERGY_TURRET_POWER_NEED
        '''Hybrid Weapon,HYBRID_TURRET_POWER_NEED
        '''Missile Launcher,MISSILE_LAUNCHER_CPU_NEED
        '''Projectile Weapon,PROJECTILE_TURRET_POWER_NEED
        '''Shield,SIGNATURE_RADIUS
        '''.
        '''</summary>
        Friend ReadOnly Property Rigs() As String
            Get
                Return ResourceManager.GetString("Rigs", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Abaddon,LARGE_ENERGY_TURRET_DAMAGE,5,1,Amarr Battleship
        '''Abaddon,EM_ARMOR_RESIST,5,1,Amarr Battleship
        '''Abaddon,EXPLOSIVE_ARMOR_RESIST,5,1,Amarr Battleship
        '''Abaddon,KINETIC_ARMOR_RESIST,5,1,Amarr Battleship
        '''Abaddon,THERMAL_ARMOR_RESIST,5,1,Amarr Battleship
        '''Absolution,MEDIUM_ENERGY_TURRET_CAPACITOR,-10,1,Battlecruisers
        '''Absolution,EM_ARMOR_RESIST,5,1,Battlecruisers
        '''Absolution,EXPLOSIVE_ARMOR_RESIST,5,1,Battlecruisers
        '''Absolution,KINETIC_ARMOR_RESIST,5,1,Battlecruisers
        '''Absolution,THERMAL_ARMOR_RESIST,5,1,B [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Ships() As String
            Get
                Return ResourceManager.GetString("Ships", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Acceleration Control,AFTERBURNER_SPEED,318
        '''Acceleration Control,MICROWARPDRIVE_SPEED,318
        '''Accounting,,
        '''Advanced Drone Interfacing,DRONE_CONTROL_UNIT,0
        '''Advanced Laboratory Operation,,
        '''Advanced Mass Production,,
        '''Advanced Spaceship Command,AGILITY_ASC,151
        '''Advanced Weapon Upgrades,MISSILE_LAUNCHER_POWER_NEED,323
        '''Advanced Weapon Upgrades,TURRET_POWER_NEED,323
        '''Afterburner,AFTERBURNER_DURATION,66
        '''Amarr Battleship,,
        '''Amarr Carrier,,
        '''Amarr Cruiser,,
        '''Amarr Dreadnought,,
        '''Amarr Drone Specialization,DRONE_EM [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Skills() As String
            Get
                Return ResourceManager.GetString("Skills", resourceCulture)
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
