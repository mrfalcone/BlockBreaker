' GameText
' ---
' Inherits MoveableObject to model drawable, animatable text.
' 
' Author   : Michael Falcone
' Modified : 4/03/10


Public NotInheritable Class GameText
    Inherits MoveableObject


    Public Event AnimationComplete()


    ''' <summary>
    ''' Specifies the animation performed by the text.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Animation

        ''' <summary>
        ''' Causes the text to fade into transparency.
        ''' </summary>
        ''' <remarks></remarks>
        FadeOut

        ''' <summary>
        ''' Causes the text to fade in from transparency.
        ''' </summary>
        ''' <remarks></remarks>
        FadeIn

        ''' <summary>
        ''' Causes the text to grow to the maximum font size.
        ''' </summary>
        ''' <remarks></remarks>
        Grow

        ''' <summary>
        ''' Causes the text to shrink to the minimum font size.
        ''' </summary>
        ''' <remarks></remarks>
        Shrink

    End Enum




    ' private member data
    Private mIsFadingOut As Boolean = False
    Private mIsFadingIn As Boolean = False

    Private mIsGrowing As Boolean = False
    Private mIsShrinking As Boolean = False


    Private mRepeating As Boolean = False

    Private mSizeStrength As Single = 0        ' strength of font size change per second
    Private mOpacityStrength As Integer = 0    ' strength of opacity change per second

    Private mMaxDuration As Double = 0
    Private mElapsedDuration As Double = 0


    Private mIsAnimating As Boolean = False



    Private mSequence() As Animation

    Private mAnimatingFromSequence As Boolean = False

    Private mCurSequenceIndex As Integer = 0


    Private mBrush As New SolidBrush(Color.Black)
    Private mText As String = ""
    Private mFont As Font = SystemFonts.DefaultFont


    Private mMinSize As Integer = 1
    Private mMaxSize As Integer = 40

    Private mDefaultSize As Integer = 8





    ''' <summary>
    ''' Gets or sets the minimum font size of the text.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property MinimumSize() As Integer
        Get
            Return mMinSize
        End Get
        Set(ByVal value As Integer)
            mMinSize = value
            If mMinSize < 0 Then
                mMinSize = 0
            ElseIf mMinSize > DefaultSize Then
                mMinSize = DefaultSize
            End If
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the default font size of the text but does not effect the current size.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property DefaultSize() As Integer
        Get
            Return mDefaultSize
        End Get
        Set(ByVal value As Integer)
            mDefaultSize = value
            If mDefaultSize < MinimumSize Then
                mDefaultSize = MinimumSize
            ElseIf mDefaultSize > MaximumSize Then
                mDefaultSize = MaximumSize
            End If

        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the maximum font size of the text.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property MaximumSize() As Integer
        Get
            Return mMaxSize
        End Get
        Set(ByVal value As Integer)
            mMaxSize = value
            If mMaxSize < mDefaultSize Then
                mMaxSize = mDefaultSize
            End If
        End Set
    End Property




    ''' <summary>
    ''' Gets whether the text is currently performing an animation or animation sequence.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property IsAnimating() As Boolean
        Get
            Return mIsAnimating
        End Get
        Private Set(ByVal value As Boolean)
            mIsAnimating = value
        End Set
    End Property





    ''' <summary>
    ''' Gets whether the currently-performing animation or animation sequence is repeating.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property RepeatingAnimation() As Boolean
        Get
            Return mRepeating
        End Get
        Private Set(ByVal value As Boolean)
            mRepeating = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the text displayed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Text() As String
        Get
            Return mText
        End Get
        Set(ByVal value As String)
            mText = value

            ' recalculate size in case it has changed
            CalculateSize()
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the font used to render the text.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Font() As Font
        Get
            Return mFont
        End Get
        Set(ByVal value As Font)
            mFont = value

            ' recalculate size in case it has changed
            CalculateSize()

            If Not IsAnimating Then
                DefaultSize = CInt(mFont.Size)
            End If

        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the color used to draw the text.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Color() As Color
        Get
            Return mBrush.Color
        End Get
        Set(ByVal value As Color)
            mBrush.Color = value
        End Set
    End Property





    ''' <summary>
    ''' Constructs a new text object at the specified position.
    ''' </summary>
    ''' <param name="x">x position of the text</param>
    ''' <param name="y">y position of the text</param>
    ''' <param name="text">the text to display</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal text As String)

        MyBase.New(x, y, 0)

        Me.Text = text

        Me.Visible = True
        Me.Collidable = False

    End Sub



    ''' <summary>
    ''' Constructs a new text object with the specified movement speed and movement boundaries.
    ''' </summary>
    ''' <param name="x">x position of the text</param>
    ''' <param name="y">y position of the text</param>
    ''' <param name="text">the text to display</param>
    ''' <param name="moveSpeed">the movement speed of the text when it moves</param>
    ''' <param name="moveBounds">the movement boundaries of the text</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal text As String, ByVal moveSpeed As Double, _
                   ByVal moveBounds As Rectangle)

        Me.New(x, y, text, moveSpeed)
        Me.MovementBoundaries = moveBounds

    End Sub



    ''' <summary>
    ''' Constructs a new text object with the specified color and font.
    ''' </summary>
    ''' <param name="x">x position of the text</param>
    ''' <param name="y">y position of the text</param>
    ''' <param name="text">the text to display</param>
    ''' <param name="color">the color of the text</param>
    ''' <param name="font">the font used to display the text</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal text As String, ByVal color As Color, ByVal font As Font)

        Me.New(x, y, text, color, font, 0)

    End Sub



    ''' <summary>
    ''' Constructs a new text object with the specified color, font, and movement speed.
    ''' </summary>
    ''' <param name="x">x position of the text</param>
    ''' <param name="y">y position of the text</param>
    ''' <param name="text">the text to display</param>
    ''' <param name="color">the color of the text</param>
    ''' <param name="font">the font used to display the text</param>
    ''' <param name="moveSpeed">the movement speed of the text when it moves</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal text As String, ByVal color As Color, ByVal font As Font, _
                   ByVal moveSpeed As Double)

        MyBase.New(x, y, moveSpeed)

        Me.Text = text
        Me.Font = font
        Me.Color = color

        Me.Visible = True
        Me.Collidable = False

    End Sub


    

    ''' <summary>
    ''' Constructs a new text object with the specified movement speed.
    ''' </summary>
    ''' <param name="x">x position of the text</param>
    ''' <param name="y">y position of the text</param>
    ''' <param name="text">the text to display</param>
    ''' <param name="moveSpeed">the movement speed of the text when it moves</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal text As String, ByVal moveSpeed As Double)

        MyBase.New(x, y, moveSpeed)

        Me.Text = text

        Me.Visible = True
        Me.Collidable = False

    End Sub



    ''' <summary>
    ''' Constructs a new text object with the specified color, font, movement speed, and movement boundaries.
    ''' </summary>
    ''' <param name="x">x position of the text</param>
    ''' <param name="y">y position of the text</param>
    ''' <param name="text">the text to display</param>
    ''' <param name="color">the color of the text</param>
    ''' <param name="font">the font used to display the text</param>
    ''' <param name="moveSpeed">the movement speed of the text when it moves</param>
    ''' <param name="moveBounds">the movement boundaries of the text</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal text As String, ByVal color As Color, ByVal font As Font, _
                       ByVal moveSpeed As Double, ByVal moveBounds As Rectangle)

        MyBase.New(x, y, moveBounds, moveSpeed)

        Me.Text = text
        Me.Font = font
        Me.Color = color

        Me.Visible = True
        Me.Collidable = False

    End Sub





    ''' <summary>
    ''' Shadows the draw method to render text instead of an image.
    ''' </summary>
    ''' <param name="g"></param>
    ''' <remarks></remarks>
    Public Shadows Sub Draw(ByVal g As Graphics)


        ' do not proceed if text is not visible
        If Not Visible Then
            Return
        End If


        g.DrawString(Me.Text, Me.Font, mBrush, Me.Coordinates)


        If BorderVisible Then
            g.DrawRectangle(BorderPen, BoundRectangle)
        End If


    End Sub





    ''' <summary>
    ''' Causes the text to begin the specified animation if it is not currently animating.
    ''' </summary>
    ''' <param name="duration">the length of time in seconds over which the animation will take place</param>
    ''' <param name="animation">the animation that should be performed</param>
    ''' <remarks></remarks>
    Public Sub BeginAnimation(ByVal duration As Double, ByVal animation As Animation)

        If IsAnimating Then
            Return
        End If


        RepeatingAnimation = False
        StartAnimation(duration, animation)

    End Sub




    ''' <summary>
    ''' Causes the text to begin the specified animation(s) at once if it is not currently animating.
    ''' </summary>
    ''' <param name="duration">the length of time in seconds over which the animation(s) will take place</param>
    ''' <param name="repeat">set true to indicate that the animation(s) will be repeated until explicitly stopped</param>
    ''' <param name="animations">the animation(s) that should be performede</param>
    ''' <remarks></remarks>
    Public Sub BeginAnimation(ByVal duration As Double, ByVal repeat As Boolean, ByVal ParamArray animations() As Animation)

        If IsAnimating Then
            Return
        End If


        RepeatingAnimation = repeat

        For Each anim In animations
            StartAnimation(duration, anim)
        Next


    End Sub




    ''' <summary>
    ''' Causes the text to begin performing the specified sequence of animations if it is not currently animating.
    ''' </summary>
    ''' <param name="duration">the length of time in seconds over which each animation will take place</param>
    ''' <param name="repeat">set true to indicate that the animations will be repeated in sequence until explicitly stopped</param>
    ''' <param name="animSequence">the sequence of animations that should be performed</param>
    ''' <remarks></remarks>
    Public Sub BeginSequence(ByVal duration As Double, ByVal repeat As Boolean, ByVal ParamArray animSequence() As Animation)

        If IsAnimating Then
            Return
        End If


        mSequence = animSequence
        mAnimatingFromSequence = True
        mCurSequenceIndex = 0


        RepeatingAnimation = repeat


        StartAnimation(duration, mSequence(mCurSequenceIndex))

    End Sub



    ''' <summary>
    ''' Stops animating, resets the text's size position to default, and makes it visible.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub Reset()

        ' reset the font size to default
        Dim f As Font = New Font(Me.Font.FontFamily, DefaultSize, Me.Font.Style)
        Me.Font = f


        StopAnimating()


        MyBase.Reset()

        Show()





        ' reset the color opacity to 100%
        Dim c As Color = Color.FromArgb(255, Me.Color.R, Me.Color.G, Me.Color.B)
        Me.Color = c

    End Sub



    ''' <summary>
    ''' Updates the text's animation and position.
    ''' </summary>
    ''' <param name="secsSinceLastFrame">the fractions of a second since the last call to this method</param>
    ''' <remarks></remarks>
    Public Overrides Sub Update(ByVal secsSinceLastFrame As Double)


        ' if not animating then try moving and then return
        If Not IsAnimating Then
            MyBase.Update(secsSinceLastFrame)
            Return
        End If



        ' update how much time has elapsed since the animation started
        mElapsedDuration += secsSinceLastFrame




        ' if we haven't yet reached the max duration of the current animation
        If mElapsedDuration < mMaxDuration Then


            ' grow or shrink
            If mIsGrowing Then

                Dim fontSize As Single = Me.Font.Size + CSng(mSizeStrength * secsSinceLastFrame)

                ' make sure the font size stays within bounds
                If fontSize > MaximumSize Then
                    fontSize = MaximumSize
                End If

                Dim f As Font = New Font(Me.Font.FontFamily, fontSize, Me.Font.Style)
                Me.Font = f


            ElseIf mIsShrinking Then

                Dim fontSize As Single = Me.Font.Size - CSng(mSizeStrength * secsSinceLastFrame)

                ' make sure the font size stays within bounds
                If fontSize < MinimumSize Then
                    fontSize = MinimumSize
                End If

                Dim f As Font = New Font(Me.Font.FontFamily, fontSize, Me.Font.Style)
                Me.Font = f

            End If



            ' fade in or out
            If mIsFadingOut Then

                Dim alpha As Integer = Me.Color.A - CInt(mOpacityStrength * secsSinceLastFrame)

                If alpha < 0 Then
                    alpha = 0
                End If

                Dim c As Color = Color.FromArgb(alpha, Me.Color.R, Me.Color.G, Me.Color.B)
                Me.Color = c

            ElseIf mIsFadingIn Then

                Dim alpha As Integer = Me.Color.A + CInt(mOpacityStrength * secsSinceLastFrame)

                If alpha > 255 Then
                    alpha = 255
                End If

                Dim c As Color = Color.FromArgb(alpha, Me.Color.R, Me.Color.G, Me.Color.B)
                Me.Color = c

            End If




        Else
            ' if we have reached the duration of the current animation

            If mIsFadingIn Then

                ' set opacity to 100%
                Dim c As Color = Color.FromArgb(255, Me.Color.R, Me.Color.G, Me.Color.B)
                Me.Color = c

            ElseIf mIsFadingOut Then

                ' rather than setting opacity to 0%, just hide the text
                Hide()

            End If



            ' activate the next animation in the sequence if animating from a sequence
            If mAnimatingFromSequence Then

                StopAnimating()

                StartNextInSequence()

            Else

                ' if not animating from a sequence

                If mRepeating Then
                    ' if repeating then just reset the elapsed time and keep animating
                    mElapsedDuration = 0

                Else
                    ' if not repeating then totally stop
                    StopAnimating()

                    ' let the application know animation is finished
                    RaiseEvent AnimationComplete()

                End If


            End If


        End If


        ' move if needed
        MyBase.Update(secsSinceLastFrame)

    End Sub



    ''' <summary>
    ''' Forces the animation to stop immediately.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StopAnimating()

        IsAnimating = False

        mIsGrowing = False
        mIsShrinking = False
        mIsFadingIn = False
        mIsFadingOut = False

    End Sub



    ''' <summary>
    ''' Causes the text to stop repeating once it finishes its current animation or animation sequence.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub StopRepeating()
        RepeatingAnimation = False
    End Sub




    ''' <summary>
    ''' Activates the next animation in the animation sequence if it exists.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub StartNextInSequence()

        mCurSequenceIndex += 1

        If mCurSequenceIndex > UBound(mSequence) Then

            'if we were already at the last animation in the sequence and we are not repeating it then this is the end
            If Not RepeatingAnimation Then
                mAnimatingFromSequence = False
                StopAnimating()

                ' let the application know animation is finished
                RaiseEvent AnimationComplete()

            Else

                ' if repeating then just restart the sequence
                mCurSequenceIndex = 0
                StartAnimation(mMaxDuration, mSequence(mCurSequenceIndex))

            End If



        Else

            ' if the new index is in bounds then start the animation
            StartAnimation(mMaxDuration, mSequence(mCurSequenceIndex))

        End If

    End Sub




    ''' <summary>
    ''' Sets the animation to be started.
    ''' </summary>
    ''' <param name="duration"></param>
    ''' <param name="anim"></param>
    ''' <remarks></remarks>
    Private Sub StartAnimation(ByVal duration As Double, ByVal anim As Animation)

        Show()  ' make sure the text is visible

        IsAnimating = True

        mElapsedDuration = 0


        mMaxDuration = duration



        ' calculate required change in opacity
        If anim = Animation.FadeIn Or anim = Animation.FadeOut Then
            mOpacityStrength = CInt(255 / duration)
        End If



        Select Case anim

            Case GameText.Animation.Grow
                mIsGrowing = True
                mIsShrinking = False

                ' set the font to the minimum size before growing
                Dim f As Font = New Font(Me.Font.FontFamily, MinimumSize, Me.Font.Style)
                Me.Font = f

                ' calculate strength of growth
                Dim growth As Single = MaximumSize - Me.Font.Size
                mSizeStrength = growth / CSng(duration)

                Exit Select

            Case GameText.Animation.Shrink
                mIsShrinking = True
                mIsGrowing = False

                ' set the font to the maximum size before shrinking
                Dim f As Font = New Font(Me.Font.FontFamily, MaximumSize, Me.Font.Style)
                Me.Font = f

                ' calculate strength of shrinkage
                Dim shrinkage As Single = Me.Font.Size - MinimumSize
                mSizeStrength = shrinkage / CSng(duration)

                Exit Select

            Case GameText.Animation.FadeOut
                mIsFadingOut = True
                mIsFadingIn = False
                Exit Select

            Case Animation.FadeIn
                mIsFadingIn = True
                mIsFadingOut = False
                Exit Select


            Case Else
                IsAnimating = False

        End Select



    End Sub



    ''' <summary>
    ''' Calculates the width and height of the object.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CalculateSize()

        Static s As Size
        s = TextRenderer.MeasureText(Text, Me.Font)

        Height = s.Height
        Width = s.Width

    End Sub



End Class