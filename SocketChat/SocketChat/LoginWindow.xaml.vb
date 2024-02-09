Public Class LoginWindow
    Private ReadOnly _isLogined As Boolean = False
    Public Property User As User

    Private Sub btnLogin_Click(sender As Object, e As RoutedEventArgs) Handles btnLogin.Click
        Dim userName = txtUserName.Text
        User = New User(userName)

        Dim main = New MainWindow(User)

        main.Show()
        Me.Close()
    End Sub
End Class
