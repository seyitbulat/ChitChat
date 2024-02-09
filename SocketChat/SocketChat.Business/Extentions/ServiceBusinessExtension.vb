Imports System.Runtime.CompilerServices
Imports Microsoft.Extensions.DependencyInjection
Imports SocketChat.DataAccess

Public Module ServiceBusinessExtension

    <Extension()>
    Public Sub AddBusinessServices(ByRef service As IServiceCollection)
        service.AddScoped(Of IUserService, UserService)
        service.AddScoped(Of IUserRoleService, UserRoleService)
        service.AddScoped(Of IUnitOfWork, UnitOfWork)
    End Sub
End Module
