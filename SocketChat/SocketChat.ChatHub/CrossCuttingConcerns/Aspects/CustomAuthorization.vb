Imports System.IdentityModel.Tokens.Jwt
Imports System.Security.Claims
Imports System.Text
Imports Microsoft.AspNetCore.Http
Imports Microsoft.AspNetCore.Identity
Imports Microsoft.AspNetCore.Mvc.Filters
Imports Microsoft.Extensions.Configuration
Imports Microsoft.IdentityModel.Tokens
Imports SocketChat.Business

Public Class CustomAuthorization : Inherits ActionFilterAttribute

    Private _role As String



    Public Sub New(Role As String)
        _role = Role
    End Sub




    Public Overrides Async Sub OnActionExecuting(context As ActionExecutingContext)
        Dim roleService = TryCast(context.HttpContext.RequestServices.GetService(GetType(IUserRoleService)), IUserRoleService)


        Dim tokenHandler = New JwtSecurityTokenHandler()
        Dim token = context.HttpContext.Request.Headers("Authorization").FirstOrDefault().Split(" ").ToList()(1)

        Dim hehe = tokenHandler.ReadJwtToken(token).Claims
        Dim userRoles = tokenHandler.ReadJwtToken(token).Claims.Where(Function(e) e.Type = ClaimTypes.Role).ToList()

        Dim username = tokenHandler.ReadJwtToken(token).Claims.Where(Function(e) e.Type = ClaimTypes.Name).FirstOrDefault().Value


        Dim roles = Await roleService.GetByUserAsync(username)

        If roles Is Nothing Then
            Throw New UnauthorizedException("Buna erismek icin yetkiniz yok")
            Return
        End If

        If Not roles.Data.Any(Function(e) e.Role.Name = _role) Then
            Throw New UnauthorizedException("Buna erismek icin yetkiniz yok")
            context.Result = Nothing
            Return


        End If
    End Sub



End Class
