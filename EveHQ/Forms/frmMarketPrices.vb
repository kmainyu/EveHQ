Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.Data.SqlClient
Imports System.Data

Public Class frmMarketPrices
    Dim saveLoc As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\MarketXMLs"
    Dim insertStat As String = "INSERT INTO marketStats (statDate, typeID, regionID, systemID, volAll, avgAll, maxAll, minAll, stdAll, medAll, volBuy, avgBuy, maxBuy, minBuy, stdBuy, medBuy, volSell, avgSell, maxSell, minSell, stdSell, medSell) "
    Dim strSQL As String = ""
    Dim orderDetails() As String
    Dim oReg, oSys, oStation As Long
    Dim oTypeID, oType, oMinVol, oVol As Long
    Dim oPrice As Double
    Dim avgBuy, avgSell, avgAll As Double
    Dim medBuy, medSell, medAll As Double
    Dim stdBuy, stdSell, stdAll As Double
    Dim sorBuy, sorSell, sorAll As New SortedList
    Dim countBuy, countSell, countAll As Integer
    Dim volBuy, volSell, volAll As Long
    Dim minBuy, minSell, minAll As Double
    Dim maxBuy, maxSell, maxAll As Double
    Dim valBuy, valSell, valAll As Double
    Dim devBuy, devSell, devAll As Double

    Private Function CheckMarketStatsDBTable() As Boolean
        Dim CreateTable As Boolean = False
        Dim tables As ArrayList = EveHQ.Core.DataFunctions.GetDatabaseTables
        If tables IsNot Nothing Then
            If tables.Contains("marketStats") = False Then
                ' The DB exists but the table doesn't so we'll create this
                CreateTable = True
            Else
                ' We have the Db and table so we can return a good result
                Return True
            End If
        Else
            ' Database doesn't exist?
            Dim msg As String = "EveHQ has detected that the new storage database is not initialised." & ControlChars.CrLf
            msg &= "This database will be used to store EveHQ specific data such as market prices and financial data." & ControlChars.CrLf
            msg &= "Defaults will be setup that you can amend later via the Database Settings. Click OK to initialise the new database."
            MessageBox.Show(msg, "EveHQ Database Initialisation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If EveHQ.Core.DataFunctions.CreateEveHQDataDB = False Then
                MessageBox.Show("There was an error creating the EveHQData database. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                MessageBox.Show("Database created successfully!", "Database Creation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                CreateTable = True
            End If
        End If

        ' Create the database table 
        If CreateTable = True Then
            Dim strSQL As New StringBuilder
            strSQL.AppendLine("CREATE TABLE marketStats")
            strSQL.AppendLine("(")
            strSQL.AppendLine("  statID         int IDENTITY(1,1),")
            strSQL.AppendLine("  statDate       datetime,")
            strSQL.AppendLine("  typeID         int,")
            strSQL.AppendLine("  regionID       int,")
            strSQL.AppendLine("  systemID       int,")
            strSQL.AppendLine("  volAll         float,")
            strSQL.AppendLine("  avgAll         float,")
            strSQL.AppendLine("  maxAll         float,")
            strSQL.AppendLine("  minAll         float,")
            strSQL.AppendLine("  stdAll         float,")
            strSQL.AppendLine("  medAll         float,")
            strSQL.AppendLine("  volBuy         float,")
            strSQL.AppendLine("  avgBuy         float,")
            strSQL.AppendLine("  maxBuy         float,")
            strSQL.AppendLine("  minBuy         float,")
            strSQL.AppendLine("  stdBuy         float,")
            strSQL.AppendLine("  medBuy         float,")
            strSQL.AppendLine("  volSell         float,")
            strSQL.AppendLine("  avgSell         float,")
            strSQL.AppendLine("  maxSell         float,")
            strSQL.AppendLine("  minSell         float,")
            strSQL.AppendLine("  stdSell         float,")
            strSQL.AppendLine("  medSell         float,")
            strSQL.AppendLine("")
            strSQL.AppendLine("  CONSTRAINT marketStats_PK PRIMARY KEY CLUSTERED (statID)")
            strSQL.AppendLine(")")
            strSQL.AppendLine("CREATE NONCLUSTERED INDEX marketStats_IX_type ON marketStats (typeID)")
            strSQL.AppendLine("CREATE NONCLUSTERED INDEX marketStats_IX_region ON marketStats (regionID)")
            strSQL.AppendLine("CREATE NONCLUSTERED INDEX marketStats_IX_system ON marketStats (systemID)")
            If EveHQ.Core.DataFunctions.SetData(strSQL.ToString) = True Then
                Return True
            Else
                MessageBox.Show("There was an error creating the marketStats database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError, "Error Creating Database", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            End If
        End If

    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        ' Check we have the EveHQDB Database and MarketStats table available
        If Me.CheckMarketStatsDBTable = False Then
            MessageBox.Show("EveHQ cannot proceed with the market price processing until the EveHQData storage database is available", "Error In Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim startTime, endTime As Date
        Dim timeTaken As TimeSpan

        ofd1.ShowDialog()
        Dim filename As String = ofd1.FileName
        Dim sr As New StreamReader(filename)
        Dim header As String = sr.ReadLine()
        Dim items As New SortedList
        Dim itemOrders As New ArrayList

        If header <> "orderid, regionid, systemid, stationid, typeid, bid, price, minvolume, volremain, volenter, issued, duration, range, reportedby, reportedtime" Then
            MessageBox.Show("File is not a valid Eve-Central dump file", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            startTime = Now
            Dim order As String = ""
            Dim orders As Long = 0
            Dim orderDetails() As String
            Do
                order = sr.ReadLine
                orderDetails = order.Split(",".ToCharArray)
                ' Add to the relevant item list
                If items.Contains(orderDetails(4).Trim) = False Then
                    itemOrders = New ArrayList
                    itemOrders.Add(order)
                    items.Add(orderDetails(4).Trim, itemOrders)
                Else
                    itemOrders = CType(items(orderDetails(4).Trim), ArrayList)
                    itemOrders.Add(order)
                End If

                orders += 1
            Loop Until sr.EndOfStream
            sr.Close()

            endTime = Now
            timeTaken = endTime - startTime
            MessageBox.Show("Time taken to read " & orders.ToString & "orders = " & timeTaken.TotalSeconds.ToString)

            EveHQ.Core.DataFunctions.SetData("SET DATEFORMAT dmy;")

            ' Calculate global statistics
            startTime = Now
            For Each item As String In items.Keys
                Call CalculateStats(CType(items(item), ArrayList), 0)
            Next

            endTime = Now
            timeTaken = endTime - startTime
            MessageBox.Show("Time taken to parse data for " & items.Count.ToString & " items = " & timeTaken.TotalSeconds.ToString)

        End If

    End Sub

    Private Sub CalculateStats(ByVal orderList As ArrayList, ByVal CalcType As Integer)

        Dim regions, systems As New SortedList
        Dim regOrders, sysOrders As New ArrayList
        sorBuy.Clear() : sorSell.Clear() : sorAll.Clear()

        countBuy = 0 : countSell = 0 : countAll = 0
        volBuy = 0 : volSell = 0 : volAll = 0
        minBuy = 0 : minSell = 0 : minAll = 0
        maxBuy = 0 : maxSell = 0 : maxAll = 0
        valBuy = 0 : valSell = 0 : valAll = 0
        devBuy = 0 : devSell = 0 : devAll = 0

        For Each order As String In orderList
            orderDetails = order.Split(",".ToCharArray)
            oReg = CLng(orderDetails(1).Trim)
            oSys = CLng(orderDetails(2).Trim)
            oStation = CLng(orderDetails(3).Trim)
            oTypeID = CLng(orderDetails(4).Trim)
            oType = CLng(orderDetails(5).Trim)
            oMinVol = CLng(orderDetails(7).Trim)
            oVol = CLng(orderDetails(8).Trim)
            oPrice = CDbl(orderDetails(6).Trim)

            If CalcType = 0 Then
                ' Add this to a regional order processing list if required
                If regions.Contains(oReg.ToString) = False Then
                    regOrders = New ArrayList
                    regOrders.Add(order)
                    regions.Add(oReg.ToString, regOrders)
                Else
                    regOrders = CType(regions(oReg.ToString), ArrayList)
                    regOrders.Add(order)
                End If
            End If
            If CalcType = 1 Then
                ' Add this to a system order processing list if required
                If systems.Contains(oSys.ToString) = False Then
                    sysOrders = New ArrayList
                    sysOrders.Add(order)
                    systems.Add(oSys.ToString, sysOrders)
                Else
                    sysOrders = CType(systems(oSys.ToString), ArrayList)
                    sysOrders.Add(order)
                End If
            End If

            countAll += 1
            volAll += oVol
            If oPrice < minAll Or minAll = 0 Then
                minAll = oPrice
            End If
            If oPrice > maxAll Then
                maxAll = oPrice
            End If
            valAll += (oVol * oPrice)
            If sorAll.Contains(oPrice.ToString) = False Then
                sorAll.Add(oPrice.ToString, oVol)
            Else
                sorAll(oPrice.ToString) = CLng(sorAll(oPrice.ToString)) + oVol
            End If
            devAll += Math.Pow(oPrice, 2) * oVol

            Select Case oType
                Case 0 ' Sell order
                    countBuy += 1
                    volSell += oVol
                    If oPrice < minSell Or minSell = 0 Then
                        minSell = oPrice
                    End If
                    If oPrice > maxSell Then
                        maxSell = oPrice
                    End If
                    valSell += (oVol * oPrice)
                    If sorSell.Contains(oPrice.ToString) = False Then
                        sorSell.Add(oPrice.ToString, oVol)
                    Else
                        sorSell(oPrice.ToString) = CLng(sorSell(oPrice.ToString)) + oVol
                    End If
                    devSell += Math.Pow(oPrice, 2) * oVol
                Case 1 ' Buy order
                    countSell += 1
                    volBuy += oVol
                    If oPrice < minBuy Or minBuy = 0 Then
                        minBuy = oPrice
                    End If
                    If oPrice > maxBuy Then
                        maxBuy = oPrice
                    End If
                    valBuy += (oVol * oPrice)
                    If sorBuy.Contains(oPrice.ToString) = False Then
                        sorBuy.Add(oPrice.ToString, oVol)
                    Else
                        sorBuy(oPrice.ToString) = CLng(sorBuy(oPrice.ToString)) + oVol
                    End If
                    devBuy += Math.Pow(oPrice, 2) * oVol
            End Select
        Next

        ' Calculate Medians
        Dim cumVol As Long = 0
        For Each chkVol As String In sorAll.Keys
            cumVol += CLng(sorAll(chkVol))
            If cumVol >= (volAll / 2) Then
                medAll = CDbl(chkVol)
                Exit For
            End If
        Next
        For Each chkVol As String In sorSell.Keys
            cumVol += CLng(sorSell(chkVol))
            If cumVol >= (volSell / 2) Then
                medSell = CDbl(chkVol)
                Exit For
            End If
        Next
        For Each chkVol As String In sorBuy.Keys
            cumVol += CLng(sorBuy(chkVol))
            If cumVol >= (volBuy / 2) Then
                medBuy = CDbl(chkVol)
                Exit For
            End If
        Next

        ' Calculate Averages & Standard Deviations
        If volAll > 0 Then
            avgAll = valAll / volAll
            stdAll = Math.Sqrt(Math.Abs((devAll / volAll) - Math.Pow(avgAll, 2)))
        Else
            avgAll = 0 : stdAll = 0
        End If
        If volSell > 0 Then
            avgSell = valSell / volSell
            stdSell = Math.Sqrt(Math.Abs((devSell / volSell) - Math.Pow(avgSell, 2)))
        Else
            avgSell = 0 : stdSell = 0
        End If
        If volBuy > 0 Then
            avgBuy = valBuy / volBuy
            stdBuy = Math.Sqrt(Math.Abs((devBuy / volBuy) - Math.Pow(avgBuy, 2)))
        Else
            avgBuy = 0 : stdBuy = 0
        End If

        ' Write some XML data
        'Dim fileName As String = saveLoc
        'Select Case CalcType
        '    Case 0 ' Global
        '        fileName &= "\" & EveHQ.Core.HQ.itemList.GetKey(EveHQ.Core.HQ.itemList.IndexOfValue(oTypeID.ToString)).ToString.Replace(":", "-") & " (Global).xml"
        '    Case 1 ' Regional
        '        fileName &= "\" & EveHQ.Core.HQ.itemList.GetKey(EveHQ.Core.HQ.itemList.IndexOfValue(oTypeID.ToString)).ToString.Replace(":", "-") & " (Region-" & oReg & ").xml"
        '    Case 2 ' System
        '        fileName &= "\" & EveHQ.Core.HQ.itemList.GetKey(EveHQ.Core.HQ.itemList.IndexOfValue(oTypeID.ToString)).ToString.Replace(":", "-") & " (Region-" & oReg & ", System-" & oSys & ").xml"
        'End Select
        'Dim XMLdoc As XmlDocument = New XmlDocument
        'Dim XMLS As New StringBuilder
        '' Prepare the XML document
        'XMLS.AppendLine("<?xml version=""1.0"" encoding=""iso-8859-1"" ?>")
        'XMLS.AppendLine("<EveHQMarket>")
        'XMLS.AppendLine(Chr(9) & "<all>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<volume>" & volAll & "</volume>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<avg>" & avgAll & "</avg>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<max>" & maxAll & "</max>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<min>" & minAll & "</min>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<stddev>" & stdAll & "</stddev>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<median>" & medAll & "</median>")
        'XMLS.AppendLine(Chr(9) & "</all>")
        'XMLS.AppendLine(Chr(9) & "<buy>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<volume>" & volBuy & "</volume>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<avg>" & avgBuy & "</avg>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<max>" & maxBuy & "</max>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<min>" & minBuy & "</min>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<stddev>" & stdBuy & "</stddev>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<median>" & medBuy & "</median>")
        'XMLS.AppendLine(Chr(9) & "</buy>")
        'XMLS.AppendLine(Chr(9) & "<sell>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<volume>" & volSell & "</volume>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<avg>" & avgSell & "</avg>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<max>" & maxSell & "</max>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<min>" & minSell & "</min>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<stddev>" & stdSell & "</stddev>")
        'XMLS.AppendLine(Chr(9) & Chr(9) & "<median>" & medSell & "</median>")
        'XMLS.AppendLine(Chr(9) & "</sell>")
        'XMLS.AppendLine("</EveHQMarket>")
        'XMLdoc.LoadXml(XMLS.ToString)
        'XMLdoc.Save(fileName)
        Select Case CalcType
            Case 0
                oReg = 0
                oSys = 0
            Case 1
                oSys = 0
        End Select

        ' Write data to the database
        strSQL = insertStat & "VALUES (" & Now.ToOADate & ", " & oTypeID.ToString & ", " & oReg.ToString & ", " & oSys.ToString & ", " & volAll & ", " & avgAll & ", " & maxAll & ", " & minAll & ", " & stdAll & ", " & medAll & ", " & volBuy & ", " & avgBuy & ", " & maxBuy & ", " & minBuy & ", " & stdBuy & ", " & medBuy & ", " & volSell & ", " & avgSell & ", " & maxSell & ", " & minSell & ", " & stdSell & ", " & medSell & ");"
        If EveHQ.Core.DataFunctions.SetData(strSQL) = False Then
            MessageBox.Show("There was an error writing data to the marketStats database table. The error was: " & ControlChars.CrLf & ControlChars.CrLf & EveHQ.Core.HQ.dataError & ControlChars.CrLf & ControlChars.CrLf & "Data: " & strSQL, "Error Writing Market Stats", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        ' Calculate regional stuff if required
        If CalcType = 0 Then
            For Each Region As String In regions.Keys
                Call CalculateStats(CType(regions(Region), ArrayList), 1)
            Next
        End If
        'If CalcType = 1 Then
        '    For Each System As String In systems.Keys
        '        Call CalculateStats(CType(systems(System), ArrayList), 2)
        '    Next
        'End If

    End Sub


End Class