<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOOB
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
        Me.components = New System.ComponentModel.Container
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtVoteID = New System.Windows.Forms.TextBox
        Me.lblTestMode = New System.Windows.Forms.Label
        Me.lblChamberLight = New System.Windows.Forms.Label
        Me.btnUnlock = New System.Windows.Forms.Button
        Me.lblStandBy = New System.Windows.Forms.Label
        Me.cboPhrase = New System.Windows.Forms.ComboBox
        Me.cboBill = New System.Windows.Forms.ComboBox
        Me.cboCalendar = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnVote = New System.Windows.Forms.Button
        Me.btnVoteID = New System.Windows.Forms.Button
        Me.btnHTML = New System.Windows.Forms.Button
        Me.cboSenator = New System.Windows.Forms.ComboBox
        Me.ReceiveTimer = New System.Windows.Forms.Timer(Me.components)
        Me.PingTimer = New System.Windows.Forms.Timer(Me.components)
        Me.cboDisplay = New System.Windows.Forms.ComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.bntChang = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(53, 62)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(100, 23)
        Me.Label5.TabIndex = 39
        Me.Label5.Text = "Vote ID"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtVoteID
        '
        Me.txtVoteID.Location = New System.Drawing.Point(153, 62)
        Me.txtVoteID.Name = "txtVoteID"
        Me.txtVoteID.Size = New System.Drawing.Size(120, 20)
        Me.txtVoteID.TabIndex = 38
        '
        'lblTestMode
        '
        Me.lblTestMode.AutoSize = True
        Me.lblTestMode.BackColor = System.Drawing.Color.Yellow
        Me.lblTestMode.Location = New System.Drawing.Point(425, 30)
        Me.lblTestMode.Name = "lblTestMode"
        Me.lblTestMode.Size = New System.Drawing.Size(58, 13)
        Me.lblTestMode.TabIndex = 37
        Me.lblTestMode.Text = "Test Mode"
        Me.lblTestMode.Visible = False
        '
        'lblChamberLight
        '
        Me.lblChamberLight.BackColor = System.Drawing.Color.PaleGreen
        Me.lblChamberLight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChamberLight.Location = New System.Drawing.Point(49, 22)
        Me.lblChamberLight.Name = "lblChamberLight"
        Me.lblChamberLight.Size = New System.Drawing.Size(260, 20)
        Me.lblChamberLight.TabIndex = 36
        Me.lblChamberLight.Text = "Order Busniess"
        Me.lblChamberLight.Visible = False
        '
        'btnUnlock
        '
        Me.btnUnlock.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUnlock.Location = New System.Drawing.Point(272, 276)
        Me.btnUnlock.Name = "btnUnlock"
        Me.btnUnlock.Size = New System.Drawing.Size(75, 28)
        Me.btnUnlock.TabIndex = 35
        Me.btnUnlock.Text = "Unlock"
        Me.btnUnlock.UseVisualStyleBackColor = True
        '
        'lblStandBy
        '
        Me.lblStandBy.AutoSize = True
        Me.lblStandBy.Location = New System.Drawing.Point(72, 324)
        Me.lblStandBy.Name = "lblStandBy"
        Me.lblStandBy.Size = New System.Drawing.Size(414, 13)
        Me.lblStandBy.TabIndex = 34
        Me.lblStandBy.Text = "Local group connection was dropped. You can not send message to Voting Computer!"
        Me.lblStandBy.Visible = False
        '
        'cboPhrase
        '
        Me.cboPhrase.FormattingEnabled = True
        Me.cboPhrase.Location = New System.Drawing.Point(153, 146)
        Me.cboPhrase.Name = "cboPhrase"
        Me.cboPhrase.Size = New System.Drawing.Size(340, 21)
        Me.cboPhrase.TabIndex = 33
        '
        'cboBill
        '
        Me.cboBill.FormattingEnabled = True
        Me.cboBill.Location = New System.Drawing.Point(153, 123)
        Me.cboBill.Name = "cboBill"
        Me.cboBill.Size = New System.Drawing.Size(121, 21)
        Me.cboBill.TabIndex = 32
        '
        'cboCalendar
        '
        Me.cboCalendar.FormattingEnabled = True
        Me.cboCalendar.Location = New System.Drawing.Point(153, 103)
        Me.cboCalendar.Name = "cboCalendar"
        Me.cboCalendar.Size = New System.Drawing.Size(121, 21)
        Me.cboCalendar.TabIndex = 31
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(53, 146)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 23)
        Me.Label4.TabIndex = 30
        Me.Label4.Text = "Phrase"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(53, 123)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 23)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "Bill"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(53, 103)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 23)
        Me.Label2.TabIndex = 28
        Me.Label2.Text = "Current Calendar"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(53, 83)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 23)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "Senator"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnVote
        '
        Me.btnVote.BackColor = System.Drawing.Color.Silver
        Me.btnVote.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnVote.Location = New System.Drawing.Point(112, 276)
        Me.btnVote.Name = "btnVote"
        Me.btnVote.Size = New System.Drawing.Size(75, 28)
        Me.btnVote.TabIndex = 26
        Me.btnVote.Text = "Vote"
        Me.btnVote.UseVisualStyleBackColor = False
        '
        'btnVoteID
        '
        Me.btnVoteID.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnVoteID.Location = New System.Drawing.Point(352, 276)
        Me.btnVoteID.Name = "btnVoteID"
        Me.btnVoteID.Size = New System.Drawing.Size(132, 28)
        Me.btnVoteID.TabIndex = 25
        Me.btnVoteID.Text = "Request Last Vote ID"
        Me.btnVoteID.UseVisualStyleBackColor = True
        '
        'btnHTML
        '
        Me.btnHTML.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHTML.Location = New System.Drawing.Point(192, 276)
        Me.btnHTML.Name = "btnHTML"
        Me.btnHTML.Size = New System.Drawing.Size(75, 28)
        Me.btnHTML.TabIndex = 24
        Me.btnHTML.Text = "HTML"
        Me.btnHTML.UseVisualStyleBackColor = True
        '
        'cboSenator
        '
        Me.cboSenator.FormattingEnabled = True
        Me.cboSenator.Location = New System.Drawing.Point(153, 83)
        Me.cboSenator.Name = "cboSenator"
        Me.cboSenator.Size = New System.Drawing.Size(121, 21)
        Me.cboSenator.TabIndex = 23
        '
        'ReceiveTimer
        '
        '
        'PingTimer
        '
        Me.PingTimer.Interval = 18000
        '
        'cboDisplay
        '
        Me.cboDisplay.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDisplay.FormattingEnabled = True
        Me.cboDisplay.Items.AddRange(New Object() {"Welcome", "Start Vote", "End Session"})
        Me.cboDisplay.Location = New System.Drawing.Point(94, 23)
        Me.cboDisplay.Name = "cboDisplay"
        Me.cboDisplay.Size = New System.Drawing.Size(105, 23)
        Me.cboDisplay.TabIndex = 41
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.bntChang)
        Me.GroupBox1.Controls.Add(Me.cboDisplay)
        Me.GroupBox1.Font = New System.Drawing.Font("Lucida Fax", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.GroupBox1.Location = New System.Drawing.Point(60, 188)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(348, 60)
        Me.GroupBox1.TabIndex = 42
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Display Backgourd"
        '
        'bntChang
        '
        Me.bntChang.BackColor = System.Drawing.Color.Silver
        Me.bntChang.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bntChang.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bntChang.Location = New System.Drawing.Point(198, 23)
        Me.bntChang.Name = "bntChang"
        Me.bntChang.Size = New System.Drawing.Size(80, 25)
        Me.bntChang.TabIndex = 42
        Me.bntChang.Text = "Chang Borad"
        Me.bntChang.UseVisualStyleBackColor = False
        '
        'frmOOB
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(569, 375)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtVoteID)
        Me.Controls.Add(Me.lblTestMode)
        Me.Controls.Add(Me.lblChamberLight)
        Me.Controls.Add(Me.btnUnlock)
        Me.Controls.Add(Me.lblStandBy)
        Me.Controls.Add(Me.cboPhrase)
        Me.Controls.Add(Me.cboBill)
        Me.Controls.Add(Me.cboCalendar)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnVote)
        Me.Controls.Add(Me.btnVoteID)
        Me.Controls.Add(Me.btnHTML)
        Me.Controls.Add(Me.cboSenator)
        Me.MaximizeBox = False
        Me.Name = "frmOOB"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Order Of Business"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtVoteID As System.Windows.Forms.TextBox
    Friend WithEvents lblTestMode As System.Windows.Forms.Label
    Friend WithEvents lblChamberLight As System.Windows.Forms.Label
    Friend WithEvents btnUnlock As System.Windows.Forms.Button
    Friend WithEvents lblStandBy As System.Windows.Forms.Label
    Friend WithEvents cboPhrase As System.Windows.Forms.ComboBox
    Friend WithEvents cboBill As System.Windows.Forms.ComboBox
    Friend WithEvents cboCalendar As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnVote As System.Windows.Forms.Button
    Friend WithEvents btnVoteID As System.Windows.Forms.Button
    Friend WithEvents btnHTML As System.Windows.Forms.Button
    Friend WithEvents cboSenator As System.Windows.Forms.ComboBox
    Friend WithEvents ReceiveTimer As System.Windows.Forms.Timer
    Friend WithEvents PingTimer As System.Windows.Forms.Timer
    Friend WithEvents cboDisplay As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents bntChang As System.Windows.Forms.Button

End Class
