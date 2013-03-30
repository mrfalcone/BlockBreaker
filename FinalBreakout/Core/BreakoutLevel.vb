' BreakoutLevel
' ---
' Models a level for a breakout game, including a blockset, blockstyle, background, and high scores.
' 
' Author   : Michael Falcone
' Modified : 4/14/10


Imports System.Xml


Public NotInheritable Class BreakoutLevel


    ''' <summary>
    ''' Raised when a single block becomes empty as the result of a performed action.
    ''' </summary>
    ''' <param name="blockLocation">the location of the emptied block in 2d space</param>
    ''' <param name="blockId">unique id number of the block in the blockset, from [0, MaximumBlocks - 1]</param>
    ''' <remarks></remarks>
    Public Event BlockEmpty(ByVal blockLocation As Point, ByVal blockId As Integer)


    ''' <summary>
    ''' Raised when a single block is destroyed by collision.
    ''' </summary>
    ''' <param name="o">reference to the object that destroyed the block</param>
    ''' <param name="edgeOfCollision">the edge nearest the point of collision, if the block was emptied by collision</param>
    ''' <param name="blockRect">rectangle describing the position and size of the block destroyed</param>
    ''' <remarks></remarks>
    Public Event BlockDestroyed(ByRef o As GameObject, ByVal edgeOfCollision As Block.Edge, ByVal blockRect As Rectangle)



    ''' <summary>
    ''' Raised when an empty block becomes solid as the result of a performed action.
    ''' </summary>
    ''' <param name="blockId">unique id number of the block in the blockset, from [0, MaximumBlocks - 1]</param>
    ''' <remarks></remarks>
    Public Event BlockSolid(ByVal blockId As Integer)


    ''' <summary>
    ''' Raised when the entire blockset becomes empty.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event BlockSetEmpty()



    ' describes the distance from the top of the canvas where the blockset will begin (its Y position)
    Private Const cBlocksetDistanceFromTop As Integer = 30



    ' private member data
    Private WithEvents mBlockSet As BlockSet = Nothing

    Private mLevelName As String = "Untitled Level"

    Private mAuthorName As String = ""

    Private mBackStyle As Integer = 0

    Private mBlockStyle As Integer = 0

    Private mCurrentPattern As BlockSet.BlockSetPattern

    Private mIsInitialized As Boolean = False

    Private mCollideEmptyBlocks As Boolean = False      ' stores whether or not the empty blocks in the blockset will be collidable


    Private mShowBorder As Boolean = True


    Private mBackBrush As TextureBrush


    Private mHighScores As HighScoresCollection


    Private mFilename As String = ""


    ''' <summary>
    ''' Gets or sets the image style index of the level's blocks. Valid values in the range [0, Breakout.BlockImageMaximum].
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property BlockStyle() As Integer
        Get
            Return mBlockStyle
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                mBlockStyle = 0
            ElseIf value > Breakout.BlockImageMaximum Then
                mBlockStyle = Breakout.BlockImageMaximum
            Else
                mBlockStyle = value
            End If
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets a value indicating whether to show a border around the level.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property ShowBorder() As Boolean
        Get
            Return mShowBorder
        End Get
        Set(ByVal value As Boolean)
            mShowBorder = value
        End Set
    End Property



    ''' <summary>
    ''' Gets the name of the file in which the level is stored.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Filename() As String
        Get
            Return mFilename
        End Get
    End Property



    ''' <summary>
    ''' Gets or sets the collection of high scores for this level.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property HighScores() As HighScoresCollection
        Get
            Return mHighScores
        End Get
        Set(ByVal value As HighScoresCollection)
            mHighScores = value
        End Set
    End Property


    ''' <summary>
    ''' Gets or sets the image style index of the background. Valid values in the range [0, Breakout.BackgroundImageMaximum].
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property BackgroundStyle() As Integer
        Get
            Return mBackStyle
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                mBackStyle = 0
            ElseIf value > Breakout.BackgroundImageMaximum Then
                mBackStyle = Breakout.BackgroundImageMaximum
            Else
                mBackStyle = value
            End If

            If mBackBrush IsNot Nothing Then
                mBackBrush.Dispose()
                mBackBrush = Nothing
            End If


        End Set
    End Property



    ''' <summary>
    ''' Provides access to the level's current blockset.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property BlockSet() As BlockSet
        Get
            Return mBlockSet
        End Get
    End Property



    ''' <summary>
    ''' Gets or sets the name of the level.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property LevelName() As String
        Get
            Return mLevelName
        End Get
        Set(ByVal value As String)
            mLevelName = value
        End Set
    End Property



    ''' <summary>
    ''' Gets or sets the name of the level's author.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property AuthorName() As String
        Get
            Return mAuthorName
        End Get
        Set(ByVal value As String)
            mAuthorName = value
        End Set
    End Property



    ''' <summary>
    ''' Overrides the Equals function to return true when both levels are stored to the same file.
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function Equals(ByVal obj As Object) As Boolean

        Try

            Return (CType(obj, BreakoutLevel).Filename = Me.Filename)

        Catch ex As Exception

            Return False

        End Try

    End Function




    ''' <summary>
    ''' Constructs an empty level.
    ''' </summary>
    ''' <param name="collideWithEmptyBlocks">set true to indicate that empty blocks will be collidable</param>
    ''' <remarks></remarks>
    Public Sub New(Optional ByVal collideWithEmptyBlocks As Boolean = False)

        mCollideEmptyBlocks = collideWithEmptyBlocks

        Dim xPos As Integer = Breakout.CanvasWidth \ 2

        mBlockSet = New BlockSet(xPos, cBlocksetDistanceFromTop, Breakout.RowsPerSet, Breakout.BlocksPerRow, _
                                 Breakout.BlockWidth, Breakout.BlockHeight)



    End Sub



    ''' <summary>
    ''' Constructs a new level from the specified level.
    ''' </summary>
    ''' <param name="prototype">level to use as a prototype for this level.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal prototype As BreakoutLevel)

        Me.New(False)


        Dim pattern As BlockSet.BlockSetPattern = prototype.BlockSet.GetPattern()

        Initialize(pattern, prototype.BlockStyle, prototype.BackgroundStyle, prototype.LevelName, prototype.AuthorName)

    End Sub



    ''' <summary>
    ''' Overrides the ToString method to return the name of the level as its string representation.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function ToString() As String
        Return LevelName
    End Function




    ''' <summary>
    ''' Resets the level's blockset to the last initialized pattern.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Reset()

        If mIsInitialized Then
            BlockSet.LoadPattern(mCurrentPattern, mCollideEmptyBlocks)
        Else
            BlockSet.Clear()
        End If

    End Sub




    ''' <summary>
    ''' Generates a preview of the level, showing its background and blockset.
    ''' </summary>
    ''' <param name="width">width of the preview image</param>
    ''' <param name="height">height of the preview image</param>
    ''' <returns>bitmap displaying a preview of the level</returns>
    ''' <remarks></remarks>
    Public Function GeneratePreview(ByVal width As Integer, ByVal height As Integer) As Bitmap

        Dim preview As Bitmap = New Bitmap(Breakout.CanvasWidth, Breakout.CanvasHeight)
        Dim gfx As Graphics = Graphics.FromImage(preview)

        gfx.Clear(Color.Black)

        RenderLevel(gfx)


        Return New Bitmap(preview, width, height)

    End Function




    ''' <summary>
    ''' Renders the level.
    ''' </summary>
    ''' <param name="g">the graphics object used for rendering</param>
    ''' <remarks></remarks>
    Public Sub RenderLevel(ByVal g As Graphics)

        ' render the background
        ' g.DrawImageUnscaledAndClipped(Breakout.GetBackgroundImage(BackgroundStyle), Breakout.CanvasBounds)

        If mBackBrush Is Nothing Then
            mBackBrush = New TextureBrush(Breakout.GetBackgroundImage(BackgroundStyle))
        End If

        g.FillRectangle(mBackBrush, Breakout.CanvasBounds)


        ' render the blockset
        BlockSet.Draw(Breakout.GetBlockImage(BlockStyle), g)

        If ShowBorder Then

            Static borderBounds As New Rectangle(1, 1, Breakout.CanvasWidth - 2, Breakout.CanvasHeight - 2)
            Static borderPen As New Pen(Color.Black, 2)

            g.DrawRectangle(borderPen, borderBounds)
        End If

    End Sub




    ''' <summary>
    ''' Initialize the level properties to the specified values.
    ''' </summary>
    ''' <param name="setPattern">the pattern to be displayed by the level's blockset</param>
    ''' <param name="blockStyle">the image style index of the blocks</param>
    ''' <param name="backStyle">the image style index of the background</param>
    ''' <param name="levelName">the name of the level</param>
    ''' <param name="authorName">the name of the author</param>
    ''' <remarks></remarks>
    Public Sub Initialize(ByVal setPattern As BlockSet.BlockSetPattern, ByVal blockStyle As Integer, _
                          ByVal backStyle As Integer, ByVal levelName As String, ByVal authorName As String)

        mCurrentPattern = setPattern

        Me.BlockStyle = blockStyle
        Me.BackgroundStyle = backStyle
        Me.LevelName = levelName
        Me.AuthorName = authorName

        mIsInitialized = True

        Reset()

    End Sub



    ''' <summary>
    ''' Saves the level to an XML file with the specified filename.
    ''' </summary>
    ''' <param name="filename">absolute filename of the file to save</param>
    ''' <param name="saveHighscores">set true to indicate that the high scores will be saved with the level</param>
    ''' <remarks></remarks>
    Public Sub SaveToXML(ByVal filename As String, Optional ByVal saveHighscores As Boolean = False)


        Static currentPattern As BlockSet.BlockSetPattern
        currentPattern = BlockSet.GetPattern()



        Static settings As New XmlWriterSettings
        settings.Indent = True
        settings.OmitXmlDeclaration = True




        Using writer As XmlWriter = XmlWriter.Create(filename, settings)

            writer.WriteStartElement("Breakout")
            writer.WriteAttributeString("version", Application.ProductVersion)



            ' <Level> tag
            writer.WriteStartElement("Level")

            writer.WriteStartAttribute("blockStyle")
            writer.WriteValue(BlockStyle)
            writer.WriteEndAttribute()

            writer.WriteStartAttribute("backStyle")
            writer.WriteValue(BackgroundStyle)
            writer.WriteEndAttribute()


            ' <Name> and <Author> tags
            writer.WriteElementString("Name", LevelName)
            writer.WriteElementString("Author", AuthorName)


            ' write row data
            writer.WriteStartElement("RowData")
            writer.WriteValue(currentPattern.rowData)
            writer.WriteEndElement()


            ' write empty blocks
            writer.WriteStartElement("EmptyBlocks")


            writer.WriteValue(currentPattern.emptyBlocks)
            writer.WriteEndElement()



            ' write high scores
            If saveHighscores Then
                writer.WriteElementString("HighScores", mHighScores.ToString())
            End If



            ' end <Level> tag
            writer.WriteEndElement()

            ' end <Breakout> tag
            writer.WriteEndElement()


            writer.Flush()

            writer.Close()

        End Using


        mFilename = filename

    End Sub



    ''' <summary>
    ''' Loads the level from an XML file with the specified filename.
    ''' </summary>
    ''' <param name="filename">absolute filename of the file to load</param>
    ''' <remarks></remarks>
    Public Sub LoadFromXML(ByVal filename As String)

        Static version As String
        Static blockStyle As Integer
        Static backStyle As Integer
        Static name As String
        Static author As String
        Static pattern As BlockSet.BlockSetPattern



        Static settings As New XmlReaderSettings
        settings.IgnoreWhitespace = True


        ' read all the level values
        Using reader As XmlReader = XmlReader.Create(filename, settings)

            reader.MoveToContent()

            ' check version
            version = reader.GetAttribute("version")

            ' if trying to read a level for a newer version then throw an exception
            If String.Compare(Application.ProductVersion, version) < 0 Then
                Throw New Exception("Version not supported.")
            End If


            ' read level styles
            reader.ReadToFollowing("Level")

            blockStyle = XmlConvert.ToInt32(reader.GetAttribute("blockStyle"))
            backStyle = XmlConvert.ToInt32(reader.GetAttribute("backStyle"))



            ' read names
            reader.ReadToFollowing("Name")

            name = reader.ReadElementString("Name")
            author = reader.ReadElementString("Author")


            ' load pattern data
            pattern.rowData = CType(reader.ReadElementContentAs(GetType(Integer()), Nothing), Integer())
            pattern.emptyBlocks = CType(reader.ReadElementContentAs(GetType(Integer()), Nothing), Integer())



            ' attempt to read in high scores
            Try

                Dim s As String = reader.ReadElementString("HighScores")
                HighScores = New HighScoresCollection(s, Breakout.MaxHighScoresPerLevel)

            Catch ex As Exception

                HighScores = New HighScoresCollection(Breakout.MaxHighScoresPerLevel)

            End Try


            reader.Close()

        End Using


        Initialize(pattern, blockStyle, backStyle, name, author)

        mFilename = filename

    End Sub




    ''' <summary>
    ''' Reloads the blockset pattern and saves the specified collection of high scored to the level's file.
    ''' </summary>
    ''' <param name="scores">the collection of scores to be saved</param>
    ''' <remarks></remarks>
    Public Sub SaveHighScores(ByVal scores As HighScoresCollection)

        If mFilename = "" Then
            Return
        End If

        Reset()
        mHighScores = scores

        SaveToXML(mFilename, True)

    End Sub





    ''' <summary>
    ''' Raises event when a block is destroyed.
    ''' </summary>
    ''' <param name="o"></param>
    ''' <param name="edgeOfCollision"></param>
    ''' <param name="blockRect"></param>
    ''' <remarks></remarks>
    Private Sub mBlockSet_BlockDestroyed(ByRef o As GameObject, ByVal edgeOfCollision As GameObject.Edge, ByVal blockRect As Rectangle) Handles mBlockSet.BlockDestroyed
        RaiseEvent BlockDestroyed(o, edgeOfCollision, blockRect)
    End Sub




    ''' <summary>
    ''' Raises events when a block becomes empty and when all blocks are empty.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub mBlockSet_BlockEmptied(ByVal location As Point, ByVal blockId As Integer) Handles mBlockSet.BlockEmptied

        RaiseEvent BlockEmpty(location, blockId)

        If BlockSet.NumSolidBlocks <= 0 Then
            RaiseEvent BlockSetEmpty()
        End If

    End Sub


    ''' <summary>
    ''' Raises an event when an empty block becomes solid.
    ''' </summary>
    ''' <param name="blockId"></param>
    ''' <remarks></remarks>
    Private Sub mBlockSet_BlockFilled(ByVal blockId As Integer) Handles mBlockSet.BlockFilled
        RaiseEvent BlockSolid(blockId)
    End Sub


End Class
