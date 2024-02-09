Imports System.Net.WebSockets
Imports System.Text
Imports System.Threading

Public Class ChatHub
    Private ReadOnly _connections As New Dictionary(Of String, WebSocket)()

    Public Async Function Connect(userId As String, client As WebSocket) As Task
        _connections(userId) = client
        ' Kullanıcı bağlandığında yapılacak işlemler
        ' Örneğin: Diğer kullanıcılara bir bildirim göndermek
    End Function

    Public Async Function BroadcastMessage(senderId As String, message As String) As Task
        ' Tüm bağlı istemcilere mesajı yayınlayın, mesajı gönderen dışında
        message = $"{senderId}: {message}"
        For Each kvp In _connections
            If kvp.Key <> senderId AndAlso kvp.Value.State = WebSocketState.Open Then
                Await SendMessageAsync(kvp.Value, message)
            End If
        Next
    End Function

    Private Async Function SendMessageAsync(client As WebSocket, message As String) As Task
        Dim buffer = Encoding.UTF8.GetBytes(message)
        Dim segment = New ArraySegment(Of Byte)(buffer)
        Await client.SendAsync(segment, WebSocketMessageType.Text, True, CancellationToken.None)
    End Function

    Public Sub Disconnect(userId As String)
        If _connections.ContainsKey(userId) Then
            _connections.Remove(userId)
            ' Kullanıcı ayrıldığında yapılacak işlemler
        End If
    End Sub
End Class
