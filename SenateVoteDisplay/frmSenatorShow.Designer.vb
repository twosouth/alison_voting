<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSenatorShow
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
        Me.lstSenator = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'lstSenator
        '
        Me.lstSenator.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstSenator.FormattingEnabled = True
        Me.lstSenator.ItemHeight = 15
        Me.lstSenator.Location = New System.Drawing.Point(4, 4)
        Me.lstSenator.Name = "lstSenator"
        Me.lstSenator.ScrollAlwaysVisible = True
        Me.lstSenator.Size = New System.Drawing.Size(244, 454)
        Me.lstSenator.TabIndex = 0
        '
        'frmSenatorShow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlDark
        Me.ClientSize = New System.Drawing.Size(252, 462)
        Me.Controls.Add(Me.lstSenator)
        Me.MaximizeBox = False
        Me.Name = "frmSenatorShow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Senators "
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstSenator As System.Windows.Forms.ListBox
End Class
