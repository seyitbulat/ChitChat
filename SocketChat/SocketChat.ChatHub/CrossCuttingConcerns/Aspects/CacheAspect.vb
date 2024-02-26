Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.AspNetCore.Mvc.Abstractions
Imports Microsoft.AspNetCore.Mvc.Filters
Imports SocketChat.Business

Public Class CacheAspect
    Inherits ActionFilterAttribute

    Private ReadOnly _duration As Integer

    Private _cacheModule As ICacheModule
    Public Sub New(duration As Integer)
        _duration = duration
    End Sub

    Public Overrides Sub OnActionExecuting(context As ActionExecutingContext)
        MyBase.OnActionExecuting(context)
        _cacheModule = TryCast(context.HttpContext.RequestServices.GetService(GetType(ICacheModule)), ICacheModule)

        Dim actionDescriptor = DirectCast(context.ActionDescriptor, Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)
        Dim methodName = String.Format($"{actionDescriptor.ControllerTypeInfo.FullName.ToString()}.{actionDescriptor.MethodInfo.Name}")
        'Dim arguments = context.ActionArguments.ToList()
        Dim arguments = context.ActionDescriptor.Parameters.ToList()

        Dim key = $"{methodName} ({String.Join(",", arguments.Select(Function(s)
                                                                         If s.Name Is Nothing Then
                                                                             Return "<Null>"
                                                                         Else
                                                                             Return s.Name.ToString()
                                                                         End If
                                                                     End Function))})"
        key = String.Join(".", key.Split(".").ToList().Skip(2))


        If _cacheModule.IsAdd(key) Then
            context.Result = New ObjectResult(_cacheModule.Get(key))
        End If
    End Sub

    Public Overrides Sub OnActionExecuted(context As ActionExecutedContext)
        MyBase.OnActionExecuted(context)
        _cacheModule = TryCast(context.HttpContext.RequestServices.GetService(GetType(ICacheModule)), ICacheModule)


        Dim actionDescriptor = DirectCast(context.ActionDescriptor, Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)
        Dim methodName = String.Format($"{actionDescriptor.ControllerTypeInfo.FullName.ToString()}.{actionDescriptor.MethodInfo.Name}")
        'Dim arguments = context.ActionArguments.ToList()
        Dim arguments = context.ActionDescriptor.Parameters.ToList()

        Dim key = $"{methodName} ({String.Join(",", arguments.Select(Function(s)
                                                                         If s.Name Is Nothing Then
                                                                             Return "<Null>"
                                                                         Else
                                                                             Return s.Name.ToString()
                                                                         End If
                                                                     End Function))})"
        key = String.Join(".", key.Split(".").ToList().Skip(2))
        Dim resultData = DirectCast(context.Result, ObjectResult)?.Value



        _cacheModule.Add(key, resultData.Data, _duration)
    End Sub



End Class
