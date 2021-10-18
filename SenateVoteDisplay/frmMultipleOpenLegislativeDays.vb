Option Strict Off
Option Explicit Off

Imports System.Windows
Imports System.Text

Friend Class frmMultipleOpenLegislativeDays
    Inherits System.Windows.Forms.Form

#Region "Windows Form Designer generated code "
	Public Sub New()
		MyBase.New()
		If m_vb6FormDefInstance Is Nothing Then
			If m_InitializingDefInstance Then
				m_vb6FormDefInstance = Me
			Else
				Try 
					'For the start-up form, the first instance created is the default instance.
					If System.Reflection.Assembly.GetExecutingAssembly.EntryPoint.DeclaringType Is Me.GetType Then
						m_vb6FormDefInstance = Me
					End If
				Catch
				End Try
			End If
		End If
		'This call is required by the Windows Form Designer.
		InitializeComponent()
	End Sub
	'Form overrides dispose to clean up the component list.
	Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents btnCancel As System.Windows.Forms.Button
	Public WithEvents btnContinue As System.Windows.Forms.Button
    Public WithEvents OpenLegislativeDay_3 As System.Windows.Forms.RadioButton
    Public WithEvents OpenLegislativeDay_2 As System.Windows.Forms.RadioButton
    Public WithEvents OpenLegislativeDay_1 As System.Windows.Forms.RadioButton
    Public WithEvents lblTestSessions As System.Windows.Forms.Label
    Public WithEvents Session_3 As System.Windows.Forms.Label
    Public WithEvents Session_2 As System.Windows.Forms.Label
    Public WithEvents lblOpenLegislativeDay_3 As System.Windows.Forms.Label
    Public WithEvents lblOpenLegislativeDay_2 As System.Windows.Forms.Label
    Public WithEvents Session_1 As System.Windows.Forms.Label
    Public WithEvents lblOpenLegislativeDay_1 As System.Windows.Forms.Label
    Public WithEvents lblSelection As System.Windows.Forms.Label
    Public WithEvents lblTitle As System.Windows.Forms.Label

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnContinue = New System.Windows.Forms.Button()
        Me.OpenLegislativeDay_3 = New System.Windows.Forms.RadioButton()
        Me.OpenLegislativeDay_2 = New System.Windows.Forms.RadioButton()
        Me.OpenLegislativeDay_1 = New System.Windows.Forms.RadioButton()
        Me.lblTestSessions = New System.Windows.Forms.Label()
        Me.Session_3 = New System.Windows.Forms.Label()
        Me.Session_2 = New System.Windows.Forms.Label()
        Me.lblOpenLegislativeDay_3 = New System.Windows.Forms.Label()
        Me.lblOpenLegislativeDay_2 = New System.Windows.Forms.Label()
        Me.Session_1 = New System.Windows.Forms.Label()
        Me.lblOpenLegislativeDay_1 = New System.Windows.Forms.Label()
        Me.lblSelection = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnCancel.Font = New System.Drawing.Font("Arial", 11.0!)
        Me.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCancel.Location = New System.Drawing.Point(228, 218)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnCancel.Size = New System.Drawing.Size(80, 30)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnContinue
        '
        Me.btnContinue.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnContinue.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnContinue.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnContinue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnContinue.Location = New System.Drawing.Point(109, 218)
        Me.btnContinue.Name = "btnContinue"
        Me.btnContinue.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnContinue.Size = New System.Drawing.Size(80, 30)
        Me.btnContinue.TabIndex = 5
        Me.btnContinue.Text = "Continue"
        Me.btnContinue.UseVisualStyleBackColor = False
        '
        'OpenLegislativeDay_3
        '
        Me.OpenLegislativeDay_3.BackColor = System.Drawing.SystemColors.Control
        Me.OpenLegislativeDay_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.OpenLegislativeDay_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OpenLegislativeDay_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OpenLegislativeDay_3.Location = New System.Drawing.Point(374, 116)
        Me.OpenLegislativeDay_3.Name = "OpenLegislativeDay_3"
        Me.OpenLegislativeDay_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OpenLegislativeDay_3.Size = New System.Drawing.Size(20, 20)
        Me.OpenLegislativeDay_3.TabIndex = 3
        Me.OpenLegislativeDay_3.TabStop = True
        Me.OpenLegislativeDay_3.Text = "Option1"
        Me.OpenLegislativeDay_3.UseVisualStyleBackColor = False
        Me.OpenLegislativeDay_3.Visible = False
        '
        'OpenLegislativeDay_2
        '
        Me.OpenLegislativeDay_2.BackColor = System.Drawing.SystemColors.Control
        Me.OpenLegislativeDay_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.OpenLegislativeDay_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OpenLegislativeDay_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OpenLegislativeDay_2.Location = New System.Drawing.Point(374, 90)
        Me.OpenLegislativeDay_2.Name = "OpenLegislativeDay_2"
        Me.OpenLegislativeDay_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OpenLegislativeDay_2.Size = New System.Drawing.Size(20, 20)
        Me.OpenLegislativeDay_2.TabIndex = 2
        Me.OpenLegislativeDay_2.TabStop = True
        Me.OpenLegislativeDay_2.Text = "Option1"
        Me.OpenLegislativeDay_2.UseVisualStyleBackColor = False
        Me.OpenLegislativeDay_2.Visible = False
        '
        'OpenLegislativeDay_1
        '
        Me.OpenLegislativeDay_1.BackColor = System.Drawing.SystemColors.Control
        Me.OpenLegislativeDay_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.OpenLegislativeDay_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OpenLegislativeDay_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OpenLegislativeDay_1.Location = New System.Drawing.Point(374, 65)
        Me.OpenLegislativeDay_1.Name = "OpenLegislativeDay_1"
        Me.OpenLegislativeDay_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.OpenLegislativeDay_1.Size = New System.Drawing.Size(20, 20)
        Me.OpenLegislativeDay_1.TabIndex = 1
        Me.OpenLegislativeDay_1.TabStop = True
        Me.OpenLegislativeDay_1.Text = "Option1"
        Me.OpenLegislativeDay_1.UseVisualStyleBackColor = False
        Me.OpenLegislativeDay_1.Visible = False
        '
        'lblTestSessions
        '
        Me.lblTestSessions.BackColor = System.Drawing.Color.Yellow
        Me.lblTestSessions.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTestSessions.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblTestSessions.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTestSessions.Location = New System.Drawing.Point(173, 44)
        Me.lblTestSessions.Name = "lblTestSessions"
        Me.lblTestSessions.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTestSessions.Size = New System.Drawing.Size(100, 18)
        Me.lblTestSessions.TabIndex = 13
        Me.lblTestSessions.Text = "Test Sessions"
        Me.lblTestSessions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblTestSessions.Visible = False
        '
        'Session_3
        '
        Me.Session_3.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Session_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Session_3.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.Session_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Session_3.Location = New System.Drawing.Point(132, 117)
        Me.Session_3.Name = "Session_3"
        Me.Session_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Session_3.Size = New System.Drawing.Size(231, 20)
        Me.Session_3.TabIndex = 12
        Me.Session_3.Visible = False
        '
        'Session_2
        '
        Me.Session_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Session_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Session_2.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.Session_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Session_2.Location = New System.Drawing.Point(132, 91)
        Me.Session_2.Name = "Session_2"
        Me.Session_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Session_2.Size = New System.Drawing.Size(231, 20)
        Me.Session_2.TabIndex = 11
        Me.Session_2.Visible = False
        '
        'lblOpenLegislativeDay_3
        '
        Me.lblOpenLegislativeDay_3.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblOpenLegislativeDay_3.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOpenLegislativeDay_3.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblOpenLegislativeDay_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOpenLegislativeDay_3.Location = New System.Drawing.Point(32, 117)
        Me.lblOpenLegislativeDay_3.Name = "lblOpenLegislativeDay_3"
        Me.lblOpenLegislativeDay_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOpenLegislativeDay_3.Size = New System.Drawing.Size(93, 20)
        Me.lblOpenLegislativeDay_3.TabIndex = 10
        Me.lblOpenLegislativeDay_3.Visible = False
        '
        'lblOpenLegislativeDay_2
        '
        Me.lblOpenLegislativeDay_2.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblOpenLegislativeDay_2.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOpenLegislativeDay_2.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblOpenLegislativeDay_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOpenLegislativeDay_2.Location = New System.Drawing.Point(32, 91)
        Me.lblOpenLegislativeDay_2.Name = "lblOpenLegislativeDay_2"
        Me.lblOpenLegislativeDay_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOpenLegislativeDay_2.Size = New System.Drawing.Size(93, 20)
        Me.lblOpenLegislativeDay_2.TabIndex = 9
        Me.lblOpenLegislativeDay_2.Visible = False
        '
        'Session_1
        '
        Me.Session_1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Session_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Session_1.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.Session_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Session_1.Location = New System.Drawing.Point(132, 66)
        Me.Session_1.Name = "Session_1"
        Me.Session_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Session_1.Size = New System.Drawing.Size(231, 20)
        Me.Session_1.TabIndex = 8
        Me.Session_1.Visible = False
        '
        'lblOpenLegislativeDay_1
        '
        Me.lblOpenLegislativeDay_1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblOpenLegislativeDay_1.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblOpenLegislativeDay_1.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblOpenLegislativeDay_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblOpenLegislativeDay_1.Location = New System.Drawing.Point(32, 66)
        Me.lblOpenLegislativeDay_1.Name = "lblOpenLegislativeDay_1"
        Me.lblOpenLegislativeDay_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOpenLegislativeDay_1.Size = New System.Drawing.Size(93, 20)
        Me.lblOpenLegislativeDay_1.TabIndex = 7
        Me.lblOpenLegislativeDay_1.Visible = False
        '
        'lblSelection
        '
        Me.lblSelection.BackColor = System.Drawing.SystemColors.Control
        Me.lblSelection.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSelection.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblSelection.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSelection.Location = New System.Drawing.Point(17, 143)
        Me.lblSelection.Name = "lblSelection"
        Me.lblSelection.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSelection.Size = New System.Drawing.Size(392, 66)
        Me.lblSelection.TabIndex = 4
        Me.lblSelection.Text = "There are multiple open legislative days.  Select one of the above days and press" & _
            " 'Continue' or press 'Cancel' to exit the Voting System."
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.LightCyan
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTitle.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTitle.Location = New System.Drawing.Point(29, 11)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTitle.Size = New System.Drawing.Size(365, 24)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Multiple Open Legislative Days"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmMultipleOpenLegislativeDays
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.BackgroundImage = Global.SenateVotingOOB.My.Resources.Resources.imagesCA5VP4MF
        Me.ClientSize = New System.Drawing.Size(421, 258)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnContinue)
        Me.Controls.Add(Me.OpenLegislativeDay_3)
        Me.Controls.Add(Me.OpenLegislativeDay_2)
        Me.Controls.Add(Me.OpenLegislativeDay_1)
        Me.Controls.Add(Me.lblTestSessions)
        Me.Controls.Add(Me.Session_3)
        Me.Controls.Add(Me.Session_2)
        Me.Controls.Add(Me.lblOpenLegislativeDay_3)
        Me.Controls.Add(Me.lblOpenLegislativeDay_2)
        Me.Controls.Add(Me.Session_1)
        Me.Controls.Add(Me.lblOpenLegislativeDay_1)
        Me.Controls.Add(Me.lblSelection)
        Me.Controls.Add(Me.lblTitle)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(1, 1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMultipleOpenLegislativeDays"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)

    End Sub
#End Region 
#Region "Upgrade Support "
	Private Shared m_vb6FormDefInstance As frmMultipleOpenLegislativeDays
	Private Shared m_InitializingDefInstance As Boolean
	Public Shared Property DefInstance() As frmMultipleOpenLegislativeDays
		Get
			If m_vb6FormDefInstance Is Nothing OrElse m_vb6FormDefInstance.IsDisposed Then
				m_InitializingDefInstance = True
				m_vb6FormDefInstance = New frmMultipleOpenLegislativeDays()
				m_InitializingDefInstance = False
			End If
			DefInstance = m_vb6FormDefInstance
		End Get
		Set
			m_vb6FormDefInstance = Value
		End Set
	End Property
#End Region 
	
	Dim i, k As Short
	Dim RetValue As Object
	
    Private Sub frmMultipleOpenLegislativeDays_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        If gWriteVotesToTest Then
            Me.lblTestSessions.Visible = True
        End If

        lblOpenLegislativeDay_1.Visible = True
        lblOpenLegislativeDay_2.Visible = True
        lblOpenLegislativeDay_3.Visible = True
        OpenLegislativeDay_1.Visible = True
        OpenLegislativeDay_2.Visible = True
        OpenLegislativeDay_3.Visible = True
        Session_1.Visible = True
        Session_2.Visible = True
        Session_3.Visible = True

        Select Case gCalendarDayTmp.Length
            Case 1
                lblOpenLegislativeDay_1.Text = gCalendarDayTmp(0)
            Case 2
                lblOpenLegislativeDay_1.Text = gCalendarDayTmp(0)
                lblOpenLegislativeDay_2.Text = gCalendarDayTmp(1)
            Case 3
                lblOpenLegislativeDay_1.Text = gCalendarDayTmp(0)
                lblOpenLegislativeDay_2.Text = gCalendarDayTmp(1)
                lblOpenLegislativeDay_1.Text = gCalendarDayTmp(2)
        End Select

        Select Case gSessionNameTmp.Length
            Case 1
                Session_1.Text = gSessionNameTmp(0)
            Case 2
                Session_1.Text = gSessionNameTmp(0)
                Session_2.Text = gSessionNameTmp(1)
            Case 3
                Session_1.Text = gSessionNameTmp(0)
                Session_2.Text = gSessionNameTmp(1)
                Session_3.Text = gSessionNameTmp(2)
        End Select

        '--- set the first day to true since it will be the most current one
        Me.OpenLegislativeDay_1.Checked = True
    End Sub

    Private Sub btnCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnCancel.Click
        If MsgBox("There are more than one LEG day open. If you selected Cancel, System will shutdown, otherwise select a specific LEG day to continue work.", MsgBoxStyle.YesNo, msgText) = MsgBoxResult.Yes Then
            End
        End If
    End Sub

    Private Sub btnContinue_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnContinue.Click
        k = 0
        If OpenLegislativeDay_1.Checked Then
            k = 0
        ElseIf OpenLegislativeDay_2.Checked Then
            k = 1
        ElseIf OpenLegislativeDay_3.Checked Then
            k = 2
        End If

        gSessionID = gSessionIDTmp(k)
        gLegislativeDay = CStr(gLegislativeDayTmp(k))
        gLegislativeDayOID = gLegislativeDayOIDTmp(k)
        gCalendarDay = gCalendarDayTmp(k)
        gSessionName = gSessionNameTmp(k)
        gSessionAbbrev = gSessionAbbrevTmp(k)

        frmMain.Focus()
        Me.Close()
    End Sub
End Class