Imports System.Text
Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports System.Windows.Forms
Imports DevExpress.Xpf.Core

''' <summary>
''' Interaction logic for MainWindow.xaml
''' </summary>
Partial Public Class MainWindow
    Inherits ThemedWindow

    Public Property User As User

    Public Sub New()
        InitializeComponent()


    End Sub

    Public Sub New(user As User)
        InitializeComponent()

        Me.User = user

        lblUserName.Content = Me.User.UserName

        Dim hubView = New HubView()
        SideBar.Content = hubView
        AddHandler hubView.HubSelected, AddressOf HubSelectedHandler
    End Sub

    Private Sub HubSelectedHandler(hubName As String)
        MainContent.Content = New ChatView(hubName, User)
    End Sub
End Class