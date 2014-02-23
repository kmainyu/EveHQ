'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports System.Xml.Linq

Public Interface IFeedParser
    Function Parse(ByVal url As String) As List(Of FeedItem)
End Interface

Public Class FeedItem
    Dim _title As String = ""
    Dim _pubDate As String = ""
    Dim _link As String = ""
    Dim _description As String = ""
    Dim _guid As String = ""
    Dim _source As String = ""
    Dim _read As Boolean = False
    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set
    End Property
    Public Property PubDate() As String
        Get
            Return _pubDate
        End Get
        Set(ByVal value As String)
            _pubDate = value
        End Set
    End Property
    Public Property Link() As String
        Get
            Return _link
        End Get
        Set(ByVal value As String)
            _link = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property
    Public Property Guid() As String
        Get
            Return _guid
        End Get
        Set(ByVal value As String)
            _guid = value
        End Set
    End Property
    Public Property Source() As String
        Get
            Return _source
        End Get
        Set(ByVal value As String)
            _source = value
        End Set
    End Property
    Public Property Read() As Boolean
        Get
            Return _read
        End Get
        Set(ByVal value As Boolean)
            _read = value
        End Set
    End Property
End Class

Public Class RssParser
    Implements IFeedParser

    Public Function Parse(ByVal url As String) As List(Of FeedItem) Implements IFeedParser.Parse
        Try
            Dim feed As XDocument = XDocument.Load(url)

            If feed.Elements.Count = 0 Then
                Return Nothing
            End If

            Dim source As String = (From ele In feed.Descendants("channel").Elements("title") Select ele.Value).[Single]

            Dim items As New List(Of FeedItem)
            For Each item As XElement In feed.Descendants("item")
                Dim fi As New FeedItem
                fi.Title = item.Element("title").Value
                fi.Link = item.Element("link").Value
                fi.PubDate = IIf(item.Element("pubDate") IsNot Nothing, item.Element("pubDate").Value, "").ToString
                If item.Element("description") IsNot Nothing Then
                    fi.Description = item.Element("description").Value
                Else
                    fi.Description = ""
                End If
                If item.Element("guid") IsNot Nothing Then
                    fi.Guid = item.Element("guid").Value
                Else
                    fi.Guid = fi.Link
                End If
                fi.Source = source
                items.Add(fi)
            Next

            Return items

        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class

Public Class RdfParser
    Implements IFeedParser

    Public Function Parse(ByVal url As String) As List(Of FeedItem) Implements IFeedParser.Parse
        Try
            Dim feed As XDocument = XDocument.Load(url)

            Dim dc As XNamespace = "http://purl.org/dc/elements/1.1/"
            Dim d As XNamespace = "http://purl.org/rss/1.0/"

            Dim source As String = (From ele In feed.Descendants(d + "channel").Elements(d + "title") Select ele.Value).Single

            Dim items As New List(Of FeedItem)
            For Each item As XElement In feed.Descendants(d + "item")
                Dim fi As New FeedItem
                fi.Title = item.Element(d + "title").Value
                fi.Link = item.Element(d + "link").Value
                If item.Element(dc + "date") IsNot Nothing Then
                    fi.PubDate = item.Element(dc + "date").Value
                Else
                    fi.PubDate = ""
                End If
                If item.Element(dc + "description") IsNot Nothing Then
                    fi.Description = item.Element(dc + "description").Value
                Else
                    fi.Description = ""
                End If
                fi.Guid = item.Element(d + "link").Value
                fi.Source = source
                items.Add(fi)
            Next

            Return items

        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class

Public Class AtomParser
    Implements IFeedParser

    Public Function Parse(ByVal url As String) As List(Of FeedItem) Implements IFeedParser.Parse
        Try
            Dim feed As XDocument = XDocument.Load(URL)

            Dim d As XNamespace = "http://www.w3.org/2005/Atom"

            Dim source As String = (From ele In feed.Descendants(d + "feed").Elements(d + "title") Select ele.Value).Single()

            Dim items As New List(Of FeedItem)
            For Each entry As XElement In feed.Descendants(d + "entry")
                Dim fi As New FeedItem
                fi.Title = entry.Element(d + "title").Value
                fi.Link = entry.Element(d + "link").Attribute("href").Value
                If entry.Element(d + "updated") IsNot Nothing Then
                    fi.PubDate = entry.Element(d + "updated").Value
                Else
                    fi.PubDate = ""
                End If
                If entry.Element(d + "summary") IsNot Nothing Then
                    fi.Description = entry.Element(d + "summary").Value
                Else
                    fi.Description = ""
                End If
                If entry.Element(d + "id") IsNot Nothing Then
                    fi.GUID = entry.Element(d + "id").Value
                Else
                    fi.GUID = fi.Link
                End If
                fi.Source = source
                items.Add(fi)
            Next

            Return items

        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class

Public Class ParserFactory
    Public Shared Function GetParser(ByVal url As String) As IFeedParser
        If url.Trim().Length = 0 Then
            Return Nothing
        End If

        Dim feed As XDocument

        Try
            feed = XDocument.Load(url)

            Dim element As XElement = feed.Element("rss")
            If element IsNot Nothing Then
                Return New RSSParser()
            End If

            Dim rdf As XNamespace = "http://www.w3.org/1999/02/22-rdf-syntax-ns#"
            element = feed.Element(rdf + "RDF")
            If element IsNot Nothing Then
                Return New RDFParser()
            End If

            Dim atom As XNamespace = "http://www.w3.org/2005/Atom"
            element = feed.Element(atom + "feed")
            If element IsNot Nothing Then
                Return New AtomParser()
            End If
        Catch
            Return Nothing
        End Try
        Return Nothing
    End Function
End Class