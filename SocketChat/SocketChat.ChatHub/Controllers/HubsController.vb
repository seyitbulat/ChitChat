Imports System
Imports System.Net.WebSockets
Imports System.Security.Claims
Imports System.Text
Imports System.Threading
Imports Microsoft.AspNetCore.Authorization
Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.Extensions.Logging
Imports SocketChat.ChatHub.SocketChat.ChatHub.GlobalStorage
Imports SocketChat.ChatHub.SocketChat.ChatHub.Models
Imports SocketChat.ChatHub.SocketChat.MvcUi.GlobalStroge

Namespace SocketChat.ChatHub.Controllers
    <ApiController>
    <Route("[controller]")>
    <Authorize>
    Public Class HubsController
        Inherits ControllerBase

        Private ReadOnly _chatHubManager As ChatHubManager

        Public Sub New()
            _chatHubManager = ChatHubManagerFactory.ChatHubManager
        End Sub

        <HttpGet>
        Public Async Function [Get]() As Task(Of IEnumerable(Of String))
            Dim roleTest = Await AuthorizationService.RoleCheck("User", HttpContext)

            Return _chatHubManager.GetAvailableHubs()
        End Function

        <HttpGet("/chatHub")>
        Public Async Function ChatHub(<FromQuery> hub As String, <FromQuery> user As String) As Task

            Dim roleTest = Await AuthorizationService.RoleCheck("User", HttpContext)

            If HttpContext.WebSockets.IsWebSocketRequest Then
                ' Query string'den hubName ve userId alın
                'Dim hubName = HttpContext.Request.Query("hub")
                'Dim userId = HttpContext.Request.Query("user")
                Dim hubName = hub
                Dim userId = user
                Dim userNew As New UserModel With {
                        .UserName = user,
                        .HubId = Convert.ToByte(hubName.Last)
                }

                Dim claimList = New List(Of Claim)

                claimList.Add(New Claim("userName", userId, ClaimValueTypes.String))
                claimList.Add(New Claim("hub", hubName, ClaimValueTypes.String))

                Dim userClaims = New ClaimsIdentity(claimList)
                ActiveUsers.Users.Add(userNew)

                HttpContext.User = New ClaimsPrincipal(userClaims)

                If String.IsNullOrEmpty(userId) OrElse String.IsNullOrEmpty(hubName) Then
                    HttpContext.Response.StatusCode = 400
                    Await HttpContext.Response.WriteAsync("Hub name and user ID are required")
                    Return
                End If

                Dim _chatHub = _chatHubManager.GetHub(hubName)

                If _chatHub IsNot Nothing Then
                    Dim webSocket As WebSocket = Await HttpContext.WebSockets.AcceptWebSocketAsync()
                    Await _chatHub.Connect(userId, webSocket)
                    Dim joinedMessage = $"{userId} katildi"
                    Await _chatHub.BroadcastMessage(userId, joinedMessage)
                    ' WebSocket mesajlarını almak için döngü
                    Dim buffer As New ArraySegment(Of Byte)(New Byte(4095) {})
                    While webSocket.State = WebSocketState.Open
                        Dim result As WebSocketReceiveResult = Await webSocket.ReceiveAsync(buffer, CancellationToken.None)
                        If result.MessageType = WebSocketMessageType.Text Then
                            Dim message As String = Encoding.UTF8.GetString(buffer.Array, 0, result.Count)
                            _chatHub.BroadcastMessage(userId, message)
                        ElseIf result.MessageType = WebSocketMessageType.Close Then
                            Await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None)
                            If webSocket.State = WebSocketState.Closed Then
                                Dim removeUser = HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault(Function(c) c.Type = "userName").Value

                                Dim removed = ActiveUsers.Users.FirstOrDefault(Function(e) e.UserName = removeUser)

                                ActiveUsers.Users.Remove(removed)
                            End If
                        End If
                    End While
                Else
                    ' Geçersiz ChatHub isteği
                    HttpContext.Response.StatusCode = 400
                    Await HttpContext.Response.WriteAsync("Invalid hub name")
                End If
            End If


        End Function

        Private Shared Async Function Echo(webSocket As WebSocket) As Task
            Dim buffer(1024 * 4 - 1) As Byte
            Dim receiveResult As WebSocketReceiveResult = Await webSocket.ReceiveAsync(
        New ArraySegment(Of Byte)(buffer), CancellationToken.None)

            While Not receiveResult.CloseStatus.HasValue
                Await webSocket.SendAsync(
            New ArraySegment(Of Byte)(buffer, 0, receiveResult.Count),
            receiveResult.MessageType,
            receiveResult.EndOfMessage,
            CancellationToken.None)

                receiveResult = Await webSocket.ReceiveAsync(
            New ArraySegment(Of Byte)(buffer), CancellationToken.None)
            End While


            Await webSocket.CloseAsync(
        receiveResult.CloseStatus.Value,
        receiveResult.CloseStatusDescription,
        CancellationToken.None)



        End Function



        <HttpGet("CurrentUser")>
        Public Async Function CurentUser() As Task(Of IActionResult)
            Dim roleTest = Await AuthorizationService.RoleCheck("User", HttpContext)
            Return Ok(ActiveUser.User)
        End Function

        <HttpGet("ActiveUsers")>
        Public Async Function GetUsers() As Task(Of IActionResult)
            Dim roleTest = Await AuthorizationService.RoleCheck("User", HttpContext)
            Return Ok(ActiveUsers.Users)
        End Function
    End Class


End Namespace
