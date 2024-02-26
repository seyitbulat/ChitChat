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


        If repoResponse Is Nothing Then
            Throw New NotFoundException("Kullanici bulunamadi")
        End If

        newResponse.Id = repoResponse.Id
        newResponse.Username = repoResponse.Username
        newResponse.Email = repoResponse.Email

        Return ApiResponse(Of UserGetDto).Success(200, newResponse)
    End Function

    Public Async Function GetAsync(Optional predicate As Expression(Of Func(Of User, Boolean)) = Nothing) As Task(Of ApiResponse(Of UserGetDto)) Implements IUserService.GetAsync
        Dim repoResponse = Await _uow.UserRepo.GetAsync(predicate)
        Dim newResponse As New UserGetDto

        If repoResponse Is Nothing Then
            Throw New NotFoundException("Kullanici bulunamadi")
        End If

        newResponse.Id = repoResponse.Id
        newResponse.Username = repoResponse.Username
        newResponse.Email = repoResponse.Email

        Return ApiResponse(Of UserGetDto).Success(200, newResponse)
    End Function

    Public Async Function GetListAsync() As Task(Of ApiResponse(Of IEnumerable(Of UserGetDto))) Implements IService(Of Long, UserGetDto, UserPostDto).GetListAsync
        Dim repoResponse = Await _uow.UserRepo.GetListAsync()
        Dim newResponse As New List(Of UserGetDto)

        If repoResponse Is Nothing Then
            Throw New NotFoundException("Kullanicilar bulunamadi")
        End If

        newResponse = repoResponse.Select(Function(e) New UserGetDto With {
            .Id = e.Id,
            .Username = e.Username,
            .Email = e.Email,
            .Password = e.Password
        }).ToList()



        Return ApiResponse(Of IEnumerable(Of UserGetDto)).Success(200, newResponse)
    End Function

    Public Async Function GetListAsync(userParameters As UserParameters, Optional predicate As Expression(Of Func(Of User, Boolean)) = Nothing) As Task(Of (ApiResponse(Of IEnumerable(Of UserGetDto)), MetaData As MetaData)) Implements IUserService.GetListAsync
        Dim repoResponse = Await _uow.UserRepo.GetListAsync(userParameters, predicate, isActive:=False)
        Dim newResponse As New List(Of UserGetDto)


        newResponse = repoResponse.Select(Function(e) New UserGetDto With {
                                          .Id = e.Id,
                                          .Username = e.Username,
                                          .Email = e.Email,
                                          .Password = e.Password,
                                          .IsActive = e.IsActive}).ToList()
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

        Await _userRoleService.AddAsync(New UserRolePostDto With {.RoleId = 2, .UserId = newUser.Id})

        Dim newUserDto As New UserGetDto
        newUserDto.Id = repoResponse.Id
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
        If repoResponse Is Nothing Then
            Throw New NotFoundException("Kullanici bulunamadi.")
        End If

        Dim newDto As New UserGetDto With {
            .Id = repoResponse.Id,
            .Email = repoResponse.Email,
            .Username = repoResponse.Username
        }

        Return ApiResponse(Of UserGetDto).Success(200, newDto)
    End Function

    Public Async Function AdminLoginAsync(dto As UserLoginDto) As Task(Of ApiResponse(Of UserGetDto)) Implements IUserService.AdminLoginAsync
        Dim user As New User

        user.Username = dto.UserName
        user.Password = dto.Password

        Dim repoResponse = Await _uow.UserRepo.GetAsync(Function(u) u.Username = dto.UserName And u.Password = dto.Password)
        If repoResponse Is Nothing Then
            Throw New NotFoundException("Kullanici bulunamadi.")
        End If



        Dim userRoles = Await _userRoleService.GetByUserAsync(dto.UserName)

        If userRoles Is Nothing Then
            Throw New BadRequestException("Rol bulunamadi, yetkili ile iletisime gecin")
        End If

        If userRoles.StatusCode = 200 Then
            If userRoles.Data.Any(Function(r) r.Role.Name = "Admin") Then
                Dim newDto As New UserGetDto With {
                        .Id = repoResponse.Id,
                        .Email = repoResponse.Email,
                        .Username = repoResponse.Username
                    }

                Return ApiResponse(Of UserGetDto).Success(200, newDto)
            End If
            Throw New UnauthorizedException("Yetkiniz yok")
        End If


    End Function

    Public Async Function GetListWithProcedure() As Task(Of ApiResponse(Of IEnumerable(Of UserGetDto))) Implements IUserService.GetListWithProcedure
        Dim repoResponse = Await _uow.UserRepo.GetListWithProcedure()
        Dim newResponse As New List(Of UserGetDto)

        newResponse = repoResponse.Select(Function(e) New UserGetDto With {
            .Id = e.Id,
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

    Public Async Function DeleteUserAsync(dto As UserPostDto) As Task(Of ApiResponse(Of NoData)) Implements IService(Of Long, UserGetDto, UserPostDto).RemoveAsync
        Dim user = Await _uow.UserRepo.GetAsync(Function(u) u.Username = dto.Username)
        Dim result = Await _uow.UserRepo.RemoveAsync(user)
        Await _uow.SaveAsync()
        Return ApiResponse(Of NoData).Success(200)
    End Function
End Class
