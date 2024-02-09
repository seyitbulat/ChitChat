Imports System.Net.Http

Public Class ApiHelper
    Public Shared Property HttpClient As HttpClient

    Public Sub New()
        HttpClient = New HttpClient()
        HttpClient.BaseAddress = New Uri("http://10.19.10.42:88")
    End Sub

    Public Shared Function Create()
        HttpClient = New HttpClient()
        HttpClient.BaseAddress = New Uri("http://10.19.10.42:88")
        Return HttpClient
    End Function

End Class
