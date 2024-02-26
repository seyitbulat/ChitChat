Public Class UserGetDto : Inherits BaseDto
    Public Property Id As Long
    Public Property Username As String
    Public Property Password As String
    Public Property Email As String
    Public Property Token As String
    Public Property IsActive As Boolean

End Class

Public Class UserPostDto : Inherits BaseDto
    Public Property Username As String
    Public Property Password As String
    Public Property Email As String


End Class


Public Class UserLoginDto : Inherits BaseDto
    Public Property UserName As String
    Public Property Password As String
End Class


Public Class UserDeleteDto : Inherits BaseDto
    Public Property UserName As String
    Public Property Email As String
End Class
