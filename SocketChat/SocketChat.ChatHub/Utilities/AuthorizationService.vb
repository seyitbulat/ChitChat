Imports System.IdentityModel.Tokens.Jwt
Imports System.Security.Claims
Imports Microsoft.AspNetCore.Http
Imports Microsoft.IdentityModel.Tokens
Imports SocketChat.Business
Imports SocketChat.Model

Public Module AuthorizationService

    Public Function UsernameCheck(token As String, username As String) As Boolean
        Dim validToken As SecurityToken = Nothing

        Dim tokenHandler = New JwtSecurityTokenHandler()

        Dim user = tokenHandler.ReadJwtToken(token).Claims.Where(Function(e) e.Type = ClaimTypes.Name).FirstOrDefault().Value

        If user = username Then
            Return True
        End If
        Return False
    End Function

    Public Async Function RoleCheck(role As String, context As HttpContext) As Task(Of Boolean)

        Dim roleService = TryCast(context.RequestServices.GetService(GetType(IUserRoleService)), IUserRoleService)


        Dim token = context.Request.Headers("Authorization").FirstOrDefault().Split(" ").ToList()(1)

        Dim tokenHandler = New JwtSecurityTokenHandler()


        Dim username = tokenHandler.ReadJwtToken(token).Claims.Where(Function(e) e.Type = ClaimTypes.Name).FirstOrDefault().Value

        Dim roles = Await roleService.GetByUserAsync(username)

        If roles Is Nothing Then
            Throw New UnauthorizedException("Buna erismek icin yetkiniz yok")
            Return False
        End If

        If Not roles.Data.Any(Function(e) e.Role.Name = role) Then
            Throw New UnauthorizedException("Buna erismek icin yetkiniz yok")
            Return False
        End If

        Return True
    End Function

End Module
