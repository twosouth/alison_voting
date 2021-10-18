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
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.TabBills = New System.Windows.Forms.TabControl()
        Me.TabSOC = New System.Windows.Forms.TabPage()
        Me.btnClearAll = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.chkUSOC = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblSR = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lstSOCBills = New System.Windows.Forms.ListBox()
        Me.lstSOCCalendar = New System.Windows.Forms.ListBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnLeftSOC = New System.Windows.Forms.Button()
        Me.btnRightSOC = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtFindSOC = New System.Windows.Forms.TextBox()
        Me.btnPUp = New System.Windows.Forms.Button()
        Me.btnPDown = New System.Windows.Forms.Button()
        Me.btnUp = New System.Windows.Forms.Button()
        Me.btnDown = New System.Windows.Forms.Button()
        Me.lblL = New System.Windows.Forms.Label()
        Me.txtAddL = New System.Windows.Forms.TextBox()
        Me.lblR = New System.Windows.Forms.Label()
        Me.txtAddR = New System.Windows.Forms.TextBox()
        Me.btnReload = New System.Windows.Forms.Button()
        Me.tbnClearL = New System.Windows.Forms.Button()
        Me.tbnClearR = New System.Windows.Forms.Button()
        Me.btnDeleteSOC = New System.Windows.Forms.Button()
        Me.txtFindBill = New System.Windows.Forms.TextBox()
        Me.TabBills.SuspendLayout()
        Me.TabSOC.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnClear
        '
        Me.btnClear.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.btnClear.Location = New System.Drawing.Point(491, 843)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(79, 39)
        Me.btnClear.TabIndex = 3
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.btnOK.Location = New System.Drawing.Point(328, 843)
        Me.btnOK.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(79, 39)
        Me.btnOK.TabIndex = 12
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.btnClose.Location = New System.Drawing.Point(571, 843)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(79, 39)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'TabBills
        '
        Me.TabBills.Controls.Add(Me.TabSOC)
        Me.TabBills.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabBills.Location = New System.Drawing.Point(10, 12)
        Me.TabBills.Margin = New System.Windows.Forms.Padding(2)
        Me.TabBills.Name = "TabBills"
        Me.TabBills.SelectedIndex = 0
        Me.TabBills.Size = New System.Drawing.Size(1177, 779)
        Me.TabBills.TabIndex = 22
        '
        'TabSOC
        '
        Me.TabSOC.BackColor = System.Drawing.SystemColors.ControlDark
        Me.TabSOC.Controls.Add(Me.btnClearAll)
        Me.TabSOC.Controls.Add(Me.Label9)
        Me.TabSOC.Controls.Add(Me.chkUSOC)
        Me.TabSOC.Controls.Add(Me.Label1)
        Me.TabSOC.Controls.Add(Me.lblSR)
        Me.TabSOC.Controls.Add(Me.Label4)
        Me.TabSOC.Controls.Add(Me.Label3)
        Me.TabSOC.Controls.Add(Me.Label6)
        Me.TabSOC.Controls.Add(Me.Label8)
        Me.TabSOC.Controls.Add(Me.lstSOCBills)
        Me.TabSOC.Controls.Add(Me.lstSOCCalendar)
        Me.TabSOC.Controls.Add(Me.Label10)
        Me.TabSOC.Controls.Add(Me.Label11)
        Me.TabSOC.Controls.Add(Me.btnLeftSOC)
        Me.TabSOC.Controls.Add(Me.btnRightSOC)
        Me.TabSOC.Controls.Add(Me.Panel1)
        Me.TabSOC.Location = New System.Drawing.Point(4, 26)
        Me.TabSOC.Margin = New System.Windows.Forms.Padding(2)
        Me.TabSOC.Name = "TabSOC"
        Me.TabSOC.Padding = New System.Windows.Forms.Padding(2)
        Me.TabSOC.Size = New System.Drawing.Size(1169, 749)
        Me.TabSOC.TabIndex = 1
        Me.TabSOC.Text = "Special Order Calendar"
        '
        'btnClearAll
        '
        Me.btnClearAll.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.btnClearAll.ForeColor = System.Drawing.Color.Maroon
        Me.btnClearAll.Location = New System.Drawing.Point(536, 313)
        Me.btnClearAll.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClearAll.Name = "btnClearAll"
        Me.btnClearAll.Size = New System.Drawing.Size(94, 43)
        Me.btnClearAll.TabIndex = 60
        Me.btnClearAll.Text = "<<  ALL"
        Me.btnClearAll.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.Label9.ForeColor = System.Drawing.Color.Maroon
        Me.Label9.Location = New System.Drawing.Point(488, 723)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(190, 24)
        Me.Label9.TabIndex = 46
        Me.Label9.Text = "Or: hb3;SB6;sb1;HB120)"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkUSOC
        '
        Me.chkUSOC.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.chkUSOC.ForeColor = System.Drawing.Color.DarkGreen
        Me.chkUSOC.Location = New System.Drawing.Point(491, 520)
        Me.chkUSOC.Name = "chkUSOC"
        Me.chkUSOC.Size = New System.Drawing.Size(164, 40)
        Me.chkUSOC.TabIndex = 58
        Me.chkUSOC.Text = "Un-release Special Order Calendar"
        Me.chkUSOC.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.chkUSOC.UseVisualStyleBackColor = True
        Me.chkUSOC.Visible = False
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Underline)
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(488, 679)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(196, 21)
        Me.Label1.TabIndex = 57
        Me.Label1.Text = "               Bill or Bills                "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblSR
        '
        Me.lblSR.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.lblSR.ForeColor = System.Drawing.Color.Maroon
        Me.lblSR.Location = New System.Drawing.Point(170, 10)
        Me.lblSR.Name = "lblSR"
        Me.lblSR.Size = New System.Drawing.Size(195, 21)
        Me.lblSR.TabIndex = 56
        Me.lblSR.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.Label4.ForeColor = System.Drawing.Color.Maroon
        Me.Label4.Location = New System.Drawing.Point(483, 433)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(202, 19)
        Me.Label4.TabIndex = 55
        Me.Label4.Text = "By DocID: (Example 12426 - 1)"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label4.Visible = False
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.Label3.ForeColor = System.Drawing.Color.Maroon
        Me.Label3.Location = New System.Drawing.Point(488, 702)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(167, 21)
        Me.Label3.TabIndex = 53
        Me.Label3.Text = "(Input can be: SB12"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(488, 657)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(196, 22)
        Me.Label6.TabIndex = 52
        Me.Label6.Text = "Add Special Order Calendar"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Underline)
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(486, 367)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(197, 30)
        Me.Label8.TabIndex = 49
        Me.Label8.Text = "Load Special Order Calendar"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label8.Visible = False
        '
        'lstSOCBills
        '
        Me.lstSOCBills.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.lstSOCBills.FormattingEnabled = True
        Me.lstSOCBills.ItemHeight = 17
        Me.lstSOCBills.Location = New System.Drawing.Point(693, 36)
        Me.lstSOCBills.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.lstSOCBills.Name = "lstSOCBills"
        Me.lstSOCBills.Size = New System.Drawing.Size(474, 718)
        Me.lstSOCBills.TabIndex = 48
        '
        'lstSOCCalendar
        '
        Me.lstSOCCalendar.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.lstSOCCalendar.FormattingEnabled = True
        Me.lstSOCCalendar.ItemHeight = 17
        Me.lstSOCCalendar.Location = New System.Drawing.Point(0, 36)
        Me.lstSOCCalendar.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.lstSOCCalendar.Name = "lstSOCCalendar"
        Me.lstSOCCalendar.Size = New System.Drawing.Size(474, 718)
        Me.lstSOCCalendar.TabIndex = 47
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(693, 15)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(173, 20)
        Me.Label10.TabIndex = 45
        Me.Label10.Text = "Specical Order Calendar:"
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(2, 9)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(195, 24)
        Me.Label11.TabIndex = 44
        Me.Label11.Text = "Regular Order Calendar:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'btnLeftSOC
        '
        Me.btnLeftSOC.ForeColor = System.Drawing.Color.Maroon
        Me.btnLeftSOC.Location = New System.Drawing.Point(536, 260)
        Me.btnLeftSOC.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnLeftSOC.Name = "btnLeftSOC"
        Me.btnLeftSOC.Size = New System.Drawing.Size(94, 43)
        Me.btnLeftSOC.TabIndex = 42
        Me.btnLeftSOC.Text = "<<"
        Me.btnLeftSOC.UseVisualStyleBackColor = True
        '
        'btnRightSOC
        '
        Me.btnRightSOC.ForeColor = System.Drawing.Color.Maroon
        Me.btnRightSOC.Location = New System.Drawing.Point(536, 207)
        Me.btnRightSOC.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRightSOC.Name = "btnRightSOC"
        Me.btnRightSOC.Size = New System.Drawing.Size(94, 43)
        Me.btnRightSOC.TabIndex = 41
        Me.btnRightSOC.Text = ">>"
        Me.btnRightSOC.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.txtFindSOC)
        Me.Panel1.Location = New System.Drawing.Point(478, 357)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(211, 135)
        Me.Panel1.TabIndex = 59
        Me.Panel1.Visible = False
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(8, 57)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(147, 17)
        Me.Label2.TabIndex = 54
        Me.Label2.Text = "or: "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.Label7.ForeColor = System.Drawing.Color.Maroon
        Me.Label7.Location = New System.Drawing.Point(6, 37)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(161, 18)
        Me.Label7.TabIndex = 51
        Me.Label7.Text = "By SR: (Example SR11)"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtFindSOC
        '
        Me.txtFindSOC.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.txtFindSOC.Location = New System.Drawing.Point(9, 105)
        Me.txtFindSOC.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtFindSOC.Name = "txtFindSOC"
        Me.txtFindSOC.Size = New System.Drawing.Size(191, 26)
        Me.txtFindSOC.TabIndex = 50
        '
        'btnPUp
        '
        Me.btnPUp.Image = Global.SenateVotingOOB.My.Resources.Resources.Up
        Me.btnPUp.Location = New System.Drawing.Point(886, 794)
        Me.btnPUp.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPUp.Name = "btnPUp"
        Me.btnPUp.Size = New System.Drawing.Size(36, 32)
        Me.btnPUp.TabIndex = 18
        Me.btnPUp.UseVisualStyleBackColor = True
        '
        'btnPDown
        '
        Me.btnPDown.Image = Global.SenateVotingOOB.My.Resources.Resources.Down
        Me.btnPDown.Location = New System.Drawing.Point(940, 794)
        Me.btnPDown.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnPDown.Name = "btnPDown"
        Me.btnPDown.Size = New System.Drawing.Size(36, 32)
        Me.btnPDown.TabIndex = 15
        Me.btnPDown.UseVisualStyleBackColor = True
        '
        'btnUp
        '
        Me.btnUp.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUp.Image = Global.SenateVotingOOB.My.Resources.Resources.Up
        Me.btnUp.Location = New System.Drawing.Point(186, 794)
        Me.btnUp.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(35, 30)
        Me.btnUp.TabIndex = 7
        Me.btnUp.UseVisualStyleBackColor = True
        '
        'btnDown
        '
        Me.btnDown.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDown.Image = Global.SenateVotingOOB.My.Resources.Resources.Down
        Me.btnDown.Location = New System.Drawing.Point(234, 794)
        Me.btnDown.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnDown.Name = "btnDown"
        Me.btnDown.Size = New System.Drawing.Size(35, 30)
        Me.btnDown.TabIndex = 6
        Me.btnDown.UseVisualStyleBackColor = True
        '
        'lblL
        '
        Me.lblL.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblL.ForeColor = System.Drawing.Color.Maroon
        Me.lblL.Location = New System.Drawing.Point(54, 879)
        Me.lblL.Name = "lblL"
        Me.lblL.Size = New System.Drawing.Size(133, 21)
        Me.lblL.TabIndex = 44
        Me.lblL.Text = "Add Bill or Bills:"
        Me.lblL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblL.Visible = False
        '
        'txtAddL
        '
        Me.txtAddL.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.txtAddL.Location = New System.Drawing.Point(57, 851)
        Me.txtAddL.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAddL.Name = "txtAddL"
        Me.txtAddL.Size = New System.Drawing.Size(171, 24)
        Me.txtAddL.TabIndex = 43
        Me.txtAddL.Visible = False
        '
        'lblR
        '
        Me.lblR.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblR.ForeColor = System.Drawing.Color.Maroon
        Me.lblR.Location = New System.Drawing.Point(1011, 879)
        Me.lblR.Name = "lblR"
        Me.lblR.Size = New System.Drawing.Size(133, 21)
        Me.lblR.TabIndex = 46
        Me.lblR.Text = "Add Bill or Bills:"
        Me.lblR.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblR.Visible = False
        '
        'txtAddR
        '
        Me.txtAddR.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddR.Location = New System.Drawing.Point(994, 850)
        Me.txtAddR.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAddR.Name = "txtAddR"
        Me.txtAddR.Size = New System.Drawing.Size(171, 25)
        Me.txtAddR.TabIndex = 45
        Me.txtAddR.Visible = False
        '
        'btnReload
        '
        Me.btnReload.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.btnReload.Image = Global.SenateVotingOOB.My.Resources.Resources.reload
        Me.btnReload.Location = New System.Drawing.Point(408, 843)
        Me.btnReload.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnReload.Name = "btnReload"
        Me.btnReload.Size = New System.Drawing.Size(82, 39)
        Me.btnReload.TabIndex = 48
        Me.btnReload.Text = "Reload"
        Me.btnReload.UseVisualStyleBackColor = True
        '
        'tbnClearL
        '
        Me.tbnClearL.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.tbnClearL.Location = New System.Drawing.Point(282, 795)
        Me.tbnClearL.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.tbnClearL.Name = "tbnClearL"
        Me.tbnClearL.Size = New System.Drawing.Size(61, 30)
        Me.tbnClearL.TabIndex = 50
        Me.tbnClearL.Text = "Clear"
        Me.tbnClearL.UseVisualStyleBackColor = True
        '
        'tbnClearR
        '
        Me.tbnClearR.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.tbnClearR.Location = New System.Drawing.Point(994, 796)
        Me.tbnClearR.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.tbnClearR.Name = "tbnClearR"
        Me.tbnClearR.Size = New System.Drawing.Size(61, 30)
        Me.tbnClearR.TabIndex = 51
        Me.tbnClearR.Text = "Clear"
        Me.tbnClearR.UseVisualStyleBackColor = True
        '
        'btnDeleteSOC
        '
        Me.btnDeleteSOC.AutoSize = True
        Me.btnDeleteSOC.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.btnDeleteSOC.Location = New System.Drawing.Point(651, 843)
        Me.btnDeleteSOC.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnDeleteSOC.Name = "btnDeleteSOC"
        Me.btnDeleteSOC.Size = New System.Drawing.Size(229, 39)
        Me.btnDeleteSOC.TabIndex = 52
        Me.btnDeleteSOC.Text = "Delete Special Order Calendar"
        Me.btnDeleteSOC.UseVisualStyleBackColor = True
        '
        'txtFindBill
        '
        Me.txtFindBill.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.txtFindBill.Location = New System.Drawing.Point(408, 790)
        Me.txtFindBill.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtFindBill.Name = "txtFindBill"
        Me.txtFindBill.Size = New System.Drawing.Size(396, 24)
        Me.txtFindBill.TabIndex = 47
        '
        'frmPreSOC
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlDark
        Me.ClientSize = New System.Drawing.Size(1198, 902)
        Me.Controls.Add(Me.txtFindBill)
        Me.Controls.Add(Me.btnDeleteSOC)
        Me.Controls.Add(Me.tbnClearR)
        Me.Controls.Add(Me.tbnClearL)
        Me.Controls.Add(Me.btnReload)
        Me.Controls.Add(Me.lblR)
        Me.Controls.Add(Me.txtAddR)
        Me.Controls.Add(Me.lblL)
        Me.Controls.Add(Me.txtAddL)
        Me.Controls.Add(Me.TabBills)
        Me.Controls.Add(Me.btnPUp)
        Me.Controls.Add(Me.btnPDown)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnUp)
        Me.Controls.Add(Me.btnDown)
        Me.Controls.Add(Me.btnClear)
        Me.Font = New System.Drawing.Font("Arial", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.Name = "frmPreSOC"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Special Order Calendar"
        Me.TabBills.ResumeLayout(False)
        Me.TabSOC.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnDown As System.Windows.Forms.Button
    Friend WithEvents btnUp As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnPDown As System.Windows.Forms.Button
    Friend WithEvents btnPUp As System.Windows.Forms.Button
    Friend WithEvents TabBills As System.Windows.Forms.TabControl
    Friend WithEvents TabSOC As System.Windows.Forms.TabPage
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtFindSOC As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lstSOCBills As System.Windows.Forms.ListBox
    Friend WithEvents lstSOCCalendar As System.Windows.Forms.ListBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnLeftSOC As System.Windows.Forms.Button
    Friend WithEvents btnRightSOC As System.Windows.Forms.Button
    Friend WithEvents lblL As System.Windows.Forms.Label
    Friend WithEvents txtAddL As System.Windows.Forms.TextBox
    Friend WithEvents lblR As System.Windows.Forms.Label
    Friend WithEvents txtAddR As System.Windows.Forms.TextBox
    Friend WithEvents btnReload As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tbnClearL As System.Windows.Forms.Button
    Friend WithEvents tbnClearR As System.Windows.Forms.Button
    Friend WithEvents btnDeleteSOC As System.Windows.Forms.Button
    Friend WithEvents lblSR As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkUSOC As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnClearAll As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtFindBill As System.Windows.Forms.TextBox
End Class
