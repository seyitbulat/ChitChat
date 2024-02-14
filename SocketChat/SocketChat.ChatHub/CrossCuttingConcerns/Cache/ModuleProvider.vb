Imports Microsoft.AspNetCore.Http

Public NotInheritable Class ModuleProvider

    Private Shared ReadOnly _instance As New Lazy(Of ModuleProvider)(Function() New ModuleProvider, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication)

    Public Shared Property ServiceModule As ICacheModule
        Get
            Return _serviceModule
        End Get
        Set(value As ICacheModule)
            value = _serviceModule
        End Set
    End Property
    Private Shared _serviceModule As ICacheModule

    Public Sub New()
    End Sub

    Public Shared Sub SetModule(serviceModule As ICacheModule)
        _serviceModule = serviceModule
    End Sub

    Public Shared ReadOnly Property Instance() As ModuleProvider
        Get
            Return _instance.Value
        End Get
    End Property

    Public Shared Function GetModule() As ICacheModule
        Return ServiceModule
    End Function
End Class
