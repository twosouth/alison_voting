<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMessage
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
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnYes = New System.Windows.Forms.Button()
        Me.btnNo = New System.Windows.Forms.Button()
        Me.Message = New System.Windows.Forms.TextBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.BackColor = System.Drawing.Color.OldLace
        Me.btnOK.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnOK.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnOK.Location = New System.Drawing.Point(219, 140)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnOK.Size = New System.Drawing.Size(57, 27)
        Me.btnOK.TabIndex = 13
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = False
        Me.btnOK.Visible = False
        '
        'btnYes
        '
        Me.btnYes.BackColor = System.Drawing.Color.OldLace
        Me.btnYes.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnYes.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnYes.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnYes.Location = New System.Drawing.Point(161, 140)
        Me.btnYes.Name = "btnYes"
        Me.btnYes.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnYes.Size = New System.Drawing.Size(57, 27)
        Me.btnYes.TabIndex = 12
        Me.btnYes.Text = "Yes"
        Me.btnYes.UseVisualStyleBackColor = False
        Me.btnYes.Visible = False
        '
        'btnNo
        '
        Me.btnNo.BackColor = System.Drawing.Color.OldLace
        Me.btnNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnNo.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnNo.Location = New System.Drawing.Point(277, 140)
        Me.btnNo.Name = "btnNo"
        Me.btnNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnNo.Size = New System.Drawing.Size(57, 27)
        Me.btnNo.TabIndex = 14
        Me.btnNo.Text = "No"
        Me.btnNo.UseVisualStyleBackColor = False
        Me.btnNo.Visible = False
        '
        'Message
        '
        Me.Message.AcceptsReturn = True
        Me.Message.BackColor = System.Drawing.SystemColors.Window
        Me.Message.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Message.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.Message.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Message.Location = New System.Drawing.Point(20, 29)
        Me.Message.MaxLength = 0
        Me.Message.Multiline = True
        Me.Message.Name = "Message"
        Me.Message.ReadOnly = True
        Me.Message.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Message.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.Message.Size = New System.Drawing.Size(453, 98)
        Me.Message.TabIndex = 10
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.BackColor = System.Drawing.Color.SlateGray
        Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTitle.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(22, 10)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTitle.Size = New System.Drawing.Size(34, 17)
        Me.lblTitle.TabIndex = 11
        Me.lblTitle.Text = "Title"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnClose.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnClose.Location = New System.Drawing.Point(415, 140)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnClose.Size = New System.Drawing.Size(60, 27)
        Me.btnClose.TabIndex = 15
        Me.btnClose.Text = "Exit"
        Me.btnClose.UseVisualStyleBackColor = False
        Me.btnClose.Visible = False
        '
        'frmMessage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.SlateGray
        Me.ClientSize = New System.Drawing.Size(492, 178)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnYes)
        Me.Controls.Add(Me.btnNo)
        Me.Controls.Add(Me.Message)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "frmMessage"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents btnOK As System.Windows.Forms.Button
    Public WithEvents btnYes As System.Windows.Forms.Button
    Public WithEvents btnNo As System.Windows.Forms.Button
    Public WithEvents Message As System.Windows.Forms.TextBox
    Public WithEvents lblTitle As System.Windows.Forms.Label
    Public WithEvents btnClose As System.Windows.Forms.Button
End Class
