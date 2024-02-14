Imports System.Linq.Expressions
Imports System.Net
Imports SocketChat.DataAccess
Imports SocketChat.Model

Public Class UserService : Implements IUserService

    Private ReadOnly _uow As IUnitOfWork
    Private ReadOnly _userRoleService As IUserRoleService

    Public Sub New(uow As IUnitOfWork, userRoleService As IUserRoleService)
        _uow = uow
        _userRoleService = userRoleService
    End Sub

    Public Async Function GetAsync(id As Long) As Task(Of ApiResponse(Of UserGetDto)) Implements IUserService.GetAsync
        Dim repoResponse = Await _uow.UserRepo.GetAsync(id)
        Dim newResponse As New UserGetDto

        newResponse.Username = repoResponse.Username
        newResponse.Email = repoResponse.Email

        Return ApiResponse(Of UserGetDto).Success(200, newResponse)
    End Function

    Public Async Function GetListAsync() As Task(Of ApiResponse(Of IEnumerable(Of UserGetDto))) Implements IService(Of Long, UserGetDto, UserPostDto).GetListAsync
        Dim repoResponse = Await _uow.UserRepo.GetListAsync()
        Dim newResponse As New List(Of UserGetDto)


        newResponse = repoResponse.Select(Function(e) New UserGetDto With {
            .Username = e.Username,
            .Email = e.Email,
            .Password = e.Password
        }).ToList()



        Return ApiResponse(Of IEnumerable(Of UserGetDto)).Success(200, newResponse)
    End Function

    Public Async Function GetListAsync(userParameters As UserParameters, Optional predicate As Expression(Of Func(Of User, Boolean)) = Nothing) As Task(Of (ApiResponse(Of IEnumerable(Of UserGetDto)), MetaData As MetaData)) Implements IUserService.GetListAsync
        Dim repoResponse = Await _uow.UserRepo.GetListAsync(userParameters, predicate)
        Dim newResponse As New List(Of UserGetDto)

        newResponse = repoResponse.Select(Function(e) New UserGetDto With {
                                          .Username = e.Username,
                                          .Email = e.Email,
                                          .Password = e.Password}).ToList()
        Dim apiResult = ApiResponse(Of IEnumerable(Of UserGetDto)).Success(200, newResponse)
        Dim metaResult = repoResponse.MetaData

        Return (apiResult, metaResult)
    End Function

    Public Async Function AddAsync(dto As UserPostDto) As Task(Of ApiResponse(Of UserGetDto)) Implements IUserService.AddAsync
        Dim user As New User

        user.Username = dto.Username
        user.Password = dto.Password
        user.Email = dto.Email

        user.AddedDate = Date.Now
        user.IsActive = True

        Dim repoResponse = Await _uow.UserRepo.AddAsync(user)
        Await _uow.SaveAsync()

        Dim newUser = Await _uow.UserRepo.GetAsync(Function(e) e.Username = user.Username)

        _userRoleService.AddAsync(New UserRolePostDto With {.RoleId = 2, .UserId = newUser.Id})

        Dim newUserDto As New UserGetDto
        newUserDto.Username = repoResponse.Username
        newUserDto.Email = repoResponse.Email

        Return ApiResponse(Of UserGetDto).Success(201, newUserDto)
    End Function

    Public Function RemoveAsync(id As Long) As Task(Of ApiResponse(Of NoData)) Implements IUserService.RemoveAsync
        Throw New NotImplementedException()
    End Function

    Public Async Function LoginAsync(dto As UserLoginDto) As Task(Of ApiResponse(Of UserGetDto)) Implements IUserService.LoginAsync
        Dim user As New User

        user.Username = dto.UserName
        user.Password = dto.Password

        Dim repoResponse = Await _uow.UserRepo.GetAsync(Function(u) u.Username = dto.UserName And u.Password = dto.Password)

        Dim newDto As New UserGetDto With {
            .Email = repoResponse.Email,
            .Username = repoResponse.Username
        }

        Return ApiResponse(Of UserGetDto).Success(200, newDto)
    End Function

    Public Async Function GetListWithProcedure() As Task(Of ApiResponse(Of IEnumerable(Of UserGetDto))) Implements IUserService.GetListWithProcedure
        Dim repoResponse = Await _uow.UserRepo.GetListWithProcedure()
        Dim newResponse As New List(Of UserGetDto)

        newResponse = repoResponse.Select(Function(e) New UserGetDto With {
            .Username = e.Username,
            .Email = e.Email,
            .Password = e.Password
        }).ToList()

        Return ApiResponse(Of IEnumerable(Of UserGetDto)).Success(200, newResponse)

    End Function

    Public Async Function AddWithProcedure(dto As UserPostDto) As Task(Of ApiResponse(Of NoData)) Implements IUserService.AddWithProcedure
        Dim user As New User

        user.Username = dto.Username
        user.Password = dto.Password
        user.Email = dto.Email

        user.AddedDate = Date.Now
        user.IsActive = True

        Await _uow.UserRepo.AddUserAsync(user)

        Return ApiResponse(Of NoData).Success(201)
    End Function
End Class
