<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChamberDisplay
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChamberDisplay))
        Me.Senators = New System.Windows.Forms.ComboBox()
        Me.LegislativeDay = New System.Windows.Forms.TextBox()
        Me.btnUpdateDisplay = New System.Windows.Forms.Button()
        Me.InsertText = New System.Windows.Forms.TextBox()
        Me.btnPhraseMaintenance = New System.Windows.Forms.Button()
        Me.btnPreviousBill = New System.Windows.Forms.Button()
        Me.btnNextBill = New System.Windows.Forms.Button()
        Me.CurrentCalendar = New System.Windows.Forms.TextBox()
        Me.CurrentOrderOfBusiness = New System.Windows.Forms.TextBox()
        Me.lblLegislativeDay = New System.Windows.Forms.Label()
        Me.lblSession = New System.Windows.Forms.Label()
        Me.lblTestMode = New System.Windows.Forms.Label()
        Me.lblInsertText = New System.Windows.Forms.Label()
        Me.lblChamberLight = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblWorkAreaDisplayed = New System.Windows.Forms.Label()
        Me.lblWorkArea = New System.Windows.Forms.Label()
        Me.lblSelectedPhrase = New System.Windows.Forms.Label()
        Me.lblSelectedSenator = New System.Windows.Forms.Label()
        Me.lblCurrentBill = New System.Windows.Forms.Label()
        Me.lblCurrentCalendar = New System.Windows.Forms.Label()
        Me.lblCalendars = New System.Windows.Forms.Label()
        Me.lblOrdersOfBusiness = New System.Windows.Forms.Label()
        Me.FindBillNbr = New System.Windows.Forms.TextBox()
        Me.OrderOfBusiness = New System.Windows.Forms.ListBox()
        Me.Calendar = New System.Windows.Forms.ListBox()
        Me.Bill = New System.Windows.Forms.ListBox()
        Me.Phrases = New System.Windows.Forms.ComboBox()
        Me.CurrentBill = New System.Windows.Forms.TextBox()
        Me.btnBIR = New System.Windows.Forms.Button()
        Me.btnDropLastPhrase = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SOCNbr = New System.Windows.Forms.TextBox()
        Me.btnFreeFormat = New System.Windows.Forms.Button()
        Me.ReceiveTimer = New System.Windows.Forms.Timer(Me.components)
        Me.btnRequestLastVoteID = New System.Windows.Forms.Button()
        Me.PingTimer = New System.Windows.Forms.Timer(Me.components)
        Me.btnRecallDisplayedBill = New System.Windows.Forms.Button()
        Me.lblSelectedCommittee = New System.Windows.Forms.Label()
        Me.Committees = New System.Windows.Forms.ComboBox()
        Me.cboDisplay = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TC = New System.Windows.Forms.TabControl()
        Me.TabPage = New System.Windows.Forms.TabPage()
        Me.WorkData = New System.Windows.Forms.TextBox()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.WorkData1 = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.WorkData2 = New System.Windows.Forms.TextBox()
        Me.btnClearWorkArea = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnVote = New System.Windows.Forms.Button()
        Me.lblAddPhraseToWorkArea = New System.Windows.Forms.Label()
        Me.lblBills = New System.Windows.Forms.Label()
        Me.lblCurrentOrderOfBusiness = New System.Windows.Forms.Label()
        Me.lblFindBillNbr = New System.Windows.Forms.Label()
        Me.btnClearDisplay = New System.Windows.Forms.Button()
        Me.btnCreateHTMLPage = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtVoteID = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnUnlock = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnCancelVote = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtSRChange = New System.Windows.Forms.TextBox()
        Me.btnSRSave = New System.Windows.Forms.Button()
        Me.AddPhrase = New System.Windows.Forms.TextBox()
        Me.btnStartService = New System.Windows.Forms.Button()
        Me.btnFullClear = New System.Windows.Forms.Button()
        Me.btnDownLoadBills = New System.Windows.Forms.Button()
        Me.btnSOC = New System.Windows.Forms.Button()
        Me.TimerSOC = New System.Windows.Forms.Timer(Me.components)
        Me.TC.SuspendLayout()
        Me.TabPage.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Senators
        '
        Me.Senators.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Senators.Cursor = System.Windows.Forms.Cursors.Default
        Me.Senators.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Senators.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Senators.ForeColor = System.Drawing.Color.Navy
        Me.Senators.Location = New System.Drawing.Point(414, 55)
        Me.Senators.MaxDropDownItems = 35
        Me.Senators.Name = "Senators"
        Me.Senators.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Senators.Size = New System.Drawing.Size(181, 30)
        Me.Senators.TabIndex = 141
        '
        'LegislativeDay
        '
        Me.LegislativeDay.AcceptsReturn = True
        Me.LegislativeDay.BackColor = System.Drawing.Color.WhiteSmoke
        Me.LegislativeDay.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.LegislativeDay.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LegislativeDay.ForeColor = System.Drawing.Color.Navy
        Me.LegislativeDay.Location = New System.Drawing.Point(250, 55)
        Me.LegislativeDay.MaxLength = 0
        Me.LegislativeDay.Name = "LegislativeDay"
        Me.LegislativeDay.ReadOnly = True
        Me.LegislativeDay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LegislativeDay.Size = New System.Drawing.Size(162, 29)
        Me.LegislativeDay.TabIndex = 140
        '
        'btnUpdateDisplay
        '
        Me.btnUpdateDisplay.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnUpdateDisplay.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnUpdateDisplay.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdateDisplay.ForeColor = System.Drawing.Color.Black
        Me.btnUpdateDisplay.Location = New System.Drawing.Point(777, 858)
        Me.btnUpdateDisplay.Name = "btnUpdateDisplay"
        Me.btnUpdateDisplay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnUpdateDisplay.Size = New System.Drawing.Size(131, 38)
        Me.btnUpdateDisplay.TabIndex = 131
        Me.btnUpdateDisplay.Text = "Update Display"
        Me.btnUpdateDisplay.UseVisualStyleBackColor = False
        '
        'InsertText
        '
        Me.InsertText.AcceptsReturn = True
        Me.InsertText.BackColor = System.Drawing.Color.WhiteSmoke
        Me.InsertText.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.InsertText.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InsertText.ForeColor = System.Drawing.Color.Black
        Me.InsertText.Location = New System.Drawing.Point(748, 149)
        Me.InsertText.MaxLength = 0
        Me.InsertText.Name = "InsertText"
        Me.InsertText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InsertText.Size = New System.Drawing.Size(466, 29)
        Me.InsertText.TabIndex = 129
        '
        'btnPhraseMaintenance
        '
        Me.btnPhraseMaintenance.BackColor = System.Drawing.Color.Silver
        Me.btnPhraseMaintenance.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnPhraseMaintenance.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnPhraseMaintenance.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPhraseMaintenance.ForeColor = System.Drawing.Color.Black
        Me.btnPhraseMaintenance.Location = New System.Drawing.Point(1062, 79)
        Me.btnPhraseMaintenance.Name = "btnPhraseMaintenance"
        Me.btnPhraseMaintenance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnPhraseMaintenance.Size = New System.Drawing.Size(152, 22)
        Me.btnPhraseMaintenance.TabIndex = 128
        Me.btnPhraseMaintenance.Text = "Phrase Maintenance"
        Me.btnPhraseMaintenance.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnPhraseMaintenance.UseVisualStyleBackColor = False
        '
        'btnPreviousBill
        '
        Me.btnPreviousBill.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnPreviousBill.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnPreviousBill.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.btnPreviousBill.ForeColor = System.Drawing.Color.Black
        Me.btnPreviousBill.Location = New System.Drawing.Point(76, 857)
        Me.btnPreviousBill.Name = "btnPreviousBill"
        Me.btnPreviousBill.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnPreviousBill.Size = New System.Drawing.Size(102, 38)
        Me.btnPreviousBill.TabIndex = 120
        Me.btnPreviousBill.Text = "Previous Bill"
        Me.btnPreviousBill.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnPreviousBill.UseVisualStyleBackColor = False
        '
        'btnNextBill
        '
        Me.btnNextBill.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnNextBill.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnNextBill.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.btnNextBill.ForeColor = System.Drawing.Color.Black
        Me.btnNextBill.Location = New System.Drawing.Point(183, 857)
        Me.btnNextBill.Name = "btnNextBill"
        Me.btnNextBill.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnNextBill.Size = New System.Drawing.Size(78, 38)
        Me.btnNextBill.TabIndex = 114
        Me.btnNextBill.Text = "Next Bill"
        Me.btnNextBill.UseVisualStyleBackColor = False
        '
        'CurrentCalendar
        '
        Me.CurrentCalendar.AcceptsReturn = True
        Me.CurrentCalendar.BackColor = System.Drawing.Color.WhiteSmoke
        Me.CurrentCalendar.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.CurrentCalendar.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentCalendar.ForeColor = System.Drawing.Color.Black
        Me.CurrentCalendar.Location = New System.Drawing.Point(273, 101)
        Me.CurrentCalendar.MaxLength = 0
        Me.CurrentCalendar.Name = "CurrentCalendar"
        Me.CurrentCalendar.ReadOnly = True
        Me.CurrentCalendar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CurrentCalendar.Size = New System.Drawing.Size(229, 29)
        Me.CurrentCalendar.TabIndex = 107
        Me.CurrentCalendar.TabStop = False
        '
        'CurrentOrderOfBusiness
        '
        Me.CurrentOrderOfBusiness.AcceptsReturn = True
        Me.CurrentOrderOfBusiness.BackColor = System.Drawing.Color.WhiteSmoke
        Me.CurrentOrderOfBusiness.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.CurrentOrderOfBusiness.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentOrderOfBusiness.ForeColor = System.Drawing.Color.Black
        Me.CurrentOrderOfBusiness.Location = New System.Drawing.Point(37, 101)
        Me.CurrentOrderOfBusiness.MaxLength = 0
        Me.CurrentOrderOfBusiness.Name = "CurrentOrderOfBusiness"
        Me.CurrentOrderOfBusiness.ReadOnly = True
        Me.CurrentOrderOfBusiness.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CurrentOrderOfBusiness.Size = New System.Drawing.Size(231, 29)
        Me.CurrentOrderOfBusiness.TabIndex = 103
        Me.CurrentOrderOfBusiness.TabStop = False
        '
        'lblLegislativeDay
        '
        Me.lblLegislativeDay.AutoSize = True
        Me.lblLegislativeDay.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblLegislativeDay.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLegislativeDay.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLegislativeDay.ForeColor = System.Drawing.Color.White
        Me.lblLegislativeDay.Image = CType(resources.GetObject("lblLegislativeDay.Image"), System.Drawing.Image)
        Me.lblLegislativeDay.Location = New System.Drawing.Point(247, 35)
        Me.lblLegislativeDay.Name = "lblLegislativeDay"
        Me.lblLegislativeDay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLegislativeDay.Size = New System.Drawing.Size(187, 24)
        Me.lblLegislativeDay.TabIndex = 139
        Me.lblLegislativeDay.Text = "Legislative Day / Date"
        Me.lblLegislativeDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblSession
        '
        Me.lblSession.BackColor = System.Drawing.Color.WhiteSmoke
        Me.lblSession.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSession.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSession.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSession.ForeColor = System.Drawing.Color.Navy
        Me.lblSession.Location = New System.Drawing.Point(37, 55)
        Me.lblSession.Name = "lblSession"
        Me.lblSession.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSession.Size = New System.Drawing.Size(210, 25)
        Me.lblSession.TabIndex = 138
        Me.lblSession.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTestMode
        '
        Me.lblTestMode.BackColor = System.Drawing.Color.Yellow
        Me.lblTestMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTestMode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTestMode.Font = New System.Drawing.Font("Arial", 12.0!)
        Me.lblTestMode.ForeColor = System.Drawing.Color.Black
        Me.lblTestMode.Location = New System.Drawing.Point(1051, 14)
        Me.lblTestMode.Name = "lblTestMode"
        Me.lblTestMode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTestMode.Size = New System.Drawing.Size(84, 32)
        Me.lblTestMode.TabIndex = 137
        Me.lblTestMode.Text = "Test Mode"
        Me.lblTestMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblInsertText
        '
        Me.lblInsertText.AutoSize = True
        Me.lblInsertText.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsertText.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsertText.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsertText.ForeColor = System.Drawing.Color.White
        Me.lblInsertText.Image = CType(resources.GetObject("lblInsertText.Image"), System.Drawing.Image)
        Me.lblInsertText.Location = New System.Drawing.Point(748, 130)
        Me.lblInsertText.Name = "lblInsertText"
        Me.lblInsertText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsertText.Size = New System.Drawing.Size(102, 24)
        Me.lblInsertText.TabIndex = 130
        Me.lblInsertText.Text = "Insert Text:"
        Me.lblInsertText.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblChamberLight
        '
        Me.lblChamberLight.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.lblChamberLight.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChamberLight.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblChamberLight.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblChamberLight.Location = New System.Drawing.Point(37, 0)
        Me.lblChamberLight.Name = "lblChamberLight"
        Me.lblChamberLight.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChamberLight.Size = New System.Drawing.Size(400, 25)
        Me.lblChamberLight.TabIndex = 124
        Me.lblChamberLight.Text = "Display Next Bill"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(919, 84)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(1, 5)
        Me.Label2.TabIndex = 123
        Me.Label2.Text = "Label2"
        '
        'lblWorkAreaDisplayed
        '
        Me.lblWorkAreaDisplayed.BackColor = System.Drawing.Color.Red
        Me.lblWorkAreaDisplayed.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWorkAreaDisplayed.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.lblWorkAreaDisplayed.ForeColor = System.Drawing.Color.White
        Me.lblWorkAreaDisplayed.Location = New System.Drawing.Point(826, 259)
        Me.lblWorkAreaDisplayed.Name = "lblWorkAreaDisplayed"
        Me.lblWorkAreaDisplayed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWorkAreaDisplayed.Size = New System.Drawing.Size(311, 28)
        Me.lblWorkAreaDisplayed.TabIndex = 117
        Me.lblWorkAreaDisplayed.Text = "This Work Area Is Currently Displayed"
        Me.lblWorkAreaDisplayed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblWorkAreaDisplayed.Visible = False
        '
        'lblWorkArea
        '
        Me.lblWorkArea.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblWorkArea.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWorkArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWorkArea.ForeColor = System.Drawing.Color.Maroon
        Me.lblWorkArea.Location = New System.Drawing.Point(856, 294)
        Me.lblWorkArea.Name = "lblWorkArea"
        Me.lblWorkArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWorkArea.Size = New System.Drawing.Size(262, 25)
        Me.lblWorkArea.TabIndex = 116
        Me.lblWorkArea.Text = "Work Area For The Current Bill"
        Me.lblWorkArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSelectedPhrase
        '
        Me.lblSelectedPhrase.AutoSize = True
        Me.lblSelectedPhrase.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblSelectedPhrase.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSelectedPhrase.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelectedPhrase.ForeColor = System.Drawing.Color.White
        Me.lblSelectedPhrase.Image = CType(resources.GetObject("lblSelectedPhrase.Image"), System.Drawing.Image)
        Me.lblSelectedPhrase.Location = New System.Drawing.Point(502, 82)
        Me.lblSelectedPhrase.Name = "lblSelectedPhrase"
        Me.lblSelectedPhrase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSelectedPhrase.Size = New System.Drawing.Size(148, 24)
        Me.lblSelectedPhrase.TabIndex = 113
        Me.lblSelectedPhrase.Text = "Selected Phrase"
        Me.lblSelectedPhrase.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblSelectedSenator
        '
        Me.lblSelectedSenator.AutoSize = True
        Me.lblSelectedSenator.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblSelectedSenator.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSelectedSenator.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelectedSenator.ForeColor = System.Drawing.Color.White
        Me.lblSelectedSenator.Image = CType(resources.GetObject("lblSelectedSenator.Image"), System.Drawing.Image)
        Me.lblSelectedSenator.Location = New System.Drawing.Point(417, 35)
        Me.lblSelectedSenator.Name = "lblSelectedSenator"
        Me.lblSelectedSenator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSelectedSenator.Size = New System.Drawing.Size(137, 20)
        Me.lblSelectedSenator.TabIndex = 111
        Me.lblSelectedSenator.Text = "Selected Senator"
        Me.lblSelectedSenator.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblCurrentBill
        '
        Me.lblCurrentBill.AutoSize = True
        Me.lblCurrentBill.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrentBill.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrentBill.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentBill.ForeColor = System.Drawing.Color.White
        Me.lblCurrentBill.Image = CType(resources.GetObject("lblCurrentBill.Image"), System.Drawing.Image)
        Me.lblCurrentBill.Location = New System.Drawing.Point(747, 177)
        Me.lblCurrentBill.Name = "lblCurrentBill"
        Me.lblCurrentBill.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrentBill.Size = New System.Drawing.Size(101, 24)
        Me.lblCurrentBill.TabIndex = 110
        Me.lblCurrentBill.Text = "Current Bill"
        Me.lblCurrentBill.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblCurrentCalendar
        '
        Me.lblCurrentCalendar.AutoSize = True
        Me.lblCurrentCalendar.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblCurrentCalendar.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrentCalendar.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentCalendar.ForeColor = System.Drawing.Color.White
        Me.lblCurrentCalendar.Image = CType(resources.GetObject("lblCurrentCalendar.Image"), System.Drawing.Image)
        Me.lblCurrentCalendar.Location = New System.Drawing.Point(269, 82)
        Me.lblCurrentCalendar.Name = "lblCurrentCalendar"
        Me.lblCurrentCalendar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrentCalendar.Size = New System.Drawing.Size(153, 24)
        Me.lblCurrentCalendar.TabIndex = 108
        Me.lblCurrentCalendar.Text = "Current Calendar"
        Me.lblCurrentCalendar.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblCalendars
        '
        Me.lblCalendars.AutoSize = True
        Me.lblCalendars.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblCalendars.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCalendars.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCalendars.ForeColor = System.Drawing.Color.White
        Me.lblCalendars.Image = CType(resources.GetObject("lblCalendars.Image"), System.Drawing.Image)
        Me.lblCalendars.Location = New System.Drawing.Point(276, 130)
        Me.lblCalendars.Name = "lblCalendars"
        Me.lblCalendars.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCalendars.Size = New System.Drawing.Size(95, 24)
        Me.lblCalendars.TabIndex = 106
        Me.lblCalendars.Text = "Calendars"
        Me.lblCalendars.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblOrdersOfBusiness
        '
        Me.lblOrdersOfBusiness.AutoSize = True
        Me.lblOrdersOfBusiness.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblOrdersOfBusiness.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOrdersOfBusiness.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrdersOfBusiness.ForeColor = System.Drawing.Color.White
        Me.lblOrdersOfBusiness.Image = CType(resources.GetObject("lblOrdersOfBusiness.Image"), System.Drawing.Image)
        Me.lblOrdersOfBusiness.Location = New System.Drawing.Point(35, 130)
        Me.lblOrdersOfBusiness.Name = "lblOrdersOfBusiness"
        Me.lblOrdersOfBusiness.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOrdersOfBusiness.Size = New System.Drawing.Size(173, 24)
        Me.lblOrdersOfBusiness.TabIndex = 104
        Me.lblOrdersOfBusiness.Text = "Orders Of Business"
        Me.lblOrdersOfBusiness.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'FindBillNbr
        '
        Me.FindBillNbr.AcceptsReturn = True
        Me.FindBillNbr.BackColor = System.Drawing.Color.WhiteSmoke
        Me.FindBillNbr.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.FindBillNbr.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FindBillNbr.ForeColor = System.Drawing.Color.Navy
        Me.FindBillNbr.Location = New System.Drawing.Point(393, 860)
        Me.FindBillNbr.MaxLength = 0
        Me.FindBillNbr.Name = "FindBillNbr"
        Me.FindBillNbr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FindBillNbr.Size = New System.Drawing.Size(108, 29)
        Me.FindBillNbr.TabIndex = 142
        '
        'OrderOfBusiness
        '
        Me.OrderOfBusiness.BackColor = System.Drawing.Color.WhiteSmoke
        Me.OrderOfBusiness.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OrderOfBusiness.ForeColor = System.Drawing.Color.Black
        Me.OrderOfBusiness.FormattingEnabled = True
        Me.OrderOfBusiness.ItemHeight = 22
        Me.OrderOfBusiness.Location = New System.Drawing.Point(37, 149)
        Me.OrderOfBusiness.Name = "OrderOfBusiness"
        Me.OrderOfBusiness.Size = New System.Drawing.Size(231, 114)
        Me.OrderOfBusiness.TabIndex = 148
        '
        'Calendar
        '
        Me.Calendar.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Calendar.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Calendar.ForeColor = System.Drawing.Color.Black
        Me.Calendar.FormattingEnabled = True
        Me.Calendar.ItemHeight = 22
        Me.Calendar.Location = New System.Drawing.Point(272, 149)
        Me.Calendar.Name = "Calendar"
        Me.Calendar.Size = New System.Drawing.Size(231, 114)
        Me.Calendar.TabIndex = 149
        '
        'Bill
        '
        Me.Bill.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Bill.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Bill.ForeColor = System.Drawing.Color.Black
        Me.Bill.FormattingEnabled = True
        Me.Bill.ItemHeight = 22
        Me.Bill.Location = New System.Drawing.Point(37, 328)
        Me.Bill.Name = "Bill"
        Me.Bill.Size = New System.Drawing.Size(465, 532)
        Me.Bill.TabIndex = 150
        '
        'Phrases
        '
        Me.Phrases.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Phrases.Cursor = System.Windows.Forms.Cursors.Default
        Me.Phrases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Phrases.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Phrases.ForeColor = System.Drawing.Color.Black
        Me.Phrases.Location = New System.Drawing.Point(505, 102)
        Me.Phrases.Name = "Phrases"
        Me.Phrases.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Phrases.Size = New System.Drawing.Size(709, 30)
        Me.Phrases.TabIndex = 152
        '
        'CurrentBill
        '
        Me.CurrentBill.BackColor = System.Drawing.Color.WhiteSmoke
        Me.CurrentBill.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentBill.ForeColor = System.Drawing.Color.Black
        Me.CurrentBill.Location = New System.Drawing.Point(748, 196)
        Me.CurrentBill.Multiline = True
        Me.CurrentBill.Name = "CurrentBill"
        Me.CurrentBill.Size = New System.Drawing.Size(466, 60)
        Me.CurrentBill.TabIndex = 153
        '
        'btnBIR
        '
        Me.btnBIR.BackColor = System.Drawing.Color.Silver
        Me.btnBIR.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnBIR.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnBIR.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnBIR.Location = New System.Drawing.Point(524, 546)
        Me.btnBIR.Name = "btnBIR"
        Me.btnBIR.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnBIR.Size = New System.Drawing.Size(205, 54)
        Me.btnBIR.TabIndex = 157
        Me.btnBIR.Text = "BIR"
        Me.btnBIR.UseVisualStyleBackColor = False
        '
        'btnDropLastPhrase
        '
        Me.btnDropLastPhrase.BackColor = System.Drawing.Color.Silver
        Me.btnDropLastPhrase.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnDropLastPhrase.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnDropLastPhrase.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDropLastPhrase.Location = New System.Drawing.Point(524, 482)
        Me.btnDropLastPhrase.Name = "btnDropLastPhrase"
        Me.btnDropLastPhrase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnDropLastPhrase.Size = New System.Drawing.Size(205, 54)
        Me.btnDropLastPhrase.TabIndex = 159
        Me.btnDropLastPhrase.Text = "Drop Last Phrase"
        Me.btnDropLastPhrase.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Image = CType(resources.GetObject("Label1.Image"), System.Drawing.Image)
        Me.Label1.Location = New System.Drawing.Point(183, 895)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(205, 21)
        Me.Label1.TabIndex = 162
        Me.Label1.Text = "Get A Special Order Calendar:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'SOCNbr
        '
        Me.SOCNbr.AcceptsReturn = True
        Me.SOCNbr.BackColor = System.Drawing.Color.WhiteSmoke
        Me.SOCNbr.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.SOCNbr.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SOCNbr.ForeColor = System.Drawing.Color.Navy
        Me.SOCNbr.Location = New System.Drawing.Point(393, 902)
        Me.SOCNbr.MaxLength = 0
        Me.SOCNbr.Name = "SOCNbr"
        Me.SOCNbr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SOCNbr.Size = New System.Drawing.Size(108, 29)
        Me.SOCNbr.TabIndex = 163
        '
        'btnFreeFormat
        '
        Me.btnFreeFormat.BackColor = System.Drawing.Color.Silver
        Me.btnFreeFormat.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnFreeFormat.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnFreeFormat.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnFreeFormat.Location = New System.Drawing.Point(524, 610)
        Me.btnFreeFormat.Name = "btnFreeFormat"
        Me.btnFreeFormat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnFreeFormat.Size = New System.Drawing.Size(205, 54)
        Me.btnFreeFormat.TabIndex = 164
        Me.btnFreeFormat.Text = "Free Format"
        Me.btnFreeFormat.UseVisualStyleBackColor = False
        '
        'ReceiveTimer
        '
        Me.ReceiveTimer.Interval = 1000
        '
        'btnRequestLastVoteID
        '
        Me.btnRequestLastVoteID.BackColor = System.Drawing.Color.Silver
        Me.btnRequestLastVoteID.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnRequestLastVoteID.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnRequestLastVoteID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnRequestLastVoteID.Location = New System.Drawing.Point(524, 674)
        Me.btnRequestLastVoteID.Name = "btnRequestLastVoteID"
        Me.btnRequestLastVoteID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnRequestLastVoteID.Size = New System.Drawing.Size(205, 54)
        Me.btnRequestLastVoteID.TabIndex = 167
        Me.btnRequestLastVoteID.Text = "Request Current Vote ID"
        Me.btnRequestLastVoteID.UseVisualStyleBackColor = False
        '
        'PingTimer
        '
        Me.PingTimer.Interval = 36000
        '
        'btnRecallDisplayedBill
        '
        Me.btnRecallDisplayedBill.BackColor = System.Drawing.Color.Fuchsia
        Me.btnRecallDisplayedBill.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnRecallDisplayedBill.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnRecallDisplayedBill.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnRecallDisplayedBill.Location = New System.Drawing.Point(524, 436)
        Me.btnRecallDisplayedBill.Name = "btnRecallDisplayedBill"
        Me.btnRecallDisplayedBill.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnRecallDisplayedBill.Size = New System.Drawing.Size(205, 36)
        Me.btnRecallDisplayedBill.TabIndex = 178
        Me.btnRecallDisplayedBill.Text = "Recall Displayed Bill"
        Me.btnRecallDisplayedBill.UseVisualStyleBackColor = False
        '
        'lblSelectedCommittee
        '
        Me.lblSelectedCommittee.AutoSize = True
        Me.lblSelectedCommittee.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblSelectedCommittee.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSelectedCommittee.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelectedCommittee.ForeColor = System.Drawing.Color.White
        Me.lblSelectedCommittee.Image = CType(resources.GetObject("lblSelectedCommittee.Image"), System.Drawing.Image)
        Me.lblSelectedCommittee.Location = New System.Drawing.Point(598, 35)
        Me.lblSelectedCommittee.Name = "lblSelectedCommittee"
        Me.lblSelectedCommittee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSelectedCommittee.Size = New System.Drawing.Size(179, 24)
        Me.lblSelectedCommittee.TabIndex = 112
        Me.lblSelectedCommittee.Text = "Selected Committee"
        Me.lblSelectedCommittee.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Committees
        '
        Me.Committees.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Committees.Cursor = System.Windows.Forms.Cursors.Default
        Me.Committees.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Committees.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Committees.ForeColor = System.Drawing.Color.Black
        Me.Committees.Location = New System.Drawing.Point(596, 55)
        Me.Committees.Name = "Committees"
        Me.Committees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Committees.Size = New System.Drawing.Size(617, 30)
        Me.Committees.TabIndex = 151
        '
        'cboDisplay
        '
        Me.cboDisplay.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cboDisplay.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDisplay.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.cboDisplay.FormattingEnabled = True
        Me.cboDisplay.Location = New System.Drawing.Point(548, 828)
        Me.cboDisplay.Name = "cboDisplay"
        Me.cboDisplay.Size = New System.Drawing.Size(155, 30)
        Me.cboDisplay.TabIndex = 187
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Image = CType(resources.GetObject("Label4.Image"), System.Drawing.Image)
        Me.Label4.Location = New System.Drawing.Point(508, 808)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(235, 17)
        Me.Label4.TabIndex = 188
        Me.Label4.Text = "Order Of Business Display Board"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'TC
        '
        Me.TC.Controls.Add(Me.TabPage)
        Me.TC.Controls.Add(Me.TabPage1)
        Me.TC.Controls.Add(Me.TabPage2)
        Me.TC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.TC.Location = New System.Drawing.Point(748, 322)
        Me.TC.Name = "TC"
        Me.TC.SelectedIndex = 0
        Me.TC.Size = New System.Drawing.Size(466, 534)
        Me.TC.TabIndex = 190
        '
        'TabPage
        '
        Me.TabPage.AllowDrop = True
        Me.TabPage.AutoScroll = True
        Me.TabPage.BackColor = System.Drawing.Color.LavenderBlush
        Me.TabPage.Controls.Add(Me.WorkData)
        Me.TabPage.Location = New System.Drawing.Point(4, 26)
        Me.TabPage.Name = "TabPage"
        Me.TabPage.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage.Size = New System.Drawing.Size(458, 504)
        Me.TabPage.TabIndex = 0
        Me.TabPage.Tag = "0"
        Me.TabPage.Text = "Work Area"
        Me.TabPage.UseVisualStyleBackColor = True
        '
        'WorkData
        '
        Me.WorkData.AcceptsReturn = True
        Me.WorkData.AllowDrop = True
        Me.WorkData.BackColor = System.Drawing.Color.WhiteSmoke
        Me.WorkData.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.WorkData.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.WorkData.ForeColor = System.Drawing.Color.Black
        Me.WorkData.Location = New System.Drawing.Point(4, 4)
        Me.WorkData.MaxLength = 0
        Me.WorkData.Multiline = True
        Me.WorkData.Name = "WorkData"
        Me.WorkData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.WorkData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.WorkData.Size = New System.Drawing.Size(451, 501)
        Me.WorkData.TabIndex = 205
        '
        'TabPage1
        '
        Me.TabPage1.AllowDrop = True
        Me.TabPage1.AutoScroll = True
        Me.TabPage1.BackColor = System.Drawing.Color.Azure
        Me.TabPage1.Controls.Add(Me.WorkData1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 26)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(458, 504)
        Me.TabPage1.TabIndex = 1
        Me.TabPage1.Tag = "1"
        Me.TabPage1.Text = "Area 1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'WorkData1
        '
        Me.WorkData1.AcceptsReturn = True
        Me.WorkData1.AllowDrop = True
        Me.WorkData1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.WorkData1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.WorkData1.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.WorkData1.ForeColor = System.Drawing.Color.Black
        Me.WorkData1.Location = New System.Drawing.Point(5, 5)
        Me.WorkData1.MaxLength = 0
        Me.WorkData1.Multiline = True
        Me.WorkData1.Name = "WorkData1"
        Me.WorkData1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.WorkData1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.WorkData1.Size = New System.Drawing.Size(450, 502)
        Me.WorkData1.TabIndex = 206
        '
        'TabPage2
        '
        Me.TabPage2.AllowDrop = True
        Me.TabPage2.AutoScroll = True
        Me.TabPage2.BackColor = System.Drawing.Color.LightYellow
        Me.TabPage2.Controls.Add(Me.WorkData2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 26)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(458, 504)
        Me.TabPage2.TabIndex = 2
        Me.TabPage2.Tag = "2"
        Me.TabPage2.Text = "Area 2"
        '
        'WorkData2
        '
        Me.WorkData2.AcceptsReturn = True
        Me.WorkData2.AllowDrop = True
        Me.WorkData2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.WorkData2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.WorkData2.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.WorkData2.ForeColor = System.Drawing.Color.Black
        Me.WorkData2.Location = New System.Drawing.Point(6, 6)
        Me.WorkData2.MaxLength = 0
        Me.WorkData2.Multiline = True
        Me.WorkData2.Name = "WorkData2"
        Me.WorkData2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.WorkData2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.WorkData2.Size = New System.Drawing.Size(450, 501)
        Me.WorkData2.TabIndex = 206
        '
        'btnClearWorkArea
        '
        Me.btnClearWorkArea.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnClearWorkArea.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnClearWorkArea.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClearWorkArea.ForeColor = System.Drawing.Color.Black
        Me.btnClearWorkArea.Location = New System.Drawing.Point(910, 858)
        Me.btnClearWorkArea.Name = "btnClearWorkArea"
        Me.btnClearWorkArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnClearWorkArea.Size = New System.Drawing.Size(131, 38)
        Me.btnClearWorkArea.TabIndex = 208
        Me.btnClearWorkArea.Text = "Clear Work Area"
        Me.btnClearWorkArea.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Maroon
        Me.Panel1.Controls.Add(Me.btnVote)
        Me.Panel1.Location = New System.Drawing.Point(525, 149)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(208, 121)
        Me.Panel1.TabIndex = 209
        '
        'btnVote
        '
        Me.btnVote.BackColor = System.Drawing.Color.LimeGreen
        Me.btnVote.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnVote.Font = New System.Drawing.Font("Arial", 16.0!, System.Drawing.FontStyle.Bold)
        Me.btnVote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnVote.Location = New System.Drawing.Point(57, 31)
        Me.btnVote.Name = "btnVote"
        Me.btnVote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnVote.Size = New System.Drawing.Size(97, 59)
        Me.btnVote.TabIndex = 162
        Me.btnVote.Text = "&Vote"
        Me.btnVote.UseVisualStyleBackColor = False
        '
        'lblAddPhraseToWorkArea
        '
        Me.lblAddPhraseToWorkArea.AutoSize = True
        Me.lblAddPhraseToWorkArea.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddPhraseToWorkArea.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddPhraseToWorkArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddPhraseToWorkArea.ForeColor = System.Drawing.Color.White
        Me.lblAddPhraseToWorkArea.Image = CType(resources.GetObject("lblAddPhraseToWorkArea.Image"), System.Drawing.Image)
        Me.lblAddPhraseToWorkArea.Location = New System.Drawing.Point(540, 325)
        Me.lblAddPhraseToWorkArea.Name = "lblAddPhraseToWorkArea"
        Me.lblAddPhraseToWorkArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddPhraseToWorkArea.Size = New System.Drawing.Size(231, 24)
        Me.lblAddPhraseToWorkArea.TabIndex = 211
        Me.lblAddPhraseToWorkArea.Text = "Add Phrase To Work Area"
        Me.lblAddPhraseToWorkArea.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblBills
        '
        Me.lblBills.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblBills.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBills.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBills.ForeColor = System.Drawing.Color.White
        Me.lblBills.Image = CType(resources.GetObject("lblBills.Image"), System.Drawing.Image)
        Me.lblBills.Location = New System.Drawing.Point(36, 310)
        Me.lblBills.Name = "lblBills"
        Me.lblBills.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBills.Size = New System.Drawing.Size(252, 16)
        Me.lblBills.TabIndex = 127
        Me.lblBills.Text = "Bills For The Current Calendar"
        Me.lblBills.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblCurrentOrderOfBusiness
        '
        Me.lblCurrentOrderOfBusiness.AutoSize = True
        Me.lblCurrentOrderOfBusiness.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblCurrentOrderOfBusiness.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrentOrderOfBusiness.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentOrderOfBusiness.ForeColor = System.Drawing.Color.White
        Me.lblCurrentOrderOfBusiness.Image = CType(resources.GetObject("lblCurrentOrderOfBusiness.Image"), System.Drawing.Image)
        Me.lblCurrentOrderOfBusiness.Location = New System.Drawing.Point(35, 82)
        Me.lblCurrentOrderOfBusiness.Name = "lblCurrentOrderOfBusiness"
        Me.lblCurrentOrderOfBusiness.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrentOrderOfBusiness.Size = New System.Drawing.Size(231, 24)
        Me.lblCurrentOrderOfBusiness.TabIndex = 105
        Me.lblCurrentOrderOfBusiness.Text = "Current Order Of Business"
        Me.lblCurrentOrderOfBusiness.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblFindBillNbr
        '
        Me.lblFindBillNbr.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lblFindBillNbr.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFindBillNbr.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!)
        Me.lblFindBillNbr.ForeColor = System.Drawing.Color.Black
        Me.lblFindBillNbr.Location = New System.Drawing.Point(311, 860)
        Me.lblFindBillNbr.Name = "lblFindBillNbr"
        Me.lblFindBillNbr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFindBillNbr.Size = New System.Drawing.Size(79, 27)
        Me.lblFindBillNbr.TabIndex = 143
        Me.lblFindBillNbr.Text = "Find Bill #:"
        Me.lblFindBillNbr.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnClearDisplay
        '
        Me.btnClearDisplay.BackColor = System.Drawing.Color.Fuchsia
        Me.btnClearDisplay.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnClearDisplay.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClearDisplay.ForeColor = System.Drawing.Color.Black
        Me.btnClearDisplay.Location = New System.Drawing.Point(691, 904)
        Me.btnClearDisplay.Name = "btnClearDisplay"
        Me.btnClearDisplay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnClearDisplay.Size = New System.Drawing.Size(111, 38)
        Me.btnClearDisplay.TabIndex = 213
        Me.btnClearDisplay.Text = "Clear Display"
        Me.btnClearDisplay.UseVisualStyleBackColor = False
        '
        'btnCreateHTMLPage
        '
        Me.btnCreateHTMLPage.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnCreateHTMLPage.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnCreateHTMLPage.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCreateHTMLPage.ForeColor = System.Drawing.Color.Black
        Me.btnCreateHTMLPage.Location = New System.Drawing.Point(934, 904)
        Me.btnCreateHTMLPage.Name = "btnCreateHTMLPage"
        Me.btnCreateHTMLPage.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnCreateHTMLPage.Size = New System.Drawing.Size(144, 38)
        Me.btnCreateHTMLPage.TabIndex = 214
        Me.btnCreateHTMLPage.Text = "Create HTML Page"
        Me.btnCreateHTMLPage.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Cyan
        Me.Label5.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(1046, 860)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(83, 27)
        Me.Label5.TabIndex = 215
        Me.Label5.Text = "Vote ID:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtVoteID
        '
        Me.txtVoteID.AcceptsReturn = True
        Me.txtVoteID.BackColor = System.Drawing.Color.WhiteSmoke
        Me.txtVoteID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtVoteID.Font = New System.Drawing.Font("Arial", 13.0!)
        Me.txtVoteID.ForeColor = System.Drawing.Color.Maroon
        Me.txtVoteID.Location = New System.Drawing.Point(1131, 860)
        Me.txtVoteID.MaxLength = 0
        Me.txtVoteID.Name = "txtVoteID"
        Me.txtVoteID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtVoteID.Size = New System.Drawing.Size(70, 32)
        Me.txtVoteID.TabIndex = 216
        Me.txtVoteID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Image = CType(resources.GetObject("Label3.Image"), System.Drawing.Image)
        Me.Label3.Location = New System.Drawing.Point(36, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(166, 17)
        Me.Label3.TabIndex = 183
        Me.Label3.Text = "Current Session"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnUnlock
        '
        Me.btnUnlock.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnUnlock.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnUnlock.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUnlock.ForeColor = System.Drawing.Color.Black
        Me.btnUnlock.Location = New System.Drawing.Point(1078, 904)
        Me.btnUnlock.Name = "btnUnlock"
        Me.btnUnlock.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnUnlock.Size = New System.Drawing.Size(72, 38)
        Me.btnUnlock.TabIndex = 217
        Me.btnUnlock.Text = "Unlock"
        Me.btnUnlock.UseVisualStyleBackColor = False
        '
        'btnExit
        '
        Me.btnExit.BackColor = System.Drawing.Color.Red
        Me.btnExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnExit.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnExit.ForeColor = System.Drawing.Color.Transparent
        Me.btnExit.Location = New System.Drawing.Point(1140, 12)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnExit.Size = New System.Drawing.Size(74, 34)
        Me.btnExit.TabIndex = 218
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = False
        '
        'btnCancelVote
        '
        Me.btnCancelVote.BackColor = System.Drawing.Color.Red
        Me.btnCancelVote.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnCancelVote.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancelVote.ForeColor = System.Drawing.Color.Transparent
        Me.btnCancelVote.Location = New System.Drawing.Point(575, 904)
        Me.btnCancelVote.Name = "btnCancelVote"
        Me.btnCancelVote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnCancelVote.Size = New System.Drawing.Size(116, 38)
        Me.btnCancelVote.TabIndex = 219
        Me.btnCancelVote.Text = "Cancel Vote"
        Me.btnCancelVote.UseVisualStyleBackColor = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Arial", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Red
        Me.Label6.Image = CType(resources.GetObject("Label6.Image"), System.Drawing.Image)
        Me.Label6.Location = New System.Drawing.Point(102, 917)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(338, 19)
        Me.Label6.TabIndex = 220
        Me.Label6.Text = "(Example: SR12 or DocID 161485-1 or SOC)"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSRChange
        '
        Me.txtSRChange.AcceptsReturn = True
        Me.txtSRChange.BackColor = System.Drawing.Color.WhiteSmoke
        Me.txtSRChange.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSRChange.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSRChange.ForeColor = System.Drawing.Color.Navy
        Me.txtSRChange.Location = New System.Drawing.Point(515, 867)
        Me.txtSRChange.MaxLength = 0
        Me.txtSRChange.Name = "txtSRChange"
        Me.txtSRChange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSRChange.Size = New System.Drawing.Size(62, 29)
        Me.txtSRChange.TabIndex = 221
        Me.txtSRChange.Visible = False
        '
        'btnSRSave
        '
        Me.btnSRSave.BackColor = System.Drawing.Color.LightGray
        Me.btnSRSave.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnSRSave.Enabled = False
        Me.btnSRSave.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.btnSRSave.ForeColor = System.Drawing.Color.Black
        Me.btnSRSave.Location = New System.Drawing.Point(575, 866)
        Me.btnSRSave.Name = "btnSRSave"
        Me.btnSRSave.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnSRSave.Size = New System.Drawing.Size(49, 26)
        Me.btnSRSave.TabIndex = 222
        Me.btnSRSave.Text = "Save"
        Me.btnSRSave.UseVisualStyleBackColor = True
        Me.btnSRSave.Visible = False
        '
        'AddPhrase
        '
        Me.AddPhrase.AcceptsReturn = True
        Me.AddPhrase.BackColor = System.Drawing.Color.WhiteSmoke
        Me.AddPhrase.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.AddPhrase.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.AddPhrase.ForeColor = System.Drawing.Color.Navy
        Me.AddPhrase.Location = New System.Drawing.Point(552, 344)
        Me.AddPhrase.MaxLength = 0
        Me.AddPhrase.Name = "AddPhrase"
        Me.AddPhrase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.AddPhrase.Size = New System.Drawing.Size(154, 29)
        Me.AddPhrase.TabIndex = 210
        '
        'btnStartService
        '
        Me.btnStartService.AutoSize = True
        Me.btnStartService.BackColor = System.Drawing.Color.Olive
        Me.btnStartService.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnStartService.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.btnStartService.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnStartService.Location = New System.Drawing.Point(1150, 902)
        Me.btnStartService.Name = "btnStartService"
        Me.btnStartService.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnStartService.Size = New System.Drawing.Size(106, 40)
        Me.btnStartService.TabIndex = 792
        Me.btnStartService.Text = "Start Queue"
        Me.btnStartService.UseVisualStyleBackColor = False
        '
        'btnFullClear
        '
        Me.btnFullClear.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnFullClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnFullClear.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFullClear.ForeColor = System.Drawing.Color.Black
        Me.btnFullClear.Location = New System.Drawing.Point(802, 904)
        Me.btnFullClear.Name = "btnFullClear"
        Me.btnFullClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnFullClear.Size = New System.Drawing.Size(132, 38)
        Me.btnFullClear.TabIndex = 793
        Me.btnFullClear.Text = "Full Clear Display"
        Me.btnFullClear.UseVisualStyleBackColor = False
        '
        'btnDownLoadBills
        '
        Me.btnDownLoadBills.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnDownLoadBills.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnDownLoadBills.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDownLoadBills.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDownLoadBills.Location = New System.Drawing.Point(524, 738)
        Me.btnDownLoadBills.Name = "btnDownLoadBills"
        Me.btnDownLoadBills.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnDownLoadBills.Size = New System.Drawing.Size(205, 36)
        Me.btnDownLoadBills.TabIndex = 795
        Me.btnDownLoadBills.Text = "Download Bills From ALIS"
        Me.btnDownLoadBills.UseVisualStyleBackColor = False
        '
        'btnSOC
        '
        Me.btnSOC.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnSOC.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnSOC.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSOC.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSOC.Location = New System.Drawing.Point(525, 390)
        Me.btnSOC.Name = "btnSOC"
        Me.btnSOC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnSOC.Size = New System.Drawing.Size(205, 36)
        Me.btnSOC.TabIndex = 796
        Me.btnSOC.Text = "Special Order Calendar"
        Me.btnSOC.UseVisualStyleBackColor = False
        '
        'TimerSOC
        '
        Me.TimerSOC.Interval = 1000
        '
        'frmChamberDisplay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.AutoSize = True
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.ClientSize = New System.Drawing.Size(1182, 777)
        Me.Controls.Add(Me.btnSOC)
        Me.Controls.Add(Me.btnDownLoadBills)
        Me.Controls.Add(Me.btnFullClear)
        Me.Controls.Add(Me.btnStartService)
        Me.Controls.Add(Me.btnSRSave)
        Me.Controls.Add(Me.txtSRChange)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.btnCancelVote)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnUnlock)
        Me.Controls.Add(Me.txtVoteID)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnCreateHTMLPage)
        Me.Controls.Add(Me.btnClearDisplay)
        Me.Controls.Add(Me.AddPhrase)
        Me.Controls.Add(Me.lblAddPhraseToWorkArea)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnClearWorkArea)
        Me.Controls.Add(Me.TC)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cboDisplay)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnRecallDisplayedBill)
        Me.Controls.Add(Me.btnRequestLastVoteID)
        Me.Controls.Add(Me.btnFreeFormat)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.SOCNbr)
        Me.Controls.Add(Me.btnBIR)
        Me.Controls.Add(Me.btnDropLastPhrase)
        Me.Controls.Add(Me.CurrentBill)
        Me.Controls.Add(Me.Phrases)
        Me.Controls.Add(Me.Committees)
        Me.Controls.Add(Me.Bill)
        Me.Controls.Add(Me.Calendar)
        Me.Controls.Add(Me.OrderOfBusiness)
        Me.Controls.Add(Me.Senators)
        Me.Controls.Add(Me.LegislativeDay)
        Me.Controls.Add(Me.btnUpdateDisplay)
        Me.Controls.Add(Me.InsertText)
        Me.Controls.Add(Me.btnPhraseMaintenance)
        Me.Controls.Add(Me.btnPreviousBill)
        Me.Controls.Add(Me.btnNextBill)
        Me.Controls.Add(Me.CurrentCalendar)
        Me.Controls.Add(Me.CurrentOrderOfBusiness)
        Me.Controls.Add(Me.lblFindBillNbr)
        Me.Controls.Add(Me.lblLegislativeDay)
        Me.Controls.Add(Me.lblSession)
        Me.Controls.Add(Me.lblTestMode)
        Me.Controls.Add(Me.lblInsertText)
        Me.Controls.Add(Me.lblBills)
        Me.Controls.Add(Me.lblChamberLight)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblWorkAreaDisplayed)
        Me.Controls.Add(Me.lblWorkArea)
        Me.Controls.Add(Me.lblSelectedPhrase)
        Me.Controls.Add(Me.lblSelectedCommittee)
        Me.Controls.Add(Me.lblSelectedSenator)
        Me.Controls.Add(Me.lblCurrentBill)
        Me.Controls.Add(Me.lblCurrentCalendar)
        Me.Controls.Add(Me.lblCalendars)
        Me.Controls.Add(Me.lblCurrentOrderOfBusiness)
        Me.Controls.Add(Me.lblOrdersOfBusiness)
        Me.Controls.Add(Me.FindBillNbr)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.MaximizeBox = False
        Me.Name = "frmChamberDisplay"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Senate Voting System - Order Of Business"
        Me.TC.ResumeLayout(False)
        Me.TabPage.ResumeLayout(False)
        Me.TabPage.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents Senators As System.Windows.Forms.ComboBox
    Public WithEvents LegislativeDay As System.Windows.Forms.TextBox
    Public WithEvents btnUpdateDisplay As System.Windows.Forms.Button
    Public WithEvents InsertText As System.Windows.Forms.TextBox
    Public WithEvents btnPhraseMaintenance As System.Windows.Forms.Button
    Public WithEvents btnPreviousBill As System.Windows.Forms.Button
    Public WithEvents btnNextBill As System.Windows.Forms.Button
    Public WithEvents CurrentCalendar As System.Windows.Forms.TextBox
    Public WithEvents CurrentOrderOfBusiness As System.Windows.Forms.TextBox
    Public WithEvents lblLegislativeDay As System.Windows.Forms.Label
    Public WithEvents lblSession As System.Windows.Forms.Label
    Public WithEvents lblTestMode As System.Windows.Forms.Label
    Public WithEvents lblInsertText As System.Windows.Forms.Label
    Public WithEvents lblChamberLight As System.Windows.Forms.Label
    Public WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents lblWorkAreaDisplayed As System.Windows.Forms.Label
    Public WithEvents lblWorkArea As System.Windows.Forms.Label
    Public WithEvents lblSelectedPhrase As System.Windows.Forms.Label
    Public WithEvents lblSelectedSenator As System.Windows.Forms.Label
    Public WithEvents lblCurrentBill As System.Windows.Forms.Label
    Public WithEvents lblCurrentCalendar As System.Windows.Forms.Label
    Public WithEvents lblCalendars As System.Windows.Forms.Label
    Public WithEvents lblOrdersOfBusiness As System.Windows.Forms.Label
    Public WithEvents FindBillNbr As System.Windows.Forms.TextBox
    Friend WithEvents OrderOfBusiness As System.Windows.Forms.ListBox
    Friend WithEvents Calendar As System.Windows.Forms.ListBox
    Friend WithEvents Bill As System.Windows.Forms.ListBox
    Public WithEvents Phrases As System.Windows.Forms.ComboBox
    Friend WithEvents CurrentBill As System.Windows.Forms.TextBox
    Public WithEvents btnBIR As System.Windows.Forms.Button
    Public WithEvents btnDropLastPhrase As System.Windows.Forms.Button
    Public WithEvents Label1 As System.Windows.Forms.Label
    Public WithEvents SOCNbr As System.Windows.Forms.TextBox
    Public WithEvents btnFreeFormat As System.Windows.Forms.Button
    Friend WithEvents ReceiveTimer As System.Windows.Forms.Timer
    Public WithEvents btnRequestLastVoteID As System.Windows.Forms.Button
    Friend WithEvents PingTimer As System.Windows.Forms.Timer
    Public WithEvents btnRecallDisplayedBill As System.Windows.Forms.Button
    Public WithEvents lblSelectedCommittee As System.Windows.Forms.Label
    Public WithEvents Committees As System.Windows.Forms.ComboBox
    Friend WithEvents cboDisplay As System.Windows.Forms.ComboBox
    Public WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TC As System.Windows.Forms.TabControl
    Friend WithEvents TabPage As System.Windows.Forms.TabPage
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents WorkData As System.Windows.Forms.TextBox
    Public WithEvents WorkData1 As System.Windows.Forms.TextBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents WorkData2 As System.Windows.Forms.TextBox
    Public WithEvents btnClearWorkArea As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents lblAddPhraseToWorkArea As System.Windows.Forms.Label
    Public WithEvents lblBills As System.Windows.Forms.Label
    Public WithEvents lblCurrentOrderOfBusiness As System.Windows.Forms.Label
    Public WithEvents lblFindBillNbr As System.Windows.Forms.Label
    Public WithEvents btnClearDisplay As System.Windows.Forms.Button
    Public WithEvents btnCreateHTMLPage As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents txtVoteID As System.Windows.Forms.TextBox
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents btnUnlock As System.Windows.Forms.Button
    Public WithEvents btnExit As System.Windows.Forms.Button
    Public WithEvents btnCancelVote As System.Windows.Forms.Button
    Public WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents txtSRChange As System.Windows.Forms.TextBox
    Public WithEvents btnSRSave As System.Windows.Forms.Button
    Public WithEvents AddPhrase As System.Windows.Forms.TextBox
    Public WithEvents btnStartService As System.Windows.Forms.Button
    Public WithEvents btnVote As System.Windows.Forms.Button
    Public WithEvents btnFullClear As System.Windows.Forms.Button
    Public WithEvents btnDownLoadBills As System.Windows.Forms.Button
    Public WithEvents btnSOC As System.Windows.Forms.Button
    Friend WithEvents TimerSOC As System.Windows.Forms.Timer
End Class
