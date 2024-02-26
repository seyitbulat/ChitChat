Imports System.Security.Cryptography
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


    Public Async Function RemoveAsync(id As Long) As Task(Of Boolean) Implements IRepository(Of Long, UserRole).RemoveAsync

        Dim roles = Await _context.UserRoles.Where(Function(e) e.UserId = id And e.IsActive = True).ToListAsync()

        If (roles Is Nothing) Then
            Return False
        End If
        roles = roles.Select(Function(r) r.IsActive = False And r.DeletedDate = DateTime.Now)

        Return True

    End Function

End Class
