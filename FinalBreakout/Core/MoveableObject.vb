' MoveableObject
' ---
' Inherits GameObject to model an object that can move.
' 
' Author   : Michael Falcone
' Modified : 3/25/10


Public Class MoveableObject
    Inherits GameObject


    ''' <summary>
    ''' Describes the direction that an object may be facing or moving.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Direction As Integer
        None
        Up
        Right
        Down
        Left
    End Enum




    ' private member data
    Private mBoundary As Rectangle
    Private mIsBounded As Boolean = False

    Private mMoveSpeed As Double = 0
    Private mSpeedMultiplier As Double = 1

    Private mVDirection As Direction = Direction.None
    Private mHDirection As Direction = Direction.None





    ''' <summary>
    ''' Gets a rectangle describing the movement boundaries of the object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property MovementBoundaries() As Rectangle
        Get
            Return mBoundary
        End Get
        Protected Set(ByVal value As Rectangle)
            mBoundary = value
            IsBounded = True
        End Set
    End Property



    ''' <summary>
    ''' Gets whether the object has movement boundaries.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property IsBounded() As Boolean
        Get
            Return mIsBounded
        End Get
        Protected Set(ByVal value As Boolean)
            mIsBounded = value
        End Set
    End Property



    ''' <summary>
    ''' Gets the movement speed of the object in pixels-per-second.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property MoveSpeed() As Double
        Get
            Return mMoveSpeed
        End Get
        Protected Set(ByVal value As Double)
            mMoveSpeed = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets a multiplier used to change the speed of object's movement. Default value is 1.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property SpeedMultiplier() As Double
        Get
            Return mSpeedMultiplier
        End Get
        Set(ByVal value As Double)
            mSpeedMultiplier = value
            If mSpeedMultiplier <= 0 Then
                mSpeedMultiplier = 0
            End If
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the horizontal movement direction of the object. Up and Down are not valid directions.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property HorizontalDirection() As Direction
        Get
            Return mHDirection
        End Get
        Set(ByVal value As Direction)
            If value = Direction.Up Or value = Direction.Down Then
                mHDirection = Direction.None
            Else
                mHDirection = value
            End If
        End Set
    End Property




    ''' <summary>
    ''' Gets or sets the vertical movement direction of the object. Left and Right are not valid directions.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property VerticalDirection() As Direction
        Get
            Return mVDirection
        End Get
        Set(ByVal value As Direction)
            If value = Direction.Left Or value = Direction.Right Then
                mVDirection = Direction.None
            Else
                mVDirection = value
            End If
        End Set
    End Property







    ''' <summary>
    ''' Constructs a new moveable object at x,y.
    ''' </summary>
    ''' <param name="x">x position of the object</param>
    ''' <param name="y">y position of the object</param>
    ''' <param name="moveSpeed">the constant movement speed of the object in pixels-per-second</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal moveSpeed As Double)
        MyBase.New(x, y)

        Me.MoveSpeed = MoveSpeed

    End Sub



    ''' <summary>
    ''' Constructs a new moveable object with the specified movement boundaries.
    ''' </summary>
    ''' <param name="x">x position of the object</param>
    ''' <param name="y">y position of the object</param>
    ''' <param name="moveBounds">describes a rectangle in 2d space that the object will be restricted to</param>
    ''' <param name="moveSpeed">the constant movement speed of the object in pixels-per-second</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal moveBounds As Rectangle, ByVal moveSpeed As Double)

        MyBase.New(x, y)

        MovementBoundaries = moveBounds
        Me.MoveSpeed = moveSpeed

    End Sub




    ''' <summary>
    ''' Constructs a new moveable object with the specified texture.
    ''' </summary>
    ''' <param name="x">x position of the object</param>
    ''' <param name="y">y position of the object</param>
    ''' <param name="texture">texture used to draw the object</param>
    ''' <param name="moveSpeed">the constant movement speed of the object in pixels-per-second</param>
    ''' <param name="isVisible">set true to indicate that this object will be visible</param>
    ''' <param name="isCenteredH">set true to indicate that the object will be created centered around the specified X</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal texture As Bitmap, ByVal moveSpeed As Double, _
                   Optional ByVal isVisible As Boolean = True, Optional ByVal isCenteredH As Boolean = False)

        MyBase.New(x, y, texture, isVisible, isCenteredH)

        Me.MoveSpeed = moveSpeed

    End Sub




    ''' <summary>
    ''' Constructs a bounded moveable object using the specified texture.
    ''' </summary>
    ''' <param name="x">x position of the object</param>
    ''' <param name="y">y position of the object</param>
    ''' <param name="texture">texture used to draw the object</param>
    ''' <param name="moveBounds">describes a rectangle in 2d space that the object will be restricted to</param>
    ''' <param name="moveSpeed">the constant movement speed of the object in pixels-per-second</param>
    ''' <param name="isVisible">set true to indicate that this object will be visible</param>
    ''' <param name="isCenteredH">set true to indicate that the object will be created centered around the specified X</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal texture As Bitmap, _
                   ByVal moveBounds As Rectangle, ByVal moveSpeed As Double, _
                   Optional ByVal isVisible As Boolean = True, Optional ByVal isCenteredH As Boolean = False)

        MyBase.New(x, y, texture, isVisible, isCenteredH)

        MovementBoundaries = moveBounds
        Me.MoveSpeed = moveSpeed

    End Sub



    ''' <summary>
    ''' Overrides the reset method to also reset movement directions to none.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub Reset()
        MyBase.Reset()

        HorizontalDirection = Direction.None
        VerticalDirection = Direction.None

    End Sub



    ''' <summary>
    ''' Updates the object's position if it is moving in some direction.
    ''' </summary>
    ''' <param name="secsSinceLastFrame">the fractions of a second since the last call to this method</param>
    ''' <remarks></remarks>
    Public Overridable Sub Update(ByVal secsSinceLastFrame As Double)


        Dim actualDistance As Double = SpeedMultiplier * MoveSpeed * secsSinceLastFrame

        Dim newX As Double = X
        Dim newY As Double = Y



        ' determine new horizontal position
        Select Case HorizontalDirection

            Case Direction.Left
                newX -= actualDistance
                Exit Select

            Case Direction.Right
                newX += actualDistance
                Exit Select

        End Select


        ' determine new vertical position
        Select Case VerticalDirection

            Case Direction.Up
                newY -= actualDistance
                Exit Select

            Case Direction.Down
                newY += actualDistance
                Exit Select

        End Select



        If HorizontalDirection = Direction.None Or VerticalDirection = Direction.None Then

            ' if only moving in one direction then we automatically are moving along a straight line
            X = CInt(newX)
            Y = CInt(newY)

        Else

            ' otherwise we must use the available method
            MoveAlongLineToward(newX, newY, actualDistance)

        End If



        ClipPositionToBounds()

    End Sub



    ''' <summary>
    ''' Gets whether or not this object is moving in the same horizontal direction as the specified object.
    ''' </summary>
    ''' <param name="o">the moveable object to test direction against</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SameHDirectionAs(ByVal o As MoveableObject) As Boolean

        If o.HorizontalDirection = Me.HorizontalDirection Then
            Return True
        End If

        Return False

    End Function



    ''' <summary>
    ''' Gets whether or not this object is moving in the same vertical direction as the specified object.
    ''' </summary>
    ''' <param name="o">the moveable object to test direction against</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SameVDirectionAs(ByVal o As MoveableObject) As Boolean

        If o.VerticalDirection = Me.VerticalDirection Then
            Return True
        End If

        Return False

    End Function



    ''' <summary>
    ''' Moves the object the specified distance along a line toward the specified destination.
    ''' </summary>
    ''' <param name="newX">new x position of the object</param>
    ''' <param name="newY">new y position of the object</param>
    ''' <param name="distance">the distance to move along the line</param>
    ''' <remarks></remarks>
    Protected Overridable Sub MoveAlongLineToward(ByVal newX As Double, ByVal newY As Double, _
                                                  ByVal distance As Double)

        Dim xDiff As Double = newX - X
        Dim yDiff As Double = newY - Y

        Dim lineLength As Double


        ' determine the length of the line using the distance formula
        lineLength = Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2))


        ' set the new x and y values
        If lineLength <> 0 Then
            X += CInt((xDiff / lineLength) * distance)
            Y += CInt((yDiff / lineLength) * distance)
        End If



    End Sub



    ''' <summary>
    ''' Tests whether the object is in its boundaries and adjusts position to the inside edge.
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClipPositionToBounds()

        If Not IsBounded Then
            Return
        End If


        ' clip to top or bottom edge
        If TopEdgeValue < MovementBoundaries.Top Then
            TopEdgeValue = MovementBoundaries.Top
        ElseIf BottomEdgeValue > MovementBoundaries.Bottom Then
            BottomEdgeValue = MovementBoundaries.Bottom
        End If


        ' clip to left or right edge
        If LeftEdgeValue < MovementBoundaries.Left Then
            LeftEdgeValue = MovementBoundaries.Left
        ElseIf RightEdgeValue > MovementBoundaries.Right Then
            RightEdgeValue = MovementBoundaries.Right
        End If

    End Sub



End Class
