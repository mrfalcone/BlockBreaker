' HighScoresCollection
' ---
' Models a collection of high scores sorted by score value.
' 
' Author   : Michael Falcone
' Modified : 4/13/10


Public NotInheritable Class HighScoresCollection



    ''' <summary>
    ''' Describes a high score entry.
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure HighScore

        ''' <summary>
        ''' The name of the score holder.
        ''' </summary>
        ''' <remarks></remarks>
        Dim ScoreHolder As String

        ''' <summary>
        ''' The high score value.
        ''' </summary>
        ''' <remarks></remarks>
        Dim ScoreValue As Integer

        ''' <summary>
        ''' Constructs a new high score.
        ''' </summary>
        ''' <param name="holder">the name of the score holder</param>
        ''' <param name="value">the value of the high score</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal holder As String, ByVal value As Integer)
            ScoreHolder = holder
            ScoreValue = value
        End Sub

    End Structure



    ' constant data
    Private Const cDefaultScoreHolder As String = "---"


    ' private member data

    Dim mHighScores() As HighScore

    Dim mIsEmpty As Boolean = True





    ''' <summary>
    ''' Returns true if no scores have been added to the collection, false otherwise.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Empty() As Boolean
        Get
            Return mIsEmpty
        End Get
    End Property



    ''' <summary>
    ''' Provides access to the array of high score objects.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Scores() As HighScore()
        Get
            Return mHighScores
        End Get
    End Property



    ''' <summary>
    ''' Gets the maximum index of the scores in the high scores collection.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property MaxScoreIndex() As Integer
        Get
            Return UBound(mHighScores)
        End Get
    End Property



    ''' <summary>
    ''' Gets the number of scores in the high scores collection.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property NumScores() As Integer
        Get
            Return mHighScores.Count()
        End Get
    End Property





    ''' <summary>
    ''' Constructs an empty collection of high scores.
    ''' </summary>
    ''' <param name="maxScores">the maximum number of scores the collection will hold</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal maxScores As Integer)

        If maxScores < 0 Then
            Throw New Exception("maxScores invalid")
        End If


        ReDim mHighScores(maxScores - 1)


        Clear()


    End Sub



    ''' <summary>
    ''' Constructs a collection of high scores from the specified string.
    ''' </summary>
    ''' <param name="s">the string from which to construct the high scores collection</param>
    ''' <param name="maxScores">the maximum number of high scores the collection will hold. if -1,
    ''' the maximum number will be the number of high scores in the specified string.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal s As String, Optional ByVal maxScores As Integer = -1)


        If maxScores > 0 Then
            ReDim mHighScores(maxScores - 1)
            Clear()

        ElseIf maxScores <= 0 And maxScores <> -1 Then
            Throw New Exception("maxScores invalid")

        End If


        ' split the string into high score expressions
        Dim expressions() As String = Split(s, ",", maxScores)


        If maxScores = -1 Then
            ReDim mHighScores(UBound(expressions))
        End If


        ' initialize the high scores

        Dim maxIndex As Integer = UBound(mHighScores)

        If UBound(expressions) < maxIndex Then
            maxIndex = UBound(expressions)
        End If


        Dim i As Integer
        For i = 0 To maxIndex
            ' loop through each expression and parse it to create a high score entry

            Dim scoreExpressions() As String = Split(expressions(i), ":")

            mHighScores(i).ScoreHolder = scoreExpressions(0)
            mHighScores(i).ScoreValue = Integer.Parse(scoreExpressions(1))


            If mHighScores(i).ScoreHolder <> cDefaultScoreHolder Or mHighScores(i).ScoreValue > 0 Then
                mIsEmpty = False
            End If

        Next

    End Sub



    ''' <summary>
    ''' Clears the list of scores to default values.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Clear()

        Dim i As Integer
        For i = 0 To UBound(mHighScores)
            mHighScores(i).ScoreHolder = cDefaultScoreHolder
            mHighScores(i).ScoreValue = 0
        Next

        mIsEmpty = True

    End Sub




    ''' <summary>
    ''' Overrides the ToString method to provide a string representation of the collection of high scores.
    ''' </summary>
    ''' <returns>a string representation of the high scores</returns>
    ''' <remarks></remarks>
    Public Overrides Function ToString() As String

        Dim s As String = ""

        For Each score In mHighScores

            s += score.ScoreHolder + ":" + score.ScoreValue.ToString() + ","

        Next

        ' remove the trailing comma
        s = s.Substring(0, s.Length - 1)

        Return s

    End Function



    ''' <summary>
    ''' Constructs a new high scores collection from a string.
    ''' </summary>
    ''' <param name="s">the string from which to construct a high scores collection</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function FromString(ByVal s As String) As HighScoresCollection

        Return New HighScoresCollection(s)

    End Function




    ''' <summary>
    ''' Tests whether the specified score is a high score.
    ''' </summary>
    ''' <param name="score">the score to test</param>
    ''' <returns>true if the score is a high score, false otherwise</returns>
    ''' <remarks></remarks>
    Public Function IsHighScore(ByVal score As Integer) As Boolean

        ' if the specified score is greater than the last score in the collection then it in a high score
        Return (score > mHighScores(UBound(mHighScores)).ScoreValue)

    End Function





    ''' <summary>
    ''' Adds a new entry in the high scores collection with the specified score value.
    ''' </summary>
    ''' <param name="scoreValue">value of the high score to add</param>
    ''' <returns>the index in the collection of the new high score, or -1 if the score is not a high score</returns>
    ''' <remarks></remarks>
    Public Function AddNew(ByVal scoreValue As Integer) As Integer

        Return AddNew(New HighScore(cDefaultScoreHolder, scoreValue))

    End Function




    ''' <summary>
    ''' Adds the specified high score to the collection of high scores.
    ''' </summary>
    ''' <param name="highScore"></param>
    ''' <returns>the index in the collection of the new high score, or -1 if the score is not a high score</returns>
    ''' <remarks></remarks>
    Public Function AddNew(ByVal highScore As HighScore) As Integer

        If Not IsHighScore(highScore.ScoreValue) Then
            Return -1
        End If


        Dim newScoreIndex As Integer = -1

        Dim tempScores(UBound(mHighScores)) As HighScore

        Dim curScoreIndex As Integer = 0

        Dim scorePlaced As Boolean = False  ' becomes true when the high score is placed in the array

        Dim i As Integer
        For i = 0 To UBound(tempScores)

            Dim curScore As HighScore = mHighScores(curScoreIndex)

            ' if the new score is higher than this score then replace it
            If highScore.ScoreValue > curScore.ScoreValue AndAlso scorePlaced = False Then

                tempScores(i) = highScore
                newScoreIndex = i

                scorePlaced = True

            Else
                curScoreIndex += 1
                tempScores(i) = curScore

            End If

        Next


        If scorePlaced Then
            mIsEmpty = False

            Array.Copy(tempScores, mHighScores, tempScores.Length)

        End If



        Return newScoreIndex

    End Function


End Class
