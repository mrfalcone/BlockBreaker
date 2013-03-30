<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CustomLevelsForm
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
        Me.PreviewPictureBox = New System.Windows.Forms.PictureBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.MoveDownButton = New System.Windows.Forms.Button
        Me.MoveUpButton = New System.Windows.Forms.Button
        Me.DeselectAllButton = New System.Windows.Forms.Button
        Me.SelectAllButton = New System.Windows.Forms.Button
        Me.LevelListBox = New System.Windows.Forms.CheckedListBox
        Me.BeginButton = New System.Windows.Forms.Button
        Me.AuthorLabel = New System.Windows.Forms.Label
        Me.BlocksLabel = New System.Windows.Forms.Label
        Me.HighScoreLabel = New System.Windows.Forms.Label
        CType(Me.PreviewPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PreviewPictureBox
        '
        Me.PreviewPictureBox.Location = New System.Drawing.Point(329, 22)
        Me.PreviewPictureBox.Name = "PreviewPictureBox"
        Me.PreviewPictureBox.Size = New System.Drawing.Size(300, 162)
        Me.PreviewPictureBox.TabIndex = 0
        Me.PreviewPictureBox.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Playing Levels:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.MoveDownButton)
        Me.GroupBox1.Controls.Add(Me.MoveUpButton)
        Me.GroupBox1.Controls.Add(Me.DeselectAllButton)
        Me.GroupBox1.Controls.Add(Me.SelectAllButton)
        Me.GroupBox1.Controls.Add(Me.LevelListBox)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(285, 237)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Levels"
        '
        'MoveDownButton
        '
        Me.MoveDownButton.Location = New System.Drawing.Point(243, 67)
        Me.MoveDownButton.Name = "MoveDownButton"
        Me.MoveDownButton.Size = New System.Drawing.Size(27, 23)
        Me.MoveDownButton.TabIndex = 7
        Me.MoveDownButton.Text = "˅"
        Me.MoveDownButton.UseVisualStyleBackColor = True
        '
        'MoveUpButton
        '
        Me.MoveUpButton.Location = New System.Drawing.Point(243, 38)
        Me.MoveUpButton.Name = "MoveUpButton"
        Me.MoveUpButton.Size = New System.Drawing.Size(27, 23)
        Me.MoveUpButton.TabIndex = 6
        Me.MoveUpButton.Text = "˄"
        Me.MoveUpButton.UseVisualStyleBackColor = True
        '
        'DeselectAllButton
        '
        Me.DeselectAllButton.Location = New System.Drawing.Point(195, 193)
        Me.DeselectAllButton.Name = "DeselectAllButton"
        Me.DeselectAllButton.Size = New System.Drawing.Size(75, 23)
        Me.DeselectAllButton.TabIndex = 5
        Me.DeselectAllButton.Text = "&Deselect All"
        Me.DeselectAllButton.UseVisualStyleBackColor = True
        '
        'SelectAllButton
        '
        Me.SelectAllButton.Location = New System.Drawing.Point(16, 193)
        Me.SelectAllButton.Name = "SelectAllButton"
        Me.SelectAllButton.Size = New System.Drawing.Size(75, 23)
        Me.SelectAllButton.TabIndex = 4
        Me.SelectAllButton.Text = "&Select All"
        Me.SelectAllButton.UseVisualStyleBackColor = True
        '
        'LevelListBox
        '
        Me.LevelListBox.FormattingEnabled = True
        Me.LevelListBox.Location = New System.Drawing.Point(15, 38)
        Me.LevelListBox.Name = "LevelListBox"
        Me.LevelListBox.Size = New System.Drawing.Size(222, 139)
        Me.LevelListBox.TabIndex = 3
        '
        'BeginButton
        '
        Me.BeginButton.Location = New System.Drawing.Point(554, 205)
        Me.BeginButton.Name = "BeginButton"
        Me.BeginButton.Size = New System.Drawing.Size(75, 23)
        Me.BeginButton.TabIndex = 4
        Me.BeginButton.Text = "&Begin"
        Me.BeginButton.UseVisualStyleBackColor = True
        '
        'AuthorLabel
        '
        Me.AuthorLabel.AutoSize = True
        Me.AuthorLabel.Location = New System.Drawing.Point(326, 187)
        Me.AuthorLabel.Name = "AuthorLabel"
        Me.AuthorLabel.Size = New System.Drawing.Size(38, 13)
        Me.AuthorLabel.TabIndex = 5
        Me.AuthorLabel.Text = "Author"
        '
        'BlocksLabel
        '
        Me.BlocksLabel.AutoSize = True
        Me.BlocksLabel.Location = New System.Drawing.Point(326, 201)
        Me.BlocksLabel.Name = "BlocksLabel"
        Me.BlocksLabel.Size = New System.Drawing.Size(39, 13)
        Me.BlocksLabel.TabIndex = 6
        Me.BlocksLabel.Text = "Blocks"
        '
        'HighScoreLabel
        '
        Me.HighScoreLabel.AutoSize = True
        Me.HighScoreLabel.Location = New System.Drawing.Point(326, 216)
        Me.HighScoreLabel.Name = "HighScoreLabel"
        Me.HighScoreLabel.Size = New System.Drawing.Size(60, 13)
        Me.HighScoreLabel.TabIndex = 7
        Me.HighScoreLabel.Text = "High Score"
        '
        'CustomLevelsForm
        '
        Me.AcceptButton = Me.BeginButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(647, 261)
        Me.Controls.Add(Me.HighScoreLabel)
        Me.Controls.Add(Me.BlocksLabel)
        Me.Controls.Add(Me.AuthorLabel)
        Me.Controls.Add(Me.BeginButton)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.PreviewPictureBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CustomLevelsForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Choose Custom Levels"
        CType(Me.PreviewPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PreviewPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents DeselectAllButton As System.Windows.Forms.Button
    Friend WithEvents SelectAllButton As System.Windows.Forms.Button
    Friend WithEvents LevelListBox As System.Windows.Forms.CheckedListBox
    Friend WithEvents BeginButton As System.Windows.Forms.Button
    Friend WithEvents AuthorLabel As System.Windows.Forms.Label
    Friend WithEvents BlocksLabel As System.Windows.Forms.Label
    Friend WithEvents MoveUpButton As System.Windows.Forms.Button
    Friend WithEvents MoveDownButton As System.Windows.Forms.Button
    Friend WithEvents HighScoreLabel As System.Windows.Forms.Label
End Class
