Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Timers
Imports System.Messaging
Imports System.Text
Imports System.DateTime
Imports System.DBNull
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Net.NetworkInformation

Public Class frmChamberDisplay
    Private ComingFrom, strBillNbr, strOrigSRName As String
    Private Highlight As Short
    Private UpdateWorkDataSW As Boolean = False
    Private ClearDisplayFlag As Boolean = False
    Private iniVoteID As Object

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

        ' isDeleteSOC = False
        '  isDownLoadBillorSOC = False
    End Sub

    Private Sub frmChamberDisplay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ds As New DataSet
        Dim dr As DataRow

        Me.MdiParent = frmMain

        displaied_work = False
        displaied_work1 = False
        displaied_work2 = False

        Try
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
                '  Me.txtVoteID.Text = gVoteID + 1
            End If

            If gTestMode Then
                lblTestMode.Visible = True
            Else
                lblTestMode.Visible = False
            End If

            If isDownLoadBillorSOC = False And isDeleteSOC = False Then         '--- use flag to avoid clear display screen aftere downloawed bills or Special Order Calendar
                CreateClearHTMLPage()
            End If

            '--- Last will move it - this model is woked 
            UpdateWorkDataSW = False
            ComingFrom = ""
            gCalendarCodeDisplayed = ""
            gBillNbrDisplayed = ""
            gCalendarDisplayed = ""
            gBillDisplayed = ""

            If isDownLoadBillorSOC = False Then
                TC.TabPages(0).BackColor = Color.Red
                TC.TabPages(1).BackColor = Color.Red
                TC.TabPages(2).BackColor = Color.Red
                TC.TabPages(0).Text = "Work Area"
                TC.TabPages(1).Text = "Area 1"
                TC.TabPages(2).Text = "Area 2"
            End If

            Me.lblChamberLight.BackColor = System.Drawing.Color.LimeGreen
            Me.lblChamberLight.Text = "Display Next Bill"
            Me.lblSession.Text = gSessionName
            Me.LegislativeDay.Text = gLegislativeDay & " - " & gCalendarDay

            '--- initialize calendar and bills 
            LoadCalendar()

            Me.Calendar.SelectedItem = "Regular Order"
            Me.OrderOfBusiness.SelectedItem = ""
            Calendar_Bill(Me.Calendar.Text)
            Calendar.SelectedIndex = 0
            Calendar_Click(sender, e)

            '--- initialize order of business
            strSQL = "Select * From tblOrderOfBusiness "
            ds = V_DataSet(strSQL, "R")
            OrderOfBusiness.Items.Add("")
            OrderOfBusiness.SelectedIndex = 0
            For Each dr In ds.Tables(0).Rows
                OrderOfBusiness.Items.Add(dr("OrderOfBusiness"))
            Next

            '--- Load Senators from gSenatorName array in to SenatorName comboBox
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

            '--- Load OOB Display Borad HTML nameS 
            strSQL = "Select [File Name] From tblOOBDisplayHTML Order By OID "
            ds = V_DataSet(strSQL, "R")

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
                If isDownLoadBillorSOC = False And isDeleteSOC = False Then
                    SendStartOOBToOOB("", "CLEAR DISPLAY")
                End If
            End If

            V_DataSet("Update tblVotingParameters Set ParameterValue='" & gSessionName & "' WHERE UCASE(Parameter) = 'LASTSESSIONNAME'", "U")
            V_DataSet("Update tblVotingParameters Set ParameterValue='" & gSessionID & "' WHERE UCASE(Parameter) = 'LASTSESSIONID'", "U")

            '---initial temple Each work area data
            tmp_DataSet("D")
            tmp_DataSet("A")

            Me.btnCreateHTMLPage.BackColor = Color.Green
            isDeleteSOC = False
            straightSOC = False
        Catch ex As Exception
            DisplayMessage(ex.Message & vbNewLine & "Please close try open again. If the error consistent occurred, contact to Administrator.", "Unable open " & "/" & "Chamber Display Window" & "/""", "I")
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub LoadCalendar()
        Dim ds As New DataSet
        Try
            '--- initialize calendar and bills 
            Calendar.Items.Clear()
            ds = V_DataSet("select * From tblCalendars Order By CalendarCode", "R")
            For Each dr In ds.Tables(0).Rows
                Calendar.Items.Add(dr("Calendar"))
            Next
            ds.Dispose()
        Catch ex As Exception
            ds.Dispose()
            DisplayMessage(ex.Message & vbNewLine & "IF the error consisten occurred, please contact to Administrator.", "Unable Load Calendar", "I")
        End Try
    End Sub

    Private Sub AddPhrase_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles AddPhrase.KeyDown
        If e.KeyCode = Keys.Enter Then
            '--- format is Phrase # followed by senator # followed by committee, ex.  3,5,A&F to get
            '--- Phrase #3 followed by senator #5 followed by committee A&F; this senator part and
            '--- committees only applies if there are senator and committee insertion point(s) in the phrase
            Dim WrkFld As String, WrkFld3 As String, i As Integer
            Dim SNbr As Integer, PNbr As Integer, CAbbrev As String
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

            PNbr = Val(Mid(WrkFld, 1, InStr(WrkFld, ",") - 1))
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

            If WrkFld3 = "" Then   '--- phrase not found so exit
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

            '--- see if committee insertion point is in the phrase, and if so add the committee name
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
            ElseIf InsertionPoint = gPhraseInsertionPoint Then      '--- append the phrase
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
            ElseIf InsertionPoint = gPhraseInsertionPoint Then      '--- append the phrase
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
            ElseIf InsertionPoint = gPhraseInsertionPoint Then      '--- append the phrase
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

    Private Sub Bill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Bill.Click
        Dim fnt As Font

        If tPage0 Then
            Me.WorkData.Text = ""
            fnt = Me.WorkData.Font
            Me.WorkData.Font = New Font(fnt.Name, 11, FontStyle.Regular)
            work_bill = Me.Bill.SelectedItem
            wrkOOB = Me.OrderOfBusiness.SelectedItem.ToString
            wrkCal = Me.Calendar.SelectedItem.ToString
            wrkCalCode = GetCalendarCode(wrkCal)
        ElseIf tPage1 Then
            Me.WorkData1.Text = ""
            fnt = Me.WorkData1.Font
            Me.WorkData1.Font = New Font(fnt.Name, 11, FontStyle.Regular)
            work1_bill = Me.Bill.SelectedItem
            wrkOOB1 = Me.OrderOfBusiness.SelectedItem.ToString
            wrkCal1 = Me.Calendar.SelectedItem.ToString
            wrkCalCode1 = GetCalendarCode(wrkCal1)
        ElseIf tPage2 Then
            Me.WorkData2.Text = ""
            fnt = Me.WorkData2.Font
            Me.WorkData2.Font = New Font(fnt.Name, 11, FontStyle.Regular)
            work2_bill = Me.Bill.SelectedItem
            wrkOOB2 = Me.OrderOfBusiness.SelectedItem.ToString
            wrkCal2 = Me.Calendar.SelectedItem.ToString
            wrkCalCode2 = GetCalendarCode(wrkCal2)
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

        '--- If no motion calendar and there is a senator insertion point in this motion, 
        '--- then pull down the senator list box
        If (gCalendarCode = "M") And (InStr(Bill.Text, gSenatorInsertionPoint) > 0) Then
            Me.CurrentBill.Text = gBill
            Me.Senators.Focus()
            SendKeys.Send("{F4}")
        Else
            '--- if Motion TEST found, add contextMenu (right click mouse pop-up menu) to allowd delete it
            Dim mnuContextMenu As New ContextMenu()
            Me.ContextMenu = mnuContextMenu
            Dim mnuItemDelete As New MenuItem()
            If gCalendarCode = "M" And InStr(UCase(Me.Bill.SelectedItem), "TEST") > 0 Then
                mnuItemDelete.Text = "&Delete " & UCase(Me.Bill.SelectedItem) & " Motion"
                AddHandler mnuItemDelete.Click, AddressOf ContextMenu_Click
                mnuContextMenu.MenuItems.Add(mnuItemDelete)
            Else
                mnuItemDelete.Text = "&Delete " & UCase(Me.Bill.SelectedItem) & " Motion"
                mnuContextMenu.MenuItems.Remove(mnuItemDelete)
            End If
        End If

        If Me.Calendar.Text = "Special Order Calendar" Then
            tmp_DataSet("U")
        End If
    End Sub

    Private Sub ContextMenu_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            strSQL = "Delete From tblBills Where CalendarCode ='M' and Bill ='" & Me.Bill.SelectedItem & "'"
            V_DataSet(strSQL, "D")

            '--- after delete TEST Motion has be reload Bills
            Calendar_Click(Calendar, Nothing)

            '--- clear workdata arae
            If tPage0 Then
                WorkData.Text = ""
            ElseIf tPage1 Then
                WorkData1.Text = ""
            ElseIf tPage2 Then
                WorkData2.Text = ""
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Remove TEST Motion", "I")
        End Try
    End Sub

    Private Sub Calendar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar.Click
        Dim dr As DataRow
        Dim ds As New DataSet

        Try
            gCalendarCode = ""
            UpdateWorkDataSW = False
            gCalendar = Me.Calendar.Text
            Me.lblWorkAreaDisplayed.Visible = False                   'reset in case it was on
            Me.CurrentCalendar.Text = Me.Calendar.Text

            If gCalendar <> "" Then
                ds = GetBills(gCalendar)
                Me.Bill.Items.Clear()
                For Each dr In ds.Tables(0).Rows
                    Me.Bill.Items.Add(dr("Bill"))
                    gCalendarCode = dr("CalendarCode")
                    If tPage0 Then
                        If isDownLoadBillorSOC = False And isDeleteSOC = False Then
                            wrkCalCode = gCalendarCode
                        Else
                            wrkCalCode = wrkCalCode
                        End If
                    ElseIf tPage1 Then
                        If isDownLoadBillorSOC = False And isDeleteSOC = False Then
                            wrkCalCode1 = gCalendarCode
                        Else
                            wrkCalCode1 = wrkCalCode1
                        End If
                    ElseIf tPage2 Then
                        If isDownLoadBillorSOC = False And isDeleteSOC = False Then
                            wrkCalCode2 = gCalendarCode
                        Else
                            wrkCalCode2 = wrkCalCode2
                        End If
                    End If
                Next
                CurrentCalendar.Text = Calendar.SelectedItem
                CurrentBill.Text = ""
            Else
                tPage0 = True
                tPage1 = False
                tPage2 = False
                wrkCal = Me.Calendar.Text
                wrkCalCode = GetCalendarCode(wrkCal)
                Me.Calendar.SelectedItem = "Regular Order"
                Me.OrderOfBusiness.SelectedIndex = 0
                Calendar_Bill(Me.Calendar.Text)
                Calendar.SelectedIndex = 0
                Calendar_Click(sender, e)
            End If

            '--- if motion calendar (code 9) is selected, then change caption on current bill field
            If UCase(Me.CurrentCalendar.Text) = UCase("MOTIONS") Then
                lblCurrentBill.Text = "Current Motion"
                Me.lblBills.Text = "Motions"
            Else
                lblCurrentBill.Text = "Current Bill"
                Me.lblBills.Text = "Bills For The Current Calendar"
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Failed Select The Calendar")
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub Calendar_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar.SelectedIndexChanged
        If tPage0 Then
            Me.WorkData.Text = ""
        ElseIf tPage1 Then
            Me.WorkData1.Text = ""
        ElseIf tPage2 Then
            Me.WorkData2.Text = ""
        End If
    End Sub

    Private Sub Calendar_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Calendar.MouseDown
        Dim tmpC As String = ""
        Dim tmpCCode As String = ""
        Static j As Integer

        If tPage0 Then
            If (wrkCalCode <> OwrkCalCode) And (wrkCal = OwrkCal) And wrkCalCode = "SOC" Then
                tmpCCode = "SOC"
            Else
                tmpCCode = gCalendarCode
            End If
        ElseIf tPage1 Then
            If (wrkCalCode1 <> OwrkCalCode1) And (wrkCal1 = OwrkCal1) And wrkCalCode1 = "SOC" Then
                tmpCCode = "SOC"
            Else
                tmpCCode = gCalendarCode
            End If
        ElseIf tPage2 Then
            If (wrkCalCode2 <> OwrkCalCode2) And (wrkCal2 = OwrkCal2) And wrkCalCode2 = "SOC" Then
                tmpCCode = "SOC"
            Else
                tmpCCode = gCalendarCode
            End If
        End If

        '--- when mouse down record original information data, after delete Special order calendar, put original data back 
        If j = 0 Then
            If tPage0 Then
                OwrkOOB = Me.OrderOfBusiness.SelectedItem
                OwrkCal = Me.Calendar.SelectedItem
                OwrkCalCode = tmpCCode
                Owork_bill = Me.CurrentBill.Text
                Owork_area_text = Me.WorkData.Text
            ElseIf tPage1 Then
                OwrkOOB1 = Me.OrderOfBusiness.SelectedItem
                OwrkCal1 = Me.Calendar.SelectedItem
                OwrkCalCode1 = tmpCCode
                Owork1_bill = Me.CurrentBill.Text
                Owork_area1_text = Me.WorkData1.Text
            ElseIf tPage2 Then
                OwrkOOB2 = Me.OrderOfBusiness.SelectedItem
                OwrkCal2 = Me.Calendar.SelectedItem
                OwrkCalCode2 = tmpCCode
                Owork2_bill = Me.CurrentBill.Text
                Owork_area2_text = Me.WorkData2.Text
            End If
        End If
        j = j + 1

        If e.Button = Windows.Forms.MouseButtons.Right Then
            rightMouse = True
            isDeleteSOC = True
            tmpC = Me.Calendar.SelectedItem.ToString
            Select Case Strings.Left(UCase(tmpC), 2)
                Case "SR"
                    If MsgBox("Are you sure want to delete selected " & Me.Calendar.SelectedItem.ToString & "?", vbYesNo, "Delete Special Order Calendar") = vbYes Then
                        V_DataSet("Delete From tblCalendars Where   UCASE(Calendar) ='" & UCase(Calendar.SelectedItem.ToString) & "'", "D")
                        V_DataSet("Delete From tblSpecialOrderCalendar Where UCASE(CalendarCode) ='" & UCase(Calendar.SelectedItem.ToString) & "'", "D")
                        If tPage0 Then
                            If displaied_work Then
                                TC.TabPages(0).BackColor = Color.Green
                                TC.TabPages(0).Text = "Displayed"
                                Me.WorkData.Text = ""
                            Else
                                TC.TabPages(0).BackColor = Color.Red
                                TC.TabPages(0).Text = "Work Area"
                            End If
                        End If
                        If tPage1 Then
                            If displaied_work1 Then
                                TC.TabPages(1).BackColor = Color.Green
                                TC.TabPages(1).Text = "Displayed"
                            Else
                                TC.TabPages(1).BackColor = Color.Red
                                TC.TabPages(1).Text = "Area 1"
                            End If
                        End If
                        If tPage2 Then
                            If displaied_work2 Then
                                TC.TabPages(2).BackColor = Color.Green
                                TC.TabPages(2).Text = "Displayed"
                            Else
                                TC.TabPages(2).BackColor = Color.Red
                                TC.TabPages(2).Text = "Area 2"
                            End If
                        End If

                        Me.Calendar.Items.Remove(Me.Calendar.Text)
                        Me.OrderOfBusiness.SelectedIndex = 0
                        Me.Calendar.SelectedIndex = 0
                        Me.CurrentCalendar.Text = ""
                        Me.Calendar_Click(Calendar.Text, New System.EventArgs())
                        Me.CurrentOrderOfBusiness.Text = ""
                        Me.CurrentBill.Text = ""
                        ' Me.OrderOfBusiness_Click(OrderOfBusiness, New System.EventArgs())
                        Me.Bill_Click(Bill.Text, New System.EventArgs())
                        isDownLoadBillorSOC = False
                        isDeleteSOC = True
                    Else
                        isDeleteSOC = False
                        ' GoTo continuProcess
                    End If
                Case ("SP")
                    If MsgBox("Are you sure want to delete selected " & Me.Calendar.SelectedItem.ToString & "?", vbYesNo, "Delete Special Order Calendar") = vbYes Then
                        '--- Delete Special Order Calendar'
                        V_DataSet("Delete From tblCalendars Where  CalendarCode ='SOC'", "D")
                        V_DataSet("Delete From tblSpecialOrderCalendar Where UCASE(BillNbr) ='SPECIAL ORDER CALENDAR'", "D")
                        If tPage0 Then
                            If displaied_work Then
                                TC.TabPages(0).BackColor = Color.Green
                                TC.TabPages(0).Text = "Displayed"
                                Me.WorkData.Text = ""
                            Else
                                TC.TabPages(0).BackColor = Color.Red
                                TC.TabPages(0).Text = "Work Area"
                            End If
                        End If
                        If tPage1 Then
                            If displaied_work1 Then
                                TC.TabPages(1).BackColor = Color.Green
                                TC.TabPages(1).Text = "Displayed"
                            Else
                                TC.TabPages(1).BackColor = Color.Red
                                TC.TabPages(1).Text = "Area 1"
                            End If
                        End If
                        If tPage2 Then
                            If displaied_work2 Then
                                TC.TabPages(2).BackColor = Color.Green
                                TC.TabPages(2).Text = "Displayed"
                            Else
                                TC.TabPages(2).BackColor = Color.Red
                                TC.TabPages(2).Text = "Area 2"
                            End If
                        End If

                        Me.Calendar.Items.Remove("Special Order Calendar")
                        Me.OrderOfBusiness.SelectedIndex = 0
                        Me.Calendar.SelectedIndex = 0
                        Me.CurrentCalendar.Text = ""
                        Me.Calendar_Click(Calendar.Text, New System.EventArgs())
                        Me.CurrentOrderOfBusiness.Text = ""
                        Me.CurrentBill.Text = ""
                        Me.Bill_Click(Bill.Text, New System.EventArgs())
                        isDownLoadBillorSOC = False
                        isDeleteSOC = True
                        isDownLoadBillorSOC = False
                    Else
                        isDeleteSOC = False
                        'GoTo continuProcess
                    End If
            End Select

            'continuProcess:  '--- if canceled or delete action, reload data to each workarea
            ReloadWindows()
            btnUpdateDisplay_Click(btnUpdateDisplay, Nothing)
        End If
        Me.SOCNbr.Text = ""

        If tmpC = "Motions" Then
            If tPage0 Then
                gCalendar = "M"
            ElseIf tPage1 Then
                gCalendar = "M"
            ElseIf tPage2 Then
                gCalendar = "M"
            End If
        End If
        isDeleteSOC = True
        isDownLoadBillorSOC = False
        straightSOC = False
    End Sub

    Private Sub initialAfterDeleteSOC()
        LoadCalendar()
        Me.Calendar.SelectedItem = "Regular Order"
        Me.CurrentCalendar.Text = "Regular Order"
        Me.OrderOfBusiness.SelectedItem = ""
        Me.CurrentOrderOfBusiness.Text = ""
        Me.CurrentBill.Text = ""
        Calendar_Bill(Me.Calendar.Text)
        Calendar_Click(Calendar, Nothing)
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
                Dim mq, mq1 As New MessageQueue
                Dim msg, msg1 As New Message

                Try
                    SVotePC_On = True
                    mq.Path = gSendQueueToVotePC           '".\PRIVATE\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE\SenateVoteQueue"
                    msg.Priority = MessagePriority.Normal
                    msg.Label = "CANCEL"
                    msg.Body = Me.txtVoteID.Text
                    mq.Send(msg)
                Catch
                    Dim p As New Process
                    p = Process.Start(gCmdFile)
                    p.WaitForExit()

                    Try
                        SVotePC_On = True
                        mq1.Path = gSendQueueToVotePC           '".\PRIVATE\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE\SenateVoteQueue"
                        msg1.Priority = MessagePriority.Normal
                        msg1.Label = "Cancel"
                        msg1.Body = Me.txtVoteID.Text
                        mq1.Send(msg)
                    Catch ex As Exception
                        DisplayMessage("Network error! Unable send 'Cancel Vote' message to Voting Machine.", "Unable Send 'Cancel Vote' Message", "S")
                        Exit Sub
                    Finally
                        mq1.Close()
                        msg1.Dispose()
                    End Try
                Finally
                    mq.Close()
                    msg.Dispose()
                End Try
            End If
        End If
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
        ds.Dispose()
    End Sub

    Private Sub btnPhraseMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPhraseMaintenance.Click
        Me.Close()
        Dim frmPL As New frmPhrasesList
        frmPL.MdiParent = frmMain
        frmPL.Show()
        frmPL.BringToFront()
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
        ds.Dispose()
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
            Kill(gLocalHTMLPage)
        End If
        If System.IO.File.Exists(FileToCopy) = True Then
            System.IO.File.Copy(FileToCopy, NewCopy)
        End If
    End Sub

    Private Sub btnUpdateDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateDisplay.Click
        Dim strWorkArea As String = ""

        Try
            '--- 2 send data information to Order of Business PC
            If SOOB_On Then

                If tPage0 Then
                    strWorkArea = WorkData.Text
                    displaied_work = True
                    displaied_work1 = False
                    displaied_work2 = False
                    TC.TabPages(0).BackColor = Color.Green
                    TC.TabPages(1).BackColor = Color.Red
                    TC.TabPages(2).BackColor = Color.Red
                    TC.TabPages(0).Text = "Displayed"
                    TC.TabPages(1).Text = "Area 1"
                    TC.TabPages(2).Text = "Area 2"

                    tmpDisplayedData(0, "Y", Me.WorkData.Text)
                ElseIf tPage1 Then
                    strWorkArea = WorkData1.Text
                    displaied_work = False
                    displaied_work1 = True
                    displaied_work2 = False

                    TC.TabPages(0).BackColor = Color.Red
                    TC.TabPages(1).BackColor = Color.Green
                    TC.TabPages(2).BackColor = Color.Red

                    TC.TabPages(0).Text = "Work Area"
                    TC.TabPages(1).Text = "Displayed"
                    TC.TabPages(2).Text = "Area 2"

                    tmpDisplayedData(1, "Y", Me.WorkData1.Text)
                ElseIf tPage2 Then
                    strWorkArea = WorkData2.Text
                    displaied_work = False
                    displaied_work1 = False
                    displaied_work2 = True

                    TC.TabPages(0).BackColor = Color.Red
                    TC.TabPages(1).BackColor = Color.Red
                    TC.TabPages(2).BackColor = Color.Green

                    TC.TabPages(0).Text = "Work Area"
                    TC.TabPages(1).Text = "Area 1"
                    TC.TabPages(2).Text = "Displayed"
                    tmpDisplayedData(2, "Y", Me.WorkData2.Text)
                End If


                '--- 1 check bill was selected  when current order of business is not 'Committee Reports' or 'Introduction of Bills' 
                If gFreeFormat = False Then
                    'If UCase(Me.CurrentOrderOfBusiness.Text) = "BILLS ON THIRD READING" And Me.CurrentBill.Text = "" Then
                    '    RetValue = DisplayMessage("Please select a bill to display", "No Bill Selected", "S")
                    '    Exit Sub
                    'End If
                End If

                SendMessageToOOB(strWorkArea)
                Me.lblWorkAreaDisplayed.Visible = True
                btnCreateHTMLPage.BackColor = Color.Red
            Else
                DisplayMessage("Display Board PC is offline. Unable display Order Of Business!", "Display Board PC is offline", "I")
            End If

            tmp_DataSet("U")

        

            '--- 3 write work data to tblBills and update tblWork WorkData field
            UpdateBillWorkData()

            '--- 4 create HTML page ---'
            gCreateHTMLPage = True
            If UCase(Me.CurrentOrderOfBusiness.Text) = "ADJOURNMENT" Or UCase(Me.CurrentOrderOfBusiness.Text) = "CONVENE" Or UCase(Me.CurrentOrderOfBusiness.Text) = "RECESS" Or UCase(Me.CurrentOrderOfBusiness.Text) = "SINE DIE" Or UCase(Me.CurrentOrderOfBusiness.Text) = "HOUSE MESSAGES" Then
                CreateHTMLPage()
            Else
                CreateHTMLPage()
            End If

            '--- 5 copy HTML file to local
            CopyHTMLPageToLocal()

            '--- 6 initial parameters
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

            If SVotePC_On = False Then
                V_DataSet("Update tblVotingParameters Set ParameterValue ='" & gLegislativeDay & "' Where Parameter = 'LastLegislativeDay'", "U")
                V_DataSet("Update tblVotingParameters Set ParameterValue ='" & gSessionID & "' Where Parameter = 'LastSessionID'", "U")
                V_DataSet("Update tblVotingParameters Set ParameterValue ='" & gSessionName & "' Where Parameter = 'LastSessionName'", "U")
            End If

            Me.btnExit.Enabled = False
            Me.lblWorkAreaDisplayed.Visible = True
            gFreeFormat = False
            btnCreateHTMLPage.BackColor = Color.Green
        Catch ex As Exception
            DisplayMessage(ex.Message, "Failed Create HTML Page.", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub SendMessageToOOB(ByVal strWorkArea As String, Optional ByVal strOOB As String = "")
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

            If Me.OrderOfBusiness.SelectedItem.ToString <> "" Or UCase(Me.CurrentCalendar.Text) <> "MOTIONS" Then
                ' If Me.OrderOfBusiness.Text <> "" Then
                If strOOB <> "ORDER OF BUSINESS" And strOOB <> "WELCOME" Then
                    If UCase(Me.OrderOfBusiness.Text) = "ADJOURNMENT" Or UCase(Me.OrderOfBusiness.Text) = "CONVENE" Or UCase(Me.OrderOfBusiness.Text) = "RECESS" Or UCase(Me.OrderOfBusiness.Text) = "SINE DIE" Then
                        strBody = "gBillCalendarPage - ||" & "gSenatorSubject - " & Me.CurrentBill.Text & "||" & "gWorkArea - " & strWorkArea & "||" & "gCOOB - " & Me.CurrentOrderOfBusiness.Text
                    Else
                        If UCase(Me.Calendar.SelectedItem.ToString) <> "CONFIRMATIONS" Then
                            Select Case UCase(Me.CurrentOrderOfBusiness.Text)
                                Case "INTRODUCTION OF BILLS", "HOUSE MESSAGES", "MOTIONS AND RESOLUTIONS", "BILLS ON THIRD READING", ""
                                    If UCase(Me.Calendar.SelectedItem.ToString) <> "OTHER" Then
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
            Else
                strBody = "gBillCalendarPage - " & Me.CurrentBill.Text & " ||" & "gSenatorSubject -  ||" & "gWorkArea - " & strWorkArea & "||" & "gCOOB - "
                ' strBody = "gBillCalendarPage - " & Me.CurrentBill.Text & " ||" & "gSenatorSubject -  ||" & "gWorkArea - " & strWorkArea & "||" & "gCOOB - " & Me.CurrentOrderOfBusiness.Text
            End If


            Dim mq, mq1 As New MessageQueue
            Dim msg, msg1 As New Message

            Try
                mq.Path = gSendQueueToOOB                '".\PRIVATE\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE\SenateVoteQueue"
                msg.Priority = MessagePriority.Normal
                msg.Label = "UPDATE"
                msg.Body = strBody
                mq.Send(msg)
                SOOB_On = True
                mq.Close()
            Catch
                Try
                    Dim p As New Process
                    p = Process.Start(gCmdFile)
                    p.WaitForExit()

                    mq1.Path = gSendQueueToOOB                '".\PRIVATE\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE\SenateVoteQueue"
                    msg1.Priority = MessagePriority.Normal
                    msg1.Label = "UPDATE"
                    msg1.Body = strBody
                    mq1.Send(msg)
                    SOOB_On = True
                    mq1.Close()
                Catch ex As Exception
                    SOOB_On = False
                    If DisplayMessage(ex.Message, "Failed Send Updated Information to Dispaly Board PC." & "Would you want to continue without display Order Of Business?", "Y") Then
                        Exit Sub
                    Else
                        End
                    End If
                End Try
            Finally
                mq.Close()
                mq1.Close()
                msg.Dispose()
                msg1.Dispose()
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
        Dim mq, mq1 As New MessageQueue
        Dim msg, msg1 As New Message

        If strOOB = "CLEAR DISPLAY" Then
            strBody = "gBillCalendarPage - '' ||" & "gSenatorSubject - '' ||" & "gWorkArea - '' ||" & "gCOOB - " & "CLEAR DISPLAY"
        ElseIf strOOB = "START" Then
            strBody = "gCOOB - " & Me.CurrentOrderOfBusiness.Text
        ElseIf strOOB = "CLEAR OOB" Then
            strBody = "gCOOB - " & "CLEAR OOB"
        End If

        Try
            mq.Path = gSendQueueToOOB                               '".\PRIVATE\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE\SenateVoteQueue"
            msg.Priority = MessagePriority.Normal
            msg.Label = "UPDATE"
            msg.Body = strBody
            mq.Send(msg)
            SOOB_On = True
            mq.Close()
        Catch
            Try
                Dim p As New Process
                p = Process.Start(gCmdFile)
                p.WaitForExit()

                mq1.Path = gSendQueueToOOB                               '".\PRIVATE\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE\SenateVoteQueue"
                msg1.Priority = MessagePriority.Normal
                msg1.Label = "UPDATE"
                msg1.Body = strBody
                mq1.Send(msg1)
                SOOB_On = True
                mq1.Close()
            Catch ex As Exception
                SOOB_On = False
                If DisplayMessage(ex.Message, "Failed Send 'Clear Dispaly' message." & "Would you want to continue without show 'Order Of Business'?", "Y") Then
                    Exit Sub
                Else
                    End
                End If
            Finally
                mq1.Close()
                msg1.Dispose()
            End Try
        Finally
            mq.Close()
            msg.Dispose()
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
                    strSQL = "SELECT tblBills.CalendarCode, tblCalendars.Calendar, tblBills.Bill, tblBills.Sponsor, tblBills.Subject, tblBills.WorkData, tblBills.BillNbr " & _
                       " FROM tblCalendars LEFT JOIN tblBills ON tblCalendars.CalendarCode = tblBills.CalendarCode " & _
                       " WHERE (((tblBills.CalendarCode)=[tblCalendars].[CalendarCode])  AND ((UCase(tblBills.Bill))='" & UCase(gBill) & "'))"
                End If
            End If
            ds = V_DataSet(strSQL, "R")

            For Each dr In ds.Tables(0).Rows
                If Strings.Left(gCalendarCode, 2) <> "SR" And Strings.Left(gCalendarCode, 3) <> "SOC" Then
                    gCalendarCode = NToB(UCase(dr("CalendarCode")))
                Else
                    gCalendarCode = gCalendarCode
                End If
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
            ds.Dispose()
        Catch ex As Exception
            DisplayMessage(ex.Message, "Bill has been Changed", "S")
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub UpdateBillWorkData()
        Dim ds, dsW As New DataSet
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

            If Strings.Left(gCalendarCode, 2) = "SR" Or Strings.Left(gCalendarCode, 3) = "SOC" Then
                If tPage0 Then
                    If CurrentWorkData <> "" Then
                        strSQL = "Update tblBills SET WorkData='" & CurrentWorkData & "'  Where ucase(Bill) ='" & UCase(work_bill) & "'"
                        V_DataSet(strSQL, "U")
                    End If

                    strSQL = "Update tblSpecialOrderCalendar SET WorkData ='" & CurrentWorkData & "'  Where ucase(Bill) ='" & UCase(work_bill) & "'"
                    V_DataSet(strSQL, "U")
                ElseIf tPage1 Then
                    If CurrentWorkData <> "" Then
                        strSQL = "Update tblBills SET WorkData='" & CurrentWorkData & "'  Where  ucase(Bill) ='" & UCase(work1_bill) & "'"
                        V_DataSet(strSQL, "U")
                    End If

                    strSQL = "Update tblSpecialOrderCalendar SET WorkData ='" & CurrentWorkData & "'  Where  ucase(Bill) ='" & UCase(work1_bill) & "'"
                    V_DataSet(strSQL, "U")
                ElseIf tPage2 Then
                    If CurrentWorkData <> "" Then
                        strSQL = "Update tblBills SET WorkData='" & CurrentWorkData & "'  Where ucase(Bill) ='" & UCase(work2_bill) & "'"
                        V_DataSet(strSQL, "U")
                    End If
                    strSQL = "Update tblSpecialOrderCalendar SET WorkData ='" & CurrentWorkData & "'  Where ucase(Bill) ='" & UCase(work2_bill) & "'"
                    V_DataSet(strSQL, "U")
                End If
            ElseIf Strings.Left(gCalendarCode, 3) <> "SOC" And Strings.Left(gCalendarCode, 3) <> "SR" Then
                If gBillNbr <> "" Then
                    '--- update tblBiils' WorkData
                    If tPage0 Then
                        If CurrentWorkData <> "" Then
                            strSQL = "Update tblBills SET WorkData='" & CurrentWorkData & "'  Where ucase(Bill) ='" & UCase(work_bill) & "'"
                            ds = V_DataSet(strSQL, "U")

                            strSQL = "Update tblSpecialOrderCalendar SET WorkData ='" & CurrentWorkData & "'  Where  ucase(Bill) ='" & UCase(work_bill) & "'"
                            V_DataSet(strSQL, "U")

                            dsW = V_DataSet("Select * From tblWork  Where CalendarCode='" & gCalendarCode & "' AND ucase(BillNbr)='" & UCase(gBillNbr) & "'", "R")
                            If dsW.Tables(0).Rows.Count <> 0 Then
                                strSQL = "Update tblWork SET WorkData='" & CurrentWorkData & "' Where  ucase(BillNbr) ='" & UCase(gBillNbr) & "' AND LegDay =" & gLegislativeDay
                                V_DataSet(strSQL, "U")
                            Else
                                strSQL = "Insert Into tblWork Values ('" & gCalendarCode & "', '" & gBillNbr & "', '" & CurrentWorkData & "', " & gLegislativeDay & ")"
                                V_DataSet(strSQL, "A")
                            End If
                        End If

                        strSQL = "Update tblSpecialOrderCalendar SET WorkData='" & CurrentWorkData & "' Where CalendarCode='SOC' AND ucase(Bill) ='" & UCase(work_bill) & "'"
                        V_DataSet(strSQL, "U")
                    ElseIf tPage1 Then
                        If CurrentWorkData <> "" Then
                            strSQL = "Update tblBills SET WorkData='" & CurrentWorkData & "' Where  ucase(Bill) ='" & UCase(work1_bill) & "'"
                            ds = V_DataSet(strSQL, "U")

                            strSQL = "Update tblSpecialOrderCalendar SET WorkData ='" & CurrentWorkData & "'  Where  ucase(Bill) ='" & UCase(work1_bill) & "'"
                            V_DataSet(strSQL, "U")
                        End If

                        strSQL = "Update tblSpecialOrderCalendar SET WorkData='" & CurrentWorkData & "' Where CalendarCode='SOC' AND ucase(Bill) ='" & UCase(work1_bill) & "'"
                        V_DataSet(strSQL, "U")
                    ElseIf tPage2 Then
                        If CurrentWorkData <> "" Then
                            strSQL = "Update tblBills SET WorkData='" & CurrentWorkData & "'  Where  ucase(Bill) ='" & UCase(work2_bill) & "'"
                            ds = V_DataSet(strSQL, "U")

                            strSQL = "Update tblSpecialOrderCalendar SET WorkData ='" & CurrentWorkData & "'  Where  ucase(Bill) ='" & UCase(work2_bill) & "'"
                            V_DataSet(strSQL, "U")
                        End If

                        strSQL = "Update tblSpecialOrderCalendar SET WorkData='" & CurrentWorkData & "' Where CalendarCode='SOC' AND ucase(Bill) ='" & UCase(work2_bill) & "'"
                        V_DataSet(strSQL, "U")
                    End If
                End If
            End If
            ds.Dispose()
        Catch ex As Exception
            DisplayMessage(ex.Message, "Update Bill and  Work Area Data", "S")
            Exit Sub
        Finally
            ds.Dispose()
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
        Dim ds, dsC, dsMax As New DataSet
        Dim dr As DataRow

        Try
            '--- if bill info is changed, then update db - skip if bill has been blanked out;
            '--- if freeformat or from motion calendar then dont change
            If (gFreeFormat) Or (gCalendarCode = "M") Then
                If tPage0 Then
                    work_bill = Me.CurrentBill.Text
                ElseIf tPage1 Then
                    work1_bill = Me.CurrentBill.Text
                ElseIf tPage2 Then
                    work2_bill = Me.CurrentBill.Text
                End If
                gBill = Me.CurrentBill.Text

                If UCase(Me.Bill.SelectedItem) = "TEST" Or UCase(Me.Bill.SelectedItem = "TEST.") Then
                    strSQL = "Select * From tblBills Where LTrim(Bill) =' " & LTrim(UCase(Me.CurrentBill.Text)) & "'"
                    dsC = V_DataSet(strSQL, "R")
                    If dsC.Tables(0).Rows.Count = 0 Then
                        dsMax = V_DataSet("Select Max(OID) as OID From tblBills Where CalendarCode ='M'", "R")
                        For Each drM As DataRow In dsMax.Tables(0).Rows
                            strSQL = "Insert Into tblBills (CalendarCode, BillNbr, Bill, WorkData, OID ) Values ('M', " & drM("OID") + 1 & ", '" & UCase(Me.CurrentBill.Text) & "', '"
                            If tPage0 Then
                                strSQL = strSQL & work_area_text & "', " & drM("OID") + 1 & ")"
                                work_bill = Me.CurrentBill.Text
                            End If
                            If tPage1 Then
                                strSQL = strSQL & work_area1_text & "', " & drM("OID") + 1 & ")"
                                work1_bill = Me.CurrentBill.Text
                            End If
                            If tPage2 Then
                                strSQL = strSQL & work_area2_text & "', " & drM("OID") + 1 & ")"
                                work2_bill = Me.CurrentBill.Text
                            End If
                            V_DataSet(strSQL, "A")

                            '--- reload Bill listBox
                            Me.Bill.Items.Add(Me.CurrentBill.Text)
                            Me.Bill.SelectedItem = Me.CurrentBill.Text
                            Me.Bill_Click(Bill, Nothing)
                            Exit For
                        Next
                        gBill = Me.CurrentBill.Text
                    End If
                End If
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

                strSQL = "UPDATE tblBills SET Bill = '" & Me.CurrentBill.Text & "', Subject = '" & gSubject & "'  WHERE (CalendarCode = '" & gCalendarCode & "') AND (ucase(BillNbr) = '" & UCase(gBillNbr) & "')"
                V_DataSet(strSQL, "U")
                gBill = Me.CurrentBill.Text

                '--- Reload bill list box after changed bill
                strSQL = "SELECT tblBills.BillNbr, tblBills.Bill FROM tblBills, tblCalendars WHERE tblBills.CalendarCode = tblCalendars.CalendarCode AND (tblCalendars.Calendar = '" & gCalendar & "') ORDER BY tblBills.BillNbr"
                ds = V_DataSet(strSQL, "R")
                Me.Bill.Items.Clear()
                For Each dr In ds.Tables(0).Rows
                    Me.Bill.Items.Add(dr("Bill"))
                Next
            End If
            ds.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Current Bill")
        Finally
            ds.Dispose()
            dsC.Dispose()
            dsMax.Dispose()
        End Try
    End Sub

    Private Sub FindBillNbr_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles FindBillNbr.KeyDown
        Dim FieldName(9), FieldValue(9) As Object
        Dim k, x, z As Integer
        Dim Title As String, WrkFld As String
        Dim ds, dsA As New DataSet
        Dim dr As DataRow

        Try
            If e.KeyValue = Keys.Enter Then
                If NToB(Me.FindBillNbr.Text) = "" Then
                    Exit Sub
                End If

                '---first check for bill on Access DB; if not found, then go to ALIS;
                '---update calendar and bill positions in look ups; dont go to ALIS
                '---in pure test mode
                strSQL = "SELECT  tblCalendars.Calendar AS Calendar, tblBills.* FROM tblBills, tblCalendars WHERE tblBills.CalendarCode = tblCalendars.CalendarCode AND (ucase(tblBills.BillNbr) = '" & UCase(Me.FindBillNbr.Text) & "')"
                ds = V_DataSet(strSQL, "R")
                x = ds.Tables(0).Rows.Count

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
                        z += 1
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
                        FieldValue(9) = z
                        V_DataSet("INSERT INTO tblBills VALUES ('" & FieldValue(0) & "', '" & FieldValue(1) & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "', '" & Replace(FieldValue(7), "'", "") & "', '" & Replace(FieldValue(8), "'", "") & "', " & FieldValue(9) & ")", "A")
                        Exit For
                    Next
                    '--- put in other category
                    Me.Calendar.Text = "Other"
                    Calendar_Click(sender, e)
                    Me.Bill.Text = FieldValue(2)
                    Bill_Click(Bill, Nothing)
                    FindBillNbr.Text = ""
                End If
            End If
        Catch ex As OleDb.OleDbException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Find Bill Number")
        Finally
            ds.Dispose()
            dsA.Dispose()
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

    Private Sub OrderOfBusiness_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles OrderOfBusiness.DoubleClick
        calendarClick = True
        Me.lblWorkAreaDisplayed.Visible = False
        Me.CurrentOrderOfBusiness.Text = OrderOfBusiness.SelectedItem.ToString
        Highlight = Me.OrderOfBusiness.SelectedIndex

        Dim strWorkArea As String = ""
        If tPage0 Then
            strWorkArea = WorkData.Text
            If displaied_work = True Then
                If SOOB_On Then
                    SendStartOOBToOOB("", "START")
                End If
                CreateOtherHTMLPage()
            End If
        ElseIf tPage1 Then
            strWorkArea = WorkData1.Text
            If displaied_work1 = True Then
                If SOOB_On Then
                    SendStartOOBToOOB("", "START")
                End If
                CreateOtherHTMLPage()
            End If
        ElseIf tPage2 Then
            strWorkArea = WorkData2.Text
            If displaied_work2 = True Then
                If SOOB_On Then
                    SendStartOOBToOOB("", "START")
                End If
                CreateOtherHTMLPage()
            End If
        End If
        gCreateHTMLPage = True

        If UCase(Me.OrderOfBusiness.Text) = "ADJOURNMENT" Or UCase(Me.OrderOfBusiness.Text) = "CONVENE" Or UCase(Me.OrderOfBusiness.Text) = "RECESS" Or UCase(Me.OrderOfBusiness.Text) = "SINE DIE" Then
            Me.CurrentBill.Text = Me.OrderOfBusiness.Text
        End If
    End Sub

    Private Sub OrderOfBusiness_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles OrderOfBusiness.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If SOOB_On Then
                If Me.OrderOfBusiness.SelectedItem <> "" Or UCase(Me.CurrentCalendar.Text) <> "MOTIONS" Then
                    SendStartOOBToOOB("", "CLEAR OOB")
                Else
                    If displaied_work = True And (Me.OrderOfBusiness.SelectedItem <> "" Or UCase(Me.CurrentCalendar.Text) <> "MOTIONS") Then
                        If SOOB_On Then
                            SendStartOOBToOOB("", "CLEAR MOTIONS")
                        End If
                    End If
                    If displaied_work1 = True And (Me.OrderOfBusiness.SelectedItem <> "" Or UCase(Me.CurrentCalendar.Text) <> "MOTIONS") Then
                        If SOOB_On Then
                            SendStartOOBToOOB("", "CLEAR MOTIONS")
                        End If
                    End If
                    If displaied_work2 = True And (Me.OrderOfBusiness.SelectedItem <> "" Or UCase(Me.CurrentCalendar.Text) <> "MOTIONS") Then
                        If SOOB_On Then
                            SendStartOOBToOOB("", "CLEAR MOTIONS")
                        End If
                    End If
                    ' SendStartOOBToOOB("", "CLEAR MOTIONS")
                End If
            End If
            CreateClearHTMLPage()
        End If
    End Sub

    Private Sub OrderOfBusiness_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderOfBusiness.SelectedIndexChanged
        calendarClick = True
        Me.lblWorkAreaDisplayed.Visible = False
        Me.CurrentOrderOfBusiness.Text = OrderOfBusiness.SelectedItem.ToString
        Highlight = Me.OrderOfBusiness.SelectedIndex

        Dim strWorkArea As String = ""

        If tPage0 Then
            strWorkArea = WorkData.Text
            If displaied_work = True Then
                If SOOB_On Then
                    SendStartOOBToOOB("", "START")
                End If
                CreateOtherHTMLPage()
            End If
            wrkOOB = Me.OrderOfBusiness.Text
        ElseIf tPage1 Then
            strWorkArea = WorkData1.Text
            If displaied_work1 = True Then
                If SOOB_On Then
                    SendStartOOBToOOB("", "START")
                End If
                CreateOtherHTMLPage()
            End If
            wrkOOB1 = Me.OrderOfBusiness.Text
        ElseIf tPage2 Then
            strWorkArea = WorkData2.Text
            If displaied_work2 = True Then
                If SOOB_On Then
                    SendStartOOBToOOB("", "START")
                End If
                CreateOtherHTMLPage()
            End If
            wrkOOB2 = Me.OrderOfBusiness.Text
        End If
        gCreateHTMLPage = True

        If UCase(Me.OrderOfBusiness.Text) = "ADJOURNMENT" Or UCase(Me.OrderOfBusiness.Text) = "CONVENE" Or UCase(Me.OrderOfBusiness.Text) = "RECESS" Or UCase(Me.OrderOfBusiness.Text) = "SINE DIE" Then
            Me.CurrentBill.Text = Me.OrderOfBusiness.Text
        End If

        If Me.Calendar.SelectedItem = "Special Order Calendar" Or UCase(Mid(Me.Calendar.SelectedItem, 1, 2)) = "SR" Or UCase(Mid(Me.Calendar.SelectedItem, 1, 2)) = "SJ" Or UCase(Mid(Me.Calendar.SelectedItem, 1, 2)) = "HR" Or UCase(Mid(Me.Calendar.SelectedItem, 1, 2)) = "HJ" Then
            Bill_Click(Bill, Nothing)
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

            '--- check file is existing or not, otherwise create a new
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
            Print(1, "  </div>      <!-- end div content -->")
            Print(1, " <br class='clearfix' />")
            Print(1, "</div> <!-- </div> end div page -->")
            Print(1, "  <!-- End Insertion Point-->")
            Print(1, "</body>")
            Print(1, "</html>")
            FileClose(1)
        Catch ex As Exception
            FileClose(1)
            DisplayMessage(ex.Message, "Create HTML Page Failed!", "S")
            Exit Sub
        Finally
            FileClose(1)
            ds.Dispose()
            dsC.Dispose()
        End Try
    End Sub

    Private Sub CreateOtherHTMLPage()
        Dim wrkFld As String = ""

        If FileExistes(gHTMLFile) = False Then
            File.Create(gHTMLFile)
        Else
            Kill(gHTMLFile)             '--make sure close the .htm file first
        End If
        FileOpen(1, gHTMLFile, OpenMode.Output)

        Try
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
        Finally
            FileClose(1)
        End Try
    End Sub

    Private Sub CreateWelcomeHTMLPage()
        If FileExistes(gHTMLFile) = False Then
            File.Create(gHTMLFile)
        Else
            Kill(gHTMLFile)             '--make sure close the .htm file first
        End If

        FileOpen(1, gHTMLFile, OpenMode.Output)     '--- open to write

        Try
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
            FileClose(1)         '--- close file
        Catch
            DisplayMessage("Faild To Create Welcome HTML Page.", "Create Welcome HTML Page", "S")
            Exit Sub
        Finally
            FileClose(1)
        End Try
    End Sub

    Public Sub Calendar_Bill(ByVal strCalendar As String)
        Dim ds As New DataSet
        Dim dr As DataRow

        If tPage0 Then
            WorkData.Text = ""
        ElseIf tPage1 Then
            WorkData1.Text = ""
        ElseIf tPage2 Then
            WorkData2.Text = ""
        End If

        UpdateWorkDataSW = False
        gCalendar = strCalendar
        lblWorkAreaDisplayed.Visible = False            ' reset in case it was on
        CurrentCalendar.Text = strCalendar

        Try
            If isDownLoadBillorSOC = False And isDeleteSOC = False Then
                strSQL = "SELECT t.BillNbr, t.Bill FROM tblBills t, tblCalendars tC WHERE t.CalendarCode=tC.CalendarCode AND t.CalendarCode='1' Order By t.BillNbr"
            Else
                strSQL = "SELECT t.BillNbr, t.Bill FROM tblBills t, tblCalendars tC WHERE t.CalendarCode=tC.CalendarCode AND tC.Calendar='" & strCalendar & "' Order By t.BillNbr"
            End If

            ds = V_DataSet(strSQL, "R")
            For Each dr In ds.Tables(0).Rows
                Bill.Items.Add(dr("Bill"))
            Next

            Me.CurrentBill.Text = ""

            '--- if motion calendar (code 9) is selected, then change caption on
            '--- current bill field
            If gCalendar = "Motions" Then
                Me.lblCurrentBill.Text = "Current Motion"
                Me.lblBills.Text = "Motions"
                If InStr(UCase(gBill), "TEST") > 0 Then
                    Me.CurrentBill.Text = gBill
                End If
            Else
                Me.lblCurrentBill.Text = "Current Bill"
                Me.lblBills.Text = "Bills For The Current Calendar"
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Calendar and Bill")
        Finally
            ds.Dispose()
        End Try
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
        Catch
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
                If DisplayMessage(em.Message & " Syetem can not communicate to Vote PC! Would you want to continue?", "Send Message to Vote PC", "Y") Then
                    Exit Sub
                Else
                    End     '---exit Voting System
                End If
            Finally
                ds.Dispose()
            End Try
        Finally
            msg.Dispose()
            mq.Close()
            ds.Dispose()
        End Try
    End Sub

    Private Sub SendMessageForVoteID(ByVal label As String, ByVal strBody As String)
        Dim mq As New MessageQueue
        Dim msg As New Message
        Dim ds As New DataSet
        Dim mPath As String = ""

        ''---Production
        If gTestMode = False Then
            mPath = "FormatName:DIRECT=OS:SENATEVOTING\PRIVATE$\requestvoteidqueue"
        Else
            mPath = "FormatName:DIRECT=OS:SENATEVOTING\PRIVATE$\requestvoteidqueuetest"
        End If
        'If gTestMode = False Then
        '    mPath = "FormatName:DIRECT=OS:ImagePC031\PRIVATE$\requestvoteidqueue"
        'Else
        '    mPath = "FormatName:DIRECT=OS:ImagePC031\PRIVATE$\requestvoteidqueuetest"
        'End If

        Try
            SVotePC_On = True
            mq.Path = mPath           '".\PRIVATE\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE\SenateVoteQueue"
            msg.Priority = MessagePriority.Normal
            msg.Label = label
            msg.Body = strBody
            mq.Send(msg)

            EnableControls()
            mq.Close()
        Catch
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
                If DisplayMessage(em.Message & " Syetem can not communicate to Vote PC! Would you want to continue?", "Send Message to Vote PC", "Y") Then
                    mq.Close()
                    Exit Sub
                Else
                    End
                End If
            Finally
                msg.Dispose()
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
                    V_DataSet("Insert Into tblOnlyOnePC Values (1, '" & SDIS & "','', '')", "A")

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
        ReceiveMessageFromQueue()
    End Sub

    Private Sub ReceiveMessageFromQueue()
        Dim strText As String = ""

        Try
            '--- gReceiveQueueName = ".\PRIVATE$\senatevotequeue"
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

                        strSQL = "Update tblVotingParameters Set ParameterValue='" & txtVoteID.Text & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSION'"
                        V_DataSet(strSQL, "U")
                    Else
                        DisplayMessage("Incorrect voting data! Please request correct voting data.", "Request correct voting data", "S")
                        SendMessageToVotePCQueue("RECALL", "")
                    End If
                ElseIf m.Label = "LASTVOTEDID" Then
                    m = queue.Receive(New TimeSpan(1000))
                    Me.lblChamberLight.BackColor = Color.LimeGreen
                    If Me.btnVote.BackColor = Color.Red Then
                        Me.btnVote.BackColor = Color.LimeGreen
                        Me.btnVote.Enabled = True
                    End If
                    Me.btnUpdateDisplay.Enabled = True
                    Me.lblChamberLight.Text = "Display Next Bill"
                    gVotingStarted = False
                    If Me.txtVoteID.Text = m.Body Then
                        Me.txtVoteID.Text = Val(m.Body)
                        iniVoteID = Val(m.Body)
                        gVoteID = Val(m.Body)
                    Else
                        Me.txtVoteID.Text = Val(m.Body) + 1
                        iniVoteID = Val(m.Body) + 1
                        gVoteID = Val(m.Body) + 1
                    End If
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
                    EnableControls()
                    gVotingStarted = False
                    queue.Close()
                End If
                SVotePC_On = True
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
        End If
    End Sub

    Private Sub SOCNbr_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles SOCNbr.KeyDown
        Dim ds, dsA, dsDoc As New DataSet
        Dim strSocLabel As String = ""
        Dim drA As DataRow
        Dim j, docID, OID As Integer
        Dim FieldValue() As Object
        Dim Title As String

        Try
            If e.KeyCode = Keys.Enter And SOCNbr.Text <> "" Then
                SOCNbr.AcceptsTab = True

                '--- skip if pure test mode
                If (gTestMode) And (Not gWriteVotesToTest) Then
                    Exit Sub
                End If

                If NToB(Me.SOCNbr.Text) = "" Then
                    Exit Sub
                End If

                If tPage0 Then
                    tPage0 = True
                    tPage1 = False
                    tPage2 = False
                ElseIf tPage1 Then
                    tPage0 = False
                    tPage1 = True
                    tPage2 = False
                ElseIf tPage2 Then
                    tPage0 = False
                    tPage1 = False
                    tPage2 = True
                End If

                strSocLabel = UCase(Trim(Me.SOCNbr.Text))

                '--- check if input is document id
                docID = InStr(strSocLabel, "-")

                '--- input Resolution label SR98
                If Strings.Left(strSocLabel, 3) <> "SOC" Then
                    If Strings.Left(strSocLabel, 2) = "SR" And docID = 0 Then
                        '--- Case 1: input is SR existing ALIS

                        strSQL = "SELECT label, oid_current_document_version " & _
                                  " FROM ALIS_OBJECT " & _
                                  " WHERE (oid_session = " & gSessionID & ") AND (oid_house_of_origin = 1753) AND (status_code = '8') AND (instrument = 'T') " & _
                                  " AND (last_transaction_date IS NOT NULL) AND EXISTS (SELECT 1 FROM Document_Section S , Special_Order_Calendar_Item SOC " & _
                                  " WHERE SOC.OID_Resolution_Clause = S.OID AND OID_Current_Document_Version = S.OID_Document_Version) AND (label = '" & strSocLabel & "')"
                        dsA = ALIS_DataSet(strSQL, "R")

                        If dsA.Tables(0).Rows.Count = 0 Then
                            RetValue = DisplayMessage("This special order calendar was not found in ALIS", "Calendar Not Found", "S")
                            dsA.Dispose()
                            Me.SOCNbr.Text = ""
                            Exit Sub
                        Else
                            gCalendarCode = strSocLabel
                            gBillNbr = ""
                            '--- first check for selected calendar exist or not; if not found, then go to ALIS;
                            strSQL = "Select CalendarCode From tblCalendars Where CalendarCode='" & strSocLabel & "'"
                            ds = V_DataSet(strSQL, "R")
                            If ds.Tables(0).Rows.Count > 0 Then
                                DisplayMessage("You have already downloaded this special order calendar.", "Download Special Order Calendar From ALIS", "S")
                                GoTo continuProcess
                            End If
                        End If
                    End If

                    '--- input Resolution document ID - 17o132-1
                    If docID > 0 And Strings.Left(strSocLabel, 2) <> "SR" Then
                        '--- Case 2: input is Document ID existing ALIS
                        strSQL = " SELECT a.oid_current_document_version, a.oid, a.label, a.id,a.last_transaction_date , d.label " & _
                                  " FROM alis_object a, document_version d " & _
                                  " WHERE a.oid_session =" & gSessionID & " AND" & _
                                         " a.OID_CURRENT_DOCUMENT_VERSION  = d.oid AND" & _
                                         " d.label ='" & Replace(strSocLabel, " ", "") & "'"
                        dsDoc = ALIS_DataSet(strSQL, "R")

                        If dsDoc.Tables(0).Rows.Count = 0 Then
                            RetValue = DisplayMessage("This special order calendar was not found in ALIS", "Calendar Not Found", "S")
                            dsDoc.Dispose()
                            Me.SOCNbr.Text = ""
                            Exit Sub
                        Else
                            For Each drDoc In dsDoc.Tables(0).Rows
                                strSocLabel = drDoc("Label")
                                gCalendarCode = drDoc("Label")
                                gBillNbr = ""
                                strSQL = "Select CalendarCode From tblCalendars Where CalendarCode='" & strSocLabel & "'"
                                ds = V_DataSet(strSQL, "R")
                                If ds.Tables(0).Rows.Count > 0 Then
                                    DisplayMessage("You have already downloaded this special order calendar.", "Download Special Order Calendars From ALIS", "S")
                                    GoTo continuProcess
                                End If
                            Next
                        End If
                    End If

                    strSQL = "SELECT label, oid_current_document_version " & _
                            " FROM ALIS_OBJECT " & _
                            " WHERE (oid_session = " & gSessionID & ") AND (oid_house_of_origin = 1753) AND (status_code = '8') AND (instrument = 'T') " & _
                                     " AND (last_transaction_date IS NOT NULL) AND EXISTS (SELECT 1 FROM Document_Section S , Special_Order_Calendar_Item SOC " & _
                            " WHERE SOC.OID_Resolution_Clause = S.OID AND OID_Current_Document_Version = S.OID_Document_Version) AND (label = '" & strSocLabel & "')"
                    dsA = ALIS_DataSet(strSQL, "R")

                    If dsA.Tables(0).Rows.Count = 0 Then
                        RetValue = DisplayMessage("This special order calendar was not found in ALIS", "Calendar Not Found", "S")
                        dsDoc.Dispose()
                        Me.SOCNbr.Text = ""
                        Exit Sub
                    Else
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
                        strSQL = "SELECT A.label, A.sponsor, A.index_word, SOC.calendar_page " & _
                             " FROM SPECIAL_ORDER_CALENDAR_ITEM SOC, ALIS_OBJECT A, MATTER M " & _
                             " WHERE (OID_Resolution_Clause IN (SELECT S.OID FROM Document_Section S " & _
                             " WHERE S.OID_Document_Version =" & OID & " AND soc.oid_matter = m.oid AND m.oid_instrument = a.oid)) ORDER BY SOC.sequence_number"
                        j = 0
                        dsA = ALIS_DataSet(strSQL, "R")

                        FieldValue(0) = strSocLabel

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
                            strSQL = "Insert Into tblSpecialOrderCalendar Values ('" & FieldValue(0) & "', '" & FieldValue(1) & "','" & FieldValue(2) & "','" & FieldValue(3) & "','" & FieldValue(4) & "','','" & FieldValue(6) & "', '" & Replace(FieldValue(7), "'", "") & "', '" & Replace(FieldValue(8), "'", "") & "')"
                            V_DataSet(strSQL, "A")
                            j += 1
                        Next
                    End If
                End If

                If Strings.Left(strSocLabel, 3) = "SOC" And docID = 0 Then
                    '--- Case 3: find  "Special Oreder Calendar" is not existing in ALIS yet
                    Dim dsSP As New DataSet
                    strSocLabel = "Special Order Calendar"
                    gCalendarCode = "SOC"
                    gBillNbr = ""
                    dsSP = V_DataSet("Select * From tblCalendars Where CalendarCode='SOC'", "R")

                    If dsSP.Tables(0).Rows.Count > 0 Then
                        DisplayMessage("You have already downloaded this special order calendar.", "Download Special Order Calendars From ALIS", "S")
                        GoTo continuProcess
                    Else
                        DisplayMessage("Can not find special order calendar. Please open 'Special Order Calendar window to add.", "Load special order calendar", "S")
                        Me.SOCNbr.Text = ""
                        Exit Sub
                    End If
                End If


continuProcess:
                Dim dsNewC As New DataSet
                dsNewC = V_DataSet("Select * From tblCalendars Order By CalendarCode", "R")
                Me.Calendar.Items.Clear()
                For Each drNewC As DataRow In dsNewC.Tables(0).Rows
                    Me.Calendar.Items.Add(drNewC("Calendar"))
                Next
                Me.Calendar.SelectedItem = strSocLabel
                Me.Calendar_Click(Calendar, Nothing)

                Me.Bill.Items.Clear()
                Dim dsSPBills As New DataSet
                If strSocLabel = "Special Order Calendar" Then
                    dsSPBills = V_DataSet("Select Bill From tblSpecialOrderCalendar Where CalendarCode ='SOC' Order By BillNbr", "R")
                Else
                    dsSPBills = V_DataSet("Select Bill From tblSpecialOrderCalendar Where CalendarCode ='" & strSocLabel & "' Order By BillNbr", "R")
                End If

                For Each drSPBill As DataRow In dsSPBills.Tables(0).Rows
                    Me.Bill.Items.Add(drSPBill("Bill"))
                Next

                Me.Bill.SelectedIndex = 0
                Me.CurrentBill.Text = Me.Bill.Items(0).ToString
                Me.Bill_Click(Bill, Nothing)
                Me.CurrentCalendar.Text = strSocLabel
                Me.OrderOfBusiness.SelectedItem = ""
                Me.CurrentOrderOfBusiness.Text = ""

                If tPage0 Then
                    work_bill = Me.Bill.SelectedItem
                    work_area_text = ""
                    wrkOOB = ""
                    Owork_bill = Me.Bill.SelectedItem
                    Owork_area_text = ""
                    OwrkOOB = ""
                    Me.WorkData.Text = ""
                    WorkData_TextChanged(WorkData, Nothing)
                    If strSocLabel = "SOC" Then
                        wrkCal = "Special Order Calendar"
                        wrkCalCode = "SOC"
                        OwrkCal = "Special Order Calendar"
                        OwrkCalCode = "SOC"

                        Me.Calendar.SelectedItem = "Special Order Calendar"
                        Calendar_Click(sender, e)
                        Bill.SelectedIndex = 0
                        Bill.SelectedItem = Bill.SelectedItems(0).ToString
                        Bill_Click(sender, e)
                    Else
                        wrkCal = strSocLabel
                        wrkCalCode = strSocLabel
                        OwrkCal = strSocLabel
                        OwrkCalCode = strSocLabel
                    End If
                ElseIf tPage1 Then
                    work1_bill = Me.Bill.SelectedItem
                    Owork1_bill = Me.Bill.SelectedItem
                    work_area1_text = ""
                    Owork_area1_text = ""
                    wrkOOB1 = ""
                    OwrkOOB1 = ""
                    Me.WorkData1.Text = ""
                    If strSocLabel = "SOC" Then
                        wrkCal1 = "Special Order Calendar"
                        wrkCalCode1 = "SOC"
                        OwrkCal1 = "Special Order Calendar"
                        OwrkCalCode1 = "SOC"

                        Me.Calendar.SelectedItem = "Special Order Calendar"
                        Calendar_Click(sender, e)
                        Bill.SelectedIndex = 0
                        Bill.SelectedItem = Bill.SelectedItems(0).ToString
                        Bill_Click(sender, e)
                    Else
                        wrkCal1 = strSocLabel
                        wrkCalCode1 = strSocLabel
                        OwrkCal1 = strSocLabel
                        OwrkCalCode1 = strSocLabel
                    End If
                    WorkData1_TextChanged(WorkData1, Nothing)
                ElseIf tPage2 Then
                    work2_bill = Me.Bill.SelectedItem
                    Owork2_bill = Me.Bill.SelectedItem
                    work_area2_text = ""
                    Owork_area2_text = ""
                    wrkOOB2 = ""
                    OwrkOOB2 = ""
                    Me.WorkData2.Text = ""
                    If strSocLabel = "SOC" Then
                        wrkCal2 = "Special Order Calendar"
                        wrkCalCode2 = "SOC"
                        OwrkCal2 = "Special Order Calendar"
                        OwrkCalCode2 = "SOC"
                        Me.Calendar.SelectedItem = "Special Order Calendar"
                        Calendar_Click(sender, e)
                        Bill.SelectedIndex = 0
                        Bill.SelectedItem = Bill.SelectedItems(0).ToString
                        Bill_Click(sender, e)
                    Else
                        wrkCal2 = strSocLabel
                        wrkCalCode2 = strSocLabel
                        OwrkCal2 = strSocLabel
                        OwrkCalCode2 = strSocLabel
                    End If
                    WorkData2_TextChanged(WorkData2, Nothing)
                End If
                Me.SOCNbr.Text = ""
            End If

        Catch ex As Exception
            DisplayMessage(ex.Message, "Failed load special order calendar", "S")
            Exit Sub
        Finally
            ds.Dispose()
            dsA.Dispose()
            dsDoc.Dispose()
        End Try
    End Sub

    Private Sub btnVote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVote.Click
        ''--- put chamber display parameters in passed table so voting can pick them up; if bir then clear the work area so operator can
        ''--- start building next motion
        Try
            If Me.txtVoteID.Text = "" Then
                DisplayMessage("You must first get Vote ID to vote on.", "No Vote ID", "S")
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
                ReceiveTimer.Start()
                ReceiveTimer.Interval = gReceiveQueueTimer

                '--- pub chamber display parameer in passed table so voting can pick them up; if BIR then clear the word area so operator can
                '--- start building next motion
                askVoteID = False
                DisableControls()
                Me.OrderOfBusiness.Enabled = True
                lblChamberLight.BackColor = Color.Red
                lblChamberLight.Text = "Waiting For Voting"
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
                ReceiveTimer.Stop()
                If DisplayMessage("Senate 'Vote Computer' is offline. Unable to Vote. Would you want to continue without Vote?", "Senate Vote Computer is offline", "Y") Then
                    EnableControls()
                    btnVote.Enabled = False

                    Me.lblChamberLight.BackColor = Color.LimeGreen
                    Me.lblChamberLight.Text = "Display Next Bill"
                    gPutChamberLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green)
                    gPutVotingLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
                    gVotingStarted = True
                Else
                    End
                End If
            End If

            gVotingStarted = False
            gPutPhrase = gCurrentPhrase

            ' 2 --- Update LastVoteID to Production or Test mode
            V_DataSet("Update tblVotingParameters Set ParameterValue='" & txtVoteID.Text & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSION'", "U")
            V_DataSet("Update tblVotingParameters Set ParameterValue='" & gLegislativeDay & "' Where ucase(Parameter) ='LASTLEGISLATIVEDAY'", "U")
            V_DataSet("Update tblVotingParameters Set ParameterValue ='" & gSessionID & "' Where Parameter = 'LastSessionID'", "U")
            V_DataSet("Update tblVotingParameters Set ParameterValue ='" & gSessionName & "' Where Parameter = 'LastSessionName'", "U")


            ' 3 --- If display PC lost voting information, 
            '--- write to text file first. Display PC is able to retreive voting infromation from the text file to response to Vote PC
            Try
                Dim txtContent As String = "VoteID=" & Me.txtVoteID.Text & ";" & "BillNbr=" & BillText & ";" & "Bill=" & Me.CurrentBill.Text & ";" & "LegislativeDay=" & gLegislativeDay & ";" & "WorkArea=" & Me.WorkData.Text & ";" & "Time=" & Now & vbNewLine
                If File.Exists(gVotingPath & "Vote.txt") = False Then
                    Dim fs As New FileStream(gVotingPath & "Vote.txt", FileMode.CreateNew)
                    fs.Close()
                End If
                FileOpen(1, gVotingPath & "Vote.txt", OpenMode.Append)
                Print(1, txtContent)
                FileClose(1)
            Catch ex As Exception
                GoTo continueNext
            Finally
                FileClose(1)
            End Try


continueNext:
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
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Send Vote Process")
        End Try
    End Sub

    Private Sub btnRecallDisplayedBill_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecallDisplayedBill.Click
        Try

            Dim dsD As New DataSet
            strSQL = ""
            strSQL = "Select * From tbltmpDisplayedData"
            dsD = V_DataSet(strSQL, "R")

            For Each dr As DataRow In dsD.Tables(0).Rows
                If tPage0 Then
                    If dr("Displayed") = "Y" And dr("PageID") = 0 Then
                        Me.OrderOfBusiness.Text = dr("OOB")
                        Me.Calendar.Text = dr("Calendar")
                        Calendar_Click(dr("Calendar"), New System.EventArgs())
                        Me.Bill.Text = dr("Bill")
                        Bill_Click(dr("Bill"), New System.EventArgs())
                    Else
                        RetValue = DisplayMessage("No bill has been displayed yet", "Nothing To Recall", "S")
                    End If
                ElseIf tPage1 Then
                    If dr("Displayed") = "Y" And dr("PageID") = 1 Then
                        Me.OrderOfBusiness.Text = dr("OOB")
                        Me.Calendar.Text = dr("Calendar")
                        Calendar_Click(dr("Calendar"), New System.EventArgs())
                        Me.Bill.Text = dr("Bill")
                        Bill_Click(dr("Bill"), New System.EventArgs())
                    Else
                        RetValue = DisplayMessage("No bill has been displayed yet", "Nothing To Recall", "S")
                    End If
                ElseIf tPage2 Then
                    If dr("Displayed") = "Y" And dr("PageID") = 2 Then
                        Me.OrderOfBusiness.Text = dr("OOB")
                        Me.Calendar.Text = dr("Calendar")
                        Calendar_Click(dr("Calendar"), New System.EventArgs())
                        Me.Bill.Text = dr("Bill")
                        Bill_Click(dr("Bill"), New System.EventArgs())
                    Else
                        RetValue = DisplayMessage("No bill has been displayed yet", "Nothing To Recall", "S")
                    End If
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Recall Displayed Bill")
        End Try
    End Sub

    Private Sub btnClearDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearDisplay.Click
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
        End If

        '--- Write a empty HTML page
        CreateOtherHTMLPage()
    End Sub

    Private Sub btnClearWorkArea_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnClearWorkArea.MouseDown
        '--- before clear work area, record original data for late retrieve
        If tPage0 Then
            Owork_area_text = WorkData.Text
            work_area_text = WorkData.Text
            OwrkCal = Me.Calendar.SelectedItem
            wrkCal = Me.Calendar.SelectedItem
            wrkCalCode = wrkCalCode
            OwrkCalCode = wrkCalCode
            work_bill = Me.CurrentBill.Text
            Owork_bill = Me.CurrentBill.Text
            OwrkOOB = OrderOfBusiness.SelectedItem
            wrkOOB = OrderOfBusiness.SelectedItem
        ElseIf tPage1 Then
            Owork_area1_text = WorkData1.Text
            work_area1_text = WorkData1.Text
            OwrkCal1 = Me.Calendar.SelectedItem
            wrkCal1 = Me.Calendar.SelectedItem
            wrkCalCode1 = wrkCalCode1
            OwrkCalCode1 = wrkCalCode1
            work1_bill = Me.CurrentBill.Text
            Owork1_bill = Me.CurrentBill.Text
            OwrkOOB1 = OrderOfBusiness.SelectedItem
            wrkOOB1 = OrderOfBusiness.SelectedItem
        ElseIf tPage2 Then
            Owork_area2_text = WorkData2.Text
            work_area2_text = WorkData2.Text
            OwrkCal2 = Me.Calendar.SelectedItem
            wrkCal2 = Me.Calendar.SelectedItem
            wrkCalCode2 = wrkCalCode2
            OwrkCalCode2 = wrkCalCode2
            work2_bill = Me.CurrentBill.Text
            Owork2_bill = Me.CurrentBill.Text
            OwrkOOB2 = OrderOfBusiness.SelectedItem
            wrkOOB2 = OrderOfBusiness.SelectedItem
        End If
    End Sub

    Private Sub btnClearWorkArea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearWorkArea.Click
        Try
            If tPage0 Then
                WorkData.Text = ""
                work_area_text = ""
                strSQL = "Update tblBills SET WorkData=''  Where ucase(Bill) ='" & UCase(work_bill) & "'"
                V_DataSet(strSQL, "U")

                strSQL = "Update tblSpecialOrderCalendar SET WorkData =''  Where ucase(Bill) ='" & UCase(work_bill) & "'"
                V_DataSet(strSQL, "U")
            ElseIf tPage1 Then
                WorkData1.Text = ""
                work_area1_text = ""
                strSQL = "Update tblBills SET WorkData=''  Where  ucase(Bill) ='" & UCase(work1_bill) & "'"
                V_DataSet(strSQL, "U")

                strSQL = "Update tblSpecialOrderCalendar SET WorkData =''  Where  ucase(Bill) ='" & UCase(work1_bill) & "'"
                V_DataSet(strSQL, "U")
            ElseIf tPage2 Then
                WorkData2.Text = ""
                work_area2_text = ""
                strSQL = "Update tblBills SET WorkData=''  Where ucase(Bill) ='" & UCase(work2_bill) & "'"
                V_DataSet(strSQL, "U")

                strSQL = "Update tblSpecialOrderCalendar SET WorkData =''  Where ucase(Bill) ='" & UCase(work2_bill) & "'"
                V_DataSet(strSQL, "U")
            End If
            UpdateWorkDataSW = False
        Catch ex As Exception
            DisplayMessage("Clear work qrea job is failed! Please try it again.", "Clear Work Area", "I")
            Exit Sub
        End Try
    End Sub

    Private Sub btnCreateHTMLPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateHTMLPage.Click
        '--- This button allow for the toggling on/off for the creation of the HTML
        '--- page display the current matter. If the button is green, the HTML page
        '--- will be created. If it is red, the page will not be create.

        If Me.btnCreateHTMLPage.BackColor = System.Drawing.Color.Green Then
            Me.btnCreateHTMLPage.BackColor = System.Drawing.Color.Gray
            gCreateHTMLPage = False
            UpdateParameter("N", "CREATEHTMLPAGE")
        Else
            Me.btnCreateHTMLPage.BackColor = System.Drawing.Color.Green
            gCreateHTMLPage = True
            UpdateParameter("Y", "CREATEHTMLPAGE")
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        If UpdateWorkDataSW Then
            UpdateBillWorkData()
        End If
        '--- clear all of messages on myself Queue
        Dim mq As New MessageQueue(gSendQueueFromDisplay)
        Dim msg As New Message
        mq.Purge()

        Me.Close()
        frmMain.Show()

        If isDownLoadBillorSOC = False And straightSOC = False Then
            Me.CurrentBill.Text = ""
            Owork_area_text = ""
            work_area_text = ""
            OwrkCal = ""
            wrkCal = ""
            wrkCalCode = ""
            OwrkCalCode = ""
            work_bill = ""
            Owork_bill = ""
            OwrkOOB = ""
            wrkOOB = ""

            Owork_area1_text = ""
            work_area1_text = ""
            OwrkCal1 = ""
            wrkCal1 = ""
            wrkCalCode1 = ""
            OwrkCalCode1 = ""
            work1_bill = ""
            Owork1_bill = ""
            OwrkOOB1 = ""
            wrkOOB1 = ""

            Owork_area2_text = ""
            work_area2_text = ""
            OwrkCal2 = ""
            wrkCal2 = ""
            wrkCalCode2 = ""
            OwrkCalCode2 = ""
            work2_bill = ""
            Owork2_bill = ""
            OwrkOOB2 = ""
            wrkOOB2 = ""
        End If
    End Sub

    Private Sub btnUnlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnlock.Click
        EnableControls()
        If SVotePC_On = False Then
            btnVote.Enabled = False
            btnCancelVote.Enabled = False
        Else
            Me.btnVote.BackColor = Color.LimeGreen
            Me.btnVote.Enabled = True
        End If
        Me.OrderOfBusiness.Enabled = True
        Me.Calendar.Enabled = True
        Me.btnClearWorkArea.Enabled = True
        Me.btnClearDisplay.Enabled = True
        Me.lblChamberLight.BackColor = Color.LimeGreen
        Me.lblChamberLight.Text = "Display Next Bill"
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

    Private Function ShowBillByCalendar(ByVal strCalendar As String)
        Try
            If strCalendar <> Nothing Then
                If Me.Calendar.SelectedItem = "Special Order Calendar" Or UCase(Mid(Me.Calendar.SelectedItem, 1, 2)) = "SR" Or UCase(Mid(Me.Calendar.SelectedItem, 1, 2)) = "HR" Or UCase(Mid(Me.Calendar.SelectedItem, 1, 2)) = "SJ" Or UCase(Mid(Me.Calendar.SelectedItem, 1, 2)) = "HJ" Then
                    strSQL = "SELECT tblSpecialOrderCalendar.BillNbr, tblSpecialOrderCalendar.Bill " & _
                       " FROM tblSpecialOrderCalendar, tblCalendars  " & _
                               " WHERE tblSpecialOrderCalendar.CalendarCode = tblCalendars.CalendarCode AND " & _
                               " tblCalendars.Calendar = '" & strCalendar & "'" & _
                               " ORDER BY tblSpecialOrderCalendar.BillNbr"
                Else
                    strSQL = "SELECT tblBills.BillNbr, tblBills.Bill " & _
                       " FROM tblBills, tblCalendars  " & _
                               " WHERE tblBills.CalendarCode = tblCalendars.CalendarCode AND " & _
                               " tblCalendars.Calendar = '" & strCalendar & "'" & _
                               " ORDER BY tblBills.BillNbr"
                End If

                ShowBillByCalendar = V_DataSet(strSQL, "R")
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Chamber Display - ShowBillByCalendar()", "S")
        End Try
    End Function

    Private Sub cboDisplay_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDisplay.SelectedIndexChanged
        '--- send message to OOB PC, write HTML files
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

        '--- copy HTML file to local
        CopyHTMLPageToLocal()

        Me.cboDisplay.SelectedIndex = 0
    End Sub

    Private Sub CreateOrderBusinessHTMLPage()
        If FileExistes(gHTMLFile) = False Then
            File.Create(gHTMLFile)
        Else
            Kill(gHTMLFile)             '--- make sure close the .htm file first
        End If

        FileOpen(1, gHTMLFile, OpenMode.Output)     '--- open file to write

        Try
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
            FileClose(1)            '--- close file
        Catch ex As Exception
            DisplayMessage("Failed Create HTML Page", "Create HTML Page", "S")
            Exit Sub
        Finally
            FileClose(1)
        End Try
    End Sub

    Private Sub CreateClearHTMLPage()
        Dim wrkFld As String = ""

        If FileExistes(gHTMLFile) = False Then
            File.Create(gHTMLFile)
        Else
            Kill(gHTMLFile)                             '--- make sure close the .htm file first
        End If
        FileOpen(1, gHTMLFile, OpenMode.Output)         '--- open file to write

        Try
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
            FileClose(1)                '--- close file
        Catch ex As Exception
            DisplayMessage(ex.Message, "Create HTML Page Failed!", "S")
            Exit Sub
        Finally
            FileClose(1)
        End Try
    End Sub

    Private Sub btnStartService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartService.Click
        Dim p As New Process
        p = Process.Start(gCmdFile)
        p.WaitForExit()
    End Sub

    Private Sub CurrentBill_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CurrentBill.TextChanged
        If UCase(gCalendar) <> "MOTIONS" Then
            If tPage0 Then
                work_bill = Me.CurrentBill.Text
            End If
            If tPage1 Then
                work1_bill = Me.CurrentBill.Text
            End If
            If tPage2 Then
                work2_bill = Me.CurrentBill.Text
            End If
        End If
    End Sub

    Private Sub btnDownLoadBills_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownLoadBills.Click
        If gNetwork Then
            If DisplayMessage("You are about to download Bills from ALIS. Do you want to continue?", "Download Bills From ALIS", "Y") Then
                Try
                    Me.Cursor = Cursors.WaitCursor
                    RetValue = DownloadBillsFromALIS()
                    If gDownloadBills.Length > 0 Then
                        Me.Bill.Items.Clear()
                        For y As Integer = 0 To gDownloadBills.Length - 1
                            Me.Bill.Items.Add(gDownloadBills(y))
                        Next
                        Calendar_Click(Me.Calendar, Nothing)
                    End If
                    Me.Cursor = Cursors.Default
                Catch ex As Exception
                    MsgBox("Download Bills from ALIS is Failed! please contact to Administrator.", vbCritical, "Download Bills From ALIS")
                Finally
                    Me.Cursor = Cursors.Default
                End Try
            End If
        Else
            DisplayMessage("Connect to ALIS database failed! Unable download Bills from ALIS. Please contact to Administrator.", "Download Bills from ALIS", "I")
        End If
    End Sub

    Private Sub frmChamberDisplay_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        isDeleteSOC = False
        isDownLoadBillorSOC = False
    End Sub

    Private Sub btnSOC_Click(sender As System.Object, e As System.EventArgs) Handles btnSOC.Click
        tmp_DataSet("U")
        Me.TimerSOC.Enabled = True
        Me.TimerSOC.Interval = 10000
        Me.TimerSOC.Start()
        frmPreSOC.Show()
        frmPreSOC.MdiParent = frmMain
        frmPreSOC.BringToFront()
    End Sub

    Private Sub TimerSOC_Tick(sender As System.Object, e As System.EventArgs) Handles TimerSOC.Tick
        If isDownLoadBillorSOC Then
            Me.Calendar.Items.Remove("Special Order Calendar")
            Me.Calendar.Items.Add("Special Order Calendar")
            TimerSOC.Enabled = False
            TimerSOC.Stop()
        End If
    End Sub

    Private Function tmp_DataSet(ByVal flag As String, Optional strS As String = "") As DataSet
        Dim daT As New OleDbDataAdapter
        Dim command0, command1, command2 As OleDbCommand
        Dim dsT As New DataSet
        Dim displayed As String = "N"

        v_Rows = 0

        Try
            If connLocal.State = ConnectionState.Closed Then connLocal.Open()

            Select Case flag
                Case "A"
                    strS = "Insert Into tblTmpWorkAreasData Values(0,'', '', '','', 'N')"
                    V_DataSet(strS, "A")

                    strS = "Insert Into tblTmpWorkAreasData Values(1,'', '', '','', 'N')"
                    V_DataSet(strS, "A")

                    strS = "Insert Into tblTmpWorkAreasData Values(2,'', '', '','', 'N')"
                    V_DataSet(strS, "A")
                Case "D"
                    strS = "Delete From tblTmpWorkAreasData Where WorkAreaID =0"
                    V_DataSet(strS, "D")

                    strS = "Delete From tblTmpWorkAreasData Where WorkAreaID =1"
                    V_DataSet(strS, "D")

                    strS = "Delete From tblTmpWorkAreasData Where WorkAreaID =2"
                    V_DataSet(strS, "D")
                Case "R"
                    If tPage0 Then
                        strS = "Select * From tblTmpWorkAreasData Where WorkAreaID = 0"
                    ElseIf tPage1 Then
                        strS = "Select * From tblTmpWorkAreasData Where WorkAreaID = 1"
                    ElseIf tPage2 Then
                        strS = "Select * From tblTmpWorkAreasData Where WorkAreaID = 2"
                    End If

                    daT = New OleDbDataAdapter(strS, connLocal)
                    daT.SelectCommand = New OleDbCommand(strS, connLocal)
                    daT.Fill(dsT, "Table")

                    tmp_DataSet = dsT
                    dataProcess = True
                    daT.Dispose()
                    Return dsT
                Case "U"
                    If tPage0 Then
                        If displaied_work Then
                            displayed = "Y"
                        Else
                            displayed = "N"
                        End If
                        strS = "Update tblTmpWorkAreasData Set OOB='" & Me.OrderOfBusiness.SelectedItem & "', Calendar ='" & Me.Calendar.SelectedItem & "', Bill ='" & Me.CurrentBill.Text & "', WorkText ='" & Me.WorkData.Text & "', Displayed ='" & displayed & "' Where WorkAreaID = 0"
                    ElseIf tPage1 Then
                        If displaied_work1 Then
                            displayed = "Y"
                        Else
                            displayed = "N"
                        End If
                        strS = "Update tblTmpWorkAreasData Set OOB='" & Me.OrderOfBusiness.SelectedItem & "', Calendar ='" & Me.Calendar.SelectedItem & "', Bill ='" & Me.CurrentBill.Text & "', WorkText ='" & Me.WorkData1.Text & "', Displayed ='" & displayed & "' Where WorkAreaID = 1"
                    ElseIf tPage2 Then
                        If displaied_work2 Then
                            displayed = "Y"
                        Else
                            displayed = "N"
                        End If
                        strS = "Update tblTmpWorkAreasData Set OOB='" & Me.OrderOfBusiness.SelectedItem & "', Calendar ='" & Me.Calendar.SelectedItem & "', Bill ='" & Me.CurrentBill.Text & "', WorkText ='" & Me.WorkData2.Text & "', Displayed ='" & displayed & "' Where WorkAreaID = 2"
                    End If
                    daT = New OleDbDataAdapter(strS, connLocal)
                    command0 = New OleDbCommand(strS, connLocal)
                    daT.UpdateCommand = command0
                    daT.Fill(dsT, "Table")
                    command0.ExecuteNonQuery()
                    command0.Dispose()
                    tmp_DataSet = dsT
                    daT.Dispose()
                    dataProcess = True
                    Return dsT
                Case "C"
                    strS = "Update tblTmpWorkAreasData Set OOB='', Calendar ='', Bill ='', WorkText ='' , Displayed ='N' Where WorkAreaID = 0"
                    daT = New OleDbDataAdapter(strS, connLocal)
                    command0 = New OleDbCommand(strS, connLocal)
                    daT.UpdateCommand = command0
                    command0.ExecuteNonQuery()

                    strS = "Update tblTmpWorkAreasData Set OOB='', Calendar ='', Bill ='', WorkText ='' , Displayed ='N' Where WorkAreaID = 1"
                    command1 = New OleDbCommand(strS, connLocal)
                    daT.UpdateCommand = command1
                    command1.ExecuteNonQuery()

                    strS = "Update tblTmpWorkAreasData Set OOB='', Calendar ='', Bill ='', WorkText ='' , Displayed ='N' Where WorkAreaID = 2"
                    command2 = New OleDbCommand(strS, connLocal)
                    daT.UpdateCommand = command2
                    command2.ExecuteNonQuery()

                    tmp_DataSet = dsT
                    dataProcess = True
                    command0.Dispose()
                    command1.Dispose()
                    command2.Dispose()
                    Return dsT
            End Select
            connLocal.Close()
        Catch ex As OleDb.OleDbException
            dataProcess = False
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Voting System")
        Finally
            daT.Dispose()
            dsT.Dispose()
            connLocal.Close()
            If DisplayImage Then
                cnOOB.Close()
            End If
        End Try
    End Function

    Private Sub ReloadWindows()
        Dim daT As New OleDbDataAdapter
        Dim dsT As New DataSet

        v_Rows = 0
        Try
            If connLocal.State = ConnectionState.Closed Then connLocal.Open()

            If tPage0 Then
                strSQL = "Select * From tblTmpWorkAreasData Where WorkAreaID = 0"
            ElseIf tPage1 Then
                strSQL = "Select * From tblTmpWorkAreasData Where WorkAreaID = 1"
            ElseIf tPage2 Then
                strSQL = "Select * From tblTmpWorkAreasData Where WorkAreaID = 2"
            End If

            daT = New OleDbDataAdapter(strSQL, connLocal)
            daT.SelectCommand = New OleDbCommand(strSQL, connLocal)
            daT.Fill(dsT, "Table")

            If dsT.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In dsT.Tables(0).Rows
                    If (dr("Calendar") = "Special Order Calendar") Or Mid(UCase(dr("Calendar")), 1, 2) = "SR" Or Mid(UCase(dr("Calendar")), 1, 2) = "SJ" Or Mid(UCase(dr("Calendar")), 1, 2) = "HR" Or Mid(UCase(dr("Calendar")), 1, 2) = "HJ" Then
                        If isDeleteSOC Then
                            Me.OrderOfBusiness.Text = ""
                            Me.Calendar.SelectedIndex = 0
                            Me.CurrentCalendar.Text = Me.Calendar.Text
                            Calendar_Click(Calendar, Nothing)
                            Me.Bill.SelectedItem = 0
                            Me.CurrentBill.Text = Me.Bill.Text
                            Bill_Click(Bill, Nothing)
                            If tPage0 Then
                                Me.WorkData.Text = ""
                            ElseIf tPage1 Then
                                Me.WorkData1.Text = ""
                            ElseIf tPage2 Then
                                Me.WorkData2.Text = ""
                            End If
                        Else
                            GoTo continu
                        End If
                    Else
continu:
                        Me.OrderOfBusiness.SelectedItem = dr("OOB")
                        Me.Calendar.SelectedItem = dr("Calendar")
                        Me.CurrentCalendar.Text = dr("Calendar")
                        Calendar_Click(Calendar, Nothing)
                        Me.Bill.SelectedItem = dr("Bill")
                        Me.CurrentBill.Text = dr("Bill")
                        Bill_Click(Bill, Nothing)

                        If tPage0 Then
                            Me.WorkData.Text = dr("WorkText")
                            If dr("Displayed") = "Y" Then
                                TC.TabPages(0).BackColor = Color.Green
                                TC.TabPages(0).Text = "Displayed"
                            Else
                                TC.TabPages(0).BackColor = Color.Red
                                TC.TabPages(0).Text = "Work Area"
                            End If
                            '  V_DataSet("Update tblBills set WorkData ='" & Me.WorkData.Text & "' Where Bill ='" & Me.CurrentBill.Text & "' and CalendarCode = (select CalendarCode from tblCalendars Where Calendar ='" & dr("Calendar") & "')", "U")
                        ElseIf tPage1 Then
                            Me.WorkData1.Text = dr("WorkText")
                            ' V_DataSet("Update tblBills set WorkData ='" & Me.WorkData1.Text & "' Where Bill ='" & Me.CurrentBill.Text & "' and CalendarCode = (select CalendarCode from tblCalendars Where Calendar ='" & dr("Calendar") & "')", "U")
                            If dr("Displayed") = "Y" Then
                                TC.TabPages(1).BackColor = Color.Green
                                TC.TabPages(1).Text = "Displayed"
                            Else
                                TC.TabPages(1).BackColor = Color.Red
                                TC.TabPages(1).Text = "Area 1"
                            End If
                        ElseIf tPage2 Then
                            Me.WorkData2.Text = dr("WorkText")
                            If dr("Displayed") = "Y" Then
                                TC.TabPages(2).BackColor = Color.Green
                                TC.TabPages(2).Text = "Displayed"
                            Else
                                TC.TabPages(2).BackColor = Color.Red
                                TC.TabPages(2).Text = "Area 2"
                            End If
                            ' V_DataSet("Update tblBills set WorkData ='" & Me.WorkData2.Text & "' Where Bill ='" & Me.CurrentBill.Text & "' and CalendarCode = (select CalendarCode from tblCalendars Where Calendar ='" & dr("Calendar") & "')", "U")
                        End If
                    End If
                Next
            End If
            isDeleteSOC = False
            dsT.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Reload Windows")
        Finally
            connLocal.Close()
        End Try
    End Sub

    Private Sub tmpDisplayedData(ByVal page As Integer, ByVal displaied As String, strText As String)
        Dim strTmp As String = ""
        Try
            strTmp = "Delete * From tbltmpDisplayedData"
            V_DataSet(strTmp, "D")

            strTmp = "Insert Into tbltmpDisplayedData Values (" & page & ", '" & displaied & "','" & Me.OrderOfBusiness.SelectedItem & "','" & Me.Calendar.SelectedItem & "','" & Me.CurrentBill.Text & "', '" & strText & "')"
            V_DataSet(strTmp, "A")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Update Template Displayed Data")
        End Try
    End Sub

#Region "Three work areas"

    Private Sub WorkData_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WorkData.TextChanged
        Dim WrkFld As String

        btnCreateHTMLPage.BackColor = Color.Silver
        lblWorkAreaDisplayed.Visible = False

        '--- always set current phrase to last line
        WrkFld = vbCrLf & Me.WorkData.Text                      ' add crlf since going from end to start and this is a flag and there may only be one phrase

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

        work_area_text = gCurrentPhrase

        UpdateBillWorkData()

        '--- do not update if free format since that would update the current bill with the free format text
        If Not gFreeFormat Then
            UpdateWorkDataSW = True
        End If
    End Sub

    Private Sub WorkData1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WorkData1.TextChanged
        Dim WrkFld As String

        lblWorkAreaDisplayed.Visible = False
        btnCreateHTMLPage.BackColor = Color.Silver

        '--- always set current phrase to last line
        WrkFld = vbCrLf & Me.WorkData1.Text                      ' add crlf since going from end to start and this is a flag and there may only be one phrase

        If Strings.Right(WrkFld, 2) = vbCrLf Then
            WrkFld = Mid(WrkFld, 1, Len(WrkFld) - 2)
        End If
        gCurrentPhrase = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) + 2)
        If InStr(Me.WorkData1.Text, gBIR) = 1 Then
            gCurrentPhrase = "BIR - " & gCurrentPhrase
        End If
        work_area1_text = gCurrentPhrase

        UpdateBillWorkData()
        'If rightMouse = False Then
        '    UpdateBillWorkData()
        '    tmp_DataSet("U")
        'End If

        '--- do not update if free format since that would update the current bill with the free format text
        If Not gFreeFormat Then
            UpdateWorkDataSW = True
        End If
    End Sub

    Private Sub WorkData2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WorkData2.TextChanged
        Dim WrkFld As String

        lblWorkAreaDisplayed.Visible = False
        btnCreateHTMLPage.BackColor = Color.Silver

        '--- always set current phrase to last line
        WrkFld = vbCrLf & Me.WorkData2.Text                      ' add crlf since going from end to start and this is a flag and there may only be one phrase

        If Strings.Right(WrkFld, 2) = vbCrLf Then
            WrkFld = Mid(WrkFld, 1, Len(WrkFld) - 2)
        End If
        gCurrentPhrase = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) + 2)
        If InStr(Me.WorkData2.Text, gBIR) = 1 Then
            gCurrentPhrase = "BIR - " & gCurrentPhrase
        End If
        work_area2_text = gCurrentPhrase

        UpdateBillWorkData()
        'If rightMouse = False Then
        '    UpdateBillWorkData()
        '    tmp_DataSet("U")
        'End If

        '--- do not update if free format since that would update the current bill with the free format text
        If Not gFreeFormat Then
            UpdateWorkDataSW = True
        End If
    End Sub

    Private Sub btnFullClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFullClear.Click
        If SOOB_On Then
            SendStartOOBToOOB("", "CLEAR DISPLAY")
        End If
    End Sub

    Private Sub btnDropLastPhrase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDropLastPhrase.Click
        If tPage0 Then
            If Strings.Right(Me.WorkData.Text, 2) = vbCrLf Then
                Me.WorkData.Text = Mid(Me.WorkData.Text, 1, Len(Me.WorkData.Text) - 2)
            End If
            If InStrRev(Me.WorkData.Text, vbCrLf) = 0 Then                      ' dropping last phrase, or first since we're going backwards
                Me.WorkData.Text = ""
            Else
                Me.WorkData.Text = Mid(Me.WorkData.Text, 1, InStrRev(Me.WorkData.Text, vbCrLf) + 1)
            End If
        ElseIf tPage1 Then
            If Strings.Right(Me.WorkData1.Text, 2) = vbCrLf Then
                Me.WorkData1.Text = Mid(Me.WorkData1.Text, 1, Len(Me.WorkData1.Text) - 2)
            End If
            If InStrRev(Me.WorkData1.Text, vbCrLf) = 0 Then                      ' dropping last phrase, or first since we're going backwards
                Me.WorkData1.Text = ""
            Else
                Me.WorkData1.Text = Mid(Me.WorkData1.Text, 1, InStrRev(Me.WorkData1.Text, vbCrLf) + 1)
            End If
        ElseIf tPage2 Then
            If Strings.Right(Me.WorkData2.Text, 2) = vbCrLf Then
                Me.WorkData2.Text = Mid(Me.WorkData2.Text, 1, Len(Me.WorkData2.Text) - 2)
            End If
            If InStrRev(Me.WorkData2.Text, vbCrLf) = 0 Then                      ' dropping last phrase, or first since we're going backwards
                Me.WorkData2.Text = ""
            Else
                Me.WorkData2.Text = Mid(Me.WorkData2.Text, 1, InStrRev(Me.WorkData2.Text, vbCrLf) + 1)
            End If
        End If
    End Sub

    Private Sub Reload_Bill(ByVal txtWorkData As String)
        Dim input As String = txtWorkData
        Dim phrase As String = vbCrLf
        Dim Occurrences As Integer = 0
        Dim ds, dsC As New DataSet
        Dim strWorkArea As String = ""

        Try
            If input <> "" Then
                Occurrences = (input.Length - input.Replace(phrase, String.Empty).Length) / phrase.Length
            End If
            If Occurrences = 1 Then
                txtWorkData = Replace(txtWorkData, vbCrLf, "")
            End If

            If Occurrences <> 0 Or (Occurrences = 0 And txtWorkData <> "") Then
                '--- find coordinator bill 
                If tPage0 Then
                    If wrkCalCode = "SOC" Or InStr(UCase(wrkCalCode), "SR") > 0 Or InStr(UCase(wrkCalCode), "SP") > 0 Then
                        strSQL = "Select * From tblSpecialOrderCalendar Where CalendarCode = '"
                    ElseIf wrkCalCode <> "SOC" And InStr(UCase(wrkCalCode), "SR") = 0 Then
                        strSQL = "Select * From tblBills Where CalendarCode = '"
                    End If
                    strSQL = strSQL & wrkCalCode & "' And LTrim(Bill) ='" & LTrim(work_bill) & "'"
                ElseIf tPage1 Then
                    If wrkCalCode1 = "SOC" Or InStr(UCase(wrkCalCode1), "SR") > 0 Or InStr(UCase(wrkCalCode1), "SP") > 0 Then
                        strSQL = "Select * From tblSpecialOrderCalendar Where CalendarCode = '"
                    ElseIf wrkCalCode1 <> "SOC" And InStr(UCase(wrkCalCode1), "SR") = 0 Then
                        strSQL = "Select * From tblBills Where CalendarCode = '"
                    End If
                    strSQL = strSQL & wrkCalCode1 & "' And LTrim(Bill) ='" & LTrim(work1_bill) & "'"
                ElseIf tPage2 Then
                    If wrkCalCode2 = "SOC" Or InStr(UCase(wrkCalCode2), "SR") > 0 Or InStr(UCase(wrkCalCode2), "SP") > 0 Then
                        strSQL = "Select * From tblSpecialOrderCalendar Where CalendarCode = '"
                    ElseIf wrkCalCode2 <> "SOC" And InStr(UCase(wrkCalCode2), "SR") = 0 Then
                        strSQL = "Select * From tblBills Where CalendarCode = '"
                    End If
                    strSQL = strSQL & wrkCalCode2 & "' And LTrim(Bill) ='" & LTrim(work2_bill) & "'"
                End If

                ds = V_DataSet(strSQL, "R")
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each dr As DataRow In ds.Tables(0).Rows
                        '--- focus on coordinator Order Of Businees
                        If tPage0 Then
                            Calendar_Bill(wrkCal)
                            Me.Calendar.SelectedItem = wrkCal
                            Me.OrderOfBusiness.SelectedItem = wrkOOB
                        ElseIf tPage1 Then
                            Calendar_Bill(wrkCal1)
                            Me.Calendar.SelectedItem = wrkCal1
                            Me.OrderOfBusiness.SelectedItem = wrkOOB1
                        ElseIf tPage2 Then
                            Calendar_Bill(wrkCal2)
                            Me.Calendar.SelectedItem = wrkCal2
                            Me.OrderOfBusiness.SelectedItem = wrkOOB2
                        End If

                        '--- focus on coordinator Calendar
                        Calendar_Click(Calendar, Nothing)
                        strSQL = "Select * From tblCalendars Where CalendarCode ='" & dr("CalendarCode") & "'"
                        dsC = V_DataSet(strSQL, "R")
                        For Each drC As DataRow In dsC.Tables(0).Rows
                            Me.Calendar.SelectedItem = drC("Calendar")
                            gCalendarCode = dr("CalendarCode")
                            Exit For
                        Next
                        Me.Calendar.SelectedItem = gCalendar

                        '--- focus on coordinator bill
                        Me.CurrentBill.Text = dr("Bill")

                        '--- focus on coordinator Work Area
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
                        Me.Bill.SelectedItem = dr("Bill")
                        Exit For
                    Next
                End If
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Retreive Bill Back", "S")
            Exit Sub
        Finally
            ds.Dispose()
            dsC.Dispose()
        End Try
    End Sub

    Private Sub TC_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TC.SelectedIndexChanged
        Dim strWorkArea As String = ""
        Try
            '--- reload data to coordinator place, meanwhile record last actived workarea data for late retrived
            Select Case TC.SelectedIndex
                Case 0
                    tPage0 = True
                    tPage1 = False
                    tPage2 = False
                    If work_bill <> "" Or InStr(UCase(wrkCal), "SR") > 0 Or InStr(UCase(wrkCal), "SOC") > 0 Then
                        Reload_Bill(wrkText)
                    End If

                    If enterSOC = False Then
                        wrkCal = Me.Calendar.SelectedItem
                        wrkOOB = Me.OrderOfBusiness.SelectedItem
                        Me.CurrentBill.Text = work_bill
                        Owork_bill = Me.CurrentBill.Text
                        Owork_area_text = Me.WorkData.Text
                        OwrkCal = Me.Calendar.SelectedItem
                        OwrkOOB = Me.OrderOfBusiness.SelectedItem
                    End If
                    enterSOC = False

                    strWorkArea = WorkData.Text
                    If displaied_work = True Then
                        If SOOB_On Then
                            SendMessageToOOB(strWorkArea)
                        End If
                    End If
                Case 1
                    tPage0 = False
                    tPage1 = True
                    tPage2 = False
                    If work1_bill <> "" Then
                        Reload_Bill(wrkText1)
                    End If
                    If enterSOC = False Then
                        wrkCal1 = Me.Calendar.SelectedItem
                        wrkOOB1 = Me.OrderOfBusiness.SelectedItem
                        Me.CurrentBill.Text = work1_bill
                        Owork1_bill = Me.CurrentBill.Text
                        Owork_area1_text = Me.WorkData1.Text
                        OwrkCal1 = Me.Calendar.SelectedItem
                        OwrkOOB1 = Me.OrderOfBusiness.SelectedItem
                    End If
                    enterSOC = False
                    strWorkArea = WorkData1.Text
                    If displaied_work1 = True Then
                        If SOOB_On Then
                            SendMessageToOOB(strWorkArea)
                        End If
                    End If
                Case 2
                    tPage0 = False
                    tPage1 = False
                    tPage2 = True
                    If work2_bill <> "" Then
                        Reload_Bill(wrkText2)
                    End If
                    If enterSOC = False Then
                        wrkCal2 = Me.Calendar.SelectedItem
                        wrkOOB2 = Me.OrderOfBusiness.SelectedItem
                        Me.CurrentBill.Text = work2_bill
                        Owork2_bill = Me.CurrentBill.Text
                        Owork_area2_text = Me.WorkData2.Text
                        OwrkCal2 = Me.Calendar.SelectedItem
                        OwrkOOB2 = Me.OrderOfBusiness.SelectedItem
                    End If
                    enterSOC = False

                    strWorkArea = WorkData2.Text
                    If displaied_work2 = True Then
                        If SOOB_On Then
                            SendMessageToOOB(strWorkArea)
                        End If
                    End If
            End Select
            tmp_DataSet("U")
        Catch ex As Exception
            Try
                TC_SelectedIndexChanged(TC, e)
            Catch ex2 As Exception
                MsgBox(ex2.Message, MsgBoxStyle.Critical, "Chamber Display Change Work Area")
                Exit Sub
            End Try
        End Try
    End Sub

    Private Function GetCalendarCode(ByVal calendar As String) As String
        Dim ds As New DataSet
        Try
            strSQL = "Select * From tblCalendars Where Calendar ='" & calendar & "'"
            ds = V_DataSet(strSQL, "R")
            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    GetCalendarCode = dr("CalendarCode")
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Failed to Retreive Calender Code")
        Finally
            ds.Dispose()
        End Try
    End Function

    Private Sub TabPage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage.TextChanged
        wrkText = Me.TabPage.Text
    End Sub

    Private Sub TabPage1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage1.TextChanged
        wrkText1 = Me.TabPage1.Text
    End Sub

    Private Sub TabPage2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage2.TextChanged
        wrkText2 = Me.TabPage2.Text
    End Sub

#End Region
  
End Class

