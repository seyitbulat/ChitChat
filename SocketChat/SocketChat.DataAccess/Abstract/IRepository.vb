Imports SocketChat.Model
Imports System.Linq.Expressions
Imports System.Security.Cryptography

Public Interface IRepository(Of TId, TEntity As {Class, IEntity(Of TId), IGeneralEntity, New})
    Function [GetAsync](id As TId, Optional includeList As List(Of String) = Nothing) As Task(Of TEntity)

    Function [GetAsync](predicate As Expression(Of Func(Of TEntity, Boolean)), Optional includeList As List(Of String) = Nothing) As Task(Of TEntity)

    Function GetListAsync(Optional includeList As List(Of String) = Nothing) As Task(Of IEnumerable(Of TEntity))
    Function GetListAsync(predicate As Expression(Of Func(Of TEntity, Boolean)), Optional includeList As List(Of String) = Nothing) As Task(Of IEnumerable(Of TEntity))

    Function AddAsync(entity As TEntity) As Task(Of TEntity)

    Function RemoveAsync(id As TId) As Task(Of Boolean)


    Function RemoveAsync(id As TId, isHard As Boolean) As Task(Of Boolean)
End Interface
