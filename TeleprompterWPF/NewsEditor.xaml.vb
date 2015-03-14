' Interaction logic for NewsEditor.xaml
Partial Public Class NewsEditor
    Inherits System.Windows.Window

    Public StoriesList As New ArrayList
    Public TitlesList As New ArrayList

    Public Sub New(ByVal Stries As ArrayList, ByVal Ttles As ArrayList)

        InitializeComponent()

        If Stries Is Nothing OrElse Ttles Is Nothing Then Exit Sub

        For Each s As String In Stries
            Stories.Items.Add(GenTitle(s))
        Next

        TitlesList = Ttles
        StoriesList = Stries

    End Sub

    Public Function GenTitle(ByVal s As String) As String
        Return Strings.Left(s, 20) & "..."
    End Function

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub Add_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Add.Click
        If Not CancelUpdate.IsEnabled Then
            StoriesList.Add(StoryContent.Text)
            TitlesList.Add(StoryTitle.Text)
            Stories.Items.Add(GenTitle(StoryTitle.Text))
        Else
            StoriesList.Item(Stories.SelectedIndex) = StoryContent.Text
            TitlesList.Item(Stories.SelectedIndex) = StoryTitle.Text
            Stories.Items.Item(Stories.SelectedIndex) = GenTitle(StoryTitle.Text)
        End If
        StoryContent.Text = ""
        StoryTitle.Text = ""
        CancelUpdate.IsEnabled = False
    End Sub

    Private Sub OkClose_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles OkClose.Click
        Me.Close()
    End Sub

    Private Sub Stories_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles Stories.SelectionChanged
        If Stories.SelectedIndex = -1 Then
            Add.Content = "Add Story"
            StoryTitle.Text = ""
            StoryContent.Text = ""
            CancelUpdate.IsEnabled = False
            Exit Sub
        Else
            StoryTitle.Text = TitlesList.Item(Stories.SelectedIndex)
            StoryContent.Text = StoriesList.Item(Stories.SelectedIndex)
            Add.Content = "Update Story"
            CancelUpdate.IsEnabled = True
        End If
    End Sub

    Private Sub CancelUpdate_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles CancelUpdate.Click
        Stories.SelectedIndex = -1
        StoryContent.Text = ""
        StoryTitle.Text = ""
        CancelUpdate.IsEnabled = False
    End Sub

    Private Sub ImportRSS_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles ImportRSS.Click
        Try
            Dim xd As New Xml.XmlDocument
            Dim wc As New Net.WebClient
            xd.LoadXml(wc.DownloadString(RSSFeed.Text))
            wc.Dispose()
            For Each n As Xml.XmlNode In xd.SelectNodes("rss/channel/item")
                TitlesList.Add(n.Item("title").InnerText)
                Stories.Items.Add(GenTitle(n.Item("title").InnerText))
                StoriesList.Add(n.Item("description").InnerText)
            Next
            xd = Nothing
            MsgBox("Imported Successfully!")
        Catch ex As Exception
            MsgBox("There was an error downloading the RSS feed.")
        End Try
    End Sub
End Class
