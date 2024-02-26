Imports System.Linq.Expressions
Imports SocketChat.Model

Public Interface IUserRepository : Inherits IRepository(Of Long, User)

    Overloads Function GetListAsync(userParameters As UserParameters, Optional predicate As Expression(Of Func(Of User, Boolean)) = Nothing, Optional trackChanges As Boolean = False, Optional isActive As Boolean = True) As Task(Of PagedList(Of User))


    Function GetListWithProcedure() As Task(Of IEnumerable(Of User))

    Function AddUserAsync(user As User) As Task




End Interface
