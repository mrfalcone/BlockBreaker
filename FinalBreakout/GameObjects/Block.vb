' Block
' ---
' Inherits GameObject to model a destroyable block.
' 
' Author   : Michael Falcone
' Modified : 3/25/10



Public NotInheritable Class Block
    Inherits GameObject


    ''' <summary>
    ''' Actions that may be performed by block.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Action As Integer
        ''' <summary>
        ''' Indicates that the block will be destroyed.
        ''' </summary>
        ''' <remarks></remarks>
        Destroy

        ''' <summary>
        ''' Indicates that the block will no longer be solid or collidable.
        ''' </summary>
        ''' <remarks></remarks>
        SetEmpty

        ''' <summary>
        ''' Indicates that the block will become solid and collidable.
        ''' </summary>
        ''' <remarks></remarks>
        SetSolid

        ''' <summary>
        ''' Toggles whether the block is solid but does not affect collision.
        ''' </summary>
        ''' <remarks></remarks>
        ToggleEmpty

        ''' <summary>
        ''' Highlights the block whether solid or empty.
        ''' </summary>
        ''' <remarks></remarks>
        Highlight


        ''' <summary>
        ''' Removes highlighting from the block.
        ''' </summary>
        ''' <remarks></remarks>
        Unhighlight

    End Enum






    ' private member data
    Private mIsEmpty As Boolean = True




    ''' <summary>
    ''' Gets or sets whether the block is empty.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Empty() As Boolean
        Get
            Return mIsEmpty
        End Get
        Private Set(ByVal value As Boolean)
            mIsEmpty = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets whether the block is currently highlighted.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property Highlighted() As Boolean
        Get
            Return BorderVisible
        End Get
        Set(ByVal value As Boolean)
            BorderVisible = value
        End Set
    End Property




    ''' <summary>
    ''' Constructs a new block object.
    ''' </summary>
    ''' <param name="blockWidth">the width of the block object</param>
    ''' <param name="blockHeight">the height of the block object</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal blockWidth As Integer, ByVal blockHeight As Integer)
        MyBase.New(0, 0, blockWidth, blockHeight, True)

        BorderColor = Color.White
        BorderWidth = 1

    End Sub




    ''' <summary>
    ''' Shadows the draw method to make sure the block is not drawn if it is empty.
    ''' </summary>
    ''' <param name="image"></param>
    ''' <param name="g"></param>
    ''' <remarks></remarks>
    Public Shadows Sub Draw(ByVal image As System.Drawing.Bitmap, ByVal g As System.Drawing.Graphics)

        If Not Empty Then
            MyBase.Draw(image, g)

        ElseIf Highlighted Then
            MyBase.Draw(image, g, True)
        End If

    End Sub




    ''' <summary>
    ''' Initializes the block to make it solid and collidable, and moves it to the specified location.
    ''' </summary>
    ''' <param name="x">x position of the block</param>
    ''' <param name="y">y position of the block</param>
    ''' <remarks></remarks>
    Public Sub Initialize(ByVal x As Integer, ByVal y As Integer)

        Me.X = x
        Me.Y = y

        Empty = False

        BorderVisible = False

        Collidable = True

    End Sub






    ''' <summary>
    ''' Perform the specified action.
    ''' </summary>
    ''' <param name="a">action to perform</param>
    ''' <returns>true if the action is successful, false otherwise</returns>
    ''' <remarks></remarks>
    Public Function PerformAction(ByVal a As Action) As Boolean


        Select Case a

            Case Action.SetEmpty
            Case Action.Destroy
                Destroy()
                Return True

            Case Action.SetSolid
                SetSolid()
                Return True

            Case Action.ToggleEmpty
                ToggleEmpty()
                Return True

            Case Action.Highlight
                Highlighted = True
                Return True

            Case Action.Unhighlight
                Highlighted = False
                Return True

        End Select



        Return False

    End Function




    ''' <summary>
    ''' Destroys the block.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Destroy()

        Empty = True
        Collidable = False

    End Sub



    ''' <summary>
    ''' Sets the block solid and collidable.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetSolid()

        Collidable = True
        Empty = False

    End Sub



    ''' <summary>
    ''' Toggles the value of the Empty property without affecting collision detection.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ToggleEmpty()

        If Empty Then
            Empty = False
        Else
            Empty = True
        End If

    End Sub



End Class
