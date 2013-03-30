' Powerup
' ---
' Abstract class inheriting MoveableObject to model a general powerup in a breakout game.
' 
' Author   : Michael Falcone
' Modified : 4/19/10


Public MustInherit Class Powerup
    Inherits MoveableObject


    ''' <summary>
    ''' Raised when the powerup becomes activated. This will not be raised if there is no time limit for the powerup.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event Activated()


    ''' <summary>
    ''' Raised each second when the powerup timer ticks.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event TimerTick()


    ''' <summary>
    ''' Raised when the timer is up and the powerup must be deactivated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event RequiresDeactivation()


    ''' <summary>
    ''' Raised when the powerup is deactivated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event Deactivated()




    ''' <summary>
    ''' The powerups that the player may collect.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum PowerupType As Integer
        ExtraBall = 0
        GrowPaddle = 1
        ShrinkPaddle = 2
        SpeedUp = 3
    End Enum




    ' private member variables
    Private mType As PowerupType

    Private mHasTimeLimit As Boolean = False

    Private mTimeRemaining As Double = 0


    Private mTimeLimit As Double = 0


    Private mIsActivated As Boolean = False

    Private mPointValue As Integer = 0



    ''' <summary>
    ''' Gets the type of this powerup.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property Type() As PowerupType
        Get
            Return mType
        End Get
    End Property



    ''' <summary>
    ''' Gets a value indicating whether the powerup has a time limit.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property HasTimeLimit() As Boolean
        Get
            Return mHasTimeLimit
        End Get
    End Property



    ''' <summary>
    ''' Gets the number of seconds remaining for the powerup.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property TimeRemaining() As Double
        Get
            Return mTimeRemaining
        End Get
    End Property




    ''' <summary>
    ''' Gets the number of points to be awarded for capturing the powerup.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property PointValue() As Integer
        Get
            Return mPointValue
        End Get
    End Property



    ''' <summary>
    ''' Gets the time limit of the powerup.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property TimeLimit() As Double
        Get
            Return mTimeLimit
        End Get
    End Property



    ''' <summary>
    ''' Gets a value indicating whether the powerup is active.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property IsActivated() As Boolean
        Get
            Return mIsActivated
        End Get
    End Property




    ''' <summary>
    ''' Constructs a new powerup of the specified type.
    ''' </summary>
    ''' <param name="type">the type of this powerup</param>
    ''' <param name="timeLimit">the number of seconds the powerup will stay active, if 0 then the powerup does not stay active</param>
    ''' <param name="pointValue">the number of points the powerup is worth</param>
    ''' <remarks></remarks>
    Protected Sub New(ByVal type As PowerupType, ByVal timeLimit As Double, ByVal pointValue As Integer)

        MyBase.New(0, 0, Breakout.PowerupDropSpeed)

        mType = type

        With Breakout.GetPowerupImage(mType)

            Me.Width = .Width
            Me.Height = .Height

        End With

        Visible = False


        If timeLimit <= 0 Then
            mHasTimeLimit = False
        Else
            mHasTimeLimit = True
            mTimeLimit = timeLimit
        End If


        mPointValue = pointValue


    End Sub




    ''' <summary>
    ''' Creates a new powerup of the specified type.
    ''' </summary>
    ''' <param name="type">the type of powerup to create</param>
    ''' <returns>a powerup object of the specified type</returns>
    ''' <remarks></remarks>
    Public Shared Function Create(ByVal type As PowerupType) As Powerup

        Select Case type

            Case PowerupType.ExtraBall
                Return New ExtraBallPowerup

            Case PowerupType.GrowPaddle
                Return New GrowPaddlePowerup

            Case PowerupType.ShrinkPaddle
                Return New ShrinkPaddlePowerup

            Case PowerupType.SpeedUp
                Return New SpeedUpPowerup

        End Select


        Throw New Exception("Invalid powerup type")

    End Function




    ''' <summary>
    ''' Creates a new powerup of a random type.
    ''' </summary>
    ''' <returns>a powerup object of a random type</returns>
    ''' <remarks></remarks>
    Public Shared Function CreateRandom() As Powerup

        Return Create(CType(Breakout.GetRandomInt(3), PowerupType))

    End Function




    ''' <summary>
    ''' Overrides the Reset method to be sure the powerup stays hidden.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub Reset()
        MyBase.Reset()

        Hide()

    End Sub




    ''' <summary>
    ''' Shadows the draw method to draw the correct powerup image.
    ''' </summary>
    ''' <param name="g"></param>
    ''' <remarks></remarks>
    Public Shadows Sub Draw(ByVal g As System.Drawing.Graphics)

        If Visible Then
            g.DrawImageUnscaledAndClipped(Breakout.GetPowerupImage(mType), BoundRectangle)
        End If

    End Sub



    ''' <summary>
    ''' Overrides the update method to update the timer if necessary.
    ''' </summary>
    ''' <param name="secsSinceLastFrame"></param>
    ''' <remarks></remarks>
    Public Overrides Sub Update(ByVal secsSinceLastFrame As Double)

        Static secondTimer As Double = 0


        secondTimer += secsSinceLastFrame

        If secondTimer >= 1 Then
            secondTimer = 0
            RaiseEvent TimerTick()
        End If


        If IsActivated And HasTimeLimit Then
            UpdateTimer(secsSinceLastFrame)
        ElseIf Visible Then
            MyBase.Update(secsSinceLastFrame)
        End If

    End Sub




    ''' <summary>
    ''' Activates the powerup.
    ''' </summary>
    ''' <param name="game">reference to the game object activating the powerup</param>
    ''' <remarks></remarks>
    Public Overridable Sub Activate(ByRef game As BreakoutGame)

        If HasTimeLimit Then

            mTimeRemaining = mTimeLimit
            mIsActivated = True

            RaiseEvent Activated()
        End If

    End Sub




    ''' <summary>
    ''' Deactivates the powerup.
    ''' </summary>
    ''' <param name="game">reference to the game object deactivating the powerup</param>
    ''' <remarks></remarks>
    Public Overridable Sub Deactivate(ByRef game As BreakoutGame)

        mIsActivated = False
        mTimeRemaining = 0
        RaiseEvent Deactivated()

    End Sub





    ''' <summary>
    ''' Updates the timer and deactivates the powerup if necessary.
    ''' </summary>
    ''' <param name="secsSinceLastFrame"></param>
    ''' <remarks></remarks>
    Private Sub UpdateTimer(ByVal secsSinceLastFrame As Double)

        mTimeRemaining -= secsSinceLastFrame


        If mTimeRemaining <= 0 Then
            mTimeRemaining = 0
            mIsActivated = False
            RaiseEvent RequiresDeactivation()
        End If

    End Sub




End Class
