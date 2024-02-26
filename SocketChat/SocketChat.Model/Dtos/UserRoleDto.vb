Imports System.ComponentModel.DataAnnotations.Schema

Public Class UserRoleGetDto

    Public Property Id As Long


    Public Property UserId As Long
    Public Property RoleId As Short



    Public Property User As UserGetDto

    Public Property Role As RoleGetDto

End Class


Public Class UserRolePostDto
    Public Property UserId As Long
    Public Property RoleId As Short

End Class


Public Class UserRoleDeleteDto
    Public Property UserId As Long

End Class
