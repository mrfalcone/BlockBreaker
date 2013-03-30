' NumberSlider
' ---
' A form control with a sliding number bar and a text input box.
' 
' Author   : Michael Falcone
' Email    : mr.falcone@gmail.com
' Modified : 3/08/10


' force variable declaration
Option Explicit On

<System.ComponentModel.DefaultEvent("ValueChanged")> _
Public Class NumberSlider
    Inherits UserControl


    ' events
    Public Event ValueChanged()




    ' private members
    Private mMaxValue As Integer
    Private mMinValue As Integer

    Private mCurValue As Integer



    ''' <summary>
    ''' Gets or sets the maximum value that the slider can be.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Specifies the maximum value the number slider can represent.")> _
    Property MaximumValue() As Integer
        Get
            Return mMaxValue
        End Get
        Set(ByVal value As Integer)

            mMaxValue = value
            TrackBar.Maximum = value
        End Set
    End Property




    ''' <summary>
    ''' Gets or sets the minimum value that the slider can be.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Specifies the minimum value the number slider can represent.")> _
    Property MinimumValue() As Integer
        Get
            Return mMinValue
        End Get
        Set(ByVal value As Integer)
            mMinValue = value
            TrackBar.Minimum = value
        End Set
    End Property




    ''' <summary>
    ''' Gets or sets the current value of the slider.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Specifies the current value of the slider.")> _
    Property Value() As Integer
        Get
            Return mCurValue
        End Get
        Set(ByVal value As Integer)
            Static current As Integer
            current = mCurValue

            mCurValue = value
            TrackBar.Value = ValidateInput(value)
            TextBox.Text = ValidateInput(value)

            If value <> current Then
                RaiseEvent ValueChanged()
            End If

        End Set
    End Property



    ''' <summary>
    ''' Gets or sets whether the slider bar can receive tab focus.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Specifies whether or not the slider bar can receive tab focus.")> _
    Property SliderTabStop() As Boolean
        Get
            Return TrackBar.TabStop
        End Get
        Set(ByVal value As Boolean)
            TrackBar.TabStop = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets whether to show ticks under the slider.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Specifies whether to show ticks under the slider bar.")> _
    Property ShowTicks() As Boolean
        Get
            Return (TrackBar.TickStyle <> TickStyle.None)
        End Get
        Set(ByVal value As Boolean)

            If value = True Then
                TrackBar.TickStyle = TickStyle.BottomRight
            Else
                TrackBar.TickStyle = TickStyle.None
            End If
        End Set
    End Property




    ''' <summary>
    ''' Constructs a new number slider with a maximum value of 10.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        MaximumValue = 10
        MinimumValue = 0
        Value = 0

    End Sub





    ''' <summary>
    ''' Convert an input string to a valid value.
    ''' </summary>
    ''' <param name="input">Text to validate.</param>
    ''' <returns>The integer created from the string if it is numeric. If not numeric, the current
    ''' value of the control is returned. If the string is numeric but the value is out of bounds,
    ''' the maximum value is returned.</returns>
    ''' <remarks></remarks>
    Private Function ValidateInput(ByVal input As String) As Integer

        Dim num As Integer


        ' first check to see if the string is a valid number
        Try
            num = input
        Catch ex As Exception
            Return Value     ' if the string is not numeric then return the current value
        End Try


        ' next make sure the number is within bounds
        Return ValidateInput(num)

    End Function



    ''' <summary>
    ''' Convert an input integer to a valid integer by verifying that it is within bounds.
    ''' </summary>
    ''' <param name="input">Integer to validate.</param>
    ''' <returns>a valid integer confined to [MinimumValue, MaximumValue]</returns>
    ''' <remarks></remarks>
    Private Function ValidateInput(ByVal input As Integer) As Integer


        If input > MaximumValue Then
            Return MaximumValue
        ElseIf input < MinimumValue Then
            Return MinimumValue
        End If


        Return input

    End Function


    Private Sub TextBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox.KeyDown

        If e.KeyCode = Keys.Left Then
            Value = ValidateInput(Value - 1)
        ElseIf e.KeyCode = Keys.Right Then
            Value = ValidateInput(Value + 1)
        End If

    End Sub



    Private Sub TextBox_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox.Leave

        Value = ValidateInput(TextBox.Text)

    End Sub



    Private Sub TrackBar_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBar.Scroll

        Value = TrackBar.Value

    End Sub


    Private Sub NumberSlider_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize


        Dim newWidth As Integer = Me.Width - TrackBar.Location.X - 5
        Dim newHeight As Integer = TrackBar.Location.Y + TrackBar.Height + 10


        ' resize the trackbar width keeping its position
        If TrackBar.Location.X + TrackBar.Width <> newWidth Then
            TrackBar.Width = newWidth
        End If


        ' resize the form height to stay with the trackbar
        If Me.Height <> newHeight Then
            Me.Height = newHeight
        End If

    End Sub


End Class
