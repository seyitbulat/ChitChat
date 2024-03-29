﻿Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Mvc
Imports SocketChat.Business

Namespace SocketChat.ChatHub.Controllers
    <ApiController>
    <Route("[controller]")>
    Public Class BaseController
        Inherits ControllerBase

        <NonAction>
        Public Function SendResponse(Of T)(response As ApiResponse(Of T)) As IActionResult
            If response.StatusCode = StatusCodes.Status204NoContent Then
                Return New ObjectResult(Nothing) With {.StatusCode = response.StatusCode}
            End If
            Return New ObjectResult(response) With {.StatusCode = response.StatusCode}
        End Function


    End Class
End Namespace
