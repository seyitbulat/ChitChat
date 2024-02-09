Imports System.Security.Principal

Public Class User : Implements IEntity(Of Long)
    Public Property Id As Long Implements IEntity(Of Long).Id
    Public Property Username As String
    Public Property Password As String
    Public Property Email As String

    Public Property AddedDate As Date Implements IEntity(Of Long).AddedDate
    Public Property UpdatedDate As Date Implements IEntity(Of Long).UpdatedDate
    Public Property DeletedDate As Date Implements IEntity(Of Long).DeletedDate

    Public Property IsActive As Boolean Implements IEntity(Of Long).IsActive
End Class
