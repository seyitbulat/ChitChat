Imports System.Data
Imports System.Linq.Expressions
Imports Microsoft.Data.SqlClient
Imports Microsoft.EntityFrameworkCore
Imports SocketChat.Model

Public Class UserRepository : Inherits Repository(Of Long, User, ChitChatContext) : Implements IUserRepository

    Private ReadOnly _context As ChitChatContext

    Public Sub New(context As ChitChatContext)
        MyBase.New(context)
        _context = context
    End Sub

    Public Async Function GetListAsync(userParameters As UserParameters, Optional predicate As Expression(Of Func(Of User, Boolean)) = Nothing, Optional trackChanges As Boolean = False, Optional isActive As Boolean = True) As Task(Of PagedList(Of User)) Implements IUserRepository.GetListAsync
        Dim list

        If isActive = True Then
            If predicate IsNot Nothing Then
                list = Await _context.Users.Where(Function(e) e.IsActive = True).Where(predicate).Skip((userParameters.PageNumber - 1) * userParameters.PageSize).Take(userParameters.PageSize).ToListAsync()

            Else
                list = Await _context.Users.Where(Function(e) e.IsActive = True).Skip((userParameters.PageNumber - 1) * userParameters.PageSize).Take(userParameters.PageSize).ToListAsync()

            End If

        Else
            If predicate IsNot Nothing Then
                list = Await _context.Users.Where(predicate).Skip((userParameters.PageNumber - 1) * userParameters.PageSize).Take(userParameters.PageSize).ToListAsync()

            Else
                list = Await _context.Users.Skip((userParameters.PageNumber - 1) * userParameters.PageSize).Take(userParameters.PageSize).ToListAsync()

            End If
        End If


        Dim count = Await _context.Users.CountAsync()

        Return PagedList(Of User).ToPagedList(list, count, userParameters.PageNumber, userParameters.PageSize)
    End Function

    Public Async Function GetListWithProcedure() As Task(Of IEnumerable(Of User)) Implements IUserRepository.GetListWithProcedure
        Return Await Task.Run(Function()
                                  Using conn As New SqlConnection(_context.Database.GetDbConnection().ConnectionString)
                                      Using cmd As New SqlCommand("GetAllUsers", conn)
                                          cmd.CommandType = CommandType.StoredProcedure


                                          conn.Open()
                                          Dim reader As SqlDataReader = cmd.ExecuteReader()
                                          Dim users As New List(Of User)()

                                          While reader.Read()
                                              Dim user As New User()
                                              ' Burada, SqlDataReader'dan alınan verileri Employee nesnesinin özelliklerine atayın
                                              user.Id = If(IsDBNull(reader("Id")), Nothing, Convert.ToInt64(reader("Id")))
                                              user.Username = If(IsDBNull(reader("Username")), String.Empty, reader("Username").ToString())
                                              user.Email = If(IsDBNull(reader("Email")), String.Empty, reader("Email").ToString())
                                              user.Password = If(IsDBNull(reader("Password")), String.Empty, reader("Password").ToString())
                                              user.AddedDate = If(IsDBNull(reader("AddedDate")), Nothing, Convert.ToDateTime(reader("AddedDate")))
                                              user.AddedDate = If(IsDBNull(reader("UpdatedDate")), Nothing, Convert.ToDateTime(reader("UpdatedDate")))
                                              user.AddedDate = If(IsDBNull(reader("DeletedDate")), Nothing, Convert.ToDateTime(reader("DeletedDate")))

                                              users.Add(user)
                                          End While

                                          conn.Close()
                                          Return users
                                      End Using
                                  End Using
                              End Function)
    End Function


    Public Async Function AddUserAsync(user As User) As Task Implements IUserRepository.AddUserAsync
        Using conn As New SqlConnection(_context.Database.GetDbConnection().ConnectionString)
            Using cmd As New SqlCommand("AddUser", conn)
                cmd.CommandType = CommandType.StoredProcedure

                ' Kullanıcı bilgilerini parametre olarak ekle
                cmd.Parameters.Add(New SqlParameter("@Username", SqlDbType.NVarChar, 50)).Value = user.Username
                cmd.Parameters.Add(New SqlParameter("@Password", SqlDbType.NVarChar, 50)).Value = user.Password
                cmd.Parameters.Add(New SqlParameter("@Email", SqlDbType.NVarChar, 50)).Value = user.Email
                cmd.Parameters.Add(New SqlParameter("@IsActive", SqlDbType.Bit)).Value = user.IsActive



                ' Bağlantıyı aç ve stored procedure'ü çalıştır
                Await conn.OpenAsync()
                Await cmd.ExecuteNonQueryAsync()
            End Using
        End Using
    End Function


End Class
