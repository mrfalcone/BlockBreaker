' BreakoutGame
' ---
' Contains main game loop, logic, and objects for a Breakout game.
' 
' Author   : Michael Falcone
' Modified : 4/20/10


Public NotInheritable Class BreakoutGame


    ''' <summary>
    ''' Raised when the game is ready to be rendered.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ReadyToPresent()



    ''' <summary>
    ''' Raised when the game is ready to accept player input.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event BeginAcceptingInput()



    ''' <summary>
    ''' Raised when the current session is ended.
    ''' </summary>
    ''' <param name="type"></param>
    ''' <remarks></remarks>
    Public Event SessionEnded(ByVal type As SessionType)


    ''' <summary>
    ''' Raised when the paddle has changed size.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event PaddleSizeChanged()



    ''' <summary>
    ''' Raised when the player has earned a high score.
    ''' </summary>
    ''' <param name="score">the value of the score earned by the player</param>
    ''' <param name="sessionType">the type of session for which the score was earned</param>
    ''' <param name="level">the level for which the score was earned, nothing if the game session is normal</param>
    ''' <remarks></remarks>
    Public Event EarnedHighScore(ByVal score As Integer, ByVal sessionType As SessionType, ByVal level As BreakoutLevel)




    ''' <summary>
    ''' Describes the type of game session that may be played.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum SessionType
        None
        Test
        Normal
        Custom
        QuitSession
    End Enum



    ' constant values
    Private cCurtainSpeed As Double = 500



    ' private member data
    Private mIsRunning As Boolean = False

    Private mIsSessionActive As Boolean = False


    Private mSessionChangingTo As SessionType = SessionType.None


    Private mSessionEnding As SessionType = SessionType.None        ' the session that is currently ending


    Private mLevelIndexChangingTo As Integer = -1   ' must stay below 0 while not changing levels


    Private mBallResetting As Boolean = False

    Private mPaused As Boolean = False

    Private mAcceptingInput As Boolean = False


    Private mCanvasCenterX As Integer = 0
    Private mCanvasCenterY As Integer = 0


    Private mSessionType As SessionType = SessionType.None
    Private mSessionLevels() As BreakoutLevel = Nothing

    Private WithEvents mCurrentLevel As BreakoutLevel
    Private mCurrentLevelIndex As Integer = 0


    Private mCurrentHighScores As HighScoresCollection



    Private mMaxLevelIndex As Integer = 0


    Private mFramesPerSecond As Integer = 0


    Private mScoreMultiplier As Integer = 1

    Private mBallsRemaining As Integer = 0

    Private mScore As Integer = 0



    ' game objects
    Private WithEvents mPaddle As Paddle

    Private mPrimaryBall As Ball

    Private WithEvents mCurrentPowerup As Powerup
    Private mPowerups() As Powerup



    ' game text
    Private mSessionStatusText As GameText
    Private WithEvents mLevelIntroText As GameText
    Private mPointsText() As GameText




    ' objects used to display a curtain animation
    Private mCurtainImage As Bitmap

    Private mCurtainLeft As MoveableObject
    Private mCurtainRight As MoveableObject


    Private mCurtainsAnimating As Boolean = False




    ''' <summary>
    ''' Gets a value indicating if a session is active.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property IsSessionActive() As Boolean
        Get
            Return mIsSessionActive
        End Get
    End Property



    ''' <summary>
    ''' Gets the type of the currently running session.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property CurrentSession() As SessionType
        Get
            Return mSessionType
        End Get
    End Property



    ''' <summary>
    ''' Gets whether the game is currently accepting input from the player.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property AcceptingInput() As Boolean
        Get
            Return mAcceptingInput
        End Get
    End Property



    ''' <summary>
    ''' Gets the number of frames being updated per second.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property FramesPerSecond() As Integer
        Get
            Return mFramesPerSecond
        End Get
    End Property



    ''' <summary>
    ''' Gets whether the game is paused.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Paused() As Boolean
        Get
            Return mPaused
        End Get
    End Property



    ''' <summary>
    ''' Provides access to the game's paddle.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Paddle() As Paddle
        Get
            Return mPaddle
        End Get
    End Property



    ''' <summary>
    ''' Provides access to the game's primary ball.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Ball() As Ball
        Get
            Return mPrimaryBall
        End Get
    End Property



    ''' <summary>
    ''' Gets the current number of points to award for a block becoming destroyed.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCurrentBlockPoints() As Integer
        Return Breakout.PointsPerBlock * mScoreMultiplier
    End Function



    ''' <summary>
    ''' Awards the player an additional ball life.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ExtraBall()

        If IsSessionActive And mSessionType = SessionType.Normal Or mSessionType = SessionType.Custom Then

            mBallsRemaining += 1
            BreakoutDisplay.SetBallsRemaining(mBallsRemaining)

        End If

    End Sub




    ''' <summary>
    ''' Constructs a new game.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()


        mCanvasCenterX = Breakout.CanvasWidth \ 2
        mCanvasCenterY = Breakout.CanvasHeight \ 2


        ' initialize the curtain objects
        mCurtainImage = New Bitmap(mCanvasCenterX, Breakout.CanvasHeight)
        Dim g As Graphics = Graphics.FromImage(mCurtainImage)
        g.FillRectangle(Brushes.Black, 0, 0, mCurtainImage.Width, mCurtainImage.Height)

        mCurtainLeft = New MoveableObject(0, 0, mCurtainImage, cCurtainSpeed)
        mCurtainRight = New MoveableObject(mCurtainLeft.RightEdgeValue, 0, mCurtainImage, cCurtainSpeed)



        ' initialize text
        Dim f As New Font("Arial", 22)

        mSessionStatusText = New GameText(0, 0, " ", Color.White, f)
        mSessionStatusText.CenterV(mCanvasCenterY)
        mSessionStatusText.BeginSequence(0.8, True, GameText.Animation.FadeOut, GameText.Animation.FadeIn)


        mLevelIntroText = New GameText(0, 0, " ", Color.White, f)
        mLevelIntroText.MinimumSize = 22
        mLevelIntroText.MaximumSize = 30
        mLevelIntroText.CenterV(mCanvasCenterY)



        SetSessionStatus("No game session active")


        ' initialize game objects
        mPaddle = New Paddle(mCanvasCenterX, Breakout.CanvasHeight - 40, Breakout.GetPaddleImage(), _
                             Breakout.CanvasBounds, 0, True)

        mPrimaryBall = New Ball(mCanvasCenterX, mPaddle.TopEdgeValue - 80, Breakout.GetBallImage(), _
                         Breakout.CanvasBounds, Breakout.BallSpeed, MoveableObject.Direction.Down, GameObject.Edge.Bottom, True)



        ' initialize the powerups
        InitPowerups()



        ' initialize the points text, the last text object will be used for collecting powerups
        ReDim mPointsText(BlockSet.MaximumBlocks)

        Dim i As Integer
        For i = 0 To UBound(mPointsText)

            mPointsText(i) = New GameText(0, 0, "0", Color.DarkOrange, New Font("Arial", 18, FontStyle.Bold), 60)
            mPointsText(i).MinimumSize = 18
            mPointsText(i).MaximumSize = 24

            mPointsText(i).Hide()

        Next


    End Sub





    ''' <summary>
    ''' Begins running the game loop but does not start a game session.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StartRunning()

        mIsRunning = True

        EnterGameLoop()

    End Sub



    ''' <summary>
    ''' Ends the game and stops running the game loop.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StopRunning()

        mIsRunning = False

    End Sub



    ''' <summary>
    ''' Sets the additional ball array to the specified array of balls.
    ''' </summary>
    ''' <param name="balls">the array of balls to use for extra game balls</param>
    ''' <remarks></remarks>
    Public Sub SetAddedBalls(ByVal balls() As Ball)

    End Sub


    ''' <summary>
    ''' Begins a new normal game session with default levels.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BeginNormalSession()


        mSessionLevels = Breakout.GetDefaultLevels()

        mSessionChangingTo = SessionType.Normal

    End Sub



    ''' <summary>
    ''' Begins a new custom game session with the specified levels.
    ''' </summary>
    ''' <param name="customLevels">array of custom levels to be played</param>
    ''' <remarks></remarks>
    Public Sub BeginCustomSession(ByVal customLevels() As BreakoutLevel)


        mSessionLevels = customLevels

        mSessionChangingTo = SessionType.Custom

    End Sub



    ''' <summary>
    ''' Begins a new test session of the specified level. This is the method that should be used to test a level.
    ''' </summary>
    ''' <param name="testLevel">the level to test</param>
    ''' <remarks></remarks>
    Public Sub BeginTestSession(ByVal testLevel As BreakoutLevel)


        ReDim mSessionLevels(0)

        mSessionLevels(0) = testLevel

        mSessionChangingTo = SessionType.Test

    End Sub


    ''' <summary>
    ''' Ends the current game session.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StopSession()
        mSessionChangingTo = SessionType.QuitSession
    End Sub



    ''' <summary>
    ''' Sets the session status text to the specified string.
    ''' </summary>
    ''' <param name="statusMsg"></param>
    ''' <remarks></remarks>
    Private Sub SetSessionStatus(ByVal statusMsg As String)

        mSessionStatusText.Text = statusMsg
        mSessionStatusText.CenterH(mCanvasCenterX)

    End Sub



    ''' <summary>
    ''' Renders the frame inside the specified rectangle using the specified graphics object.
    ''' </summary>
    ''' <param name="g">the graphics object with which to draw the current frame</param>
    ''' <param name="rect">the rectangle in which to draw the frame</param>
    ''' <remarks></remarks>
    Public Sub RenderFrame(ByVal g As Graphics, ByVal rect As Rectangle)

        Static frame As New Bitmap(Breakout.CanvasWidth, Breakout.CanvasHeight)
        Static frameGraphics As Graphics = Graphics.FromImage(frame)


        frameGraphics.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
        frameGraphics.InterpolationMode = Drawing2D.InterpolationMode.Low
        frameGraphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighSpeed
        frameGraphics.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
        frameGraphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias



        RenderFrame(frameGraphics)

        g.DrawImageUnscaledAndClipped(frame, rect)



    End Sub


    ''' <summary>
    ''' Renders the frame using the specified graphics object.
    ''' </summary>
    ''' <param name="g">the graphics object with which to draw the current frame</param>
    ''' <remarks></remarks>
    Public Sub RenderFrame(ByVal g As Graphics)

        g.Clear(Color.Black)



        If IsSessionActive Or mCurtainsAnimating Then

            mCurrentLevel.RenderLevel(g)

            mPaddle.Draw(g)

            mPrimaryBall.Draw(g)


            If mSessionType <> SessionType.Test Then
                For Each p In mPowerups
                    p.Draw(g)
                Next
            End If



            For Each t In mPointsText
                t.Draw(g)
            Next


        End If


        mCurtainLeft.Draw(g)
        mCurtainRight.Draw(g)


        If mLevelIntroText.IsAnimating Then
            mLevelIntroText.Draw(g)


        ElseIf mCurtainsAnimating = False And mCurtainLeft.Visible Then
            ' if the curtains are closed and level intro isn't displaying then display status text
            mSessionStatusText.Draw(g)

        End If

    End Sub


    ''' <summary>
    ''' Renders the current frame onto the specified bitmap.
    ''' </summary>
    ''' <param name="image">the bitmap on which to draw the current frame</param>
    ''' <remarks></remarks>
    Public Sub RenderFrame(ByRef image As Bitmap)

        Dim g As Graphics = Graphics.FromImage(image)

        RenderFrame(g)

    End Sub



    ''' <summary>
    ''' Pause the game if a session is active.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Pause()

        If IsSessionActive Then
            mPaused = True

            SetSessionStatus("Paused")

            CloseCurtains(True)
        End If



    End Sub



    ''' <summary>
    ''' Unpause the game if a session is active.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Unpause()

        If IsSessionActive Then
            mPaused = False

            SetSessionStatus("")

            OpenCurtains()
        End If

    End Sub




    ''' <summary>
    ''' Activates the specified session type.
    ''' </summary>
    ''' <param name="type"></param>
    ''' <remarks></remarks>
    Private Sub ActivateSession(ByVal type As SessionType)

        If type = SessionType.QuitSession Then
            EndSession()
            Return
        End If

        mSessionType = type


        mMaxLevelIndex = UBound(mSessionLevels)

        mCurrentLevelIndex = 0

        ' show author for all but default levels
        If type = SessionType.Normal Then
            BreakoutDisplay.ShowAuthor = False
        Else
            BreakoutDisplay.ShowAuthor = True
        End If




        ' reset number of balls
        If type = SessionType.Normal Or type = SessionType.Custom Then
            mBallsRemaining = Breakout.StartBallCount
        Else
            mBallsRemaining = 0
        End If

        BreakoutDisplay.SetBallsRemaining(mBallsRemaining, False)



        ' reset score
        mScore = 0
        BreakoutDisplay.SetScore(0, False)

        mScoreMultiplier = 1
        BreakoutDisplay.SetScoreMultiplier(1, False)
        BreakoutDisplay.ShowScoreMultiplier = False


        BreakoutDisplay.Visible = True


        mPaused = False



        SetSessionStatus("")


        mCurrentHighScores = Breakout.GetGameHighScores()


        ' start the level
        StartLevel(mCurrentLevelIndex)


        mIsSessionActive = True

    End Sub


    ''' <summary>
    ''' Ends the current session if there is one in progress.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EndSession()

        mSessionEnding = mSessionType

        mSessionLevels = Nothing

        mSessionType = SessionType.None

        BreakoutDisplay.Visible = False


        mMaxLevelIndex = 0

        mCurrentLevelIndex = 0


        If Not Paused Then
            CloseCurtains()
        End If

        SetSessionStatus("No game session active")


        If mCurrentPowerup IsNot Nothing Then
            mCurrentPowerup.Deactivate(Me)
            mCurrentPowerup = Nothing
        End If


        mIsSessionActive = False

        mPaused = False

    End Sub




    ''' <summary>
    ''' Starts the level specified by the index.
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Private Sub StartLevel(ByVal index As Integer)

        If index < 0 Or index > mMaxLevelIndex Then
            Throw New Exception("Level index out of bounds.")
        End If

        mCurrentLevelIndex = index

        StartLevel(mSessionLevels(index))

    End Sub


    ''' <summary>
    ''' Starts the specified level.
    ''' </summary>
    ''' <param name="level"></param>
    ''' <remarks></remarks>
    Private Sub StartLevel(ByVal level As BreakoutLevel)

        mCurrentLevel = level

        mCurrentLevel.Reset()

        If mSessionType = SessionType.Custom Then
            mCurrentHighScores = mCurrentLevel.HighScores
        End If



        ' reset the points text
        For Each t In mPointsText
            t.Reset()
            t.Text = ""
            t.Hide()
        Next



        ' reinitialize powerups
        InitPowerups()

        BreakoutDisplay.ShowPowerup = False



        If mCurrentLevel.AuthorName = "" Or mSessionType = SessionType.Normal Then
            BreakoutDisplay.ShowAuthor = False

        Else
            BreakoutDisplay.SetAuthorName(mCurrentLevel.AuthorName)
            BreakoutDisplay.ShowAuthor = True

        End If



        BreakoutDisplay.SetLevelName(mCurrentLevel.LevelName)

        BreakoutDisplay.SetBlocksRemaining(mCurrentLevel.BlockSet.NumSolidBlocks, False)

        BreakoutDisplay.BlockImage = Breakout.GetBlockImage(mCurrentLevel.BlockStyle)


        mLevelIntroText.Text = "Level: " + mCurrentLevel.LevelName
        mLevelIntroText.CenterH(mCanvasCenterX)
        mLevelIntroText.BeginSequence(0.6, False, GameText.Animation.Grow, GameText.Animation.Shrink)


        mPaddle.Reset()
        mPrimaryBall.Reset()


        If mSessionType = SessionType.Custom Then
            mScore = 0
            BreakoutDisplay.SetScore(0, False)
            mBallsRemaining = Breakout.StartBallCount
            BreakoutDisplay.SetBallsRemaining(Breakout.StartBallCount)
            mScoreMultiplier = 1

            BreakoutDisplay.ShowScoreMultiplier = False
        End If


    End Sub




    ''' <summary>
    ''' Called when all the blocks are empty.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LevelComplete()

        If mCurrentLevelIndex = mMaxLevelIndex Then

            Dim status As String

            If mSessionType = SessionType.Normal Then
                status = "Game won with score: " + BreakoutDisplay.FormatScore(mScore)
            Else
                status = "No game session active"
            End If


            EndSession()

            SetSessionStatus(status)

            Return
        End If

        mLevelIndexChangingTo = mCurrentLevelIndex + 1


        CloseCurtains()

    End Sub




    ''' <summary>
    ''' Begins opening the curtains after the level intro text completes its animation.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mLevelIntroText_AnimationComplete() Handles mLevelIntroText.AnimationComplete
        OpenCurtains()
    End Sub



    ''' <summary>
    ''' Enters the main game loop that drives the breakout game.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EnterGameLoop()


        Dim watch As New Stopwatch()

        Dim elapsedFrame As Double = 0

        Dim secCounter As Double = 0
        Dim frameCounter As Integer = 0



        Do


            If mSessionChangingTo <> SessionType.None Then
                ' if the session is changing then change it

                ActivateSession(mSessionChangingTo)

                elapsedFrame = 0

                mSessionChangingTo = SessionType.None


            ElseIf mSessionEnding <> SessionType.None Then
                ' if the current session is ending then end it


                ' wait until the curtains are done closing to raise the events
                If Not mCurtainsAnimating Then
                    RaiseEvent SessionEnded(mSessionEnding)

                    Try

                        ' if the player earned a high score then raise the event
                        If mSessionEnding = SessionType.Normal AndAlso mCurrentHighScores.IsHighScore(mScore) Then
                            RaiseEvent EarnedHighScore(mScore, SessionType.Normal, Nothing)

                        ElseIf mSessionEnding = SessionType.Custom AndAlso mCurrentHighScores.IsHighScore(mScore) Then
                            RaiseEvent EarnedHighScore(mScore, SessionType.Custom, mCurrentLevel)
                        End If

                    Catch ex As Exception
                        MsgBox(ex.ToString())
                    End Try


                mSessionEnding = SessionType.None
            End If


            End If


            ' if the level is changing then change it
            If mLevelIndexChangingTo >= 0 Then


                If Not mCurtainsAnimating Then

                    If mSessionType = SessionType.Custom AndAlso mCurrentLevel.HighScores.IsHighScore(mScore) Then
                        RaiseEvent EarnedHighScore(mScore, SessionType.Custom, mCurrentLevel)
                    End If

                    StartLevel(mLevelIndexChangingTo)

                    mLevelIndexChangingTo = -1   ' make sure level index is less than 0

                End If


            End If




            watch.Start()


            ' update current session
            If IsSessionActive AndAlso mCurtainsAnimating = False AndAlso Paused = False AndAlso _
                    mLevelIntroText.IsAnimating = False AndAlso mBallResetting = False Then
                UpdateSession(elapsedFrame)

                If Not mAcceptingInput Then
                    RaiseEvent BeginAcceptingInput()
                    mAcceptingInput = True
                End If

            ElseIf mLevelIntroText.IsAnimating Then
                mAcceptingInput = False

                mLevelIntroText.Update(elapsedFrame)
                mLevelIntroText.CenterH(mCanvasCenterX)
                mLevelIntroText.CenterV(mCanvasCenterY)

            ElseIf mCurtainsAnimating AndAlso Paused = False Then
                mAcceptingInput = False

                UpdateCurtains(elapsedFrame)



            ElseIf mBallResetting Then
                ' wait for the ball for 1 second then keep playing

                mAcceptingInput = False
                mPaddle.Reset()

                Static waiting As Double = 0
                waiting += elapsedFrame

                If waiting >= 1 Then
                    waiting = 0
                    mBallResetting = False
                End If


            Else
                mAcceptingInput = False

                CloseCurtains(True)

                mSessionStatusText.Update(elapsedFrame)

            End If





            BreakoutDisplay.Update(elapsedFrame)


            RaiseEvent ReadyToPresent()



            ' perform fps calculation
            secCounter += elapsedFrame
            frameCounter += 1

            If secCounter >= 1 Then
                secCounter = 0
                mFramesPerSecond = frameCounter
                BreakoutDisplay.SetFPS(frameCounter)
                frameCounter = 0
            End If



            ' allow the application to process events
            Application.DoEvents()


            ' update elapsed time and go to next frame
            elapsedFrame = watch.Elapsed.TotalSeconds
            watch.Reset()

        Loop While mIsRunning




    End Sub



    ''' <summary>
    ''' Causes the curtains to begin opening.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OpenCurtains(Optional ByVal suppressAnimation As Boolean = False)

        If suppressAnimation Then

            mCurtainsAnimating = False

            mCurtainLeft.Visible = False
            mCurtainRight.Visible = False

            Return

        End If



        mCurtainLeft.Reset()
        mCurtainRight.Reset()

        mCurtainLeft.Visible = True
        mCurtainRight.Visible = True

        mCurtainLeft.HorizontalDirection = MoveableObject.Direction.Left
        mCurtainRight.HorizontalDirection = MoveableObject.Direction.Right

        mCurtainsAnimating = True


    End Sub



    ''' <summary>
    ''' Causes the curtains to begin closing.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CloseCurtains(Optional ByVal suppressAnimation As Boolean = False)

        If suppressAnimation Then

            mCurtainsAnimating = False

            mCurtainLeft.Visible = True
            mCurtainRight.Visible = True

            mCurtainLeft.Reset()
            mCurtainRight.Reset()

            Return

        End If


        mCurtainLeft.Visible = True
        mCurtainRight.Visible = True

        ' make sure curtains start off canvas
        mCurtainLeft.RightEdgeValue = 0
        mCurtainRight.LeftEdgeValue = Breakout.CanvasWidth

        mCurtainLeft.HorizontalDirection = MoveableObject.Direction.Right
        mCurtainRight.HorizontalDirection = MoveableObject.Direction.Left

        mCurtainsAnimating = True

    End Sub


    ''' <summary>
    ''' Updates the positions of the curtains if they are being animated.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateCurtains(ByVal elapsedTime As Double)

        If mCurtainsAnimating = False Then
            Return
        End If

        mCurtainLeft.Update(elapsedTime)
        mCurtainRight.Update(elapsedTime)


        ' if one of the curtains leaves the canvas then stop opening
        If mCurtainLeft.HorizontalDirection = MoveableObject.Direction.Left And mCurtainRight.HorizontalDirection = MoveableObject.Direction.Right Then

            If mCurtainLeft.RightEdgeValue <= 0 Or mCurtainRight.LeftEdgeValue >= Breakout.CanvasWidth Then

                mCurtainLeft.Visible = False
                mCurtainRight.Visible = False

                mCurtainLeft.HorizontalDirection = MoveableObject.Direction.None
                mCurtainRight.HorizontalDirection = MoveableObject.Direction.None

                mCurtainsAnimating = False

            End If

        End If


        ' if the curtains touch each other then stop closing
        If mCurtainLeft.HorizontalDirection = MoveableObject.Direction.Right And mCurtainRight.HorizontalDirection = MoveableObject.Direction.Left Then


            If mCurtainLeft.CollidesWith(mCurtainRight) Then

                mCurtainLeft.Reset()
                mCurtainRight.Reset()

                mCurtainLeft.HorizontalDirection = MoveableObject.Direction.None
                mCurtainRight.HorizontalDirection = MoveableObject.Direction.None

                mCurtainsAnimating = False

            End If

        End If


    End Sub



    ''' <summary>
    ''' Called when the player loses the level.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LevelLost()

        Dim status As String = "No game session active"


        If mSessionType = SessionType.Normal Then
            status = "Game over with score: " + BreakoutDisplay.FormatScore(mScore)
        ElseIf mSessionType = SessionType.Custom Then
            status = "Level lost with score: " + BreakoutDisplay.FormatScore(mScore)
        End If



        EndSession()


        SetSessionStatus(status)

    End Sub



    ''' <summary>
    ''' Updates the game session and game objects.
    ''' </summary>
    ''' <param name="elapsed"></param>
    ''' <remarks></remarks>
    Private Sub UpdateSession(ByVal elapsed As Double)

        If IsSessionActive = False Then
            Return
        End If


        mPaddle.Update(elapsed)

        mPrimaryBall.Update(elapsed)


        ' handle ball collision with paddle
        Dim padCollisionEdge As GameObject.Edge = mPaddle.GetEdgeCollidingWith(mPrimaryBall, True)

        If padCollisionEdge <> GameObject.Edge.None Then

            HandlePaddleCollision(mPrimaryBall, padCollisionEdge)

        End If


        ' handle ball out of bounds
        If mPrimaryBall.IsOutOfBounds Then
            HandleBallOutOfBounds(mPrimaryBall)
        Else
            mCurrentLevel.BlockSet.PerformActionIfCollides(Block.Action.Destroy, CType(mPrimaryBall, GameObject))
        End If



        mPaddle.HorizontalDirection = MoveableObject.Direction.None





        ' update the powerups
        If mSessionType = SessionType.Normal Or mSessionType = SessionType.Custom Then
            For Each p In mPowerups
                p.Update(elapsed)

                If p.Visible And p.CollidesWith(mPaddle) Then
                    HandlePowerupCollision(p)
                End If

            Next
        End If



        ' update the points text
        For Each t In mPointsText
            t.Update(elapsed)
        Next


    End Sub




    ''' <summary>
    ''' Handles the specified ball's collision with the paddle.
    ''' </summary>
    ''' <param name="b"></param>
    ''' <param name="edge"></param>
    ''' <remarks></remarks>
    Private Sub HandlePaddleCollision(ByRef b As Ball, ByVal edge As GameObject.Edge)


        Dim ballStep As Double = b.XStep



        If b.HorizontalDirection = MoveableObject.Direction.None And mPaddle.HorizontalDirection = MoveableObject.Direction.None Then

            Dim posFromCenter As Integer = b.CenterPoint.X - mPaddle.CenterPoint.X

            If posFromCenter <> 0 Then
                ballStep = 0.05
            End If

            If posFromCenter < 0 Then
                b.HorizontalDirection = MoveableObject.Direction.Left
            ElseIf posFromCenter > 0 Then
                b.HorizontalDirection = MoveableObject.Direction.Right
            End If



        ElseIf b.HorizontalDirection = MoveableObject.Direction.None And mPaddle.HorizontalDirection <> MoveableObject.Direction.None Then
            b.HorizontalDirection = mPaddle.HorizontalDirection
            ballStep = 0.06


        Else

            If b.SameHDirectionAs(mPaddle) And mPaddle.HorizontalDirection <> MoveableObject.Direction.None Then
                ballStep += 0.02

            ElseIf mPaddle.HorizontalDirection <> MoveableObject.Direction.None Then
                ballStep -= 0.09

                If ballStep <= 0 Then
                    b.HorizontalDirection = mPaddle.HorizontalDirection
                    ballStep = 0.08
                End If

            End If


        End If


        b.XStep = ballStep




        If edge = GameObject.Edge.Left Then
            b.RightEdgeValue = mPaddle.LeftEdgeValue
            b.XStep = Breakout.MaxBallStep
            b.BounceH(MoveableObject.Direction.Left)

        ElseIf edge = GameObject.Edge.Right Then
            b.LeftEdgeValue = mPaddle.RightEdgeValue
            b.XStep = Breakout.MaxBallStep
            b.BounceH(MoveableObject.Direction.Right)

        End If


        If b.SpeedMultiplier < Breakout.MaximumBallSpeedMultiplier Then
            b.SpeedMultiplier += 0.05
        End If


        b.BottomEdgeValue = mPaddle.TopEdgeValue
        b.BounceV(MoveableObject.Direction.Up)


    End Sub




    ''' <summary>
    ''' Called when the specified ball goes out of bounds.
    ''' </summary>
    ''' <param name="b"></param>
    ''' <remarks></remarks>
    Private Sub HandleBallOutOfBounds(ByRef b As Ball)

        If mSessionType = SessionType.Normal Or mSessionType = SessionType.Custom Then

            mBallsRemaining -= 1

            If mBallsRemaining <= 0 Then
                LevelLost()
                Return
            End If

            BreakoutDisplay.SetBallsRemaining(mBallsRemaining)

            mScoreMultiplier = 1
            BreakoutDisplay.ShowScoreMultiplier = False

        End If

        If mCurrentPowerup IsNot Nothing Then
            mCurrentPowerup.Deactivate(Me)
            mCurrentPowerup = Nothing
        End If


        mBallResetting = True

        b.Reset()

    End Sub


    ''' <summary>
    ''' Handles ball collision and awards points when a block is destroyed.
    ''' </summary>
    ''' <param name="o"></param>
    ''' <param name="edgeOfCollision"></param>
    ''' <param name="blockRect"></param>
    ''' <remarks></remarks>
    Private Sub mCurrentLevel_BlockDestroyed(ByRef o As GameObject, ByVal edgeOfCollision As GameObject.Edge, ByVal blockRect As Rectangle) Handles mCurrentLevel.BlockDestroyed

        Dim b As Ball

        ' if the object that destroyed the block was not a ball then return
        Try
            b = CType(o, Ball)
        Catch ex As Exception
            Return
        End Try


        ' handle bouncing
        If edgeOfCollision = GameObject.Edge.Bottom Then
            b.TopEdgeValue = blockRect.Bottom
            b.BounceV(MoveableObject.Direction.Down)

        ElseIf edgeOfCollision = GameObject.Edge.Top Then
            b.BottomEdgeValue = blockRect.Top
            b.BounceV(MoveableObject.Direction.Up)

        ElseIf edgeOfCollision = GameObject.Edge.Left Then
            b.RightEdgeValue = blockRect.Left
            b.BounceH(MoveableObject.Direction.Left)

        ElseIf edgeOfCollision = GameObject.Edge.Right Then
            b.LeftEdgeValue = blockRect.Right
            b.BounceH(MoveableObject.Direction.Right)

        End If




        BreakoutDisplay.SetBlocksRemaining(mCurrentLevel.BlockSet.NumSolidBlocks)


        ' if the session is not normal or custom then don't award any points
        If mSessionType <> SessionType.Normal And mSessionType <> SessionType.Custom Then
            Return
        End If


        mScore += GetCurrentBlockPoints()

        BreakoutDisplay.SetScore(mScore)

        ' increase the score multiplier if it is not at max
        If mScoreMultiplier < Breakout.ScoreMultiplierMaximum Then
            mScoreMultiplier += 1
            BreakoutDisplay.ShowScoreMultiplier = True
            BreakoutDisplay.SetScoreMultiplier(mScoreMultiplier)
        End If






    End Sub



    ''' <summary>
    ''' Shows the specified text at the specified point.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="p"></param>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Private Sub ShowPointText(ByVal text As String, ByVal p As Point, ByVal index As Integer)

        mPointsText(index).Reset()
        mPointsText(index).Text = text
        mPointsText(index).Coordinates = p
        mPointsText(index).Coordinates.Offset(-5, -10)
        mPointsText(index).VerticalDirection = MoveableObject.Direction.Up
        mPointsText(index).HorizontalDirection = MoveableObject.Direction.Left
        mPointsText(index).BeginSequence(0.5, False, GameText.Animation.Grow, GameText.Animation.FadeOut)

    End Sub



    ''' <summary>
    ''' Shows point text when a block becomes empty and randomly show powerups.
    ''' </summary>
    ''' <param name="blockId"></param>
    ''' <remarks></remarks>
    Private Sub mCurrentLevel_BlockEmpty(ByVal blockLocation As Point, ByVal blockId As Integer) Handles mCurrentLevel.BlockEmpty

        If mSessionType <> SessionType.Normal And mSessionType <> SessionType.Custom Then
            Return
        End If

        ' animate the points text
        ShowPointText(GetCurrentBlockPoints().ToString(), blockLocation, blockId)


        If Breakout.GetRandomBool(Breakout.PowerupDropRate) Then

            mPowerups(blockId).Reset()
            mPowerups(blockId).Coordinates = blockLocation
            mPowerups(blockId).VerticalDirection = MoveableObject.Direction.Down
            mPowerups(blockId).Show()
        End If


    End Sub


    ''' <summary>
    ''' Ends the level when the entire blockset is destroyed.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mCurrentLevel_BlockSetEmpty() Handles mCurrentLevel.BlockSetEmpty

        LevelComplete()

    End Sub



    ''' <summary>
    ''' Fills the powerups array with random powerups.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitPowerups()

        ReDim mPowerups(BlockSet.MaximumBlocks - 1)

        ' if a powerup is currently active then deactivate it
        If mCurrentPowerup IsNot Nothing Then
            mCurrentPowerup.Deactivate(Me)
        End If

        mCurrentPowerup = Nothing


        Dim i As Integer
        For i = 0 To UBound(mPowerups)
            mPowerups(i) = Powerup.CreateRandom()
        Next

    End Sub




    ''' <summary>
    ''' Handles paddle collision with the specified powerup.
    ''' </summary>
    ''' <param name="p"></param>
    ''' <remarks></remarks>
    Private Sub HandlePowerupCollision(ByVal p As Powerup)

        Dim points As Integer = p.PointValue * mScoreMultiplier


        ShowPointText(points.ToString(), p.Coordinates, mPointsText.Length - 1)

        mScore += points


        BreakoutDisplay.SetScore(mScore)


        If mCurrentPowerup IsNot Nothing And p.HasTimeLimit Then
            mCurrentPowerup.Deactivate(Me)
            mCurrentPowerup = Nothing
            BreakoutDisplay.ShowPowerup = False
        End If


        ' if the powerup has a time limit then set it as the active powerup so we can handle events
        If p.HasTimeLimit Then
            mCurrentPowerup = p
        End If





        ' always reset the powerup captured
        p.Reset()


        p.Activate(Me)

    End Sub



    ''' <summary>
    ''' Handles a powerup becoming active.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mCurrentPowerup_Activated() Handles mCurrentPowerup.Activated

        BreakoutDisplay.ShowPowerup = True
        BreakoutDisplay.SetPowerupImage(Breakout.GetPowerupImage(mCurrentPowerup.Type))

    End Sub



    ''' <summary>
    ''' Handles a powerup becoming deactivated.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mCurrentPowerup_Deactivated() Handles mCurrentPowerup.Deactivated
        BreakoutDisplay.ShowPowerup = False
    End Sub



    ''' <summary>
    ''' Updates the powerup timer display.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mCurrentPowerup_TimerTick() Handles mCurrentPowerup.TimerTick
        BreakoutDisplay.SetPowerupTime(CInt(mCurrentPowerup.TimeRemaining))
    End Sub



    ''' <summary>
    ''' Deactivates the powerup when necessary.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mCurrentPowerup_RequiresDeactivation() Handles mCurrentPowerup.RequiresDeactivation
        mCurrentPowerup.Deactivate(Me)
        mCurrentPowerup = Nothing
    End Sub



    ''' <summary>
    ''' Raise an event when the paddle is resized.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mPaddle_Resized() Handles mPaddle.Resized
        RaiseEvent PaddleSizeChanged()
    End Sub

End Class

' todo:
' splash screen