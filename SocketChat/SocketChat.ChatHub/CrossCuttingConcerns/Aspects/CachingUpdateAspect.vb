Imports System.Security.Cryptography
Imports Microsoft.AspNetCore.Mvc.Filters
Imports Microsoft.Extensions.Caching.Memory
Imports SocketChat.Business

Public Class CachingUpdateAspect : Inherits ActionFilterAttribute


    Public Property Key As String

    Public Sub New(key As String)
        Me.Key = key
    End Sub

    Public Overrides Async Sub OnActionExecuted(context As ActionExecutedContext)
        MyBase.OnActionExecuted(context)
        Dim memoryCache As IMemoryCache = TryCast(context.HttpContext.RequestServices.GetService(GetType(IMemoryCache)), IMemoryCache)
        Dim userService As IUserService = context.HttpContext.RequestServices.GetService(GetType(IUserService))

        Dim objList As Object = Nothing

        If memoryCache.TryGetValue(Key, objList) Then
            Dim list = Await userService.GetListAsync()

            memoryCache.Remove(Key)
            memoryCache.Set(Key, list.Data, DateTime.Now.AddMinutes(10))
        End If



    End Sub

End Class
