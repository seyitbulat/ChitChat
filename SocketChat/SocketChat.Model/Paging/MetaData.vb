Imports System.Numerics

Public Class MetaData
    Public Property CurrentPage As Integer
    Public Property TotalPages As Integer

    Public Property PageSize As Integer
    Public Property TotalCount As Long


    Public Function HasPrevious() As Boolean
        Return CurrentPage > 1
    End Function

    Public Function HasNext() As Boolean
        Return CurrentPage < TotalPages
    End Function
End Class
