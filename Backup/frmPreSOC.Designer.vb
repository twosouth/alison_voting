<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPreSOC
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
        Me.btnClear = New System.Windows.Forms.Button
        Me.btnDown = New System.Windows.Forms.Button
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnPDown = New System.Windows.Forms.Button
        Me.btnPUp = New System.Windows.Forms.Button
        Me.btnUp = New System.Windows.Forms.Button
        Me.btnLeft = New System.Windows.Forms.Button
        Me.btnRight = New System.Windows.Forms.Button
        Me.lstCalendar = New System.Windows.Forms.ListBox
        Me.lstSCalendar = New System.Windows.Forms.ListBox
        Me.SuspendLayout()
        '
        'btnClear
        '
        Me.btnClear.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClear.Location = New System.Drawing.Point(340, 433)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(48, 24)
        Me.btnClear.TabIndex = 3
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnDown
        '
        Me.btnDown.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDown.Location = New System.Drawing.Point(105, 435)
        Me.btnDown.Name = "btnDown"
        Me.btnDown.Size = New System.Drawing.Size(48, 24)
        Me.btnDown.TabIndex = 6
        Me.btnDown.Text = "Down"
        Me.btnDown.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(194, 404)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(76, 21)
        Me.txtSearch.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(21, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 16)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Calendar"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Navy
        Me.Label2.Location = New System.Drawing.Point(280, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(149, 16)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Special Order Calendar"
        '
        'btnOK
        '
        Me.btnOK.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(284, 433)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(48, 24)
        Me.btnOK.TabIndex = 12
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(396, 433)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(48, 24)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(185, 385)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 16)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Search and Add"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnPDown
        '
        Me.btnPDown.Image = Global.SenateVotingOOB.My.Resources.Resources.Down
        Me.btnPDown.Location = New System.Drawing.Point(213, 269)
        Me.btnPDown.Name = "btnPDown"
        Me.btnPDown.Size = New System.Drawing.Size(44, 47)
        Me.btnPDown.TabIndex = 15
        Me.btnPDown.UseVisualStyleBackColor = True
        '
        'btnPUp
        '
        Me.btnPUp.Image = Global.SenateVotingOOB.My.Resources.Resources.imagesCAU421KR1
        Me.btnPUp.Location = New System.Drawing.Point(214, 211)
        Me.btnPUp.Name = "btnPUp"
        Me.btnPUp.Size = New System.Drawing.Size(44, 47)
        Me.btnPUp.TabIndex = 14
        Me.btnPUp.UseVisualStyleBackColor = True
        '
        'btnUp
        '
        Me.btnUp.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUp.Location = New System.Drawing.Point(49, 435)
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(48, 24)
        Me.btnUp.TabIndex = 7
        Me.btnUp.Text = "Up"
        Me.btnUp.UseVisualStyleBackColor = True
        '
        'btnLeft
        '
        Me.btnLeft.Image = Global.SenateVotingOOB.My.Resources.Resources.arrow_big_left
        Me.btnLeft.Location = New System.Drawing.Point(209, 164)
        Me.btnLeft.Name = "btnLeft"
        Me.btnLeft.Size = New System.Drawing.Size(52, 36)
        Me.btnLeft.TabIndex = 4
        Me.btnLeft.UseVisualStyleBackColor = True
        '
        'btnRight
        '
        Me.btnRight.Image = Global.SenateVotingOOB.My.Resources.Resources.arrow_big_right
        Me.btnRight.Location = New System.Drawing.Point(209, 117)
        Me.btnRight.Name = "btnRight"
        Me.btnRight.Size = New System.Drawing.Size(52, 36)
        Me.btnRight.TabIndex = 2
        Me.btnRight.UseVisualStyleBackColor = True
        '
        'lstCalendar
        '
        Me.lstCalendar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstCalendar.FormattingEnabled = True
        Me.lstCalendar.ItemHeight = 15
        Me.lstCalendar.Location = New System.Drawing.Point(22, 30)
        Me.lstCalendar.Name = "lstCalendar"
        Me.lstCalendar.Size = New System.Drawing.Size(162, 394)
        Me.lstCalendar.TabIndex = 16
        '
        'lstSCalendar
        '
        Me.lstSCalendar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstSCalendar.FormattingEnabled = True
        Me.lstSCalendar.ItemHeight = 15
        Me.lstSCalendar.Location = New System.Drawing.Point(281, 30)
        Me.lstSCalendar.Name = "lstSCalendar"
        Me.lstSCalendar.Size = New System.Drawing.Size(162, 394)
        Me.lstSCalendar.TabIndex = 17
        '
        'frmPreSOC
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(465, 472)
        Me.Controls.Add(Me.lstSCalendar)
        Me.Controls.Add(Me.lstCalendar)
        Me.Controls.Add(Me.btnPDown)
        Me.Controls.Add(Me.btnPUp)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.btnUp)
        Me.Controls.Add(Me.btnDown)
        Me.Controls.Add(Me.btnLeft)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnRight)
        Me.MaximizeBox = False
        Me.Name = "frmPreSOC"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Special Order Calendar"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnRight As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnLeft As System.Windows.Forms.Button
    Friend WithEvents btnDown As System.Windows.Forms.Button
    Friend WithEvents btnUp As System.Windows.Forms.Button
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnPDown As System.Windows.Forms.Button
    Friend WithEvents btnPUp As System.Windows.Forms.Button
    Friend WithEvents lstCalendar As System.Windows.Forms.ListBox
    Friend WithEvents lstSCalendar As System.Windows.Forms.ListBox
End Class
