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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChamberDisplay))
        Me.Senators = New System.Windows.Forms.ComboBox
        Me.LegislativeDay = New System.Windows.Forms.TextBox
        Me.btnCancelVote = New System.Windows.Forms.Button
        Me.btnUpdateDisplay = New System.Windows.Forms.Button
        Me.InsertText = New System.Windows.Forms.TextBox
        Me.btnPhraseMaintenance = New System.Windows.Forms.Button
        Me.btnPreviousBill = New System.Windows.Forms.Button
        Me.btnNextBill = New System.Windows.Forms.Button
        Me.CurrentCalendar = New System.Windows.Forms.TextBox
        Me.CurrentOrderOfBusiness = New System.Windows.Forms.TextBox
        Me.lblFindBillNbr = New System.Windows.Forms.Label
        Me.lblLegislativeDay = New System.Windows.Forms.Label
        Me.lblSession = New System.Windows.Forms.Label
        Me.lblTestMode = New System.Windows.Forms.Label
        Me.lblInsertText = New System.Windows.Forms.Label
        Me.lblChamberLight = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblWorkAreaDisplayed = New System.Windows.Forms.Label
        Me.lblWorkArea = New System.Windows.Forms.Label
        Me.lblSelectedPhrase = New System.Windows.Forms.Label
        Me.lblSelectedSenator = New System.Windows.Forms.Label
        Me.lblCurrentBill = New System.Windows.Forms.Label
        Me._lblCurrentCalendar_0 = New System.Windows.Forms.Label
        Me.lblCalendars = New System.Windows.Forms.Label
        Me.lblCurrentOrderOfBusiness = New System.Windows.Forms.Label
        Me.lblOrdersOfBusiness = New System.Windows.Forms.Label
        Me.FindBillNbr = New System.Windows.Forms.TextBox
        Me.OrderOfBusiness = New System.Windows.Forms.ListBox
        Me.Calendar = New System.Windows.Forms.ListBox
        Me.Bill = New System.Windows.Forms.ListBox
        Me.Phrases = New System.Windows.Forms.ComboBox
        Me.CurrentBill = New System.Windows.Forms.TextBox
        Me.btnBIR = New System.Windows.Forms.Button
        Me.btnDropLastPhrase = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.SOCNbr = New System.Windows.Forms.TextBox
        Me.btnFreeFormat = New System.Windows.Forms.Button
        Me.ReceiveTimer = New System.Windows.Forms.Timer(Me.components)
        Me.btnRequestLastVoteID = New System.Windows.Forms.Button
        Me.PingTimer = New System.Windows.Forms.Timer(Me.components)
        Me.btnUnlock = New System.Windows.Forms.Button
        Me.btnRecallDisplayedBill = New System.Windows.Forms.Button
        Me.lblSelectedCommittee = New System.Windows.Forms.Label
        Me.Committees = New System.Windows.Forms.ComboBox
        Me.lblVoteID = New System.Windows.Forms.Label
        Me.VoteID = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnExit = New System.Windows.Forms.Button
        Me.cboDisplay = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.TC = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.WorkData = New System.Windows.Forms.TextBox
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.WorkData2 = New System.Windows.Forms.TextBox
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.WorkData3 = New System.Windows.Forms.TextBox
        Me.btnClear = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnVote = New System.Windows.Forms.Button
        Me.AddPhrase = New System.Windows.Forms.TextBox
        Me.lblAddPhraseToWorkArea = New System.Windows.Forms.Label
        Me.btnSOC = New System.Windows.Forms.Button
        Me.btnChange = New System.Windows.Forms.Button
        Me.lblBills = New System.Windows.Forms.Label
        Me.TC.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Senators
        '
        Me.Senators.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Senators.Cursor = System.Windows.Forms.Cursors.Default
        Me.Senators.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Senators.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Senators.ForeColor = System.Drawing.Color.Navy
        Me.Senators.Location = New System.Drawing.Point(390, 62)
        Me.Senators.MaxDropDownItems = 35
        Me.Senators.Name = "Senators"
        Me.Senators.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Senators.Size = New System.Drawing.Size(148, 22)
        Me.Senators.TabIndex = 141
        '
        'LegislativeDay
        '
        Me.LegislativeDay.AcceptsReturn = True
        Me.LegislativeDay.BackColor = System.Drawing.Color.WhiteSmoke
        Me.LegislativeDay.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.LegislativeDay.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LegislativeDay.ForeColor = System.Drawing.Color.Navy
        Me.LegislativeDay.Location = New System.Drawing.Point(254, 62)
        Me.LegislativeDay.MaxLength = 0
        Me.LegislativeDay.Name = "LegislativeDay"
        Me.LegislativeDay.ReadOnly = True
        Me.LegislativeDay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LegislativeDay.Size = New System.Drawing.Size(134, 22)
        Me.LegislativeDay.TabIndex = 140
        '
        'btnCancelVote
        '
        Me.btnCancelVote.BackColor = System.Drawing.SystemColors.Control
        Me.btnCancelVote.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnCancelVote.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancelVote.ForeColor = System.Drawing.Color.Black
        Me.btnCancelVote.Location = New System.Drawing.Point(112, 28)
        Me.btnCancelVote.Name = "btnCancelVote"
        Me.btnCancelVote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnCancelVote.Size = New System.Drawing.Size(72, 41)
        Me.btnCancelVote.TabIndex = 133
        Me.btnCancelVote.Text = "Cancel Vote"
        Me.btnCancelVote.UseVisualStyleBackColor = False
        '
        'btnUpdateDisplay
        '
        Me.btnUpdateDisplay.BackColor = System.Drawing.SystemColors.Control
        Me.btnUpdateDisplay.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnUpdateDisplay.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdateDisplay.ForeColor = System.Drawing.Color.Black
        Me.btnUpdateDisplay.Location = New System.Drawing.Point(708, 638)
        Me.btnUpdateDisplay.Name = "btnUpdateDisplay"
        Me.btnUpdateDisplay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnUpdateDisplay.Size = New System.Drawing.Size(101, 26)
        Me.btnUpdateDisplay.TabIndex = 131
        Me.btnUpdateDisplay.Text = "Update Display"
        Me.btnUpdateDisplay.UseVisualStyleBackColor = False
        '
        'InsertText
        '
        Me.InsertText.AcceptsReturn = True
        Me.InsertText.BackColor = System.Drawing.Color.WhiteSmoke
        Me.InsertText.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.InsertText.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InsertText.ForeColor = System.Drawing.Color.Black
        Me.InsertText.Location = New System.Drawing.Point(648, 156)
        Me.InsertText.MaxLength = 0
        Me.InsertText.Name = "InsertText"
        Me.InsertText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.InsertText.Size = New System.Drawing.Size(413, 21)
        Me.InsertText.TabIndex = 129
        '
        'btnPhraseMaintenance
        '
        Me.btnPhraseMaintenance.BackColor = System.Drawing.SystemColors.Control
        Me.btnPhraseMaintenance.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnPhraseMaintenance.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnPhraseMaintenance.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPhraseMaintenance.ForeColor = System.Drawing.Color.Black
        Me.btnPhraseMaintenance.Location = New System.Drawing.Point(933, 86)
        Me.btnPhraseMaintenance.Name = "btnPhraseMaintenance"
        Me.btnPhraseMaintenance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnPhraseMaintenance.Size = New System.Drawing.Size(128, 21)
        Me.btnPhraseMaintenance.TabIndex = 128
        Me.btnPhraseMaintenance.Text = "Phrase Maintenance"
        Me.btnPhraseMaintenance.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnPhraseMaintenance.UseVisualStyleBackColor = False
        '
        'btnPreviousBill
        '
        Me.btnPreviousBill.BackColor = System.Drawing.SystemColors.Control
        Me.btnPreviousBill.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnPreviousBill.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPreviousBill.ForeColor = System.Drawing.Color.Black
        Me.btnPreviousBill.Location = New System.Drawing.Point(67, 640)
        Me.btnPreviousBill.Name = "btnPreviousBill"
        Me.btnPreviousBill.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnPreviousBill.Size = New System.Drawing.Size(84, 26)
        Me.btnPreviousBill.TabIndex = 120
        Me.btnPreviousBill.Text = "Previous Bill"
        Me.btnPreviousBill.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnPreviousBill.UseVisualStyleBackColor = False
        '
        'btnNextBill
        '
        Me.btnNextBill.BackColor = System.Drawing.SystemColors.Control
        Me.btnNextBill.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnNextBill.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNextBill.ForeColor = System.Drawing.Color.Black
        Me.btnNextBill.Location = New System.Drawing.Point(156, 640)
        Me.btnNextBill.Name = "btnNextBill"
        Me.btnNextBill.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnNextBill.Size = New System.Drawing.Size(60, 26)
        Me.btnNextBill.TabIndex = 114
        Me.btnNextBill.Text = "Next Bill"
        Me.btnNextBill.UseVisualStyleBackColor = False
        '
        'CurrentCalendar
        '
        Me.CurrentCalendar.AcceptsReturn = True
        Me.CurrentCalendar.BackColor = System.Drawing.Color.WhiteSmoke
        Me.CurrentCalendar.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.CurrentCalendar.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentCalendar.ForeColor = System.Drawing.Color.Black
        Me.CurrentCalendar.Location = New System.Drawing.Point(213, 110)
        Me.CurrentCalendar.MaxLength = 0
        Me.CurrentCalendar.Name = "CurrentCalendar"
        Me.CurrentCalendar.ReadOnly = True
        Me.CurrentCalendar.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CurrentCalendar.Size = New System.Drawing.Size(195, 22)
        Me.CurrentCalendar.TabIndex = 107
        Me.CurrentCalendar.TabStop = False
        '
        'CurrentOrderOfBusiness
        '
        Me.CurrentOrderOfBusiness.AcceptsReturn = True
        Me.CurrentOrderOfBusiness.BackColor = System.Drawing.Color.WhiteSmoke
        Me.CurrentOrderOfBusiness.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.CurrentOrderOfBusiness.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentOrderOfBusiness.ForeColor = System.Drawing.Color.Black
        Me.CurrentOrderOfBusiness.Location = New System.Drawing.Point(17, 110)
        Me.CurrentOrderOfBusiness.MaxLength = 0
        Me.CurrentOrderOfBusiness.Name = "CurrentOrderOfBusiness"
        Me.CurrentOrderOfBusiness.ReadOnly = True
        Me.CurrentOrderOfBusiness.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.CurrentOrderOfBusiness.Size = New System.Drawing.Size(195, 22)
        Me.CurrentOrderOfBusiness.TabIndex = 103
        Me.CurrentOrderOfBusiness.TabStop = False
        '
        'lblFindBillNbr
        '
        Me.lblFindBillNbr.BackColor = System.Drawing.SystemColors.Control
        Me.lblFindBillNbr.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblFindBillNbr.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFindBillNbr.ForeColor = System.Drawing.Color.White
        Me.lblFindBillNbr.Image = CType(resources.GetObject("lblFindBillNbr.Image"), System.Drawing.Image)
        Me.lblFindBillNbr.Location = New System.Drawing.Point(221, 646)
        Me.lblFindBillNbr.Name = "lblFindBillNbr"
        Me.lblFindBillNbr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFindBillNbr.Size = New System.Drawing.Size(55, 24)
        Me.lblFindBillNbr.TabIndex = 143
        Me.lblFindBillNbr.Text = "Find Bill #:"
        Me.lblFindBillNbr.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblLegislativeDay
        '
        Me.lblLegislativeDay.AutoSize = True
        Me.lblLegislativeDay.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblLegislativeDay.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLegislativeDay.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLegislativeDay.ForeColor = System.Drawing.Color.White
        Me.lblLegislativeDay.Image = CType(resources.GetObject("lblLegislativeDay.Image"), System.Drawing.Image)
        Me.lblLegislativeDay.Location = New System.Drawing.Point(252, 46)
        Me.lblLegislativeDay.Name = "lblLegislativeDay"
        Me.lblLegislativeDay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLegislativeDay.Size = New System.Drawing.Size(140, 16)
        Me.lblLegislativeDay.TabIndex = 139
        Me.lblLegislativeDay.Text = "Legislative Day / Date"
        Me.lblLegislativeDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblSession
        '
        Me.lblSession.BackColor = System.Drawing.Color.WhiteSmoke
        Me.lblSession.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSession.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSession.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSession.ForeColor = System.Drawing.Color.Navy
        Me.lblSession.Location = New System.Drawing.Point(80, 62)
        Me.lblSession.Name = "lblSession"
        Me.lblSession.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSession.Size = New System.Drawing.Size(172, 22)
        Me.lblSession.TabIndex = 138
        Me.lblSession.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblTestMode
        '
        Me.lblTestMode.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lblTestMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTestMode.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTestMode.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTestMode.ForeColor = System.Drawing.Color.Black
        Me.lblTestMode.Location = New System.Drawing.Point(988, 12)
        Me.lblTestMode.Name = "lblTestMode"
        Me.lblTestMode.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTestMode.Size = New System.Drawing.Size(69, 32)
        Me.lblTestMode.TabIndex = 137
        Me.lblTestMode.Text = "Test Mode"
        Me.lblTestMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblInsertText
        '
        Me.lblInsertText.AutoSize = True
        Me.lblInsertText.BackColor = System.Drawing.SystemColors.Control
        Me.lblInsertText.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblInsertText.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInsertText.ForeColor = System.Drawing.Color.White
        Me.lblInsertText.Image = CType(resources.GetObject("lblInsertText.Image"), System.Drawing.Image)
        Me.lblInsertText.Location = New System.Drawing.Point(647, 140)
        Me.lblInsertText.Name = "lblInsertText"
        Me.lblInsertText.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblInsertText.Size = New System.Drawing.Size(72, 16)
        Me.lblInsertText.TabIndex = 130
        Me.lblInsertText.Text = "Insert Text:"
        Me.lblInsertText.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblChamberLight
        '
        Me.lblChamberLight.AutoSize = True
        Me.lblChamberLight.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblChamberLight.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblChamberLight.Font = New System.Drawing.Font("Palatino Linotype", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChamberLight.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblChamberLight.Location = New System.Drawing.Point(21, 17)
        Me.lblChamberLight.Name = "lblChamberLight"
        Me.lblChamberLight.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblChamberLight.Size = New System.Drawing.Size(133, 21)
        Me.lblChamberLight.TabIndex = 124
        Me.lblChamberLight.Text = "Display Next Bill"
        Me.lblChamberLight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(828, 101)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(1, 5)
        Me.Label2.TabIndex = 123
        Me.Label2.Text = "Label2"
        '
        'lblWorkAreaDisplayed
        '
        Me.lblWorkAreaDisplayed.BackColor = System.Drawing.Color.Maroon
        Me.lblWorkAreaDisplayed.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWorkAreaDisplayed.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWorkAreaDisplayed.ForeColor = System.Drawing.Color.White
        Me.lblWorkAreaDisplayed.Location = New System.Drawing.Point(712, 258)
        Me.lblWorkAreaDisplayed.Name = "lblWorkAreaDisplayed"
        Me.lblWorkAreaDisplayed.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWorkAreaDisplayed.Size = New System.Drawing.Size(304, 21)
        Me.lblWorkAreaDisplayed.TabIndex = 117
        Me.lblWorkAreaDisplayed.Text = "This Work Area Is Currently Displayed"
        Me.lblWorkAreaDisplayed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblWorkAreaDisplayed.Visible = False
        '
        'lblWorkArea
        '
        Me.lblWorkArea.AutoSize = True
        Me.lblWorkArea.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblWorkArea.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblWorkArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWorkArea.ForeColor = System.Drawing.Color.White
        Me.lblWorkArea.Image = CType(resources.GetObject("lblWorkArea.Image"), System.Drawing.Image)
        Me.lblWorkArea.Location = New System.Drawing.Point(648, 281)
        Me.lblWorkArea.Name = "lblWorkArea"
        Me.lblWorkArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblWorkArea.Size = New System.Drawing.Size(188, 16)
        Me.lblWorkArea.TabIndex = 116
        Me.lblWorkArea.Text = "Work Area For The Current Bill"
        Me.lblWorkArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSelectedPhrase
        '
        Me.lblSelectedPhrase.AutoSize = True
        Me.lblSelectedPhrase.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblSelectedPhrase.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSelectedPhrase.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelectedPhrase.ForeColor = System.Drawing.Color.White
        Me.lblSelectedPhrase.Image = CType(resources.GetObject("lblSelectedPhrase.Image"), System.Drawing.Image)
        Me.lblSelectedPhrase.Location = New System.Drawing.Point(409, 93)
        Me.lblSelectedPhrase.Name = "lblSelectedPhrase"
        Me.lblSelectedPhrase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSelectedPhrase.Size = New System.Drawing.Size(108, 16)
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
        Me.lblSelectedSenator.Location = New System.Drawing.Point(391, 46)
        Me.lblSelectedSenator.Name = "lblSelectedSenator"
        Me.lblSelectedSenator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSelectedSenator.Size = New System.Drawing.Size(112, 16)
        Me.lblSelectedSenator.TabIndex = 111
        Me.lblSelectedSenator.Text = "Selected Senator"
        Me.lblSelectedSenator.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblCurrentBill
        '
        Me.lblCurrentBill.AutoSize = True
        Me.lblCurrentBill.BackColor = System.Drawing.SystemColors.Control
        Me.lblCurrentBill.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrentBill.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentBill.ForeColor = System.Drawing.Color.White
        Me.lblCurrentBill.Image = CType(resources.GetObject("lblCurrentBill.Image"), System.Drawing.Image)
        Me.lblCurrentBill.Location = New System.Drawing.Point(647, 180)
        Me.lblCurrentBill.Name = "lblCurrentBill"
        Me.lblCurrentBill.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrentBill.Size = New System.Drawing.Size(71, 16)
        Me.lblCurrentBill.TabIndex = 110
        Me.lblCurrentBill.Text = "Current Bill"
        Me.lblCurrentBill.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        '_lblCurrentCalendar_0
        '
        Me._lblCurrentCalendar_0.AutoSize = True
        Me._lblCurrentCalendar_0.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me._lblCurrentCalendar_0.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblCurrentCalendar_0.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblCurrentCalendar_0.ForeColor = System.Drawing.Color.White
        Me._lblCurrentCalendar_0.Image = CType(resources.GetObject("_lblCurrentCalendar_0.Image"), System.Drawing.Image)
        Me._lblCurrentCalendar_0.Location = New System.Drawing.Point(212, 95)
        Me._lblCurrentCalendar_0.Name = "_lblCurrentCalendar_0"
        Me._lblCurrentCalendar_0.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblCurrentCalendar_0.Size = New System.Drawing.Size(108, 16)
        Me._lblCurrentCalendar_0.TabIndex = 108
        Me._lblCurrentCalendar_0.Text = "Current Calendar"
        Me._lblCurrentCalendar_0.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblCalendars
        '
        Me.lblCalendars.AutoSize = True
        Me.lblCalendars.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblCalendars.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCalendars.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCalendars.ForeColor = System.Drawing.Color.White
        Me.lblCalendars.Image = CType(resources.GetObject("lblCalendars.Image"), System.Drawing.Image)
        Me.lblCalendars.Location = New System.Drawing.Point(213, 140)
        Me.lblCalendars.Name = "lblCalendars"
        Me.lblCalendars.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCalendars.Size = New System.Drawing.Size(70, 16)
        Me.lblCalendars.TabIndex = 106
        Me.lblCalendars.Text = "Calendars"
        Me.lblCalendars.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblCurrentOrderOfBusiness
        '
        Me.lblCurrentOrderOfBusiness.AutoSize = True
        Me.lblCurrentOrderOfBusiness.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblCurrentOrderOfBusiness.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCurrentOrderOfBusiness.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentOrderOfBusiness.ForeColor = System.Drawing.Color.White
        Me.lblCurrentOrderOfBusiness.Image = CType(resources.GetObject("lblCurrentOrderOfBusiness.Image"), System.Drawing.Image)
        Me.lblCurrentOrderOfBusiness.Location = New System.Drawing.Point(15, 95)
        Me.lblCurrentOrderOfBusiness.Name = "lblCurrentOrderOfBusiness"
        Me.lblCurrentOrderOfBusiness.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCurrentOrderOfBusiness.Size = New System.Drawing.Size(161, 16)
        Me.lblCurrentOrderOfBusiness.TabIndex = 105
        Me.lblCurrentOrderOfBusiness.Text = "Current Order Of Business"
        Me.lblCurrentOrderOfBusiness.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblOrdersOfBusiness
        '
        Me.lblOrdersOfBusiness.AutoSize = True
        Me.lblOrdersOfBusiness.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblOrdersOfBusiness.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOrdersOfBusiness.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrdersOfBusiness.ForeColor = System.Drawing.Color.White
        Me.lblOrdersOfBusiness.Image = CType(resources.GetObject("lblOrdersOfBusiness.Image"), System.Drawing.Image)
        Me.lblOrdersOfBusiness.Location = New System.Drawing.Point(15, 140)
        Me.lblOrdersOfBusiness.Name = "lblOrdersOfBusiness"
        Me.lblOrdersOfBusiness.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOrdersOfBusiness.Size = New System.Drawing.Size(123, 16)
        Me.lblOrdersOfBusiness.TabIndex = 104
        Me.lblOrdersOfBusiness.Text = "Orders Of Business"
        Me.lblOrdersOfBusiness.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'FindBillNbr
        '
        Me.FindBillNbr.AcceptsReturn = True
        Me.FindBillNbr.BackColor = System.Drawing.Color.WhiteSmoke
        Me.FindBillNbr.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.FindBillNbr.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FindBillNbr.ForeColor = System.Drawing.Color.Navy
        Me.FindBillNbr.Location = New System.Drawing.Point(276, 640)
        Me.FindBillNbr.MaxLength = 0
        Me.FindBillNbr.Name = "FindBillNbr"
        Me.FindBillNbr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FindBillNbr.Size = New System.Drawing.Size(76, 22)
        Me.FindBillNbr.TabIndex = 142
        '
        'OrderOfBusiness
        '
        Me.OrderOfBusiness.BackColor = System.Drawing.Color.WhiteSmoke
        Me.OrderOfBusiness.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OrderOfBusiness.ForeColor = System.Drawing.Color.Black
        Me.OrderOfBusiness.FormattingEnabled = True
        Me.OrderOfBusiness.ItemHeight = 16
        Me.OrderOfBusiness.Location = New System.Drawing.Point(17, 156)
        Me.OrderOfBusiness.Name = "OrderOfBusiness"
        Me.OrderOfBusiness.Size = New System.Drawing.Size(195, 100)
        Me.OrderOfBusiness.TabIndex = 148
        '
        'Calendar
        '
        Me.Calendar.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Calendar.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Calendar.ForeColor = System.Drawing.Color.Black
        Me.Calendar.FormattingEnabled = True
        Me.Calendar.ItemHeight = 16
        Me.Calendar.Location = New System.Drawing.Point(214, 156)
        Me.Calendar.Name = "Calendar"
        Me.Calendar.Size = New System.Drawing.Size(195, 100)
        Me.Calendar.TabIndex = 149
        '
        'Bill
        '
        Me.Bill.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Bill.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Bill.ForeColor = System.Drawing.Color.Black
        Me.Bill.FormattingEnabled = True
        Me.Bill.ItemHeight = 15
        Me.Bill.Location = New System.Drawing.Point(17, 297)
        Me.Bill.Name = "Bill"
        Me.Bill.Size = New System.Drawing.Size(391, 334)
        Me.Bill.TabIndex = 150
        '
        'Phrases
        '
        Me.Phrases.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Phrases.Cursor = System.Windows.Forms.Cursors.Default
        Me.Phrases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Phrases.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Phrases.ForeColor = System.Drawing.Color.Black
        Me.Phrases.Location = New System.Drawing.Point(412, 109)
        Me.Phrases.Name = "Phrases"
        Me.Phrases.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Phrases.Size = New System.Drawing.Size(649, 23)
        Me.Phrases.TabIndex = 152
        '
        'CurrentBill
        '
        Me.CurrentBill.BackColor = System.Drawing.Color.WhiteSmoke
        Me.CurrentBill.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentBill.ForeColor = System.Drawing.Color.Black
        Me.CurrentBill.Location = New System.Drawing.Point(648, 196)
        Me.CurrentBill.Multiline = True
        Me.CurrentBill.Name = "CurrentBill"
        Me.CurrentBill.Size = New System.Drawing.Size(413, 60)
        Me.CurrentBill.TabIndex = 153
        '
        'btnBIR
        '
        Me.btnBIR.BackColor = System.Drawing.SystemColors.Control
        Me.btnBIR.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnBIR.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBIR.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnBIR.Location = New System.Drawing.Point(463, 420)
        Me.btnBIR.Name = "btnBIR"
        Me.btnBIR.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnBIR.Size = New System.Drawing.Size(133, 30)
        Me.btnBIR.TabIndex = 157
        Me.btnBIR.Text = "BIR"
        Me.btnBIR.UseVisualStyleBackColor = False
        '
        'btnDropLastPhrase
        '
        Me.btnDropLastPhrase.BackColor = System.Drawing.SystemColors.Control
        Me.btnDropLastPhrase.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnDropLastPhrase.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDropLastPhrase.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDropLastPhrase.Location = New System.Drawing.Point(463, 384)
        Me.btnDropLastPhrase.Name = "btnDropLastPhrase"
        Me.btnDropLastPhrase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnDropLastPhrase.Size = New System.Drawing.Size(133, 30)
        Me.btnDropLastPhrase.TabIndex = 159
        Me.btnDropLastPhrase.Text = "Drop Last Phrase"
        Me.btnDropLastPhrase.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Image = CType(resources.GetObject("Label1.Image"), System.Drawing.Image)
        Me.Label1.Location = New System.Drawing.Point(429, 543)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(206, 17)
        Me.Label1.TabIndex = 162
        Me.Label1.Text = "Get A Special Order Calendar"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'SOCNbr
        '
        Me.SOCNbr.AcceptsReturn = True
        Me.SOCNbr.BackColor = System.Drawing.Color.WhiteSmoke
        Me.SOCNbr.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.SOCNbr.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SOCNbr.ForeColor = System.Drawing.Color.Navy
        Me.SOCNbr.Location = New System.Drawing.Point(441, 560)
        Me.SOCNbr.MaxLength = 0
        Me.SOCNbr.Name = "SOCNbr"
        Me.SOCNbr.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SOCNbr.Size = New System.Drawing.Size(112, 22)
        Me.SOCNbr.TabIndex = 163
        '
        'btnFreeFormat
        '
        Me.btnFreeFormat.BackColor = System.Drawing.SystemColors.Control
        Me.btnFreeFormat.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnFreeFormat.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFreeFormat.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnFreeFormat.Location = New System.Drawing.Point(463, 456)
        Me.btnFreeFormat.Name = "btnFreeFormat"
        Me.btnFreeFormat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnFreeFormat.Size = New System.Drawing.Size(133, 30)
        Me.btnFreeFormat.TabIndex = 164
        Me.btnFreeFormat.Text = "Free Format"
        Me.btnFreeFormat.UseVisualStyleBackColor = False
        '
        'ReceiveTimer
        '
        Me.ReceiveTimer.Enabled = True
        Me.ReceiveTimer.Interval = 600
        '
        'btnRequestLastVoteID
        '
        Me.btnRequestLastVoteID.BackColor = System.Drawing.SystemColors.Control
        Me.btnRequestLastVoteID.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnRequestLastVoteID.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRequestLastVoteID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnRequestLastVoteID.Location = New System.Drawing.Point(463, 492)
        Me.btnRequestLastVoteID.Name = "btnRequestLastVoteID"
        Me.btnRequestLastVoteID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnRequestLastVoteID.Size = New System.Drawing.Size(133, 30)
        Me.btnRequestLastVoteID.TabIndex = 167
        Me.btnRequestLastVoteID.Text = "Request Last Vote ID"
        Me.btnRequestLastVoteID.UseVisualStyleBackColor = False
        '
        'PingTimer
        '
        Me.PingTimer.Interval = 18000
        '
        'btnUnlock
        '
        Me.btnUnlock.BackColor = System.Drawing.SystemColors.Control
        Me.btnUnlock.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnUnlock.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUnlock.ForeColor = System.Drawing.Color.Black
        Me.btnUnlock.Location = New System.Drawing.Point(814, 638)
        Me.btnUnlock.Name = "btnUnlock"
        Me.btnUnlock.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnUnlock.Size = New System.Drawing.Size(62, 26)
        Me.btnUnlock.TabIndex = 173
        Me.btnUnlock.Text = "Unlock"
        Me.btnUnlock.UseVisualStyleBackColor = False
        '
        'btnRecallDisplayedBill
        '
        Me.btnRecallDisplayedBill.BackColor = System.Drawing.SystemColors.Control
        Me.btnRecallDisplayedBill.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnRecallDisplayedBill.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRecallDisplayedBill.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnRecallDisplayedBill.Location = New System.Drawing.Point(463, 348)
        Me.btnRecallDisplayedBill.Name = "btnRecallDisplayedBill"
        Me.btnRecallDisplayedBill.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnRecallDisplayedBill.Size = New System.Drawing.Size(133, 30)
        Me.btnRecallDisplayedBill.TabIndex = 178
        Me.btnRecallDisplayedBill.Text = "Recall Displayed Bill"
        Me.btnRecallDisplayedBill.UseVisualStyleBackColor = False
        '
        'lblSelectedCommittee
        '
        Me.lblSelectedCommittee.AutoSize = True
        Me.lblSelectedCommittee.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblSelectedCommittee.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSelectedCommittee.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelectedCommittee.ForeColor = System.Drawing.Color.White
        Me.lblSelectedCommittee.Image = CType(resources.GetObject("lblSelectedCommittee.Image"), System.Drawing.Image)
        Me.lblSelectedCommittee.Location = New System.Drawing.Point(538, 46)
        Me.lblSelectedCommittee.Name = "lblSelectedCommittee"
        Me.lblSelectedCommittee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSelectedCommittee.Size = New System.Drawing.Size(129, 16)
        Me.lblSelectedCommittee.TabIndex = 112
        Me.lblSelectedCommittee.Text = "Selected Committee"
        Me.lblSelectedCommittee.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Committees
        '
        Me.Committees.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Committees.Cursor = System.Windows.Forms.Cursors.Default
        Me.Committees.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Committees.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Committees.ForeColor = System.Drawing.Color.Black
        Me.Committees.Location = New System.Drawing.Point(540, 61)
        Me.Committees.Name = "Committees"
        Me.Committees.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Committees.Size = New System.Drawing.Size(520, 23)
        Me.Committees.TabIndex = 151
        '
        'lblVoteID
        '
        Me.lblVoteID.BackColor = System.Drawing.Color.FloralWhite
        Me.lblVoteID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblVoteID.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVoteID.ForeColor = System.Drawing.SystemColors.Window
        Me.lblVoteID.Image = Global.SenateVotingOOB.My.Resources.Resources.imagesCA5VP4MF
        Me.lblVoteID.Location = New System.Drawing.Point(19, 48)
        Me.lblVoteID.Name = "lblVoteID"
        Me.lblVoteID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblVoteID.Size = New System.Drawing.Size(61, 14)
        Me.lblVoteID.TabIndex = 180
        Me.lblVoteID.Text = "Vote ID"
        Me.lblVoteID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'VoteID
        '
        Me.VoteID.AcceptsReturn = True
        Me.VoteID.BackColor = System.Drawing.Color.WhiteSmoke
        Me.VoteID.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.VoteID.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.VoteID.ForeColor = System.Drawing.Color.Navy
        Me.VoteID.Location = New System.Drawing.Point(17, 62)
        Me.VoteID.MaxLength = 0
        Me.VoteID.Name = "VoteID"
        Me.VoteID.ReadOnly = True
        Me.VoteID.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.VoteID.Size = New System.Drawing.Size(61, 22)
        Me.VoteID.TabIndex = 181
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Image = CType(resources.GetObject("Label3.Image"), System.Drawing.Image)
        Me.Label3.Location = New System.Drawing.Point(79, 46)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(102, 16)
        Me.Label3.TabIndex = 183
        Me.Label3.Text = "Current Session"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnExit
        '
        Me.btnExit.BackColor = System.Drawing.Color.Maroon
        Me.btnExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnExit.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.ForeColor = System.Drawing.Color.Transparent
        Me.btnExit.Location = New System.Drawing.Point(948, 638)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnExit.Size = New System.Drawing.Size(62, 26)
        Me.btnExit.TabIndex = 184
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = False
        '
        'cboDisplay
        '
        Me.cboDisplay.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cboDisplay.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDisplay.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.cboDisplay.FormattingEnabled = True
        Me.cboDisplay.Location = New System.Drawing.Point(441, 611)
        Me.cboDisplay.Name = "cboDisplay"
        Me.cboDisplay.Size = New System.Drawing.Size(112, 22)
        Me.cboDisplay.TabIndex = 187
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Image = CType(resources.GetObject("Label4.Image"), System.Drawing.Image)
        Me.Label4.Location = New System.Drawing.Point(426, 592)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(206, 17)
        Me.Label4.TabIndex = 188
        Me.Label4.Text = "Display Borad Back Ground"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'TC
        '
        Me.TC.Controls.Add(Me.TabPage1)
        Me.TC.Controls.Add(Me.TabPage2)
        Me.TC.Controls.Add(Me.TabPage3)
        Me.TC.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TC.Location = New System.Drawing.Point(649, 297)
        Me.TC.Name = "TC"
        Me.TC.SelectedIndex = 0
        Me.TC.Size = New System.Drawing.Size(412, 334)
        Me.TC.TabIndex = 190
        '
        'TabPage1
        '
        Me.TabPage1.AllowDrop = True
        Me.TabPage1.AutoScroll = True
        Me.TabPage1.BackColor = System.Drawing.Color.LavenderBlush
        Me.TabPage1.Controls.Add(Me.WorkData)
        Me.TabPage1.Location = New System.Drawing.Point(4, 24)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(404, 306)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Tag = "0"
        Me.TabPage1.Text = "Active Work Area"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'WorkData
        '
        Me.WorkData.AcceptsReturn = True
        Me.WorkData.AllowDrop = True
        Me.WorkData.BackColor = System.Drawing.Color.WhiteSmoke
        Me.WorkData.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.WorkData.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WorkData.ForeColor = System.Drawing.Color.Black
        Me.WorkData.Location = New System.Drawing.Point(6, 6)
        Me.WorkData.MaxLength = 0
        Me.WorkData.Multiline = True
        Me.WorkData.Name = "WorkData"
        Me.WorkData.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.WorkData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.WorkData.Size = New System.Drawing.Size(392, 294)
        Me.WorkData.TabIndex = 205
        '
        'TabPage2
        '
        Me.TabPage2.AllowDrop = True
        Me.TabPage2.AutoScroll = True
        Me.TabPage2.BackColor = System.Drawing.Color.Azure
        Me.TabPage2.Controls.Add(Me.WorkData2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 24)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(404, 306)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Tag = "1"
        Me.TabPage2.Text = "Area 1"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'WorkData2
        '
        Me.WorkData2.AcceptsReturn = True
        Me.WorkData2.AllowDrop = True
        Me.WorkData2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.WorkData2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.WorkData2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WorkData2.ForeColor = System.Drawing.Color.Black
        Me.WorkData2.Location = New System.Drawing.Point(6, 6)
        Me.WorkData2.MaxLength = 0
        Me.WorkData2.Multiline = True
        Me.WorkData2.Name = "WorkData2"
        Me.WorkData2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.WorkData2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.WorkData2.Size = New System.Drawing.Size(392, 294)
        Me.WorkData2.TabIndex = 206
        '
        'TabPage3
        '
        Me.TabPage3.AllowDrop = True
        Me.TabPage3.AutoScroll = True
        Me.TabPage3.BackColor = System.Drawing.Color.LightYellow
        Me.TabPage3.Controls.Add(Me.WorkData3)
        Me.TabPage3.Location = New System.Drawing.Point(4, 24)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(404, 306)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Tag = "2"
        Me.TabPage3.Text = "Area 2"
        '
        'WorkData3
        '
        Me.WorkData3.AcceptsReturn = True
        Me.WorkData3.AllowDrop = True
        Me.WorkData3.BackColor = System.Drawing.Color.WhiteSmoke
        Me.WorkData3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.WorkData3.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WorkData3.ForeColor = System.Drawing.Color.Black
        Me.WorkData3.Location = New System.Drawing.Point(4, 6)
        Me.WorkData3.MaxLength = 0
        Me.WorkData3.Multiline = True
        Me.WorkData3.Name = "WorkData3"
        Me.WorkData3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.WorkData3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.WorkData3.Size = New System.Drawing.Size(395, 295)
        Me.WorkData3.TabIndex = 206
        '
        'btnClear
        '
        Me.btnClear.BackColor = System.Drawing.Color.YellowGreen
        Me.btnClear.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnClear.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClear.ForeColor = System.Drawing.Color.Black
        Me.btnClear.Location = New System.Drawing.Point(881, 638)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnClear.Size = New System.Drawing.Size(62, 26)
        Me.btnClear.TabIndex = 208
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Maroon
        Me.Panel1.Controls.Add(Me.btnVote)
        Me.Panel1.Controls.Add(Me.btnCancelVote)
        Me.Panel1.Location = New System.Drawing.Point(425, 156)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(208, 99)
        Me.Panel1.TabIndex = 209
        '
        'btnVote
        '
        Me.btnVote.BackColor = System.Drawing.Color.RosyBrown
        Me.btnVote.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnVote.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnVote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnVote.Location = New System.Drawing.Point(25, 28)
        Me.btnVote.Name = "btnVote"
        Me.btnVote.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnVote.Size = New System.Drawing.Size(72, 41)
        Me.btnVote.TabIndex = 162
        Me.btnVote.Text = "&Vote"
        Me.btnVote.UseVisualStyleBackColor = False
        '
        'AddPhrase
        '
        Me.AddPhrase.AcceptsReturn = True
        Me.AddPhrase.BackColor = System.Drawing.Color.WhiteSmoke
        Me.AddPhrase.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.AddPhrase.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AddPhrase.ForeColor = System.Drawing.Color.Navy
        Me.AddPhrase.Location = New System.Drawing.Point(463, 297)
        Me.AddPhrase.MaxLength = 0
        Me.AddPhrase.Name = "AddPhrase"
        Me.AddPhrase.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.AddPhrase.Size = New System.Drawing.Size(133, 22)
        Me.AddPhrase.TabIndex = 210
        '
        'lblAddPhraseToWorkArea
        '
        Me.lblAddPhraseToWorkArea.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddPhraseToWorkArea.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddPhraseToWorkArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddPhraseToWorkArea.ForeColor = System.Drawing.Color.White
        Me.lblAddPhraseToWorkArea.Image = CType(resources.GetObject("lblAddPhraseToWorkArea.Image"), System.Drawing.Image)
        Me.lblAddPhraseToWorkArea.Location = New System.Drawing.Point(449, 281)
        Me.lblAddPhraseToWorkArea.Name = "lblAddPhraseToWorkArea"
        Me.lblAddPhraseToWorkArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddPhraseToWorkArea.Size = New System.Drawing.Size(182, 15)
        Me.lblAddPhraseToWorkArea.TabIndex = 211
        Me.lblAddPhraseToWorkArea.Text = "Add Phrase To Work Area"
        Me.lblAddPhraseToWorkArea.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'btnSOC
        '
        Me.btnSOC.BackColor = System.Drawing.SystemColors.Control
        Me.btnSOC.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnSOC.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSOC.ForeColor = System.Drawing.Color.Black
        Me.btnSOC.Location = New System.Drawing.Point(557, 560)
        Me.btnSOC.Name = "btnSOC"
        Me.btnSOC.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnSOC.Size = New System.Drawing.Size(59, 24)
        Me.btnSOC.TabIndex = 212
        Me.btnSOC.Text = "Create"
        Me.btnSOC.UseVisualStyleBackColor = False
        '
        'btnChange
        '
        Me.btnChange.BackColor = System.Drawing.SystemColors.Control
        Me.btnChange.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnChange.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnChange.ForeColor = System.Drawing.Color.Black
        Me.btnChange.Location = New System.Drawing.Point(557, 611)
        Me.btnChange.Name = "btnChange"
        Me.btnChange.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnChange.Size = New System.Drawing.Size(60, 24)
        Me.btnChange.TabIndex = 213
        Me.btnChange.Text = "Change"
        Me.btnChange.UseVisualStyleBackColor = False
        '
        'lblBills
        '
        Me.lblBills.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.lblBills.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBills.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBills.ForeColor = System.Drawing.Color.White
        Me.lblBills.Image = Global.SenateVotingOOB.My.Resources.Resources.imagesCA5VP4M
        Me.lblBills.Location = New System.Drawing.Point(16, 280)
        Me.lblBills.Name = "lblBills"
        Me.lblBills.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBills.Size = New System.Drawing.Size(186, 16)
        Me.lblBills.TabIndex = 127
        Me.lblBills.Text = "Bills For The Current Calendar"
        Me.lblBills.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'frmChamberDisplay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.BackgroundImage = Global.SenateVotingOOB.My.Resources.Resources.imagesCA5VP4MF
        Me.ClientSize = New System.Drawing.Size(1078, 686)
        Me.Controls.Add(Me.btnChange)
        Me.Controls.Add(Me.btnSOC)
        Me.Controls.Add(Me.AddPhrase)
        Me.Controls.Add(Me.lblAddPhraseToWorkArea)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.TC)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cboDisplay)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.VoteID)
        Me.Controls.Add(Me.lblVoteID)
        Me.Controls.Add(Me.btnRecallDisplayedBill)
        Me.Controls.Add(Me.btnUnlock)
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
        Me.Controls.Add(Me._lblCurrentCalendar_0)
        Me.Controls.Add(Me.lblCalendars)
        Me.Controls.Add(Me.lblCurrentOrderOfBusiness)
        Me.Controls.Add(Me.lblOrdersOfBusiness)
        Me.Controls.Add(Me.FindBillNbr)
        Me.MaximizeBox = False
        Me.Name = "frmChamberDisplay"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Senate Voting System - Order Of Business"
        Me.TC.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents Senators As System.Windows.Forms.ComboBox
    Public WithEvents LegislativeDay As System.Windows.Forms.TextBox
    Public WithEvents btnCancelVote As System.Windows.Forms.Button
    Public WithEvents btnUpdateDisplay As System.Windows.Forms.Button
    Public WithEvents InsertText As System.Windows.Forms.TextBox
    Public WithEvents btnPhraseMaintenance As System.Windows.Forms.Button
    Public WithEvents btnPreviousBill As System.Windows.Forms.Button
    Public WithEvents btnNextBill As System.Windows.Forms.Button
    Public WithEvents CurrentCalendar As System.Windows.Forms.TextBox
    Public WithEvents CurrentOrderOfBusiness As System.Windows.Forms.TextBox
    Public WithEvents lblFindBillNbr As System.Windows.Forms.Label
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
    Public WithEvents _lblCurrentCalendar_0 As System.Windows.Forms.Label
    Public WithEvents lblCalendars As System.Windows.Forms.Label
    Public WithEvents lblCurrentOrderOfBusiness As System.Windows.Forms.Label
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
    Public WithEvents btnUnlock As System.Windows.Forms.Button
    Public WithEvents btnRecallDisplayedBill As System.Windows.Forms.Button
    Public WithEvents lblSelectedCommittee As System.Windows.Forms.Label
    Public WithEvents Committees As System.Windows.Forms.ComboBox
    Public WithEvents lblVoteID As System.Windows.Forms.Label
    Public WithEvents VoteID As System.Windows.Forms.TextBox
    Public WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents cboDisplay As System.Windows.Forms.ComboBox
    Public WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TC As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents WorkData As System.Windows.Forms.TextBox
    Public WithEvents WorkData2 As System.Windows.Forms.TextBox
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents WorkData3 As System.Windows.Forms.TextBox
    Public WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Public WithEvents btnVote As System.Windows.Forms.Button
    Public WithEvents AddPhrase As System.Windows.Forms.TextBox
    Public WithEvents lblAddPhraseToWorkArea As System.Windows.Forms.Label
    Public WithEvents btnSOC As System.Windows.Forms.Button
    Public WithEvents btnChange As System.Windows.Forms.Button
    Public WithEvents lblBills As System.Windows.Forms.Label
End Class
