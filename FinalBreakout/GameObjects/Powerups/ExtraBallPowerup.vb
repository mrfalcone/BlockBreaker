' ExtraBallPowerup
' ---
' Inherits Powerup to model a powerup causing the player to gain an extra ball life.
' 
' Author   : Michael Falcone
' Modified : 4/19/10



Public NotInheritable Class ExtraBallPowerup
    Inherits Powerup


    ' constants
    Private Const cTimeLimit As Double = 0
    Private Const cPointValue As Integer = 20



    ''' <summary>
    ''' Constructs a new ExtraBall powerup.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New(PowerupType.ExtraBall, cTimeLimit, cPointValue)
    End Sub





    ''' <summary>
    ''' Overrides the Activate method to activate the powerup.
    ''' </summary>
    ''' <param name="game">reference to the game object activating the powerup</param>
    ''' <remarks></remarks>
    Public Overrides Sub Activate(ByRef game As BreakoutGame)

        game.ExtraBall()
        MyBase.Activate(game)

    End Sub



End Class
