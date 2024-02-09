Imports SocketChat.Model

Public Interface IService(Of TId, TGetDto, TPostDto)

    Function [GetAsync](id As TId) As Task(Of ApiResponse(Of TGetDto))

    Function GetListAsync() As Task(Of ApiResponse(Of IEnumerable(Of TGetDto)))

    Function AddAsync(user As TPostDto) As Task(Of ApiResponse(Of TGetDto))

    Function RemoveAsync(id As Long) As Task(Of ApiResponse(Of NoData))

End Interface

