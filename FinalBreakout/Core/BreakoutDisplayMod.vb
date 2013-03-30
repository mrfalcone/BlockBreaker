' BreakoutDisplay
' ---
' Provides methods and properties for a resizable, animated breakout display.
' 
' Author   : Michael Falcone
' Modified : 4/20/10


Module BreakoutDisplay


    ' constant data
    Private Const cTopOffset As Integer = 30
    Private Const cBottomOffset As Integer = 20

    Private Const cPadding As Integer = 14

    Private Const cAnimatedTextMinimum As Integer = 14
    Private Const cAnimatedTextMaximum As Integer = 20

    Private Const cDefaultWidth As Integer = 900
    Private Const cDefaultHeight As Integer = 720


    ' private member data
    Private mFontFamily As New FontFamily("Comic Sans MS")

    Private mBounds As New Rectangle(0, 0, cDefaultWidth, cDefaultHeight)


    Private mCanvasSpace As Rectangle


    Private mBallPicture As GameObject = New GameObject(0, 0, My.Resources.ball_gray_large)

    Private mBackBrush As New TextureBrush(My.Resources.displayback)


    Private mPowerupPicture As GameObject = New GameObject(0, 0, 30, 30)


    Private mBlockPicture As GameObject = New GameObject(0, 0, 64, 24)

    Private mBlockImage As Bitmap = Nothing



    Private mSizeChanged As Boolean = True

    Private mShowAuthor As Boolean = False

    Private mVisible As Boolean = False

    Private mShowMultiplier As Boolean = False

    Private mShowFPS As Boolean = False

    Private mShowCanvasBorder As Boolean = False


    Private mCenterX As Integer = 0     ' stores the x position of the center of the display



    Private mShowPowerup As Boolean = False


    Private mHeaderHeight As Integer = 0
    Private mFooterHeight As Integer = 0



    ' variables for updating score
    Private mScoreValue As Integer = 0
    Private mCurrentScoreValue As Integer = 0
    Private mScorePointStep As Integer = 15


    ' text elements
    Private mLevelLabelText As New GameText(0, 0, "Level:", Color.Black, New Font(mFontFamily, 20, FontStyle.Bold))
    Private mLevelText As New GameText(0, 0, "0", Color.Black, New Font(mFontFamily, 20))

    Private mAuthorText As New GameText(0, 0, "By:", Color.Black, New Font(mFontFamily, 12))


    Private mScoreLabelText As New GameText(0, 0, "Score:", Color.Black, New Font(mFontFamily, 20, FontStyle.Bold))
    Private mScoreText As New GameText(0, 0, "0", Color.Black, New Font(mFontFamily, 20))

    Private mScoreMultiplierText As New GameText(0, 0, "Gaining Points x 1", Color.Black, New Font(mFontFamily, cAnimatedTextMinimum))


    Private mBallsRemainingText As New GameText(0, 0, "x 0", Color.Black, New Font(mFontFamily, cAnimatedTextMinimum))
    Private mBlocksRemainingText As New GameText(0, 0, "x 0", Color.Black, New Font(mFontFamily, cAnimatedTextMinimum))

    Private mFPSText As New GameText(0, 0, "FPS: 00", Color.Black, New Font(mFontFamily, 12))


    Private mPowerupLabelText As New GameText(0, 0, "Powerup:", Color.Black, New Font(mFontFamily, 18, FontStyle.Bold))

    Private mPowerupTimerText As New GameText(0, 0, "0", Color.Black, New Font(mFontFamily, 14))




    ''' <summary>
    ''' Gets or sets a property indicating whether the display elements are displayed. If false,
    ''' only level and score labels will be displayed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Visible() As Boolean
        Get
            Return mVisible
        End Get
        Set(ByVal value As Boolean)
            mVisible = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the width of the display.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Width() As Integer
        Get
            Return mBounds.Width
        End Get
        Set(ByVal value As Integer)
            mBounds.Width = value
            mSizeChanged = True
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the height of the display.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Height() As Integer
        Get
            Return mBounds.Height
        End Get
        Set(ByVal value As Integer)
            mBounds.Height = value
            mSizeChanged = True
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the size of the display.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Size() As Size
        Get
            Return mBounds.Size
        End Get
        Set(ByVal value As Size)
            mBounds.Size = value
            mSizeChanged = True
        End Set
    End Property



    ''' <summary>
    ''' Gets a rectangle describing the size and position of the game canvas.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property CanvasSpace() As Rectangle
        Get
            Return mCanvasSpace
        End Get
    End Property



    ''' <summary>
    ''' Returns a rectangle describing the size and location of the display.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Bounds() As Rectangle
        Get
            Return mBounds
        End Get
    End Property



    ''' <summary>
    ''' Gets or sets a value indicating whether to draw a border around the canvas space.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property ShowCanvasSpaceBorder() As Boolean
        Get
            Return mShowCanvasBorder
        End Get
        Set(ByVal value As Boolean)
            mShowCanvasBorder = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets a value indicating whether or not the level author will be shown.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property ShowAuthor() As Boolean
        Get
            Return mShowAuthor
        End Get
        Set(ByVal value As Boolean)
            mShowAuthor = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets a value indicating whether to show an active powerup.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property ShowPowerup() As Boolean
        Get
            Return mShowPowerup
        End Get
        Set(ByVal value As Boolean)
            mShowPowerup = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets a value indicating whether to show the FPS.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property ShowFPS() As Boolean
        Get
            Return mShowFPS
        End Get
        Set(ByVal value As Boolean)
            mShowFPS = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets a value indicating whether to show the score multiplier text.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property ShowScoreMultiplier() As Boolean
        Get
            Return mShowMultiplier
        End Get
        Set(ByVal value As Boolean)
            mShowMultiplier = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the image used to render the block picture.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property BlockImage() As Bitmap
        Get
            Return mBlockImage
        End Get
        Set(ByVal value As Bitmap)
            mBlockImage = value
            mBlockPicture.Visible = True
        End Set
    End Property



    ''' <summary>
    ''' Gets the minimum size the display may be.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property MinimumSize() As Size
        Get
            Return CalculateMinimumSize()
        End Get
    End Property




    ''' <summary>
    ''' Determines the minimum size of the display based on the size of the game canvas.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CalculateMinimumSize() As Size

        Static s As Size = New Size(0, 0)


        If s.Width = 0 Or s.Height = 0 Then

            Update()

            s.Height = mHeaderHeight + Breakout.CanvasHeight + mFooterHeight
            s.Width = Breakout.CanvasWidth + (cPadding * 2)

        End If


        Return s

    End Function



    ''' <summary>
    ''' Sets the aspect ratio of the display to the specified value.
    ''' </summary>
    ''' <param name="ratio">ratio of width over height to set the display to</param>
    ''' <remarks></remarks>
    Public Sub SetAspectRatio(ByVal ratio As Double)


        Dim newHeight As Integer = MinimumSize.Height
        Dim newWidth As Integer = CInt(newheight * ratio)



        If newWidth < MinimumSize.Width Then

            newWidth = MinimumSize.Width
            newHeight = CInt(newWidth / ratio)

        End If


        Width = newWidth
        Height = newHeight

    End Sub



    ''' <summary>
    ''' Sets the specified number of points to be displayed as the score.
    ''' </summary>
    ''' <param name="pointAmount">the score to show</param>
    ''' <param name="countTo">set true to indicate that the point value will be counted to</param>
    ''' <remarks></remarks>
    Public Sub SetScore(ByVal pointAmount As Integer, Optional ByVal countTo As Boolean = True)


        If countTo Then
            mScoreValue = pointAmount

            If mScoreValue > mCurrentScoreValue Then
                mScorePointStep = Math.Abs(mScorePointStep)
            Else
                mScorePointStep = mScorePointStep * -1
            End If


        Else
            mScoreValue = pointAmount
            mCurrentScoreValue = pointAmount

            mScoreText.Text = pointAmount.ToString()

        End If

    End Sub




    ''' <summary>
    ''' Sets the score multiplier to the specified value.
    ''' </summary>
    ''' <param name="multiplier">the value of the score multiplier</param>
    ''' <param name="animate">set true to indicate that an animation will be performed when set</param>
    ''' <remarks></remarks>
    Public Sub SetScoreMultiplier(ByVal multiplier As Integer, Optional ByVal animate As Boolean = True)

        mScoreMultiplierText.Text = "Gaining Points x " + multiplier.ToString()

        If animate Then
            AnimateText(mScoreMultiplierText)
        End If

    End Sub



    ''' <summary>
    ''' Sets the number of balls remaining to the specified value.
    ''' </summary>
    ''' <param name="remaining">the number of balls remaining</param>
    ''' <param name="animate">set true to indicate that an animation will be performed when set</param>
    ''' <remarks></remarks>
    Public Sub SetBallsRemaining(ByVal remaining As Integer, Optional ByVal animate As Boolean = True)

        mBallsRemainingText.Text = "x " + remaining.ToString()

        If animate Then
            AnimateText(mBallsRemainingText)
        End If

    End Sub



    ''' <summary>
    ''' Sets the number of blocks remaining to the specified value.
    ''' </summary>
    ''' <param name="remaining">the number of blocks remaining</param>
    ''' <param name="animate">set true to indicate that an animation will be performed when set</param>
    ''' <remarks></remarks>
    Public Sub SetBlocksRemaining(ByVal remaining As Integer, Optional ByVal animate As Boolean = False)

        mBlocksRemainingText.Text = "x " + remaining.ToString()

        If animate Then
            AnimateText(mBlocksRemainingText)
        End If

    End Sub



    ''' <summary>
    ''' Sets the image to be displayed for the active powerup.
    ''' </summary>
    ''' <param name="image">the image of the powerup</param>
    ''' <remarks></remarks>
    Public Sub SetPowerupImage(ByVal image As Bitmap)

        mPowerupPicture.Texture = image

    End Sub



    ''' <summary>
    ''' Sets the number of seconds remaining for the powerup to the specified number.
    ''' </summary>
    ''' <param name="secs">the number of seconds remaining for the active powerup</param>
    ''' <remarks></remarks>
    Public Sub SetPowerupTime(ByVal secs As Integer)

        mPowerupTimerText.Text = secs.ToString()

        mPowerupTimerText.MaximumSize = 18
        mPowerupTimerText.MinimumSize = 14
        mPowerupTimerText.BeginAnimation(0.4, GameText.Animation.Shrink)

    End Sub



    ''' <summary>
    ''' Sets the name of the level to the specified string.
    ''' </summary>
    ''' <param name="levelName">the name of the level</param>
    ''' <param name="animate">set true to indicate that an animation will be performed when set</param>
    ''' <remarks></remarks>
    Public Sub SetLevelName(ByVal levelName As String, Optional ByVal animate As Boolean = False)

        mLevelText.Text = levelName

        If animate Then
            AnimateText(mLevelText)
        End If

    End Sub



    ''' <summary>
    ''' Sets the name of the level's author to the specified string.
    ''' </summary>
    ''' <param name="authorName">the name of the author</param>
    ''' <param name="animate">set true to indicate that an animation will be performed when set</param>
    ''' <remarks></remarks>
    Public Sub SetAuthorName(ByVal authorName As String, Optional ByVal animate As Boolean = False)

        mAuthorText.Text = "By: " + authorName

        If animate Then
            AnimateText(mAuthorText)
        End If

    End Sub



    ''' <summary>
    ''' Sets the framerate of the breakout game.
    ''' </summary>
    ''' <param name="fps">frames per second value</param>
    ''' <remarks></remarks>
    Public Sub SetFPS(ByVal fps As Integer)

        mFPSText.Text = "FPS: " + fps.ToString()

    End Sub



    ''' <summary>
    ''' Creates a formatted string representation of the specified score.
    ''' </summary>
    ''' <param name="score">the score from which to create the string</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FormatScore(ByVal score As Integer) As String

        Static nfi As System.Globalization.NumberFormatInfo = New System.Globalization.CultureInfo("en-US", False).NumberFormat
        nfi.NumberDecimalDigits = 0


        Dim s As String

        s = score.ToString("N", nfi)

        Return s

    End Function




    ''' <summary>
    ''' Updates the display. Must be called when resized.
    ''' </summary>
    ''' <param name="secsSinceLastFrame">the fractions of a second since the last call to this method</param>
    ''' <remarks></remarks>
    Public Sub Update(Optional ByVal secsSinceLastFrame As Double = 0)


        ' update to adapt to resize if necessary
        If mSizeChanged Then

            mCenterX = Width \ 2


            UpdateCanvasSpace()
            UpdatePositions()



            mSizeChanged = False


        End If


        ' if no time has passed since the last update then we can skip the rest of the method
        If secsSinceLastFrame = 0 Then
            Return
        End If




        ' count towards the score
        If mCurrentScoreValue < mScoreValue Then
            mCurrentScoreValue += mScorePointStep
            mScoreText.Text = FormatScore(mCurrentScoreValue)
        End If

        If mCurrentScoreValue > mScoreValue Then
            mCurrentScoreValue = mScoreValue
            mScoreText.Text = FormatScore(mCurrentScoreValue)
        End If



        ' animate the point multiplier
        If mScoreMultiplierText.IsAnimating Then

            mScoreMultiplierText.Update(secsSinceLastFrame)
            mScoreMultiplierText.CenterH(mCenterX)
        End If



        mBlocksRemainingText.Update(secsSinceLastFrame)
        mBallsRemainingText.Update(secsSinceLastFrame)


        If ShowPowerup Then
            mPowerupTimerText.Update(secsSinceLastFrame)
        End If


    End Sub



    ''' <summary>
    ''' Renders the current display using the specified graphics object.
    ''' </summary>
    ''' <param name="g">the graphics object with which to draw the display</param>
    ''' <remarks></remarks>
    Public Sub Render(ByVal g As Graphics)

        'g.Clear(Color.SlateGray)
        g.FillRectangle(mBackBrush, Bounds)


        ' always display the level and score labels so the user knows where they are
        mLevelLabelText.Draw(g)
        mScoreLabelText.Draw(g)


        If ShowCanvasSpaceBorder Then
            g.DrawRectangle(Pens.Black, CanvasSpace)
        End If


        If ShowFPS Then
            mFPSText.Draw(g)
        End If


        If Not Visible Then
            Return
        End If



        mLevelText.Draw(g)

        If ShowAuthor Then
            mAuthorText.Draw(g)
        End If

        mScoreText.Draw(g)

        If ShowScoreMultiplier Then
            mScoreMultiplierText.Draw(g)
        End If


        If ShowPowerup Then
            mPowerupLabelText.Draw(g)
            mPowerupPicture.Draw(g)
            mPowerupTimerText.Draw(g)
            mPowerupTimerText.CenterH(mPowerupPicture.CenterPoint.X)
        End If


        mBallsRemainingText.Draw(g)
        mBlocksRemainingText.Draw(g)

        mBallPicture.Draw(g)
        mBlockPicture.Draw(mBlockImage, g)


    End Sub


    ''' <summary>
    ''' Renders the current display onto the specified bitmap.
    ''' </summary>
    ''' <param name="image">the bitmap on which to draw the display</param>
    ''' <remarks></remarks>
    Public Sub Render(ByRef image As Bitmap)

        Dim g As Graphics = Graphics.FromImage(image)

        Render(g)

    End Sub


    ''' <summary>
    ''' Computes the location of the specified point on the display into canvas coordinates.
    ''' </summary>
    ''' <param name="displayPoint">point on the display</param>
    ''' <returns>the point on the canvas</returns>
    ''' <remarks></remarks>
    Public Function PointToCanvas(ByVal displayPoint As Point) As Point

        Dim p As New Point

        p.X = displayPoint.X - mCanvasSpace.X
        p.Y = displayPoint.Y - mCanvasSpace.Y

        Return p

    End Function



    ''' <summary>
    ''' Computes the location of the specified point on the canvas into display coordinates.
    ''' </summary>
    ''' <param name="canvasPoint">point on the canvas</param>
    ''' <returns>the point on the display</returns>
    ''' <remarks></remarks>
    Public Function PointToDisplay(ByVal canvasPoint As Point) As Point

        Dim p As New Point

        p.X = canvasPoint.X + mCanvasSpace.X
        p.Y = canvasPoint.Y + mCanvasSpace.Y

        Return p

    End Function



    ''' <summary>
    ''' Sets the specified text to begin animation.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <remarks></remarks>
    Private Sub AnimateText(ByRef text As GameText)

        text.MinimumSize = cAnimatedTextMinimum
        text.MaximumSize = cAnimatedTextMaximum
        text.BeginSequence(0.3, False, GameText.Animation.Grow, GameText.Animation.Shrink)

    End Sub



    ''' <summary>
    ''' Sets the positions of each of the elements of the display.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdatePositions()


        ' update header positions

        mLevelLabelText.LeftEdgeValue = cPadding
        mLevelLabelText.TopEdgeValue = cTopOffset

        mLevelText.LeftEdgeValue = mLevelLabelText.RightEdgeValue
        mLevelText.TopEdgeValue = cTopOffset

        mAuthorText.LeftEdgeValue = mLevelText.LeftEdgeValue
        mAuthorText.TopEdgeValue = mLevelText.BottomEdgeValue

        mScoreLabelText.LeftEdgeValue = cPadding
        mScoreLabelText.TopEdgeValue = mAuthorText.BottomEdgeValue + cPadding

        mScoreText.LeftEdgeValue = mLevelText.LeftEdgeValue
        mScoreText.TopEdgeValue = mScoreLabelText.TopEdgeValue


        mFPSText.TopEdgeValue = mScoreLabelText.BottomEdgeValue
        mFPSText.CenterH(mCenterX)

        mHeaderHeight = mFPSText.BottomEdgeValue


        mPowerupLabelText.RightEdgeValue = Width - cPadding
        mPowerupLabelText.TopEdgeValue = cTopOffset

        mPowerupPicture.CenterH(mPowerupLabelText.CenterPoint.X)
        mPowerupPicture.TopEdgeValue = mPowerupLabelText.BottomEdgeValue

        mPowerupTimerText.TopEdgeValue = mPowerupPicture.BottomEdgeValue
        mPowerupTimerText.LeftEdgeValue = mPowerupPicture.LeftEdgeValue



        ' update canvas position
        mCanvasSpace.X = mCenterX - (mCanvasSpace.Width \ 2)
        mCanvasSpace.Y = mHeaderHeight



        ' update footer positions

        mBallPicture.LeftEdgeValue = cPadding
        mBallPicture.TopEdgeValue = mCanvasSpace.Bottom + cPadding

        mBallsRemainingText.TopEdgeValue = mBallPicture.TopEdgeValue
        mBallsRemainingText.LeftEdgeValue = mBallPicture.RightEdgeValue


        mBlocksRemainingText.RightEdgeValue = Width - cPadding
        mBlocksRemainingText.TopEdgeValue = mBallsRemainingText.TopEdgeValue

        mBlockPicture.RightEdgeValue = mBlocksRemainingText.LeftEdgeValue
        mBlockPicture.TopEdgeValue = mBlocksRemainingText.TopEdgeValue


        mScoreMultiplierText.TopEdgeValue = mBallPicture.TopEdgeValue
        mScoreMultiplierText.CenterH(mCenterX)


        mFooterHeight = cPadding + mBallPicture.Height + cBottomOffset


    End Sub



    ''' <summary>
    ''' Updates the size and position of the canvas to fit properly within the window.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateCanvasSpace()

        ' always keep the same ratio
        Static sizeRatio As Double = Breakout.CanvasHeight / Breakout.CanvasWidth



        ' new size candidates
        Dim widthCandidate As Integer = Width - (cPadding * 2)
        Dim heightCandidate As Integer = Height - mHeaderHeight - mFooterHeight


        Dim newWidth As Integer
        Dim newHeight As Integer



        If widthCandidate < Breakout.CanvasWidth Or heightCandidate < Breakout.CanvasHeight Then

            newWidth = Breakout.CanvasWidth
            newHeight = Breakout.CanvasHeight

        Else

            newWidth = widthCandidate
            newHeight = CInt(widthCandidate * sizeRatio)


            If newHeight + mFooterHeight + mHeaderHeight > Height Then

                newHeight = heightCandidate
                newWidth = CInt(heightCandidate / sizeRatio)

            End If

        End If


        mCanvasSpace.Width = newWidth
        mCanvasSpace.Height = newHeight

    End Sub



End Module
