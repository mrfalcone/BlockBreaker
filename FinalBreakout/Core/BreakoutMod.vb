' Breakout
' ---
' Provides methods, constant properties, and images for a breakout game.
' 
' Author   : Michael Falcone
' Modified : 4/17/10


Module Breakout




    ' private constants
    Private Const cPointsPerBlock As Integer = 25
    Private Const cMaxPointMultiplier As Integer = 12

    Private Const cMaxBallSpeedMultiplier As Integer = 2
    Private Const cMaxBallStep As Double = 0.5

    Private Const cStartBallCount As Integer = 3

    Private Const cMaxRowsPerSet As Integer = 10        ' the maximum number of rows in a blockset
    Private Const cMaxBlocksPerRow As Integer = 14      ' the maximum number of blocks in each row in a blockset






    ' directory locations, relative to the application's directory
    Private Const cHighScoresFilename As String = "Data\HighScores.dat"

    Private Const cPaddleFilename As String = "Data\images\paddle.png"
    Private Const cBallFilename As String = "Data\images\ball.png"

    Private Const cBlockImageDir As String = "Data\images\blocks"
    Private Const cBackImageDir As String = "Data\images\backgrounds"

    Private Const cCustomLevelDir As String = "Data\levels\custom"
    Private Const cDefaultLevelDir As String = "Data\levels\default"


    Private Const cBallSpeed As Double = 220            ' movement speed of the ball in pixels per second


    ' powerup constants
    Private Const cPowerupImageDir As String = "Data\images\powerups"

    Private Const cExtraBallImageFilename As String = "extraball.png"
    Private Const cGrowPaddleImageFilename As String = "growpaddle.png"
    Private Const cShrinkPaddleImageFilename As String = "shrinkpaddle.png"
    Private Const cSpeedUpImageFilename As String = "speedup.png"



    Private Const cPowerupDropSpeed As Double = 100

    Private Const cPowerupDropRate As Double = 0.3      ' droprate percentage of a random powerup



    ' private module variables
    Private mAppDirectory As String

    Private mDefaultLevels() As BreakoutLevel
    Private mCustomLevels() As BreakoutLevel


    Private mIsInitialized As Boolean = False


    Private mBlockWidth As Integer = 0
    Private mBlockHeight As Integer = 0

    Private mCanvasBounds As Rectangle


    Private mBlockImages() As Bitmap = Nothing
    Private mBackImages() As Bitmap = Nothing
    Private mPaddleImage As Bitmap = Nothing
    Private mBallImage As Bitmap = Nothing

    Private mPowerupImages() As Bitmap = Nothing



    ''' <summary>
    ''' Gets the absolute path to the custom levels directory.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property CustomLevelsDirectory() As String
        Get
            Return mAppDirectory + "\" + cCustomLevelDir
        End Get
    End Property



    ''' <summary>
    ''' Gets the absolute path of the default levels directory.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property DefaultLevelsDirectory() As String
        Get
            Return mAppDirectory + "\" + cDefaultLevelDir
        End Get
    End Property




    ''' <summary>
    ''' Gets the maximum step of the ball.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property MaxBallStep() As Double
        Get
            Return cMaxBallStep
        End Get
    End Property



    ''' <summary>
    ''' Gets the number of balls to start with each game.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property StartBallCount() As Integer
        Get
            Return cStartBallCount
        End Get
    End Property



    ''' <summary>
    ''' Gets the drop rate percentage of a random powerup.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property PowerupDropRate() As Double
        Get
            Return cPowerupDropRate
        End Get
    End Property



    ''' <summary>
    ''' Gets the highest speed multiplier the ball may gain.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property MaximumBallSpeedMultiplier() As Integer
        Get
            Return cMaxBallSpeedMultiplier
        End Get
    End Property


    ''' <summary>
    ''' Gets the default number of points to be given when a block is destroyed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property PointsPerBlock() As Integer
        Get
            Return cPointsPerBlock
        End Get
    End Property



    ''' <summary>
    ''' Gets the speed of the powerups as they are dropping.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property PowerupDropSpeed() As Double
        Get
            Return cPowerupDropSpeed
        End Get
    End Property



    ''' <summary>
    ''' Gets the maximum number that the score multiplier can be.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property ScoreMultiplierMaximum() As Integer
        Get
            Return cMaxPointMultiplier
        End Get
    End Property



    ''' <summary>
    ''' Gets the maximum number of high scores stored by each level.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property MaxHighScoresPerLevel() As Integer
        Get
            Return HighScoresForm.MaxHighScoresCustom
        End Get
    End Property



    ''' <summary>
    ''' Gets the maximum index of available block images.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property BlockImageMaximum() As Integer
        Get
            Return UBound(mBlockImages)
        End Get
    End Property




    ''' <summary>
    ''' Gets the maximum index of available background images.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property BackgroundImageMaximum() As Integer
        Get
            Return UBound(mBackImages)
        End Get
    End Property




    ''' <summary>
    ''' Gets the number of blocks per blockrow.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property BlocksPerRow() As Integer
        Get
            Return cMaxBlocksPerRow
        End Get
    End Property



    ''' <summary>
    ''' Gets the number of rows per blockset.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property RowsPerSet() As Integer
        Get
            Return cMaxRowsPerSet
        End Get
    End Property



    ''' <summary>
    ''' Gets the width of the block objects.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property BlockWidth() As Integer
        Get
            Return mBlockWidth
        End Get
    End Property




    ''' <summary>
    ''' Gets the height of the block objects.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property BlockHeight() As Integer
        Get
            Return mBlockHeight
        End Get
    End Property



    ''' <summary>
    ''' Gets a rectangle describing the boundaries of the game canvas.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property CanvasBounds() As Rectangle
        Get
            Return mCanvasBounds
        End Get
    End Property



    ''' <summary>
    ''' Gets the width of the game canvas.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property CanvasWidth() As Integer
        Get
            Return mCanvasBounds.Width
        End Get
    End Property



    ''' <summary>
    ''' Gets the height of the game canvas.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property CanvasHeight() As Integer
        Get
            Return mCanvasBounds.Height
        End Get
    End Property




    ''' <summary>
    ''' Gets the movement speed of the ball in pixels per second.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property BallSpeed() As Double
        Get
            Return cBallSpeed
        End Get
    End Property




    ''' <summary>
    ''' Gets the block image at the specified index.
    ''' </summary>
    ''' <param name="index">index of the block image</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBlockImage(ByVal index As Integer) As Bitmap

        If mIsInitialized Then
            Return mBlockImages(index)
        End If

        Return Nothing

    End Function



    ''' <summary>
    ''' Gets the background image at the specified index.
    ''' </summary>
    ''' <param name="index">index of the background image</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBackgroundImage(ByVal index As Integer) As Bitmap

        If mIsInitialized Then
            Return mBackImages(index)
        End If

        Return Nothing

    End Function



    ''' <summary>
    ''' Gets the image to be used for the paddle.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPaddleImage() As Bitmap

        If mIsInitialized Then
            Return mPaddleImage
        End If

        Return Nothing

    End Function



    ''' <summary>
    ''' Gets the image to be used for the ball.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBallImage() As Bitmap

        If mIsInitialized Then
            Return mBallImage
        End If

        Return Nothing

    End Function




    ''' <summary>
    ''' Gets the image of the specified powerup.
    ''' </summary>
    ''' <param name="powerup"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPowerupImage(ByVal powerup As Powerup.PowerupType) As Bitmap

        Try
            Return mPowerupImages(powerup)
        Catch ex As Exception
            Return Nothing
        End Try

    End Function



    ''' <summary>
    ''' Gets an array of default game levels for a normal game session.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDefaultLevels() As BreakoutLevel()

        If mIsInitialized Then

            Return mDefaultLevels

        End If


        Return Nothing

    End Function



    ''' <summary>
    ''' Gets an array of custom game levels for a custom game session.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCustomLevels() As BreakoutLevel()

        If mIsInitialized Then

            ' update the custom levels in case were added
            LoadCustomLevels()

            Return mCustomLevels

        End If


        Return Nothing

    End Function




    ''' <summary>
    ''' Returns the collection of high scores for a normal game session.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetGameHighScores() As HighScoresCollection

        Try
            Return HighScoresCollection.FromString(My.Computer.FileSystem.ReadAllText(mAppDirectory + "\" + cHighScoresFilename))

        Catch ex As Exception

            Return New HighScoresCollection(HighScoresForm.MaxHighScoresNormal)

        End Try

    End Function


    ''' <summary>
    ''' Saves the collection of high scores for a normal game session.
    ''' </summary>
    ''' <param name="scores"></param>
    ''' <remarks></remarks>
    Public Sub SaveGameHighScores(ByVal scores As HighScoresCollection)

        Try

            My.Computer.FileSystem.WriteAllText(mAppDirectory + "\" + cHighScoresFilename, scores.ToString(), False)

        Catch ex As Exception
            MsgBox("High scores cannot be saved: " + ex.ToString(), MsgBoxStyle.Critical)
        End Try


    End Sub



    ''' <summary>
    ''' Returns a random integer between and including the min and max values.
    ''' </summary>
    ''' <param name="max">the maximum value that can be returned</param>
    ''' <param name="min">the minimum value that can be returned</param>
    ''' <returns>a random number between the min and max values</returns>
    ''' <remarks></remarks>
    Public Function GetRandomInt(ByVal max As Integer, Optional ByVal min As Integer = 0) As Integer

        Static rand As New Random()

        Return rand.Next(min, max + 1)

    End Function




    ''' <summary>
    ''' Gets a random boolean value having the specified chance to be true.
    ''' </summary>
    ''' <param name="trueChance">the chance for the boolean to be true as a percentage, must be less than 1 and greater than 0</param>
    ''' <returns>a random boolean number</returns>
    ''' <remarks></remarks>
    Public Function GetRandomBool(ByVal trueChance As Double) As Boolean

        If trueChance >= 1 Or trueChance <= 0 Then
            Throw New Exception("Invalid parameters")
        End If


        Dim randomNum As Integer = GetRandomInt(100, 1)

        If randomNum < trueChance * 100 Then
            Return True
        End If


        Return False

    End Function




    ''' <summary>
    ''' Initialize the module. This must be called before the module is used.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()

        ' if the module has already been initialized then return
        If mIsInitialized Then
            Return
        End If


        mAppDirectory = CurDir()


        ' attempt to initialize all the image files

        Try
            InitBlocks()
        Catch ex As Exception
            MsgBox("Cannot initialize blocks: " + ex.ToString(), MsgBoxStyle.Critical)
            End
        End Try


        Try
            InitBackground()
        Catch ex As Exception
            MsgBox("Cannot initialize backgrounds: " + ex.ToString(), MsgBoxStyle.Critical)
            End
        End Try



        Try
            InitPowerups()
        Catch ex As Exception
            MsgBox("Cannot initialize powerups: " + ex.ToString(), MsgBoxStyle.Critical)
            End
        End Try


        Try
            mPaddleImage = New Bitmap(mAppDirectory + "\" + cPaddleFilename)
            mBallImage = New Bitmap(mAppDirectory + "\" + cBallFilename)
        Catch ex As Exception
            MsgBox("Cannot initialize images: " + ex.ToString(), MsgBoxStyle.Critical)
            End
        End Try



        Try
            LoadDefaultLevels()
            LoadCustomLevels()
        Catch ex As Exception
            MsgBox("Cannot initialize levels: " + ex.ToString(), MsgBoxStyle.Critical)
            End
        End Try






        mIsInitialized = True

    End Sub


    ''' <summary>
    ''' Initializes powerup images.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitPowerups()

        ReDim mPowerupImages(3)

        mPowerupImages(Powerup.PowerupType.GrowPaddle) = New Bitmap(mAppDirectory + "\" + cPowerupImageDir + "\" + cGrowPaddleImageFilename)
        mPowerupImages(Powerup.PowerupType.ExtraBall) = New Bitmap(mAppDirectory + "\" + cPowerupImageDir + "\" + cExtraBallImageFilename)
        mPowerupImages(Powerup.PowerupType.ShrinkPaddle) = New Bitmap(mAppDirectory + "\" + cPowerupImageDir + "\" + cShrinkPaddleImageFilename)
        mPowerupImages(Powerup.PowerupType.SpeedUp) = New Bitmap(mAppDirectory + "\" + cPowerupImageDir + "\" + cSpeedUpImageFilename)

    End Sub




    ''' <summary>
    ''' Initialize the block images and properties.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitBlocks()

        Dim dirInfo As New IO.DirectoryInfo(mAppDirectory + "\" + cBlockImageDir)

        Dim fileList() As IO.FileInfo = dirInfo.GetFiles("*.png")


        ReDim mBlockImages(UBound(fileList))

        ' loop through each file and load it into the image array
        Dim i As Integer
        For i = 0 To UBound(fileList)

            mBlockImages(i) = New Bitmap(fileList(i).FullName)

        Next


        ' set the dimensions of the block objects to the first image's dimensions
        mBlockHeight = mBlockImages(0).Height
        mBlockWidth = mBlockImages(0).Width

    End Sub



    ''' <summary>
    ''' Initializes the background images and canvas properties.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitBackground()

        Dim dirInfo As New IO.DirectoryInfo(mAppDirectory + "\" + cBackImageDir)

        Dim fileList() As IO.FileInfo = dirInfo.GetFiles("*.png")


        ReDim mBackImages(UBound(fileList))

        ' loop through each file and load it into the image array
        Dim i As Integer
        For i = 0 To UBound(fileList)

            mBackImages(i) = New Bitmap(fileList(i).FullName)

        Next


        ' set the dimensions of the canvas to the first image's dimensions
        mCanvasBounds.X = 0
        mCanvasBounds.Y = 0
        mCanvasBounds.Height = mBackImages(0).Height
        mCanvasBounds.Width = mBackImages(0).Width

    End Sub



    ''' <summary>
    ''' Loads the default game levels into the array.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadDefaultLevels()

        Static levelDir As String = DefaultLevelsDirectory

        Dim dirInfo As New IO.DirectoryInfo(levelDir)

        Dim xmlList() As IO.FileInfo = dirInfo.GetFiles("*.xml")

        Dim tempArray(UBound(xmlList)) As BreakoutLevel


        Dim curIndex As Integer = 0


        ' loop through each xml file and attempt to load it into the temporary array
        For Each file In xmlList


            Try
                tempArray(curIndex) = New BreakoutLevel
                tempArray(curIndex).LoadFromXML(file.FullName)

                curIndex += 1

            Catch ex As Exception
                Continue For
            End Try

        Next

        If curIndex = 0 Then
            mDefaultLevels = Nothing

            Throw New Exception("Default levels cannot be loaded.")
            Return
        End If


        ReDim mDefaultLevels(curIndex - 1)
        Array.Copy(tempArray, mDefaultLevels, curIndex)


    End Sub


    ''' <summary>
    ''' Loads the custom levels into the array.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadCustomLevels()

        Static levelDir As String = CustomLevelsDirectory

        Static dirInfo As New IO.DirectoryInfo(levelDir)

        dirInfo.Refresh()

        Dim xmlList() As IO.FileInfo = dirInfo.GetFiles("*.xml")


        Dim tempArray(UBound(xmlList)) As BreakoutLevel

        Dim curIndex As Integer = 0


        ' loop through each xml file and attempt to load it into the temporary array
        For Each file In xmlList


            Try
                tempArray(curIndex) = New BreakoutLevel
                tempArray(curIndex).LoadFromXML(file.FullName)

                If tempArray(curIndex).BlockSet.NumSolidBlocks > 0 Then
                    curIndex += 1
                End If


            Catch ex As Exception
                Continue For
            End Try

        Next

        If curIndex = 0 Then
            mCustomLevels = Nothing
            Return
        End If

        ReDim mCustomLevels(curIndex - 1)
        Array.Copy(tempArray, mCustomLevels, curIndex)


    End Sub


End Module
