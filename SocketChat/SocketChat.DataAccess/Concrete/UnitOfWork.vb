Imports Microsoft.EntityFrameworkCore
Imports SocketChat.Model

Public Class UnitOfWork : Implements IUnitOfWork


    Private isDisposed As Boolean = False
    Private ReadOnly Context As ChitChatContext




    Public Sub New(context As ChitChatContext)
        Me.Context = context

        UserRepo = New UserRepository(context)
        UserRoleRepo = New UserRoleRepository(context)
    End Sub


    Public ReadOnly Property UserRepo As IUserRepository Implements IUnitOfWork.UserRepo
    Public ReadOnly Property UserRoleRepo As IUserRoleRepository Implements IUnitOfWork.UserRoleRepo



    Public Function Save() As Task Implements IUnitOfWork.Save
        Using transaction = Context.Database.BeginTransaction()
            Try
                Context.SaveChanges()
                transaction.Commit()
            Catch ex As Exception

                transaction.Rollback()
            End Try
        End Using
    End Function

    Public Async Function SaveAsync() As Task Implements IUnitOfWork.SaveAsync
        Using transaction = Await Context.Database.BeginTransactionAsync()
            Try
                Await Context.SaveChangesAsync()
                Await transaction.CommitAsync()
            Catch ex As Exception
                transaction.RollbackAsync()
            End Try
        End Using
        For Each item In Context.ChangeTracker.Entries(Of IEntity(Of Long))

            If item.State = EntityState.Added Then

                item.Entity.AddedDate = DateTime.UtcNow
                item.Entity.UpdatedDate = DateTime.UtcNow
                item.Entity.IsActive = True
            ElseIf item.State = EntityState.Modified Then
                item.Entity.UpdatedDate = DateTime.UtcNow

            End If

        Next

    End Function

    Public Function Dispose(disposing As Boolean) As Task Implements IUnitOfWork.Dispose
        If Not isDisposed Then
            If disposing Then
                Context.Dispose()
            End If
            isDisposed = True
        End If
    End Function

    Public Async Function DisposeAsync(disposing As Boolean) As Task Implements IUnitOfWork.DisposeAsync
        If Not isDisposed Then
            If disposing Then
                Await Context.DisposeAsync()
            End If
            isDisposed = True
        End If
    End Function

    'Public Sub Dispose() Implements IDisposable.Dispose
    '    Dispose(True)
    '    GC.SuppressFinalize(Me)
    'End Sub
End Class
