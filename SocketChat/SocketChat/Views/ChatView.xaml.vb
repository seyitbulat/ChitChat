Imports System.Collections.ObjectModel
Imports System.Net.WebSockets
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic.ApplicationServices

Public Class ChatView
    Public Property ChatHistory As New ObservableCollection(Of String)
    Public Property CurrentHub As String
    Public Property CurrentUser As User


    Private _clientWebSocket As ClientWebSocket
    Private _cancellationToken As CancellationToken

    Public Sub New(currentHub As String, user As User)
        InitializeComponent()
        Me.CurrentUser = user
        ChatHubLbl.Content = currentHub & "(" & user.UserName & ")"
        Me.CurrentHub = currentHub
        ChatList.ItemsSource = ChatHistory
        InitializeWebSocket()
    End Sub

    Private Async Sub SendMessageButton_Click(sender As Object, e As RoutedEventArgs)
        ' Mesajı ChatHistory'ye ekleyin
        Dim messageToSend As String = $"{CurrentUser.UserName}: " & MessageTextBox.Text
        ChatHistory.Add(messageToSend)
        MessageTextBox.Clear()

        ' WebSocket üzerinden mesaj gönderme
        If _clientWebSocket IsNot Nothing AndAlso _clientWebSocket.State = WebSocketState.Open Then
            Dim messageBytes As Byte() = Encoding.UTF8.GetBytes(messageToSend)
            Dim segment As New ArraySegment(Of Byte)(messageBytes)

            Try
                Await _clientWebSocket.SendAsync(segment, WebSocketMessageType.Text, True, _cancellationToken)
            Catch ex As Exception
                ' Hata mesajını loglayın veya gösterin
            End Try
        End If
    End Sub

    Private Async Sub InitializeWebSocket()
        _clientWebSocket = New ClientWebSocket()
        _cancellationToken = New CancellationToken()

        Try
            ' WebSocket sunucusuna bağlanın
            Await _clientWebSocket.ConnectAsync(New Uri($"ws://10.19.10.42:88/chatHub?hub={CurrentHub}&user={CurrentUser.UserName}"), _cancellationToken)
            ListenForMessages()
        Catch ex As Exception
            ' Bağlantı hatası işleme
        End Try
    End Sub

    Private Async Sub ListenForMessages()
        Dim result As WebSocketReceiveResult
        While _clientWebSocket.State = WebSocketState.Open
            Dim buffer = New Byte(1024) {}
            Try
                result = Await _clientWebSocket.ReceiveAsync(New ArraySegment(Of Byte)(buffer), _cancellationToken)

            Catch ex As Exception
                ' Hata mesajını loglayın veya gösterin
                Throw ex
            End Try

            If result.MessageType = WebSocketMessageType.Text Then
                Dim message = Encoding.UTF8.GetString(buffer, 0, result.Count)
                ' UI thread üzerinde çalıştığınızdan emin olun
                Dispatcher.Invoke(Sub()
                                      ChatHistory.Add(message)
                                  End Sub)
            End If
        End While
    End Sub
End Class
