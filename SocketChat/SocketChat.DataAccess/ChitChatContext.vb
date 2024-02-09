Imports Microsoft.EntityFrameworkCore
Imports SocketChat.Model

Public Class ChitChatContext : Inherits DbContext

    Public Overridable Property Users As DbSet(Of User)
    Public Overridable Property Hubs As DbSet(Of Hub)
    Public Overridable Property Messages As DbSet(Of Message)

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        MyBase.OnConfiguring(optionsBuilder)

        optionsBuilder.UseSqlServer("Data Source=DESKTOP-R04PVQ3;Initial Catalog=ChitChat;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;")
    End Sub


End Class
