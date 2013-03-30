' Paddle
' ---
' Inherits MoveableObject to model a bounded paddle that can move one direction at a time.
' 
' Author   : Michael Falcone
' Modified : 4/20/10


Public NotInheritable Class Paddle
    Inherits MoveableObject



    Private mIsMoving As Boolean = False


    Private mOriginalWidth As Integer
    Private mOriginalHeight As Integer



    ''' <summary>
    ''' Constructs a new paddle object.
    ''' </summary>
    ''' <param name="x">the x position of the paddle</param>
    ''' <param name="y">the y position of the paddle</param>
    ''' <param name="texture">the texture used to draw the paddle</param>
    ''' <param name="bounds">the rectangle describing the movement boundaries of the paddle</param>
    ''' <param name="speed">the movement speed of the paddle in pixels per second</param>
    ''' <param name="isCenteredH">set true to indicate that the paddle will be created centered around x</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal texture As Bitmap, ByVal bounds As Rectangle, ByVal speed As Double, _
                   Optional ByVal isCenteredH As Boolean = False)

        MyBase.New(x, y, texture, bounds, speed, True, isCenteredH)

        mOriginalHeight = Me.Height
        mOriginalWidth = Me.Width

    End Sub



    

    ''' <summary>
    ''' Causes the paddle to begin moving in the specified direction.
    ''' </summary>
    ''' <param name="direction">the horizontal or vertical direction that the paddle will move</param>
    ''' <remarks></remarks>
    Public Sub BeginMoving(ByVal direction As Direction)


        If direction = MoveableObject.Direction.Left Or direction = MoveableObject.Direction.Right Then

            HorizontalDirection = direction
            VerticalDirection = MoveableObject.Direction.None

            mIsMoving = True

        ElseIf direction = MoveableObject.Direction.Up Or direction = MoveableObject.Direction.Down Then

            VerticalDirection = direction
            HorizontalDirection = MoveableObject.Direction.None

            mIsMoving = True

        End If


    End Sub



    ''' <summary>
    ''' Resets the size of the paddle back to its original size.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ResetSize()

        Me.Width = mOriginalWidth
        Me.Height = mOriginalHeight

    End Sub



    ''' <summary>
    ''' Causes the paddle to stop moving in the specified direction.
    ''' </summary>
    ''' <param name="direction">if the paddle is moving in this direction it will stop</param>
    ''' <remarks></remarks>
    Public Sub StopMoving(ByVal direction As Direction)

        If VerticalDirection = direction Or HorizontalDirection = direction Then

            VerticalDirection = MoveableObject.Direction.None
            HorizontalDirection = MoveableObject.Direction.None

            mIsMoving = False

        End If

    End Sub




    ''' <summary>
    ''' Sets the horizontal position of the paddle.
    ''' </summary>
    ''' <param name="x">the new x location of the paddle</param>
    ''' <param name="calculateDirection">set true to indicate that the paddle will determine its movement direction from the new value</param>
    ''' <remarks></remarks>
    Public Sub SetHorizontalPosition(ByVal x As Integer, Optional ByVal calculateDirection As Boolean = False)

        Static oldX As Integer = 0


        oldX = Me.X

        Me.X = x

        ClipPositionToBounds()


        If calculateDirection Then

            If Me.X > oldX Then
                HorizontalDirection = Direction.Right
            ElseIf Me.X < oldX Then
                HorizontalDirection = Direction.Left
            End If


        Else
            HorizontalDirection = Direction.None

        End If

    End Sub




    ''' <summary>
    ''' Sets the vertical position of the paddle.
    ''' </summary>
    ''' <param name="y">the new y location of the paddle</param>
    ''' <param name="calculateDirection">set true to indicate that the paddle will determine its movement direction from the new value</param>
    ''' <remarks></remarks>
    Public Sub SetVerticalPosition(ByVal y As Integer, Optional ByVal calculateDirection As Boolean = False)

        Static oldY As Integer = 0

        oldY = Me.Y

        Me.Y = y

        ClipPositionToBounds()


        If calculateDirection Then

            If Me.Y > oldY Then
                VerticalDirection = Direction.Down
            ElseIf Me.Y < oldY Then
                VerticalDirection = Direction.Up
            End If


        Else
            VerticalDirection = Direction.None

        End If

    End Sub



    ''' <summary>
    ''' Overrides update method to make sure paddle does not move if it is not necessary.
    ''' </summary>
    ''' <param name="secsSinceLastFrame">the fractions of a second since last frame</param>
    ''' <remarks></remarks>
    Public Overrides Sub Update(ByVal secsSinceLastFrame As Double)

        If mIsMoving Then

            ' if the paddle is currently moving then update it
            MyBase.Update(secsSinceLastFrame)

        End If


    End Sub



End Class
