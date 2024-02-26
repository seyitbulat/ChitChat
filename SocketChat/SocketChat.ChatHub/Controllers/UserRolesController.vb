Imports System
Imports Microsoft.AspNetCore.Authorization
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.Extensions.Logging
Imports SocketChat.Business
Imports SocketChat.Model

Namespace SocketChat.ChatHub.Controllers
    <ApiController>
    <Route("[controller]")>
    <Authorize>
    Public Class UserRolesController
        Inherits BaseController

        Private ReadOnly _userRoleService As IUserRoleService

        Public Sub New(userRoleService As IUserRoleService)
            _userRoleService = userRoleService
        End Sub

        <HttpGet("GetUserRoles")>
        Public Async Function GetUserRoles(<FromQuery> id As Long) As Task(Of IActionResult)
            Dim response = Await _userRoleService.GetByUserAsync(id)

            Return SendResponse(response)
        End Function


        <HttpPost>
        Public Async Function AddUserRoles(<FromBody> dto As UserRolePostDto) As Task(Of IActionResult)
            Dim response = Await _userRoleService.AddAsync(dto)

            Return SendResponse(response)
        End Function
    End Class
End Namespace
