﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.3074
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
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("EveHQ.Resources", GetType(Resources).Assembly)
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
        
        Friend ReadOnly Property CertificateCategories() As Byte()
            Get
                Dim obj As Object = ResourceManager.GetObject("CertificateCategories", resourceCulture)
                Return CType(obj,Byte())
            End Get
        End Property
        
        Friend ReadOnly Property CertificateClasses() As Byte()
            Get
                Dim obj As Object = ResourceManager.GetObject("CertificateClasses", resourceCulture)
                Return CType(obj,Byte())
            End Get
        End Property
        
        Friend ReadOnly Property Certificates() As Byte()
            Get
                Dim obj As Object = ResourceManager.GetObject("Certificates", resourceCulture)
                Return CType(obj,Byte())
            End Get
        End Property
        
        Friend ReadOnly Property Collapse() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Collapse", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Collapse_h() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Collapse_h", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property CollapsedHighlightImage() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("CollapsedHighlightImage", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property CollapsedImage() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("CollapsedImage", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property doh3() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("doh3", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&apos;1.0&apos; encoding=&apos;UTF-8&apos;?&gt;
        '''&lt;eveapi version=&quot;2&quot;&gt;
        '''  &lt;result&gt;
        '''    &lt;rowset name=&quot;errors&quot; key=&quot;errorCode&quot; columns=&quot;errorCode,errorText&quot;&gt;
        '''      &lt;row errorCode=&quot;100&quot; errorText=&quot;Expected before ref/trans ID = 0: wallet not previously loaded.&quot; /&gt;
        '''      &lt;row errorCode=&quot;101&quot; errorText=&quot;Wallet exhausted: retry after {0}.&quot; /&gt;
        '''      &lt;row errorCode=&quot;102&quot; errorText=&quot;Expected before ref/trans ID [{0}] but supplied [{1}]: wallet previously loaded.&quot; /&gt;
        '''      &lt;row errorCode=&quot;103&quot; errorText=&quot;Already returned  [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Errors() As String
            Get
                Return ResourceManager.GetString("Errors", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property EveHQ() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("EveHQ", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        Friend ReadOnly Property EveHQ_offline() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("EveHQ_offline", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        Friend ReadOnly Property EveHQ_online() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("EveHQ_online", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        Friend ReadOnly Property EveHQ_starting() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("EveHQ_starting", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        Friend ReadOnly Property Expand() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Expand", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Expand_h() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Expand_h", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property ExpandedHighlightImage() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("ExpandedHighlightImage", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property ExpandedImage() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("ExpandedImage", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to EveHQ - Version History
        '''-----------------------
        '''
        '''1.8.5.420 (11/03/2009)
        '''---------
        '''New Features:
        '''- Prism: Introducing the new Industry plug-in containing various viewers for wallet, orders and jobs
        '''- Recycling: Added recycling information into the Prism plug-in
        '''- Certificates: Added a certificate viewer to the Pilot Info screen together with a new Certificate Details form
        '''- Certificate Planning: Now you are able to make skill queues to fulfil your certificate targets
        '''- User Database: A new database [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property History() As String
            Get
                Return ResourceManager.GetString("History", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property icon22_41() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("icon22_41", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property info_grey() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("info_grey", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property info_icon() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("info_icon", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property level0() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("level0", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property level1() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("level1", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property level1_act() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("level1_act", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property level2() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("level2", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property level2_act() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("level2_act", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property level3() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("level3", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property level3_act() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("level3_act", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property level4() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("level4", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property level4_act() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("level4_act", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property level5() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("level5", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property level5_act() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("level5_act", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property noitem() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("noitem", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property panel_close() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("panel_close", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property panel_open() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("panel_open", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Splashv1() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Splashv1", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Splashv2() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Splashv2", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Splashv3() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Splashv3", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Splashv4() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Splashv4", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Splashv5() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Splashv5", resourceCulture)
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
