' LevelMakerForm
' ---
' Main form of the level designer for a breakout game.
' 
' Author   : Michael Falcone
' Modified : 4/06/10


Public Class LevelDesignerForm


    ' constants
    Private Const cDefaultAuthor As String = ""
    Private Const cDefaultLevelName As String = "Untitled Level"

    Private Const cTitleText As String = " - Level Designer"


    ' private member data
    Private mIsUnsaved As Boolean = False

    Private mRequiresDraw As Boolean = False

    Private mActiveFilename As String = ""

    Private mIsLevelLocked As Boolean = False    ' will prevent the designer from altering the level data while loading


    Private WithEvents mLevel As BreakoutLevel



    ''' <summary>
    ''' Prompts the user to save data when the form is closing.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub LevelDesignerForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        ' if the level has been changed since the last save
        If mIsUnsaved Then

            Dim r As MsgBoxResult
            r = MsgBox("Your level has not been saved. Would you like to save before closing the designer?", _
                       MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Exclamation, "Close Designer")

            Select Case r
                Case MsgBoxResult.Yes
                    SaveLevel()
                    Exit Select

                Case MsgBoxResult.No
                    Exit Select

                Case MsgBoxResult.Cancel
                    e.Cancel = True
                    Return

            End Select

        End If


        DrawTimer.Stop()

    End Sub




    ''' <summary>
    ''' Initialize the level designer.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub LevelMakerForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        mLevel = New BreakoutLevel(True)



        ' set image style trackbar values
        BlockStyleTrackbar.Maximum = Breakout.BlockImageMaximum
        BackgroundTrackBar.Maximum = Breakout.BackgroundImageMaximum



        ' set text box values
        AuthorNameTextBox.Text = cDefaultAuthor
        LevelNameTextBox.Text = cDefaultLevelName


        LoadDesignerToLevel()


        mIsUnsaved = False

        DrawTimer.Start()

    End Sub




    '' Rowslider event handlers
    Private Sub RowSlider0_ValueChanged() Handles RowSlider0.ValueChanged
        ChangeRowValue(0, RowSlider0.Value)
    End Sub

    Private Sub RowSlider1_ValueChanged() Handles RowSlider1.ValueChanged
        ChangeRowValue(1, RowSlider1.Value)
    End Sub

    Private Sub RowSlider2_ValueChanged() Handles RowSlider2.ValueChanged
        ChangeRowValue(2, RowSlider2.Value)
    End Sub

    Private Sub RowSlider3_ValueChanged() Handles RowSlider3.ValueChanged
        ChangeRowValue(3, RowSlider3.Value)
    End Sub

    Private Sub RowSlider4_ValueChanged() Handles RowSlider4.ValueChanged
        ChangeRowValue(4, RowSlider4.Value)
    End Sub

    Private Sub RowSlider5_ValueChanged() Handles RowSlider5.ValueChanged
        ChangeRowValue(5, RowSlider5.Value)
    End Sub

    Private Sub RowSlider6_ValueChanged() Handles RowSlider6.ValueChanged
        ChangeRowValue(6, RowSlider6.Value)
    End Sub

    Private Sub RowSlider7_ValueChanged() Handles RowSlider7.ValueChanged
        ChangeRowValue(7, RowSlider7.Value)
    End Sub

    Private Sub RowSlider8_ValueChanged() Handles RowSlider8.ValueChanged
        ChangeRowValue(8, RowSlider8.Value)
    End Sub

    Private Sub RowSlider9_ValueChanged() Handles RowSlider9.ValueChanged
        ChangeRowValue(9, RowSlider9.Value)
    End Sub



    ''' <summary>
    ''' Changes the value of the specified row to the specified value.
    ''' </summary>
    ''' <param name="rowIndex"></param>
    ''' <param name="newValue"></param>
    ''' <remarks></remarks>
    Private Sub ChangeRowValue(ByVal rowIndex As Integer, ByVal newValue As Integer)

        If mIsLevelLocked Then
            Return
        End If

        mLevel.BlockSet.SetRowValue(rowIndex, newValue)

        mIsUnsaved = True

        mRequiresDraw = True

    End Sub



    ' trackbar event handlers
    Private Sub BlockStyleTrackbar_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlockStyleTrackbar.Scroll

        If mIsLevelLocked Then
            Return
        End If

        mIsUnsaved = True

        mRequiresDraw = True

        mLevel.BlockStyle = BlockStyleTrackbar.Value

    End Sub

    Private Sub BackgroundTrackBar_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackgroundTrackBar.Scroll

        If mIsLevelLocked Then
            Return
        End If

        mIsUnsaved = True

        mRequiresDraw = True

        mLevel.BackgroundStyle = BackgroundTrackBar.Value

    End Sub





    ''' <summary>
    ''' Resets the designer.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ResetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetButton.Click


        ' find out if the user really wants to clear the level
        Dim r As MsgBoxResult = MsgBox("This will clear the set of blocks. Are you sure you would like to do this?", _
                                       MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation, "Clear Blocks")

        If r = MsgBoxResult.No Then
            Return
        End If

        ClearLevel()

    End Sub



    ''' <summary>
    ''' Updates the level's name.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub LevelNameTextBox_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles LevelNameTextBox.Leave

        UpdateNameText()

    End Sub




    ''' <summary>
    ''' Updates the author's name.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AuthorNameTextBox_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles AuthorNameTextBox.Leave

        UpdateNameText()
        
    End Sub



    ''' <summary>
    ''' Handle 'New' click.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click


        ' if the level has been changed since the last save
        If mIsUnsaved Then

            Dim r As MsgBoxResult
            r = MsgBox("Your level has not been saved. Would you like to save before creating a new level?", _
                       MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Exclamation, "New Level")

            Select Case r
                Case MsgBoxResult.Yes
                    SaveLevel()
                    Exit Select

                Case MsgBoxResult.No
                    Exit Select

                Case MsgBoxResult.Cancel
                    Return

            End Select

        End If


        ClearLevel()

        ' reset trackbars
        BlockStyleTrackbar.Value = 0
        BackgroundTrackBar.Value = 0


        ' reset level author and name data
        LevelNameTextBox.Text = cDefaultLevelName
        AuthorNameTextBox.Text = cDefaultAuthor


        LoadDesignerToLevel()

        mActiveFilename = ""

        mIsUnsaved = False
        mRequiresDraw = True

    End Sub


    ''' <summary>
    ''' Close the designer.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub



    ''' <summary>
    ''' Clears all level data.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearLevel()

        ' reset slider values
        RowSlider0.Value = 0
        RowSlider1.Value = 0
        RowSlider2.Value = 0
        RowSlider3.Value = 0
        RowSlider4.Value = 0
        RowSlider5.Value = 0
        RowSlider6.Value = 0
        RowSlider7.Value = 0
        RowSlider8.Value = 0
        RowSlider9.Value = 0


    End Sub



    ''' <summary>
    ''' Prompts the user for a save location and saves the level.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveLevel(Optional ByVal prompt As Boolean = False)


        If Not IsLevelValid() Then
            Return
        End If


        LoadDesignerToLevel()

        Dim file As String = mActiveFilename


        ' only show the prompt if it is necessary
        If prompt = True Or file = "" Then


            ' initialize the save dialog
            Static dialog As SaveFileDialog = New SaveFileDialog()

            dialog.Filter = "Breakout Level Files (*.xml)|*.xml"
            dialog.Title = "Save " + mLevel.LevelName
            dialog.InitialDirectory = Breakout.CustomLevelsDirectory
            dialog.CreatePrompt = False
            dialog.OverwritePrompt = True
            dialog.AddExtension = True
            dialog.ValidateNames = True
            dialog.FileName = ""



            Dim r As DialogResult = dialog.ShowDialog()

            If r = Windows.Forms.DialogResult.OK Then
                file = dialog.FileName
            Else
                Return
            End If



        End If



        ' attempt to save the file
        Try
            mLevel.SaveToXML(file)

            mActiveFilename = file

            mIsUnsaved = False
            mRequiresDraw = True


        Catch ex As Exception

            MsgBox("Level file cannot be saved!", MsgBoxStyle.Critical, "Error Saving")

        End Try



    End Sub



    ''' <summary>
    ''' Prompts the user for a file location and loads the selected level.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadLevel()

        ' if the level has been changed since the last save
        If mIsUnsaved Then

            Dim r As MsgBoxResult
            r = MsgBox("Your level has not been saved. Would you like to save before opening a level?", _
                       MsgBoxStyle.YesNoCancel Or MsgBoxStyle.Exclamation, "Open Level")

            Select Case r
                Case MsgBoxResult.Yes
                    SaveLevel()
                    Exit Select

                Case MsgBoxResult.No
                    Exit Select

                Case MsgBoxResult.Cancel
                    Return

            End Select

        End If




        ' initialize the open dialog
        Static dialog As OpenFileDialog = New OpenFileDialog()

        dialog.Filter = "Breakout Level Files (*.xml)|*.xml"
        dialog.Title = "Open Level"
        dialog.InitialDirectory = Breakout.CustomLevelsDirectory
        dialog.ValidateNames = True
        dialog.Multiselect = False
        dialog.FileName = ""



        Dim dr As DialogResult = dialog.ShowDialog()

        If dr = Windows.Forms.DialogResult.OK Then

            ' attempt to load the file
            Try
                mLevel.LoadFromXML(dialog.FileName)

                mActiveFilename = dialog.FileName

                LoadLevelToDesigner()

                mIsUnsaved = False
                mRequiresDraw = True

                ' highlight all the blocks if the user selects it
                If ShowEmptyBlocksToolStripMenuItem.Checked Then
                    HighlightEmptyBlocks()
                End If

            Catch ex As Exception

                MsgBox("Level file cannot be loaded!", MsgBoxStyle.Critical, "Error Opening")

            End Try

        End If



    End Sub


    ''' <summary>
    ''' Loads the properties and values from the designer to the level object.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadDesignerToLevel()

        mIsLevelLocked = True

        UpdateNameText()

        ChangeRowValue(0, RowSlider0.Value)
        ChangeRowValue(1, RowSlider1.Value)
        ChangeRowValue(2, RowSlider2.Value)
        ChangeRowValue(3, RowSlider3.Value)
        ChangeRowValue(4, RowSlider4.Value)
        ChangeRowValue(5, RowSlider5.Value)
        ChangeRowValue(6, RowSlider6.Value)
        ChangeRowValue(7, RowSlider7.Value)
        ChangeRowValue(8, RowSlider8.Value)
        ChangeRowValue(9, RowSlider9.Value)

        
        mLevel.BlockStyle = BlockStyleTrackbar.Value
        mLevel.BackgroundStyle = BackgroundTrackBar.Value


        mIsUnsaved = True
        mRequiresDraw = True


        mIsLevelLocked = False

    End Sub


    ''' <summary>
    ''' Loads the properties and values from the level object into the designer.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadLevelToDesigner()

        mIsLevelLocked = True

        LevelNameTextBox.Text = mLevel.LevelName
        AuthorNameTextBox.Text = mLevel.AuthorName


        RowSlider0.Value = mLevel.BlockSet.GetRowValue(0)
        RowSlider1.Value = mLevel.BlockSet.GetRowValue(1)
        RowSlider2.Value = mLevel.BlockSet.GetRowValue(2)
        RowSlider3.Value = mLevel.BlockSet.GetRowValue(3)
        RowSlider4.Value = mLevel.BlockSet.GetRowValue(4)
        RowSlider5.Value = mLevel.BlockSet.GetRowValue(5)
        RowSlider6.Value = mLevel.BlockSet.GetRowValue(6)
        RowSlider7.Value = mLevel.BlockSet.GetRowValue(7)
        RowSlider8.Value = mLevel.BlockSet.GetRowValue(8)
        RowSlider9.Value = mLevel.BlockSet.GetRowValue(9)


        BlockStyleTrackbar.Value = mLevel.BlockStyle
        BackgroundTrackBar.Value = mLevel.BackgroundStyle

        mIsUnsaved = True
        mRequiresDraw = True


        mIsLevelLocked = False

    End Sub



    ''' <summary>
    ''' Updates the level and author name to match the text boxes.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateNameText()

        If IsInputValid(LevelNameTextBox.Text, False) Then

            mLevel.LevelName = LevelNameTextBox.Text

            mIsUnsaved = True
            mRequiresDraw = True

        Else
            LevelNameTextBox.Text = mLevel.LevelName

        End If


        If IsInputValid(AuthorNameTextBox.Text, True) Then

            mLevel.AuthorName = AuthorNameTextBox.Text

            mIsUnsaved = True

        Else
            AuthorNameTextBox.Text = mLevel.AuthorName

        End If




    End Sub



    ''' <summary>
    ''' Save handler.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        SaveLevel()
    End Sub


    ''' <summary>
    ''' Save As handler.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
        SaveLevel(True)
    End Sub


    
    ''' <summary>
    ''' Open handler.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        LoadLevel()
    End Sub



    ''' <summary>
    ''' Redraws the designer when the timer ticks.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DrawTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DrawTimer.Tick

        If Not mRequiresDraw Then
            Return
        End If

        PreviewPictureBox.Image = mLevel.GeneratePreview(PreviewPictureBox.Width, PreviewPictureBox.Height)

        SolidBlocksLabel.Text = mLevel.BlockSet.NumSolidBlocks.ToString


        ' update the title bar text
        Static unsavedStr As String = ""

        If mIsUnsaved Then
            unsavedStr = "*"
        Else
            unsavedStr = ""
        End If

        Me.Text = unsavedStr + mLevel.LevelName + cTitleText


        mRequiresDraw = False

    End Sub



    ''' <summary>
    ''' Highlights empty blocks if necessary.
    ''' </summary>
    ''' <param name="blockLocation"></param>
    ''' <param name="blockId"></param>
    ''' <remarks></remarks>.
    Private Sub mLevel_BlockEmpty(ByVal blockLocation As System.Drawing.Point, ByVal blockId As Integer) Handles mLevel.BlockEmpty

        If ShowEmptyBlocksToolStripMenuItem.Checked Then
            mLevel.BlockSet.PerformActionOnBlock(Block.Action.Highlight, blockId)
            mRequiresDraw = True
        End If

    End Sub


    ''' <summary>
    ''' Unhighlights solid blocks if necessary.
    ''' </summary>
    ''' <param name="blockId"></param>
    ''' <remarks></remarks>
    Private Sub mLevel_BlockSolid(ByVal blockId As Integer) Handles mLevel.BlockSolid

        If ShowEmptyBlocksToolStripMenuItem.Checked Then
            mLevel.BlockSet.PerformActionOnBlock(Block.Action.Unhighlight, blockId)
            mRequiresDraw = True
        End If

    End Sub


    ''' <summary>
    ''' Sets all the empty blocks to either highlighted or unhighlighted based on the checked value.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ShowEmptyBlocksToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowEmptyBlocksToolStripMenuItem.Click

        If ShowEmptyBlocksToolStripMenuItem.Checked Then
            HighlightEmptyBlocks()
        Else
            UnhighlightEmptyBlocks()
        End If

    End Sub



    ''' <summary>
    ''' Highlights all empty blocks.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub HighlightEmptyBlocks()

        Static pattern As BlockSet.BlockSetPattern
        pattern = mLevel.BlockSet.GetPattern()

        For Each id In pattern.emptyBlocks
            mLevel.BlockSet.PerformActionOnBlock(Block.Action.Highlight, id)
        Next

        mRequiresDraw = True

    End Sub



    ''' <summary>
    ''' Unhighlights all empty blocks.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UnhighlightEmptyBlocks()

        Static pattern As BlockSet.BlockSetPattern
        pattern = mLevel.BlockSet.GetPattern()

        For Each id In pattern.emptyBlocks
            mLevel.BlockSet.PerformActionOnBlock(Block.Action.Unhighlight, id)
        Next

        mRequiresDraw = True

    End Sub


    ''' <summary>
    ''' Handle picture box clicks.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PreviewPictureBox_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PreviewPictureBox.MouseClick

        ' object to model the mouse click in canvas space
        Static clickObject As New GameObject(0, 0, 1, 1)


        Static xScale As Double = Breakout.CanvasWidth / PreviewPictureBox.Width
        Static yScale As Double = Breakout.CanvasHeight / PreviewPictureBox.Height


        clickObject.X = CInt(e.Location.X * xScale)
        clickObject.Y = CInt(e.Location.Y * yScale)



        If e.Button = Windows.Forms.MouseButtons.Left Then

            ' on left click empty the blocks
            mLevel.BlockSet.PerformActionIfCollides(Block.Action.ToggleEmpty, clickObject)

        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then

            ' on right click find the row clicked and select it in the designer
            Dim row As Integer = mLevel.BlockSet.GetRowCollidingWith(clickObject)

            If row < 0 Then
                Return
            End If

            Select Case row

                Case 0
                    RowSlider0.Focus()
                    Exit Select
                Case 1
                    RowSlider1.Focus()
                    Exit Select
                Case 2
                    RowSlider2.Focus()
                    Exit Select
                Case 3
                    RowSlider3.Focus()
                    Exit Select
                Case 4
                    RowSlider4.Focus()
                    Exit Select
                Case 5
                    RowSlider5.Focus()
                    Exit Select
                Case 6
                    RowSlider6.Focus()
                    Exit Select
                Case 7
                    RowSlider7.Focus()
                    Exit Select
                Case 8
                    RowSlider8.Focus()
                    Exit Select
                Case 9
                    RowSlider9.Focus()
                    Exit Select

            End Select

        End If


        mRequiresDraw = True
        mIsUnsaved = True

    End Sub



    ''' <summary>
    ''' Tests whether or not the specified string is valid text input.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsInputValid(ByVal text As String, ByVal emptyValid As Boolean) As Boolean


        If emptyValid Then

            If text = "" Then
                Return True
            End If

        ElseIf text = "" Then
            Return False

        End If


        Dim i As Integer
        For i = 0 To text.Length - 1

            If text.Chars(i) Like "[A-Z]" Or text.Chars(i) Like "[a-z]" Or text.Chars(i) Like "#" Or text.Chars(i) Like " " Then
                Continue For
            Else
                Return False
            End If

        Next


        Return True

    End Function



    ''' <summary>
    ''' Tests whether the level is a valid level and displays a message if not.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsLevelValid(Optional ByVal suppressMessage As Boolean = False) As Boolean

        If mLevel.BlockSet.NumSolidBlocks < 1 Then

            If Not suppressMessage Then
                MsgBox("Level must contain at least one block!", MsgBoxStyle.Exclamation, "Level Invalid")
            End If


            Return False

        End If


        Return True

    End Function



    ''' <summary>
    ''' Runs the level in a test game session.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PlayButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayButton.Click

        LoadDesignerToLevel()

        If IsLevelValid() Then

            Dim l As New BreakoutLevel(mLevel)

            MainForm.Focus()
            MainForm.TestLevel(l)
        End If

    End Sub

End Class

