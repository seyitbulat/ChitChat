Imports System.ComponentModel.DataAnnotations.Schema

Public Class Message : Implements IEntity(Of Long)

    Public Property Id As Long Implements IEntity(Of Long).Id

    Public Property SenderUserId As Long
    Public Property ReceiverUserId As Long

    Public Property HubId As Long

    Public Property Body As String
    Public Property SendDate As Date


    ' Navigation Properties
    <ForeignKey("SenderUserId")>
    Public Property Sender As User


    <ForeignKey("ReceiverUserId")>
    Public Property Receiver As User

    <ForeignKey("HubId")>
    Public Property Hub As Hub

    Public Property AddedDate As Date? Implements IEntity(Of Long).AddedDate


    Public Property UpdatedDate As Date? Implements IEntity(Of Long).UpdatedDate


    Public Property DeletedDate As Date? Implements IEntity(Of Long).DeletedDate


    Public Property IsActive As Boolean Implements IEntity(Of Long).IsActive

End Class
