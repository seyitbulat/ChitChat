Imports AutoMapper
Imports SocketChat.DataAccess
Imports SocketChat.Model

Public Class UserRoleService : Implements IUserRoleService

    Private ReadOnly _uow As IUnitOfWork
    Private ReadOnly _mapper As IMapper

    Public Sub New(uow As IUnitOfWork, mapper As IMapper)
        _uow = uow
        _mapper = mapper
    End Sub

    Public Async Function GetByUserAsync(id As Long) As Task(Of ApiResponse(Of IEnumerable(Of UserRoleGetDto))) Implements IUserRoleService.GetByUserAsync
        Dim includeList As New List(Of String)
        includeList.Add("User")
        includeList.Add("Role")
        Dim repoResponse = Await _uow.UserRoleRepo.GetListAsync(Function(e) e.UserId = id, includeList)
        Dim newUserRoleList As New List(Of UserRoleGetDto)

        'newUserRoleList = repoResponse.Select(Function(e) New UserRoleGetDto With {.RoleId = e.RoleId, .UserId = e.UserId, .User = New UserGetDto With {.Username = e.User.Username, .Email = e.User.Email}, .Role = New RoleGetDto With {.Name = e.Role.Name, .Id = e.Role.Id}}).ToList()

        'For Each userRole In repoResponse
        '    newUserRoleList.Add(_mapper.Map(Of UserRoleGetDto)(userRole))
        'Next

        newUserRoleList = _mapper.Map(Of List(Of UserRoleGetDto))(repoResponse)

        Return ApiResponse(Of IEnumerable(Of UserRoleGetDto)).Success(200, newUserRoleList)
    End Function

    Public Async Function GetByUserAsync(username As String) As Task(Of ApiResponse(Of IEnumerable(Of UserRoleGetDto))) Implements IUserRoleService.GetByUserAsync
        Dim includeList As New List(Of String)
        includeList.Add("User")
        includeList.Add("Role")
        Dim repoResponse = Await _uow.UserRoleRepo.GetListAsync(Function(e) e.User.Username = username, includeList)
        Dim newUserRoleList As New List(Of UserRoleGetDto)

        newUserRoleList = _mapper.Map(Of List(Of UserRoleGetDto))(repoResponse)

        Return ApiResponse(Of IEnumerable(Of UserRoleGetDto)).Success(200, newUserRoleList)
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

    Public Async Function RemoveAsync(dto As UserRolePostDto) As Task(Of ApiResponse(Of NoData)) Implements IService(Of Long, UserRoleGetDto, UserRolePostDto).RemoveAsync
        Dim userRole = Await _uow.UserRoleRepo.GetAsync(Function(u) u.UserId = dto.UserId)

        If userRole Is Nothing Then
            Throw New BadRequestException("Kullanıcının rolu bulunmamakta")
        End If

        Dim result = Await _uow.UserRoleRepo.RemoveAsync(userRole)
        Await _uow.SaveAsync()
        Return ApiResponse(Of NoData).Success(200)
    End Function
End Class
