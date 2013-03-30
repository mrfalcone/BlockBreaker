' HighScoresForm
' ---
' Form showing the high scores for normal and custom game sessions.
' 
' Author   : Michael Falcone
' Modified : 4/14/10


Public Class HighScoresForm


    ' constants
    Private Const cMaxHighScoresCustomSession As Integer = 5
    Private Const cMaxHighScoresNormalSession As Integer = 10



    ' private member data
    Private mHighlightColor As Color = Color.Blue

    Private mHighlightedIndex As Integer = -1

    Private mHighlightedLevel As BreakoutLevel = Nothing




    ''' <summary>
    ''' Gets the maximum number of normal session high scores supported by the form.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared ReadOnly Property MaxHighScoresNormal() As Integer
        Get
            Return cMaxHighScoresNormalSession
        End Get
    End Property



    ''' <summary>
    ''' Gets the maximum number of custom session high scores supported by the form.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared ReadOnly Property MaxHighScoresCustom() As Integer
        Get
            Return cMaxHighScoresCustomSession
        End Get
    End Property


    ''' <summary>
    ''' Loads the high scores into the form to be displayed.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub HighScoresForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        ' store the custom levels tab page in case we need to remove and re-add it
        Static customLevelsTab As TabPage = ScoresTabControl.TabPages(1)


        ' set highlighting of normal session high scores
        InitNormalScores()




        ' load custom levels into the listbox

        Dim customLevels() As BreakoutLevel = Breakout.GetCustomLevels()


        If customLevels Is Nothing Then

            ' remove the custom levels tab if there are no custom levels to show
            If ScoresTabControl.TabCount = 2 Then
                ScoresTabControl.TabPages.RemoveAt(1)
            End If



        Else


            If ScoresTabControl.TabCount < 2 Then
                ScoresTabControl.TabPages.Add(customLevelsTab)
            End If



            ' store the index of the score to be highlighted
            Dim highlightScoreIndex As Integer = mHighlightedIndex


            LevelListBox.Items.Clear()

            For Each level In customLevels

                LevelListBox.Items.Add(level)
            Next


            ' select the level to display
            LevelListBox.SelectedIndex = 0

            If mHighlightedLevel IsNot Nothing Then


                Dim i As Integer
                For i = 0 To LevelListBox.Items.Count - 1

                    If mHighlightedLevel.Equals(LevelListBox.Items(i)) Then
                        mHighlightedIndex = highlightScoreIndex
                        LevelListBox.SelectedIndex = i
                        Exit For

                    End If


                Next


                mHighlightedIndex = -1


            End If


        End If


    End Sub



    ''' <summary>
    ''' Initialize the normal session scores.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitNormalScores()

        With Breakout.GetGameHighScores()

            Dim i As Integer
            For i = 0 To .MaxScoreIndex
                SetEntry(i, .Scores(i), False)

                If i = mHighlightedIndex And mHighlightedLevel Is Nothing Then
                    SetEntryColor(i, mHighlightColor, False)
                    mHighlightedIndex = -1
                Else
                    SetEntryColor(i, Color.Black, False)
                End If

            Next

        End With

    End Sub




    ''' <summary>
    ''' Overloads the ShowDialog method to display scores with no highlighting.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub ShowDialog()

        mHighlightedIndex = -1

        ScoresTabControl.SelectedIndex = 0

        mHighlightedLevel = Nothing

        MyBase.ShowDialog()
    End Sub



    ''' <summary>
    ''' Overloads the ShowDialog method to highlight the high score entry at the specified index.
    ''' </summary>
    ''' <param name="index">the index of the entry to highlight</param>
    ''' <remarks></remarks>
    Public Overloads Sub ShowDialog(ByVal index As Integer)

        mHighlightedIndex = index

        mHighlightedLevel = Nothing


        ScoresTabControl.SelectedIndex = 0
        MyBase.ShowDialog()

    End Sub




    ''' <summary>
    ''' Overloads the ShowDialog method to highlight the high score entry at the specified index on the
    ''' specified level.
    ''' </summary>
    ''' <param name="index">the index of the entry to highlight</param>
    ''' <param name="level">the level to which the highscore belongs</param>
    ''' <remarks></remarks>
    Public Overloads Sub ShowDialog(ByVal index As Integer, ByVal level As BreakoutLevel)


        ScoresTabControl.SelectedIndex = 1

        mHighlightedIndex = index

        mHighlightedLevel = level

        MyBase.ShowDialog()

    End Sub




    ''' <summary>
    ''' Sets the forecolor of the entry specified by the index to the specified color.
    ''' </summary>
    ''' <param name="index"></param>
    ''' <param name="c"></param>
    ''' <param name="isCustom">specifies whether the entry is for a custom level</param>
    ''' <remarks></remarks>
    Private Sub SetEntryColor(ByVal index As Integer, ByVal c As Color, ByVal isCustom As Boolean)

        Select Case index

            Case 0
                If isCustom Then
                    CustomNameLabel0.ForeColor = c
                    CustomScoreLabel0.ForeColor = c
                Else
                    NormalNameLabel0.ForeColor = c
                    NormalScoreLabel0.ForeColor = c
                End If

                Exit Select

            Case 1
                If isCustom Then
                    CustomNameLabel1.ForeColor = c
                    CustomScoreLabel1.ForeColor = c
                Else
                    NormalNameLabel1.ForeColor = c
                    NormalScoreLabel1.ForeColor = c
                End If

                Exit Select

            Case 2
                If isCustom Then
                    CustomNameLabel2.ForeColor = c
                    CustomScoreLabel2.ForeColor = c
                Else
                    NormalNameLabel2.ForeColor = c
                    NormalScoreLabel2.ForeColor = c
                End If

                Exit Select

            Case 3
                If isCustom Then
                    CustomNameLabel3.ForeColor = c
                    CustomScoreLabel3.ForeColor = c
                Else
                    NormalNameLabel3.ForeColor = c
                    NormalScoreLabel3.ForeColor = c
                End If

                Exit Select

            Case 4
                If isCustom Then
                    CustomNameLabel4.ForeColor = c
                    CustomScoreLabel4.ForeColor = c
                Else
                    NormalNameLabel4.ForeColor = c
                    NormalScoreLabel4.ForeColor = c
                End If

                Exit Select

            Case 5
                NormalNameLabel5.ForeColor = c
                NormalScoreLabel5.ForeColor = c
                Exit Select

            Case 6
                NormalNameLabel6.ForeColor = c
                NormalScoreLabel6.ForeColor = c
                Exit Select

            Case 7
                NormalNameLabel7.ForeColor = c
                NormalScoreLabel7.ForeColor = c
                Exit Select

            Case 8
                NormalNameLabel8.ForeColor = c
                NormalScoreLabel8.ForeColor = c
                Exit Select

            Case 9
                NormalNameLabel9.ForeColor = c
                NormalScoreLabel9.ForeColor = c
                Exit Select

        End Select

    End Sub



    ''' <summary>
    ''' Displays the specified score at the normal game session entry specified by the index.
    ''' </summary>
    ''' <param name="index">index of the score in the collection and form</param>
    ''' <param name="score">score object</param>
    ''' <param name="isCustom">true if the score should be displayed as a custom level score</param>
    ''' <remarks></remarks>
    Private Sub SetEntry(ByVal index As Integer, ByVal score As HighScoresCollection.HighScore, ByVal isCustom As Boolean)


        Dim nameString As String = score.ScoreHolder
        Dim scoreString As String


        If score.ScoreValue = 0 Then
            scoreString = "-"
        Else
            scoreString = BreakoutDisplay.FormatScore(score.ScoreValue)
        End If


        Select Case index

            Case 0
                If isCustom Then
                    CustomNameLabel0.Text = nameString
                    CustomScoreLabel0.Text = scoreString
                Else
                    NormalNameLabel0.Text = nameString
                    NormalScoreLabel0.Text = scoreString
                End If

                Exit Select

            Case 1
                If isCustom Then
                    CustomNameLabel1.Text = nameString
                    CustomScoreLabel1.Text = scoreString
                Else
                    NormalNameLabel1.Text = nameString
                    NormalScoreLabel1.Text = scoreString
                End If

                Exit Select

            Case 2
                If isCustom Then
                    CustomNameLabel2.Text = nameString
                    CustomScoreLabel2.Text = scoreString
                Else
                    NormalNameLabel2.Text = nameString
                    NormalScoreLabel2.Text = scoreString
                End If

                Exit Select

            Case 3
                If isCustom Then
                    CustomNameLabel3.Text = nameString
                    CustomScoreLabel3.Text = scoreString
                Else
                    NormalNameLabel3.Text = nameString
                    NormalScoreLabel3.Text = scoreString
                End If

                Exit Select

            Case 4
                If isCustom Then
                    CustomNameLabel4.Text = nameString
                    CustomScoreLabel4.Text = scoreString
                Else
                    NormalNameLabel4.Text = nameString
                    NormalScoreLabel4.Text = scoreString
                End If

                Exit Select

            Case 5
                NormalNameLabel5.Text = nameString
                NormalScoreLabel5.Text = scoreString
                Exit Select

            Case 6
                NormalNameLabel6.Text = nameString
                NormalScoreLabel6.Text = scoreString
                Exit Select

            Case 7
                NormalNameLabel7.Text = nameString
                NormalScoreLabel7.Text = scoreString
                Exit Select

            Case 8
                NormalNameLabel8.Text = nameString
                NormalScoreLabel8.Text = scoreString
                Exit Select

            Case 9
                NormalNameLabel9.Text = nameString
                NormalScoreLabel9.Text = scoreString
                Exit Select


        End Select


    End Sub



    ''' <summary>
    ''' Loads the scores when a level is selected.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub LevelListBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LevelListBox.SelectedIndexChanged

        InitCustomScores(LevelListBox.SelectedIndex)

        mHighlightedIndex = -1

    End Sub




    ''' <summary>
    ''' Initialize the display of the scores at the specified index in the listbox.
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Private Sub InitCustomScores(ByVal index As Integer)

        With CType(LevelListBox.Items(index), BreakoutLevel).HighScores

            Dim i As Integer
            For i = 0 To .MaxScoreIndex
                SetEntry(i, .Scores(i), True)
                If mHighlightedIndex = i Then
                    SetEntryColor(i, mHighlightColor, True)
                Else
                    SetEntryColor(i, Color.Black, True)
                End If

            Next

        End With

    End Sub




    ''' <summary>
    ''' Closes the form when the ok button is pressed.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkButton.Click
        Me.Close()
    End Sub



    ''' <summary>
    ''' Prompts the user to continue then clears every high score entry.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ClearAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearAllButton.Click

        Dim r As MsgBoxResult = _
        MsgBox("This will clear ALL high scores for a normal session and custom levels. Are you sure you would like to continue?", _
               MsgBoxStyle.Information Or MsgBoxStyle.YesNo, "Clear all scores?")


        If r = MsgBoxResult.No Then
            Return
        End If


        mHighlightedIndex = -1


        Dim normalScores As HighScoresCollection = Breakout.GetGameHighScores()
        normalScores.Clear()
        Breakout.SaveGameHighScores(normalScores)


        InitNormalScores()


        ' loop through each level and clear the scores
        Dim i As Integer
        For i = 0 To LevelListBox.Items.Count - 1

            Dim l As BreakoutLevel = CType(LevelListBox.Items(i), BreakoutLevel)

            Dim scores As HighScoresCollection = l.HighScores
            scores.Clear()
            l.SaveHighScores(scores)

            If LevelListBox.SelectedIndex = i Then
                InitCustomScores(i)
            End If

        Next

    End Sub


End Class
