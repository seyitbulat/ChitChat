Imports SocketChat.Model

Public Interface ICacheModule
    Function [Get](Of T)(key As String) As T
    Function [Get](key As String) As Object
    Sub Add(key As String, value As Object, duration As Integer)
    Function IsAdd(key As String) As Boolean
    Sub Remove(key As String)
    Sub RemoveByPattern(pattern As String)

End Interface
