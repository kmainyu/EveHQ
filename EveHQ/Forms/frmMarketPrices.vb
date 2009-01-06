Imports System.IO
Imports System.Xml
Imports System.Text

Public Class frmMarketPrices
    Dim saveLoc As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\MarketXMLs"

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
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
        Dim orderDetails() As String
        Dim regions, systems As New SortedList
        Dim regOrders, sysOrders As New ArrayList
        Dim oReg, oSys, oStation As Long
        Dim oTypeID, oType, oMinVol, oVol As Long
        Dim oPrice As Double
        Dim countBuy, countSell, countAll As Integer
        Dim volBuy, volSell, volAll As Long
        Dim minBuy, minSell, minAll As Double
        Dim maxBuy, maxSell, maxAll As Double
        Dim valBuy, valSell, valAll As Double
        Dim avgBuy, avgSell, avgAll As Double
        Dim sorBuy, sorSell, sorAll As New SortedList
        Dim medBuy, medSell, medAll As Double
        Dim stdBuy, stdSell, stdAll As Double

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
            End Select
        Next

        ' Calculate Averages
        avgAll = valAll / volAll
        avgSell = valSell / volSell
        avgBuy = valBuy / volBuy

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

        ' Calculate Standard Deviations
        Dim devBuy, devSell, devAll As Double
        For Each order As String In orderList
            orderDetails = order.Split(",".ToCharArray)
            'oReg = CLng(orderDetails(1).Trim)
            'oSys = CLng(orderDetails(2).Trim)
            'oStation = CLng(orderDetails(3).Trim)
            oType = CLng(orderDetails(5).Trim)
            'oMinVol = CLng(orderDetails(7).Trim)
            'oVol = CLng(orderDetails(8).Trim)
            oPrice = CDbl(orderDetails(6).Trim)
            devAll += Math.Pow(avgAll - oPrice, 2)
            Select Case oType
                Case 0 ' Sell order
                    devSell += Math.Pow(avgAll - oPrice, 2)
                Case 1 ' Buy order
                    devBuy += Math.Pow(avgAll - oPrice, 2)
            End Select
        Next
        stdAll = Math.Sqrt(devAll / countAll)
        stdBuy = Math.Sqrt(devBuy / countBuy)
        stdSell = Math.Sqrt(devSell / countSell)

        ' Write some XML data
        Dim fileName As String = saveLoc
        Select Case CalcType
            Case 0 ' Global
                fileName &= "\" & EveHQ.Core.HQ.itemList.GetKey(EveHQ.Core.HQ.itemList.IndexOfValue(oTypeID.ToString)).ToString.Replace(":", "-") & " (Global).xml"
            Case 1 ' Regional
                fileName &= "\" & EveHQ.Core.HQ.itemList.GetKey(EveHQ.Core.HQ.itemList.IndexOfValue(oTypeID.ToString)).ToString.Replace(":", "-") & " (Region-" & oReg & ").xml"
            Case 2 ' System
                fileName &= "\" & EveHQ.Core.HQ.itemList.GetKey(EveHQ.Core.HQ.itemList.IndexOfValue(oTypeID.ToString)).ToString.Replace(":", "-") & " (Region-" & oReg & ", System-" & oSys & ").xml"
        End Select
        Dim XMLdoc As XmlDocument = New XmlDocument
        Dim XMLS As New StringBuilder
        ' Prepare the XML document
        XMLS.AppendLine("<?xml version=""1.0"" encoding=""iso-8859-1"" ?>")
        XMLS.AppendLine("<EveHQMarket>")
        XMLS.AppendLine(Chr(9) & "<all>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<volume>" & volAll & "</volume>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<avg>" & avgAll & "</avg>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<max>" & maxAll & "</max>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<min>" & minAll & "</min>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<stddev>" & stdAll & "</stddev>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<median>" & medAll & "</median>")
        XMLS.AppendLine(Chr(9) & "</all>")
        XMLS.AppendLine(Chr(9) & "<buy>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<volume>" & volBuy & "</volume>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<avg>" & avgBuy & "</avg>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<max>" & maxBuy & "</max>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<min>" & minBuy & "</min>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<stddev>" & stdBuy & "</stddev>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<median>" & medBuy & "</median>")
        XMLS.AppendLine(Chr(9) & "</buy>")
        XMLS.AppendLine(Chr(9) & "<sell>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<volume>" & volSell & "</volume>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<avg>" & avgSell & "</avg>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<max>" & maxSell & "</max>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<min>" & minSell & "</min>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<stddev>" & stdSell & "</stddev>")
        XMLS.AppendLine(Chr(9) & Chr(9) & "<median>" & medSell & "</median>")
        XMLS.AppendLine(Chr(9) & "</sell>")
        XMLS.AppendLine("</EveHQMarket>")
        XMLdoc.LoadXml(XMLS.ToString)
        XMLdoc.Save(fileName)

        ' Calculate regional stuff if required
        If CalcType = 0 Then
            For Each Region As String In regions.Keys
                Call CalculateStats(CType(regions(Region), ArrayList), 1)
            Next
        End If
        If CalcType = 1 Then
            For Each System As String In systems.Keys
                Call CalculateStats(CType(systems(System), ArrayList), 2)
            Next
        End If

    End Sub
End Class