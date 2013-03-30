' NewScoreEntryForm
' ---
' Form allowing the user to enter his or her name upon earning a high score, and saves the high score entry.
' 
' Author   : Michael Falcone
' Modified : 4/16/10


Public Class NewScoreEntryForm



    ' private members
    Private mName As String = ""

    Private mScore As Integer = 0

    Private mLevel As BreakoutLevel = Nothing


    Private mEntryIndex As Integer = -1


    Private mScoreCollection As HighScoresCollection = Nothing




    ''' <summary>
    ''' Gets or sets the value of the high score to be displayed when the form loads.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Score() As Integer
        Get
            Return mScore
        End Get
        Set(ByVal value As Integer)
            mScore = value
        End Set
    End Property




    ''' <summary>
    ''' Gets or sets the level to which the high score belongs. If nothing, the score is for a normal session.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Level() As BreakoutLevel
        Get
            Return mLevel
        End Get
        Set(ByVal value As BreakoutLevel)
            mLevel = value
        End Set
    End Property


    ''' <summary>
    ''' Returns the name entered by the user.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property EntryName() As String
        Get
            Return mName
        End Get
    End Property



    ''' <summary>
    ''' Gets the index of the created score entry.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property EntryIndex() As Integer
        Get
            Return mEntryIndex
        End Get
    End Property




    ''' <summary>
    ''' Stores the name entered by the user and saves the high score entry.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub NameInputForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If AcceptInput() = False Then
            e.Cancel = True

        Else


            Try
                ' store the entered name in the collection and save it
                mScoreCollection.Scores(mEntryIndex).ScoreHolder = mName
                SaveScores()

            Catch ex As Exception
                MsgBox("Error saving high score!", MsgBoxStyle.Critical)

                Me.DialogResult = Windows.Forms.DialogResult.Abort
                Return
            End Try


            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If

    End Sub



    ''' <summary>
    ''' Initialize the form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub NameInputForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        NameTextBox.SelectAll()

        ScoreLabel.Text = BreakoutDisplay.FormatScore(mScore)

        mEntryIndex = -1

        mScoreCollection = Nothing


        If Level Is Nothing Then
            mEntryIndex = NewNormalEntry()
        Else
            mEntryIndex = NewCustomEntry()
        End If


    End Sub




    ''' <summary>
    ''' Creates a new score entry in the custom level and returns its index.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function NewCustomEntry() As Integer

        Dim index As Integer = -1

        Try
            mScoreCollection = mLevel.HighScores
            index = mScoreCollection.AddNew(mScore)
        Catch ex As Exception
            MsgBox("Error creating high score!", MsgBoxStyle.Critical)
        End Try

        Return index

    End Function



    ''' <summary>
    ''' Creates a new score entry for a normal session and returns its index.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function NewNormalEntry() As Integer

        Dim index As Integer = -1

        Try
            mScoreCollection = Breakout.GetGameHighScores()
            index = mScoreCollection.AddNew(mScore)
        Catch ex As Exception
            MsgBox("Error creating high score!", MsgBoxStyle.Critical)
        End Try

        Return index

    End Function




    ''' <summary>
    ''' Saves the score collection. 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveScores()

        If mLevel Is Nothing Then
            Breakout.SaveGameHighScores(mScoreCollection)

        Else
            mLevel.SaveHighScores(mScoreCollection)
        End If


    End Sub




    ''' <summary>
    ''' Validates the text in the name text box and sets the NameString property.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <returns>true if name is valid, false otherwise</returns>
    Private Function AcceptInput() As Boolean


        If IsInputValid(NameTextBox.Text) Then

            mName = NameTextBox.Text
            Return True
        End If


        MsgBox("The name you have entered is invalid.", MsgBoxStyle.Exclamation, "Invalid Name")
        NameTextBox.SelectAll()

        Return False

    End Function



    ''' <summary>
    ''' Tests whether the specified string is valid input.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsInputValid(ByVal text As String) As Boolean


        If text = "" Then
            Return False
        End If


        Dim i As Integer
        For i = 0 To text.Length - 1

            If text.Chars(i) Like "[A-Z]" Or text.Chars(i) Like "[a-z]" Or text.Chars(i) Like " " Then
                Continue For
            Else
                Return False
            End If

        Next


        Return True

    End Function



    ''' <summary>
    ''' Accepts the name entered by the user.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkButton.Click

        If AcceptInput() Then

            Me.Close()

        End If
    End Sub



End Class