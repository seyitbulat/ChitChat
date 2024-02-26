Imports SocketChat.Model

Public Interface IUserRoleService : Inherits IService(Of Long, UserRoleGetDto, UserRolePostDto)


    Function GetByUserAsync(id As Long) As Task(Of ApiResponse(Of IEnumerable(Of UserRoleGetDto)))
    Function GetByUserAsync(username As String) As Task(Of ApiResponse(Of IEnumerable(Of UserRoleGetDto)))

End Interface
