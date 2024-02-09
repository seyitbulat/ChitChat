Public Class ChatHubManager
    Private ReadOnly _hubs As New Dictionary(Of String, ChatHub)()

    Public Sub New()
        ' 6 ChatHub örneği oluştur ve yöneticiye ekle
        For i As Integer = 1 To 6
            _hubs.Add("Hub" & i.ToString(), New ChatHub())
        Next
    End Sub

    Public Function GetAvailableHubs() As IEnumerable(Of String)
        ' Mevcut tüm ChatHub'ların isimlerini döndür
        Return _hubs.Keys
    End Function

    Public Function GetHub(hubName As String) As ChatHub
        ' Belirtilen isimdeki ChatHub'ı döndür
        Return If(_hubs.ContainsKey(hubName), _hubs(hubName), Nothing)
    End Function
End Class


Public Class ChatHubManagerFactory
    Private Shared _chatHubManager As ChatHubManager

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property ChatHubManager As ChatHubManager
        Get
            If _chatHubManager Is Nothing Then
                _chatHubManager = New ChatHubManager()
            End If
            Return _chatHubManager
        End Get
    End Property

    Public Shared Function Create() As ChatHubManager
        Return ChatHubManager
    End Function
End Class
