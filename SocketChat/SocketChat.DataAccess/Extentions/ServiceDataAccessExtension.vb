Imports System.Runtime.CompilerServices
Imports Microsoft.Extensions.DependencyInjection

Public Module ServiceDataAccessExtension

    <Extension()>
    Public Sub AddDataAccesServices(ByRef service As IServiceCollection)
        service.AddDbContext(Of ChitChatContext)
    End Sub
End Module
