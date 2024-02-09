Public Class PagedList(Of T) : Inherits List(Of T)

    Public Property MetaData As MetaData
    Public Sub New(items As List(Of T), count As Integer, pageNumber As Integer, pageSize As Integer)
        MetaData = New MetaData With {
        .TotalCount = count,
        .PageSize = pageSize,
        .CurrentPage = pageNumber,
        .TotalPages = CType(Math.Ceiling(count / CType(pageSize, Double)), Integer)
        }

        AddRange(items)
    End Sub


    Public Shared Function ToPagedList(source As IEnumerable(Of T), pageNumber As Integer, pageSize As Integer) As PagedList(Of T)
        Dim count = source.Count()
        Dim items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()

        Return New PagedList(Of T)(items, count, pageNumber, pageSize)
    End Function

    Public Shared Function ToPagedList(source As IEnumerable(Of T), count As Integer, pageNumber As Integer, pageSize As Integer) As PagedList(Of T)
        Dim items = source.ToList()

        Return New PagedList(Of T)(items, count, pageNumber, pageSize)
    End Function
End Class
