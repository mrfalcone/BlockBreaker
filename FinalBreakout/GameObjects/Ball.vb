' Ball
' ---
' Inherits MoveableObject to model a bounded ball that can move along a line and bounce.
' 
' Author   : Michael Falcone
' Modified : 3/27/10


Public NotInheritable Class Ball
    Inherits MoveableObject


    Public Event OutOfBounds()



    Private mInitialDirection As Direction = Direction.None

    Private mOutOfBounds As Boolean = False

    Private mOOBEdge As Edge = Edge.None


    Private mXStep As Double = 1
    Private mYStep As Double = 1




    ''' <summary>
    ''' Gets a value indicating whether this ball is out of bounds.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property IsOutOfBounds() As Boolean
        Get
            Return mOutOfBounds
        End Get
    End Property


    ''' <summary>
    ''' Gets or sets the edge that the ball is allowed to go through, causing it to be out of bounds.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property EdgeOOB() As Edge
        Get
            Return mOOBEdge
        End Get
        Set(ByVal value As Edge)
            mOOBEdge = value
        End Set
    End Property


    ''' <summary>
    ''' Gets or sets the number used to increase the horizontal step of the ball's movement. Valid
    ''' values are [0, Breakout.MaxBallStep].
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property XStep() As Double
        Get
            Return mXStep
        End Get
        Set(ByVal value As Double)
            mXStep = value
            If mXStep < 0 Then
                mXStep = 0
            End If
            If mXStep > Breakout.MaxBallStep Then
                mXStep = Breakout.MaxBallStep
            End If
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the number used to increase the vertical step of the ball's movement. Valid
    ''' values are [0, Breakout.MaxBallStep].
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property YStep() As Double
        Get
            Return mYStep
        End Get
        Set(ByVal value As Double)
            mYStep = value
            If mYStep < 0 Then
                mYStep = 0
            End If
            If mYStep > Breakout.MaxBallStep Then
                mYStep = Breakout.MaxBallStep
            End If
        End Set
    End Property






    ''' <summary>
    ''' Constructs a new ball object.
    ''' </summary>
    ''' <param name="x">the x position of the ball</param>
    ''' <param name="y">the y position of the balle</param>
    ''' <param name="texture">the texture used to draw the ball</param>
    ''' <param name="bounds">the rectangle describing the movement boundaries of the ball</param>
    ''' <param name="speed">the movement speed of the ball in pixels per second</param>
    ''' <param name="initialDirection">describes the initial direction of the ball's movement</param>
    ''' <param name="oobEdge">specifies a boundary edge where the ball may go out of bounds</param>
    ''' <param name="isCenteredH">set true to indicate that the ball will be created centered around x</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal texture As Bitmap, ByVal bounds As Rectangle, ByVal speed As Double, _
                   ByVal initialDirection As Direction, Optional ByVal oobEdge As Edge = Edge.None, _
                   Optional ByVal isCenteredH As Boolean = False)

        MyBase.New(x, y, texture, bounds, speed, True, isCenteredH)


        mInitialDirection = initialDirection

        EdgeOOB = oobEdge

        Me.Reset()

    End Sub





    ''' <summary>
    ''' Overrides the reset method to reset the direction of the ball, its angles, and speed multiplier,
    ''' and set it within bounds.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub Reset()
        MyBase.Reset()

        mOutOfBounds = False

        If mInitialDirection = Direction.Left Or mInitialDirection = Direction.Right Then
            HorizontalDirection = mInitialDirection
        ElseIf mInitialDirection = Direction.Up Or mInitialDirection = Direction.Down Then
            VerticalDirection = mInitialDirection
        End If


        XStep = 1
        YStep = 1

        SpeedMultiplier = 1

    End Sub





    ''' <summary>
    '''  Overrides the update method to move the ball along a line based on direction.
    ''' </summary>
    ''' <param name="secsSinceLastFrame">the fractions of a second since the last call to this method</param>
    ''' <remarks></remarks>
    Public Overrides Sub Update(ByVal secsSinceLastFrame As Double)

        Dim actualDistance As Double = SpeedMultiplier * MoveSpeed * secsSinceLastFrame


        Dim newX As Double = X
        Dim newY As Double = Y



        ' calculate new x
        Select Case HorizontalDirection

            Case Direction.Left
                newX -= XStep
                Exit Select

            Case Direction.Right
                newX += XStep
                Exit Select

        End Select


        ' calculate new y
        Select Case VerticalDirection

            Case Direction.Up
                newY -= YStep
                Exit Select

            Case Direction.Down
                newY += YStep
                Exit Select

        End Select




        MoveAlongLineToward(newX, newY, actualDistance)


        HandleBoundaryCollision()

    End Sub




    ''' <summary>
    ''' Bounces the ball left or right.
    ''' </summary>
    ''' <param name="horizontalDirection">the new horizontal direction of the ball</param>
    ''' <remarks></remarks>
    Public Sub BounceH(ByVal horizontalDirection As Direction)

        If horizontalDirection = Direction.Left Or horizontalDirection = Direction.Right Then

            Me.HorizontalDirection = horizontalDirection

        End If

    End Sub



    ''' <summary>
    ''' Bounces the ball up or down.
    ''' </summary>
    ''' <param name="verticalDirection">the new vertical direction of the ball</param>
    ''' <remarks></remarks>
    Public Sub BounceV(ByVal verticalDirection As Direction)

        If verticalDirection = Direction.Up Or verticalDirection = Direction.Down Then

            Me.VerticalDirection = verticalDirection

        End If

    End Sub




    ''' <summary>
    ''' Tests collision with each boundary edge and causes ball to react.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub HandleBoundaryCollision()


        ' test for and handle vertical boundary collision
        If Me.TopEdgeValue <= MovementBoundaries.Top Then

            ' top edge

            If Not EdgeOOB = Edge.Top Then
                ' if we can't go out of bounds on the top then bounce
                Me.TopEdgeValue = MovementBoundaries.Top
                BounceV(Direction.Down)
            Else
                ' if we can go out of bounds then let it happen and raise an event
                If Me.BottomEdgeValue < MovementBoundaries.Top Then
                    mOutOfBounds = True
                    RaiseEvent OutOfBounds()
                End If
            End If


        ElseIf Me.BottomEdgeValue >= MovementBoundaries.Bottom Then

            ' bottom edge

            If Not EdgeOOB = Edge.Bottom Then
                ' if we can't go out of bounds on the bottom then bounce
                Me.BottomEdgeValue = MovementBoundaries.Bottom
                BounceV(Direction.Up)
            Else
                ' if we can go out of bounds then let it happen and raise an event
                If Me.TopEdgeValue > MovementBoundaries.Bottom Then
                    mOutOfBounds = True
                    RaiseEvent OutOfBounds()
                End If
            End If


        End If  ' vertical boundary collision testing




        ' test for and handle horizontal boundary collision
        If Me.LeftEdgeValue <= MovementBoundaries.Left Then

            ' left edge

            If Not EdgeOOB = Edge.Left Then
                ' if we can't go out of bounds on the left then bounce
                Me.LeftEdgeValue = MovementBoundaries.Left
                BounceH(Direction.Right)
            Else
                ' if we can go out of bounds then let it happen and raise an event
                If Me.RightEdgeValue < MovementBoundaries.Left Then
                    mOutOfBounds = True
                    RaiseEvent OutOfBounds()
                End If
            End If


        ElseIf Me.RightEdgeValue >= MovementBoundaries.Right Then

            ' right edge

            If Not EdgeOOB = Edge.Right Then
                ' if we can't go out of bounds on the right then bounce
                Me.RightEdgeValue = MovementBoundaries.Right
                BounceH(Direction.Left)
            Else
                ' if we can go out of bounds then let it happen and raise an event
                If Me.LeftEdgeValue > MovementBoundaries.Right Then
                    mOutOfBounds = True
                    RaiseEvent OutOfBounds()
                End If
            End If


        End If  ' horizontal boundary collision testing



    End Sub




End Class
