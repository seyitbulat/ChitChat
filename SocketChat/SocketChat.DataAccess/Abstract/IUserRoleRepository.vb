Imports SocketChat.Model

Public Interface IUserRoleRepository : Inherits IRepository(Of Long, UserRole)


    Function GetByUserAsync(id As Long) As Task(Of IEnumerable(Of UserRole))

End Interface
