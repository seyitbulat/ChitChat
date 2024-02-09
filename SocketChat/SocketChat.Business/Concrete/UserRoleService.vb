Imports SocketChat.DataAccess
Imports SocketChat.Model

Public Class UserRoleService : Implements IUserRoleService

    Private ReadOnly _uow As IUnitOfWork

    Public Sub New(uow As IUnitOfWork)
        _uow = uow
    End Sub

    Public Async Function GetByUserAsync(id As Long) As Task(Of ApiResponse(Of IEnumerable(Of UserRoleGetDto))) Implements IUserRoleService.GetByUserAsync
        Dim repoResponse = Await _uow.UserRoleRepo.GetListAsync(Function(e) e.UserId = id)
        Dim newUserRoleList As New List(Of UserRoleGetDto)

        newUserRoleList = repoResponse.Select(Function(e) New UserRoleGetDto With {.RoleId = e.RoleId, .UserId = e.UserId}).ToList()
        Return ApiResponse(Of IEnumerable(Of UserRoleGetDto)).Success(200, repoResponse)
    End Function

    Public Function GetAsync(id As Long) As Task(Of ApiResponse(Of UserRoleGetDto)) Implements IService(Of Long, UserRoleGetDto, UserRolePostDto).GetAsync
        Throw New NotImplementedException()
    End Function

    Public Function GetListAsync() As Task(Of ApiResponse(Of IEnumerable(Of UserRoleGetDto))) Implements IService(Of Long, UserRoleGetDto, UserRolePostDto).GetListAsync
        Throw New NotImplementedException()
    End Function

    Public Async Function AddAsync(user As UserRolePostDto) As Task(Of ApiResponse(Of UserRoleGetDto)) Implements IService(Of Long, UserRoleGetDto, UserRolePostDto).AddAsync
        Dim userRole As New UserRole

        userRole.UserId = user.UserId
        userRole.RoleId = user.RoleId


        userRole.AddedDate = Date.Now
        userRole.IsActive = True

        Dim repoResponse = Await _uow.UserRoleRepo.AddAsync(userRole)
        Await _uow.SaveAsync()

        Dim newUserRoleDto As New UserRoleGetDto
        newUserRoleDto.UserId = repoResponse.UserId
        newUserRoleDto.RoleId = repoResponse.RoleId

        Return ApiResponse(Of UserRoleGetDto).Success(201, newUserRoleDto)
    End Function

    Public Function RemoveAsync(id As Long) As Task(Of ApiResponse(Of NoData)) Implements IService(Of Long, UserRoleGetDto, UserRolePostDto).RemoveAsync
        Throw New NotImplementedException()
    End Function
End Class
