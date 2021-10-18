'Option Strict Off
'Option Explicit Off

Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Timers
Imports System.Messaging
Imports System.Text
Imports System.DateTime
Imports System.DBNull
Imports System.Net.NetworkInformation
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class frmChamberDisplay
    Private RetValue As Object
    Private ComingFrom, strBillNbr, strOrigSRName As String
    Private Highlight As Short
    Private UpdateWorkDataSW As Boolean = False
    Private ClearDisplayFlag As Boolean = False

    Public Shared WithEvents frmPhraseShow As New frmPhraseShow
    Public Shared WithEvents frmSenatorShow As New frmSenatorShow
    Private iniVoteID As String
    Private work_bill, work1_bill, work2_bill As String

    Private Sub frmChamberDisplay_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If gTestMode Then
            Me.lblTestMode.Visible = True
        Else
            Me.lblTestMode.Visible = False
        End If

        If gCreateHTMLPage Then
            Me.btnCreateHTMLPage.BackColor = Color.Green
        Else
            Me.btnCreateHTMLPage.BackColor = Color.Red
        End If

        If SVotePC_On Then
            btnVote.Enabled = True
        Else
            btnVote.Enabled = False
        End If
    End Sub

    Private Sub frmChamberDisplay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim str As String
        Dim ds As New DataSet
        Dim dsC As New DataSet
        Dim dsSalu As New DataSet
        Dim dsB As New DataSet
        Dim dr As DataRow

        Me.MdiParent = frmMain

        '--- clear all of messages on Vote PS
        If SVotePC_On Then
            ReceiveTimer.Enabled = True
            ReceiveTimer.Start()
            ReceiveTimer.Interval = gReceiveQueueTimer
            ReceiveMessageFromQueue()
            Me.txtVoteID.Text = iniVoteID
        Else
            gOnlyOnePC = True
            ReceiveTimer.Enabled = False
            Me.txtVoteID.Text = gVoteID + 1
        End If

        If gTestMode Then
            lblTestMode.Visible = True
            'If SVotePC_On = False Then
            '    ' Me.txtVoteID.Text = gVoteID + 1
            'End If
        Else
            lblTestMode.Visible = False
        End If

        OOBBackgroudChang = False

        CreateClearHTMLPage()

        '--- Last will move it - this model is woked 
        UpdateWorkDataSW = False
        ComingFrom = ""
        gCalendarCodeDisplayed = ""
        gBillNbrDisplayed = ""
        gCalendarDisplayed = ""
        gBillDisplayed = ""

        Me.lblChamberLight.BackColor = System.Drawing.Color.LimeGreen
        Me.lblChamberLight.Text = "Display Next Bill"
        Me.lblSession.Text = gSessionName
        Me.LegislativeDay.Text = gLegislativeDay & " - " & gCalendarDay

        ERHPushStack(("LoadSenatorsIntoArray"))

        ''--- initialize calendar and bills 
        LoadCalendar()

        Calendar_Bill(Calendar.Text)
        Calendar.SelectedIndex = 0
        Calendar_Click(sender, e)

        '--- initialize order of business
        str = "Select * From tblOrderOfBusiness "
        ds = V_DataSet(str, "R")
        OrderOfBusiness.Items.Add("")
        OrderOfBusiness.SelectedIndex = 0
        For Each dr In ds.Tables(0).Rows
            OrderOfBusiness.Items.Add(dr("OrderOfBusiness"))
        Next

        ' --- Load Senators from gSenatorName array in to SenatorName comboBox
        Senators.Items.Clear()
        If gSenatorName.Length > 0 Then
            For i As Integer = 0 To gSenatorName.Length - 2
                Senators.Items.Add(gSenatorName(i))
            Next
        Else
            gNbrSenators = 0
        End If

        '--- Load Committees from gCommitte() and gCommitteAbbreves() arrays to Committee comboBox
        LoadCommitteesIntoArray()
        Committees.Items.Clear()
        For j As Integer = 1 To gCommittees.Length - 1
            Committees.Items.Add(gCommitteeAbbrevs(j) & " - " & gCommittees(j))
        Next

        '--- Load  Phases from gThePhrases(i) array in to Phases comboBox. Notic: gThePhrases(i) array included PhraseCode and Phrase, gPhrases has only Phrases string in array
        Phrases.Items.Clear()
        Dim dsPH As New DataSet
        dsPH = V_DataSet("Select Cstr(Code)  + ' - ' + Phrase  AS ThePhrase, Code, Phrase From tblPhrases Order By Code", "R")
        If dsPH.Tables(0).Rows.Count > 0 Then
            For Each drPH As DataRow In dsPH.Tables(0).Rows
                Phrases.Items.Add(drPH("ThePhrase"))
            Next
        Else
            DisplayMessage("There is not the Phrase to load.", "Load Phases", "S")
        End If

        '---Load OOB Display Borad HTML nameS 
        str = "Select [File Name] From tblOOBDisplayHTML Order By OID "
        ds = V_DataSet(str, "R")

        cboDisplay.Items.Add("")
        For Each dr In ds.Tables(0).Rows
            cboDisplay.Items.Add(dr(0))
        Next
        For j As Integer = 0 To Me.cboDisplay.Items.Count - 1
            If InStr(UCase(cboDisplay.Items(j).ToString), "SENATEVOTINGPP") > 0 Then
                cboDisplay.Text = UCase(cboDisplay.Items(j).ToString)
            End If
        Next

        EnableControls()

        If SVotePC_On Then
            SendMessageForVoteID("REQUESTVOTEID", "")
            btnVote.Enabled = True
            btnCancelVote.Enabled = True
        Else
            btnVote.Enabled = False
            btnCancelVote.Enabled = False
        End If

        If SOOB_On Then
            SendStartOOBToOOB("", "CLEAR DISPLAY")
        End If

        tPage0 = True
        tPage1 = False
        tPage2 = False
        WorkData.Focus()
    End Sub

    Private Sub LoadCalendar()
        Dim ds As New DataSet
        '--- initialize calendar and bills 
        Calendar.Items.Clear()
        ds = V_DataSet("Select * From tblCalendars", "R")
        For Each dr In ds.Tables(0).Rows
            Calendar.Items.Add(dr("Calendar"))
        Next
    End Sub

    Private Sub AddPhrase_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles AddPhrase.KeyDown
        If e.KeyCode = Keys.Enter Then
            '--- format is Phrase # followed by senator # followed by committee, ex.  3,5,A&F to get
            '--- Phrase #3 followed by senator #5 followed by committee A&F; this senator part and
            '--- committees only applies if there are senator and committee insertion point(s) in the phrase

            Dim fnt As Font
            If tPage0 Then
                fnt = Me.WorkData.Font
                Me.WorkData.Font = New Font(fnt.Name, 11, FontStyle.Regular)
            ElseIf tPage1 Then
                fnt = Me.WorkData1.Font
                Me.WorkData1.Font = New Font(fnt.Name, 11, FontStyle.Regular)
            ElseIf tPage2 Then
                fnt = Me.WorkData2.Font
                Me.WorkData2.Font = New Font(fnt.Name, 11, FontStyle.Regular)
            End If

            Dim WrkFld As String, WrkFld3 As String, i As Integer
            Dim SNbr As Integer, PNbr As Integer, CAbbrev As String

            WrkFld = Me.AddPhrase.Text
            If Strings.Right(WrkFld, 1) <> "," Then
                WrkFld = WrkFld & ","
            End If
            WrkFld = ReplaceCharacter(WrkFld, " ", "")  '-- get rid of all spaces
            If NToB(Me.AddPhrase.Text) = "," Then       '-- no senator so exit
                Exit Sub
            End If
            Me.AddPhrase.Text = ""

            PNbr = 0
            SNbr = 0
            CAbbrev = ""

            If InStr(WrkFld, ",") > 0 Then
                PNbr = Val(Mid(WrkFld, 1, InStr(WrkFld, ",") - 1))
            Else
                PNbr = Val(Mid(WrkFld, 1))
            End If

            WrkFld = Mid(WrkFld, InStr(WrkFld, ","))           '-- delete up to ,
            If (WrkFld <> ",") And (WrkFld <> ",,") And (WrkFld <> ",,,") Then
                If Strings.Left(WrkFld, 2) = ",," Then         '-- senator and abbrev entered
                    CAbbrev = UCase(Mid(WrkFld, 3, InStr(3, WrkFld, ",") - 3))
                Else
                    WrkFld = Mid(WrkFld, 2)
                    SNbr = Val(Mid(WrkFld, 1, InStr(2, WrkFld, ",") - 1))
                    WrkFld = Mid(WrkFld, InStr(WrkFld, ","))   '-- delete up to ,
                    If (WrkFld <> ",") And (WrkFld <> ",,") Then
                        CAbbrev = UCase(Mid(WrkFld, 2, InStr(2, WrkFld, ",") - 2))
                    End If
                End If
            End If

            '--- capture phrase
            WrkFld3 = ""
            For i = 1 To gNbrPhrases
                If PNbr = gPhraseCodes(i) Then
                    WrkFld3 = gPhrases(i)
                    Exit For
                End If
            Next i

            If WrkFld3 = "" Then   ' phrase not found so exit
                Exit Sub
            End If

            '--- see if senator insertion point is in the phrase, and if so add the senator name
            If InStr(WrkFld3, gSenatorInsertionPoint) > 0 Then
                If SNbr = 0 Then
                ElseIf (SNbr < 0) Or (SNbr > gNbrSenators) Then
                    If gChamberHelp Then
                        RetValue = DisplayMessage("Senator # must be between 1 and " & gNbrSenators _
                           & ".  Please re-enter", "Invalid Senator #", "S")
                    End If
                    Exit Sub
                ElseIf InStr(WrkFld3, gSenatorInsertionPoint) = 1 Then
                    WrkFld3 = "Senator " & gSenatorDistrictName(SNbr) & Mid(WrkFld3, 2)
                Else
                    WrkFld3 = Mid(WrkFld3, 1, InStr(WrkFld3, gSenatorInsertionPoint) - 1) & "Senator " & gSenatorDistrictName(SNbr) & Mid(WrkFld3, InStr(WrkFld3, gSenatorInsertionPoint) + 1)
                End If
            End If

            '---see if committee insertion point is in the phrase, and if so add the committee name
            If InStr(WrkFld3, gCommitteeInsertionPoint) > 0 Then
                If CAbbrev = "" Then
                Else
                    For i = 1 To gNbrCommittees
                        If gCommitteeAbbrevs(i) = CAbbrev Then
                            If InStr(WrkFld3, gCommitteeInsertionPoint) = 1 Then
                                WrkFld3 = gCommittees(i) & Mid(WrkFld3, 2)
                            Else
                                WrkFld3 = Mid(WrkFld3, 1, InStr(WrkFld3, gCommitteeInsertionPoint) - 1) & gCommittees(i) & Mid(WrkFld3, InStr(WrkFld3, gCommitteeInsertionPoint) + 1)
                            End If
                            Exit For
                        End If
                    Next i
                    If InStr(WrkFld3, gCommitteeInsertionPoint) > 0 Then  ' if still there, then invalid abbrev
                        If gChamberHelp Then
                            RetValue = DisplayMessage("The committee abbreviation " & CAbbrev & " you entered is invalid.", _
                               "Invalid Committee Abbreviation", "S")
                        End If
                        Exit Sub
                    End If
                End If
            End If

            '--- put this phrase in the work area at the phrase insertion point; if no phrase insertion point
            '--- then add it to the end of the work area
            If WrkFld3 > "" Then
                If WrkFld3 <> "ADJOURNMENT" Then
                    WrkFld3 = WrkFld3 & vbCrLf
                    InsertIntoWorkData(WrkFld3, gPhraseInsertionPoint)
                Else
                    Dim fntn As Font

                    If tPage0 Then
                        fntn = Me.WorkData.Font
                        Me.WorkData.Font = New Font(fntn.Name, 11, FontStyle.Bold)
                        Me.CurrentBill.Text = "ADJOURMENT"
                        Me.WorkData.Text = "The senate is in adjournment until "
                    ElseIf tPage1 Then
                        fntn = Me.WorkData1.Font
                        Me.WorkData1.Font = New Font(fntn.Name, 11, FontStyle.Bold)
                        Me.CurrentBill.Text = "ADJOURMENT"
                        Me.WorkData1.Text = "The senate is in adjournment until "
                    ElseIf tPage2 Then
                        fntn = Me.WorkData2.Font
                        Me.WorkData2.Font = New Font(fntn.Name, 11, FontStyle.Bold)
                        Me.CurrentBill.Text = "ADJOURMENT"
                        Me.WorkData2.Text = "The senate is in adjournment until "
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Bill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Bill.Click
        Dim fnt As Font

        If tPage0 Then
            fnt = Me.WorkData.Font
            Me.WorkData.Font = New Font(fnt.Name, 11, FontStyle.Regular)
            work_bill = Me.Bill.SelectedItem
        ElseIf tPage1 Then
            fnt = Me.WorkData1.Font
            Me.WorkData1.Font = New Font(fnt.Name, 11, FontStyle.Regular)
            work1_bill = Me.Bill.SelectedItem
        ElseIf tPage2 Then
            fnt = Me.WorkData2.Font
            Me.WorkData2.Font = New Font(fnt.Name, 11, FontStyle.Regular)
            work2_bill = Me.Bill.SelectedItem
        End If

        btnCreateHTMLPage.BackColor = Color.Silver
        Me.CurrentBill.Enabled = True

        If (ComingFrom = "NextBill") Or (ComingFrom = "PreviousBill") Then
            ComingFrom = ""
        End If

        '--- bookmark so we know where we are in the list when using next/previous buttons
        If NToB(Bill.Text) <> "" Then
            CurrentBill.Text = Bill.Text
            gBill = Me.CurrentBill.Text
            BillHasChanged()
            Me.CurrentBill.Text = Me.Bill.Text
        End If

        '---If no motion calendar and there is a senator insertion point in this motion, 
        '---then pull down the senator list box
        If (gCalendarCode = "M") And (InStr(Bill.Text, gSenatorInsertionPoint) > 0) Then
            Me.CurrentBill.Text = gBill
            Me.Senators.Focus()
            SendKeys.Send("{F4}")
        End If
    End Sub

    Private Sub Calendar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar.Click
        Dim dr As DataRow
        Dim ds As New DataSet
        gCalendarCode = ""

       
        Me.WorkData.Text = ""
        Me.WorkData1.Text = ""
        Me.WorkData2.Text = ""

        UpdateWorkDataSW = False
        gCalendar = Me.Calendar.Text
        Me.lblWorkAreaDisplayed.Visible = False                   'reset in case it was on
        Me.CurrentCalendar.Text = Me.Calendar.Text

        ds = GetBills(gCalendar)
        Bill.Items.Clear()
        For Each dr In ds.Tables(0).Rows
            Bill.Items.Add(dr("Bill"))
            gCalendarCode = dr("CalendarCode")
        Next
        CurrentCalendar.Text = Calendar.SelectedItem
        CurrentBill.Text = ""

        '--- if motion calendar (code 9) is selected, then change caption on current bill field
        If UCase(Me.CurrentCalendar.Text) = UCase("MOTIONS") Then
            lblCurrentBill.Text = "Current Motion"
            Me.lblBills.Text = "Motions"
        Else
            lblCurrentBill.Text = "Current Bill"
            Me.lblBills.Text = "Bills For The Current Calendar"
        End If
    End Sub

    Private Sub Calendar_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Calendar.MouseDown
        Dim tmpC As String = ""
        If e.Button = Windows.Forms.MouseButtons.Right Then

            Try
                tmpC = Me.Calendar.SelectedItem.ToString

                Select Case Strings.Left(UCase(tmpC), 2)
                    Case "SR"
                        If MsgBox("Are you sure want to delete selected " & Me.Calendar.SelectedItem.ToString & "?", vbYesNo, "Delete Special Order Calendar") = vbYes Then
                            V_DataSet("Delete From tblCalendars Where   UCASE(Calendar) ='" & UCase(Calendar.SelectedItem.ToString) & "'", "D")
                            V_DataSet("Delete From tblSpecialOrderCalendar Where UCASE(BillNbr) ='" & UCase(Calendar.SelectedItem.ToString) & "'", "D")
                        End If
                    Case ("SP")
                        If MsgBox("Are you sure want to delete selected " & Me.Calendar.SelectedItem.ToString & "?", vbYesNo, "Delete Special Order Calendar") = vbYes Then
                            '--- Delete Special Order Calendar'
                            V_DataSet("Delete From tblCalendars Where  CalendarCode ='SOC'", "D")
                            V_DataSet("Delete From tblSpecialOrderCalendar Where UCASE(BillNbr) ='SPECIAL ORDER CALENDAR'", "D")
                        End If
                End Select
                LoadCalendar()
                Calendar_Click(Calendar, Nothing)
                SOCNbr.Text = ""
            Catch
                Exit Sub
            End Try
        End If
    End Sub

    Public Sub InsertIntoWorkData(ByVal Data As String, ByVal InsertionPoint As String)
        Dim i As Integer

        '-- put stuff into the work area at the insertion point; if this is
        '-- a phrase then append if no insertion point found; if the current
        '-- calendar is the motion calendar, then insert into the current bill area,
        '-- then try the work area

        If gCalendarCode = "M" Then
            i = InStr(NToB(Me.CurrentBill.Text), InsertionPoint)
            If i > 0 Then
                Me.CurrentBill.Text = Mid(Me.CurrentBill.Text, 1, i - 1) & Data & Mid(Me.CurrentBill.Text, i + 1)
                gBill = Me.CurrentBill.Text
            End If
        End If

        '--- insert into work area
        If tPage0 Then
            i = InStr(NToB(Me.WorkData.Text), InsertionPoint)
            If i > 0 Then
                Me.WorkData.Text = Mid(Me.WorkData.Text, 1, i - 1) & Data & Mid(Me.WorkData.Text, i + 1)
            ElseIf InsertionPoint = gPhraseInsertionPoint Then  ' append the phrase
                Me.WorkData.Text = Me.WorkData.Text & Data
                If Len(Me.WorkData.Text) > 1 Then
                    If Strings.Right(Me.WorkData.Text, 2) <> vbCrLf Then
                        Me.WorkData.Text = Me.WorkData.Text & vbCrLf
                    End If
                End If
            End If
        ElseIf tPage1 Then
            i = InStr(NToB(Me.WorkData1.Text), InsertionPoint)
            If i > 0 Then
                Me.WorkData1.Text = Mid(Me.WorkData1.Text, 1, i - 1) & Data & Mid(Me.WorkData1.Text, i + 1)
            ElseIf InsertionPoint = gPhraseInsertionPoint Then      '-- append the phrase
                Me.WorkData1.Text = Me.WorkData1.Text & Data
                If Len(Me.WorkData1.Text) > 1 Or DragDrop2 = True Then
                    If Strings.Right(Me.WorkData1.Text, 2) <> vbCrLf Then
                        Me.WorkData1.Text = Me.WorkData1.Text & vbCrLf
                    End If
                End If
            End If
        ElseIf tPage2 Then
            i = InStr(NToB(Me.WorkData2.Text), InsertionPoint)
            If i > 0 Then
                Me.WorkData2.Text = Mid(Me.WorkData2.Text, 1, i - 1) & Data & Mid(Me.WorkData2.Text, i + 1)
            ElseIf InsertionPoint = gPhraseInsertionPoint Then      '-- append the phrase
                Me.WorkData2.Text = Me.WorkData2.Text & Data
                If Len(Me.WorkData2.Text) > 1 Or DragDrop3 = True Then
                    If Strings.Right(Me.WorkData2.Text, 2) <> vbCrLf Then
                        Me.WorkData2.Text = Me.WorkData2.Text & vbCrLf
                    End If
                End If
            End If
        End If

        '--- if senator insertion point is in the phrase, or bill area for the motion calendar, then pull down the senator list
        If (InsertionPoint = gPhraseInsertionPoint) And (InStr(Data, gSenatorInsertionPoint)) Then
            Me.Senators.Focus()
            SendKeys.Send("{F4}")
        End If
    End Sub

    Private Sub btnBIR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBIR.Click
        '--- if BIR, then fill in work area (if blank) and put cursor on senator box and open it
        If tPage0 Then
            If Me.WorkData.Text = "" Then
                Me.WorkData.Text = gBIR & vbCrLf & "^ MOTION TO ADOPT" & vbCrLf
                Me.Senators.Focus()
                SendKeys.Send("{F4}")
            End If
        ElseIf tPage1 Then
            If Me.WorkData1.Text = "" Then
                Me.WorkData1.Text = gBIR & vbCrLf & "^ MOTION TO ADOPT" & vbCrLf
                Me.Senators.Focus()
                SendKeys.Send("{F4}")
            End If
        ElseIf tPage2 Then
            If Me.WorkData2.Text = "" Then
                Me.WorkData2.Text = gBIR & vbCrLf & "^ MOTION TO ADOPT" & vbCrLf
                Me.Senators.Focus()
                SendKeys.Send("{F4}")
            End If
        End If
    End Sub

    Private Sub btnCancelVote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelVote.Click
        If DisplayMessage("Are you sure you want to cancel this vote?", "Cancel Vote", "Y") Then
            gPutParams = True
            gPutCalendarCode = "Cancel"
            gPutCalendar = ""
            gPutBill = ""
            gPutBillNbr = ""
            gPutPhrase = ""
            Me.lblChamberLight.Text = "Display Next Bill"
            Me.lblChamberLight.BackColor = Color.LimeGreen
            Me.btnVote.BackColor = Color.LimeGreen
            gPutChamberLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen)
            gPutVotingLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LimeGreen)
            Me.lblWorkAreaDisplayed.Visible = False
            Me.btnRecallDisplayedBill.Enabled = True
            Me.btnUnlock.Enabled = True
            EnableControls()
            gCalendarDisplayed = ""
            gCalendarCodeDisplayed = ""
            gBillDisplayed = ""
            gBillNbrDisplayed = ""
            cboDisplay.Enabled = True

            If SVotePC_On Then
                '--- clear all of messages on myself Queue
                Dim mq As New MessageQueue(gSendQueueToVotePC)
                Dim msg As New Message
                mq.Purge()
            End If
        End If
    End Sub

    Private Sub btnClearWorkArea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '--- update the rec and then clear the area
        If gChamberHelp Then
            If Not DisplayMessage("Are you sure you want to clear work area?", "Clear Work Area", "U") Then
                Exit Sub
            End If
        End If

        UpdateBillWorkData()
        Me.WorkData.Text = ""
        UpdateWorkDataSW = False
    End Sub

    Private Sub btnFreeFormat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFreeFormat.Click
        '--- change color of background to let user know they are in free format;
        '--- user must manually turn this off

        If System.Drawing.ColorTranslator.ToOle(Me.btnFreeFormat.BackColor) = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green) Then
            Me.btnFreeFormat.BackColor = System.Drawing.ColorTranslator.FromOle(&HC0C0C0)
            gFreeFormat = False
            Me.CurrentBill.Enabled = False
            Me.btnRecallDisplayedBill.Enabled = True
            Exit Sub
        End If

        Me.btnRecallDisplayedBill.Enabled = True
        Me.lblWorkAreaDisplayed.Visible = False
        Me.CurrentBill.Enabled = True
        Me.btnFreeFormat.BackColor = System.Drawing.Color.Green
        gFreeFormat = True
        If UpdateWorkDataSW Then
            UpdateBillWorkData()
        End If
        If tPage0 Then
            Me.WorkData.Text = ""
        ElseIf tPage1 Then
            Me.WorkData1.Text = ""
        ElseIf tPage2 Then
            Me.WorkData2.Text = ""
        End If
        gCalendarCodeDisplayed = ""
        gBillNbrDisplayed = ""
        gCalendarDisplayed = ""
        gBillDisplayed = ""
        gCurrentPhrase = ""
        Me.CurrentBill.Text = ""
        gBill = ""
        gBillNbr = ""
        Me.OrderOfBusiness.Enabled = True
    End Sub

    Private Sub btnNextBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNextBill.Click
        Dim i As Integer

        '--- update the work area for the current bill and get the next one
        Dim ds As DataSet = ShowBillByCalendar(Calendar.Text)
        If ds.Tables(0).Rows.Count > 0 Then
            i = Bill.SelectedIndex
            If i >= 0 And i <= Bill.Items.Count - 2 Then
                Bill.SelectedItem = Bill.Items.Item(i + 1)
            End If
            ComingFrom = "NextBill"
        End If
    End Sub

    Private Sub btnPhraseMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPhraseMaintenance.Click
        Dim frmPL As New frmPhrasesList
        frmPL.MdiParent = frmMain
        frmPL.Show()
        frmPL.BringToFront()
        Me.Close()
    End Sub

    Private Sub btnPreviousBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreviousBill.Click
        Dim i As Integer
        Dim ds As DataSet = ShowBillByCalendar(Calendar.Text)

        If ds.Tables(0).Rows.Count > 0 Then
            i = Bill.SelectedIndex

            If i >= 1 Then
                Bill.SelectedItem = Bill.Items.Item(i - 1)
            End If
            ComingFrom = "PreviousBill"
        End If
    End Sub

    Private Sub btnRecallDisplayedBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If gBillDisplayed = "" Then
            RetValue = DisplayMessage("No bill has been displayed yet", "Nothing To Recall", "S")
        Else
            Me.Calendar.Text = gCalendarCodeDisplayed
            Calendar_Click(sender, e)
            Me.Bill.Text = gBillDisplayed
            Bill_Click(sender, e)
        End If
    End Sub

    Private Sub CopyHTMLPageToLocal()
        Dim FileToCopy As String
        Dim NewCopy As String

        FileToCopy = gHTMLFile
        NewCopy = gLocalHTMLPage

        If System.IO.File.Exists(gLocalHTMLPage) = True Then
            System.IO.File.Delete(gLocalHTMLPage)
        End If

        If System.IO.File.Exists(FileToCopy) = True Then
            System.IO.File.Copy(FileToCopy, NewCopy)
        End If
    End Sub

    Private Sub btnUpdateDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateDisplay.Click
        Dim strWorkArea As String = ""
        If tPage0 Then
            strWorkArea = WorkData.Text
        ElseIf tPage1 Then
            strWorkArea = WorkData1.Text
        ElseIf tPage2 Then
            strWorkArea = WorkData2.Text
        End If

        Try
            '*** 1 check bill was selected  when current order of business is not 'Committee Reports' or 'Introduction of Bills' 
            If gFreeFormat = False Then
                'If UCase(Me.CurrentOrderOfBusiness.Text) = "BILLS ON THIRD READING" And Me.CurrentBill.Text = "" Then
                '    RetValue = DisplayMessage("Please select a bill to display", "No Bill Selected", "S")
                '    Exit Sub
                'End If
            End If


            '*** 2 send data information to Order of Business PC
            If SOOB_On Then
                SendMessageToOOB(strWorkArea)
            End If

            Me.lblWorkAreaDisplayed.Visible = True
            btnCreateHTMLPage.BackColor = Color.Red

            '*** 3 write work data to tblBills and update tblWork WorkData field
            UpdateBillWorkData()


            '*** 4 create HTML page ***'
            gCreateHTMLPage = True
            If UCase(Me.CurrentOrderOfBusiness.Text) = "ADJOURNMENT" Or UCase(Me.CurrentOrderOfBusiness.Text) = "CONVENE" Or UCase(Me.CurrentOrderOfBusiness.Text) = "RECESS" Or UCase(Me.CurrentOrderOfBusiness.Text) = "SINE DIE" Or UCase(Me.CurrentOrderOfBusiness.Text) = "HOUSE MESSAGES" Then
                CreateHTMLPage()
            Else
                CreateHTMLPage()
            End If

            '*** 5 copy HTML file to local
            CopyHTMLPageToLocal()

            '*** 6 initial parameters
            Me.OrderOfBusiness.Enabled = True
            Me.btnExit.Enabled = True
            If Me.CurrentBill.Text <> "" Then
                gBillDisplayed = True
            Else
                gBillDisplayed = False
            End If

            gCalendarCodeDisplayed = gCalendarCode
            gBillNbrDisplayed = gBillNbr
            gCalendarDisplayed = gCalendar
            gBillDisplayed = gBill

            Me.btnExit.Enabled = False
            Me.lblWorkAreaDisplayed.Visible = True
            gFreeFormat = False
        Catch ex As Exception
            DisplayMessage(ex.Message, "Failed Create HTML Page.", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub SendMessageToOOB(strWorkArea As String, Optional strOOB As String = "")
        Dim BillText As String = ""
        Dim strBody As String = ""
        Dim ds, dsC As New DataSet
        Dim BillCalendarPage As String = ""
        Dim SenatorSubject As String = ""
        Dim chkCalendar As String = ""

        Try
            chkCalendar = UCase(Me.Calendar.SelectedItem.ToString)
            If Strings.Left(chkCalendar, 2) = "SR" Or Strings.Left(chkCalendar, 2) = "SJ" Or Strings.Left(chkCalendar, 2) = "HR" Or Strings.Left(chkCalendar, 2) = "HJ" Or Strings.Left(chkCalendar, 2) = "SP" Then
                dsC = V_DataSet("Select * From tblSpecialOrderCalendar Where ucase(Bill) ='" & UCase(Me.CurrentBill.Text) & "'", "R")
            Else
                dsC = V_DataSet("Select * From tblBills Where ucase(Bill) ='" & UCase(Me.CurrentBill.Text) & "'", "R")
            End If

            If chkCalendar <> "MOTIONS" Then
                If dsC.Tables(0).Rows.Count = 0 Or Me.CurrentBill.Text = "" Then
                    If dsC.Tables(0).Rows.Count = 0 Or Me.CurrentBill.Text <> "" Then
                        BillCalendarPage = Me.CurrentBill.Text
                        SenatorSubject = "" & Space(1)
                    Else
                        BillCalendarPage = "" & Space(1)
                        SenatorSubject = "" & Space(1)
                    End If
                Else
                    For Each drC As DataRow In dsC.Tables(0).Rows
                        BillCalendarPage = drC("BillCalendarPage")
                        SenatorSubject = drC("SenatorSubject")
                        Exit For
                    Next
                End If
            Else
                SenatorSubject = Me.CurrentBill.Text
            End If
            If strWorkArea = "" Or strWorkArea = " " Then
                strWorkArea = "" & Space(1)
            End If

            If strOOB <> "ORDER OF BUSINESS" And strOOB <> "WELCOME" Then
                If UCase(Me.OrderOfBusiness.Text) = "ADJOURNMENT" Or UCase(Me.OrderOfBusiness.Text) = "CONVENE" Or UCase(Me.OrderOfBusiness.Text) = "RECESS" Or UCase(Me.OrderOfBusiness.Text) = "SINE DIE" Then
                    strBody = "gBillCalendarPage - ||" & "gSenatorSubject - " & Me.CurrentBill.Text & "||" & "gWorkArea - " & strWorkArea & "||" & "gCOOB - " & Me.CurrentOrderOfBusiness.Text
                Else
                    If UCase(Me.Calendar.SelectedItem.ToString) <> "CONFIRMATIONS" Then
                        Select Case UCase(Me.CurrentOrderOfBusiness.Text)
                            Case "INTRODUCTION OF BILLS", "HOUSE MESSAGES", "MOTIONS AND RESOLUTIONS", "BILLS ON THIRD READING", ""
                                If UCase(Me.Calendar.SelectedItem.ToString) <> "OTHER" Then
                                    '  If UCase(Me.Calendar.SelectedItem.ToString) <> "MOTIONS" Then
                                    If gFreeFormat = False Then
                                        strBody = "gBillCalendarPage - " & BillCalendarPage & "||" & "gSenatorSubject - " & SenatorSubject & "||" & "gWorkArea - " & strWorkArea & "||" & "gCOOB - " & Me.CurrentOrderOfBusiness.Text
                                    Else
                                        strBody = "gBillCalendarPage - " & Me.CurrentBill.Text & "||" & "gSenatorSubject - ||" & "gWorkArea - " & strWorkArea & "||" & "gCOOB - " & Me.CurrentOrderOfBusiness.Text
                                    End If
                                Else
                                    strBody = "gBillCalendarPage - " & "" & "||" & "gSenatorSubject - " & Me.CurrentBill.Text & "||" & "gWorkArea - " & strWorkArea & "||" & "gCOOB - " & Me.CurrentOrderOfBusiness.Text
                                End If
                            Case "LOCAL BILLS"
                                If UCase(Me.Calendar.SelectedItem.ToString) <> "OTHER" Then
                                    If gFreeFormat = False Then
                                        strBody = "gBillCalendarPage - " & BillCalendarPage & "||" & "gSenatorSubject - " & SenatorSubject & "||" & "gWorkArea - " & strWorkArea & "||" & "gCOOB - " & Me.CurrentOrderOfBusiness.Text
                                    Else
                                        strBody = "gBillCalendarPage - " & Me.CurrentBill.Text & "||" & "gSenatorSubject - ||" & "gWorkArea - " & strWorkArea & "||" & "gCOOB - " & Me.CurrentOrderOfBusiness.Text
                                    End If
                                Else
                                    strBody = "gBillCalendarPage - " & "" & "||" & "gSenatorSubject - " & Me.CurrentBill.Text & "||" & "gWorkArea - " & strWorkArea & "||" & "gCOOB - " & Me.CurrentOrderOfBusiness.Text
                                End If
                            Case "MOTIONS"
                                strBody = "gBillCalendarPage - " & Me.CurrentBill.Text & "||" & "gSenatorSubject - " & Me.CurrentBill.Text & "||" & "gWorkArea - " & strWorkArea & "||" & "gCOOB - " & Me.CurrentOrderOfBusiness.Text
                            Case "COMMITTEE REPORTS"
                                strBody = "gBillCalendarPage - ||" & "gSenatorSubject - " & Me.CurrentBill.Text & "||" & "gWorkArea - " & strWorkArea & "||" & "gCOOB - " & Me.CurrentOrderOfBusiness.Text
                            Case "ADJOURNMENT", "CONVENE", "RECESS", "SINE DIE"
                                strBody = "gBillCalendarPage - '' ||" & "gSenatorSubject - '' ||" & "gWorkArea - " & strWorkArea & "||" & "gCOOB - " & Me.CurrentOrderOfBusiness.Text
                        End Select
                    Else
                        strBody = "gBillCalendarPage - " & Me.CurrentBill.Text & "||" & "gSenatorSubject - ||" & "gWorkArea - " & strWorkArea & "||" & "gCOOB - " & Me.CurrentOrderOfBusiness.Text
                    End If
                End If
            Else
                If strOOB = "ORDER OF BUSINESS" Then
                    strBody = "gBillCalendarPage - '' ||" & "gSenatorSubject - '' ||" & "gWorkArea - '' ||" & "gCOOB - " & "ORDER OF BUSINESS"
                ElseIf strOOB = "WELCOME" Then
                    strBody = "gBillCalendarPage - '' ||" & "gSenatorSubject - '' ||" & "gWorkArea - '' ||" & "gCOOB - " & "WELCOME"
                End If
            End If

            Dim mq As New MessageQueue
            Dim msg As New Message

            Try
                mq.Path = gSendQueueToOOB                               '".\PRIVATE\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE\SenateVoteQueue"
                msg.Priority = MessagePriority.Normal
                msg.Label = "UPDATE"
                msg.Body = strBody
                mq.Send(msg)
                SOOB_On = True
                mq.Close()
            Catch ex As Exception
                SOOB_On = False
                If DisplayMessage(ex.Message, "Failed Send Update Dispaly Information." & "Would you want to continue without display Order Of Business?", "Y") Then
                    Exit Sub
                Else
                    End
                End If
            End Try
        Catch ex As Exception
            Dim p As New Process
            p = Process.Start(gCmdFile)
            p.WaitForExit()

            DisplayMessage(ex.Message, "Send Update Dispaly Information", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub SendStartOOBToOOB(ByVal strWorkArea As String, Optional ByVal strOOB As String = "")
        Dim strBody As String = ""

        Try
            If strOOB = "CLEAR DISPLAY" Then
                strBody = "gBillCalendarPage - '' ||" & "gSenatorSubject - '' ||" & "gWorkArea - '' ||" & "gCOOB - " & "CLEAR DISPLAY"
            ElseIf strOOB = "START" Then
                strBody = "gCOOB - " & Me.CurrentOrderOfBusiness.Text
            ElseIf strOOB = "CLEAR OOB" Then
                strBody = "gCOOB - " & "CLEAR OOB"
            End If

            Dim mq As New MessageQueue
            Dim msg As New Message

            Try
                mq.Path = gSendQueueToOOB                               '".\PRIVATE\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE\SenateVoteQueue"
                msg.Priority = MessagePriority.Normal
                msg.Label = "UPDATE"
                msg.Body = strBody
                mq.Send(msg)
                SOOB_On = True
                mq.Close()
            Catch ex As Exception
                mq.Close()
                SOOB_On = False
                If DisplayMessage(ex.Message, "Failed Send Clear Dispaly Information." & "Would you want to continue without display Order Of Business?", "Y") Then
                    Exit Sub
                Else
                    End
                End If
            End Try
        Catch ex As Exception
            Dim p As New Process
            p = Process.Start(gCmdFile)
            p.WaitForExit()

            DisplayMessage(ex.Message, "Send Update Dispaly Information", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub BillHasChanged()
        Dim WrkFld As String = ""
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim strWrkData As String = ""

        Try
            gBill = Strings.Replace(gBill, "'", "")

            If Strings.Left(gCalendarCode, 2) <> "SR" And Strings.Left(gCalendarCode, 3) <> "SOC" Then
                strSQL = "SELECT tblBills.CalendarCode, tblCalendars.Calendar, tblBills.Bill, tblBills.Sponsor, tblBills.Subject, tblBills.WorkData, tblBills.BillNbr " & _
                          " FROM tblCalendars LEFT JOIN tblBills ON tblCalendars.CalendarCode = tblBills.CalendarCode " & _
                          " WHERE (((tblBills.CalendarCode)=[tblCalendars].[CalendarCode]) AND ((ucase(tblCalendars.Calendar))='" & UCase(gCalendar) & "') AND ((UCase(tblBills.Bill))='" & UCase(gBill) & "'))"
            Else
                If gBill <> "" Then
                    gBillNbr = Mid(gBill, 1, InStr(gBill, " ") - 1)
                    strSQL = "SELECT tblSpecialOrderCalendar.CalendarCode, tblCalendars.Calendar, tblSpecialOrderCalendar.Bill, tblSpecialOrderCalendar.Sponsor, tblSpecialOrderCalendar.Subject, tblSpecialOrderCalendar.WorkData, tblSpecialOrderCalendar.BillNbr " & _
                           " FROM tblCalendars LEFT JOIN tblSpecialOrderCalendar ON tblCalendars.CalendarCode = tblSpecialOrderCalendar.CalendarCode " & _
                           " WHERE tblSpecialOrderCalendar.CalendarCode=[tblCalendars].[CalendarCode] AND (ucase(tblCalendars.Calendar)='" & UCase(gCalendar) & "') AND tblSpecialOrderCalendar.BillNbr='" & UCase(gBillNbr) & "' AND UCase(tblSpecialOrderCalendar.Bill)='" & UCase(gBill) & "'"
                End If
            End If
            ds = V_DataSet(strSQL, "R")
          
            For Each dr In ds.Tables(0).Rows
                gCalendarCode = NToB(UCase(dr("CalendarCode")))
                gBillNbr = NToB(dr("BillNbr"))
                gSenator = NToB(dr("Sponsor"))
                gSubject = NToB(dr("Subject"))
                If NToB(dr("Workdata")) = "" Then
                    strWrkData = ""
                Else
                    strWrkData = NToB(dr("Workdata"))
                End If

                If tPage0 Then
                    If strWrkData = "" Then
                        Me.WorkData.Text = ""
                    Else
                        Me.WorkData.Text = strWrkData
                    End If
                    work_bill = gBill
                ElseIf tPage1 Then
                    If strWrkData = "" Then
                        Me.WorkData1.Text = ""
                    Else
                        Me.WorkData1.Text = strWrkData
                    End If
                    work1_bill = gBill
                ElseIf tPage2 Then
                    If strWrkData = "" Then
                        Me.WorkData2.Text = ""
                    Else
                        Me.WorkData2.Text = strWrkData
                    End If
                    work2_bill = gBill
                End If
                '---end new

                ''---orginal
                'If tPage0 Then
                '    Me.WorkData.Text = NToB(dr("Workdata"))
                '    If NToB(dr("Workdata")) = "" Then
                '        Me.WorkData.Text = ""
                '    End If
                'ElseIf tPage1 Then
                '    Me.WorkData1.Text = NToB(dr("Workdata"))
                '    If NToB(dr("Workdata")) = "" Then
                '        Me.WorkData1.Text = ""
                '    End If
                'ElseIf tPage2 Then
                '    Me.WorkData2.Text = NToB(dr("Workdata"))
                '    If NToB(dr("Workdata")) = "" Then
                '        Me.WorkData2.Text = ""
                '    End If
                'End If
            Next

            If tPage0 Then
                If Me.WorkData.Text = "" Then
                    gCurrentPhrase = ""
                Else
                    WrkFld = vbCrLf & Me.WorkData.Text
                    If Strings.Right(WrkFld, 2) = vbCrLf Then
                        WrkFld = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) - 2)
                    End If
                    If InStrRev(WrkFld, vbCrLf) > 0 Then
                        gCurrentPhrase = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) + 2)
                    Else
                        gCurrentPhrase = Mid(WrkFld, 1)
                    End If

                    If InStr(Me.WorkData.Text, gBIR) = 1 Then
                        gCurrentPhrase = "BIR - " & gCurrentPhrase
                    End If
                End If
            ElseIf tPage1 Then
                If Me.WorkData1.Text = "" Then
                    gCurrentPhrase = ""
                Else
                    WrkFld = vbCrLf & Me.WorkData1.Text
                    If Strings.Right(WrkFld, 2) = vbCrLf Then
                        WrkFld = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) - 2)
                    End If
                    gCurrentPhrase = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) + 2)
                    If InStr(Me.WorkData1.Text, gBIR) = 1 Then
                        gCurrentPhrase = "BIR - " & gCurrentPhrase
                    End If
                End If
            ElseIf tPage2 Then
                If Me.WorkData2.Text = "" Then
                    gCurrentPhrase = ""
                Else
                    WrkFld = vbCrLf & Me.WorkData2.Text
                    If Strings.Right(WrkFld, 2) = vbCrLf Then
                        WrkFld = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) - 2)
                    End If
                    gCurrentPhrase = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) + 2)
                    If InStr(Me.WorkData2.Text, gBIR) = 1 Then
                        gCurrentPhrase = "BIR - " & gCurrentPhrase
                    End If
                End If
            End If

            '--- if this bill is the current one being displayed on the projection
            '--- screen then make visible the indicator boxes
            If gCalendarCode & gBillNbr = gCalendarCodeDisplayed & gBillNbrDisplayed Then
                Me.lblWorkAreaDisplayed.Visible = True
            Else
                Me.lblWorkAreaDisplayed.Visible = False
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Bill has been Changed", "S")
        End Try
    End Sub

    Private Sub UpdateBillWorkData()
        Dim str As String = ""
        Dim ds As New DataSet
        Dim CurrentWorkData As String = ""
        UpdateWorkDataSW = False

        Try
            If tPage0 Then
                CurrentWorkData = Me.WorkData.Text
            ElseIf tPage1 Then
                CurrentWorkData = Me.WorkData1.Text
            ElseIf tPage2 Then
                CurrentWorkData = Me.WorkData2.Text
            End If
            CurrentWorkData = Replace(CurrentWorkData, "'", "''")

            Dim input As String = CurrentWorkData
            Dim phrase As String = vbCrLf
            Dim Occurrences As Integer = 0
            If input <> "" Then
                Occurrences = (input.Length - input.Replace(phrase, String.Empty).Length) / phrase.Length
            End If
            If Occurrences = 1 Then
                CurrentWorkData = Replace(CurrentWorkData, vbCrLf, "")
            End If

            If Strings.Left(gCalendarCode, 2) = "SR" Then
                If tPage0 Then
                    strSQL = "Update tblSpecialOrderCalendar SET WorkData ='" & CurrentWorkData & "'  Where CalendarCode='" & gCalendarCode & "' AND ucase(Bill) ='" & UCase(work_bill) & "'"
                ElseIf tPage1 Then
                    strSQL = "Update tblSpecialOrderCalendar SET WorkData ='" & CurrentWorkData & "'  Where CalendarCode='" & gCalendarCode & "' AND ucase(Bill) ='" & UCase(work1_bill) & "'"
                ElseIf tPage2 Then
                    strSQL = "Update tblSpecialOrderCalendar SET WorkData ='" & CurrentWorkData & "'  Where CalendarCode='" & gCalendarCode & "' AND ucase(Bill) ='" & UCase(work2_bill) & "'"
                End If
                If CurrentWorkData <> "" Then
                    ds = V_DataSet(strSQL, "U")
                End If
            ElseIf Strings.Left(gCalendarCode, 3) = "SOC" Then
                strSQL = "Update tblSpecialOrderCalendar SET WorkData='" & CurrentWorkData & "' Where CalendarCode='SOC' AND ucase(BillNbr) ='" & UCase(gBillNbr) & "'"
                If CurrentWorkData <> "" Then
                    ds = V_DataSet(strSQL, "U")
                End If
            End If

            If gBillNbr <> "" Then
                '--- update tblBiils' WorkData
                If tPage0 Then
                    strSQL = "Update tblBills SET WorkData='" & CurrentWorkData & "'  Where CalendarCode='" & gCalendarCode & "' AND ucase(Bill) ='" & UCase(work_bill) & "'"
                ElseIf tPage1 Then
                    strSQL = "Update tblBills SET WorkData='" & CurrentWorkData & "' Where CalendarCode='" & gCalendarCode & "' AND ucase(Bill) ='" & UCase(work1_bill) & "'"
                ElseIf tPage2 Then
                    strSQL = "Update tblBills SET WorkData='" & CurrentWorkData & "'  Where CalendarCode='" & gCalendarCode & "' AND ucase(Bill) ='" & UCase(work2_bill) & "'"
                End If
                If CurrentWorkData <> "" Then
                    ds = V_DataSet(strSQL, "U")
                End If
            End If

            ds.Dispose()
        Catch ex As Exception
            DisplayMessage(ex.Message, "Update Bill and  Work Area Data", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub Committees_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Committees.SelectedIndexChanged
        If InStr(Me.Committees.Text, "-") > 0 Then
            InsertIntoWorkData(Mid(Me.Committees.Text, InStr(Me.Committees.Text, "-") + 2), gCommitteeInsertionPoint)
        Else
            InsertIntoWorkData(Mid(Me.Committees.Text, 1), gCommitteeInsertionPoint)
        End If
    End Sub

    Private Sub CurrentBill_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles CurrentBill.LostFocus
        Dim i, j As Integer
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim str As String

        '--- if bill info is changed, then update db - skip if bill has been blanked out;
        '--- if freeformat or from motion calendar then dont change
        If (gFreeFormat) Or (gCalendarCode = "M") Then
            gBill = Me.CurrentBill.Text
        ElseIf (Me.CurrentBill.Text <> gBill) And (Me.CurrentBill.Text <> "") Then
            If Strings.Right(Me.CurrentBill.Text, 2) = vbCrLf Then      ' drop trailing crlf
                Me.CurrentBill.Text = Mid(Me.CurrentBill.Text, 1, Len(Me.CurrentBill.Text) - 2)
            End If

            '--- capture subject since it probably changed
            If gSubject <> "" Then
                i = InStr(Me.CurrentBill.Text, " - ")
                j = InStr(Me.CurrentBill.Text, " p.")                   ' calendar page at end of line

                If i > 0 Or j > 0 Then
                    gSubject = Mid(Me.CurrentBill.Text, i + 3, j - i - 3)
                Else
                    gSubject = Mid(Me.CurrentBill.Text, 1)
                End If
            End If

            str = "UPDATE tblBills SET Bill = '" & Me.CurrentBill.Text & "', Subject = '" & gSubject & "'  WHERE (CalendarCode = '" & gCalendarCode & "') AND (ucase(BillNbr) = '" & UCase(gBillNbr) & "')"
            V_DataSet(str, "U")
            gBill = Me.CurrentBill.Text

            '--- Reload bill list box after changed bill
            str = "SELECT tblBills.BillNbr, tblBills.Bill FROM tblBills, tblCalendars WHERE tblBills.CalendarCode = tblCalendars.CalendarCode AND (tblCalendars.Calendar = '" & gCalendar & "') ORDER BY tblBills.BillNbr"
            ds = V_DataSet(str, "R")
            Me.Bill.Items.Clear()
            For Each dr In ds.Tables(0).Rows
                Me.Bill.Items.Add(dr("Bill"))
            Next
        End If
    End Sub

    Private Sub FindBillNbr_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles FindBillNbr.KeyDown
        Try

            If e.KeyValue = Keys.Enter Then
                Dim FieldName(8), FieldValue(8) As Object
                Dim k, x As Integer
                Dim Title As String, WrkFld As String
                Dim ds, dsA As New DataSet
                Dim dr As DataRow

                If NToB(Me.FindBillNbr.Text) = "" Then
                    Exit Sub
                End If

                '---first check for bill on Access DB; if not found, then go to ALIS;
                '---update calendar and bill positions in look ups; dont go to ALIS
                '---in pure test mode


                strSQL = "SELECT  tblCalendars.Calendar AS Calendar, tblBills.* FROM tblBills, tblCalendars WHERE tblBills.CalendarCode = tblCalendars.CalendarCode AND (ucase(tblBills.BillNbr) = '" & UCase(Me.FindBillNbr.Text) & "')"
                ds = V_DataSet(strSQL, "R")
                x = ds.Tables(0).Rows.Count

                '  For Each dr In ds.Tables(0).Rows
                If x > 0 Then
                    For Each dr In ds.Tables(0).Rows
                        Me.Calendar.Text = dr("Calendar")
                        Calendar_Click(sender, e)
                        Me.Bill.Text = dr("Bill")
                        Bill_Click(sender, e)
                    Next
                ElseIf (gTestMode) And (Not gWriteVotesToTest) Then
                Else
                    '---check ALIS                
                    WrkFld = UCase(Me.FindBillNbr.Text)
                    If Strings.Left(WrkFld, 2) = "CF" Then
                        WrkFld = "SCF"
                    End If

                    strSQL = " SELECT I.OID, I.LABEL, I.SPONSOR, I.INDEX_WORD, I.LEGISLATIVE_BODY, I.COMMITTEE, I.RECOMMENDATION, " & _
                                    " TO_CHAR(I.LAST_TRANSACTION_DATE, 'MM/DD/YYYY') AS LAST_TRANSACTION_DATE, I.STATUS_CODE, I.TYPE_CODE, " & _
                                    " I.ENACTED_YEAR, I.ENACTED_SEQUENCE, CV.VALUE AS STATUS_TEXT, D.SHORTTITLE " & _
                            " FROM ALIS_OBJECT I, CODE_VALUES CV, " & _
                                    " DOCUMENT_VERSION DV, DOCUMENT D " & _
                            " WHERE I.STATUS_CODE = CV.CODE AND I.OID_CURRENT_DOCUMENT_VERSION = DV.OID (+) AND DV.OID_DOCUMENT = D.OID (+) AND (I.INSTRUMENT = 'T') " & _
                                    " AND (I.OID_SESSION = " & gSessionID & ") AND (I.LABEL = '" & WrkFld & "') AND (CV.CODE_TYPE = 'InstrumentStatus')"
                    dsA = ALIS_DataSet(strSQL, "R")

                    If dsA.Tables(0).Rows.Count = 0 Then
                        MsgBox(WrkFld & " instrument can not found.", vbInformation, "Find Instrument")
                        Exit Sub
                    End If

                    '---add the bill to the other category
                    For k = 0 To 6
                        FieldName(k) = ds.Tables(0).Columns(k).ColumnName
                    Next k

                    FieldValue(0) = "ZZ"


                    For Each drA As DataRow In dsA.Tables(0).Rows
                        FieldValue(1) = drA("Label")
                        If Strings.Left(FieldValue(1), 1) = "S" Then
                            Title = " by Senator "
                        Else
                            Title = " by Representative "
                        End If
                        FieldValue(3) = Replace(NToB(drA("Sponsor")), "'", "''")
                        FieldValue(4) = Replace(NToB(drA("Index_Word")), "'", "''")
                        FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4)
                        FieldValue(5) = ""
                        FieldValue(6) = ""
                        FieldValue(7) = Title & " " & FieldValue(3) & " - " & FieldValue(4)

                        If FieldValue(6) <> "" Then
                            FieldValue(8) = FieldValue(1) & "&nbsp;&nbsp;&nbsp;&nbsp;" & " p." & FieldValue(6)
                        Else
                            FieldValue(7) = FieldValue(1)
                        End If
                        V_DataSet("INSERT INTO tblBills VALUES ('" & FieldValue(0) & "', '" & FieldValue(1) & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "', '" & Replace(FieldValue(7), "'", "") & "', '" & Replace(FieldValue(8), "'", "") & "')", "A")
                        Exit For
                    Next
                    '---put in other category
                    Me.Calendar.Text = "Other"
                    Calendar_Click(sender, e)
                    Me.Bill.Text = FieldValue(2)
                    Bill_Click(Bill, Nothing)
                    FindBillNbr.Text = ""
                End If
            End If

        Catch ex As OleDb.OleDbException
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub frmChamberDisplay_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        '--- Make the return key act like a tab key
        '--- If Keys.Return = vbCrLf Then
        If Keys.Return = Keys.Return Then
            SendKeys.Send("{Tab}")
        End If
    End Sub

    Private Sub InsertText_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles InsertText.KeyDown
        If e.KeyValue = Keys.Enter Then
            InsertIntoWorkData(Me.InsertText.Text, gTextInsertionPoint)
            Me.InsertText.Text = ""
        End If
    End Sub

    Private Sub OrderOfBusiness_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OrderOfBusiness.Click
        If SOOB_On Then
            SendStartOOBToOOB("", "START")
        End If

        Dim strWorkArea As String = ""
        If tPage0 Then
            strWorkArea = WorkData.Text
        ElseIf tPage1 Then
            strWorkArea = WorkData1.Text
        ElseIf tPage2 Then
            strWorkArea = WorkData2.Text
        End If
       
        CreateOtherHTMLPage()

        If UCase(Me.OrderOfBusiness.Text) = "ADJOURNMENT" Or UCase(Me.OrderOfBusiness.Text) = "CONVENE" Or UCase(Me.OrderOfBusiness.Text) = "RECESS" Or UCase(Me.OrderOfBusiness.Text) = "SINE DIE" Then
            Me.CurrentBill.Text = Me.OrderOfBusiness.Text
        End If
    End Sub

    Private Sub OrderOfBusiness_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles OrderOfBusiness.DoubleClick
        Dim strWorkArea As String = ""

        If SOOB_On Then
            SendStartOOBToOOB("", "START")
        End If

        CreateOtherHTMLPage()


        If UCase(Me.OrderOfBusiness.Text) = "ADJOURNMENT" Or UCase(Me.OrderOfBusiness.Text) = "CONVENE" Or UCase(Me.OrderOfBusiness.Text) = "RECESS" Or UCase(Me.OrderOfBusiness.Text) = "SINE DIE" Then
            Me.CurrentBill.Text = Me.OrderOfBusiness.Text
        End If
    End Sub

    Private Sub OrderOfBusiness_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles OrderOfBusiness.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If SOOB_On Then
                SendStartOOBToOOB("", "CLEAR OOB")
            End If
            CreateClearHTMLPage()
        End If
    End Sub

    Private Sub OrderOfBusiness_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderOfBusiness.SelectedIndexChanged
        calendarClick = True
        lblWorkAreaDisplayed.Visible = False
        Me.CurrentOrderOfBusiness.Text = OrderOfBusiness.SelectedItem.ToString
        Highlight = Me.OrderOfBusiness.SelectedIndex

        Dim strWorkArea As String = ""
        If tPage0 Then
            strWorkArea = WorkData.Text
        ElseIf tPage1 Then
            strWorkArea = WorkData1.Text
        ElseIf tPage2 Then
            strWorkArea = WorkData2.Text
        End If
        gCreateHTMLPage = True


        If SOOB_On Then
            SendStartOOBToOOB("", "START")
        End If

        If UCase(Me.OrderOfBusiness.Text) = "ADJOURNMENT" Or UCase(Me.OrderOfBusiness.Text) = "CONVENE" Or UCase(Me.OrderOfBusiness.Text) = "RECESS" Or UCase(Me.OrderOfBusiness.Text) = "SINE DIE" Then
            Me.CurrentBill.Text = Me.OrderOfBusiness.Text
        End If
    End Sub

    Private Sub Phrases_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Phrases.SelectedIndexChanged
        If tPage0 Then
            If Strings.Right(WorkData.Text, 2) <> vbCrLf And WorkData.Text <> "" Then
                WorkData.Text = WorkData.Text & vbCrLf
            End If
        ElseIf tPage1 Then
            If Strings.Right(WorkData1.Text, 2) <> vbCrLf And WorkData1.Text <> "" Then
                WorkData1.Text = WorkData1.Text & vbCrLf
            End If
        ElseIf tPage2 Then
            If Strings.Right(WorkData1.Text, 2) <> vbCrLf And WorkData1.Text <> "" Then
                WorkData1.Text = WorkData1.Text & vbCrLf
            End If
        End If
        If InStr(Me.Phrases.Text, "-") > 0 Then
            InsertIntoWorkData(Mid(Me.Phrases.Text, InStr(Me.Phrases.Text, "-") + 2), gPhraseInsertionPoint)
        Else
            InsertIntoWorkData(Mid(Me.Phrases.Text, 1), gPhraseInsertionPoint)
        End If
    End Sub

    Private Sub CreateHTMLPage()
        '--- print the doc in HTML and put on the web server so all can see - done everytime the chamber
        '--- display operator hits update display
        Dim ds, dsC As New DataSet
        Dim dr As DataRow
        Dim WrkFld As String = ""
        Dim BillCalendarPage As String = ""
        Dim SenatorSubject As String = ""

        Try
            If tPage0 Then
                WrkFld = Me.WorkData.Text
            ElseIf tPage1 Then
                WrkFld = Me.WorkData1.Text
            ElseIf tPage2 Then
                WrkFld = Me.WorkData2.Text
            End If

            dsC = V_DataSet("Select * From tblBills Where ucase(Bill) ='" & UCase(Me.CurrentBill.Text) & "'", "R")
            If dsC.Tables(0).Rows.Count = 0 Or Me.CurrentBill.Text = "" Then
                BillCalendarPage = ""
                SenatorSubject = ""
            Else
                For Each drC As DataRow In dsC.Tables(0).Rows
                    BillCalendarPage = NToB(drC("BillCalendarPage"))
                    SenatorSubject = NToB(drC("SenatorSubject"))
                Next
            End If

            '---check file is existing or not, other create new
            If FileExistes(gHTMLFile) = False Then
                File.Create(gHTMLFile)
            Else
                Kill(gHTMLFile)             '--make sure close the .htm file first
            End If

            strSQL = "Select * From tblOrderOfBusiness"
            ds = V_DataSet(strSQL, "R")

            FileOpen(1, gHTMLFile, OpenMode.Output)

            Print(1, "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
            Print(1, "<html xmlns='http://www.w3.org/1999/xhtml'>")
            Print(1, "<head id='Head1' runat='server'>")
            Print(1, "<title>Alabama Senate Voting System</title>")
            Print(1, "<meta http-equiv='refresh' content='2'/>")
            Print(1, "<link rel='stylesheet' type='text/css' href='Styles/styleIE.css' />")
            Print(1, "</head>")
            Print(1, "<body class ='Body'>")
            Print(1, "<div class='header'>")
            Print(1, "<div class='headgraphic' >")
            Print(1, "</div> <!-- end div headgraphic -->")
            Print(1, "</div> <!-- end div header-->")
            Print(1, "<!-- Begin Insertion Point -->")
            Print(1, "<div class='order_of_business'>")
            Print(1, "<div class='order_of_business_heading'>")
            Print(1, "<p style='margin-top:20px;'>")
            For Each dr In ds.Tables(0).Rows
                If dr("OrderOfBusiness") = Me.CurrentOrderOfBusiness.Text Then
                    Print(1, Me.CurrentOrderOfBusiness.Text)
                    Exit For
                End If
            Next
            Print(1, "</p>")
            Print(1, "</div>")
            Print(1, "</div>")
            Print(1, "<div class='page'>")
            Print(1, "<div class='content'>")
            Print(1, "<div class='content_bill'>")

            If UCase(Me.Calendar.Text) <> "MOTIONS" And UCase(Me.Calendar.Text) <> "CONFIRMATIONS" And UCase(Me.Calendar.Text) <> "OTHER" And UCase(Strings.Left(Me.Calendar.Text, 2)) <> "SR" And UCase(Strings.Left(Me.Calendar.Text, 2)) <> "SP" Then
                If Me.CurrentBill.Text <> "" Then
                    Print(1, BillCalendarPage)
                End If
                Print(1, "</div>")
                Print(1, "<div class='content_subject'>")
                If Me.CurrentBill.Text <> "" Then
                    If BillCalendarPage <> "" Then
                        Print(1, " by " & SenatorSubject)
                    Else
                        Print(1, SenatorSubject)
                    End If

                End If
            Else
                Print(1, "</div>")
                Print(1, "<div class='content_subject'>")
                If Me.CurrentBill.Text <> "" Then
                    Print(1, Me.CurrentBill.Text)
                End If
            End If

            Print(1, "</div>")
            Print(1, "<div class='content_text'>")
            '--- put work area data here
            Print(1, " <div class='content_text'>")

            If Strings.Right(WrkFld, 2) <> vbCrLf Then
                WrkFld = WrkFld & vbCrLf
            End If
            Do
                If InStr(WrkFld, vbCrLf) > 0 Then
                    If InStr(WrkFld, vbCrLf) > 0 Then
                        Print(1, Mid(WrkFld, 1, InStr(WrkFld, vbCrLf) - 1))
                    Else
                        Print(1, Mid(WrkFld, 1))
                    End If

                Else
                    Print(1, Mid(WrkFld, 1))
                End If

                Print(1, "<BR>")
                If InStr(WrkFld, vbCrLf) = Len(WrkFld) - 1 Then
                    Exit Do
                End If
                WrkFld = Mid(WrkFld, InStr(WrkFld, vbCrLf) + 2)
            Loop
            Print(1, "</div>")
            Print(1, "  </div>")
            ' End If

            Print(1, "  </div>      <!-- end div content -->")
            Print(1, " <br class='clearfix' />")
            Print(1, "</div> <!-- </div> end div page -->")
            Print(1, "  <!-- End Insertion Point-->")
            Print(1, "</body>")
            Print(1, "</html>")

            FileClose()
        Catch ex As Exception
            DisplayMessage(ex.Message, "Create HTML Page Failed!", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub CreateOtherHTMLPage()
        Dim wrkFld As String = ""

        Try
            If FileExistes(gHTMLFile) = False Then
                File.Create(gHTMLFile)
            Else
                Kill(gHTMLFile)             '--make sure close the .htm file first
            End If
            FileOpen(1, gHTMLFile, OpenMode.Output)

            Print(1, "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
            Print(1, "<html xmlns='http://www.w3.org/1999/xhtml'>")
            Print(1, "<head id='Head1' runat='server'>")
            Print(1, "<title>Alabama Senate Voting System</title>")
            Print(1, "<meta http-equiv='refresh' content='2'/>")
            Print(1, "<link rel='stylesheet' type='text/css' href='Styles/styleIE.css' />")
            Print(1, "</head>")
            Print(1, "<body class ='Body'>")
            Print(1, "<div class='header'>")
            Print(1, "<div class='headgraphic' >")
            Print(1, "</div> <!-- end div headgraphic -->")
            Print(1, "</div> <!-- end div header-->")
            Print(1, "<!-- Begin Insertion Point -->")
            Print(1, "<div class='order_of_business'>")
            Print(1, "<div class='order_of_business_heading'>")
            If UCase(Me.CurrentOrderOfBusiness.Text) = "ADJOURNMENT" Then
                Print(1, "<p style='margin-top:20px;'>Adjournment </p>")
            ElseIf UCase(Me.CurrentOrderOfBusiness.Text) = "CONVENE" Then
                Print(1, "<p style='margin-top:20px;'>Convene </p>")
            ElseIf UCase(Me.CurrentOrderOfBusiness.Text) = "RECESS" Then
                Print(1, "<p style='margin-top:20px;'>Recess </p>")
            ElseIf UCase(Me.CurrentOrderOfBusiness.Text) = "SINE DIE" Then
                Print(1, "<p style='margin-top:20px;'>Sine Die </p>")
            ElseIf UCase(Me.CurrentOrderOfBusiness.Text) = "HOUSE MESSAGES" Then
                Print(1, "<p style='margin-top:20px;'>House Messages </p>")
            Else
                Print(1, "<p style='margin-top:20px;'>" & Me.OrderOfBusiness.Text & " </p>")
            End If
            Print(1, "</div>")
            Print(1, "</div>")
            Print(1, "<div class='page'>")
            Print(1, "<div class='content'>")
            Print(1, "<div class='content_recess'>")
            If calendarClick = False Then
                If tPage0 Then
                    wrkFld = Me.WorkData.Text
                ElseIf tPage1 Then
                    wrkFld = Me.WorkData1.Text
                ElseIf tPage2 Then
                    wrkFld = Me.WorkData2.Text
                End If
                If Strings.Right(wrkFld, 2) <> vbCrLf Then
                    wrkFld = wrkFld & vbCrLf
                End If
                Print(1, wrkFld)
            Else
                Print(1, Chr(32))
            End If
         
            Print(1, "</div>")
            Print(1, "</div>    <!-- end div content -->")
            Print(1, "<br class='clearfix' />")
            Print(1, "</div> <!-- </div> end div page -->")
            Print(1, "<!-- End Insertion Point-->")
            Print(1, "</body>")
            Print(1, "</html>")

            FileClose(1)
        Catch ex As Exception
            DisplayMessage(ex.Message, "Create HTML Page Failed!", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub CreateWelcomeHTMLPage()
        Try
            If FileExistes(gHTMLFile) = False Then
                File.Create(gHTMLFile)
            Else
                Kill(gHTMLFile)             '--make sure close the .htm file first
            End If

            FileOpen(1, gHTMLFile, OpenMode.Output)
            Print(1, "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
            Print(1, "<html xmlns='http://www.w3.org/1999/xhtml'>")
            Print(1, "<head id='Head1' runat='server'>")
            Print(1, "<title>Alabama Senate Voting System</title>")
            Print(1, "<link rel='stylesheet' type='text/css' href='Styles/styleIE.css' />")
            Print(1, "<meta http-equiv='refresh' content='2'/>")
            Print(1, "<div class='content'>")
            Print(1, "</head>")
            Print(1, "<body class ='Body'>")
            Print(1, "<div class='header'>")
            Print(1, "<div class='headgraphic' >")
            Print(1, "</div> <!-- end div headgraphic -->")
            Print(1, "</div> <!-- end div header-->")
            Print(1, "<!-- Begin Insertion Point -->")
            Print(1, "<div class='order_of_business'>")
            Print(1, "<div class='order_of_business_heading'>")
            Print(1, "<p style='margin-top:20px;'>&nbsp; </p>")
            Print(1, "</div>")
            Print(1, "</div>")
            Print(1, "<div class='page'>")
            Print(1, "<div class='content'>")
            Print(1, "<div class='content_welcome'>")
            Print(1, "<p style='text-align:center; font-size:50px; font-weight:bold; text-shadow:2px 2px #FFCC00; line-height:100%;height:70px;'>Welcome </p>")
            Print(1, "<p style='text-align:center; font-size:40px; text-shadow:2px 2px #FFCC00;height:50px;'>to the </p>")
            Print(1, "<p style='text-align:center; font-size:55px;text-shadow:2px 2px #FFCC00;  line-height:105%;height:80px;'>Alabama Senate </p>")
            Print(1, "</div>")
            Print(1, "</div>      <!-- end div content -->")
            Print(1, "<br class='clearfix' />")
            Print(1, "</div> <!-- </div> end div page -->")
            Print(1, "</body>")
            Print(1, "</html>")

            FileClose()
        Catch
            DisplayMessage("Faild To Create Welcome HTML Page.", "Create Welcome HTML Page", "S")
            Exit Sub
        End Try
    End Sub

    Public Sub Calendar_Bill(ByVal strCalendar As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim str As String

        '--- select the calendar and requery the child bill list
        If UpdateWorkDataSW Then
            ' UpdateBillWorkData()
        End If
        WorkData.Text = ""
        UpdateWorkDataSW = False
        gCalendar = strCalendar
        lblWorkAreaDisplayed.Visible = False            ' reset in case it was on
        CurrentCalendar.Text = strCalendar

        str = "SELECT t.BillNbr, t.Bill FROM tblBills t, tblCalendars tC WHERE t.CalendarCode=tC.CalendarCode AND t.CalendarCode='1' Order By t.BillNbr"
        ds = V_DataSet(str, "R")
        For Each dr In ds.Tables(0).Rows
            Bill.Items.Add(dr("Bill"))
        Next

        Me.CurrentBill.Text = ""

        '--- if motion calendar (code 9) is selected, then change caption on
        '--- current bill field
        If gCalendar = "Motions" Then
            Me.lblCurrentBill.Text = "Current Motion"
            Me.lblBills.Text = "Motions"
        Else
            Me.lblCurrentBill.Text = "Current Bill"
            Me.lblBills.Text = "Bills For The Current Calendar"
        End If
    End Sub

    Public Sub Senators_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Senators.SelectedIndexChanged
        Dim i As Integer

        InsertIntoWorkData("Senator " & Me.Senators.Text, gSenatorInsertionPoint)

        '--- also try to insert in current bill
        i = InStr(NToB(Me.CurrentBill.Text), gSenatorInsertionPoint)
        If i > 0 Then
            Me.CurrentBill.Text = Mid(Me.CurrentBill.Text, 1, i - 1) & "Senator " & Me.Senators.Text & Mid(Me.CurrentBill.Text, i + 1)
        End If
    End Sub

    Private Sub SendMessageToVotePCQueue(ByVal label As String, ByVal strBody As String)
        Dim mq As New MessageQueue
        Dim msg As New Message
        Dim ds As New DataSet

        Try
            SVotePC_On = True
            mq.Path = gSendQueueToVotePC           '".\PRIVATE\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE\SenateVoteQueue"
            msg.Priority = MessagePriority.Normal
            msg.Label = label
            msg.Body = strBody
            mq.Send(msg)

            ReceiveTimer.Enabled = True
            ReceiveTimer.Start()
            ReceiveTimer.Interval = gReceiveQueueTimer
            mq.Close()
        Catch ex As Exception
            Try
                Dim p As New Process
                p = Process.Start(gCmdFile)
                p.WaitForExit()
                SendMessageToVotePCQueue("STARTVOTE", bodyParamters)
            Catch em As Exception
                SVotePC_On = False
                EnableControls()
                btnVote.BackColor = System.Drawing.Color.LimeGreen
                Me.lblChamberLight.Text = "Display Next Bill"
                Me.lblChamberLight.BackColor = Color.LimeGreen
                btnVote.Text = "Vote"
                ReceiveTimer.Enabled = False
                ReceiveTimer.Stop()
                If DisplayMessage(ex.Message & " Syetem can not communicate to Vote PC! Would you want to continue?", "Send Message to Vote PC", "Y") Then
                    Exit Sub
                Else
                    End
                End If
            End Try
        End Try
    End Sub

    Private Sub SendMessageForVoteID(ByVal label As String, ByVal strBody As String)
        Dim mq As New MessageQueue
        Dim msg As New Message
        Dim ds As New DataSet
        Dim mPath As String = "FormatName:DIRECT=OS:SENATEVOTING\PRIVATE$\requestvoteidqueue"

        Try
            SVotePC_On = True
            mq.Path = mPath           '".\PRIVATE\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE\SenateVoteQueue"
            msg.Priority = MessagePriority.Normal
            msg.Label = label
            msg.Body = strBody
            mq.Send(msg)

            EnableControls()
            mq.Close()
        Catch ex As Exception
            Try
                Dim p As New Process
                p = Process.Start(gCmdFile)
                p.WaitForExit()
                SendMessageToVotePCQueue("STARTVOTE", bodyParamters)
                mq.Close()
            Catch em As Exception
                SVotePC_On = False
                EnableControls()
                btnVote.BackColor = System.Drawing.Color.LimeGreen
                Me.lblChamberLight.Text = "Display Next Bill"
                Me.lblChamberLight.BackColor = Color.LimeGreen
                btnVote.Text = "Vote"
                ReceiveTimer.Enabled = False
                ReceiveTimer.Stop()
                If DisplayMessage(ex.Message & " Syetem can not communicate to Vote PC! Would you want to continue?", "Send Message to Vote PC", "Y") Then
                    mq.Close()
                    Exit Sub
                Else
                    End
                End If
            Finally
                mq.Close()
            End Try
        End Try
    End Sub

    Private Sub btnRequestLastVoteID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRequestLastVoteID.Click
        If SVotePC_On Then
            askVoteID = True
            lblChamberLight.BackColor = Color.Yellow
            lblChamberLight.Text = "Request Vote ID"
            btnVote.Enabled = False

            SendMessageForVoteID("REQUESTVOTEID", "")
        End If
    End Sub

    Private Sub PingComputers()
        Dim RetValue As New Object
        Dim Ping As Ping
        Dim pReply As PingReply
        Dim mes As String = "Ping Computers"
        Dim intTime As Long

        intTime += 1
        Ping = New Ping
        SDisplay_On = True

        If intTime > 6000 Then
            Try
                '--- ping Voting PC 
                pReply = Ping.Send(gVotePC_IPAddress)
                If pReply.Status = IPStatus.Success Then
                    SVotePC_On = True
                    gOnlyOnePC = False

                    ReceiveTimer.Enabled = True
                    ReceiveTimer.Interval = gReceiveQueueTimer
                Else
                    ReceiveTimer.Enabled = False
                    SVotePC_On = False
                    gOnlyOnePC = True
                    V_DataSet("Delete From tblOnlyOnePC", "D")
                    V_DataSet("Insert Into tblOnlyOnePC Values (1, '" & SDIS & "','')", "A")
                    ' V_DataSet("Update tblOnlyOnePC set OnlyOnePCName='" & SDIS & "', OnlyOnePC = 1", "U")

                    If Not DisplayMessage("Unable Communicate to Senate Voting Computers. Would you want continue?", "Senate Vote PC is Offline", "Y") Then
                        '--- before shotdown update work area value to database
                        UpdateBillWorkData()
                        End
                    Else
                        EnableControls()
                        btnVote.Enabled = False
                        ReceiveTimer.Enabled = False
                        ReceiveTimer.Stop()
                        lblChamberLight.BackColor = Color.LimeGreen
                        lblChamberLight.Text = "Display Next Bill"
                        RetValue = DisplayMessage("Senate Voting Computer if offline. Unable open machine to vote! Would you want to continue?", mes, "I")
                    End If
                End If
            Catch
                SVotePC_On = False
                Exit Sub
            End Try
        End If
    End Sub

    Private Sub ReceiveTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReceiveTimer.Tick
        'If SVotePC_On Or gOnlyOnePC = False Then
        ReceiveMessageFromQueue()
        'End If
    End Sub

    Private Sub ReceiveMessageFromQueue()
        Dim strText As String = ""

        Try
            ' gReceiveQueueName = ".\PRIVATE$\senatevotequeue"
            If (MessageQueue.Exists(gReceiveQueueName)) = False Then
                MessageQueue.Create(gReceiveQueueName)
            End If

            Dim queue As New MessageQueue(gReceiveQueueName)
            queue.Formatter = New XmlMessageFormatter(New String() {"System.String"})
            Dim qenum As MessageEnumerator

            qenum = queue.GetMessageEnumerator2
            While qenum.MoveNext
                Dim m As Message = qenum.Current
                If m.Label = "VOTING" Then
                    gVotingStarted = True
                    lblChamberLight.BackColor = Color.Red
                    lblChamberLight.Text = "Waiting For Voting"
                    btnCancelVote.Enabled = False
                    btnClearDisplay.Enabled = False
                    OrderOfBusiness.Enabled = False
                    btnExit.Enabled = False
                ElseIf m.Label = "CLOSEDVOTE" Then
                    msgText = m.Body
                    m = queue.Receive(New TimeSpan(1000))
                    If msgText <> "" Then
                        If InStr(m.Body, "||") > 0 Then
                            txtVoteID.Text = Mid(m.Body, 1, InStr(m.Body, "||") - 1) + 1
                        Else
                            txtVoteID.Text = Mid(m.Body, 1) + 1
                        End If


                        Dim votedBillNbr As String = Mid(m.Body, InStr(m.Body, "||") + 2)
                        EnableControls()
                        Me.btnVote.BackColor = Color.LimeGreen
                        Me.lblChamberLight.BackColor = Color.LimeGreen
                        Me.lblChamberLight.Text = "Display Nex Bill"
                        Me.btnUnlock.Enabled = True
                        gVotingStarted = False

                        If gTestMode Then
                            strSQL = "Update tblVotingParameters Set ParameterValue='" & txtVoteID.Text & "' Where ucase(Parameter) = 'LASTVOTEIDFORTHISSESSIONTEST'"
                            V_DataSet(strSQL, "U")
                        Else
                            strSQL = "Update tblVotingParameters Set ParameterValue='" & txtVoteID.Text & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSION'"
                            V_DataSet(strSQL, "U")
                        End If
                    Else
                        DisplayMessage("Incorrect voting data! Please request correct voting data.", "Request correct voting data", "S")
                        SendMessageToVotePCQueue("RECALL", "")
                    End If
                ElseIf m.Label = "LASTVOTEDID" Then
                    m = queue.Receive(New TimeSpan(1000))
                
                    'Me.txtVoteID.Text = Val(m.Body)
                    'iniVoteID = Val(m.Body)
                    Me.lblChamberLight.BackColor = Color.LimeGreen
                    If Me.btnVote.BackColor = Color.Red Then
                        Me.btnVote.BackColor = Color.LimeGreen
                        Me.btnVote.Enabled = True
                    End If
                    Me.btnUpdateDisplay.Enabled = True
                    Me.lblChamberLight.Text = "Display Next Bill"
                    gVotingStarted = False
                    If Me.txtVoteID.Text = Val(m.Body) Then
                        Me.txtVoteID.Text = Val(m.Body)
                        iniVoteID = Val(m.Body)
                        gVoteID = Val(m.Body)
                    Else
                        Me.txtVoteID.Text = Val(m.Body) + 1
                        iniVoteID = Val(m.Body) + 1
                        gVoteID = Val(m.Body) + 1
                    End If
                
                    ' gVoteID = Val(m.Body)
                    queue.Close()
                    Exit Sub
                ElseIf m.Label = "ANSWERVOTEID" Then
                    m = queue.Receive(New TimeSpan(1000))
                    Me.txtVoteID.Text = m.Body
                    gVoteID = Val(m.Body)
                    Me.lblChamberLight.BackColor = Color.LimeGreen
                    Me.lblChamberLight.Text = "Display Next Bill"
                    gVotingStarted = False
                    queue.Close()
                    Exit Sub
                ElseIf m.Label = "CANCEL" Then
                    m = queue.Receive(New TimeSpan(1000))
                    Me.lblChamberLight.BackColor = Color.LimeGreen
                    Me.lblChamberLight.Text = "Display Next Bill"
                    Me.btnVote.BackColor = Color.LimeGreen
                    'Me.btnVote.Enabled = True
                    'Me.btnCancelVote.Enabled = True
                    'Me.OrderOfBusiness.Enabled = True
                    EnableControls()
                    gVotingStarted = False
                    queue.Close()
                    ' Exit Sub
                End If
                SVotePC_On = True
                ' PingTimer.Enabled = False
            End While

            queue.Close()
        Catch ex As Exception
            Try
                Dim p As New Process
                p = Process.Start(gCmdFile)
                p.WaitForExit()
                SVotePC_On = True
            Catch Mx As Exception
                SVotePC_On = False
                DisplayMessage(ex.Message, "Receive Message from Vote PC", "S")
                Exit Sub
            End Try
        End Try
    End Sub

    Private Sub btnVoteID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If SVotePC_On Then
            askVoteID = True
            lblChamberLight.BackColor = Color.Yellow
            lblChamberLight.Text = "Request The Last Vote ID"
            btnVote.Enabled = False

            SendMessageToVotePCQueue("ASKVOTEID", "")
            ' clear()
        End If
    End Sub

    Private Sub SOCNbr_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles SOCNbr.KeyDown
        Try
            If e.KeyCode = Keys.Enter And SOCNbr.Text <> "" Then
                Dim ds, dsA, dsC, dsDoc As New DataSet
                Dim strSocLabel As String = ""
                Dim drA, drC As DataRow
                Dim str As String
                Dim j, docID As Integer
                Dim FieldValue() As Object
                Dim OID As Long, Title As String, ThisSOCNbr As String

                SOCNbr.AcceptsTab = True

                '--- skip if pure test mode
                If (gTestMode) And (Not gWriteVotesToTest) Then
                    Exit Sub
                End If

                If NToB(Me.SOCNbr.Text) = "" Then
                    Exit Sub
                End If

                Me.SOCNbr.Text = Trim(UCase(Me.SOCNbr.Text))
                ThisSOCNbr = Me.SOCNbr.Text

                '--- check if input is document id
                docID = InStr(Me.SOCNbr.Text, "-")


                If Strings.Left(Me.SOCNbr.Text, 3) <> "SOC" Then
                    '--- Case 1: input is SR existing ALIS
                    If Strings.Left(Me.SOCNbr.Text, 2) = "SR" And docID = 0 Then
                        '--- first check for selected calendar exist or not; if not found, then go to ALIS;
                        str = "Select CalendarCode From tblCalendars Where CalendarCode='" & ThisSOCNbr & "'"
                        ds = V_DataSet(str, "R")

                        If ds.Tables(0).Rows.Count > 0 Then
                            DisplayMessage("You have already downloaded this special order calendar.", "Download Special Order Calendar From ALIS", "S")
                            ds.Dispose()
                            SOCNbr.Text = ""
                            SOCNbr.Focus()
                            Exit Sub
                            ' V_DataSet("Delete From tblCalendars Where ucase(CalendarCode) ='" & UCase(ThisSOCNbr) & "'", "D")
                        End If
                        ds.Dispose()

                        strSQL = "SELECT label, oid_current_document_version " & _
                                " FROM ALIS_OBJECT " & _
                                " WHERE (oid_session = " & gSessionID & ") AND (oid_house_of_origin = 1753) AND (status_code = '8') AND (instrument = 'T') " & _
                                " AND (last_transaction_date IS NOT NULL) AND EXISTS (SELECT 1 FROM Document_Section S , Special_Order_Calendar_Item SOC " & _
                                " WHERE SOC.OID_Resolution_Clause = S.OID AND OID_Current_Document_Version = S.OID_Document_Version) AND (label = '" & ThisSOCNbr & "')"
                        dsA = ALIS_DataSet(strSQL, "R")

                        If dsA.Tables(0).Rows.Count = 0 Then
                            RetValue = DisplayMessage("This special order calendar was not found in ALIS", "Calendar Not Found", "S")
                            dsA.Dispose()
                            Exit Sub
                        End If
                    ElseIf Strings.Left(Me.SOCNbr.Text, 2) <> "SR" And docID > 0 Then
                        '--- Case 2: input is Document ID existing ALIS
                        strSQL = " SELECT a.oid_current_document_version, a.oid, a.label, a.id,a.last_transaction_date , d.label " & _
                                  " FROM alis_object a, document_version d " & _
                                  " WHERE a.oid_session =" & gSessionID & " AND" & _
                                  " a.OID_CURRENT_DOCUMENT_VERSION  = d.oid AND" & _
                                  " d.label ='" & Replace(Trim(Me.SOCNbr.Text), " ", "") & "'"
                        dsDoc = ALIS_DataSet(strSQL, "R")

                        For Each drDoc In dsDoc.Tables(0).Rows
                            strSocLabel = drDoc("Label")

                            str = "Select CalendarCode From tblCalendars Where CalendarCode='" & strSocLabel & "'"
                            ds = V_DataSet(str, "R")

                            If ds.Tables(0).Rows.Count > 0 Then
                                DisplayMessage("You have already downloaded this special order calendar.", "Download Special Order Calendars From ALIS", "S")
                                ds.Dispose()
                                SOCNbr.Text = ""
                                SOCNbr.Focus()
                                Exit Sub

                            End If

                            strSQL = "SELECT label, oid_current_document_version " & _
                                    " FROM ALIS_OBJECT " & _
                                    " WHERE (oid_session = " & gSessionID & ") AND (oid_house_of_origin = 1753) AND (status_code = '8') AND (instrument = 'T') " & _
                                    " AND (last_transaction_date IS NOT NULL) AND EXISTS (SELECT 1 FROM Document_Section S , Special_Order_Calendar_Item SOC " & _
                                    " WHERE SOC.OID_Resolution_Clause = S.OID AND OID_Current_Document_Version = S.OID_Document_Version) AND (label = '" & strSocLabel & "')"
                            dsA = ALIS_DataSet(strSQL, "R")

                            If dsA.Tables(0).Rows.Count = 0 Then
                                RetValue = DisplayMessage("This special order calendar was not found in ALIS", "Calendar Not Found", "S")
                                dsA.Dispose()
                                Exit Sub
                            End If
                        Next
                    End If

                    '--- add this Calendar to tblCalendar
                    ReDim FieldValue(2)
                    For Each drA In dsA.Tables(0).Rows
                        OID = drA("OID_Current_Document_Version")
                        FieldValue(0) = drA("Label")
                        FieldValue(1) = drA("Label")
                        FieldValue(2) = Val(drA("OID_Current_Document_Version"))
                        V_DataSet("INSERT INTO tblCalendars  VALUES ('" & drA("Label") & "', '" & drA("Label") & "', " & drA("OID_Current_Document_Version") & ")", "A")
                        j += 1
                    Next
                    dsA.Dispose()

                    '--- get special order calendar bills from ALIS
                    ReDim FieldValue(8)
                    str = "SELECT A.label, A.sponsor, A.index_word, SOC.calendar_page " & _
                        " FROM SPECIAL_ORDER_CALENDAR_ITEM SOC, ALIS_OBJECT A, MATTER M " & _
                        " WHERE (OID_Resolution_Clause IN (SELECT S.OID FROM Document_Section S " & _
                        " WHERE S.OID_Document_Version =" & OID & " AND soc.oid_matter = m.oid AND m.oid_instrument = a.oid)) ORDER BY SOC.sequence_number"
                    j = 0
                    dsA = ALIS_DataSet(str, "R")

                    If docID = 0 Then
                        FieldValue(0) = ThisSOCNbr
                    Else
                        FieldValue(0) = strSocLabel
                    End If

                    For Each drA In dsA.Tables(0).Rows
                        If IsDBNull(drA("Label")) Then
                            FieldValue(1) = ""
                        Else
                            FieldValue(1) = drA("Label")
                        End If
                        If Strings.Left(FieldValue(1), 1) = "S" Then
                            Title = " by Senator "
                        Else
                            Title = " by Rep. "
                        End If
                        If IsDBNull(drA("Sponsor")) Then
                            FieldValue(3) = ""
                        Else
                            FieldValue(3) = drA("Sponsor")
                        End If
                        If IsDBNull(drA("Index_Word")) Then
                            FieldValue(4) = ""
                        Else
                            FieldValue(4) = NToB(Replace(drA("Index_Word"), "'", " "))
                        End If
                        If IsDBNull(FieldValue(5)) Or Not IsDBNull(FieldValue(5)) Then
                            FieldValue(5) = ""
                        End If
                        If IsDBNull(drA("Calendar_Page")) Then
                            FieldValue(6) = 0
                        Else
                            FieldValue(6) = drA("Calendar_Page")
                        End If

                        If FieldValue(6) <> 0 Then
                            FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4) & " p." & FieldValue(6)
                        Else
                            FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4)
                        End If

                        '--- added tow new fields
                        Dim strTitle As String = ""
                        If Strings.Left(FieldValue(1), 1) = "S" Then
                            strTitle = " by Senator "
                        Else
                            Title = " by Rep. "
                        End If

                        FieldValue(7) = strTitle & " " & FieldValue(3) & " - " & FieldValue(4)
                        If FieldValue(6) <> 0 Then
                            FieldValue(8) = FieldValue(1) & "&nbsp;&nbsp;&nbsp;&nbsp;" & " p." & FieldValue(6)
                        Else
                            FieldValue(7) = FieldValue(1)
                        End If

                        '--- insert SOC bills into local tblSpecialOrderCalendar table
                        str = "Insert Into tblSpecialOrderCalendar Values ('" & FieldValue(0) & "', '" & FieldValue(1) & "','" & FieldValue(2) & "','" & FieldValue(3) & "','" & FieldValue(4) & "','','" & FieldValue(6) & "', '" & Replace(FieldValue(7), "'", "") & "', '" & Replace(FieldValue(8), "'", "") & "')"
                        V_DataSet(str, "A")
                        j += 1
                    Next
                    dsA.Dispose()

                    '--- initialize calendar and bills
                    dsC = V_DataSet("Select Calendar, CalendarCode FROM tblCalendars ORDER BY CalendarCode ", "R")
                    Me.Calendar.Items.Clear()
                    For Each drC In dsC.Tables(0).Rows
                        Me.Calendar.Items.Add(drC("Calendar"))
                    Next
                    dsC.Dispose()

                    If docID = 0 Then
                        Me.Calendar.SelectedItem = ThisSOCNbr
                    Else
                        Me.Calendar.SelectedItem = strSocLabel
                    End If
                    Calendar_Click(sender, e)
                ElseIf Strings.Left(Me.SOCNbr.Text, 3) = "SOC" Then
                    '--- Case 3: find  "Special Oreder Calendar" is not existing in ALIS yet
                    Dim dsSP, dsSPBills As New DataSet
                    dsSP = V_DataSet("Select * From tblCalendars Where CalendarCode='SOC'", "R")

                    If dsSP.Tables(0).Rows.Count > 0 Then
                        For k As Integer = 0 To Me.Calendar.Items.Count - 1
                            If UCase(Me.Calendar.Items(k).ToString) = Trim(UCase("SPECIAL ORDER CALENDAR")) Then
                                Me.Calendar.SelectedIndex = k
                                Me.Calendar.SelectedItem = "Spcial Order Calendar"
                                Exit For
                            End If
                        Next

                        Me.Bill.Items.Clear()
                        dsSPBills = V_DataSet("Select Bill From tblSpecialOrderCalendar Where CalendarCode ='SOC'", "R")
                        For Each drSPBill As DataRow In dsSPBills.Tables(0).Rows
                            Me.Bill.Items.Add(drSPBill("Bill"))
                        Next
                    Else
                        DisplayMessage("Can not find special order calendar. Please open 'Special Order Calendar window to add.", "Load special order calendar", "S")
                        Me.SOCNbr.Text = ""
                        Exit Sub
                    End If
                End If
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Failed load special order calendar", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub btnVote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVote.Click
        ''--- put chamber display parameters in passed table so voting can pick them up; if bir then clear the work area so operator can
        ''--- start building next motion
        If Me.txtVoteID.Text = "" Then
            DisplayMessage("You must first get Vote ID to vote on.", "No Vote ID", "S")
            ' frmBuildVoteID.Show()
            Me.txtVoteID.Focus()
            Exit Sub
        End If
        If Me.Phrases.Text = "" And Me.CurrentBill.Text = "" Then
            RetValue = DisplayMessage("You must first select a bill and a motion to vote on.", "No Bill Or Motion Selected", "S")
            Exit Sub
        End If

        '--- in case user displayed a bill and then went to work on another before pressing the vote button
        If gBillNbrDisplayed <> gBillNbr Then
            btnRecallDisplayedBill_Click(gBillNbrDisplayed, New System.EventArgs())
            btnUpdateDisplay.Enabled = True
            Exit Sub
        Else
            btnUpdateDisplay.Enabled = False
        End If

        btnVote.Enabled = False
        btnVote.BackColor = Color.Red
        btnUnlock.Enabled = False

        '--- 1 send vote data to Vote Computer
        Dim BillText As String = ""
        If UCase(Me.OrderOfBusiness.Text) <> "ADJOURNMENT" And UCase(Me.OrderOfBusiness.Text) <> "CONVENE" And UCase(Me.OrderOfBusiness.Text) <> "SINE DIE" And UCase(Me.OrderOfBusiness.Text) <> "RECESS" Then
            Dim y As Integer = InStr(Bill.Text, " ")
            If y > 0 Then
                Select Case UCase(Trim(Calendar.Text))
                    Case "REGULAR ORDER"
                        BillText = Mid(Bill.Text, 1, InStr(Bill.Text, " ") - 1)
                    Case "LOCAL BILLS"
                        BillText = Mid(Bill.Text, 1, InStr(Bill.Text, " ") - 1)
                    Case "OTHER"
                        BillText = Mid(Bill.Text, 1, InStr(Bill.Text, " ") - 1)
                    Case "CONFIRMATIONS"
                        BillText = Mid(Bill.Text, 1, InStr(Bill.Text, "-") - 2)
                    Case "MOTIONS"
                        BillText = Me.CurrentBill.Text
                    Case UCase(Strings.Left(Calendar.Text, 2)) = "SR"
                        BillText = Mid(Bill.Text, 1, InStr(Bill.Text, " ") - 1)
                End Select
            Else
                BillText = Bill.Text
            End If
        Else
            BillText = Me.CurrentBill.Text
        End If


        If SVotePC_On Then
            ReceiveTimer.Enabled = True

            '--- pub chamber display parameer in passed table so voting can pick them up; if BIR then clear the word area so operator can
            '--- start building next motion
            askVoteID = False

            DisableControls()
            lblChamberLight.BackColor = Color.Red
            lblChamberLight.Text = "Waiting For Voting"
            ' lblChamberLight.Text = "Now Voting"
            gVotingStarted = False
            bodyParamters = ""

            If tPage0 Then
                If WorkData.Text = "" Then
                    gCurrentPhrase = ""
                Else
                    gCurrentPhrase = WorkData.Text
                End If
            ElseIf tPage1 Then
                If WorkData1.Text = "" Then
                    gCurrentPhrase = ""
                Else
                    gCurrentPhrase = WorkData1.Text
                End If
            ElseIf tPage2 Then
                If WorkData2.Text = "" Then
                    gCurrentPhrase = ""
                Else
                    gCurrentPhrase = WorkData2.Text
                End If
            End If

            If UCase(Me.Calendar.Text) <> "MOTIONS" Then
                bodyParamters = "gVoteID - " & Me.txtVoteID.Text & "||" & "gBillNbr - " & BillText & "||" & "gCurrentPhrase - " & gCurrentPhrase & "||" & "gBill - " & Me.CurrentBill.Text & "||" & "gCalendarCode - " & gCalendarCode
            Else
                bodyParamters = "gVoteID - " & Me.txtVoteID.Text & "||" & "gBillNbr - " & Me.CurrentBill.Text & "||" & "gCurrentPhrase - " & gCurrentPhrase & "||" & "gBill - " & Me.CurrentBill.Text & "||" & "gCalendarCode - " & gCalendarCode
            End If

            SendMessageToVotePCQueue("STARTVOTE", bodyParamters)
        Else
            ReceiveTimer.Enabled = False
            If DisplayMessage("Senate Vote Computer is offline. Would you want to continue?", "Senate Vote Computer is offline", "Y") Then
                EnableControls()
                btnVote.Enabled = False

                Me.lblChamberLight.BackColor = Color.LimeGreen
                Me.lblChamberLight.Text = "Display Next Bill"
                gPutChamberLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green)
                gPutVotingLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
                gVotingStarted = True
            End If
        End If

        gVotingStarted = False
        gPutParams = True
        gPutPhrase = gCurrentPhrase

        ' 2 --- Update LastVoteID to Production or Test mode
        If gTestMode Then
            strSQL = "Update tblVotingParameters Set ParameterValue='" & txtVoteID.Text & "' Where ucase(Parameter) = 'LASTVOTEIDFORTHISSESSIONTEST'"
            V_DataSet(strSQL, "U")
        Else
            strSQL = "Update tblVotingParameters Set ParameterValue='" & txtVoteID.Text & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSION'"
            V_DataSet(strSQL, "U")
        End If
        strSQL = "Update tblVotingParameters Set ParameterValue='" & gLegislativeDay & "' Where ucase(Parameter) ='LASTLEGISLATIVEDAY'"
        V_DataSet(strSQL, "U")


        ' 3 --- If display PC lost voting information, 
        '---write to text file first. Display PC is able to retreive voting infromation from the text file to response to Vote PC
        Dim txtContent As String = "VoteID=" & Me.txtVoteID.Text & ";" & "BillNbr=" & BillText & ";" & "Bill=" & Me.CurrentBill.Text & ";" & "LegislativeDay=" & gLegislativeDay & ";" & "WorkArea=" & Me.WorkData.Text & ";" & "Time=" & Now & vbNewLine
        If File.Exists(gVotingPath & "Vote.txt") = False Then
            Dim fs As New FileStream(gVotingPath & "Vote.txt", FileMode.CreateNew)
            fs.Close()
        End If
        FileOpen(1, gVotingPath & "Vote.txt", OpenMode.Append)
        Print(1, txtContent)
        FileClose(1)

        ' 4 --- Check is it on FreeFormat statu
        If gFreeFormat Then
            gPutCalendarCode = "FreeFormat"
            gPutCalendar = "FreeFormat"
            gPutBill = gBill
            gPutBillNbr = "FreeFormat"
        Else
            gPutCalendarCode = gCalendarCode
            gPutCalendar = gCalendar
            gPutBill = gBill
            gPutBillNbr = gBillNbr
        End If

        ' 5 ---write HTML page
        If gNetwork Then
            '---write to server and local
            CreateHTMLPage()
        Else
            '---write to local only
            CreateHTMLPage()
        End If
    End Sub

    Private Sub btnRecallDisplayedBill_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecallDisplayedBill.Click
        Dim ds, dsC As New DataSet

        If gBillDisplayed = "" Then
            RetValue = DisplayMessage("No bill has been displayed yet", "Nothing To Recall", "S")
        Else
            Me.Calendar.Text = gCalendarDisplayed
            Calendar_Click(gCalendarDisplayed, New System.EventArgs())
            Me.Bill.Text = gBillDisplayed
            Bill_Click(gBillDisplayed, New System.EventArgs())
        End If
    End Sub

    Private Sub btnClearDisplay_Click(sender As System.Object, e As System.EventArgs) Handles btnClearDisplay.Click
        Me.btnCreateHTMLPage.BackColor = Color.Silver
        gCalendarDisplayed = ""
        gCalendarCodeDisplayed = ""
        gBillDisplayed = ""
        gBillNbrDisplayed = ""
        Me.lblWorkAreaDisplayed.Visible = False
        Me.btnExit.Enabled = True
        Me.OrderOfBusiness.Enabled = True

        '--- send Clear message to OOB PC
        If SOOB_On Then
            SendStartOOBToOOB("", "START")
            'SendStartOOBToOOB("", "CLEAR DISPLAY")
        End If

        '--- Write a empty HTML page
        CreateOtherHTMLPage()
    End Sub

    Private Sub btnClearWorkArea_Click_1(sender As System.Object, e As System.EventArgs) Handles btnClearWorkArea.Click
        If tPage0 Then
            WorkData.Text = ""
        ElseIf tPage1 Then
            WorkData1.Text = ""
        ElseIf tPage2 Then
            WorkData2.Text = ""
        End If

        UpdateBillWorkData()

        UpdateWorkDataSW = False
    End Sub

    Private Sub btnCreateHTMLPage_Click(sender As System.Object, e As System.EventArgs) Handles btnCreateHTMLPage.Click
        '-- This button allow for the toggling on/off for the creation of the HTML
        '-- page display the current matter. If the button is green, the HTML page
        '-- will be created. If it is red, the page will not be create.

        If Me.btnCreateHTMLPage.BackColor = System.Drawing.Color.Green Then
            Me.btnCreateHTMLPage.BackColor = System.Drawing.Color.Red
            gCreateHTMLPage = False
            UpdateParameter("N", "CREATEHTMLPAGE")
        Else
            Me.btnCreateHTMLPage.BackColor = System.Drawing.Color.Green
            gCreateHTMLPage = True
            UpdateParameter("Y", "CREATEHTMLPAGE")
        End If
    End Sub

    Private Sub btnExit_Click1(sender As Object, e As System.EventArgs) Handles btnExit.Click
        If UpdateWorkDataSW Then
            UpdateBillWorkData()
        End If
        '--- clear all of messages on myself Queue
        Dim mq As New MessageQueue(gSendQueueFromDisplay)
        Dim msg As New Message
        mq.Purge()

        Me.Close()
        frmMain.Show()
    End Sub

    Private Sub btnUnlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnlock.Click
        Dim strM As String
        If SVotePC_On Then
            strM = "Are you sure want unlock without waiting voting?"
        Else
            strM = "Are you sure want unlock?"
        End If
        If DisplayMessage(strM, "Unlock Buttons", "Y") Then
            EnableControls()
            If SVotePC_On = False Then
                btnVote.Enabled = False
                btnCancelVote.Enabled = False
            End If
            Me.btnVote.BackColor = Color.LimeGreen
            Me.btnClearWorkArea.Enabled = True
            Me.btnClearDisplay.Enabled = True
            Me.OrderOfBusiness.Enabled = True  '?
            Me.lblChamberLight.BackColor = Color.LimeGreen
            Me.lblChamberLight.Text = "Display Next Bill"
        End If
    End Sub

    Private Sub EnableControls()
        Me.btnVote.Enabled = True
        Me.btnClearDisplay.Enabled = True
        Me.btnUpdateDisplay.Enabled = True
        Me.OrderOfBusiness.Enabled = True
        Me.btnRecallDisplayedBill.Enabled = True
        Me.btnExit.Enabled = True
        Me.btnClearDisplay.Enabled = True
        Me.btnUnlock.Enabled = True
        If SVotePC_On Then
            btnRequestLastVoteID.Enabled = True
        Else
            btnRequestLastVoteID.Enabled = False
        End If
    End Sub

    Private Sub DisableControls()
        Me.btnVote.Enabled = False
        Me.btnClearDisplay.Enabled = False
        Me.btnUpdateDisplay.Enabled = False
        Me.OrderOfBusiness.Enabled = False
        Me.btnRecallDisplayedBill.Enabled = False
        Me.btnExit.Enabled = False
        Me.btnClearDisplay.Enabled = False
    End Sub

    Private Function ShowBillByCalendar(strCalendar As String)
        Try
            If strCalendar <> Nothing Then
                strSQL = "SELECT tblBills.BillNbr, tblBills.Bill " & _
                " FROM tblBills, tblCalendars  " & _
                        " WHERE tblBills.CalendarCode = tblCalendars.CalendarCode AND " & _
                        " tblCalendars.Calendar = '" & strCalendar & "'" & _
                        " ORDER BY tblBills.BillNbr"
                ShowBillByCalendar = V_DataSet(strSQL, "R")
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Chamber Display - ShowBillByCalendar()", "S")
        End Try
    End Function

    Private Sub cboDisplay_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboDisplay.SelectedIndexChanged
        '*** send message to OOB PC, write HTML files
        If cboDisplay.Text = "WELCOME" Then
            If SOOB_On Then
                SendMessageToOOB("", "WELCOME")
            End If
            CreateWelcomeHTMLPage()
        ElseIf cboDisplay.Text = "ORDER OF BUSINESS" Then
            If SOOB_On Then
                SendMessageToOOB("", "ORDER OF BUSINESS")
            End If
            CreateOrderBusinessHTMLPage()
        End If

        '*** 2 copy HTML file to local
        CopyHTMLPageToLocal()
        Me.cboDisplay.SelectedIndex = 0
    End Sub


    Private Sub CreateOrderBusinessHTMLPage()
        '---replace * by current open session abbreviation
        ' gHTMLFile = Replace(gHTMLFile, "*", gSessionAbbrev)

        Try
            If FileExistes(gHTMLFile) = False Then
                File.Create(gHTMLFile)
            Else
                Kill(gHTMLFile)             '--make sure close the .htm file first
            End If

            FileOpen(1, gHTMLFile, OpenMode.Output)

            Print(1, "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
            Print(1, "<html xmlns='http://www.w3.org/1999/xhtml'>")
            Print(1, "<head id='Head1' runat='server'>")
            Print(1, "	<title>Alabama Senate Voting System</title>")
            Print(1, "<meta http-equiv='refresh' content='2'/>")
            Print(1, "<link rel='stylesheet' type='text/css' href='Styles/styleIE.css' />")
            Print(1, "</head>")
            Print(1, "<body class ='Body'>")
            Print(1, "<div class='header'>")
            Print(1, "<div class='headgraphic' >")
            Print(1, "</div> <!-- end div headgraphic -->")
            Print(1, "</div> <!-- end div header-->")
            Print(1, "<!-- Begin Insertion Point -->")
            Print(1, "<div class='order_of_business'>")
            Print(1, " <div class='order_of_business_heading'>")
            Print(1, "<p style='margin-top:20px;'>Order of Business </p>")
            Print(1, "</div>")
            Print(1, "</div>   ")
            Print(1, "<div class='page'>  ")
            Print(1, " <div class='content'>")
            Print(1, "<div class='content_text_center'>")
            Print(1, "<br>")
            Print(1, "Introduction of Bills")
            Print(1, "<br>")
            Print(1, "House Messages")
            Print(1, "<br>")
            Print(1, "Committee Reports")
            Print(1, "<br>")
            Print(1, "Motions and Resolutions")
            Print(1, "<br>")
            Print(1, "Local Bills")
            Print(1, "<br>")
            Print(1, "Bills on Third Reading")
            Print(1, "</div>")
            Print(1, "</div>      <!-- end div content -->")
            Print(1, " <br class='clearfix' />")
            Print(1, "</div> <!-- </div> end div page --> ")
            Print(1, "</body>")
            Print(1, "</html>")

            FileClose(1)
        Catch ex As Exception
            DisplayMessage("Failed Create HTML Page", "Create HTML Page", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub CreateClearHTMLPage()
        Dim wrkFld As String = ""

        Try
            If FileExistes(gHTMLFile) = False Then
                File.Create(gHTMLFile)
            Else
                Kill(gHTMLFile)             '--make sure close the .htm file first
            End If
            FileOpen(1, gHTMLFile, OpenMode.Output)

            Print(1, "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>")
            Print(1, "<html xmlns='http://www.w3.org/1999/xhtml'>")
            Print(1, "<head id='Head1' runat='server'>")
            Print(1, "<title>Alabama Senate Voting System</title>")
            Print(1, "<meta http-equiv='refresh' content='2'/>")
            Print(1, "<link rel='stylesheet' type='text/css' href='Styles/styleIE.css' />")
            Print(1, "</head>")
            Print(1, "<body class ='Body'>")
            Print(1, "<div class='header'>")
            Print(1, "<div class='headgraphic' >")
            Print(1, "</div> <!-- end div headgraphic -->")
            Print(1, "</div> <!-- end div header-->")
            Print(1, "<!-- Begin Insertion Point -->")
            Print(1, "<div class='order_of_business'>")
            Print(1, "<div class='order_of_business_heading'>")
            Print(1, "<p style='margin-top:20px;'></p>")
            Print(1, "</div>")
            Print(1, "</div>")
            Print(1, "<div class='page'>")
            Print(1, "<div class='content'>")
            Print(1, "<div class='content_recess'>")
            Print(1, "")
            Print(1, "</div>")
            Print(1, "</div>    <!-- end div content -->")
            Print(1, "<br class='clearfix' />")
            Print(1, "</div> <!-- </div> end div page -->")
            Print(1, "<!-- End Insertion Point-->")
            Print(1, "</body>")
            Print(1, "</html>")

            FileClose(1)
        Catch ex As Exception
            DisplayMessage(ex.Message, "Create HTML Page Failed!", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub btnStartService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartService.Click
        Dim p As New Process
        p = Process.Start(gCmdFile)
        p.WaitForExit()
    End Sub

#Region "Phrase and Senator drag-drop; click codes"

    Private Sub WorkData_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WorkData.TextChanged
        Dim WrkFld As String

        btnCreateHTMLPage.BackColor = Color.Silver
        lblWorkAreaDisplayed.Visible = False

        '--- always set current phrase to last line
        WrkFld = vbCrLf & Me.WorkData.Text                      ' add crlf since going from end to start and this is a flag and there may only be one phrase
        ' WrkFld = vbTab & vbTab & Me.WorkData.Text                      ' add crlf since going from end to start and this is a flag and there may only be one phrase

        If Strings.Right(WrkFld, 2) = vbCrLf Then
            WrkFld = Mid(WrkFld, 1, Len(WrkFld) - 2)
        End If
        If InStrRev(WrkFld, vbCrLf) > 0 Then
            gCurrentPhrase = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) + 2)
        Else
            gCurrentPhrase = Mid(WrkFld, 1)
        End If

        If InStr(Me.WorkData.Text, gBIR) = 1 Then
            gCurrentPhrase = "BIR - " & gCurrentPhrase
        End If

        UpdateBillWorkData()

        '--- do not update if free format since that would update the current bill with the free format text
        If Not gFreeFormat Then
            UpdateWorkDataSW = True
        End If
    End Sub


    Private Sub WorkData1_TextChanged(sender As System.Object, e As System.EventArgs) Handles WorkData1.TextChanged
        Dim WrkFld As String

        lblWorkAreaDisplayed.Visible = False
        btnCreateHTMLPage.BackColor = Color.Silver

        '--- always set current phrase to last line
        WrkFld = vbCrLf & Me.WorkData1.Text                      ' add crlf since going from end to start and this is a flag and there may only be one phrase
        ' WrkFld = vbTab & vbTab & Me.WorkData.Text                      ' add crlf since going from end to start and this is a flag and there may only be one phrase

        If Strings.Right(WrkFld, 2) = vbCrLf Then
            WrkFld = Mid(WrkFld, 1, Len(WrkFld) - 2)
        End If
        gCurrentPhrase = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) + 2)
        If InStr(Me.WorkData1.Text, gBIR) = 1 Then
            gCurrentPhrase = "BIR - " & gCurrentPhrase
        End If

        UpdateBillWorkData()

        '--- do not update if free format since that would update the current bill with the free format text
        If Not gFreeFormat Then
            UpdateWorkDataSW = True
        End If
    End Sub


    Private Sub WorkData2_TextChanged(sender As System.Object, e As System.EventArgs) Handles WorkData2.TextChanged
        Dim WrkFld As String

        lblWorkAreaDisplayed.Visible = False
        btnCreateHTMLPage.BackColor = Color.Silver

        '--- always set current phrase to last line
        WrkFld = vbCrLf & Me.WorkData2.Text                      ' add crlf since going from end to start and this is a flag and there may only be one phrase
        ' WrkFld = vbTab & vbTab & Me.WorkData.Text                      ' add crlf since going from end to start and this is a flag and there may only be one phrase

        If Strings.Right(WrkFld, 2) = vbCrLf Then
            WrkFld = Mid(WrkFld, 1, Len(WrkFld) - 2)
        End If
        gCurrentPhrase = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) + 2)
        If InStr(Me.WorkData2.Text, gBIR) = 1 Then
            gCurrentPhrase = "BIR - " & gCurrentPhrase
        End If

        UpdateBillWorkData()

        '--- do not update if free format since that would update the current bill with the free format text
        If Not gFreeFormat Then
            UpdateWorkDataSW = True
        End If
    End Sub

    Private Sub btnFullClear_Click(sender As System.Object, e As System.EventArgs) Handles btnFullClear.Click
        If SOOB_On Then
            SendStartOOBToOOB("", "CLEAR DISPLAY")
        End If
    End Sub

    Private Sub Reload_Bill(txtWorkData As String)
        Dim input As String = txtWorkData
        Dim phrase As String = vbCrLf
        Dim Occurrences As Integer = 0
        Dim ds, dsC As New DataSet

        If input <> "" Then
            Occurrences = (input.Length - input.Replace(phrase, String.Empty).Length) / phrase.Length
        End If

        If Occurrences = 1 Then
            txtWorkData = Replace(txtWorkData, vbCrLf, "")
        End If

        If Occurrences <> 0 Or (Occurrences = 0 And txtWorkData <> "") Then
            Select Case UCase(gCalendar)
                Case "REGULAR ORDER"
                    ' strSQL = "Select * From tblBills Where CalendarCode = '1' and workdata ='" & txtWorkData & "'"
                    strSQL = "Select * From tblBills Where CalendarCode = '1' "
                    If tPage0 Then
                        strSQL = strSQL & " And Bill ='" & work_bill & "'"
                    End If
                    If tPage1 Then
                        strSQL = strSQL & " And Bill ='" & work1_bill & "'"
                    End If
                    If tPage2 Then
                        strSQL = strSQL & " And Bill ='" & work2_bill & "'"
                    End If
                Case "LOCAL BILLS"
                    strSQL = "Select * From tblBills Where CalendarCode ='2' and workdata ='" & txtWorkData & "'"
                Case "CONFIRMATIONS"
                    strSQL = "Select * From tblBills Where CalendarCode ='3' and workdata ='" & txtWorkData & "'"
                Case "MOTIONS"
                    strSQL = "Select * From tblBills Where CalendarCode ='M' and workdata ='" & txtWorkData & "'"
                Case "OTHER"
                    strSQL = "Select * From tblBills Where CalendarCode ='ZZ' and workdata ='" & txtWorkData & "'"
            End Select
            'strSQL = "Select * From tblBills Where workdata ='" & txtWorkData & "'"
            ds = V_DataSet(strSQL, "R")
            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Me.CurrentBill.Text = dr("Bill")
                    If tPage0 Then
                        Me.WorkData.Text = NToB(dr("WorkData"))
                        work_bill = dr("Bill")
                    ElseIf tPage1 Then
                        Me.WorkData1.Text = NToB(dr("WorkData"))
                        work1_bill = dr("Bill")
                    ElseIf tPage2 Then
                        Me.WorkData2.Text = NToB(dr("WorkData"))
                        work2_bill = dr("Bill")
                    End If

                    '--- fouce on right bill
                    Me.Bill.SelectedItem = dr("Bill")

                    '--- fouce on right calendar
                    strSQL = "Select * From tblCalendars Where CalendarCode ='" & dr("CalendarCode") & "'"
                    dsC = V_DataSet(strSQL, "R")
                    For Each drC As DataRow In dsC.Tables(0).Rows
                        Me.Calendar.SelectedItem = drC("Calendar")
                        Exit For
                    Next

                    Exit For
                Next
            End If
        End If
    End Sub

    Private Sub TC_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles TC.SelectedIndexChanged
        Select Case TC.SelectedIndex
            Case 0
                tPage0 = True
                tPage1 = False
                tPage2 = False
                WorkData.Focus()
                If work_bill <> "" Then
                    Reload_Bill(WorkData.Text)
                End If
            Case 1
                tPage0 = False
                tPage1 = True
                tPage2 = False
                WorkData1.Focus()
                If work1_bill <> "" Then
                    Reload_Bill(WorkData1.Text)
                End If
            Case 2
                tPage0 = False
                tPage1 = False
                tPage2 = True
                WorkData2.Focus()

                If work2_bill <> "" Then
                    Reload_Bill(WorkData2.Text)
                End If
        End Select
    End Sub
#End Region


#Region "old codes"

    'Private Sub PingTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PingTimer.Tick
    '    ' ''--- if Votie PC is online, skip ping
    '    ''If (SVotePC_On And SOOB_On) Or gOnlyOnePC = False Then
    '    ''    Exit Sub
    '    ''Else
    '    'PingComputers()
    '    '' End If

    '    ''--- not here to call it:   Get_Parameters()
    '    Dim RetValue As New Object
    '    Dim Ping As Ping
    '    Dim pReply As PingReply
    '    Dim mes As String = "Ping Computers"
    '    Static intTime As Long

    '    intTime += 1
    '    Ping = New Ping
    '    SDisplay_On = True

    '    If intTime > 36000 Then
    '        If SVotePC_On = False Then
    '            Try
    '                '-- ping Voting PC 
    '                pReply = Ping.Send(gVotePC_IPAddress)
    '                If pReply.Status = IPStatus.Success Then
    '                    SVotePC_On = True
    '                    gOnlyOnePC = False
    '                    ReceiveTimer.Enabled = True
    '                    ReceiveTimer.Interval = gReceiveQueueTimer
    '                Else
    '                    ReceiveTimer.Enabled = False
    '                    SVotePC_On = False
    '                    gOnlyOnePC = True
    '                    V_DataSet("Update tblOnlyOnePC set OnlyOnePCName='" & SDIS & "', OnlyOnePC = 1", "U")

    '                    If Not DisplayMessage("Senate Vote Computer is offline. Would you want continue without voting?", "Senate Vote PC is Offline", "Y") Then
    '                        '--- before shotdown update work area value to database
    '                        UpdateBillWorkData()
    '                        End
    '                    Else
    '                        EnableControls()
    '                        btnVote.Enabled = False
    '                        ReceiveTimer.Enabled = False
    '                        ReceiveTimer.Stop()
    '                        lblChamberLight.BackColor = Color.LimeGreen
    '                        lblChamberLight.Text = "Display Next Bill"
    '                        RetValue = DisplayMessage("Senate Voting Computer if offline. Unable open machine to vote! Would you want to continue?", mes, "I")
    '                    End If
    '                End If

    '            Catch
    '                SVotePC_On = False
    '            End Try
    '        End If
    '    End If
    'End Sub

    'Private Sub btnDropLastPhrase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDropLastPhrase.Click
    '    If tPage0 Then
    '        If Strings.Right(Me.WorkData.Text, 2) = vbCrLf Then
    '            Me.WorkData.Text = Mid(Me.WorkData.Text, 1, Len(Me.WorkData.Text) - 2)
    '        End If
    '        If InStrRev(Me.WorkData.Text, vbCrLf) = 0 Then                      ' dropping last phrase, or first since we're going backwards
    '            Me.WorkData.Text = ""
    '        Else
    '            Me.WorkData.Text = Mid(Me.WorkData.Text, 1, InStrRev(Me.WorkData.Text, vbCrLf) + 1)
    '        End If
    '    ElseIf tPage1 Then
    '        If Strings.Right(Me.WorkData1.Text, 2) = vbCrLf Then
    '            Me.WorkData1.Text = Mid(Me.WorkData1.Text, 1, Len(Me.WorkData1.Text) - 2)
    '        End If
    '        If InStrRev(Me.WorkData1.Text, vbCrLf) = 0 Then                      ' dropping last phrase, or first since we're going backwards
    '            Me.WorkData1.Text = ""
    '        Else
    '            Me.WorkData1.Text = Mid(Me.WorkData1.Text, 1, InStrRev(Me.WorkData1.Text, vbCrLf) + 1)
    '        End If
    '    ElseIf tPage2 Then
    '        If Strings.Right(Me.WorkData2.Text, 2) = vbCrLf Then
    '            Me.WorkData2.Text = Mid(Me.WorkData2.Text, 1, Len(Me.WorkData2.Text) - 2)
    '        End If
    '        If InStrRev(Me.WorkData2.Text, vbCrLf) = 0 Then                      ' dropping last phrase, or first since we're going backwards
    '            Me.WorkData2.Text = ""
    '        Else
    '            Me.WorkData2.Text = Mid(Me.WorkData2.Text, 1, InStrRev(Me.WorkData2.Text, vbCrLf) + 1)
    '        End If
    '    End If
    'End Sub

    'Private Sub Bill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Bill.Click
    '    Dim fnt As Font
    '    Dim strBill As String
    '    gBill = ""
    '    If tPage0 Then
    '        fnt = Me.WorkData.Font
    '        Me.WorkData.Font = New Font(fnt.Name, 11, FontStyle.Regular)
    '        Me.WorkData.Enabled = True
    '    ElseIf tPage1 Then
    '        fnt = Me.WorkData1.Font
    '        Me.WorkData1.Font = New Font(fnt.Name, 11, FontStyle.Regular)
    '        Me.WorkData1.Enabled = True
    '    ElseIf tPage2 Then
    '        fnt = Me.WorkData2.Font
    '        Me.WorkData2.Font = New Font(fnt.Name, 11, FontStyle.Regular)
    '        Me.WorkData2.Enabled = True
    '    End If

    '    btnCreateHTMLPage.BackColor = Color.Silver
    '    Me.CurrentBill.Enabled = True

    '    If (ComingFrom = "NextBill") Or (ComingFrom = "PreviousBill") Then
    '        ComingFrom = ""
    '    ElseIf UpdateWorkDataSW Then
    '        UpdateBillWorkData()
    '    End If

    '    '--- bookmark so we know where we are in the list when using next/previous buttons
    '    'If NToB(Bill.Text) <> "" And (gCalendarCode <> "M") Then
    '    If Me.Bill.Text <> "" And (gCalendarCode <> "M") Then
    '        CurrentBill.Text = Bill.SelectedItem.ToString
    '        gBill = Bill.Text
    '        Me.CurrentBill.Text = Me.Bill.Text
    '        BillHasChanged()
    '    End If
    '    gBill = Bill.SelectedItem.ToString
    '    strBill = Bill.SelectedItem.ToString
    '    '--- bookmark so we know where we are in the list when using next/previous buttons
    '    If NToB(Bill.Text) <> "" And (gCalendarCode = "M") Then
    '        ' CurrentBill.Text = Bill.SelectedItem.ToString
    '        Dim i As Integer = 0
    '        If InStr(UCase(Me.Bill.Text), "RECESS") > 0 And InStr(Bill.Text, gSenatorInsertionPoint) > 0 Then
    '            ' If (InStr(Bill.Text, gSenatorInsertionPoint) = 0) Then
    '            Me.CurrentBill.Text = "RECESS"
    '            'Else
    '            '    Me.CurrentBill.Text = "^ RECESS"
    '            'End If
    '        End If
    '        If InStr(UCase(Me.Bill.Text), "ROLLCALL") > 0 Then
    '            ' If (InStr(Bill.Text, gSenatorInsertionPoint) = 0) Then
    '            Me.CurrentBill.Text = "ROLLCALL"
    '            'Else
    '            '    Me.CurrentBill.Text = "^ ROLLCALL"
    '            'End If

    '        End If
    '        If InStr(UCase(Me.Bill.Text), "ROLL CALL") > 0 Then
    '            ' If (InStr(Bill.Text, gSenatorInsertionPoint) = 0) Then
    '            Me.CurrentBill.Text = "ROLL CALL"
    '            'Else
    '            '    Me.CurrentBill.Text = "^ ROLLCALL"
    '            'End If

    '        End If
    '        If InStr(UCase(Me.Bill.Text), "ADJOURNMENT") > 0 Then
    '            ' If (InStr(Bill.Text, gSenatorInsertionPoint) = 0) Then
    '            Me.CurrentBill.Text = "ADJOURNMENT"
    '            'Else
    '            '    Me.CurrentBill.Text = "^ ADJOURNMENT"
    '            'End If

    '        End If
    '        If InStr(UCase(Me.Bill.Text), "ADJOURN") > 0 Then
    '            'If (InStr(Bill.Text, gSenatorInsertionPoint) = 0) Then
    '            Me.CurrentBill.Text = "ADJOURN"
    '            'Else
    '            '    Me.CurrentBill.Text = "^ ADJOURN"
    '            'End If

    '        End If
    '        If InStr(UCase(Me.Bill.Text), "MOTION TO") > 0 Then
    '            'If (InStr(Bill.Text, gSenatorInsertionPoint) > 0) Then
    '            '    Me.CurrentBill.Text = "^ " & Mid(UCase(Me.Bill.Text), InStr(UCase(Me.Bill.Text), "MOTION TO") + 10)
    '            'Else
    '            Me.CurrentBill.Text = Mid(UCase(Me.Bill.Text), InStr(UCase(Me.Bill.Text), "MOTION TO") + 10)
    '            'End If

    '        End If

    '        '---If no motion calendar and there is a senator insertion point in this motion, 
    '        '---then pull down the senator list box
    '        If (gCalendarCode = "M") And (InStr(Bill.Text, gSenatorInsertionPoint) > 0) Then

    '            Me.Senators.Focus()
    '            SendKeys.Send("{F4}")
    '            If tPage0 Then
    '                Me.WorkData.Text = gBill
    '            ElseIf tPage1 Then
    '                Me.WorkData1.Text = gBill
    '            ElseIf tPage2 Then
    '                Me.WorkData2.Text = gBill
    '            End If
    '        End If
    '        gBill = Me.CurrentBill.Text
    '        BillHasChanged()
    '        If InStr(UCase(Me.Bill.Text), "SR ?") > 0 Then
    '            Me.CurrentBill.Text = "SR ?"

    '        End If
    '        If InStr(UCase(Me.Bill.Text), "SJR ?") > 0 Then
    '            Me.CurrentBill.Text = "SJR ?"
    '        End If
    '        If InStr(UCase(Me.Bill.Text), "HR ?") > 0 Then
    '            Me.CurrentBill.Text = "HR ?"
    '        End If
    '        If InStr(UCase(Me.Bill.Text), "HJR ?") > 0 Then
    '            Me.CurrentBill.Text = "HJR ?"
    '        End If
    '        If InStr(UCase(Me.Bill.Text), "SR ?") > 0 Or InStr(UCase(Me.Bill.Text), "SJR ?") > 0 Or InStr(UCase(Me.Bill.Text), "HR ?") > 0 Or InStr(UCase(Me.Bill.Text), "HJR ?") > 0 Then

    '            If tPage0 Then
    '                Me.WorkData.Text = strBill
    '            ElseIf tPage1 Then
    '                Me.WorkData1.Text = strBill
    '            ElseIf tPage2 Then
    '                Me.WorkData2.Text = strBill
    '            End If
    '        End If


    '        'gBill = Me.CurrentBill.Text
    '        ' BillHasChanged()


    '    Else
    '        If (gCalendarCode <> "M") And (InStr(Bill.Text, gSenatorInsertionPoint) > 0) Then
    '            Me.Senators.Focus()
    '            SendKeys.Send("{F4}")
    '            Me.CurrentBill.Text = gBill
    '        End If
    '    End If
    '    '  gCalendarCode = gCalendarCode
    '    ''---If no motion calendar and there is a senator insertion point in this motion, 
    '    ''---then pull down the senator list box
    '    'If (gCalendarCode = "M") And (InStr(Bill.Text, gSenatorInsertionPoint) > 0) Then

    '    '    Me.Senators.Focus()
    '    '    SendKeys.Send("{F4}")
    '    '    If tPage0 Then
    '    '        Me.WorkData.Text = gBill
    '    '    ElseIf tPage1 Then
    '    '        Me.WorkData1.Text = gBill
    '    '    ElseIf tPage2 Then
    '    '        Me.WorkData2.Text = gBill
    '    '    End If
    '    'End If

    '    ''---If no motion calendar and there is a senator insertion point in this motion, 
    '    ''---then pull down the senator list box
    '    'If (gCalendarCode = "M") And (InStr(Bill.Text, gSenatorInsertionPoint) > 0) Then
    '    '    Me.CurrentBill.Text = gBill
    '    '    Me.Senators.Focus()
    '    '    SendKeys.Send("{F4}")
    '    '    If tPage0 Then
    '    '        Me.WorkData.Text = ""
    '    '    ElseIf tPage1 Then
    '    '        Me.WorkData1.Text = ""
    '    '    ElseIf tPage2 Then
    '    '        Me.WorkData2.Text = ""
    '    '    End If
    '    'End If
    'End Sub


    'Private Sub WorkData_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles WorkData.DragEnter
    '    ' ----- Yes, we accept the items.
    '    If (e.Data.GetDataPresent(frmPhraseShow.lstPhrase.SelectedItems.GetType()) = True) Then e.Effect = DragDropEffects.Move
    'End Sub

    'Private Sub WorkData_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles WorkData.DragDrop
    '    ' ----- Accept the dropped items.
    '    For Each oneItem As Object In e.Data.GetData(frmPhraseShow.lstPhrase.SelectedItems.GetType())
    '        If bPhrase Then
    '            If WorkData.Text = "" Then
    '                WorkData.Text = Mid(oneItem, InStr(oneItem, " - ") + 3)
    '            Else
    '                If Strings.Right(WorkData.Text, 2) <> vbCrLf Then
    '                    Me.WorkData.Text = Me.WorkData.Text & vbCrLf & Mid(oneItem, InStr(oneItem, " - ") + 3)
    '                Else
    '                    Me.WorkData.Text = Me.WorkData.Text & Mid(oneItem, InStr(oneItem, " - ") + 3)
    '                End If
    '            End If
    '        ElseIf bSenator Then
    '            InsertIntoWorkData("Senator" & Me.Senators.Text, gSenatorInsertionPoint)
    '        End If
    '    Next oneItem
    'End Sub

    'Private Sub WorkData1_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles WorkData1.DragEnter
    '    ' ----- Yes, we accept the items.
    '    If (e.Data.GetDataPresent(frmPhraseShow.lstPhrase.SelectedItems.GetType()) = True) Then e.Effect = DragDropEffects.Move
    'End Sub

    'Private Sub WorkData1_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles WorkData1.DragDrop
    '    ' ----- Accept the dropped items.
    '    For Each oneItem As Object In e.Data.GetData(frmPhraseShow.lstPhrase.SelectedItems.GetType())
    '        If bPhrase Then
    '            If WorkData1.Text = "" Then
    '                WorkData1.Text = Mid(oneItem, InStr(oneItem, " - ") + 3)
    '            Else
    '                If Strings.Right(WorkData1.Text, 2) <> vbCrLf Then
    '                    Me.WorkData1.Text = Me.WorkData1.Text & vbCrLf & Mid(oneItem, InStr(oneItem, " - ") + 3)
    '                Else
    '                    Me.WorkData1.Text = Me.WorkData1.Text & Mid(oneItem, InStr(oneItem, " - ") + 3)
    '                End If
    '            End If
    '        ElseIf bSenator Then
    '            InsertIntoWorkData("Senator" & Me.Senators.Text, gSenatorInsertionPoint)
    '        End If
    '    Next oneItem
    'End Sub

    'Private Sub WorkData2_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles WorkData2.DragEnter
    '    ' ----- Yes, we accept the items.
    '    If (e.Data.GetDataPresent(frmPhraseShow.lstPhrase.SelectedItems.GetType()) = True) Then e.Effect = DragDropEffects.Move
    'End Sub

    'Private Sub WorkData2_DragDrop(sender As Object, e As System.Windows.Forms.DragEventArgs) Handles WorkData2.DragDrop
    '    ' ----- Accept the dropped items.
    '    For Each oneItem As Object In e.Data.GetData(frmPhraseShow.lstPhrase.SelectedItems.GetType())
    '        If bPhrase Then
    '            If WorkData2.Text = "" Then
    '                WorkData2.Text = Mid(oneItem, InStr(oneItem, " - ") + 3)
    '            Else
    '                If Strings.Right(WorkData2.Text, 2) <> vbCrLf Then
    '                    Me.WorkData2.Text = Me.WorkData2.Text & vbCrLf & Mid(oneItem, InStr(oneItem, " - ") + 3)
    '                Else
    '                    Me.WorkData2.Text = Me.WorkData2.Text & Mid(oneItem, InStr(oneItem, " - ") + 3)
    '                End If
    '            End If
    '        ElseIf bSenator Then
    '            InsertIntoWorkData("Senator" & Me.Senators.Text, gSenatorInsertionPoint)
    '        End If
    '    Next oneItem
    'End Sub

    'Private Sub btnPing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartService.Click
    '    Dim Ping As Ping
    '    Dim pReply As PingReply

    '    SDisplay_On = True

    '    '  If SVotePC_On = False Then
    '    Try

    '        Ping = New Ping
    '        pReply = Ping.Send(gVotePC_IPAddress)
    '        If pReply.Status = IPStatus.Success Then
    '            SVotePC_On = True
    '            gOnlyOnePC = False
    '            DisplayMessage("Senate Vote Computer is online. System is back to normal process", "Vote PC is Online", "S")
    '            Exit Sub
    '        Else
    '            SVotePC_On = False
    '            gOnlyOnePC = True

    '            If DisplayMessage("Senate Vote Computer is still offline. Would you want to continue to work without vote?", "Vote PC is Offline", "Y") Then
    '                LockReadOnly = False
    '            Else
    '                '--- before shotdown update work area value to database
    '                '!!!! do I need??? - UpdateLastVoteIDAndLastLegDay(Me.CurrentVoteID.Text)
    '                End
    '            End If
    '        End If
    '    Catch ex As Exception
    '        If DisplayMessage(ex.Message & vbNewLine & "Would you want to continue work without vote?", "Vote PC is Offline", "Y") Then
    '            Exit Sub
    '        Else
    '            End
    '        End If
    '    End Try
    '    '  End If
    'End Sub

    'Private Sub btnSRSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSRSave.Click
    '    Dim ds As New DataSet
    '    Dim NewSRName As String = UCase(Me.txtSRChange.Text)

    '    If strOrigSRName <> Me.Calendar.Text Then
    '        V_DataSet("Update tblCalendars Set CalendarCode ='" & NewSRName & "', Calendar ='" & NewSRName & "', 0 ", "U")
    '        V_DataSet("Update tblSpecialOrderCalendar Set CalendarCode ='" & NewSRName & "' Where CalendarCode ='" & strOrigSRName & "'", "U")
    '    End If
    'End Sub

    'Private Sub SendMessageToDisplay(ByVal label As String, ByVal strBody As String, ByVal QueueName As String)
    '    Try
    '        Dim mq As New MessageQueue
    '        Dim msg As New Message
    '        Dim ds As New DataSet

    '        'mq.Path = gSendQueueToDisplay
    '        mq.Path = QueueName

    '        'queueName = ".\PRIVATE\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE\SenateVoteQueue"
    '        msg.Priority = MessagePriority.Normal
    '        msg.Label = label
    '        msg.Body = strBody
    '        mq.Send(msg)

    '        DisplayImage = False

    '        mq.Close()
    '    Catch ex As Exception
    '        DisplayMessage(ex.Message & vbCrLf & "Error Occurred When Sending Vote Message", "Sending Vote Message", "S")
    '    End Try

    'End Sub

    'Private Sub CreateHTMLPage()
    '    '--- print the doc in HTML and put on the web server so all can see - done everytime the chamber
    '    '--- display operator hits update display
    '    Dim ds As New DataSet
    '    Dim dr As DataRow
    '    Dim WrkFld As String = ""

    '    Try
    '        '---replace * by current open session abbreviation
    '        gHTMLFile = Replace(gHTMLFile, "*", gSessionAbbrev)

    '        Kill(gHTMLFile)             '--make sure close the .htm file first

    '        strSQL = "Select * From tblOrderOfBusiness"
    '        ds = V_DataSet(strSQL, "R")

    '        FileOpen(1, gHTMLFile, OpenMode.Output)
    '        Print(1, "<HTML>")
    '        Print(1, "<meta http-equiv='refresh' content='4'/>")
    '        Print(1, "<Body><p>&nbsp;</p> ")

    '        Print(1, "<table align='center' width=100%>")
    '        Print(1, "<tr align='center'><td></td>")
    '        Print(1, "<td align='center'>")

    '        Print(1, "<H1><Center><B><U><FONT FACE=" & Chr(34) & "Arial" & Chr(34) & " SIZE=" & Chr(34) & "4" & Chr(34) & " COLOR=" & Chr(34) & "Black" & Chr(34) & ">Order Of Business</U></B><BR><BR>")
    '        For Each dr In ds.Tables(0).Rows
    '            If dr("OrderOfBusiness") = Me.CurrentOrderOfBusiness.Text Then
    '                Print(1, "<Font Color=" & Chr(34) & "Red" & Chr(34) & ">" & Me.CurrentOrderOfBusiness.Text & "</Font><BR>")
    '            Else
    '                Print(1, dr("OrderOfBusiness") & "<BR>")
    '            End If
    '        Next
    '        Print(1, "</td></tr></table>")

    '        If ClearDisplayFlag = False Then
    '            Print(1, "</Center></H1>-------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
    '            Print(1, "<BR>")
    '            Print(1, "<Font size='4' face='Arial' Color=" & Chr(34) & "Blue" & Chr(34) & "><b>" & Me.CurrentBill.Text & "</b></Font><BR>")
    '            Print(1, "<BR>-------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
    '            Print(1, "<BR><font size='3' face='Arial' ><b>")
    '            If tPage0 Then
    '                WrkFld = Me.WorkData.Text
    '            ElseIf tPage1 Then
    '                WrkFld = Me.WorkData1.Text
    '            ElseIf tPage2 Then
    '                WrkFld = Me.WorkData2.Text
    '            End If

    '            If Strings.Right(WrkFld, 2) <> vbCrLf Then
    '                WrkFld = WrkFld & vbCrLf
    '            End If
    '            Do
    '                Print(1, Mid(WrkFld, 1, InStr(WrkFld, vbCrLf) - 1))
    '                Print(1, "<BR>")
    '                If InStr(WrkFld, vbCrLf) = Len(WrkFld) - 1 Then
    '                    Exit Do
    '                End If
    '                WrkFld = Mid(WrkFld, InStr(WrkFld, vbCrLf) + 2)
    '            Loop
    '            Print(1, "</b></font><BR><BR><BR>")
    '            Print(1, "<BR><BR><BR>")
    '            Print(1, "<Font Size=" & Chr(34) & "2" & Chr(34) & " face='Arial'> Current Date/Time:  " & Now & "  " & Now.ToShortTimeString & "</Font>")
    '            Print(1, "</Body>")
    '            Print(1, "</HTML>")
    '        Else
    '            Print(1, "</Center></H1>------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
    '            Print(1, "<BR>")
    '            Print(1, "<Font Color=" & Chr(34) & "Blue" & Chr(34) & ">" & "" & "</Font><BR>")
    '            Print(1, "<BR>----------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
    '            Print(1, "<BR>")
    '            WrkFld = ""

    '            Print(1, WrkFld)
    '            Print(1, "<BR>")
    '            Print(1, "<BR><BR><BR>")
    '            Print(1, "<BR><BR><BR>")
    '            Print(1, "<Font Size=" & Chr(34) & "2" & Chr(34) & " face='Arial'> Current Date/Time:  " & Now & "  " & Now.ToShortTimeString & "</Font>")
    '            Print(1, "</Body>")
    '            Print(1, "</HTML>")
    '            ClearDisplayFlag = False
    '        End If
    '        FileClose(1)

    '    Catch ex As Exception
    '        'If ex.Source = 76 Then
    '        '    RetValue = DisplayMessage("The folder for this session " & gHTMLFile & " needs to be created before an HTML " & _
    '        '       "can be written.  Please create the folder.", "Missing Folder For HTML Document", "S")
    '        '    Exit Sub
    '        'End If
    '        DisplayMessage(ex.Message, "Create HTML Page", "S")

    '    End Try
    'End Sub



    'If Not IsNothing(frmPhraseShow) Then
    '    If Not frmPhraseShow.IsDisposed Then
    '        frmPhraseShow.WindowState = FormWindowState.Normal
    '        frmPhraseShow.BringToFront()
    '        frmPhraseShow.Show()
    '    Else
    '        frmPhraseShow = New frmPhraseShow
    '        frmPhraseShow.Show()
    '    End If
    'Else
    '    frmPhraseShow = New frmPhraseShow
    '    frmPhraseShow.Show()
    'End If

    'If Not IsNothing(frmSenatorShow) Then
    '    If Not frmSenatorShow.IsDisposed Then
    '        frmSenatorShow.WindowState = FormWindowState.Normal
    '        frmSenatorShow.BringToFront()
    '        frmSenatorShow.Show()
    '    Else
    '        frmSenatorShow = New frmSenatorShow
    '        frmSenatorShow.Show()
    '    End If
    'Else
    '    frmSenatorShow = New frmSenatorShow
    '    frmSenatorShow.Show()
    'End If
    'Private Sub btnUpdateDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateDisplay.Click
    ''1--- Make sure current bill was selected
    '    If Me.CurrentBill.Text = "" Then
    '        RetValue = DisplayMessage("Please select a bill to display", "No Bill Selected", "S")
    '        Exit Sub
    '    End If

    ''*** 2 write work data to tblBills and update tblWork WorkData field
    '    UpdateBillWorkData()

    ''*** 3 create HTML page ***'
    '    gCreateHTMLPage = True
    '    CreateHTMLPage()

    ''--- send this work area to the projection display
    'Dim i, j As Integer
    'Dim bodyParamters As String

    'If Me.CurrentBill.Text = "" Then
    '    RetValue = DisplayMessage("Please select a bill to display", "No Bill Selected", "S")
    '    Exit Sub
    'End If

    ''-- if confirmation then triple the height of the bill display board and take away from the phrase board

    'If Strings.Left(Me.CurrentBill.Text, 2) = "CF" Then
    '    If Pfrm.BillDisplayBoard.Height <> 3 * gSaveBillHeight Then
    '        Pfrm.BillDisplayBoard.Height = 3 * gSaveBillHeight
    '        Pfrm.PhraseDisplayBoard.Height = Pfrm.PhraseDisplayBoard.Height - gSaveBillHeight
    '        Pfrm.PhraseDisplayBoard.Top = Pfrm.BillDisplayBoard.Top + (3 * gSaveBillHeight)
    '    End If
    '    If gFreeFormat Then   ' can have free format confirmations
    '        Pfrm.BillDisplayBoard.Text = Me.CurrentBill.Text
    '    Else
    '        Pfrm.BillDisplayBoard.Text = Mid(Me.CurrentBill.Text, 1, InStr(Me.CurrentBill.Text, " -- ") - 1) & vbCrLf & Mid(Me.CurrentBill.Text, InStr(Me.CurrentBill.Text, " -- ") + 4)
    '    End If
    '    gStart = 0
    '    gLength = Len(Pfrm.BillDisplayBoard.Text)
    'Else
    '    ResetDisplayHeight()

    '    ' first do subject - not for CFs; if a calendar page follows then subject
    '    ' then do it with the bill attributes

    '    Pfrm.BillDisplayBoard.Text = ""
    '    i = 0
    '    If gSubject <> "" Then
    '        i = InStr(gBill, " - " & gSubject)
    '        j = InStr(gBill, " p.")  ' calendar page at end of line
    '        If j > i Then ' page found
    '            Pfrm.BillDisplayBoard.Text = Mid(gBill, j)
    '            gStart = 0
    '            gLength = Len(Mid(gBill, j))
    '            ApplySpecificAttributesToDisplayBoard("Bill", gBillAttributes)
    '        End If

    '        Pfrm.BillDisplayBoard.Text = "- " & gSubject & Pfrm.BillDisplayBoard.Text
    '        gStart = 0
    '        gLength = Len(gSubject) + 2  'allow for the dash and space
    '        ApplySpecificAttributesToDisplayBoard("Bill", gSubjectAttributes)
    '    End If

    '    ' now do senator and put in front of subject - makes colors work by building line from end to start

    '    gStart = 0
    '    If i > 0 Then
    '        gLength = Len(Mid(gBill, 1, i - 1)) ' pick up to the - separating bill and subject
    '        Pfrm.BillDisplayBoard.Text = Mid(gBill, 1, i) & Pfrm.BillDisplayBoard.Text
    '    Else
    '        gLength = Len(gBill)
    '        Pfrm.BillDisplayBoard.Text = gBill
    '    End If
    'End If ' not CFs

    'ApplySpecificAttributesToDisplayBoard("Bill", gBillAttributes)
    'gCalendarCodeDisplayed = gCalendarCode
    'gBillNbrDisplayed = gBillNbr
    'gCalendarDisplayed = gCalendar
    'gBillDisplayed = gBill

    'DisplayPhraseWorkArea()

    'Me.OrderOfBusiness.Enabled = False
    'Me.btnExit.Enabled = False
    'Me.lblWorkAreaDisplayed.Visible = True

    'CreateHTMLPage()

    'Try
    '    '---New codes
    '    If SDisplay_On Then
    '        gCalendarCodeDisplayed = gCalendarCode
    '        gBillNbrDisplayed = gBillNbr
    '        gCalendarDisplayed = gCalendar
    '        gBillDisplayed = gBill

    '        Me.OrderOfBusiness.Enabled = False
    '        Me.btnExit.Enabled = False
    '        Me.lblWorkAreaDisplayed.Visible = True


    '        '---send voting information to display PC message queue
    '        bodyParamters = ""
    '        If tPage0 Then
    '            bodyParamters = "gBill - " & Me.Bill.Text & "||" & "gWorkArea - " & Me.WorkData.Text & "||" & "gCurrentOrderOfBusiness - " & CurrentOrderOfBusiness.Text
    '        ElseIf tPage1 Then
    '            bodyParamters = "gBill - " & Me.Bill.Text & "||" & "gWorkArea - " & Me.WorkData1.Text & "||" & "gCurrentOrderOfBusiness - " & CurrentOrderOfBusiness.Text
    '        ElseIf tPage2 Then
    '            bodyParamters = "gBill - " & Me.Bill.Text & "||" & "gWorkArea - " & Me.WorkData2.Text & "||" & "gCurrentOrderOfBusiness - " & CurrentOrderOfBusiness.Text
    '        End If

    '        DisplayImage = True
    '        If SOOB_On Then
    '            SendMessageToDisplay(UCase(cboDisplay.Text), bodyParamters, gSendQueueToOOB)
    '        End If
    '        '---Update HTML page for Web Site
    '        CreateHTMLPage()
    '    Else
    '        DisplayImage = False
    '        MsgBox("Senate voting display borad computer is off. Unable to change the background.", MsgBoxStyle.Critical, msgText)
    '    End If
    'Catch ex As Exception
    '    MsgBox(ex.Message, msgText)
    'End Try
    ' End Sub

    'Private Sub btnClearDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDisplay.Click
    '    If gChamberHelp Then
    '        If Not DisplayMessage("Are you sure you want to clear then display?", "Clear Display", "Y") Then
    '            Exit Sub
    '        End If
    '    End If

    '    gCalendarCodeDisplayed = ""
    '    gCalendarCode = ""
    '    gBillDisplayed = ""
    '    gBillNbrDisplayed = ""
    '    Me.lblWorkAreaDisplayed.Visible = False
    '    Me.btnExit.Enabled = True
    '    Me.OrderOfBusiness.Enabled = True

    '    OrderOfBusiness.Enabled = True
    '    ClearDisplayFlag = True
    '    CreateHTMLPage()
    'End Sub


    'Private Sub frmChamberDisplay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim str As String
    '    Dim ds As New DataSet
    '    Dim dsC As New DataSet
    '    Dim dsB As New DataSet
    '    Dim dr As DataRow


    '    Me.Top = 8
    '    Me.Left = 1


    '    If Not IsNothing(frmPhraseShow) Then
    '        If Not frmPhraseShow.IsDisposed Then
    '            frmPhraseShow.WindowState = FormWindowState.Normal
    '            frmPhraseShow.BringToFront()
    '            frmPhraseShow.Show()
    '        Else
    '            frmPhraseShow = New frmPhraseShow
    '            frmPhraseShow.Show()
    '        End If
    '    Else
    '        frmPhraseShow = New frmPhraseShow
    '        frmPhraseShow.Show()
    '    End If

    '    If Not IsNothing(frmSenatorShow) Then
    '        If Not frmSenatorShow.IsDisposed Then
    '            frmSenatorShow.WindowState = FormWindowState.Normal
    '            frmSenatorShow.BringToFront()
    '            frmSenatorShow.Show()
    '        Else
    '            frmSenatorShow = New frmSenatorShow
    '            frmSenatorShow.Show()
    '        End If
    '    Else
    '        frmSenatorShow = New frmSenatorShow
    '        frmSenatorShow.Show()
    '    End If

    '    '--- clear all of messages on myself Queue
    '    Dim mq As New MessageQueue(gSendQueueFromDisplay)
    '    Dim msg As New Message
    '    mq.Purge()

    '    '--- clear all of messages on Vote PS
    '    If SVotePC_On Then
    '        gOnlyOnePC = False
    '        Dim mq1 As New MessageQueue(gSendQueueToVotePC)
    '        Dim msg1 As New Message
    '        mq1.Purge()

    '        ReceiveTimer.Enabled = True
    '        ReceiveTimer.Interval = gReceiveQueueTimer
    '    Else
    '        gOnlyOnePC = True
    '        ReceiveTimer.Enabled = False
    '        If Not DisplayMessage("Senate Vote Computer is Offline! Would you want continue?", "Vote Computer is Offline", "Y") Then
    '            End
    '        End If
    '    End If

    '    If SOOB_On Then
    '        '--- clear all of messages on OOB PC
    '        Dim mqOOB As New MessageQueue(gSendQueueToOOB)
    '        Dim msgOOB As New Message
    '        mqOOB.Purge()
    '    End If



    '    If gTestMode Then
    '        lblTestMode.Visible = True
    '    Else
    '        lblTestMode.Visible = False
    '    End If

    '    '--- Put last vote id + 1
    '    If gTestMode = False Then
    '        Me.VoteID.Text = gVoteID + 1
    '    Else
    '        Me.VoteID.Text = 0
    '    End If

    '    lblChamberLight.Visible = True
    '    lblChamberLight.Text = "RR"
    '    OOBBackgroudChang = False

    '    '--- Last will move it - this model is woked 
    '    UpdateWorkDataSW = False
    '    ComingFrom = ""
    '    gCalendarCodeDisplayed = ""
    '    gBillNbrDisplayed = ""
    '    gCalendarDisplayed = ""
    '    gBillDisplayed = ""
    '    Me.lblChamberLight.Image = Nothing
    '    ' Me.lblChamberLight.BackColor = System.Drawing.Color.Lime
    '    Me.lblChamberLight.BackColor = System.Drawing.Color.LightGreen
    '    Me.lblChamberLight.Text = "Display Next Bill"
    '    Me.lblSession.Text = gSessionName
    '    Me.LegislativeDay.Text = gLegislativeDay & " - " & gCalendarDay

    '    ERHPushStack(("LoadSenatorsIntoArray"))

    '    '--- init so nothing can be entered until a bill has been selcted
    '    Me.WorkData.Enabled = False
    '    Me.WorkData1.Enabled = False
    '    Me.WorkData2.Enabled = False

    '    '--- initialize calendar and bills 
    '    Calendar.Items.Clear()
    '    ds = V_DataSet("Select * From tblCalendars", "R")
    '    For Each dr In ds.Tables(0).Rows
    '        Calendar.Items.Add(dr("Calendar"))
    '    Next

    '    Calendar_Bill(Calendar.Text)
    '    Calendar.SelectedIndex = 0
    '    Calendar_Click(sender, e)

    '    '--- initialize order of business
    '    str = "Select * From tblOrderOfBusiness "
    '    ds = V_DataSet(str, "R")
    '    For Each dr In ds.Tables(0).Rows
    '        OrderOfBusiness.Items.Add(dr("OrderOfBusiness"))
    '    Next

    '    OrderOfBusiness.Text = ds.Tables(0).Rows(0)("OrderOfBusiness")          '   Get first row record
    '    CurrentOrderOfBusiness.Text = OrderOfBusiness.Text

    '    ''PutOrdersOfBusinessOnOOBDisplayBoard()
    '    ''ApplyAttributesToOOBDisplayBoard()


    '    ' --- Load Senators from gSenatorName array in to SenatorName comboBox
    '    For i As Integer = 1 To gSenatorName.Length - 1
    '        Senators.Items.Add(gSenatorName(i))
    '    Next
    '    gNbrSenators = Senators.Items.Count

    '    '--- Load Committees from gCommitte() and gCommitteAbbreves() arrays to Committee comboBox
    '    ' LoadCommitteesIntoArray()
    '    For j As Integer = 1 To gCommittees.Length - 1
    '        Committees.Items.Add(gCommitteeAbbrevs(j) & " - " & gCommittees(j))
    '    Next

    '    '--- Load  Phases from gThePhrases(i) array in to Phases comboBox. Notic: gThePhrases(i) array included PhraseCode and Phrase, gPhrases has only Phrases string in array
    '    ' LoadPhrasesIntoArray()
    '    For k As Integer = 1 To gThePhrases.Length - 1
    '        Phrases.Items.Add(gThePhrases(k))
    '    Next

    '    '---Load Display Borader Image PowerPoint file name to cboBox
    '    str = "Select [File Name] From tblDisplayBoradImages "
    '    ds = V_DataSet(str, "R")
    '    For Each dr In ds.Tables(0).Rows
    '        cboDisplay.Items.Add(dr(0))
    '    Next
    '    For j As Integer = 0 To Me.cboDisplay.Items.Count - 1
    '        If InStr(UCase(cboDisplay.Items(j).ToString), "SENATEVOTINGPP") > 0 Then
    '            cboDisplay.Text = UCase(cboDisplay.Items(j).ToString)
    '        End If
    '    Next


    '    PingTimer.Enabled = True
    '    PingTimer.Interval = CInt(gPingPCTimer)

    'End Sub

    'Private Sub bntChang_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If cboDisplay.Text = "Clear Display" Then
    '        If gChamberHelp Then
    '            If Not DisplayMessage("Are you sure you want to clear then display?", "Clear Display", "Y") Then
    '                Exit Sub
    '            End If
    '        End If

    '        gCalendarCodeDisplayed = ""
    '        gCalendarCode = ""
    '        gBillDisplayed = ""
    '        gBillNbrDisplayed = ""
    '        Me.lblWorkAreaDisplayed.Visible = False
    '        Me.btnExit.Enabled = True
    '        Me.OrderOfBusiness.Enabled = True

    '        OrderOfBusiness.Enabled = True
    '        ClearDisplayFlag = True
    '    Else
    '        '---send voting information to display PC             bodyParamters = ""
    '        bodyParamters = "gCalendar - " & Me.CurrentCalendar.Text & "||" & "gBill - " & Me.CurrentBill.Text & "||" & "gCurrentPhrase - " & Me.Phrases.Text
    '        If SDisplay_On Then
    '            SendMessageToDisplay(UCase(cboDisplay.Text), bodyParamters, gSendQueueToDisplay)
    '        Else
    '            MsgBox("Senate voting display borad computer is off. Unable to change the background.", MsgBoxStyle.Critical, msgText)
    '        End If
    '    End If
    'End Sub


    'Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click
    '    '---send voting information to display PC 
    '    bodyParamters = ""
    '    bodyParamters = "gBill - " & Me.Bill.Text & "||" & "gWorkArea - " & Me.WorkData.Text & "||" & "gCurrentOrderOfBusiness - " & CurrentOrderOfBusiness.Text

    '    If SDisplay_On Then
    '        DisplayImage = True
    '        SendMessageToDisplay(UCase(cboDisplay.Text), bodyParamters, gSendQueueToDisplay)
    '    Else
    '        DisplayImage = False
    '        MsgBox("Senate voting display borad computer is off. Unable to change the background.", MsgBoxStyle.Critical, msgText)
    '    End If
    'End Sub

    ''--- print the doc in HTML and put on the web server so all can see - done everytime the chamber
    ''--- display operator hits update display
    'Dim ds As New DataSet
    'Dim dr As DataRow
    'Dim WrkFld, str As String, i As Integer, RetValue As Object

    'Try
    '    str = "Select * From tblOrderOfBusiness"
    '    ds = V_DataSet(str, connLocal, "R")

    '    FileOpen(1, gHTMLFile, OpenMode.Output)

    '    Print(1, "<HTML>")
    '    ' Print(1, "<Body><p>&nbsp;</p> ")
    '    Print(1, "<Body>")
    '    Print(1, "<table><tr>")
    '    Print(1, "<td align='left'><img src='http://Neverland/SoaTest/Images/senateseal.jpg' width=225 height=225></td>")
    '    Print(1, "<td width='200px'>&nbsp;</td>")
    '    Print(1, "<td width='300px'>")
    '    Print(1, "<table align='center' width=100%>")
    '    Print(1, "<tr align='center'><td></td>")
    '    Print(1, "<td align='center'>")
    '    Print(1, "<H1><Center><B><U><FONT FACE=" & Chr(34) & "Arial" & Chr(34) & " SIZE=" & Chr(34) & "4" & Chr(34) & " COLOR=" & Chr(34) & "Black" & Chr(34) & ">Order Of Business</U></B><BR><BR>")
    '    For Each dr In ds.Tables(0).Rows
    '        If dr("OrderOfBusiness") = Me.CurrentOrderOfBusiness.Text Then
    '            Print(1, "<Font Color=" & Chr(34) & "Red" & Chr(34) & ">" & Me.CurrentOrderOfBusiness.Text & "</Font><BR>")
    '        Else
    '            Print(1, dr("OrderOfBusiness") & "<BR>")
    '        End If
    '    Next
    '    Print(1, "</td><td></td></tr></table></td>")
    '    Print(1, "<td width='200px'>&nbsp;</td><td>")
    '    Print(1, "<tdalign='right'><img src='http://Neverland/SoaTest/Images/houseseal.jpg' width=225 height=225>")
    '    Print(1, "</td></tr></table>")


    '    If ClearDisplayFlag = False Then
    '        Print(1, "</Center></H1>--------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
    '        'Print(1, "<Font Color=" & Chr(34) & "Blue" & Chr(34) & ">" & Pfrm!BillDisplayBoard.Text & "</Font><BR>")
    '        Print(1, "<BR>")
    '        Print(1, "<Font size='4' face='Arial' Color=" & Chr(34) & "Blue" & Chr(34) & "><b>" & Me.CurrentBill.Text & "</b></Font><BR>")
    '        Print(1, "<BR>--------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
    '        Print(1, "<BR><font size='3' face='Arial' ><b>")
    '        'WrkFld = Pfrm!PhraseDisplayBoard.Text
    '        WrkFld = Me.WorkData.Text
    '        If Strings.Right(WrkFld, 2) <> vbCrLf Then
    '            WrkFld = WrkFld & vbCrLf
    '        End If
    '        Do
    '            Print(1, Mid(WrkFld, 1, InStr(WrkFld, vbCrLf) - 1))
    '            Print(1, "<BR>")
    '            If InStr(WrkFld, vbCrLf) = Len(WrkFld) - 1 Then
    '                Exit Do
    '            End If
    '            WrkFld = Mid(WrkFld, InStr(WrkFld, vbCrLf) + 2)
    '        Loop
    '        Print(1, "</b></font><BR><BR><BR>")
    '        Print(1, "<BR><BR><BR>")
    '        Print(1, "<Font Size=" & Chr(34) & "2" & Chr(34) & "</Font> Current Date/Time:  " & Now & "  " & Now.ToShortTimeString & "</Font>")
    '        Print(1, "</Body>")
    '        Print(1, "</HTML>")
    '    Else
    '        Print(1, "</Center></H1>------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
    '        Print(1, "<BR>")
    '        'Print(1, "<Font Color=" & Chr(34) & "Blue" & Chr(34) & ">" & Pfrm!BillDisplayBoard.Text & "</Font><BR>")
    '        Print(1, "<Font Color=" & Chr(34) & "Blue" & Chr(34) & ">" & "" & "</Font><BR>")
    '        Print(1, "<BR>----------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
    '        Print(1, "<BR>")
    '        'WrkFld = Pfrm!PhraseDisplayBoard.Text
    '        WrkFld = ""

    '        Print(1, WrkFld)
    '        Print(1, "<BR>")


    '        Print(1, "<BR><BR><BR>")
    '        Print(1, "<BR><BR><BR>")
    '        Print(1, "<Font Size=" & Chr(34) & "2" & Chr(34) & "</Font> Current Date/Time:  " & Now & "  " & Now.ToShortTimeString & "</Font>")
    '        Print(1, "</Body>")
    '        Print(1, "</HTML>")
    '        ClearDisplayFlag = False
    '    End If
    '    FileClose(1)

    '    Exit Sub

    'Catch ex As Exception

    '    If ex.Source = 76 Then
    '        RetValue = DisplayMessage("The folder for this session " & gHTMLFile & " needs to be created before an HTML " & _
    '           "can be written.  Please create the folder.", "Missing Folder For HTML Document", "S")
    '        Exit Sub
    '    End If
    'End Try


    'Private Sub btnUpdateDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateDisplay.Click
    '    '--- send this work area to the projection display

    '    If Me.CurrentBill.Text = "" Then
    '        RetValue = DisplayMessage("Please select a bill to display", "No Bill Selected", "S")
    '        Exit Sub
    '    End If

    '    gCalendarCodeDisplayed = gCalendarCode
    '    gBillNbrDisplayed = gBillNbr
    '    gCalendarDisplayed = gCalendar
    '    gBillDisplayed = gBill


    '    Me.OrderOfBusiness.Enabled = False
    '    Me.btnExit.Enabled = False
    '    Me.lblWorkAreaDisplayed.Visible = True

    '    CreateHTMLPage()
    ''--- if confirmation then triple the height of the bill display board and take away from the phrase board

    'If Left(Me.CurrentBill, 2) = "CF" Then
    '    If Pfrm!BillDisplayBoard.Height <> 3 * gSaveBillHeight Then
    '        Pfrm!BillDisplayBoard.Height = 3 * gSaveBillHeight
    '        Pfrm!PhraseDisplayBoard.Height = Pfrm!PhraseDisplayBoard.Height - gSaveBillHeight
    '        Pfrm!PhraseDisplayBoard.Top = Pfrm!BillDisplayBoard.Top + (3 * gSaveBillHeight)
    '    End If
    '    If gFreeFormat Then   ' can free format confirmations
    '        Pfrm!BillDisplayBoard.Text = Me.CurrentBill
    '    Else
    '        Pfrm!BillDisplayBoard.Text = Mid(Me.CurrentBill, 1, InStr(Me.CurrentBill, " -- ") - 1) & vbCrLf & Mid(Me.CurrentBill, InStr(Me.CurrentBill, " -- ") + 4)
    '    End If
    '    gStart = 0
    '    gLength = Len(Pfrm!BillDisplayBoard.Text)
    'Else
    '    ResetDisplayHeight()

    '    ' first do subject - not for CFs; if a calendar page follows then subject
    '    ' then do it with the bill attributes

    '    Pfrm!BillDisplayBoard.Text = ""
    '    i = 0
    '    If gSubject <> "" Then
    '        i = InStr(gBill, " - " & gSubject)
    '        j = InStr(gBill, " p.")  ' calendar page at end of line
    '        If j > i Then ' page found
    '            Pfrm!BillDisplayBoard.Text = Mid(gBill, j)
    '            gStart = 0
    '            gLength = Len(Mid(gBill, j))
    '            ApplySpecificAttributesToDisplayBoard("Bill", gBillAttributes)
    '        End If

    '        Pfrm!BillDisplayBoard.Text = "- " & gSubject & Pfrm!BillDisplayBoard.Text
    '        gStart = 0
    '        gLength = Len(gSubject) + 2  'allow for the dash and space
    '        ApplySpecificAttributesToDisplayBoard("Bill", gSubjectAttributes)
    '    End If

    '    ' now do senator and put in front of subject - makes colors work by building line from end to start

    '    gStart = 0
    '    If i > 0 Then
    '        gLength = Len(Mid(gBill, 1, i - 1)) ' pick up to the - separating bill and subject
    '        Pfrm!BillDisplayBoard.Text = Mid(gBill, 1, i) & Pfrm!BillDisplayBoard.Text
    '    Else
    '        gLength = Len(gBill)
    '        Pfrm!BillDisplayBoard.Text = gBill
    '    End If
    'End If ' not CFs

    'ApplySpecificAttributesToDisplayBoard("Bill", gBillAttributes)
    'gCalendarCodeDisplayed = gCalendarCode
    'gBillNbrDisplayed = gBillNbr
    'gCalendarDisplayed = gCalendar
    'gBillDisplayed = gBill

    'DisplayPhraseWorkArea()


    'End Sub

    ''Try
    ''    Dim mq As New MessageQueue
    ''    Dim msg As New Message
    ''    Dim bodyText As String = ""
    ''    Dim bodyParamters As String

    ''    Select Case Trim(CurrentCalendar.Text)
    ''        Case "Regular Order"
    ''            bodyText = Mid(CurrentBill.Text, 1, InStr(CurrentBill.Text, " ") - 1)
    ''        Case "Local Bills"
    ''            bodyText = Mid(CurrentBill.Text, 1, InStr(CurrentBill.Text, " ") - 1)
    ''        Case "Other"
    ''            bodyText = Mid(CurrentBill.Text, 1, InStr(CurrentBill.Text, " ") - 1)
    ''        Case "Confirmations"
    ''            bodyText = Mid(CurrentBill.Text, 1, InStr(CurrentBill.Text, "-") - 2)
    ''        Case "Motions"
    ''            bodyText = CurrentBill.Text
    ''    End Select

    ''    bodyParamters = "gCalendar - " & Me.Calendar.Text & "||" & "gCalendarCode - " & gCalendarCode & "||" & "gBill - " & Me.Bill.SelectedItem & "||" & "gBillNbr - " & gBillNbr & "||" & "gCurrentPhrase - " & gCurrentPhrase

    ''    ' Create the queqe if it does not already exist
    ''    'If Not MessageQueue.Exists(queueName) Then
    ''    '    Try
    ''    '        'msgQ = MessageQueue.Create(queueName)
    ''    '        MessageQueue.Create(queueName)
    ''    '    Catch ex As Exception
    ''    '        Throw New Exception("Error Creating Quequ")
    ''    '    End Try
    ''    'Else
    ''    Try
    ''        mq.Path = gSendQueueFromVotePC                         'queueName = ".\PRIVATE\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE\SenateVoteQueue"
    ''        msg.Priority = MessagePriority.Normal
    ''        ' msg.Label = "STARTVOTE"
    ''        msg.Label = label
    ''        msg.Body = bodyParamters
    ''        mq.Send(msg)
    ''    Catch ex As Exception
    ''        Throw New Exception("Error Getting Quequ")
    ''    End Try
    ''    mq.Close()
    ''Catch ex As Exception
    ''    MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Voting System")
    ''End Try


    ''using the peek methos
    'dim MyQueue as new MessageQueue(".\privete\myQueue")
    'dim tempMSg as new Message
    'tempMsg = MyQueue.Peek()
    ''Check if the message is ours 
    'If tempMsg.Label ="Example" then
    ''we got the message so we remove it from the Queue
    'MyQueue.Receive()
    'end if



    ' ''Try
    ' ''    Dim msg As Message

    ' ''    If (MessageQueue.Exists(GetQueueName)) = False Then
    ' ''        MessageQueue.Create(GetQueueName)
    ' ''    End If

    ' ''    Dim oque As New MessageQueue(GetQueueName)
    ' ''    oque.Formatter = New XmlMessageFormatter(New Type() {GetType(System.String)})
    ' ''    Dim que_len As Message() = oque.GetAllMessages

    ' ''    If que_len.Length > 0 Then
    ' ''        msg = oque.Peek(New TimeSpan(0, 0, 5))
    ' ''        'msg = oque.Receive(New TimeSpan(0, 0, 5))
    ' ''        msgText = msg.Body
    ' ''        If msgText <> "" Then
    ' ''            If msg.Label = "CLOSEDVOTE" Then

    ' ''                Me.btnVote.Enabled = True
    ' ''                Me.btnUpdateDisplay.Enabled = True
    ' ''                Me.OrderOfBusiness.Enabled = True
    ' ''                Me.btnExit.Enabled = True
    ' ''                Me.lblChamberLight.Text = "Waiting For Voting"
    ' ''                gVotingStarted = True
    ' ''            Else
    ' ''                Me.btnVote.Enabled = False
    ' ''                Me.btnUpdateDisplay.Enabled = False
    ' ''                Me.OrderOfBusiness.Enabled = False
    ' ''                Me.btnExit.Enabled = False
    ' ''                Me.lblChamberLight.Text = "Waiting For Voting"
    ' ''                gVotingStarted = False
    ' ''            End If
    ' ''        End If
    ' ''    Else
    ' ''        msgText = ""
    ' ''    End If
    ' ''    oque.Close()
    ' ''Catch ex As Exception
    ' ''    ' MessageBox.Show(ex.Message)
    ' ''End Try


    'Try
    '    Dim msg As Message

    '    If (MessageQueue.Exists(queueName)) = False Then
    '        MessageQueue.Create(queueName)
    '    End If

    '    Dim oque As New MessageQueue(queueName)


    '    oque.Formatter = New XmlMessageFormatter(New Type() {GetType(System.String)})

    '    Dim que_len As Message() = oque.GetAllMessages

    '    If que_len.Length > 0 Then
    '        msg = oque.Receive(New TimeSpan(0, 0, 5))
    '        msgText = msg.Body
    '        'que_len.DefaultIfEmpty()
    '    Else
    '        MessageBox.Show("No Messages in Queue")
    '        msgText = ""
    '    End If
    'Catch ex As Exception
    '    MessageBox.Show(ex.Message)
    'End Try




    'Try
    '    ''Dim mq As New MessageQueue("FormatName:DIRECT=OS:LEG13\PRIVATE\SenateVoteQueue")

    '    Dim mq As New MessageQueue(queueName)
    '    Dim msg As Message = mq.Receive(New TimeSpan(0, 0, 3))        '3 = three seconds

    '    mq.Receive()

    '    If msg.Label = "CLOSE" Then
    '        Me.lblChamberLight.BackColor = System.Drawing.Color.Lime
    '        Me.lblChamberLight.Text = "Display Next Bill"
    '    ElseIf msg.Label = "CANCLE" Then
    '        Me.lblChamberLight.Text = "Cancle Voting"
    '        Me.lblChamberLight.BackColor = Color.Red
    '    End If

    '    Me.btnVote.Enabled = True
    '    Me.btnClearDisplay.Enabled = True
    '    Me.btnUpdateDisplay.Enabled = True
    '    Me.OrderOfBusiness.Enabled = True
    '    Me.btnExit.Enabled = True

    'Catch ex As Exception
    '    MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Voting System")
    'End Try


    'Private Sub AddPhrase_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
    '    '--- format is Phrase # followed by senator # followed by committee, ex.  3,5,A&F to get
    '    '--- Phrase #3 followed by senator #5 followed by committee A&F; this senator part and
    '    '--- committees only applies if there are senator and committee insertion point(s) in the phrase

    '    Dim fnt As Font
    '    fnt = Me.WorkData.Font
    '    Me.WorkData.Font = New Font(fnt.Name, 8, FontStyle.Regular)

    '    ' LoadSenatorsIntoArray()

    '    Dim WrkFld As String, WrkFld3 As String, i As Integer
    '    Dim SNbr As Integer, PNbr As Integer, CAbbrev As String

    '    WrkFld = Me.AddPhrase.Text
    '    If Strings.Right(WrkFld, 1) <> "," Then
    '        WrkFld = WrkFld & ","
    '    End If
    '    WrkFld = ReplaceCharacter(WrkFld, " ", "") ' get rid of all spaces
    '    ''WrkFld = Strings.Replace(WrkFld, " ", "") ' get rid of all spaces
    '    If NToB(Me.AddPhrase.Text) = "," Then  ' no senator so exit
    '        Exit Sub
    '    End If

    '    Me.AddPhrase.Text = ""

    '    PNbr = 0
    '    SNbr = 0
    '    CAbbrev = ""

    '    PNbr = Val(Mid(WrkFld, 1, InStr(WrkFld, ",") - 1))
    '    WrkFld = Mid(WrkFld, InStr(WrkFld, ","))   ' delete up to ,
    '    If (WrkFld <> ",") And (WrkFld <> ",,") And (WrkFld <> ",,,") Then
    '        If Strings.Left(WrkFld, 2) = ",," Then  ' senator and abbrev entered
    '            CAbbrev = UCase(Mid(WrkFld, 3, InStr(3, WrkFld, ",") - 3))
    '        Else
    '            WrkFld = Mid(WrkFld, 2)
    '            SNbr = Val(Mid(WrkFld, 1, InStr(2, WrkFld, ",") - 1))
    '            WrkFld = Mid(WrkFld, InStr(WrkFld, ",")) ' delete up to ,
    '            If (WrkFld <> ",") And (WrkFld <> ",,") Then
    '                CAbbrev = UCase(Mid(WrkFld, 2, InStr(2, WrkFld, ",") - 2))
    '            End If
    '        End If
    '    End If

    '    '---capture phrase
    '    WrkFld3 = ""
    '    For i = 1 To gNbrPhrases
    '        If PNbr = gPhraseCodes(i) Then
    '            WrkFld3 = gPhrases(i)
    '            Exit For
    '        End If
    '    Next i

    '    If WrkFld3 = "" Then   ' phrase not found so exit
    '        Exit Sub
    '    End If

    '    '--- see if senator insertion point is in the phrase, and if so add the senator name
    '    If InStr(WrkFld3, gSenatorInsertionPoint) > 0 Then
    '        If SNbr = 0 Then
    '        ElseIf (SNbr < 0) Or (SNbr > gNbrSenators) Then
    '            If gChamberHelp Then
    '                RetValue = DisplayMessage("Senator # must be between 1 and " & gNbrSenators _
    '                   & ".  Please re-enter", "Invalid Senator #", "S")
    '            End If
    '            Exit Sub
    '        ElseIf InStr(WrkFld3, gSenatorInsertionPoint) = 1 Then
    '            WrkFld3 = "Senator " & gSenatorName(SNbr) & Mid(WrkFld3, 2)
    '        Else
    '            WrkFld3 = Mid(WrkFld3, 1, InStr(WrkFld3, gSenatorInsertionPoint) - 1) & "Senator " & gSenatorName(SNbr) & Mid(WrkFld3, InStr(WrkFld3, gSenatorInsertionPoint) + 1)
    '        End If
    '    End If

    '    '---see if committee insertion point is in the phrase, and if so add the committee name
    '    If InStr(WrkFld3, gCommitteeInsertionPoint) > 0 Then
    '        If CAbbrev = "" Then
    '        Else
    '            For i = 1 To gNbrCommittees
    '                If gCommitteeAbbrevs(i) = CAbbrev Then
    '                    If InStr(WrkFld3, gCommitteeInsertionPoint) = 1 Then
    '                        WrkFld3 = gCommittees(i) & Mid(WrkFld3, 2)
    '                    Else
    '                        WrkFld3 = Mid(WrkFld3, 1, InStr(WrkFld3, gCommitteeInsertionPoint) - 1) & gCommittees(i) & Mid(WrkFld3, InStr(WrkFld3, gCommitteeInsertionPoint) + 1)
    '                    End If
    '                    Exit For
    '                End If
    '            Next i
    '            If InStr(WrkFld3, gCommitteeInsertionPoint) > 0 Then  ' if still there, then invalid abbrev
    '                If gChamberHelp Then
    '                    RetValue = DisplayMessage("The committee abbreviation " & CAbbrev & " you entered is invalid.", _
    '                       "Invalid Committee Abbreviation", "S")
    '                End If
    '                Exit Sub
    '            End If
    '        End If
    '    End If

    '    '---put this phrase in the work area at the phrase insertion point; if no phrase insertion point
    '    '---then add it to the end of the work area
    '    If WrkFld3 > "" Then
    '        If WrkFld3 <> "ADJOURNMENT" Then
    '            WrkFld3 = WrkFld3 & vbCrLf
    '            InsertIntoWorkData(WrkFld3, gPhraseInsertionPoint)
    '        Else
    '            Dim fntn As Font
    '            fntn = Me.WorkData.Font
    '            Me.WorkData.Font = New Font(fntn.Name, 12, FontStyle.Bold)
    '            Me.WorkData.Enabled = True
    '            Me.CurrentBill.Text = "ADJOURMENT"
    '            Me.WorkData.Text = "The senate is in adjournment until "
    '        End If
    '    End If
    'End Sub
#End Region

#Region "original code"
    'Private Sub CreateHTMLPage()
    '    '--- print the doc in HTML and put on the web server so all can see - done everytime the chamber
    '    '--- display operator hits update display
    '    Dim ds As New DataSet
    '    Dim dr As DataRow
    '    Dim WrkFld, str As String, i As Integer, RetValue As Object

    '    Try

    '        str = "Select * From tblOrderOfBusiness"
    '        ds = V_DataSet(str, connLocal, "R")


    '        FileOpen(1, gHTMLFile, OpenMode.Output)
    '        Print(1, "<HTML>")
    '        Print(1, "<Body><p>&nbsp;</p> ")
    '        Print(1, "<H1><Center><B><U><FONT FACE=" & Chr(34) & "Arial" & Chr(34) & " SIZE=" & Chr(34) & "4" & Chr(34) & " COLOR=" & Chr(34) & "Black" & Chr(34) & ">Order Of Business</U></B><BR><BR>")
    '        For Each dr In ds.Tables(0).Rows
    '            If dr("OrderOfBusiness") = Me.CurrentOrderOfBusiness.Text Then
    '                Print(1, "<Font Color=" & Chr(34) & "Red" & Chr(34) & ">" & Me.CurrentOrderOfBusiness.Text & "</Font><BR>")
    '            Else
    '                Print(1, dr("OrderOfBusiness") & "<BR>")
    '            End If
    '        Next

    '        If ClearDisplayFlag = False Then
    '            Print(1, "</Center></H1>--------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
    '            'Print(1, "<Font Color=" & Chr(34) & "Blue" & Chr(34) & ">" & Pfrm!BillDisplayBoard.Text & "</Font><BR>")
    '            Print(1, "<BR>")
    '            Print(1, "<Font Color=" & Chr(34) & "Blue" & Chr(34) & ">" & Me.CurrentBill.Text & "</Font><BR>")
    '            Print(1, "<BR>--------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
    '            Print(1, "<BR>")
    '            'WrkFld = Pfrm!PhraseDisplayBoard.Text
    '            WrkFld = Me.WorkData.Text
    '            If Strings.Right(WrkFld, 2) <> vbCrLf Then
    '                WrkFld = WrkFld & vbCrLf
    '            End If
    '            Do
    '                Print(1, Mid(WrkFld, 1, InStr(WrkFld, vbCrLf) - 1))
    '                Print(1, "<BR>")
    '                If InStr(WrkFld, vbCrLf) = Len(WrkFld) - 1 Then
    '                    Exit Do
    '                End If
    '                WrkFld = Mid(WrkFld, InStr(WrkFld, vbCrLf) + 2)
    '            Loop
    '            Print(1, "<BR><BR><BR>")
    '            Print(1, "<BR><BR><BR>")
    '            Print(1, "<Font Size=" & Chr(34) & "2" & Chr(34) & "</Font> Current Date/Time:  " & Now & "  " & Now.ToShortTimeString & "</Font>")
    '            Print(1, "</Body>")
    '            Print(1, "</HTML>")
    '        Else
    '            Print(1, "</Center></H1>------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
    '            Print(1, "<BR>")
    '            'Print(1, "<Font Color=" & Chr(34) & "Blue" & Chr(34) & ">" & Pfrm!BillDisplayBoard.Text & "</Font><BR>")
    '            Print(1, "<Font Color=" & Chr(34) & "Blue" & Chr(34) & ">" & "" & "</Font><BR>")
    '            Print(1, "<BR>----------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
    '            Print(1, "<BR>")
    '            'WrkFld = Pfrm!PhraseDisplayBoard.Text
    '            WrkFld = ""

    '            Print(1, WrkFld)
    '            Print(1, "<BR>")


    '            Print(1, "<BR><BR><BR>")
    '            Print(1, "<BR><BR><BR>")
    '            Print(1, "<Font Size=" & Chr(34) & "2" & Chr(34) & "</Font> Current Date/Time:  " & Now & "  " & Now.ToShortTimeString & "</Font>")
    '            Print(1, "</Body>")
    '            Print(1, "</HTML>")
    '            ClearDisplayFlag = False
    '        End If
    '        FileClose(1)

    '        Exit Sub

    '    Catch ex As Exception

    '        If ex.Source = 76 Then
    '            RetValue = DisplayMessage("The folder for this session " & gHTMLFile & " needs to be created before an HTML " & _
    '               "can be written.  Please create the folder.", "Missing Folder For HTML Document", "S")
    '            Exit Sub
    '        End If
    '    End Try

    'End Sub



    'Private Sub CreateHTMLPage()
    '    '--- print the doc in HTML and put on the web server so all can see - done everytime the chamber
    '    '--- display operator hits update display
    '    Dim ds As New DataSet
    '    Dim dr As DataRow
    '    Dim WrkFld, str As String, i As Integer, RetValue As Object

    '    Try

    '        str = "Select * From tblOrderOfBusiness"
    '        ds = V_DataSet(str, connLocal, "R")


    '        FileOpen(1, gHTMLFile, OpenMode.Output)
    '        Print(1, "<HTML>")
    '        Print(1, "<Body>")
    '        Print(1, "<H1><Center><B><U><FONT FACE=" & Chr(34) & "Arial" & Chr(34) & " SIZE=" & Chr(34) & "4" & Chr(34) & " COLOR=" & Chr(34) & "Black" & Chr(34) & ">Order Of Business</U></B><BR><BR>")
    '        For Each dr In ds.Tables(0).Rows
    '            If dr("OrderOfBusiness") = Me.CurrentOrderOfBusiness.Text Then
    '                Print(1, "<Font Color=" & Chr(34) & "Red" & Chr(34) & ">" & Me.CurrentOrderOfBusiness.Text & "</Font><BR>")
    '            Else
    '                Print(1, dr("OrderOfBusiness") & "<BR>")
    '            End If

    '        Next

    '        Print(1, "</Center></H1>-----------------------------------------------------------------------------------------------------------<BR>")
    '        'Print(1, "<Font Color=" & Chr(34) & "Blue" & Chr(34) & ">" & Pfrm!BillDisplayBoard.Text & "</Font><BR>")
    '        Print(1, "<Font Color=" & Chr(34) & "Blue" & Chr(34) & ">" & Me.CurrentBill.Text & "</Font><BR>")
    '        Print(1, "<BR>-----------------------------------------------------------------------------------------------------------<BR>")
    '        'WrkFld = Pfrm!PhraseDisplayBoard.Text
    '        WrkFld = Me.WorkData.Text
    '        If Strings.Right(WrkFld, 2) <> vbCrLf Then
    '            WrkFld = WrkFld & vbCrLf
    '        End If
    '        Do
    '            Print(1, Mid(WrkFld, 1, InStr(WrkFld, vbCrLf) - 1))
    '            Print(1, "<BR>")
    '            If InStr(WrkFld, vbCrLf) = Len(WrkFld) - 1 Then
    '                Exit Do
    '            End If
    '            WrkFld = Mid(WrkFld, InStr(WrkFld, vbCrLf) + 2)
    '        Loop
    '        Print(1, "<BR><BR><BR>")
    '        Print(1, "<Font Size=" & Chr(34) & "2" & Chr(34) & "</Font> Current Date/Time:  " & Now & "  " & Now.ToShortTimeString & "</Font>")
    '        Print(1, "</Body>")
    '        Print(1, "</HTML>")
    '        FileClose(1)

    '        Exit Sub

    '    Catch ex As Exception

    '        If ex.Source = 76 Then
    '            RetValue = DisplayMessage("The folder for this session " & gHTMLFile & " needs to be created before an HTML " & _
    '               "can be written.  Please create the folder.", "Missing Folder For HTML Document", "S")
    '            Exit Sub
    '        End If
    '    End Try

    'End Sub



    'Private Sub DisplayHTMLTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReceiveMessageTimer.Tick
    '    ' CreateHTMLPage()
    '    ReceiveMessageFromQueue()
    '    'If msgText = "CLOSEDVOTE" Then
    '    '    Me.btnVote.Enabled = True
    '    '    Me.btnClearDisplay.Enabled = True
    '    '    Me.btnUpdateDisplay.Enabled = True
    '    '    Me.OrderOfBusiness.Enabled = True
    '    '    Me.btnExit.Enabled = True
    '    '    Me.lblChamberLight.Text = "Display Next Bill"
    '    '    gVotingStarted = True
    '    'End If
    'End Sub



    'Private Sub DisplayPhraseWorkArea()

    '    ' put work area to projection display

    '    Dim i As Integer, WrkFld As String

    '    WrkFld = Me.WorkData.Text
    '    If Strings.Left(WrkFld, 2) <> vbCrLf Then  ' prefix with crlf since working back to front and need as a flag
    '        WrkFld = vbCrLf & WrkFld
    '    End If
    '    If Strings.Right(WrkFld, 2) = vbCrLf Then  ' drop trailing crlf
    '        WrkFld = Mid(WrkFld, 1, Len(WrkFld) - 2)
    '    End If

    '    Pfrm.PhraseDisplayBoard.Text = ""
    '    For i = 1 To gNbrPhrasesToDisplay
    '        Pfrm.PhraseDisplayBoard.Text = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) + 2) & vbCrLf & Pfrm.PhraseDisplayBoard.Text
    '        If InStrRev(WrkFld, vbCrLf) = 1 Then  ' work area has less than # of phrases which can be displayed
    '            Exit For
    '        End If
    '        If WrkFld = "" Then
    '            Exit For
    '        End If
    '        WrkFld = Mid(WrkFld, 1, InStrRev(WrkFld, vbCrLf) - 1)
    '    Next i

    '    ' now set attributes

    '    gStart = 0
    '    gLength = Len(Pfrm.PhraseDisplayBoard.Text)
    '    ''  ApplySpecificAttributesToDisplayBoard("Phrase", gPhraseAttributes)

    '    ' if button is disabled, then have already told vote PC to vote

    '    If Not Me.btnVote.Enabled Then
    '        gPutParams = True
    '        gPutCalendarCode = gCalendarCode
    '        gPutCalendar = gCalendar
    '        gPutBill = gBill
    '        gPutBillNbr = gBillNbr
    '        gPutPhrase = gCurrentPhrase
    '        gPutChamberLight = System.Drawing.Color.Red.ToArgb
    '        gPutVotingLight = System.Drawing.Color.Green.ToArgb
    '    End If
    'End Sub

    'Sub ResetDisplayHeight()
    '    If Pfrm.BillDisplayBoard.Height = 3 * gSaveBillHeight Then
    '        Pfrm.BillDisplayBoard.Height = gSaveBillHeight
    '        Pfrm.PhraseDisplayBoard.Top = Pfrm.BillDisplayBoard.Top + gSaveBillHeight
    '        Pfrm.PhraseDisplayBoard.Height = gSavePhraseHeight
    '    End If

    'End Sub


    'Private Sub SOCNbr_TextChanged(sender As System.Object, e As System.EventArgs) Handles SOCNbr.TextChanged

    'End Sub

    'Private Sub txtSRChange_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtSRChange.TextChanged
    '    If strOrigSRName <> Me.Calendar.Text Then
    '        btnSRSave.Enabled = True
    '    Else
    '        btnSRSave.Enabled = False
    '    End If
    'End Sub
#End Region
  
  
  



   

    
    Private Sub CurrentBill_TextChanged(sender As Object, e As System.EventArgs) Handles CurrentBill.TextChanged
        If tPage0 Then
            work_bill = Me.CurrentBill.Text
        End If
        If tPage1 Then
            work1_bill = Me.CurrentBill.Text
        End If
        If tPage2 Then
            work2_bill = Me.CurrentBill.Text
        End If
    End Sub
End Class