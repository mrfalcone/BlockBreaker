' BlockRow
' ---
' Inherits GameObject to model a row of blocks capable of performing actions.
' 
' Author   : Michael Falcone
' Modified : 3/25/10



Public NotInheritable Class BlockRow
    Inherits GameObject



    ''' <summary>
    ''' Used to pass a requested action to a block.
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure BlockActionRequest

        ''' <summary>
        ''' The action that should be performed on a block.
        ''' </summary>
        ''' <remarks></remarks>
        Dim action As Block.Action

        ''' <summary>
        ''' Object to test for collision if onlyIfCollides is set to true. Not required if onlyIfCollides
        ''' is false and blockIndex is specified.
        ''' </summary>
        ''' <remarks></remarks>
        Dim collidingObject As GameObject

        ''' <summary>
        ''' Indicates that the action will only be performed if collidingObject collides with a block.
        ''' </summary>
        ''' <remarks></remarks>
        Dim onlyIfCollides As Boolean

        ''' <summary>
        ''' The index of the block on which to perform the action regardless of collision. Not
        ''' required if onlyIfCollides is true.
        ''' </summary>
        ''' <remarks></remarks>
        Dim blockIndex As Integer

    End Structure



    ''' <summary>
    ''' Used to return information about a block action request.
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure BlockActionResponse

        ''' <summary>
        ''' Whether the action was successful. If false all other values are undefined.
        ''' </summary>
        ''' <remarks></remarks>
        Dim succeeded As Boolean

        ''' <summary>
        ''' Index of the block affected by the action if succeeded is true.
        ''' </summary>
        ''' <remarks></remarks>
        Dim blockIndex As Integer

        ''' <summary>
        ''' The affected block's location if succeeded is true.
        ''' </summary>
        ''' <remarks></remarks>
        Dim blockCoord As Point

        ''' <summary>
        ''' Stores the edge nearest the collision, if a collision has occurred.
        ''' </summary>
        ''' <remarks></remarks>
        Dim collisionEdge As Edge


        ''' <summary>
        ''' The change, if any, in solid blocks in the row.
        ''' </summary>
        ''' <remarks></remarks>
        Dim changeInSolidBlocks As Integer

    End Structure





    ' private member data
    Private mBlockList() As Block

    Private mInitializedBlocks As Integer = 0

    Private mNonEmptyBlocks As Integer = 0







    ''' <summary>
    ''' Gets the number of blocks in the row that are non-empty.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property NumSolidBlocks() As Integer
        Get
            Return mNonEmptyBlocks
        End Get
        Private Set(ByVal value As Integer)
            mNonEmptyBlocks = value
        End Set
    End Property



    ''' <summary>
    ''' Gets the number of empty blocks in the row.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property NumEmptyBlocks() As Integer
        Get
            Return NumInitializedBlocks - NumSolidBlocks
        End Get
    End Property



    ''' <summary>
    ''' Gets the number of initialized, empty and nonempty blocks in the row.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property NumInitializedBlocks() As Integer
        Get
            Return mInitializedBlocks
        End Get
        Private Set(ByVal value As Integer)
            mInitializedBlocks = value
        End Set
    End Property



    ''' <summary>
    ''' Get the maximum index of accessible blocks in the array.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property MaxBlockIndex() As Integer
        Get
            Return NumInitializedBlocks - 1
        End Get
    End Property








    ''' <summary>
    ''' Constructs a new row of empty blocks.
    ''' </summary>
    ''' <param name="x">x position to be the center of the row</param>
    ''' <param name="y">y position of the top of the row</param>
    ''' <param name="maxBlocks">the maximum number of blocks allowed in the row</param>
    ''' <param name="blockWidth">width of each block</param>
    ''' <param name="blockHeight">height of each block</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal maxBlocks As Integer, ByVal blockWidth As Integer, _
                   ByVal blockHeight As Integer)
        MyBase.New(x, y, 0, blockHeight, True)


        ' initialize the blocks
        ReDim mBlockList(maxBlocks - 1)

        Dim i As Integer
        For i = 0 To UBound(mBlockList)
            mBlockList(i) = New Block(blockWidth, blockHeight)
        Next


    End Sub




    ''' <summary>
    ''' Initializes the specified number of blocks as solid blocks.
    ''' </summary>
    ''' <param name="numBlocks">number of blocks to initialize</param>
    ''' <remarks></remarks>
    Public Sub InitializeBlocks(ByVal numBlocks As Integer)


        ' return if the number is not within bounds
        If numBlocks < 0 Or numBlocks > UBound(mBlockList) + 1 Then
            Return
        End If

        NumInitializedBlocks = numBlocks
        NumSolidBlocks = numBlocks

        ReformRow()

    End Sub




    ''' <summary>
    ''' Renders each block in the row.
    ''' </summary>
    ''' <param name="blockImage">image to use for each block</param>
    ''' <param name="g">graphics object used to render the set</param>
    ''' <remarks></remarks>
    Public Shadows Sub Draw(ByVal blockImage As System.Drawing.Bitmap, ByVal g As System.Drawing.Graphics)

        Dim i As Integer = 0
        For i = 0 To MaxBlockIndex

            mBlockList(i).Draw(blockImage, g)

        Next

    End Sub




    ''' <summary>
    ''' Gets an array of the indices of the empty blocks in the row.
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetEmptyBlockIndices() As Integer()

        Dim indices(NumEmptyBlocks - 1) As Integer
        Dim curIndex As Integer = 0


        Dim i As Integer
        For i = 0 To MaxBlockIndex

            If mBlockList(i).Empty Then
                indices(curIndex) = i
                curIndex += 1
            End If

        Next



        Return indices

    End Function




    ''' <summary>
    ''' Perform an action on some block in the row.
    ''' </summary>
    ''' <param name="request">BlockActionRequestParameters oject</param>
    ''' <returns>An action response</returns>
    ''' <remarks></remarks>
    Public Function PerformBlockAction(ByVal request As BlockActionRequest) As BlockActionResponse


        Dim response As BlockActionResponse
        response.succeeded = False



        Dim targetIndex As Integer = -1 ' stores the index of the target block of the action
        Dim wasBlockEmpty As Boolean    ' stores whether the target block is empty before performing the action




        If request.onlyIfCollides = False Then
            ' if a valid index is specified and the action should be performed regardless of collision

            If request.blockIndex > MaxBlockIndex Or request.blockIndex < 0 Then
                Return response
            End If

            targetIndex = request.blockIndex
            response.collisionEdge = Edge.None



        ElseIf request.onlyIfCollides = True Then

            ' if the object doesn't collide with this row then it won't collide with any blocks in the row
            If Not request.collidingObject.CollidesWith(Me) Then
                Return response
            End If

            Dim e As Edge = Edge.None

            ' loop through each block and determine if there is one colliding
            Dim i As Integer
            For i = 0 To MaxBlockIndex

                e = mBlockList(i).GetEdgeCollidingWith(request.collidingObject, True)

                If Not e = Edge.None Then
                    targetIndex = i
                    response.collisionEdge = e

                    Exit For
                End If

            Next


        End If



        ' if a target has not been identified yet then return
        If targetIndex < 0 Then
            Return response
        End If



        wasBlockEmpty = mBlockList(targetIndex).Empty


        ' perform the action
        response.succeeded = mBlockList(targetIndex).PerformAction(request.action)



        ' adjust solid block count if necessary
        Dim solidChange As Integer = 0
        If wasBlockEmpty = False And mBlockList(targetIndex).Empty = True Then
            ' if the action made the block empty reduce the count
            solidChange -= 1
        ElseIf wasBlockEmpty = True And mBlockList(targetIndex).Empty = False Then
            ' or if the action made the block solid increase the count
            solidChange += 1
        End If

        NumSolidBlocks += solidChange

        response.changeInSolidBlocks = solidChange
        response.blockIndex = targetIndex
        response.blockCoord = mBlockList(targetIndex).Coordinates



        Return response

    End Function








    ''' <summary>
    ''' Creates MaximumBlocks number of blocks and centers the row.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ReformRow()

        ' set the new width and center the row
        Width = NumInitializedBlocks * mBlockList(0).Width

        CenterH(DefaultX)



        ' loop to initialize all the blocks
        Dim xOffset As Integer = 0

        Dim i As Integer = 0
        For i = 0 To MaxBlockIndex

            mBlockList(i).Initialize(X + xOffset, Y)

            xOffset += mBlockList(i).Width

        Next

    End Sub




End Class
