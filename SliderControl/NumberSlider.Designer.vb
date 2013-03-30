<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NumberSlider
    Inherits System.Windows.Forms.UserControl

    'UserControl1 overrides dispose to clean up the component list.
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
        Me.TrackBar = New System.Windows.Forms.TrackBar
        Me.TextBox = New System.Windows.Forms.TextBox
        CType(Me.TrackBar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TrackBar
        '
        Me.TrackBar.Location = New System.Drawing.Point(38, 0)
        Me.TrackBar.Name = "TrackBar"
        Me.TrackBar.Size = New System.Drawing.Size(101, 45)
        Me.TrackBar.TabIndex = 1
        Me.TrackBar.TabStop = False
        '
        'TextBox
        '
        Me.TextBox.Location = New System.Drawing.Point(3, 3)
        Me.TextBox.Name = "TextBox"
        Me.TextBox.Size = New System.Drawing.Size(29, 20)
        Me.TextBox.TabIndex = 0
        '
        'NumberSlider
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TextBox)
        Me.Controls.Add(Me.TrackBar)
        Me.Name = "NumberSlider"
        Me.Size = New System.Drawing.Size(176, 98)
        CType(Me.TrackBar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TrackBar As System.Windows.Forms.TrackBar
    Friend WithEvents TextBox As System.Windows.Forms.TextBox

End Class
