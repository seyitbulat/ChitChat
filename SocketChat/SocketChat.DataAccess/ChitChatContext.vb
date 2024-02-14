Imports Microsoft.EntityFrameworkCore
Imports SocketChat.Model

Public Class ChitChatContext : Inherits DbContext

    Public Overridable Property Users As DbSet(Of User)
    Public Overridable Property Hubs As DbSet(Of Hub)
    Public Overridable Property Messages As DbSet(Of Message)
    Public Overridable Property Roles As DbSet(Of Role)
    Public Overridable Property UserRoles As DbSet(Of UserRole)

    Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
        MyBase.OnConfiguring(optionsBuilder)

        optionsBuilder.UseSqlServer("Server=10.19.10.42,1433;Initial Catalog=ChitChat;Persist Security Info=False;User ID=sa;Password=hehe123;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;")


    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
        MyBase.OnModelCreating(modelBuilder)

    End Sub


End Class
