Imports Microsoft.EntityFrameworkCore
Imports SocketChat.Model

Public Class UserRoleRepository : Inherits Repository(Of Long, UserRole, ChitChatContext) : Implements IUserRoleRepository

    Private ReadOnly _context As ChitChatContext

    Public Sub New(context As ChitChatContext)
        MyBase.New(context)
        _context = context
    End Sub

    Public Async Function GetByUserAsync(id As Long) As Task(Of IEnumerable(Of UserRole)) Implements IUserRoleRepository.GetByUserAsync
        Dim roles = Await _context.UserRoles.Where(Function(e) e.UserId = id And e.IsActive = True).ToListAsync()

        Return roles
    End Function
End Class
