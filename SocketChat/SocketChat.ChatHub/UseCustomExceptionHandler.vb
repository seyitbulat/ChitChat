Imports System.Runtime.CompilerServices
Imports System.Text.Json
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Diagnostics
Imports Microsoft.AspNetCore.Http
Imports SocketChat.Business

Public Module UseCustomExceptionHandler

    <Extension()>
    Public Sub UseCustomException(ByRef app As IApplicationBuilder)
        app.UseExceptionHandler(Sub(o)
                                    o.Run(Async Function(context)
                                              context.Response.ContentType = "application/json"
                                              Dim exceptionFeature = context.Features.Get(Of IExceptionHandlerFeature)
                                              Dim err = exceptionFeature.Error.GetType()
                                              Dim statusCode = StatusCodes.Status500InternalServerError


                                              If exceptionFeature.Error.GetType() Is GetType(BadRequestException) Then
                                                  statusCode = StatusCodes.Status400BadRequest

                                              ElseIf exceptionFeature.Error.GetType() Is GetType(NotFoundException) Then
                                                  statusCode = StatusCodes.Status404NotFound


                                              ElseIf exceptionFeature.Error.GetType() Is GetType(UnauthorizedException) Then
                                                  statusCode = StatusCodes.Status401Unauthorized
                                              End If

                                              context.Response.StatusCode = statusCode
                                              Dim response = ApiResponse(Of NoData).Fail(statusCode, exceptionFeature.Error.Message.ToString())
                                              Await context.Response.WriteAsync(JsonSerializer.Serialize(response))
                                          End Function)
                                End Sub)
    End Sub
End Module
