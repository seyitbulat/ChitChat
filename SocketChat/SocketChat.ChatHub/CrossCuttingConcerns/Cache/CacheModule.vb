Imports System.Text.RegularExpressions
Imports Microsoft.Extensions.Caching.Memory
Imports SocketChat.Model

Public Class CacheModule : Implements ICacheModule

    Private _memoryCache As IMemoryCache

    Public Sub New(memoryCache As IMemoryCache)
        _memoryCache = memoryCache
    End Sub

    Public Sub Remove(key As String) Implements ICacheModule.Remove
        _memoryCache.Remove(key)
    End Sub

    Public Sub RemoveByPattern(pattern As String) Implements ICacheModule.RemoveByPattern
        Dim cacheEntriesCollectionDefinition = GetType(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance)
        Dim cacheEntriesCollection As Dynamic.DynamicObject = cacheEntriesCollectionDefinition.GetValue(_memoryCache)

        Dim cacheCollectionValues As New List(Of ICacheEntry)

        For Each item In cacheCollectionValues
            Dim cacheItemValue As ICacheEntry = item.GetType().GetProperty("Value").GetValue(item, Nothing)
            cacheCollectionValues.Add(cacheItemValue)
        Next

        Dim regex = New Regex(pattern, RegexOptions.Singleline Or RegexOptions.Compiled Or RegexOptions.IgnoreCase)
        Dim keysToRemove = cacheCollectionValues.Where(Function(e) regex.IsMatch(e.Key.ToString())).Select(Function(s) s.Key).ToList()

        For Each item In keysToRemove
            _memoryCache.Remove(item)
        Next

    End Sub

    Public Function [Get](Of T)(key As String) As T Implements ICacheModule.Get
        Return _memoryCache.Get(Of T)(key)
    End Function

    Public Function [Get](key As String) As Object Implements ICacheModule.Get
        Return _memoryCache.Get(key)
    End Function

    Public Sub Add(key As String, value As Object, duration As Integer) Implements ICacheModule.Add
        _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration))
    End Sub

    Public Function IsAdd(key As String) As Boolean Implements ICacheModule.IsAdd
        Dim obj As Object
        Return _memoryCache.TryGetValue(key, obj)
    End Function
End Class
