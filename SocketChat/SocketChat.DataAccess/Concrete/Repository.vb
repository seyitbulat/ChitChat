﻿Imports System.Linq.Expressions
Imports Microsoft.EntityFrameworkCore
Imports SocketChat.Model

Public Class Repository(Of TId As Structure, TEntity As {IEntity(Of TId), IGeneralEntity, Class, New}, TContext As {DbContext, New}) : Implements IRepository(Of TId, TEntity)
    Private ReadOnly _context As TContext
    Private ReadOnly _dbSet As DbSet(Of TEntity)

    Public Sub New(context As TContext)
        _context = context
        _dbSet = _context.Set(Of TEntity)
    End Sub
    Public Async Function GetAsync(id As TId, Optional includeList As List(Of String) = Nothing) As Task(Of TEntity) Implements IRepository(Of TId, TEntity).GetAsync


        Dim query As IQueryable(Of TEntity) = _dbSet
        If includeList IsNot Nothing Then
            For Each item In includeList
                query = query.Include(item)
            Next
        End If

        Return Await query.Where(Function(e) e.Id.Equals(id) And e.IsActive = True).SingleOrDefaultAsync()
    End Function

    Public Async Function GetAsync(predicate As Expression(Of Func(Of TEntity, Boolean)), Optional includeList As List(Of String) = Nothing) As Task(Of TEntity) Implements IRepository(Of TId, TEntity).GetAsync

        Dim query As IQueryable(Of TEntity) = _dbSet
        If includeList IsNot Nothing Then
            For Each item In includeList
                query = query.Include(item)
            Next
        End If

        Return Await query.Where(Function(e) e.IsActive = True).SingleOrDefaultAsync(predicate)
    End Function

    Public Async Function GetListAsync(Optional includeList As List(Of String) = Nothing) As Task(Of IEnumerable(Of TEntity)) Implements IRepository(Of TId, TEntity).GetListAsync

        Dim query As IQueryable(Of TEntity) = _dbSet
        If includeList IsNot Nothing Then
            For Each item In includeList
                query = query.Include(item)
            Next
        End If
        Return Await query.ToListAsync()
    End Function
    Public Async Function GetListAsync(predicate As Expression(Of Func(Of TEntity, Boolean)), Optional includeList As List(Of String) = Nothing) As Task(Of IEnumerable(Of TEntity)) Implements IRepository(Of TId, TEntity).GetListAsync


        Dim query As IQueryable(Of TEntity) = _dbSet
        If includeList IsNot Nothing Then
            For Each item In includeList
                query = query.Include(item)
            Next
        End If

        Dim list = Await query.Where(Function(e) e.IsActive = True).Where(predicate).ToListAsync()
        Return list
    End Function
    Public Async Function AddAsync(entity As TEntity) As Task(Of TEntity) Implements IRepository(Of TId, TEntity).AddAsync
        Dim newEntity = Await _dbSet.AddAsync(entity)

        Return newEntity.Entity
    End Function

    Public Async Function RemoveAsync(id As TId) As Task(Of Boolean) Implements IRepository(Of TId, TEntity).RemoveAsync

        Dim entity = Await _dbSet.FirstOrDefaultAsync(Function(u) u.Id.Equals(id))

        entity.IsActive = False
        entity.DeletedDate = Date.Now


        Return True
    End Function


    Public Async Function RemoveAsync(user As TEntity) As Task(Of Boolean) Implements IRepository(Of TId, TEntity).RemoveAsync

        user.DeletedDate = DateTime.Now
        user.IsActive = False

        Return True
    End Function

    Public Async Function RemoveAsync(id As TId, isHard As Boolean) As Task(Of Boolean) Implements IRepository(Of TId, TEntity).RemoveAsync
        If isHard Then
            Dim entity = Await _dbSet.FirstOrDefaultAsync(Function(u) u.Id.Equals(id))
            _dbSet.Remove(entity)

            Return True
        Else
            Return Await RemoveAsync(id)
        End If
    End Function


End Class
