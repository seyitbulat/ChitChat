Imports System
Imports System.Text.Json
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.Extensions.Caching.Memory
Imports Microsoft.Extensions.Logging
Imports SocketChat.Business
Imports SocketChat.DataAccess
Imports SocketChat.Model

Namespace SocketChat.ChatHub.Controllers
    <ApiController>
    <Route("[controller]")>
    Public Class UsersController
        Inherits BaseController

        Private ReadOnly _userService As IUserService
        Private ReadOnly _memoryCache As IMemoryCache
        Private ReadOnly _cacheModule As ICacheModule



        Public Sub New(userService As IUserService, memoryCache As IMemoryCache, cacheModule As ICacheModule)
            _userService = userService
            _memoryCache = memoryCache
            _cacheModule = cacheModule
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

            Dim response = Await _userService.GetListAsync()
            'Dim response = Await _userService.GetListWithProcedure()

            Return SendResponse(response)
        End Function

        <HttpGet("Procedure")>
        Public Async Function GetListProcedure() As Task(Of IActionResult)
            Dim response = Await _userService.GetListWithProcedure()

            Return SendResponse(response)
        End Function


        <HttpGet("Page")>
        Public Async Function GetList(<FromQuery> userParameters As UserParameters) As Task(Of IActionResult)
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
            Dim response = Await _userService.AddWithProcedure(user)
            Return SendResponse(response)
        End Function

        <HttpPost("Login")>
        Public Async Function Login(<FromBody> loginDto As UserLoginDto) As Task(Of IActionResult)
            Dim response = Await _userService.LoginAsync(loginDto)


            Return SendResponse(response)
        End Function
    End Class
End Namespace
