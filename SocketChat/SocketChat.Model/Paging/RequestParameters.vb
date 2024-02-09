

Public MustInherit Class RequestParameters
    Const maxPageSize = 50

    Public Property PageNumber As Integer

    Private _pageSize As Integer = 10

    Public Property PageSize As Integer
        Get
            Return _pageSize
        End Get
        Set(value As Integer)
            If value > maxPageSize Then
                _pageSize = maxPageSize
            Else
                _pageSize = value
            End If
        End Set
    End Property
End Class
