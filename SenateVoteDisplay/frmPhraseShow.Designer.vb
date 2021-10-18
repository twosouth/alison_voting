<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPhraseShow
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
        Me.lstPhrase = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'lstPhrase
        '
        Me.lstPhrase.AllowDrop = True
        Me.lstPhrase.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstPhrase.FormattingEnabled = True
        Me.lstPhrase.ItemHeight = 16
        Me.lstPhrase.Location = New System.Drawing.Point(4, 6)
        Me.lstPhrase.Name = "lstPhrase"
        Me.lstPhrase.ScrollAlwaysVisible = True
        Me.lstPhrase.Size = New System.Drawing.Size(240, 452)
        Me.lstPhrase.TabIndex = 0
        '
        'frmPhraseShow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlDark
        Me.ClientSize = New System.Drawing.Size(252, 465)
        Me.Controls.Add(Me.lstPhrase)
        Me.MaximizeBox = False
        Me.Name = "frmPhraseShow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Coded Phrase List"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstPhrase As System.Windows.Forms.ListBox
End Class
