Imports AutoMapper
Imports SocketChat.Model

Public Class MainProfile : Inherits Profile

    Public Sub New()
        CreateMap(Of User, UserGetDto)()
        CreateMap(Of Role, RoleGetDto)()
        CreateMap(Of UserRole, UserRoleGetDto)()

    End Sub
End Class
