Imports System.ComponentModel.DataAnnotations.Schema

Public Class UserRole : Implements IEntity(Of Long)

    Public Property Id As Long Implements IEntity(Of Long).Id


    Public Property UserId As Long
    Public Property RoleId As Short

    <ForeignKey("UserId")>
    Public Property User As User

    <ForeignKey("RoleId")>
    Public Property Role As Role

    Public Property AddedDate As Date? Implements IEntity(Of Long).AddedDate


    Public Property UpdatedDate As Date? Implements IEntity(Of Long).UpdatedDate


    Public Property DeletedDate As Date? Implements IEntity(Of Long).DeletedDate


    Public Property IsActive As Boolean Implements IEntity(Of Long).IsActive

End Class
