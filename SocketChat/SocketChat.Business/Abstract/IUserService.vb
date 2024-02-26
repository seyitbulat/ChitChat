Imports System.Linq.Expressions
Imports SocketChat.Model

Public Interface IUserService : Inherits IService(Of Long, UserGetDto, UserPostDto)

    Function LoginAsync(dto As UserLoginDto) As Task(Of ApiResponse(Of UserGetDto))
    Function AdminLoginAsync(dto As UserLoginDto) As Task(Of ApiResponse(Of UserGetDto))
    Overloads Function GetAsync(Optional predicate As Expression(Of Func(Of User, Boolean)) = Nothing) As Task(Of ApiResponse(Of UserGetDto))

    Overloads Function GetListAsync(userParameters As UserParameters, Optional predicate As Expression(Of Func(Of User, Boolean)) = Nothing) As Task(Of (ApiResponse(Of IEnumerable(Of UserGetDto)), MetaData As MetaData))

    Function GetListWithProcedure() As Task(Of ApiResponse(Of IEnumerable(Of UserGetDto)))


    Function AddWithProcedure(dto As UserPostDto) As Task(Of ApiResponse(Of NoData))


End Interface
