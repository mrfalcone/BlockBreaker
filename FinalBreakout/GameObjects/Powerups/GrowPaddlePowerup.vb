' GrowPaddlePowerup
' ---
' Inherits Powerup to model a powerup causing the paddle to grow.
' 
' Author   : Michael Falcone
' Modified : 4/20/10



Public NotInheritable Class GrowPaddlePowerup
    Inherits Powerup


    ' constants
    Private Const cTimeLimit As Double = 10
    Private Const cPointValue As Integer = 20

    Private Const cScaleFactor As Single = 1.5




    ''' <summary>
    ''' Constructs a new GrowPaddle powerup.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New(PowerupType.GrowPaddle, cTimeLimit, cPointValue)
    End Sub





    ''' <summary>
    ''' Overrides the Activate method to activate the powerup.
    ''' </summary>
    ''' <param name="game">reference to the game object activating the powerup</param>
    ''' <remarks></remarks>
    Public Overrides Sub Activate(ByRef game As BreakoutGame)

        game.Paddle.ScaleWidth(cScaleFactor)


        MyBase.Activate(game)

    End Sub




    ''' <summary>
    ''' Overrides the Deactivate method to deactivate the powerup.
    ''' </summary>
    ''' <param name="game">reference to the game object deactivating the powerup</param>
    ''' <remarks></remarks>
    Public Overrides Sub Deactivate(ByRef game As BreakoutGame)

        game.Paddle.ResetSize()

        MyBase.Deactivate(game)

    End Sub



End Class
