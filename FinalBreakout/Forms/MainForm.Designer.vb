<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.GameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StartNewSessionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StartCustomSessionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EndSessionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.PauseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.HighScoresToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ShowFramerateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenLevelDesignerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DisplayPictureBox = New System.Windows.Forms.PictureBox
        Me.StartButton = New System.Windows.Forms.Button
        Me.MenuStrip1.SuspendLayout()
        CType(Me.DisplayPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GameToolStripMenuItem, Me.ViewToolStripMenuItem, Me.EditToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(264, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'GameToolStripMenuItem
        '
        Me.GameToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartNewSessionToolStripMenuItem, Me.StartCustomSessionToolStripMenuItem, Me.EndSessionToolStripMenuItem, Me.ToolStripSeparator1, Me.PauseToolStripMenuItem, Me.ToolStripSeparator4, Me.HighScoresToolStripMenuItem, Me.ToolStripSeparator2, Me.ExitToolStripMenuItem})
        Me.GameToolStripMenuItem.Name = "GameToolStripMenuItem"
        Me.GameToolStripMenuItem.Size = New System.Drawing.Size(50, 20)
        Me.GameToolStripMenuItem.Text = "&Game"
        '
        'StartNewSessionToolStripMenuItem
        '
        Me.StartNewSessionToolStripMenuItem.Name = "StartNewSessionToolStripMenuItem"
        Me.StartNewSessionToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.StartNewSessionToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.StartNewSessionToolStripMenuItem.Text = "Start &New Session"
        '
        'StartCustomSessionToolStripMenuItem
        '
        Me.StartCustomSessionToolStripMenuItem.Name = "StartCustomSessionToolStripMenuItem"
        Me.StartCustomSessionToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.StartCustomSessionToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.StartCustomSessionToolStripMenuItem.Text = "Start &Custom Session"
        '
        'EndSessionToolStripMenuItem
        '
        Me.EndSessionToolStripMenuItem.Name = "EndSessionToolStripMenuItem"
        Me.EndSessionToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.EndSessionToolStripMenuItem.Text = "End Session"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(201, 6)
        '
        'PauseToolStripMenuItem
        '
        Me.PauseToolStripMenuItem.Name = "PauseToolStripMenuItem"
        Me.PauseToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.P), System.Windows.Forms.Keys)
        Me.PauseToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.PauseToolStripMenuItem.Text = "&Pause"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(201, 6)
        '
        'HighScoresToolStripMenuItem
        '
        Me.HighScoresToolStripMenuItem.Name = "HighScoresToolStripMenuItem"
        Me.HighScoresToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4
        Me.HighScoresToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.HighScoresToolStripMenuItem.Text = "&High Scores"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(201, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowFramerateToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ViewToolStripMenuItem.Text = "&View"
        '
        'ShowFramerateToolStripMenuItem
        '
        Me.ShowFramerateToolStripMenuItem.CheckOnClick = True
        Me.ShowFramerateToolStripMenuItem.Name = "ShowFramerateToolStripMenuItem"
        Me.ShowFramerateToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.ShowFramerateToolStripMenuItem.Size = New System.Drawing.Size(178, 22)
        Me.ShowFramerateToolStripMenuItem.Text = "Show F&ramerate"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenLevelDesignerToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.EditToolStripMenuItem.Text = "&Edit"
        '
        'OpenLevelDesignerToolStripMenuItem
        '
        Me.OpenLevelDesignerToolStripMenuItem.Name = "OpenLevelDesignerToolStripMenuItem"
        Me.OpenLevelDesignerToolStripMenuItem.Size = New System.Drawing.Size(182, 22)
        Me.OpenLevelDesignerToolStripMenuItem.Text = "Open Level &Designer"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(107, 22)
        Me.AboutToolStripMenuItem.Text = "&About"
        '
        'DisplayPictureBox
        '
        Me.DisplayPictureBox.Location = New System.Drawing.Point(42, 61)
        Me.DisplayPictureBox.Name = "DisplayPictureBox"
        Me.DisplayPictureBox.Size = New System.Drawing.Size(171, 132)
        Me.DisplayPictureBox.TabIndex = 2
        Me.DisplayPictureBox.TabStop = False
        '
        'StartButton
        '
        Me.StartButton.BackColor = System.Drawing.Color.SlateGray
        Me.StartButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.StartButton.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StartButton.ForeColor = System.Drawing.Color.Black
        Me.StartButton.Location = New System.Drawing.Point(69, 119)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(103, 38)
        Me.StartButton.TabIndex = 3
        Me.StartButton.Text = "&Start"
        Me.StartButton.UseVisualStyleBackColor = False
        '
        'MainForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(264, 244)
        Me.Controls.Add(Me.StartButton)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.DisplayPictureBox)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "MainForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Breakout Game"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.DisplayPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents GameToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenLevelDesignerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StartNewSessionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StartCustomSessionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PauseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowFramerateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DisplayPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents EndSessionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HighScoresToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents StartButton As System.Windows.Forms.Button

End Class
