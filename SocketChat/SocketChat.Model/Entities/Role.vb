Public Class Role : Implements IEntity(Of Short)
    Public Property Id As Short Implements IEntity(Of Short).Id

    Public Property Name As String


    Public Property UserRoles As IEnumerable(Of UserRole)

    Public Property AddedDate As Date? Implements IEntity(Of Short).AddedDate


    Public Property UpdatedDate As Date? Implements IEntity(Of Short).UpdatedDate


    Public Property DeletedDate As Date? Implements IEntity(Of Short).DeletedDate


    Public Property IsActive As Boolean Implements IEntity(Of Short).IsActive

End Class
