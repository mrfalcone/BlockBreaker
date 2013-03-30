' GameObject
' ---
' Base class modeling a general game object that can be positioned in 2d space, drawn with GDI+ graphics,
' and collide with other objects.
' 
' Author   : Michael Falcone
' Modified : 4/20/10


Public Class GameObject


    ''' <summary>
    ''' Describes which edge of an object.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Edge As Integer
        None = 0
        Top = 1
        Right = 2
        Bottom = 3
        Left = 4
    End Enum



    ''' <summary>
    ''' Raised when the object is resized.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event Resized()


    ' private member data
    Private mIsObjectVisible As Boolean = False

    Private mIsBorderVisible As Boolean = False

    Private mBorderPen As New Pen(Color.Black, 1)


    Private mTexture As Bitmap = Nothing
    Private mHasTexture As Boolean = False


    Private mBounds As New Rectangle


    Private mDefaultX As Integer = 0
    Private mDefaultY As Integer = 0


    Private mIsCollidable As Boolean = True




    ''' <summary>
    ''' Gets the X position of the object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property X() As Integer
        Get
            Return mBounds.X
        End Get
        Set(ByVal value As Integer)
            mBounds.X = value
        End Set
    End Property



    ''' <summary>
    ''' Gets the Y position of the object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Y() As Integer
        Get
            Return mBounds.Y
        End Get
        Set(ByVal value As Integer)
            mBounds.Y = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the coordinates of the upper-left corner of the object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Coordinates() As Point
        Get
            Return mBounds.Location
        End Get
        Set(ByVal value As Point)
            mBounds.Location = value
        End Set
    End Property



    ''' <summary>
    ''' Gets the default X position of the object. Only derived classes may access this property.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Property DefaultX() As Integer
        Get
            Return mDefaultX
        End Get
        Set(ByVal value As Integer)
            mDefaultX = value
        End Set
    End Property



    ''' <summary>
    ''' Gets the default Y position of the object. Only derived classes may access this property.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Property DefaultY() As Integer
        Get
            Return mDefaultY
        End Get
        Set(ByVal value As Integer)
            mDefaultY = value
        End Set
    End Property



    ''' <summary>
    ''' Gets the width of the object. Only derived classes may change this property.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Width() As Integer
        Get
            Return mBounds.Width
        End Get
        Protected Set(ByVal value As Integer)
            mBounds.Width = value
            RaiseEvent Resized()
        End Set
    End Property




    ''' <summary>
    ''' Gets the height of the object. Only derived classes may change this property.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Height() As Integer
        Get
            Return mBounds.Height
        End Get
        Protected Set(ByVal value As Integer)
            mBounds.Height = value
            RaiseEvent Resized()
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the Y value of the top edge of the object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property TopEdgeValue() As Integer
        Get
            Return Y
        End Get
        Set(ByVal value As Integer)
            Y = value
        End Set
    End Property



    ''' <summary>
    ''' Gets a point describing the center of the object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property CenterPoint() As Point
        Get
            Return New Point(X + Width \ 2, Y + Height \ 2)
        End Get
    End Property



    ''' <summary>
    ''' Gets or sets the Y value of the bottom edge of the object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property BottomEdgeValue() As Integer
        Get
            Return Y + Height
        End Get
        Set(ByVal value As Integer)
            Y = value - Height
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the X value of the left edge of the object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property LeftEdgeValue() As Integer
        Get
            Return X
        End Get
        Set(ByVal value As Integer)
            X = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the X value of the right edge of the object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property RightEdgeValue() As Integer
        Get
            Return X + Width
        End Get
        Set(ByVal value As Integer)
            X = value - Width
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets whether the object can collide with other collidable objects.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Collidable() As Boolean
        Get
            Return mIsCollidable
        End Get
        Set(ByVal value As Boolean)
            mIsCollidable = value
        End Set
    End Property





    ''' <summary>
    ''' Gets a rectangle describing the bounds of the object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property BoundRectangle() As Rectangle
        Get
            Return mBounds
        End Get

    End Property



    ''' <summary>
    ''' Gets or sets the texture used to render the object, makes it visible, and sets object's width and height to the texture's.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Texture() As Bitmap
        Get
            Return mTexture
        End Get
        Set(ByVal value As Bitmap)
            Visible = True
            mTexture = value
            mHasTexture = True

            Width = value.Width
            Height = value.Height

        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the pen used to render the object's border. Only derived classes may access this property.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Property BorderPen() As Pen
        Get
            Return mBorderPen
        End Get
        Set(ByVal value As Pen)
            mBorderPen = value
        End Set
    End Property




    ''' <summary>
    ''' Returns whether or not the object has a texture available for rendering.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property HasTexture() As Boolean
        Get
            Return mHasTexture
        End Get
    End Property



    ''' <summary>
    ''' Gets or sets whether the object is visible.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Visible() As Boolean
        Get
            Return mIsObjectVisible
        End Get
        Set(ByVal value As Boolean)
            mIsObjectVisible = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the width of the object's border.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property BorderWidth() As Single
        Get
            Return mBorderPen.Width
        End Get
        Set(ByVal value As Single)
            If value >= 0 Then
                mBorderPen.Width = value
            End If
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the object's border color.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property BorderColor() As Color
        Get
            Return mBorderPen.Color
        End Get
        Set(ByVal value As Color)
            mBorderPen.Color = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets whether the object's border is displayed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property BorderVisible() As Boolean
        Get
            Return mIsBorderVisible
        End Get
        Set(ByVal value As Boolean)
            mIsBorderVisible = value
        End Set
    End Property




    ''' <summary>
    ''' Constructs an invisible, non-collidable GameObject at 0,0
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

        Collidable = False

    End Sub



    ''' <summary>
    ''' Constructs an invisible GameObject with a position in the game.
    ''' </summary>
    ''' <param name="x">x position</param>
    ''' <param name="y">y position</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer)

        Me.New(x, y, 0, 0)

    End Sub



    ''' <summary>
    ''' Constructs a drawable GameObject with a position in the game and a size.
    ''' </summary>
    ''' <param name="x">x position</param>
    ''' <param name="y">y position</param>
    ''' <param name="texture">the image to display when drawing the object</param>
    ''' <param name="isVisible">set true to indicate that this object will be visible</param>
    ''' <param name="isCenteredH">set true to indicate that the object will be created centered around the specified X</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal texture As Bitmap, _
                   Optional ByVal isVisible As Boolean = True, Optional ByVal isCenteredH As Boolean = False)

        Me.New(x, y, texture.Width, texture.Height, texture, isVisible, isCenteredH)

    End Sub



    ''' <summary>
    ''' Constructs a drawable GameObject with a position in the game and a size.
    ''' </summary>
    ''' <param name="x">x position</param>
    ''' <param name="y">y position</param>
    ''' <param name="width">width of the object in pixels</param>
    ''' <param name="height">height of the object in pixels</param>
    ''' <param name="texture">the image to display when drawing the object</param>
    ''' <param name="isVisible">set true to indicate that this object will be visible</param>
    ''' <param name="isCenteredH">set true to indicate that the object will be created centered around the specified X</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, _
                   ByVal height As Integer, ByVal texture As Bitmap, _
                   Optional ByVal isVisible As Boolean = True, _
                   Optional ByVal isCenteredH As Boolean = False)

        Me.New(x, y, width, height, isVisible, isCenteredH)

        Me.Texture = texture

    End Sub



    ''' <summary>
    ''' Constructs a GameObject with a position in the game and a size.
    ''' </summary>
    ''' <param name="x">x position</param>
    ''' <param name="y">y position</param>
    ''' <param name="objectWidth">width of the object in pixels</param>
    ''' <param name="objectHeight">height of the object in pixels</param>
    ''' <param name="isVisible">set true to indicate that this object will be visible</param>
    ''' <param name="isCenteredH">set true to indicate that the object will be created centered around the specified X</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal objectWidth As Integer, _
                   ByVal objectHeight As Integer, Optional ByVal isVisible As Boolean = False, _
                   Optional ByVal isCenteredH As Boolean = False)

        ' set height and width
        Me.Width = objectWidth
        Me.Height = objectHeight



        ' set up x position
        If isCenteredH Then
            Me.CenterH(x)
        Else
            Me.X = x
        End If

        Me.DefaultX = Me.X


        ' set up y position
        Me.Y = y
        Me.DefaultY = y


        Me.Visible = isVisible


    End Sub



    ''' <summary>
    ''' Center the object horizontally at the specified x position.
    ''' </summary>
    ''' <param name="centerX">the x position at which to center the object</param>
    ''' <remarks></remarks>
    Public Sub CenterH(ByVal centerX As Integer)
        X = centerX - Width \ 2
    End Sub



    ''' <summary>
    ''' Center the object vertically at the specified y position.
    ''' </summary>
    ''' <param name="centerY">the y position at which to center the object</param>
    ''' <remarks></remarks>
    Public Sub CenterV(ByVal centerY As Integer)
        Y = centerY - Height \ 2
    End Sub



    ''' <summary>
    ''' Resets the object to its default position.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Sub Reset()

        X = DefaultX
        Y = DefaultY

    End Sub



    ''' <summary>
    ''' Makes the object visible.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Sub Show()
        Visible = True
    End Sub



    ''' <summary>
    ''' Makes the object invisible.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Sub Hide()
        Visible = False
    End Sub




    ''' <summary>
    ''' Renders the object.
    ''' </summary>
    ''' <param name="g">the graphics object used for rendering the game object</param>
    ''' <param name="borderOnly">set true to render only the object's border</param>
    ''' <remarks></remarks>
    Public Overridable Sub Draw(ByVal g As Graphics, Optional ByVal borderOnly As Boolean = False)

        If HasTexture Then
            Draw(Texture, g, borderOnly)
        End If

    End Sub



    ''' <summary>
    ''' Renders the object using the specified image.
    ''' </summary>
    ''' <param name="image">the image used to render the object</param>
    ''' <param name="g">the graphics object used for rendering the game object</param>
    ''' <param name="borderOnly">set true to render only the object's border</param>
    ''' <remarks></remarks>
    Public Overridable Sub Draw(ByVal image As Bitmap, ByVal g As Graphics, Optional ByVal borderOnly As Boolean = False)

        ' do not proceed if object is not visible
        If Not Visible Then
            Return
        End If


        If Not borderOnly Then
            ' g.DrawImage(image, BoundRectangle)
            g.DrawImageUnscaledAndClipped(image, BoundRectangle)
        End If


        If BorderVisible Then
            g.DrawRectangle(BorderPen, BoundRectangle)
        End If


    End Sub




    ''' <summary>
    ''' Tests whether this object collides with the specified game object.
    ''' </summary>
    ''' <param name="o">the object with which to test collision</param>
    ''' <returns>true if the objects collide, false otherwise</returns>
    ''' <remarks></remarks>
    Public Overridable Function CollidesWith(ByVal o As GameObject) As Boolean

        If Not Me.Collidable Then
            Return False
        End If

        If Not o.Collidable Then
            Return False
        End If


        ' checks vertical coincidence
        If o.BottomEdgeValue >= Me.TopEdgeValue AndAlso o.TopEdgeValue <= Me.BottomEdgeValue Then

            ' if we have vertical coincidence, check horizontal coincidence
            If o.RightEdgeValue >= Me.LeftEdgeValue AndAlso o.LeftEdgeValue <= Me.RightEdgeValue Then

                ' if we have coincidence both vertically and horizontally, the objects collide
                Return True
            End If

        End If


        Return False

    End Function




    ''' <summary>
    ''' Gets the edge of this object to which the colliding object is closest, if the objects are colliding.
    ''' </summary>
    ''' <param name="o">the object with which to test collision</param>
    ''' <param name="testMovementToward">specify true to indicate that the collision will only be detected if the specified object is moving toward this object</param>
    ''' <returns>the edge closest to the collision, or Edge.None if no collision occurs.</returns>
    ''' <remarks></remarks>
    Public Overridable Function GetEdgeCollidingWith(ByVal o As GameObject, Optional ByVal testMovementToward As Boolean = False) As Edge


        ' if there is no collision then no edge is being collided with
        If Not CollidesWith(o) Then
            Return Edge.None
        End If



        Dim closestEdge As Edge = Edge.None

        Dim closestDist As Integer = Me.Width
        Dim workingDist As Integer = 0



        ' test left edge
        workingDist = Math.Abs(o.RightEdgeValue - Me.LeftEdgeValue)

        If workingDist < closestDist Then
            closestDist = workingDist
            closestEdge = Edge.Left
        End If



        ' test top edge
        workingDist = Math.Abs(o.BottomEdgeValue - Me.TopEdgeValue)

        If workingDist < closestDist Then
            closestDist = workingDist
            closestEdge = Edge.Top
        End If



        ' test right edge
        workingDist = Math.Abs(o.LeftEdgeValue - Me.RightEdgeValue)

        If workingDist < closestDist Then
            closestDist = workingDist
            closestEdge = Edge.Right
        End If



        ' test bottom edge
        workingDist = Math.Abs(o.TopEdgeValue - Me.BottomEdgeValue)

        If workingDist < closestDist Then
            closestDist = workingDist
            closestEdge = Edge.Bottom
        End If




        ' if testing collision with a moveable object then make sure collision edge makes sense
        If testMovementToward AndAlso TypeOf o Is MoveableObject Then

            Dim m As MoveableObject = CType(o, MoveableObject)


            ' if collision occurred on top or bottom check vertical direction
            If closestEdge = Edge.Top And m.VerticalDirection <> MoveableObject.Direction.Down Then
                closestEdge = Edge.None

            ElseIf closestEdge = Edge.Bottom And m.VerticalDirection <> MoveableObject.Direction.Up Then
                closestEdge = Edge.None

            End If


            ' if collision occurred on left or right check horizontal direction
            If closestEdge = Edge.Left And m.HorizontalDirection <> MoveableObject.Direction.Right Then
                closestEdge = Edge.None

            ElseIf closestEdge = Edge.Right And m.HorizontalDirection <> MoveableObject.Direction.Left Then
                closestEdge = Edge.None

            End If


        End If



        Return closestEdge

    End Function



    ''' <summary>
    ''' Scales the width of the object by the specified scale factor.
    ''' </summary>
    ''' <param name="scaleFactor">amount by which the width will be scaled</param>
    ''' <remarks></remarks>
    Public Sub ScaleWidth(ByVal scaleFactor As Single)

        If scaleFactor > 0 Then
            Width = CInt(Width * scaleFactor)
            RaiseEvent Resized()
        End If

    End Sub



    ''' <summary>
    ''' Scales the height of the object by the specified scale factor.
    ''' </summary>
    ''' <param name="scaleFactor">amount by which the width will be scaled</param>
    ''' <remarks></remarks>
    Public Sub ScaleHeight(ByVal scaleFactor As Single)

        If scaleFactor > 0 Then
            Height = CInt(Height * scaleFactor)
            RaiseEvent Resized()
        End If

    End Sub




End Class
