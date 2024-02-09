Imports Microsoft.EntityFrameworkCore

Public Interface IUnitOfWork


    ReadOnly Property UserRepo As IUserRepository
    ReadOnly Property UserRoleRepo As IUserRoleRepository

    Function Save() As Task
    Function SaveAsync() As Task

    Function Dispose(disposing As Boolean) As Task
    Function DisposeAsync(disposing As Boolean) As Task

End Interface
