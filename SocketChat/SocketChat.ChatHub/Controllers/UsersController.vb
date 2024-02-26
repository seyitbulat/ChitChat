Imports System
Imports System.IdentityModel.Tokens.Jwt
Imports System.Security.Claims
Imports System.Text
Imports System.Text.Json
Imports Microsoft.AspNetCore.Authentication
Imports Microsoft.AspNetCore.Authorization
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.Extensions.Caching.Memory
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.Logging
Imports Microsoft.IdentityModel.Tokens
Imports SocketChat.Business
Imports SocketChat.DataAccess
Imports SocketChat.Model

Namespace SocketChat.ChatHub.Controllers
    <ApiController>
    <Route("[controller]")>
    <Authorize>
    Public Class UsersController
        Inherits BaseController

        Private ReadOnly _userService As IUserService
        Private ReadOnly _memoryCache As IMemoryCache
        Private ReadOnly _cacheModule As ICacheModule
        Private ReadOnly _configuration As IConfiguration
        Private ReadOnly _userRoleService As IUserRoleService



        Public Sub New(userService As IUserService, memoryCache As IMemoryCache, cacheModule As ICacheModule, configuration As IConfiguration, userRoleService As IUserRoleService)
            _userService = userService
            _memoryCache = memoryCache
            _cacheModule = cacheModule
            Dim provide As ModuleProvider = ModuleProvider.Instance
            _configuration = configuration
            _userRoleService = userRoleService
        End Sub

        <CacheAspect(2)>
        <HttpGet("{id}")>
        Public Async Function [Get](<FromRoute> id As Long) As Task(Of IActionResult)
            Dim response = Await _userService.GetAsync(id)
            Return SendResponse(response)
        End Function

        '<CacheAspect(2)>
        <HttpGet>
        Public Async Function GetList() As Task(Of IActionResult)
            Dim roleTest = Await AuthorizationService.RoleCheck("Admin", HttpContext)

            If roleTest Then
                Dim response = Await _userService.GetListAsync()
                'Dim response = Await _userService.GetListWithProcedure()

                Return SendResponse(response)
            End If


        End Function

        <HttpGet("Procedure")>
        Public Async Function GetListProcedure() As Task(Of IActionResult)
            Dim roleTest = Await AuthorizationService.RoleCheck("Admin", HttpContext)

            Dim response = Await _userService.GetListWithProcedure()

            Return SendResponse(response)
        End Function

        <HttpGet("Page")>
        Public Async Function GetList(<FromQuery> userParameters As UserParameters) As Task(Of IActionResult)

            Dim roleTest = Await AuthorizationService.RoleCheck("Admin", HttpContext)


            Dim response = Await _userService.GetListAsync(userParameters)

            HttpContext.Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(response.MetaData))

            Return SendResponse(response.Item1)
        End Function

        <HttpPost>
        Public Async Function Add(<FromBody> user As UserPostDto) As Task(Of IActionResult)


            Dim response = Await _userService.AddAsync(user)
            Return SendResponse(response)
        End Function

        <HttpPost("AddUser")>
        Public Async Function AddUser(<FromBody> user As UserPostDto) As Task(Of IActionResult)
            Dim roleTest = Await AuthorizationService.RoleCheck("Admin", HttpContext)


            Dim response = Await _userService.AddWithProcedure(user)
            Return SendResponse(response)
        End Function

        <AllowAnonymous>
        <HttpPost("Login")>
        Public Async Function Login(<FromBody> loginDto As UserLoginDto) As Task(Of IActionResult)
            Dim response = Await _userService.LoginAsync(loginDto)



            If response.StatusCode = 200 Then


                Dim claims = New List(Of Claim)({
                    New Claim(ClaimTypes.Name, response.Data.Username)
                })





                Dim accessToken = New JwtGenerator(_configuration).CreateAccessToken(claims)

                response.Data.Token = accessToken.Token
            End If

            Return SendResponse(response)
        End Function


        <AllowAnonymous>
        <HttpPost("AdminLogin")>
        Public Async Function AdminLogin(<FromBody> loginDto As UserLoginDto) As Task(Of IActionResult)
            Dim response = Await _userService.AdminLoginAsync(loginDto)
            If response.StatusCode = 200 Then


                Dim claims = New List(Of Claim)({
                    New Claim(ClaimTypes.Name, response.Data.Username)
                })





                Dim accessToken = New JwtGenerator(_configuration).CreateAccessToken(claims)

                response.Data.Token = accessToken.Token
            End If

            Return SendResponse(response)

        End Function

        <HttpGet("GetByUsername")>
        Public Async Function GetByUserName(<FromQuery> username As String) As Task(Of IActionResult)

            Dim validToken As SecurityToken = Nothing
            Dim token = HttpContext.Request.Headers("Authorization").FirstOrDefault().Split(" ").ToList()(1)



            If AuthorizationService.UsernameCheck(token, username) Then
                Dim response = Await _userService.GetAsync(Function(e) e.Username = username)
                Return SendResponse(response)

            End If

            Return SendResponse(ApiResponse(Of UserGetDto).Fail(401, "Bu kullaniciyi goruntulemeye yetkiniz yok"))
        End Function

        <HttpPost("RemoveUser")>
        Public Async Function RemoveUser(<FromBody> dto As UserPostDto) As Task(Of IActionResult)
            Dim roleTest = Await AuthorizationService.RoleCheck("Admin", HttpContext)

            Dim response = Await _userService.RemoveAsync(dto)

            Return SendResponse(ApiResponse(Of NoData).Success(200))
        End Function
    End Class
End Namespace
