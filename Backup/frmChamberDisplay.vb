Option Strict Off
Option Explicit Off

Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Timers
Imports System.ComponentModel
Imports System.Net
Imports System.Net.Sockets
Imports System.Messaging
Imports System.Runtime.Serialization
Imports System.Text
Imports System.DateTime
Imports System.DBNull
Imports System.Net.NetworkInformation
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class frmChamberDisplay
    Private RetValue As Object
    Private ComingFrom As String
    Private Highlight As Short
    Private UpdateWorkDataSW As Short
    Private ClearDisplayFlag As Boolean = False

    Public Shared WithEvents frmPhraseShow As New frmPhraseShow
    Public Shared WithEvents frmSenatorShow As New frmSenatorShow

    Private Sub AddPhrase_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles AddPhrase.KeyDown
        If e.KeyCode = Keys.Enter Then
            '--- format is Phrase # followed by senator # followed by committee, ex.  3,5,A&F to get
            '--- Phrase #3 followed by senator #5 followed by committee A&F; this senator part and
            '--- committees only applies if there are senator and committee insertion point(s) in the phrase

            Dim fnt As Font
            fnt = Me.WorkData.Font
            Me.WorkData.Font = New Font(fnt.Name, 8, FontStyle.Regular)

            ' LoadSenatorsIntoArray()

            Dim WrkFld As String, WrkFld3 As String, i As Integer
            Dim SNbr As Integer, PNbr As Integer, CAbbrev As String

            WrkFld = Me.AddPhrase.Text
            If Strings.Right$(WrkFld, 1) <> "," Then
                WrkFld = WrkFld & ","
            End If
            WrkFld = ReplaceCharacter(WrkFld, " ", "") ' get rid of all spaces
            ''WrkFld = Strings.Replace(WrkFld, " ", "") ' get rid of all spaces
            If NToB(Me.AddPhrase.Text) = "," Then  ' no senator so exit
                Exit Sub
            End If

            Me.AddPhrase.Text = ""

            PNbr = 0
            SNbr = 0
            CAbbrev = ""

            PNbr = Val(Mid$(WrkFld, 1, InStr(WrkFld, ",") - 1))
            WrkFld = Mid$(WrkFld, InStr(WrkFld, ","))   ' delete up to ,
            If (WrkFld <> ",") And (WrkFld <> ",,") And (WrkFld <> ",,,") Then
                If Strings.Left$(WrkFld, 2) = ",," Then  ' senator and abbrev entered
                    CAbbrev = UCase(Mid$(WrkFld, 3, InStr(3, WrkFld, ",") - 3))
                Else
                    WrkFld = Mid$(WrkFld, 2)
                    SNbr = Val(Mid$(WrkFld, 1, InStr(2, WrkFld, ",") - 1))
                    WrkFld = Mid$(WrkFld, InStr(WrkFld, ",")) ' delete up to ,
                    If (WrkFld <> ",") And (WrkFld <> ",,") Then
                        CAbbrev = UCase(Mid$(WrkFld, 2, InStr(2, WrkFld, ",") - 2))
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
                    WrkFld3 = "Senator " & gSenatorName(SNbr) & Mid$(WrkFld3, 2)
                Else
                    WrkFld3 = Mid$(WrkFld3, 1, InStr(WrkFld3, gSenatorInsertionPoint) - 1) & "Senator " & gSenatorName(SNbr) & Mid$(WrkFld3, InStr(WrkFld3, gSenatorInsertionPoint) + 1)
                End If
            End If

            '---see if committee insertion point is in the phrase, and if so add the committee name
            If InStr(WrkFld3, gCommitteeInsertionPoint) > 0 Then
                If CAbbrev = "" Then
                Else
                    For i = 1 To gNbrCommittees
                        If gCommitteeAbbrevs(i) = CAbbrev Then
                            If InStr(WrkFld3, gCommitteeInsertionPoint) = 1 Then
                                WrkFld3 = gCommittees(i) & Mid$(WrkFld3, 2)
                            Else
                                WrkFld3 = Mid$(WrkFld3, 1, InStr(WrkFld3, gCommitteeInsertionPoint) - 1) & gCommittees(i) & Mid$(WrkFld3, InStr(WrkFld3, gCommitteeInsertionPoint) + 1)
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
                    fntn = Me.WorkData.Font
                    Me.WorkData.Font = New Font(fntn.Name, 12, FontStyle.Bold)
                    Me.WorkData.Enabled = True
                    Me.CurrentBill.Text = "ADJOURMENT"
                    Me.WorkData.Text = "The senate is in adjournment until "
                End If
            End If
        End If
    End Sub

    Private Sub Bill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Bill.Click
        Me.WorkData.Enabled = True
        Me.CurrentBill.Enabled = True

        If (ComingFrom = "NextBill") Or (ComingFrom = "PreviousBill") Then
            ComingFrom = ""
        ElseIf UpdateWorkDataSW Then
            UpdateBillWorkData()
        End If

        If Bill.SelectedItem <> "" Then
            CurrentBill.Text = Bill.SelectedItem.ToString
            gBill = Bill.Text
            BillHasChanged()
        End If

        '--- If no motion calendar and there is a senator insertion point in this motion, 
        '--- then pull down the senator list box
        If (gCalendarCode = "M") And (InStr(Bill.Text, gSenatorInsertionPoint) > 0) Then
            Senators.Focus()
            SendKeys.Send("F4")
        End If
    End Sub

    Public Sub InsertIntoWorkData(ByVal Data As String, ByVal InsertionPoint As String)
        Dim i As Integer

        '--- put stuff into the work area at the insertion point; if this is
        '--- a phrase then append if no insertion point found; if the current
        '--- calendar is the motion calendar, then insert into the current bill area,
        '--- then try the work area

        If Data <> "ADJOURNMENT" Then
            Dim fntt As Font
            fntt = Me.WorkData.Font
            Me.WorkData.Font = New Font(fntt.Name, 8, FontStyle.Regular)

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
                    pWorkData = Me.WorkData.Text
                ElseIf InsertionPoint = gPhraseInsertionPoint Then                  ' append the phrase
                    Me.WorkData.Text = Me.WorkData.Text & Data
                    pWorkData = Me.WorkData.Text
                    If Len(Me.WorkData.Text) > 1 Then
                        If Strings.Right(Me.WorkData.Text, 2) <> vbCrLf Then
                            pWorkData = Me.WorkData.Text
                            Me.WorkData.Text = Me.WorkData.Text & vbCrLf
                        End If
                    End If
                End If
            ElseIf tPage1 Then
                i = InStr(NToB(Me.WorkData2.Text), InsertionPoint)
                If i > 0 Then
                    Me.WorkData2.Text = Mid(Me.WorkData2.Text, 1, i - 1) & Data & Mid(Me.WorkData2.Text, i + 1)
                    pWorkData2 = Me.WorkData2.Text
                ElseIf InsertionPoint = gPhraseInsertionPoint Then                  ' append the phrase
                    Me.WorkData2.Text = Me.WorkData2.Text & Data
                    pWorkData2 = Me.WorkData2.Text
                    If Len(Me.WorkData2.Text) > 1 Then
                        If Strings.Right(Me.WorkData2.Text, 2) <> vbCrLf Then
                            pWorkData2 = Me.WorkData2.Text
                            Me.WorkData2.Text = Me.WorkData2.Text & vbCrLf
                        End If
                    End If
                End If
            ElseIf tPage2 Then
                i = InStr(NToB(Me.WorkData3.Text), InsertionPoint)
                If i > 0 Then
                    Me.WorkData3.Text = Mid(Me.WorkData3.Text, 1, i - 1) & Data & Mid(Me.WorkData3.Text, i + 1)
                    pWorkData3 = Me.WorkData3.Text
                ElseIf InsertionPoint = gPhraseInsertionPoint Then                  ' append the phrase
                    Me.WorkData3.Text = Me.WorkData3.Text & Data
                    pWorkData3 = Me.WorkData3.Text
                    If Len(Me.WorkData3.Text) > 1 Then
                        If Strings.Right(Me.WorkData3.Text, 2) <> vbCrLf Then
                            pWorkData3 = Me.WorkData3.Text
                            Me.WorkData3.Text = Me.WorkData3.Text & vbCrLf
                        End If
                    End If
                End If
            End If

            '--- if senator insertion point is in the phrase, or bill area for the motion calendar, then pull down the senator list
            If (InsertionPoint = gPhraseInsertionPoint) And (InStr(Data, gSenatorInsertionPoint)) Then
                Me.Senators.Focus()
                SendKeys.Send("{F4}")
            End If
        Else
            Dim fnt As Font
            fnt = Me.WorkData.Font
            Me.WorkData.Font = New Font(fnt.Name, 12, FontStyle.Bold)
            Me.WorkData.Enabled = True
            Me.CurrentBill.Text = Data
            Me.WorkData.Text = "The senate is in adjournment until "
        End If
    End Sub

    Private Sub btnBIR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBIR.Click
        '--- if bir, then fill in work area (if blank) and put cursor on senator box and open it
        If Me.WorkData.Text = "" Then
            Me.WorkData.Text = gBIR & vbCrLf & "^ MOTION TO ADOPT" & vbCrLf
            Me.Senators.Focus()
            SendKeys.Send("{F4}")
        End If
    End Sub

    Private Sub btnCancelVote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelVote.Click
        If MsgBox("Are you sure you want to cancel this vote?", MsgBoxStyle.YesNo, "Senate Voting System") = MsgBoxResult.Yes Then
            gPutParams = True
            gPutCalendarCode = "Cancel"
            gPutCalendar = ""
            gPutBill = ""
            gPutBillNbr = ""
            gPutPhrase = ""
            Me.lblChamberLight.Text = "Display Next Bill"
            gPutChamberLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Lime)
            gPutVotingLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
            Me.WorkData.Text = ""
            Me.lblWorkAreaDisplayed.Visible = False
            Me.btnUpdateDisplay.Enabled = True
            Me.btnVote.Enabled = True
            Me.btnRecallDisplayedBill.Enabled = True
            Me.OrderOfBusiness.Enabled = True
            Me.btnExit.Enabled = True
            cboDisplay.Enabled = True
            gCalendarDisplayed = ""
            gCalendarCodeDisplayed = ""
            gBillDisplayed = ""
            gBillNbrDisplayed = ""
        End If
    End Sub

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

    Private Sub btnDropLastPhrase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDropLastPhrase.Click
        If Strings.Right(Me.WorkData.Text, 2) = vbCrLf Then
            Me.WorkData.Text = Mid$(Me.WorkData.Text, 1, Len(Me.WorkData.Text) - 2)
        End If
        If InStrRev(Me.WorkData.Text, vbCrLf) = 0 Then                      ' dropping last phrase, or first since we're going backwards
            Me.WorkData.Text = ""
        Else
            Me.WorkData.Text = Mid$(Me.WorkData.Text, 1, InStrRev(Me.WorkData.Text, vbCrLf) + 1)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If UpdateWorkDataSW Then
            UpdateBillWorkData()
        End If

        Me.Close()
        frmStart.Show()
    End Sub


    Private Sub btnFreeFormat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFreeFormat.Click
        '--- change color of background to let user know they are in free format;
        '--- user must manually turn this off

        If System.Drawing.ColorTranslator.ToOle(Me.btnFreeFormat.BackColor) = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green) Then
            Me.btnFreeFormat.BackColor = System.Drawing.ColorTranslator.FromOle(&HC0C0C0)
            gFreeFormat = False
            Me.CurrentBill.Enabled = False
            Me.CurrentBill.BackColor = Color.White
            Me.btnRecallDisplayedBill.Enabled = True
            Me.OrderOfBusiness.Enabled = True
            Exit Sub
        End If

        Me.CurrentBill.Enabled = True
        Me.btnFreeFormat.BackColor = System.Drawing.Color.Green
        gFreeFormat = True
        If UpdateWorkDataSW Then
            UpdateBillWorkData()
        End If

        Me.WorkData.Text = ""
        gCalendarCodeDisplayed = ""
        gBillNbrDisplayed = ""
        gCalendarDisplayed = ""
        gBillDisplayed = ""
        gCurrentPhrase = ""
        Me.CurrentBill.Text = ""
        gBill = ""
        Me.WorkData.Enabled = True
        Me.OrderOfBusiness.Enabled = False
    End Sub

    Private Sub btnNextBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNextBill.Click
        Dim i As Integer
        i = Bill.SelectedIndex

        If i >= 0 And i <= Bill.Items.Count - 2 Then
            Bill.SelectedItem = Bill.Items.Item(i + 1)
        End If
        ComingFrom = "NextBill"
    End Sub

    Private Sub btnPhraseMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPhraseMaintenance.Click
        Dim frmPL As New frmPhraseList
        frmPL.Show()
    End Sub

    Private Sub btnPreviousBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreviousBill.Click
        Dim i As Integer
        i = Bill.SelectedIndex

        If i >= 1 Then
            Bill.SelectedItem = Bill.Items.Item(i - 1)
        End If
        ComingFrom = "PreviousBill"
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

    Private Sub btnUpdateDisplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateDisplay.Click
        '--- send this work area to the projection display
        If Me.CurrentBill.Text = "" Then
            RetValue = DisplayMessage("Please select a bill to display", "No Bill Selected", "S")
            Exit Sub
        End If

        If SDisplay_On Then
            gCalendarCodeDisplayed = gCalendarCode
            gBillNbrDisplayed = gBillNbr
            gCalendarDisplayed = gCalendar
            gBillDisplayed = gBill

            Me.OrderOfBusiness.Enabled = False
            Me.btnExit.Enabled = False
            Me.lblWorkAreaDisplayed.Visible = True


            '---send voting information to display PC message queue
            bodyParamters = ""
            bodyParamters = "gBill - " & Me.Bill.Text & "||" & "gWorkArea - " & Me.WorkData.Text & "||" & "gCurrentOrderOfBusiness - " & CurrentOrderOfBusiness.Text
            DisplayImage = True
            SendMessageToDisplay(UCase(cboDisplay.Text), bodyParamters, gSendQueueToDisplay)

            '---Update HTML page for Web Site
            CreateHTMLPage()
        Else
            DisplayImage = False
            MsgBox("Senate voting display borad computer is off. Unable to change the background.", MsgBoxStyle.Critical, msgText)
        End If
    End Sub

    Private Sub Calendar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Calendar.Click
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim str As String

        If UpdateWorkDataSW Then
            UpdateBillWorkData()
        End If
        Me.WorkData.Text = ""
        UpdateWorkDataSW = False
        gCalendar = Me.Calendar.Text
        Me.lblWorkAreaDisplayed.Visible = False                   'reset in case it was on
        Me.CurrentCalendar.Text = Me.Calendar.Text

        str = "Select Bill From tblBills Where CalendarCode = (Select CalendarCode From tblCalendars Where Calendar='" & gCalendar & "') Order By Bill"
        ds = V_DataSet(str, "R")
        Bill.Items.Clear()
        For Each dr In ds.Tables(0).Rows
            Bill.Items.Add(dr("Bill"))
        Next
        CurrentCalendar.Text = Calendar.SelectedItem

        CurrentBill.Text = ""

        '--- if motion calendar (code 9) is selected, then change caption on current bill field
        If UCase(CurrentCalendar.Text) = UCase("Motions") Then
            lblCurrentBill.Text = "Current Motion"
        Else
            lblCurrentBill.Text = "Current Bill"
        End If
    End Sub

    Private Sub BillHasChanged()
        Dim WrkFld As String
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim str As String

        gBill = Strings.Replace(gBill, "'", "")
        str = "SELECT tblBills.Sponsor, tblBills.Subject, tblBills.WorkData, tblBills.BillNbr, tblBills.CalendarCode  " & _
             " FROM tblBills, tblCalendars " & _
             " WHERE tblBills.CalendarCode = tblCalendars.CalendarCode AND (tblCalendars.Calendar = '" & gCalendar & "') AND (tblBills.Bill ='" & gBill & "')"
        ds = V_DataSet(str, "R")

        For Each dr In ds.Tables(0).Rows
            gCalendarCode = dr("CalendarCode")
            gBillNbr = dr("BillNbr")
            gSenator = dr("Sponsor")
            gSubject = dr("Subject")
            Me.WorkData.Text = NToB(dr("Workdata"))
            If NToB(dr("Workdata")) = "" Then
                Me.WorkData.Text = ""
            End If
        Next

        If Me.WorkData.Text = "" Then
            gCurrentPhrase = ""
        Else
            WrkFld = vbCrLf & Me.WorkData.Text
            If Strings.Right(WrkFld, 2) = vbCrLf Then
                WrkFld = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) - 2)
            End If
            gCurrentPhrase = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) + 2)
            'WrkFld = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) + 2)
            If InStr(Me.WorkData.Text, gBIR) = 1 Then
                gCurrentPhrase = "BIR - " & gCurrentPhrase
            End If
        End If

        '--- if this bill is the current one being displayed on the projection
        '--- screen then make visible the indicator boxes
        If gCalendarCode & gBillNbr = gCalendarCodeDisplayed & gBillNbrDisplayed Then
            Me.lblWorkAreaDisplayed.Visible = True
        Else
            Me.lblWorkAreaDisplayed.Visible = False
        End If
    End Sub

    Private Sub Bill_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bill.SelectedIndexChanged
        Dim fnt As Font

        fnt = Me.WorkData.Font
        Me.WorkData.Font = New Font(fnt.Name, 8, FontStyle.Regular)
        Me.WorkData.Enabled = True
        Me.CurrentBill.Enabled = True

        If (ComingFrom = "NextBill") Or (ComingFrom = "PreviousBill") Then
            ComingFrom = ""
        ElseIf UpdateWorkDataSW Then
            UpdateBillWorkData()
        End If

        If NToB(Bill.Text) <> "" Then
            CurrentBill.Text = Bill.SelectedItem.ToString
            gBill = Bill.Text
            BillHasChanged()
        End If

        '---If no motion calendar and there is a senator insertion point in this motion, 
        '---then pull down the senator list box
        If (gCalendarCode = "M") And (InStr(Bill.Text, gSenatorInsertionPoint) > 0) Then
            Senators.Focus()
            SendKeys.Send("F4")
        End If
    End Sub

    Private Sub UpdateBillWorkData()
        Dim str As String
        Dim ds As New DataSet

        UpdateWorkDataSW = False

        str = "Update tblBills SET WorkData='" & Me.WorkData.Text & "' Where CalendarCode='" & gCalendarCode & "' AND BillNbr='" & gBillNbr & "'"
        ds = V_DataSet(str, "U")
    End Sub

    Private Sub Committees_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Committees.SelectedIndexChanged
        InsertIntoWorkData(Mid(Me.Committees.Text, InStr(Me.Committees.Text, "-") + 2), gCommitteeInsertionPoint)
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
                gSubject = Mid(Me.CurrentBill.Text, i + 3, j - i - 3)
            End If

            str = "UPDATE tblBills SET Bill = '" & Me.CurrentBill.Text & "', Subject = '" & gSubject & "'  WHERE (CalendarCode = '" & gCalendarCode & "') AND (BillNbr = '" & gBillNbr & "')"
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
        If e.KeyValue = Keys.Enter Then
            Dim FieldName(6), FieldValue(6) As Object
            Dim k, x As Integer
            Dim Title As String, WrkFld As String
            Dim ds, dsA As New DataSet
            Dim dr As DataRow
            Dim str As String

            If NToB(Me.FindBillNbr.Text) = "" Then
                Exit Sub
            End If

            '---first check for bill on Access DB; if not found, then go to ALIS;
            '---update calendar and bill positions in look ups; dont go to ALIS
            '---in pure test mode

            str = "SELECT  tblCalendars.Calendar AS Calendar, tblBills.* FROM tblBills, tblCalendars WHERE tblBills.CalendarCode = tblCalendars.CalendarCode AND (tblBills.BillNbr = '" & UCase(Me.FindBillNbr.Text) & "')"
            ds = V_DataSet(str, "R")
            x = ds.Tables(0).Rows.Count

            For Each dr In ds.Tables(0).Rows
                If x > 0 Then
                    Me.Calendar.Text = dr("Calendar")
                    Calendar_Click(sender, e)
                    Me.Bill.Text = dr("Bill")
                    Bill_Click(sender, e)
                ElseIf (gTestMode) And (Not gWriteVotesToTest) Then
                Else
                    '---check ALIS                
                    WrkFld = UCase(Me.FindBillNbr.Text)
                    If Strings.Left(WrkFld, 2) = "CF" Then
                        WrkFld = "SCF"
                    End If

                    str = " SELECT I.OID, I.LABEL, I.SPONSOR, I.INDEX_WORD, I.LEGISLATIVE_BODY, I.COMMITTEE, I.RECOMMENDATION, " & _
                                " TO_CHAR(I.LAST_TRANSACTION_DATE, 'MM/DD/YYYY') AS LAST_TRANSACTION_DATE, I.STATUS_CODE, I.TYPE_CODE, " & _
                                " I.ENACTED_YEAR, I.ENACTED_SEQUENCE, CV.VALUE AS STATUS_TEXT, D.SHORTTITLE " & _
                        " FROM ALIS_OBJECT I, CODE_VALUES CV, " & _
                                " DOCUMENT_VERSION DV, DOCUMENT D " & _
                        " WHERE I.STATUS_CODE = CV.CODE AND I.OID_CURRENT_DOCUMENT_VERSION = DV.OID (+) AND DV.OID_DOCUMENT = D.OID (+) AND (I.INSTRUMENT = 'T') " & _
                                " AND (I.OID_SESSION = " & gSessionID & ") AND (I.LABEL = '" & WrkFld & "') AND (CV.CODE_TYPE = 'InstrumentStatus')"

                    dsA = A_DataSet(str, "R")

                    If dsA.Tables(0).Rows.Count = 0 Then
                        RetValue = DisplayMessage("Bill not found", "", "S")
                        Exit Sub
                    End If

                    '---add the bill to the other category
                    For k = 0 To 6
                        FieldName(k) = ds.Tables(0).Columns(k).ColumnName
                    Next k

                    FieldValue(0) = "ZZ"
                    FieldValue(1) = dsA.Tables(0).Rows.Item("Label")
                    If Strings.Left(FieldValue(1), 1) = "S" Then
                        Title = " by Senator "
                    Else
                        Title = " by Representative "
                    End If
                    FieldValue(3) = NToB(dsA.Tables(0).Rows.Item("Sponsor"))
                    FieldValue(4) = NToB(dsA.Tables(0).Rows.Item("Index_Word"))
                    FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4)
                    FieldValue(5) = ""
                    FieldValue(6) = ""
                    ds.Tables(0).Columns.Add(FieldName(k), FieldValue(k))

                    '---put in other category
                    Me.Calendar.Text = "Other"
                    Calendar_Click(sender, e)
                    Me.Bill.Text = FieldValue(2)
                    Bill_Click(sender, e)
                End If
                Exit For
            Next
        End If
    End Sub

    Private Sub frmChamberDisplay_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If gTestMode Then
            Me.lblTestMode.Visible = True
        Else
            Me.lblTestMode.Visible = False
        End If
    End Sub

    Private Sub frmChamberDisplay_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        '--- Make the return key act like a tab key
        If Keys.Return = vbCrLf Then
            SendKeys.Send("Tab")
        End If
    End Sub

    Private Sub frmChamberDisplay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim str As String
        Dim ds As New DataSet
        Dim dsC As New DataSet
        Dim dsB As New DataSet
        Dim dr As DataRow

        'Main()
        Me.Top = 8
        Me.Left = 1

        If Not IsNothing(frmPhraseShow) Then
            If Not frmPhraseShow.IsDisposed Then
                frmPhraseShow.WindowState = FormWindowState.Normal
                frmPhraseShow.BringToFront()
                frmPhraseShow.Show()
            Else
                frmPhraseShow = New frmPhraseShow
                frmPhraseShow.Show()
            End If
        Else
            frmPhraseShow = New frmPhraseShow
            frmPhraseShow.Show()
        End If

        If Not IsNothing(frmSenatorShow) Then
            If Not frmSenatorShow.IsDisposed Then
                frmSenatorShow.WindowState = FormWindowState.Normal
                frmSenatorShow.BringToFront()
                frmSenatorShow.Show()
            Else
                frmSenatorShow = New frmSenatorShow
                frmSenatorShow.Show()
            End If
        Else
            frmSenatorShow = New frmSenatorShow
            frmSenatorShow.Show()
        End If

        '--- clear all of messages from senatevotequeue
        If SPrimary_On Then
            Dim mq As New MessageQueue(gSendQueueName)
            Dim msg As New Message
            mq.Purge()
        ElseIf SSecondary_On Then
            Dim mq As New MessageQueue(gSendQueueNameForSecondary)
            Dim msg As New Message
            mq.Purge()
        End If

        '--- clear all of messages from senatevotequeue
        Dim mq2 As New MessageQueue(gReceiveQueueName)
        Dim msg2 As New Message
        mq2.Purge()

        If gTestMode Then
            lblTestMode.Visible = True
        End If

        '--- Put last vote id + 1
        If gTestMode = False Then
            Me.VoteID.Text = gVoteID + 1
        Else
            Me.VoteID.Text = 0
        End If

        lblChamberLight.Visible = True
        lblChamberLight.Text = "RR"

        '--- Last will move it - this model is woked 
        UpdateWorkDataSW = False
        ComingFrom = ""
        gCalendarCodeDisplayed = ""
        gBillNbrDisplayed = ""
        gCalendarDisplayed = ""
        gBillDisplayed = ""
        Me.lblChamberLight.Image = Nothing
        ' Me.lblChamberLight.BackColor = System.Drawing.Color.Lime
        Me.lblChamberLight.BackColor = System.Drawing.Color.LightGreen
        Me.lblChamberLight.Text = "Display Next Bill"
        Me.lblSession.Text = gSessionName
        Me.LegislativeDay.Text = gLegislativeDay & " - " & gCalendarDay

        ERHPushStack(("LoadSenatorsIntoArray"))

        '--- init so nothing can be entered until a bill has been selcted
        Me.WorkData.Enabled = False

        '--- initialize calendar and bills   
        '--- str = "Select Calendar, CalendarCode From tblCalendars Where CalendarCode='1'"
        str = "Select Calendar, CalendarCode From tblCalendars  Order By CalendarCode"
        ds = V_DataSet(str, "R")
        For Each dr In ds.Tables(0).Rows
            Calendar.Items.Add(dr("Calendar"))
        Next

        Calendar_Bill(Calendar.Text)
        Calendar.SelectedIndex = 0
        Calendar_Click(sender, e)

        '--- initialize order of business
        str = "Select * From tblOrderOfBusiness "
        ds = V_DataSet(str, "R")
        For Each dr In ds.Tables(0).Rows
            OrderOfBusiness.Items.Add(dr("OrderOfBusiness"))
        Next

        OrderOfBusiness.Text = ds.Tables(0).Rows(0)("OrderOfBusiness")          '   Get first row record
        CurrentOrderOfBusiness.Text = OrderOfBusiness.Text

        ''PutOrdersOfBusinessOnOOBDisplayBoard()
        ''ApplyAttributesToOOBDisplayBoard()


        ' --- Load Senators from gSenatorName array in to SenatorName comboBox
        ' LoadSenatorsIntoArray()
        For i As Integer = 1 To gSenatorName.Length - 1
            Senators.Items.Add(gSenatorName(i))
        Next
        gNbrSenators = Senators.Items.Count

        '--- Load Committees from gCommitte() and gCommitteAbbreves() arrays to Committee comboBox
        LoadCommitteesIntoArray()
        For j As Integer = 1 To gCommittees.Length - 1
            Committees.Items.Add(gCommitteeAbbrevs(j) & " - " & gCommittees(j))
        Next

        '--- Load  Phases from gThePhrases(i) array in to Phases comboBox. Notic: gThePhrases(i) array included PhraseCode and Phrase, gPhrases has only Phrases string in array
        LoadPhrasesIntoArray()
        For k As Integer = 1 To gThePhrases.Length - 1
            Phrases.Items.Add(gThePhrases(k))
        Next

        '---Load Display Borader Image PowerPoint file name to cboBox
        str = "Select [File Name] From tblDisplayBoradImages "
        ds = V_DataSet(str, "R")
        For Each dr In ds.Tables(0).Rows
            cboDisplay.Items.Add(dr(0))
        Next
        For j As Integer = 0 To Me.cboDisplay.Items.Count - 1
            If InStr(UCase(cboDisplay.Items(j).ToString), "SENATEVOTINGPP") > 0 Then
                cboDisplay.Text = UCase(cboDisplay.Items(j).ToString)
            End If
        Next


        '---initialize and start timers
        If SPrimary_On Or SSecondary_On Then
            ReceiveTimer.Enabled = True
            ReceiveTimer.Start()
            ReceiveTimer.Interval = CInt(gReceiverQueueTimer)

            PingTimer.Enabled = False
            PingTimer.Stop()
        ElseIf SPrimary_On = False And SSecondary_On = False Then
            ReceiveTimer.Enabled = False
            ReceiveTimer.Stop()

            '!!! Using threading to ping to avoid lock process
            'PingTimer.Enabled = True
            'PingTimer.Start()
            'PingTimer.Interval = 1000000
            'PingComputer()
        End If
        'If Network_On Then
        '    WriteHTML.Enabled = True
        '    WriteHTML.Start()
        '    WriteHTML.Interval = CInt(gHTMLTimer)
        'End If
    End Sub

    Private Sub InsertText_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles InsertText.KeyDown
        If e.KeyValue = Keys.Enter Then
            InsertIntoWorkData(Me.InsertText.Text, gTextInsertionPoint)
            Me.InsertText.Text = ""
        End If
    End Sub

    Private Sub OrderOfBusiness_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles OrderOfBusiness.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Highlight = True
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            Highlight = False
        End If
    End Sub

    Private Sub OrderOfBusiness_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderOfBusiness.SelectedIndexChanged
        CurrentOrderOfBusiness.Text = OrderOfBusiness.SelectedItem.ToString
    End Sub

    Private Sub Phrases_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Phrases.SelectedIndexChanged
        If tPage0 Then
            If Strings.Right(WorkData.Text, 2) <> vbCrLf And WorkData.Text <> "" Then
                WorkData.Text = WorkData.Text & vbCrLf
            End If
        ElseIf tPage1 Then
            If Strings.Right(WorkData2.Text, 2) <> vbCrLf And WorkData2.Text <> "" Then
                WorkData2.Text = WorkData2.Text & vbCrLf
            End If
        ElseIf tPage2 Then
            If Strings.Right(WorkData3.Text, 2) <> vbCrLf And WorkData3.Text <> "" Then
                WorkData3.Text = WorkData3.Text & vbCrLf
            End If
        End If
        InsertIntoWorkData(Mid$(Me.Phrases.Text, InStr(Me.Phrases.Text, "-") + 2), gPhraseInsertionPoint)
    End Sub

    Private Sub ChamberTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' RetValue = GetPassedParameters()
    End Sub

    Private Sub CreateHTMLPage()
        '--- print the doc in HTML and put on the web server so all can see - done everytime the chamber
        '--- display operator hits update display
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim WrkFld, str As String, RetValue As Object

        Try
            str = "Select * From tblOrderOfBusiness"
            ds = V_DataSet(str, "R")

            FileOpen(1, gHTMLFile, OpenMode.Output)
            Print(1, "<HTML>")
            Print(1, "<meta http-equiv='refresh' content='4'/>")
            Print(1, "<Body><p>&nbsp;</p> ")

            Print(1, "<table align='center' width=100%>")
            Print(1, "<tr align='center'><td></td>")
            Print(1, "<td align='center'>")

            Print(1, "<H1><Center><B><U><FONT FACE=" & Chr(34) & "Arial" & Chr(34) & " SIZE=" & Chr(34) & "4" & Chr(34) & " COLOR=" & Chr(34) & "Black" & Chr(34) & ">Order Of Business</U></B><BR><BR>")
            For Each dr In ds.Tables(0).Rows
                If dr("OrderOfBusiness") = Me.CurrentOrderOfBusiness.Text Then
                    Print(1, "<Font Color=" & Chr(34) & "Red" & Chr(34) & ">" & Me.CurrentOrderOfBusiness.Text & "</Font><BR>")
                Else
                    Print(1, dr("OrderOfBusiness") & "<BR>")
                End If
            Next
            Print(1, "</td></tr></table>")

            If ClearDisplayFlag = False Then
                Print(1, "</Center></H1>-------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
                Print(1, "<BR>")
                Print(1, "<Font size='4' face='Arial' Color=" & Chr(34) & "Blue" & Chr(34) & "><b>" & Me.CurrentBill.Text & "</b></Font><BR>")
                Print(1, "<BR>-------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
                Print(1, "<BR><font size='3' face='Arial' ><b>")
                WrkFld = Me.WorkData.Text
                If Strings.Right(WrkFld, 2) <> vbCrLf Then
                    WrkFld = WrkFld & vbCrLf
                End If
                Do
                    Print(1, Mid(WrkFld, 1, InStr(WrkFld, vbCrLf) - 1))
                    Print(1, "<BR>")
                    If InStr(WrkFld, vbCrLf) = Len(WrkFld) - 1 Then
                        Exit Do
                    End If
                    WrkFld = Mid$(WrkFld, InStr(WrkFld, vbCrLf) + 2)
                Loop
                Print(1, "</b></font><BR><BR><BR>")
                Print(1, "<BR><BR><BR>")
                Print(1, "<Font Size=" & Chr(34) & "2" & Chr(34) & " face='Arial'> Current Date/Time:  " & Now & "  " & Now.ToShortTimeString & "</Font>")
                Print(1, "</Body>")
                Print(1, "</HTML>")
            Else
                Print(1, "</Center></H1>------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
                Print(1, "<BR>")
                Print(1, "<Font Color=" & Chr(34) & "Blue" & Chr(34) & ">" & "" & "</Font><BR>")
                Print(1, "<BR>----------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
                Print(1, "<BR>")
                WrkFld = ""

                Print(1, WrkFld)
                Print(1, "<BR>")
                Print(1, "<BR><BR><BR>")
                Print(1, "<BR><BR><BR>")
                Print(1, "<Font Size=" & Chr(34) & "2" & Chr(34) & " face='Arial'> Current Date/Time:  " & Now & "  " & Now.ToShortTimeString & "</Font>")
                Print(1, "</Body>")
                Print(1, "</HTML>")
                ClearDisplayFlag = False
            End If
            FileClose(1)

        Catch ex As Exception
            If ex.Source = 76 Then
                RetValue = DisplayMessage("The folder for this session " & gHTMLFile & " needs to be created before an HTML " & _
                   "can be written.  Please create the folder.", "Missing Folder For HTML Document", "S")
                Exit Sub
            End If
        End Try
    End Sub

    Public Sub Calendar_Bill(ByVal strCalendar As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim str As String

        '--- select the calendar and requery the child bill list
        If UpdateWorkDataSW Then
            UpdateBillWorkData()
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

    Private Sub SendMessageToQueue(ByVal label As String, ByVal strBody As String)
        Try
            Dim mq As New MessageQueue
            Dim msg As New Message
            Dim ds As New DataSet

            Try
                If (SPrimary_On And SSecondary_On = False) Then
                    mq.Path = gSendQueueName
                ElseIf (SPrimary_On = False And SSecondary_On) Then
                    mq.Path = gSendQueueNameForSecondary
                End If
                'queueName = ".\PRIVATE$\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE$\SenateVoteQueue"
                msg.Priority = MessagePriority.Normal
                msg.Label = label
                msg.Body = strBody
                mq.Send(msg)

                btnVote.Enabled = False
                lblChamberLight.BackColor = Color.Red
                lblChamberLight.Text = "Waiting Vote!"

                ReceiveTimer.Enabled = True
                ReceiveTimer.Start()
                ReceiveTimer.Interval = gReceiverQueueTimer

                If SDisplay_On Then
                    mq.Path = gSendQueueToDisplay
                    msg.Priority = MessagePriority.Normal
                    msg.Label = label
                    msg.Body = strBody
                    mq.Send(msg)
                End If

                V_DataSet("Update tblVotingParameters Set ParameterValue='" & Me.VoteID.Text & "' Where Parameter ='LastVoteIDForThisSession'", "U")
            Catch ex As Exception
                btnVote.Enabled = True
                btnVote.BackColor = System.Drawing.Color.Silver
                btnVote.Text = "Vote"
                Throw New Exception("Error Getting Quequ")
            End Try
            mq.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Voting System")

            '---if Message Queue had exception, write the voting data to text file for writing to ALIS database late
            If File.Exists(gVotingPath & "Vote-" & gVoteID & ".txt") = False Then
                Dim fs As New FileStream(gVotingPath & "Vote-" & gVoteID & ".txt", FileMode.CreateNew)
                fs.Close()
            End If
            FileOpen(1, gVotingPath & "Vote-" & gVoteID & ".txt", OpenMode.Output)
            Print(1, strBody)
        End Try
    End Sub

    Private Sub btnRequestLastVoteID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRequestLastVoteID.Click
        If SPrimary_On Or SSecondary_On Then
            askVoteID = True
            lblChamberLight.BackColor = Color.Yellow
            lblChamberLight.Text = "Waiting for Last Vote ID"
            btnVote.Enabled = False

            SendMessageToQueue("ASKVOTEID", "")
            'clear()
        End If
    End Sub

    Private Sub ReceiveMessageFromQueue()
        Dim strText As String = ""

        Try
            If (MessageQueue.Exists(gReceiveQueueName)) = False Then
                MessageQueue.Create(gReceiveQueueName)
            End If

            Dim queue As New MessageQueue(gReceiveQueueName)
            queue.Formatter = New XmlMessageFormatter(New String() {"System.String"})
            Dim qenum As MessageEnumerator

            qenum = queue.GetMessageEnumerator2
            While qenum.MoveNext
                Dim m As Message = qenum.Current
                If m.Label = "CLOSEDVOTE" Then
                    msgText = m.Body
                    m = queue.Receive(New TimeSpan(1000))
                    If msgText <> "" Then
                        VoteID.Text = m.Body

                        Me.btnVote.Enabled = True
                        Me.btnUpdateDisplay.Enabled = True
                        Me.OrderOfBusiness.Enabled = True
                        Me.btnExit.Enabled = True
                        Me.lblChamberLight.Text = "Waiting For Voting"
                        gVotingStarted = True
                    Else
                        MsgBox("Incorrect voting data! Please request correct voting data.", MsgBoxStyle.OkOnly, "Request correct voting data")
                        SendMessageToQueue("RECALL", "")
                    End If
                ElseIf m.Label = "VOTEID" Then
                    m = queue.Receive(New TimeSpan(1000))

                    MessageBox.Show("Request Last Vote ID Is: " & m.Body)
                    VoteID.Text = m.Body

                    Me.btnVote.Enabled = False
                    Me.btnUpdateDisplay.Enabled = False
                    Me.OrderOfBusiness.Enabled = False
                    Me.btnExit.Enabled = False
                    Me.lblChamberLight.Text = "Waiting For Voting"
                    gVotingStarted = False
                ElseIf m.Label = "DISPLAYASK" Then
                    MessageBox.Show("Display computer does not receive voted data to display. Please resend it." & m.Body)

                    If File.Exists(gVotingPath & gDisplayTextFile) = False Then
                        Dim fs As New FileStream(gVotingPath & gDisplayTextFile, FileMode.Open, FileAccess.Read)
                        Dim rd As New StreamReader(fs)

                        rd.BaseStream.Seek(0, SeekOrigin.Begin)
                        While (rd.Peek > -1)
                            strText = rd.ReadLine
                        End While
                        SendMessageToDisplay("STARTDISPLAY", strText, gSendQueueToDisplay)
                    Else
                        MessageBox.Show("System does not retreive voting data for displying!" & m.Body)
                    End If
                End If
            End While

            queue.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub bntChang_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If cboDisplay.Text = "Clear Display" Then
            If gChamberHelp Then
                If Not DisplayMessage("Are you sure you want to clear then display?", "Clear Display", "Y") Then
                    Exit Sub
                End If
            End If

            gCalendarCodeDisplayed = ""
            gCalendarCode = ""
            gBillDisplayed = ""
            gBillNbrDisplayed = ""
            Me.lblWorkAreaDisplayed.Visible = False
            Me.btnExit.Enabled = True
            Me.OrderOfBusiness.Enabled = True

            OrderOfBusiness.Enabled = True
            ClearDisplayFlag = True
        Else
            '---send voting information to display PC             bodyParamters = ""
            bodyParamters = "gCalendar - " & Me.CurrentCalendar.Text & "||" & "gBill - " & Me.CurrentBill.Text & "||" & "gCurrentPhrase - " & Me.Phrases.Text
            If SDisplay_On Then
                SendMessageToDisplay(UCase(cboDisplay.Text), bodyParamters, gSendQueueToDisplay)
            Else
                MsgBox("Senate voting display borad computer is off. Unable to change the background.", MsgBoxStyle.Critical, msgText)
            End If
        End If
    End Sub

    Private Sub PingComputer()
        Dim RetValue As New Object
        Dim Ping As Ping
        Dim pReply As PingReply
        Dim mes As String = "Ping Computers"

        Ping = New Ping

        '---ping Voting System Paramery Computer
        pReply = Ping.Send(gPrimary_IPAddress)
        If pReply.Status = IPStatus.Success Then
            SPrimary_On = True
            'RetValue = DisplayMessage("Senate Voting Primary Computer Is On Again.", mes, "I")
        Else
            SPrimary_On = False
            'RetValue = DisplayMessage("Senate Voting Primary Computer Is Off.", mes, "I")
        End If

        '   ping Voting System Secondary Computer
        pReply = Ping.Send(gSecondary_IPAddress)
        If pReply.Status = IPStatus.Success Then
            SSecondary_On = True
            ' RetValue = DisplayMessage("Senate Voting Secondary Computer Is On Again.", mes, "I")
        Else
            SSecondary_On = False
            ' RetValue = DisplayMessage("Senate Voting Seconday Computer Is Off.", mes, "I")
        End If

        If SPrimary_On Or SSecondary_On Then
            lblChamberLight.Visible = True
            lblChamberLight.BackColor = Color.Red
            lblChamberLight.Text = "Waiting to Vote"
            btnVote.Enabled = True

            PingTimer.Enabled = False
            PingTimer.Stop()

            ReceiveTimer.Enabled = True
            ReceiveTimer.Start()
            ReceiveTimer.Interval = gReceiverQueueTimer
        ElseIf SPrimary_On = False And SSecondary_On = False Then
            PingTimer.Enabled = True
            PingTimer.Start()
            PingTimer.Interval = 10000000

            ReceiveTimer.Enabled = False
            ReceiveTimer.Stop()

            lblChamberLight.Visible = True
            lblChamberLight.BackColor = Color.Red
            lblChamberLight.Text = "Unable Communicate to Senate Voting Computers."
            ' RetValue = DisplayMessage("Unable Communicate to Senate Voting Computers.", mes, "I")
            btnVote.Enabled = False
        End If
    End Sub

    Private Sub ReceiveTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReceiveTimer.Tick
        If SPrimary_On Or SSecondary_On Or SDisplay_On Then
            ReceiveMessageFromQueue()
        End If
    End Sub

    Private Sub PingTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PingTimer.Tick
        PingComputer()
    End Sub

    Private Sub btnVoteID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If SPrimary_On Or SSecondary_On Then
            askVoteID = True
            lblChamberLight.BackColor = Color.Yellow
            lblChamberLight.Text = "Waiting for Last Vote ID"
            btnVote.Enabled = False

            SendMessageToQueue("ASKVOTEID", "")
            ' clear()
        End If
    End Sub

    Private Sub WorkData_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WorkData.TextChanged
        Dim WrkFld As String

        '--- always set current phrase to last line
        WrkFld = vbCrLf & Me.WorkData.Text                      ' add crlf since going from end to start and this is a flag and there may only be one phrase
        ' WrkFld = vbTab & vbTab & Me.WorkData.Text                      ' add crlf since going from end to start and this is a flag and there may only be one phrase

        If Strings.Right(WrkFld, 2) = vbCrLf Then
            WrkFld = Mid(WrkFld, 1, Len(WrkFld) - 2)
        End If
        gCurrentPhrase = Mid(WrkFld, InStrRev(WrkFld, vbCrLf) + 2)
        If InStr(Me.WorkData.Text, gBIR) = 1 Then
            gCurrentPhrase = "BIR - " & gCurrentPhrase
        End If

        UpdateWorkDataSW = True
    End Sub

    Private Sub TC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TC.Click
        Select Case TC.SelectedIndex
            Case 0
                tPage0 = True
                tPage1 = False
                tPage2 = False
            Case 1
                tPage0 = False
                tPage1 = True
                tPage2 = False
            Case 2
                tPage0 = False
                tPage1 = False
                tPage2 = True
        End Select
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        If tPage0 Then
            WorkData.Text = ""
        ElseIf tPage1 Then
            WorkData2.Text = ""
        ElseIf tPage2 Then
            WorkData3.Text = ""
        End If
    End Sub

    Private Sub AddPhrase_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        '--- format is Phrase # followed by senator # followed by committee, ex.  3,5,A&F to get
        '--- Phrase #3 followed by senator #5 followed by committee A&F; this senator part and
        '--- committees only applies if there are senator and committee insertion point(s) in the phrase

        Dim fnt As Font
        fnt = Me.WorkData.Font
        Me.WorkData.Font = New Font(fnt.Name, 8, FontStyle.Regular)

        ' LoadSenatorsIntoArray()

        Dim WrkFld As String, WrkFld3 As String, i As Integer
        Dim SNbr As Integer, PNbr As Integer, CAbbrev As String

        WrkFld = Me.AddPhrase.Text
        If Strings.Right$(WrkFld, 1) <> "," Then
            WrkFld = WrkFld & ","
        End If
        WrkFld = ReplaceCharacter(WrkFld, " ", "") ' get rid of all spaces
        ''WrkFld = Strings.Replace(WrkFld, " ", "") ' get rid of all spaces
        If NToB(Me.AddPhrase.Text) = "," Then  ' no senator so exit
            Exit Sub
        End If

        Me.AddPhrase.Text = ""

        PNbr = 0
        SNbr = 0
        CAbbrev = ""

        PNbr = Val(Mid$(WrkFld, 1, InStr(WrkFld, ",") - 1))
        WrkFld = Mid$(WrkFld, InStr(WrkFld, ","))   ' delete up to ,
        If (WrkFld <> ",") And (WrkFld <> ",,") And (WrkFld <> ",,,") Then
            If Strings.Left$(WrkFld, 2) = ",," Then  ' senator and abbrev entered
                CAbbrev = UCase(Mid$(WrkFld, 3, InStr(3, WrkFld, ",") - 3))
            Else
                WrkFld = Mid$(WrkFld, 2)
                SNbr = Val(Mid$(WrkFld, 1, InStr(2, WrkFld, ",") - 1))
                WrkFld = Mid$(WrkFld, InStr(WrkFld, ",")) ' delete up to ,
                If (WrkFld <> ",") And (WrkFld <> ",,") Then
                    CAbbrev = UCase(Mid$(WrkFld, 2, InStr(2, WrkFld, ",") - 2))
                End If
            End If
        End If

        '---capture phrase
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
                WrkFld3 = "Senator " & gSenatorName(SNbr) & Mid$(WrkFld3, 2)
            Else
                WrkFld3 = Mid$(WrkFld3, 1, InStr(WrkFld3, gSenatorInsertionPoint) - 1) & "Senator " & gSenatorName(SNbr) & Mid$(WrkFld3, InStr(WrkFld3, gSenatorInsertionPoint) + 1)
            End If
        End If

        '---see if committee insertion point is in the phrase, and if so add the committee name
        If InStr(WrkFld3, gCommitteeInsertionPoint) > 0 Then
            If CAbbrev = "" Then
            Else
                For i = 1 To gNbrCommittees
                    If gCommitteeAbbrevs(i) = CAbbrev Then
                        If InStr(WrkFld3, gCommitteeInsertionPoint) = 1 Then
                            WrkFld3 = gCommittees(i) & Mid$(WrkFld3, 2)
                        Else
                            WrkFld3 = Mid$(WrkFld3, 1, InStr(WrkFld3, gCommitteeInsertionPoint) - 1) & gCommittees(i) & Mid$(WrkFld3, InStr(WrkFld3, gCommitteeInsertionPoint) + 1)
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

        '---put this phrase in the work area at the phrase insertion point; if no phrase insertion point
        '---then add it to the end of the work area
        If WrkFld3 > "" Then
            If WrkFld3 <> "ADJOURNMENT" Then
                WrkFld3 = WrkFld3 & vbCrLf
                InsertIntoWorkData(WrkFld3, gPhraseInsertionPoint)
            Else
                Dim fntn As Font
                fntn = Me.WorkData.Font
                Me.WorkData.Font = New Font(fntn.Name, 12, FontStyle.Bold)
                Me.WorkData.Enabled = True
                Me.CurrentBill.Text = "ADJOURMENT"
                Me.WorkData.Text = "The senate is in adjournment until "
            End If
        End If
    End Sub

    Private Sub SendMessageToDisplay(ByVal label As String, ByVal strBody As String, ByVal QueueName As String)
        Try
            Dim mq As New MessageQueue
            Dim msg As New Message
            Dim ds As New DataSet

            Try
                'mq.Path = gSendQueueToDisplay
                mq.Path = QueueName

                'queueName = ".\PRIVATE$\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE$\SenateVoteQueue"
                msg.Priority = MessagePriority.Normal
                msg.Label = label
                msg.Body = strBody
                mq.Send(msg)

                DisplayImage = False
            Catch ex As Exception
                Throw New Exception("Error Getting Quequ")
            End Try
            mq.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Voting System")
        End Try

    End Sub

 

    Private Sub SOCNbr_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles SOCNbr.Validating
        Dim ds, dsA, dsC As New DataSet
        Dim drA, drC As DataRow
        Dim str As String
        Dim j As Integer = 0
        Dim FieldName() As Object, FieldValue() As Object, k As Integer
        Dim OID As Long, Title As String, ThisSOCNbr As String

        SOCNbr.AcceptsTab = True

        '--- skip if pure test mode
        If (gTestMode) And (Not gWriteVotesToTest) Then
            Exit Sub
        End If

        If NToB(Me.SOCNbr.Text) = "" Then
            Exit Sub
        End If

        Me.SOCNbr.Text = UCase(Me.SOCNbr.Text)
        If Strings.Left(Me.SOCNbr.Text, 2) <> "SR" Then
            ThisSOCNbr = "SR" & Me.SOCNbr.Text
        Else
            ThisSOCNbr = Me.SOCNbr.Text
        End If

        '---first check for this calendar on Access DB; if not found, then go to ALIS;
        str = "Select CalendarCode From tblCalendars Where CalendarCode='" & ThisSOCNbr & "'"
        ds = V_DataSet(str, "R")

        If ds.Tables(0).Rows.Count > 0 Then
            If Not DisplayMessage("You have already downloaded this special order calendar. " & _
               "Do you want to download it again?", "Download From ALIS", "Y") Then
                ds.Dispose()
                Exit Sub
            End If
            V_DataSet("Delete From tblCalendars Where CalendarCode='" & ThisSOCNbr & "'", "D")
        End If
        ds.Dispose()

        str = "SELECT label, oid_current_document_version " & _
            " FROM ALIS_OBJECT " & _
            " WHERE (oid_session = " & gSessionID & ") AND (oid_house_of_origin = 1753) AND (status_code = '8') AND (instrument = 'T') " & _
            " AND (last_transaction_date IS NOT NULL) AND EXISTS (SELECT 1 FROM Document_Section S , Special_Order_Calendar_Item SOC " & _
            " WHERE SOC.OID_Resolution_Clause = S.OID AND OID_Current_Document_Version = S.OID_Document_Version) AND (label = '" & ThisSOCNbr & "')"
        dsA = A_DataSet(str, "R")

        If dsA.Tables(0).Rows.Count = 0 Then
            RetValue = DisplayMessage("This special order calendar was not found in ALIS", "Calendar Not Found", "S")
            dsA.Dispose()
            Exit Sub
        End If

        '--- add this SOC to VoteSys.mdb
        ReDim FieldValue(2)
        ReDim FieldName(2)
        FieldName(0) = "CalendarCode"
        FieldName(1) = "Calendar"
        FieldName(2) = "ALISID"

        For Each drA In dsA.Tables(0).Rows
            OID = drA("OID_Current_Document_Version")
            FieldValue(0) = drA("Label")
            FieldValue(1) = drA("Label")
            FieldValue(2) = Val(drA("OID_Current_Document_Version"))
            V_DataSet("INSERT INTO tblCalendars  VALUES ('" & drA("Label") & "', '" & drA("Label") & "', " & drA("OID_Current_Document_Version") & ")", "A")
            j += 1
        Next
        dsA.Dispose()

        '--- now get bills for special order calendars
        ReDim FieldValue(6)
        ReDim FieldName(6)
        ds = New DataSet
        ds = V_DataSet("Select * From tblBills", "R")

        For k = 0 To 6
            FieldName(k) = ds.Tables(0).Columns(k).ColumnName
        Next

        str = "SELECT A.label, A.sponsor, A.index_word, SOC.calendar_page " & _
            " FROM SPECIAL_ORDER_CALENDAR_ITEM SOC, ALIS_OBJECT A, MATTER M " & _
            " WHERE (OID_Resolution_Clause IN (SELECT S.OID FROM Document_Section S " & _
            " WHERE S.OID_Document_Version =" & OID & " AND soc.oid_matter = m.oid AND m.oid_instrument = a.oid)) ORDER BY SOC.sequence_number"
        j = 0
        dsA = A_DataSet(str, "R")
        For Each drA In dsA.Tables(0).Rows
            FieldValue(0) = ThisSOCNbr
            FieldValue(1) = drA("Label")
            If Strings.Left(FieldValue(1), 1) = "S" Then
                Title = " by Senator "
            Else
                Title = " by Rep. "
            End If
            FieldValue(3) = NToB(drA("Sponsor"))
            FieldValue(4) = NToB(drA("Index_Word"))
            FieldValue(5) = ""
            FieldValue(6) = NToB(drA("Calendar_Page"))
            If FieldValue(6) <> 0 Then
                FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4) & " p." & FieldValue(6)
            Else
                FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4)
            End If
            V_DataSet("INSERT INTO tblBills VALUES ('" & ThisSOCNbr & "', '" & drA("Label") & "', '" & FieldValue(2) & "', '" & drA("Sponsor") & "', '" & drA("Index_Word") & "', '" & "" & "', '" & drA("Calendar_Page") & "')", "A")
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

        Me.Calendar.SelectedItem = ThisSOCNbr
        Calendar_Click(sender, e)
    End Sub

    Private Sub btnSOC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOC.Click
        frmPreSOC.Show()
    End Sub

    Private Sub btnVote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVote.Click
        ''--- put chamber display parameters in passed table so voting can pick them up; if bir then clear the work area so operator can
        ''--- start building next motion
        If Me.Phrases.Text = "" And Me.Bill.Text = "" Then
            retv = DisplayMessage("You must first select a bill and a motion to vote on.", "No Bill Or Motion Selected", "S")
            Exit Sub
        End If

        Me.btnVote.Enabled = False
        Me.btnUpdateDisplay.Enabled = False
        Me.btnExit.Enabled = False
        gVotingStarted = False

        gPutParams = True
        gPutChamberLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
        ' gPutVotingLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Lime)

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

        gPutPhrase = gCurrentPhrase

        '---new codes
        If Me.Calendar.Text <> "" And Me.CurrentBill.Text <> "" Then
            If SPrimary_On Or SSecondary_On Then
                askVoteID = False
                lblChamberLight.Visible = True
                lblChamberLight.BackColor = Color.Red
                lblChamberLight.Text = "Now Voting"
                btnCancelVote.Enabled = False
                btnExit.Enabled = False
                cboDisplay.Enabled = False

                ReceiveTimer.Enabled = False
                ReceiveTimer.Stop()


                Dim BillText As String = ""
                Select Case Trim(Calendar.Text)
                    Case "Regular Order"
                        BillText = Mid(Bill.Text, 1, InStr(Bill.Text, " ") - 1)
                    Case "Local Bills"
                        BillText = Mid(Bill.Text, 1, InStr(Bill.Text, " ") - 1)
                    Case "Other"
                        BillText = Mid(Bill.Text, 1, InStr(Bill.Text, " ") - 1)
                    Case "Confirmations"
                        BillText = Mid(Bill.Text, 1, InStr(Bill.Text, "-") - 2)
                    Case "Motions"
                        BillText = Bill.Text
                    Case "Special Order Calendar"
                        BillText = Mid(Bill.Text, 1, InStr(Bill.Text, " ") - 1)
                End Select

                '---send voting information to SPrimary or SSecondary PC   
                bodyParamters = ""
                bodyParamters = "gVoteID - " & Me.VoteID.Text & "||" & "gBill - " & BillText & "||" & "gCurrentPhrase - " & gCurrentPhrase()
                If SPrimary_On Or SSecondary_On Then
                    SendMessageToQueue("STARTVOTE", bodyParamters)
                End If

                '---send voting information to display PC 
                If SDisplay_On Then
                    bodyParamters = ""
                    bodyParamters = "gVoteID - " & Me.VoteID.Text & "||" & "gBill - " & Me.Bill.Text & "||" & "gWorkArea - " & Me.WorkData.Text & "||" & "gCurrentOrderOfBusiness - " & CurrentOrderOfBusiness.Text
                    If SDisplay_On Then
                        SendMessageToDisplay(UCase(cboDisplay.Text), bodyParamters, gSendVoteQueueToDisplay)
                    End If
                Else
                    MsgBox("Senate voting display borad computer is off. Unable display voting information.", MsgBoxStyle.Critical, msgText)
                End If

                If gNetwork Then
                    '---write to server and local
                    CreateHTMLPage()
                Else
                    '---write to local only
                    CreateHTMLPage()
                End If


                '---Update LastVoteID to Production or Test mode
                If gTestMode = False Then
                    str = "Update tblVotingParameters Set ParameterValue='" & VoteID.Text & "' Where Parameter='LastVoteIDForThisSession'"
                Else
                    str = "Update tblVotingParameters Set ParameterValue='" & VoteID.Text & "' Where Parameter='LastVoteIDForThisSessionTEST'"
                End If
                V_DataSet(str, "U")


                '---If display PC lost voting information, it will request SOOB1 or SOOB2 resend information to display. 
                '---so write to text file first. SOOB PC is able to retreive voting infromation from the text file and respons the display PC request
                bodyParamters = "gVoteID - " & Me.VoteID.Text & "||" & "gBill - " & BillText & "||" & "gWorkArea - " & Me.WorkData.Text
                If File.Exists(gVotingPath & gDisplayTextFile) = False Then
                    Dim fs As New FileStream(gVotingPath & gDisplayTextFile, FileMode.CreateNew)
                    fs.Close()
                End If
                FileOpen(1, gVotingPath & gDisplayTextFile, OpenMode.Output)
                Print(1, bodyParamters)
                FileClose(1)


                '---Move data from Second WorkData and Thired WorkData Area
                If WorkData2.Text <> "" Then
                    WorkData.Text = WorkData2.Text
                    If WorkData3.Text <> "" Then
                        WorkData2.Text = WorkData3.Text
                        WorkData3.Text = ""
                    Else
                        WorkData2.Text = ""
                    End If
                End If
            Else
                If SPrimary_On = False And SSecondary_On = False Then
                    MsgBox("Senate Voting Parimary and Secondary PCs are offline. Unable send voting information to it. Please contact to Administrator.", MsgBoxStyle.Critical, "Senate Voting System")
                    '!!! write vote information on local 
               
                End If
            End If
        Else
            If Me.CurrentBill.Text = "" Then
                MsgBox("Please select bill for voting.")
                Me.CurrentBill.Focus()
            ElseIf Me.Calendar.Text = "" Then
                MsgBox("Please select Calendar for voting.")
            End If
            Me.btnVote.Enabled = True
            Me.btnUpdateDisplay.Enabled = True
            Me.OrderOfBusiness.Enabled = True
            Me.btnExit.Enabled = True
            Me.lblChamberLight.Text = "Display Next Bill"
            gVotingStarted = True
        End If
    End Sub


    Private Sub btnRecallDisplayedBill_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecallDisplayedBill.Click
        If gBillDisplayed = "" Then
            RetValue = DisplayMessage("No bill has been displayed yet", "Nothing To Recall", "S")
        Else
            Me.Calendar.Text = gCalendarDisplayed
            Calendar_Click(Calendar, New System.EventArgs())
            Me.Bill.Text = gBillDisplayed
            Bill_Click(Bill, New System.EventArgs())
        End If
    End Sub

    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click
        '---send voting information to display PC 
        bodyParamters = ""
        bodyParamters = "gBill - " & Me.Bill.Text & "||" & "gWorkArea - " & Me.WorkData.Text & "||" & "gCurrentOrderOfBusiness - " & CurrentOrderOfBusiness.Text

        If SDisplay_On Then
            DisplayImage = True
            SendMessageToDisplay(UCase(cboDisplay.Text), bodyParamters, gSendQueueToDisplay)
        Else
            DisplayImage = False
            MsgBox("Senate voting display borad computer is off. Unable to change the background.", MsgBoxStyle.Critical, msgText)
        End If
    End Sub

#Region "Phrase and Senator drag-drop; click codes"

    Private Sub WorkData_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles WorkData.DragEnter
        ' ----- Yes, we accept the items.
        If (e.Data.GetDataPresent(frmPhraseShow.lstPhrase.SelectedItems.GetType()) = True) Then e.Effect = DragDropEffects.Move
    End Sub

    Private Sub WorkData_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles WorkData.DragDrop
        ' ----- Accept the dropped items.
        For Each oneItem As Object In e.Data.GetData(frmPhraseShow.lstPhrase.SelectedItems.GetType())
            If bPhrase Then
                If WorkData.Text = "" Then
                    WorkData.Text = Mid(oneItem, InStr(oneItem, " - ") + 3)
                Else
                    If Strings.Right(WorkData.Text, 2) <> vbCrLf Then
                        Me.WorkData.Text = Me.WorkData.Text & vbCrLf & Mid(oneItem, InStr(oneItem, " - ") + 3)
                    Else
                        Me.WorkData.Text = Me.WorkData.Text & Mid(oneItem, InStr(oneItem, " - ") + 3)
                    End If
                End If
            ElseIf bSenator Then
                InsertIntoWorkData("Senator" & Me.Senators.Text, gSenatorInsertionPoint)
            End If
        Next oneItem
    End Sub

    Private Sub WorkData2_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles WorkData2.DragEnter
        ' ----- Yes, we accept the items.
        If (e.Data.GetDataPresent(frmPhraseShow.lstPhrase.SelectedItems.GetType()) = True) Then e.Effect = DragDropEffects.Move
    End Sub

    Private Sub WorkData2_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles WorkData2.DragDrop
        ' ----- Accept the dropped items.
        For Each oneItem As Object In e.Data.GetData(frmPhraseShow.lstPhrase.SelectedItems.GetType())
            If bPhrase Then
                If WorkData2.Text = "" Then
                    WorkData2.Text = Mid(oneItem, InStr(oneItem, " - ") + 3)
                Else
                    If Strings.Right(WorkData.Text, 2) <> vbCrLf Then
                        Me.WorkData2.Text = Me.WorkData2.Text & vbCrLf & Mid(oneItem, InStr(oneItem, " - ") + 3)
                    Else
                        Me.WorkData2.Text = Me.WorkData2.Text & Mid(oneItem, InStr(oneItem, " - ") + 3)
                    End If
                End If
            ElseIf bSenator Then
                InsertIntoWorkData("Senator" & Me.Senators.Text, gSenatorInsertionPoint)
            End If
        Next oneItem
    End Sub

    Private Sub WorkData3_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles WorkData3.DragEnter
        ' ----- Yes, we accept the items.
        If (e.Data.GetDataPresent(frmPhraseShow.lstPhrase.SelectedItems.GetType()) = True) Then e.Effect = DragDropEffects.Move
    End Sub

    Private Sub WorkData3_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles WorkData3.DragDrop
        ' ----- Accept the dropped items.
        For Each oneItem As Object In e.Data.GetData(frmPhraseShow.lstPhrase.SelectedItems.GetType())
            If bPhrase Then
                If WorkData3.Text = "" Then
                    WorkData3.Text = Mid(oneItem, InStr(oneItem, " - ") + 3)
                Else
                    If Strings.Right(WorkData3.Text, 2) <> vbCrLf Then
                        Me.WorkData3.Text = Me.WorkData3.Text & vbCrLf & Mid(oneItem, InStr(oneItem, " - ") + 3)
                    Else
                        Me.WorkData3.Text = Me.WorkData3.Text & Mid(oneItem, InStr(oneItem, " - ") + 3)
                    End If
                End If
            ElseIf bSenator Then
                InsertIntoWorkData("Senator" & Me.Senators.Text, gSenatorInsertionPoint)
            End If
        Next oneItem
    End Sub
#End Region


#Region "old codes"
    ''--- print the doc in HTML and put on the web server so all can see - done everytime the chamber
    ''--- display operator hits update display
    'Dim ds As New DataSet
    'Dim dr As DataRow
    'Dim WrkFld, str As String, i As Integer, RetValue As Object

    'Try
    '    str = "Select * From tblOrderOfBusiness"
    '    ds = V_DataSet(str, cnVoting, "R")

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
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, vbCrLf) + 2)
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

    'If Left$(Me!CurrentBill, 2) = "CF" Then
    '    If Pfrm!BillDisplayBoard.Height <> 3 * gSaveBillHeight Then
    '        Pfrm!BillDisplayBoard.Height = 3 * gSaveBillHeight
    '        Pfrm!PhraseDisplayBoard.Height = Pfrm!PhraseDisplayBoard.Height - gSaveBillHeight
    '        Pfrm!PhraseDisplayBoard.Top = Pfrm!BillDisplayBoard.Top + (3 * gSaveBillHeight)
    '    End If
    '    If gFreeFormat Then   ' can free format confirmations
    '        Pfrm!BillDisplayBoard.Text = Me!CurrentBill
    '    Else
    '        Pfrm!BillDisplayBoard.Text = Mid$(Me!CurrentBill, 1, InStr(Me!CurrentBill, " -- ") - 1) & vbCrLf & Mid$(Me!CurrentBill, InStr(Me!CurrentBill, " -- ") + 4)
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
    '            Pfrm!BillDisplayBoard.Text = Mid$(gBill, j)
    '            gStart = 0
    '            gLength = Len(Mid$(gBill, j))
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
    '        gLength = Len(Mid$(gBill, 1, i - 1)) ' pick up to the - separating bill and subject
    '        Pfrm!BillDisplayBoard.Text = Mid$(gBill, 1, i) & Pfrm!BillDisplayBoard.Text
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
    ''        mq.Path = gSendQueueName                         'queueName = ".\PRIVATE$\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE$\SenateVoteQueue"
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
    'dim MyQueue as new MessageQueue(".\privete$\myQueue")
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
    '    ''Dim mq As New MessageQueue("FormatName:DIRECT=OS:LEG13\PRIVATE$\SenateVoteQueue")

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
    '        ds = V_DataSet(str, cnVoting, "R")


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
    '                WrkFld = Mid$(WrkFld, InStr(WrkFld, vbCrLf) + 2)
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
    '        ds = V_DataSet(str, cnVoting, "R")


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
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, vbCrLf) + 2)
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
#End Region

   
  
End Class