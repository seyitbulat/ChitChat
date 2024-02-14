Imports System.Security.Principal

Public Class Hub : Implements IEntity(Of Long)
    Public Property Id As Long Implements IEntity(Of Long).Id
    Public Property Hubname As String
    Public Property Status As Boolean


    Public Property AddedDate As Date? Implements IEntity(Of Long).AddedDate
    Public Property UpdatedDate As Date? Implements IEntity(Of Long).UpdatedDate
    Public Property DeletedDate As Date? Implements IEntity(Of Long).DeletedDate

    Public Property IsActive As Boolean Implements IEntity(Of Long).IsActive
End Class
