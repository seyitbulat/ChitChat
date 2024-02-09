Imports System.Text.Json
Imports DevExpress.Xpf.Core

Public Class HubView
    Public Sub New()
        InitializeComponent()
        LoadHubsAsync()
    End Sub

    Private Async Sub LoadHubsAsync()
        Try
            Dim client = ApiHelper.HttpClient
            Dim response = Await client.GetAsync("/Hub")

            If response.IsSuccessStatusCode Then
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim hubList = JsonSerializer.Deserialize(Of List(Of String))(json)
                HubsControl.ItemsSource = hubList
            Else
                ' Hata yönetimi
            End If
        Catch ex As Exception
            ' Hata yönetimi
        End Try
    End Sub

    Private Sub HubButton_Click(sender As Object, e As RoutedEventArgs)
        Dim button As SimpleButton = DirectCast(sender, SimpleButton)
        Dim hubName As String = button.Content.ToString()

        ' Burada hubName'i kullanarak gerekli işlemleri yapabilirsiniz.
        ' Örneğin, MainWindow'a bir mesaj gönderebilirsiniz.
        RaiseEvent HubSelected(hubName)
    End Sub

    Public Event HubSelected(hubName As String)

End Class