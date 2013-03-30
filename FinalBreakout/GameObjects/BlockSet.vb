' BlockSet
' ---
' Inherits GameObject to model a set a blocks capable of performing actions.
' 
' Author   : Michael Falcone
' Modified : 4/12/10


Public NotInheritable Class BlockSet
    Inherits GameObject


    ''' <summary>
    ''' Raised when a block becomes empty as the result of a performed action.
    ''' </summary>
    ''' <param name="blockLocation">the location of the emptied block in 2d space</param>
    ''' <param name="blockId">unique id number of the block in the blockset, from [0, MaximumBlocks - 1]</param>
    ''' <remarks></remarks>
    Public Event BlockEmptied(ByVal blockLocation As Point, ByVal blockId As Integer)


    ''' <summary>
    ''' Raised when a block is destroyed by collision.
    ''' </summary>
    ''' <param name="o">reference to the object that destroyed the block</param>
    ''' <param name="edgeOfCollision">the edge nearest the point of collision, if the block was emptied by collision</param>
    ''' <param name="blockRect">rectangle describing the position and size of the block destroyed</param>
    ''' <remarks></remarks>
    Public Event BlockDestroyed(ByRef o As GameObject, ByVal edgeOfCollision As Edge, ByVal blockRect As Rectangle)


    ''' <summary>
    ''' Raised when an empty block becomes solid as the result of a performed action.
    ''' </summary>
    ''' <param name="blockId">unique id number of the block in the blockset, from [0, MaximumBlocks - 1]</param>
    ''' <remarks></remarks>
    Public Event BlockFilled(ByVal blockId As Integer)



    ''' <summary>
    ''' Describes the location of a block within a blockset.
    ''' </summary>
    ''' <remarks></remarks>
    Structure BlockDescriptor
        ''' <summary>
        ''' The index of the row holding the block.
        ''' </summary>
        ''' <remarks></remarks>
        Dim rowIndex As Integer

        ''' <summary>
        ''' The index of the block within the row at rowIndex.
        ''' </summary>
        ''' <remarks></remarks>
        Dim blockIndex As Integer

        ''' <summary>
        ''' Constructs a new block descriptor with the specified values.
        ''' </summary>
        ''' <param name="row">the index of the row holding the block</param>
        ''' <param name="block">the index of the block within the row</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal row As Integer, ByVal block As Integer)
            rowIndex = row
            blockIndex = block
        End Sub

    End Structure



    ''' <summary>
    ''' Describes the blockset's pattern of blocks.
    ''' </summary>
    ''' <remarks></remarks>
    Structure BlockSetPattern

        ''' <summary>
        ''' Array of integers where each index represents a row and each integer represents the number of blocks in that row.
        ''' </summary>
        ''' <remarks></remarks>
        Dim rowData() As Integer

        ''' <summary>
        ''' Array of unique block ids of empty blocks.
        ''' </summary>
        ''' <remarks></remarks>
        Dim emptyBlocks() As Integer

    End Structure




    Private mRowList() As BlockRow

    Private mNumBlocks As Integer = 0

    Private Shared mMaxBlockCount As Integer = 0

    Private mBlocksPerRow As Integer = 0

    Private mActionRequest As New BlockRow.BlockActionRequest

    Private mSuppressEvents As Boolean = False



    ''' <summary>
    ''' Gets the current number of non-empty blocks contained within the block set.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property NumSolidBlocks() As Integer
        Get
            Return mNumBlocks
        End Get
        Private Set(ByVal value As Integer)
            mNumBlocks = value
        End Set
    End Property



    ''' <summary>
    ''' Gets the maximum number of blocks the blockset can hold.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Property MaximumBlocks() As Integer
        Get
            Return mMaxBlockCount
        End Get
        Private Set(ByVal value As Integer)
            mMaxBlockCount = value
        End Set
    End Property




    ''' <summary>
    ''' Constructs a set of empty blocks.
    ''' </summary>
    ''' <param name="x">the x position to be the center of the set</param>
    ''' <param name="y">the y position of the top of the set</param>
    ''' <param name="maxRows">the maximum number of rows allowed</param>
    ''' <param name="maxBlocksPerRow">the maximum number of blocks allowed per row</param>
    ''' <param name="blockWidth">width of each block</param>
    ''' <param name="blockHeight">height of each block</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal x As Integer, ByVal y As Integer, _
                   ByVal maxRows As Integer, ByVal maxBlocksPerRow As Integer, _
                   ByVal blockWidth As Integer, ByVal blockHeight As Integer)
        MyBase.New(x, y)

        Visible = True

        MaximumBlocks = maxRows * maxBlocksPerRow

        Me.mBlocksPerRow = maxBlocksPerRow

        ' initialize the rows
        ReDim mRowList(maxRows - 1)

        Dim yOffset As Integer = 0

        Dim i As Integer
        For i = 0 To UBound(mRowList)
            mRowList(i) = New BlockRow(x, y + yOffset, maxBlocksPerRow, blockWidth, blockHeight)
            yOffset += mRowList(i).Height
        Next

    End Sub



    ''' <summary>
    ''' Sets the size of the specified row to the specified value.
    ''' </summary>
    ''' <param name="rowIndex">the index of the row to change</param>
    ''' <param name="value">the number of blocks of the row</param>
    ''' <remarks></remarks>
    Public Sub SetRowValue(ByVal rowIndex As Integer, ByVal value As Integer)


        If rowIndex >= 0 AndAlso rowIndex <= UBound(mRowList) Then

            If value < 0 Or value > mBlocksPerRow Then
                Return
            End If


            NumSolidBlocks += value - mRowList(rowIndex).NumSolidBlocks

            mRowList(rowIndex).InitializeBlocks(value)


            CalculateDimensions()

        End If


    End Sub


    ''' <summary>
    ''' Gets the size of the specified row.
    ''' </summary>
    ''' <param name="rowIndex">index of the row to get the value of</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRowValue(ByVal rowIndex As Integer) As Integer

        Return mRowList(rowIndex).NumInitializedBlocks

    End Function



    ''' <summary>
    ''' Clears the set of blocks.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Clear()

        Dim i As Integer
        For i = 0 To UBound(mRowList)
            SetRowValue(i, 0)
        Next

    End Sub



    ''' <summary>
    ''' Renders each row in the set of blocks.
    ''' </summary>
    ''' <param name="blockImage">image to use for each block</param>
    ''' <param name="g">graphics object used to render the set</param>
    ''' <remarks></remarks>
    Public Shadows Sub Draw(ByVal blockImage As System.Drawing.Bitmap, ByVal g As System.Drawing.Graphics)


        For Each row In mRowList

            row.Draw(blockImage, g)

        Next


    End Sub



    ''' <summary>
    ''' Gets the index of the row colliding with the specified object.
    ''' </summary>
    ''' <param name="o">the object with which to test collision</param>
    ''' <returns>index of the row colliding with the object, -1 if no collision takes place</returns>
    ''' <remarks></remarks>
    Public Function GetRowCollidingWith(ByVal o As GameObject) As Integer

        Dim i As Integer
        For i = 0 To UBound(mRowList)

            If mRowList(i).CollidesWith(o) Then
                Return i
            End If

        Next

        Return -1

    End Function




    ''' <summary>
    ''' Constructs a structure describing the current pattern of this blockset. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetPattern() As BlockSetPattern

        Dim pattern As New BlockSetPattern

        Dim emptyCount As Integer = 0


        ReDim pattern.rowData(UBound(mRowList))



        ' loop through each row to count the empty blocks and store the row values
        Dim i As Integer
        For i = 0 To UBound(mRowList)

            emptyCount += mRowList(i).NumEmptyBlocks


            pattern.rowData(i) = mRowList(i).NumInitializedBlocks

        Next


        ' reallocate the array of empty blocks now that we have counted them
        ReDim pattern.emptyBlocks(emptyCount - 1)


        ' loop through each row and store the empty ids
        Dim idBase As Integer = 0

        Dim curPatternEmptyIndex As Integer = 0

        For i = 0 To UBound(mRowList)

            Dim emptyIndices() As Integer = mRowList(i).GetEmptyBlockIndices()

            For Each j In emptyIndices

                pattern.emptyBlocks(curPatternEmptyIndex) = idBase + j
                curPatternEmptyIndex += 1
            Next

            idBase += mBlocksPerRow

        Next



        Return pattern

    End Function



    ''' <summary>
    ''' Loads the blocks into a pattern described by the specified structure.
    ''' </summary>
    ''' <param name="pattern">structure describing the pattern of the set of blocks</param>
    ''' <param name="collideEmptyBlocks">set true to indicate that empty blocks will be collidable</param>
    ''' <remarks></remarks>
    Public Sub LoadPattern(ByVal pattern As BlockSetPattern, Optional ByVal collideEmptyBlocks As Boolean = False)

        mSuppressEvents = True


        ' initialize the rows
        Dim i As Integer
        For i = 0 To UBound(mRowList)

            If i > UBound(pattern.rowData) Then
                SetRowValue(i, 0)
            Else
                SetRowValue(i, pattern.rowData(i))
            End If

        Next


        ' set empty blocks

        For Each id In pattern.emptyBlocks

            If collideEmptyBlocks Then
                PerformActionOnBlock(Block.Action.ToggleEmpty, id)
            Else
                PerformActionOnBlock(Block.Action.Destroy, id)
            End If

        Next


        mSuppressEvents = False

    End Sub




    ''' <summary>
    ''' Performs the specified action on one block colliding with the specified object, if collision occurs.
    ''' </summary>
    ''' <param name="action">the action to perform on a block</param>
    ''' <param name="o">reference to the object with which to test collision</param>
    ''' <remarks></remarks>
    Public Sub PerformActionIfCollides(ByVal action As Block.Action, ByRef o As GameObject)


        ' if the object isn't colliding with the whole blockset then it won't be colliding with any blocks at all
        If Not o.CollidesWith(Me) Then
            Return
        End If


        mActionRequest.action = action
        mActionRequest.collidingObject = o
        mActionRequest.onlyIfCollides = True


        ' loop through each row to try the action and return when an action has succeeded
        Dim i As Integer
        For i = 0 To UBound(mRowList)

            Dim response As BlockRow.BlockActionResponse = mRowList(i).PerformBlockAction(mActionRequest)


            If AnalyzeActionResponse(response, i) Then

                If action = Block.Action.Destroy Then
                    Dim rect As New Rectangle(response.blockCoord, New Size(Breakout.BlockWidth, Breakout.BlockHeight))

                    RaiseEvent BlockDestroyed(o, response.collisionEdge, rect)
                End If

                Return
            End If

        Next


    End Sub




    ''' <summary>
    ''' Performs the specified action on the block specified by the block descriptor.
    ''' </summary>
    ''' <param name="action">the action to perform on a block</param>
    ''' <param name="bd">block descriptor describing the location of the block within the blockset</param>
    ''' <remarks></remarks>
    Public Sub PerformActionOnBlock(ByVal action As Block.Action, ByVal bd As BlockDescriptor)

        mActionRequest.action = action
        mActionRequest.blockIndex = bd.blockIndex
        mActionRequest.collidingObject = Nothing
        mActionRequest.onlyIfCollides = False

        AnalyzeActionResponse(mRowList(bd.rowIndex).PerformBlockAction(mActionRequest), bd.rowIndex)

    End Sub




    ''' <summary>
    ''' Performs the specified action on the block specified by the unique block id.
    ''' </summary>
    ''' <param name="action">the action to perform on a block</param>
    ''' <param name="id">the unique id of the block within the blockset</param>
    ''' <remarks></remarks>
    Public Sub PerformActionOnBlock(ByVal action As Block.Action, ByVal id As Integer)

        mActionRequest.action = action
        mActionRequest.collidingObject = Nothing
        mActionRequest.onlyIfCollides = False


        Dim response As BlockRow.BlockActionResponse

        Dim rowIndex As Integer = GetResponseFromBlockId(mActionRequest, id, response)

        AnalyzeActionResponse(response, rowIndex)

    End Sub


    ''' <summary>
    ''' Gets an action response from the specified unique block id.
    ''' </summary>
    ''' <param name="request">the requested action description</param>
    ''' <param name="id">id of the block</param>
    ''' <param name="response">reference to an action response structure to populate</param>
    ''' <returns>the index of the row containing the block</returns>
    ''' <remarks></remarks>
    Private Function GetResponseFromBlockId(ByVal request As BlockRow.BlockActionRequest, _
                                            ByVal id As Integer, ByRef response As BlockRow.BlockActionResponse) As Integer


        Dim idBase As Integer = 0


        Dim i As Integer
        For i = 0 To UBound(mRowList)

            Dim newBase As Integer = idBase + mBlocksPerRow

            If id >= newBase Then
                ' if we can go through the whole row without finding the block then go to the next row

                idBase = newBase

            Else

                ' otherwise the block is in the current row

                request.blockIndex = id - idBase

                response = mRowList(i).PerformBlockAction(request)

                ' return the row index
                Return i

            End If

        Next

        Return -1

    End Function



    ''' <summary>
    ''' Analyzes an action response and reacts.
    ''' </summary>
    ''' <param name="response">the response to analyze</param>
    ''' <param name="rowIndex">the index of the row from which the response originates</param>
    ''' <returns>true if the action was successful and false otherwise</returns>
    ''' <remarks></remarks>
    Private Function AnalyzeActionResponse(ByVal response As BlockRow.BlockActionResponse, ByVal rowIndex As Integer) As Boolean


        If Not response.succeeded Then
            Return False
        End If


        ' determine the block id
        Dim id As Integer = (mBlocksPerRow * rowIndex) + response.blockIndex


        NumSolidBlocks += response.changeInSolidBlocks


        If response.changeInSolidBlocks < 0 Then

            ' raise an event to let the application know a block has emptied
            If Not mSuppressEvents Then
                RaiseEvent BlockEmptied(response.blockCoord, id)
            End If



        ElseIf response.changeInSolidBlocks > 0 Then

            ' raise an event to let the application know a block has become solid
            If Not mSuppressEvents Then
                RaiseEvent BlockFilled(id)
            End If

        End If


        Return True

    End Function




    ''' <summary>
    ''' Recalculates the blockset's width and height.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CalculateDimensions()

        ' counts the number of rows that are not empty
        Dim numRows As Integer = 0


        Dim i As Integer
        For i = 0 To UBound(mRowList)

            If mRowList(i).Width > Width Then
                Width = mRowList(i).Width
                X = mRowList(i).X
            End If

            If mRowList(i).NumInitializedBlocks > 0 Then
                numRows = i + 1
            End If

        Next


        Height = numRows * mRowList(0).Height


    End Sub


End Class
