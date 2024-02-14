Public Interface IEntity(Of TId) : Inherits IGeneralEntity
    Property Id As TId
    Property AddedDate As Date?
    Property UpdatedDate As Date?
    Property DeletedDate As Date?

    Property IsActive As Boolean
End Interface
