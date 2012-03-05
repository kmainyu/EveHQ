' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2012  EveHQ Development Team
'
' This file is part of EveHQ.
'
' EveHQ is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
'=========================================================================
Imports System.Collections.Generic
Imports System.Linq
Imports System.Xml.Linq

Namespace RSS

    Public Interface IFeedParser
        Function Parse(ByVal URL As String) As List(Of FeedItem)
    End Interface

    Public Class FeedItem
        Dim cTitle As String = ""
        Dim cPubDate As String = ""
        Dim cLink As String = ""
        Dim cDescription As String = ""
        Dim cGUID As String = ""
        Dim cSource As String = ""
        Dim cRead As Boolean = False
        Public Property Title() As String
            Get
                Return cTitle
            End Get
            Set(ByVal value As String)
                cTitle = value
            End Set
        End Property
        Public Property PubDate() As String
            Get
                Return cPubDate
            End Get
            Set(ByVal value As String)
                cPubDate = value
            End Set
        End Property
        Public Property Link() As String
            Get
                Return cLink
            End Get
            Set(ByVal value As String)
                cLink = value
            End Set
        End Property
        Public Property Description() As String
            Get
                Return cDescription
            End Get
            Set(ByVal value As String)
                cDescription = value
            End Set
        End Property
        Public Property GUID() As String
            Get
                Return cGUID
            End Get
            Set(ByVal value As String)
                cGUID = value
            End Set
        End Property
        Public Property Source() As String
            Get
                Return cSource
            End Get
            Set(ByVal value As String)
                cSource = value
            End Set
        End Property
        Public Property Read() As Boolean
            Get
                Return cRead
            End Get
            Set(ByVal value As Boolean)
                cRead = value
            End Set
        End Property
    End Class

    Public Class RSSParser
        Implements IFeedParser

        Public Function Parse(ByVal URL As String) As System.Collections.Generic.List(Of FeedItem) Implements IFeedParser.Parse
            Try
                Dim feed As XDocument = XDocument.Load(URL)

                If feed.Elements.Count = 0 Then
                    Return Nothing
                End If

                Dim ns As XNamespace = "http://purl.org/rss/1.0/modules/slash/"

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
                        fi.GUID = item.Element("guid").Value
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

    Public Class RDFParser
        Implements IFeedParser

        Public Function Parse(ByVal URL As String) As System.Collections.Generic.List(Of FeedItem) Implements IFeedParser.Parse
            Try
                Dim feed As XDocument = XDocument.Load(URL)

                Dim dc As XNamespace = "http://purl.org/dc/elements/1.1/"
                Dim rdf As XNamespace = "http://www.w3.org/1999/02/22-rdf-syntax-ns#"
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
                    fi.GUID = item.Element(d + "link").Value
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

        Public Function Parse(ByVal URL As String) As System.Collections.Generic.List(Of FeedItem) Implements IFeedParser.Parse
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

End Namespace


