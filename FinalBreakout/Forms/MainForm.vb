' MainForm
' ---
' The main form for a breakout game. Handles displaying the game, resizing the window, and controlling
' the mouse and game paddle.
' 
' Author   : Michael Falcone
' Modified : 4/10/10


Public Class MainForm


    ' private member data
    Private mIsLoaded As Boolean = False

    Private WithEvents mGame As BreakoutGame


    Private mDisplayImage As Bitmap
    Private mDisplayGraphics As Graphics

    Private mIsFullscreen As Boolean = False

    Private mControllingMouse As Boolean = False

    Private mDefaultMouseClip As Rectangle


    Private mCanvasScaleH As Double = 1







    ''' <summary>
    ''' Begins a new test session using the specified level.
    ''' </summary>
    ''' <param name="level">the level that is to be tested</param>
    ''' <remarks></remarks>
    Public Sub TestLevel(ByVal level As BreakoutLevel)

        If PromptForNewGame() Then
            ControlMouse()
            mGame.BeginTestSession(level)
        End If

    End Sub

  


   


    ''' <summary>
    ''' Initializes the form, display, and game.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        System.Diagnostics.Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.RealTime
        System.Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.Highest


        ' initialize the game module
        Breakout.Initialize()


        ' resize the form to the display's minimum size and set it as the form's minimum
        Me.ClientSize = BreakoutDisplay.MinimumSize
        Me.MinimumSize = Me.Size
        Me.CenterToScreen()

        UpdateSize()


        mDefaultMouseClip = Cursor.Clip


        mGame = New BreakoutGame()


        mIsLoaded = True



        ' set the form to keep a fixed size
        Me.MaximizeBox = False
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Fixed3D


        ' position the start button in the center
        Dim centerX As Integer = Me.ClientSize.Width \ 2
        Dim buttonCenter As Integer = StartButton.Width \ 2

        StartButton.Location = New Point(centerX - buttonCenter, BreakoutDisplay.CanvasSpace.Bottom - StartButton.Height - 10)

    End Sub




    ''' <summary>
    ''' Process form key down events.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MainForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        If e.KeyCode = Keys.Escape Then

            If mGame.Paused = False AndAlso mGame.IsSessionActive = True Then

                If mGame.CurrentSession = BreakoutGame.SessionType.Test AndAlso LevelDesignerForm.Visible Then

                    Dim r As MsgBoxResult = MsgBox("End test session?", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo)

                    If r = MsgBoxResult.Yes Then
                        mGame.StopSession()
                    End If

                End If

                PauseGame()

            Else
                ExitFullscreen()
            End If

        End If


    End Sub






    ''' <summary>
    ''' Once the form is shown then begin running the game.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MainForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        mGame.StartRunning()

    End Sub



    ''' <summary>
    ''' When the form is closing then stop running the game.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        PauseGame()


        ' if the level designer is open then attempt to close it
        If LevelDesignerForm.Visible Then

            LevelDesignerForm.Focus()
            LevelDesignerForm.Close()

            ' if it stays open then cancel closing the application
            If LevelDesignerForm.Visible Then
                e.Cancel = True
                Return
            End If

        End If

        mGame.StopRunning()

    End Sub



    ''' <summary>
    ''' Handles change in form size.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MainForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        If mIsLoaded Then
            UpdateSize()
            Render()
        End If

    End Sub



    ''' <summary>
    ''' Pause if the form no longer has focus.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MainForm_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        PauseGame()
        Render()
    End Sub


    ''' <summary>
    ''' Sets the mouse cursor's location to match the game's paddle.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mGame_BeginAcceptingInput() Handles mGame.BeginAcceptingInput

        Dim padPos As Point = mGame.Paddle.Coordinates

        padPos.X = CInt(padPos.X / mCanvasScaleH)


        Cursor.Position = PointToScreen(BreakoutDisplay.PointToDisplay(padPos))

    End Sub


    ''' <summary>
    ''' Handles a game session end.
    ''' </summary>
    ''' <param name="type"></param>
    ''' <remarks></remarks>
    Private Sub mGame_SessionEnded(ByVal type As BreakoutGame.SessionType) Handles mGame.SessionEnded

        If mControllingMouse Then
            ReleaseMouse()
        End If


        If type = BreakoutGame.SessionType.Test AndAlso LevelDesignerForm.Visible Then

            ExitFullscreen()
            LevelDesignerForm.Focus()

        End If

    End Sub



    ''' <summary>
    ''' Adds a new high score entry when the player earns a high score.
    ''' </summary>
    ''' <param name="score"></param>
    ''' <param name="sessionType"></param>
    ''' <param name="level"></param>
    ''' <remarks></remarks>
    Private Sub mGame_EarnedHighScore(ByVal score As Integer, ByVal sessionType As BreakoutGame.SessionType, ByVal level As BreakoutLevel) Handles mGame.EarnedHighScore

        Static entryForm As New NewScoreEntryForm


        entryForm.Score = score


        If sessionType = BreakoutGame.SessionType.Normal Then

            entryForm.Level = Nothing
            entryForm.ShowDialog()
            HighScoresForm.ShowDialog(entryForm.EntryIndex)


        ElseIf sessionType = BreakoutGame.SessionType.Custom Then

            entryForm.Level = level
            entryForm.ShowDialog()
            HighScoresForm.ShowDialog(entryForm.EntryIndex, level)

        End If


        If mGame.Paused Then
            UnpauseGame()
        End If

    End Sub




    ''' <summary>
    ''' Updates the form for presentation to the player.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mGame_ReadyToPresent() Handles mGame.ReadyToPresent

        Render()


        ' update menu items
        If mGame.Paused Then
            PauseToolStripMenuItem.Text = "Unpause"
        Else
            PauseToolStripMenuItem.Text = "Pause"
        End If

        PauseToolStripMenuItem.Enabled = mGame.IsSessionActive

        EndSessionToolStripMenuItem.Enabled = mGame.IsSessionActive

        If mGame.IsSessionActive Then
            StartButton.Visible = False
            StartButton.Enabled = False
        Else
            StartButton.Visible = True
            StartButton.Enabled = True
        End If

    End Sub





    ''' <summary>
    ''' Renders the game and displays to picture box.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Render()

        ' only render every third frame when fullscreen to achieve higher framerate
        Static skipCount As Integer = 0

        If mIsFullscreen Then

            If skipCount < 3 Then
                skipCount += 1
                Return
            Else
                skipCount = 0
            End If

        End If

        


        BreakoutDisplay.Render(mDisplayGraphics)

        mGame.RenderFrame(mDisplayGraphics, BreakoutDisplay.CanvasSpace)

        DisplayPictureBox.Image = mDisplayImage


    End Sub



    ''' <summary>
    ''' Updates the game display's size to match the form's client size, and updates the form's objects
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateSize()

        If Me.WindowState = FormWindowState.Minimized Then
            Return
        End If


        Dim aspectRatio As Double = Me.ClientRectangle.Width / Me.ClientRectangle.Height

        BreakoutDisplay.SetAspectRatio(aspectRatio)
        BreakoutDisplay.Update()

        DisplayPictureBox.Bounds = Me.ClientRectangle


        ' update the horizontal canvas scale
        mCanvasScaleH = Breakout.CanvasWidth / BreakoutDisplay.CanvasSpace.Width


        ' update display bitmap
        mDisplayImage = New Bitmap(BreakoutDisplay.Width, BreakoutDisplay.Height)

        mDisplayGraphics = Graphics.FromImage(mDisplayImage)
        mDisplayGraphics.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
        mDisplayGraphics.InterpolationMode = Drawing2D.InterpolationMode.Low
        mDisplayGraphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighSpeed
        mDisplayGraphics.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
        mDisplayGraphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias


        Return

        BreakoutDisplay.Size = Me.ClientSize
        BreakoutDisplay.Update()


        DisplayPictureBox.Bounds = BreakoutDisplay.Bounds



        ' update the horizontal canvas scale
        mCanvasScaleH = Breakout.CanvasWidth / BreakoutDisplay.CanvasSpace.Width


        ' update display bitmap
        mDisplayImage = New Bitmap(DisplayPictureBox.Width, DisplayPictureBox.Height)

        mDisplayGraphics = Graphics.FromImage(mDisplayImage)
        mDisplayGraphics.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
        mDisplayGraphics.InterpolationMode = Drawing2D.InterpolationMode.Low
        mDisplayGraphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighSpeed
        mDisplayGraphics.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
        mDisplayGraphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias



    End Sub



    ''' <summary>
    ''' Sets the form to display fullscreen.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnterFullscreen()

        Return

        Dim screenBounds As Rectangle = System.Windows.Forms.Screen.GetBounds(Me.Location)

        Me.WindowState = FormWindowState.Normal
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.ClientSize = New Size(screenBounds.Width, screenBounds.Height)
        Me.Location = New Point(0, 0)
        Me.TopMost = True


        mIsFullscreen = True

        'ViewFullscreenToolStripMenuItem.Checked = True


        ' if the mouse is under the form's control then regain control with new size
        If mControllingMouse Then
            ControlMouse()
        End If

    End Sub



    ''' <summary>
    ''' Sets the form to be displayed in windowed mode at its minimum size.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExitFullscreen()

        Return

        If Not mIsFullscreen Then
            Return
        End If


        Me.TopMost = False
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
        Me.Size = Me.MinimumSize
        Me.CenterToScreen()

        mIsFullscreen = False

        'ViewFullscreenToolStripMenuItem.Checked = False


        ' if the mouse is under the form's control then regain control with new size
        If mControllingMouse Then
            ControlMouse()
        End If

    End Sub



    ''' <summary>
    ''' Takes control of the mouse and clips its location to stay within the canvas space.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ControlMouse()


        If Not mControllingMouse Then

            Cursor.Hide()

            mControllingMouse = True

        End If


        ClipMouseToPaddleSpace()


    End Sub


    ''' <summary>
    ''' Release control of the mouse.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ReleaseMouse()

        If mControllingMouse Then
            Cursor.Show()

            Cursor.Clip = mDefaultMouseClip

            mControllingMouse = False

        End If

    End Sub



    ''' <summary>
    ''' Prompts the user to begin a new game.
    ''' </summary>
    ''' <returns>true if user selects "Yes" or if there is no game active</returns>
    ''' <remarks></remarks>
    Private Function PromptForNewGame() As Boolean

        If mGame.IsSessionActive = False Then
            Return True
        End If

        PauseGame()

        Render()


        Dim r As MsgBoxResult = MsgBox("A game session is currently active. Would you like to begin a new one?", _
                                       MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo, "Begin New Game")


        If r = MsgBoxResult.Yes Then
            Return True
        End If


        Return False

    End Function



    ''' <summary>
    ''' Pauses the game and releases the mouse.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PauseGame()

        mGame.Pause()

        ReleaseMouse()

    End Sub


    ''' <summary>
    ''' Unpauses the game and regains control of the mouse.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UnpauseGame()

        ControlMouse()

        mGame.Unpause()

    End Sub


    ''' <summary>
    ''' Toggles fullscreen mode.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ViewFullscreenToolStripMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        'If ViewFullscreenToolStripMenuItem.Checked Then
        '    EnterFullscreen()
        'Else
        '    ExitFullscreen()
        'End If

    End Sub



    ''' <summary>
    ''' Toggle showing framerate.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ShowFramerateToolStripMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ShowFramerateToolStripMenuItem.CheckStateChanged

        BreakoutDisplay.ShowFPS = ShowFramerateToolStripMenuItem.Checked

    End Sub




    ''' <summary>
    ''' Shows the level designer form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OpenLevelDesignerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenLevelDesignerToolStripMenuItem.Click

        ExitFullscreen()
        LevelDesignerForm.Show()

    End Sub



    ''' <summary>
    ''' Shows the about form.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click

        ExitFullscreen()
        AboutForm.ShowDialog()

    End Sub



    ''' <summary>
    ''' Ends the application.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub



    ''' <summary>
    ''' Prompts the user to choose custom levels then begins a game session with those levels.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub StartCustomSessionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartCustomSessionToolStripMenuItem.Click

        Static levelDialog As New CustomLevelsForm()


        If PromptForNewGame() Then

            ExitFullscreen()

            Dim r As DialogResult = levelDialog.ShowDialog()

            If r = Windows.Forms.DialogResult.OK Then
                ControlMouse()
                mGame.BeginCustomSession(levelDialog.SelectedLevels)
            End If

        End If

    End Sub



    ''' <summary>
    ''' If another session is active then prompt user to start a new one. Then start a new 
    ''' normal game session.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub StartNewSessionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartNewSessionToolStripMenuItem.Click

        If PromptForNewGame() Then
            ControlMouse()
            mGame.BeginNormalSession()
        End If

    End Sub



    ''' <summary>
    ''' Toggle paused state.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PauseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PauseToolStripMenuItem.Click

        ' set the paused state of the game
        If mGame.Paused Then
            UnpauseGame()
        Else
            PauseGame()
        End If

    End Sub



    ''' <summary>
    ''' Ends an active session.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub EndSessionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EndSessionToolStripMenuItem.Click
        mGame.StopSession()
    End Sub


    


    ''' <summary>
    ''' Moves the game's paddle to the cursor position.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DisplayPictureBox_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DisplayPictureBox.MouseMove

        If Not mGame.AcceptingInput Then
            Return
        End If

        Static canvasX As Integer = 0

        ' calculate the position in the game canvas of the cursor

        Dim canvasPoint As Point = BreakoutDisplay.PointToCanvas(e.Location)

        canvasX = CInt(canvasPoint.X * mCanvasScaleH)


        mGame.Paddle.SetHorizontalPosition(canvasX, True)

    End Sub



    ''' <summary>
    ''' Displays the high scores dialog.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub HighScoresToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HighScoresToolStripMenuItem.Click
        ExitFullscreen()
        HighScoresForm.ShowDialog()
    End Sub

    

    ''' <summary>
    ''' Updates the mouse clip region when the size of the paddle changes.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mGame_PaddleSizeChanged() Handles mGame.PaddleSizeChanged
        ClipMouseToPaddleSpace()
    End Sub



    ''' <summary>
    ''' Sets the mouse clip region to disallow the cursor from moving the paddle out of boundaries.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClipMouseToPaddleSpace()

        Dim clip As Rectangle = BreakoutDisplay.CanvasSpace

        clip.Width -= CInt(mGame.Paddle.Width / mCanvasScaleH)


        clip.Location = PointToScreen(clip.Location)
        clip.X += 2


        Cursor.Clip = clip

    End Sub


    ''' <summary>
    ''' Starts a new normal session when the start button is clicked.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub StartButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartButton.Click
        If PromptForNewGame() Then
            ControlMouse()
            mGame.BeginNormalSession()
        End If
    End Sub

End Class

