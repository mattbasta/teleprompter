' Interaction logic for Window1.xaml
Imports System.Windows.Media.Animation
Partial Public Class Window1
    Inherits System.Windows.Window

    Public IAFS As Boolean = False 'Is At Full Screen?
    Public FSX As Integer = 0
    Public FSY As Integer = 0
    Public FSH As Integer = 0
    Public FSW As Integer = 0

    Public tan As Integer = 200

    Public t As New Timers.Timer(500)

    Public Stories As ArrayList
    Public Titles As ArrayList

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub SizeSlider_ValueChanged(ByVal sender As Object, ByVal e As System.Windows.RoutedPropertyChangedEventArgs(Of Double)) Handles SizeSlider.ValueChanged
        TelepText.FontSize = SizeSlider.Value
    End Sub

    Private Sub Window1_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Dim dax As New DoubleAnimation(Me.Width, New TimeSpan(2000000))
        TelepText.BeginAnimation(FrameworkElement.WidthProperty, dax)
        Dim day As New DoubleAnimation(Me.Height / 2 - 48, New TimeSpan(3500000))
        TelepText.BeginAnimation(Canvas.TopProperty, day)
        tan = Me.Height / 2 - 48
    End Sub

    Private Sub Window1_SizeChanged(ByVal sender As Object, ByVal e As System.Windows.SizeChangedEventArgs) Handles Me.SizeChanged
        Dim dax As New DoubleAnimation(Me.Width, New TimeSpan(2000000))
        TelepText.BeginAnimation(FrameworkElement.WidthProperty, dax)
    End Sub

    Private Sub Window1_StateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.StateChanged
        Dim w As Double
        If Me.WindowState = Windows.WindowState.Maximized Then
            w = Forms.Screen.PrimaryScreen.WorkingArea.Width
        Else
            w = Me.Width
        End If
        w -= 10
        TelepText.Width = w
        'TODO: This doesn't update the width! Why? It sets a valid value, but the value doesn't take!
    End Sub

    Private Sub FullScreen_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles FullScreen.Click
        If IAFS Then
            Me.Height = FSH
            Me.Width = FSW
            Me.Left = FSX
            Me.Top = FSY

            Me.WindowStyle = Windows.WindowStyle.SingleBorderWindow

        Else
            FSW = Me.Width
            FSH = Me.Height
            FSX = Me.Left
            FSY = Me.Top

            Me.Width = Forms.Screen.PrimaryScreen.WorkingArea.Width - 335
            Me.Height = Forms.Screen.PrimaryScreen.WorkingArea.Height - 200
            Me.Left = 0
            Me.Top = 0

            Me.WindowStyle = Windows.WindowStyle.None

        End If
        IAFS = Not IAFS
    End Sub

    Public Sub MoveText(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim day As New DoubleAnimation(tan - SpeedSlider.Value, New TimeSpan(3000000))
        AddHandler day.Completed, AddressOf MoveText
        TelepText.BeginAnimation(Canvas.TopProperty, day)
        tan -= SpeedSlider.Value
    End Sub
    Public Sub MoveText(ByVal sender As Object, ByVal e As System.EventArgs)
        MoveText(Nothing, Nothing)
    End Sub

    Private Sub ResetText_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles ResetText.Click
        Dim day As New DoubleAnimation(Me.Height / 2 - 48, New TimeSpan(3500000))
        AddHandler day.Completed, AddressOf MoveText
        TelepText.BeginAnimation(Canvas.TopProperty, day)
        tan = Me.Height / 2 - 48
    End Sub

    Private Sub EditNews_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles EditNews.Click
        EditNews.BitmapEffect = Nothing
        Dim ne As New NewsEditor(Stories, Titles)
        ne.ShowDialog()
        TelepText.Text = ""
        For Each s As String In ne.StoriesList
            TelepText.Text &= s & vbCrLf & vbCrLf
        Next
        ne = Nothing
    End Sub

    Private Sub ControlPanel_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles ControlPanel.MouseEnter
        Dim dao As New DoubleAnimation(1, New TimeSpan(2000000))
        ControlPanel.BeginAnimation(FrameworkElement.OpacityProperty, dao)
    End Sub

    Private Sub ControlPanel_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles ControlPanel.MouseLeave
        Dim dao As New DoubleAnimation(0.5, New TimeSpan(2000000))
        ControlPanel.BeginAnimation(FrameworkElement.OpacityProperty, dao)
    End Sub
End Class
