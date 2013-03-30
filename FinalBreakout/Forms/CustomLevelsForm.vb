' CustomLevelsForm
' ---
' Form used to allow the player to select which custom levels he or she would like to play.
' 
' Author   : Michael Falcone
' Modified : 4/06/10



Public Class CustomLevelsForm


    Private mSelectedLevels() As BreakoutLevel





    ''' <summary>
    ''' Gets the array of selected that the user has selected.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property SelectedLevels() As BreakoutLevel()
        Get
            Return mSelectedLevels
        End Get
    End Property






    ''' <summary>
    ''' Loads the level data into the listbox.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CustomLevelsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        ' make sure there are custom levels
        If Breakout.GetCustomLevels() Is Nothing Then

            MsgBox("No custom levels available.", MsgBoxStyle.Information, "No custom levels")
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            Return
        End If




        LevelListBox.Items.Clear()


        For Each level In Breakout.GetCustomLevels()

            LevelListBox.Items.Add(level)
        Next


        LevelListBox.SelectedIndex = 0

        SetListCheckedState(CheckState.Checked)

    End Sub



    ''' <summary>
    ''' Builds the array of selected levels and closes the dialog.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BeginButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BeginButton.Click


        Dim maxCheckedIndex As Integer = LevelListBox.CheckedItems.Count - 1


        If maxCheckedIndex < 0 Then
            MsgBox("You must select at least one level to play!", MsgBoxStyle.Exclamation, "Error")
            Return
        End If



        ReDim mSelectedLevels(maxCheckedIndex)

        Dim i As Integer
        For i = 0 To maxCheckedIndex

            mSelectedLevels(i) = CType(LevelListBox.CheckedItems(i), BreakoutLevel)

        Next


        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub


    ''' <summary>
    ''' Selects each element in the list.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub SelectAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllButton.Click

        SetListCheckedState(CheckState.Checked)
    End Sub


    ''' <summary>
    ''' Deselects each element in the list.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DeselectAllButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeselectAllButton.Click

        SetListCheckedState(CheckState.Unchecked)

    End Sub



    ''' <summary>
    ''' Displays a level preview when an element is selected.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub LevelListBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LevelListBox.SelectedIndexChanged

        If LevelListBox.SelectedIndex < 0 Then
            Return
        End If

        With CType(LevelListBox.Items(LevelListBox.SelectedIndex), BreakoutLevel)

            PreviewPictureBox.Image = .GeneratePreview(PreviewPictureBox.Width, PreviewPictureBox.Height)

            If .AuthorName <> "" Then
                AuthorLabel.Text = "Author: " + .AuthorName
            Else
                AuthorLabel.Text = ""
            End If


            BlocksLabel.Text = "Blocks: " + .BlockSet.NumSolidBlocks.ToString()


            If .HighScores.Scores(0).ScoreValue = 0 Then

                HighScoreLabel.Hide()

            Else

                HighScoreLabel.Show()
                HighScoreLabel.Text = "High score: " + BreakoutDisplay.FormatScore(.HighScores.Scores(0).ScoreValue) _
                + " by " + .HighScores.Scores(0).ScoreHolder

            End If


        End With

    End Sub


    ''' <summary>
    ''' Moves the selected element down in the list.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MoveDownButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveDownButton.Click

        Dim curIndex As Integer = LevelListBox.SelectedIndex
        Dim nextIndex As Integer = curIndex + 1

        Dim curObject As Object
        Dim nextObject As Object

        Dim curChecked As Boolean
        Dim nextChecked As Boolean


        ' if it's not possible to move down then return
        If nextIndex >= LevelListBox.Items.Count Then
            Return
        End If


        ' store objects in variables
        curObject = LevelListBox.Items(curIndex)
        curChecked = LevelListBox.GetItemChecked(curIndex)

        nextObject = LevelListBox.Items(nextIndex)
        nextChecked = LevelListBox.GetItemChecked(nextIndex)


        ' put objects back in list in each other's previous position
        LevelListBox.Items(curIndex) = nextObject
        LevelListBox.SetItemChecked(curIndex, nextChecked)

        LevelListBox.Items(nextIndex) = curObject
        LevelListBox.SetItemChecked(nextIndex, curChecked)


        LevelListBox.SelectedIndex = nextIndex

    End Sub



    ''' <summary>
    ''' Moves the selected element in the list up.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MoveUpButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveUpButton.Click

        Dim curIndex As Integer = LevelListBox.SelectedIndex
        Dim prevIndex As Integer = curIndex - 1

        Dim curObject As Object
        Dim prevObject As Object

        Dim curChecked As Boolean
        Dim prevChecked As Boolean


        ' if it's not possible to move up then return
        If prevIndex < 0 Then
            Return
        End If


        ' store objects in variables
        curObject = LevelListBox.Items(curIndex)
        curChecked = LevelListBox.GetItemChecked(curIndex)

        prevObject = LevelListBox.Items(prevIndex)
        prevChecked = LevelListBox.GetItemChecked(prevIndex)


        ' put objects back in list in each other's previous position
        LevelListBox.Items(curIndex) = prevObject
        LevelListBox.SetItemChecked(curIndex, prevChecked)

        LevelListBox.Items(prevIndex) = curObject
        LevelListBox.SetItemChecked(prevIndex, curChecked)


        LevelListBox.SelectedIndex = prevIndex


    End Sub


    ''' <summary>
    ''' Sets the check state of each of the list elements.
    ''' </summary>
    ''' <param name="state"></param>
    ''' <remarks></remarks>
    Private Sub SetListCheckedState(ByVal state As CheckState)

        Dim i As Integer
        For i = 0 To LevelListBox.Items.Count - 1
            LevelListBox.SetItemCheckState(i, state)
        Next

    End Sub


End Class