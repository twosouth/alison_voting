Imports System
Imports System.Windows.Forms
Imports System.Data
Imports System.Data.OleDb
Imports System.Exception

Public Class frmPreSOC
    Private frmOpen As Boolean
    Private socExist, addL, addR, added, unSOC As Boolean
    Private strSOCLabel, strNewSR As String

    Private Sub frmPreSOC_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadCalendar()
        txtFindBill.Focus()
    End Sub

    Private Sub LoadCalendar()
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim str As String

        Try
            lstSOCCalendar.Items.Clear()
            lstSOCBills.Items.Clear()

            '--- initialize calendar and bills   
            str = "Select Bill From tblBills Where CalendarCode='1' Order By Bill"
            ds = V_DataSet(str, "R")
            For Each dr In ds.Tables(0).Rows
                lstSOCCalendar.Items.Add(dr("Bill"))
            Next
            lstSOCCalendar.SelectedIndex = 0
        Catch ex As Exception
            DisplayMessage(ex.Message, "Download Calendar Bills", "S")
        End Try
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        lstSOCBills.Items.Clear()
        txtFindSOC.Text = ""
        txtFindBill.Text = ""
        txtFindBill.Focus()
        ' VisibleTrue()

        added = False
        addL = False
        addR = False
        txtFindBill.Text = ""
        txtFindBill.Text = ""
        txtAddL.Text = ""
        txtAddR.Text = ""
        lblSR.Text = ""
        txtFindBill.Focus()
        unSOC = False
        chkUSOC.Checked = False
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnPUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPUp.Click
        Dim item1, item2 As String
        Dim index1, index2 As Integer
      
            If lstSOCBills.SelectedIndex <> -1 And lstSOCBills.SelectedIndex <> 0 Then
                '---Highlined item
                item1 = Me.lstSOCBills.SelectedItem.ToString
                index1 = Me.lstSOCBills.SelectedIndex

                index2 = index1 - 1
                item2 = Me.lstSOCBills.Items(index2).ToString

                Me.lstSOCBills.Items.Item(index2) = item1
                Me.lstSOCBills.Items.Item(index1) = item2

                index1 = 0
                index2 = 0

                Me.lstSOCBills.SelectedIndex = Me.lstSOCBills.SelectedIndex - 1
        End If
    End Sub

    Private Sub btnPDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPDown.Click
        Dim item1, item2 As String
        Dim index1, index2 As Integer

        If lstSOCBills.SelectedIndex <> -1 And lstSOCBills.SelectedIndex < lstSOCBills.Items.Count Then
            '---Highlined item
            item1 = Me.lstSOCBills.SelectedItem.ToString
            index1 = Me.lstSOCBills.SelectedIndex

            index2 = index1 + 1
            If index2 < Me.lstSOCBills.Items.Count Then
                item2 = Me.lstSOCBills.Items(index2).ToString

                Me.lstSOCBills.Items.Item(index2) = item1
                Me.lstSOCBills.Items.Item(index1) = item2

                index1 = 0
                index2 = 0

                '--- Highline 
                Me.lstSOCBills.SelectedIndex = Me.lstSOCBills.SelectedIndex + 1
            End If
        End If
    End Sub

    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click
        If lstSOCCalendar.SelectedIndex <> -1 And lstSOCCalendar.SelectedIndex <= lstSOCCalendar.Items.Count - 1 Then
            Me.lstSOCCalendar.SelectedIndex = Me.lstSOCCalendar.SelectedIndex + 1
        End If
    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        If lstSOCCalendar.SelectedIndex <> -1 And lstSOCCalendar.SelectedIndex <> 0 Then
            Me.lstSOCCalendar.SelectedIndex = Me.lstSOCCalendar.SelectedIndex - 1
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim sw As New Stopwatch
        Dim newSR As String = ""
        Dim label, lstValue As String
        Dim dsBill As New DataSet

        Try
            If lstSOCBills.Items.Count <> 0 Then
                V_DataSet("Delete From tblCalendarS Where CalendarCode ='SOC'", "D")
                V_DataSet("DeletE From tblSpecialOrderCalendar Where CalendarCode ='SOC'", "D")
                V_DataSet("Insert Into tblCalendars Values ('SOC', 'Special Order Calendar', 0)", "A")

                For i As Integer = 0 To lstSOCBills.Items.Count - 1
                    label = ""
                    lstValue = lstSOCBills.Items(i).ToString
                    label = Mid(lstValue, 1, InStr(lstValue, " ") - 1)
                    dsBill = V_DataSet("Select * From tblBills Where ucase(Bill) ='" & UCase(lstValue) & "'", "R")
                    If dsBill.Tables(0).Rows.Count > 0 Then
                        For Each dr As DataRow In dsBill.Tables(0).Rows
                            strSQL = ""
                            strSQL = "Insert Into tblSpecialOrderCalendar Values ('SOC', '" & dr("BillNbr") & "','" & dr("Bill") & "','" & dr("Sponsor") & "','" & Replace(dr("Subject"), "'", "''") & "','" & Replace(dr("WorkData"), "'", "''") & "','" & dr("CalendarPage") & "','" & dr("SenatorSubject") & "','" & dr("BillCalendarPage") & "')"
                            V_DataSet(strSQL, "A")
                        Next
                    End If
                Next

                '---Finding out 'Order Of Business' window does open or close 
                sw.Start()
                For i As Int16 = 0 To My.Application.OpenForms.Count - 1
                    If My.Application.OpenForms.Item(i).Text = "Senate Voting System - Order Of Business" Then
                        frmOpen = True
                        Exit For
                    Else
                        frmOpen = False
                    End If
                Next
                sw.Stop()

                '--- paste Special Order Calendar List Bills to 'Order Of Business' window
                frmChamberDisplay.Bill.Items.Clear()
                If lstSOCBills.Items.Count <> 0 Then
                    For j As Integer = 0 To Me.lstSOCBills.Items.Count - 1
                        frmChamberDisplay.Bill.Items.Add(Me.lstSOCBills.Items(j).ToString)
                    Next
                Else
                    For j As Integer = 0 To Me.lstSOCCalendar.Items.Count - 1
                        frmChamberDisplay.Bill.Items.Add(Me.lstSOCCalendar.Items(j).ToString)
                    Next
                End If

                '---set foucse on passed SR
                For i As Integer = 0 To frmChamberDisplay.Calendar.Items.Count - 1
                    If frmChamberDisplay.Calendar.Items(i).ToString = "Spcial Order Calendar" Then
                        frmChamberDisplay.Calendar.SelectedIndex = i
                        frmChamberDisplay.Calendar.SelectedItem = "Spcial Order Calendar"
                    End If
                Next

                '--- if 'Chamber Display' window is close, open it first         
                If frmOpen = False Then
                    frmChamberDisplay.MdiParent = frmMain
                    frmChamberDisplay.Show()
                    frmChamberDisplay.BringToFront()
                Else
                    frmChamberDisplay.Close()
                    frmChamberDisplay.MdiParent = frmMain
                    frmChamberDisplay.Show()
                    frmChamberDisplay.BringToFront()
                End If

                unSOC = False
                chkUSOC.Checked = False
                lblSR.Text = ""
                '  Me.Close()
            Else
                MsgBox("Please add special order calendar bills", MsgBoxStyle.Information, "Add Bills")
                Exit Sub
            End If
        Catch ex As Exception
            '  DisplayMessage(ex.Message, "Download Bills", "S")
        End Try
    End Sub

    Private Function SOCBillExist(ByVal SOCBill As String) As Boolean
        If Me.lstSOCBills.Items.Count > 0 Then
            For j As Integer = 0 To Me.lstSOCBills.Items.Count - 1
                If UCase(SOCBill) = Me.lstSOCBills.Items(j).ToString Then
                    MsgBox(SOCBill & " is existed.", MsgBoxStyle.Information)
                    SOCBillExist = True
                    Exit Function
                End If
            Next
        Else
            SOCBillExist = False
        End If
    End Function

    Private Sub txtFindBill_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtFindBill.KeyDown
        Try
            If e.KeyCode = Keys.Enter And txtFindBill.Text <> "" Then
                Dim ds, dsA As New DataSet
                Dim billsArray() As String = {""}
                Dim cBill, sBill As String
                Dim itemFound As Boolean = False
                Dim intFound As Integer = 0
                Dim i As Integer = InStr(txtFindBill.Text, ";")

                '--- if i > 0 inputed multiple bills (example: hb2;hb123;sb23...), else a single bill was inputed
                If i > 0 Then
                    billsArray = Split(txtFindBill.Text, ";")
                End If

                If lstSOCCalendar.Items.Count <> 0 And lstSOCCalendar.Items.Count > 0 Then
                    '-- input a single bill
                    If i < 1 Then
                        For x As Integer = 0 To lstSOCCalendar.Items.Count - 1
                            cBill = Mid(lstSOCCalendar.Items(x).ToString, 1, InStr(lstSOCCalendar.Items(x).ToString, " ") - 1)
                            If UCase(txtFindBill.Text) = UCase(cBill) Then        '--found the bill
                                lstSOCCalendar.SelectedIndex = x
                                For y As Integer = 0 To lstSOCBills.Items.Count - 1
                                    sBill = Mid(lstSOCBills.Items(y).ToString, 1, InStr(lstSOCBills.Items(y).ToString, " ") - 1)
                                    If UCase(txtFindBill.Text) = UCase(sBill) Then
                                        itemFound = True
                                        intFound = 1
                                        MsgBox("Bill " & UCase(sBill) & " already added in list!", MsgBoxStyle.Information, msgText)
                                    Else
                                        itemFound = False
                                        intFound = 0
                                    End If
                                Next
                                If itemFound = False Then
                                    lstSOCBills.Items.Add(lstSOCCalendar.Items(x).ToString)
                                    intFound = 1
                                End If
                            End If
                        Next
                        If intFound = 0 Then
                            MsgBox("can't find " & UCase(txtFindBill.Text) & " in Regular Order.", MsgBoxStyle.Information, msgText)
                        End If
                    Else
                        '-- multiple input bills (example: HB2;HB234;SB12)
                        For j As Integer = 0 To billsArray.Length - 1
                            intFound = 0
                            For x As Integer = 0 To lstSOCCalendar.Items.Count - 1
                                cBill = Mid(lstSOCCalendar.Items(x).ToString, 1, InStr(lstSOCCalendar.Items(x).ToString, " ") - 1)
                                If UCase(billsArray(j)) = UCase(cBill) Then        '--found the bill
                                    lstSOCCalendar.SelectedIndex = x
                                    For y As Integer = 0 To lstSOCBills.Items.Count - 1
                                        sBill = Mid(lstSOCBills.Items(y).ToString, 1, InStr(lstSOCBills.Items(y).ToString, " ") - 1)
                                        If UCase(billsArray(j)) = UCase(sBill) Then
                                            itemFound = True
                                            intFound = 1
                                            MsgBox("Bill " & UCase(sBill) & " already added in list!", MsgBoxStyle.Information, msgText)
                                        Else
                                            itemFound = False
                                            intFound = 0
                                        End If
                                    Next
                                    If itemFound = False Then
                                        lstSOCBills.Items.Add(lstSOCCalendar.Items(x).ToString)
                                        intFound = 1
                                    End If
                                End If
                            Next
                            If intFound = 0 Then
                                MsgBox("can't find " & UCase(billsArray(j)) & " in Regular Order.", MsgBoxStyle.Information, msgText)
                            End If
                        Next
                    End If
                    txtFindBill.Text = ""
                    txtFindBill.Focus()
                End If
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Find The Bill", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub btnReload_Click(sender As Object, e As System.EventArgs) Handles btnReload.Click
        lstSOCCalendar.Items.Clear()
        lstSOCBills.Items.Clear()
        LoadCalendar()
     
        txtFindBill.Text = ""
        txtFindBill.Text = ""
        txtAddL.Text = ""
        txtAddR.Text = ""
        txtFindBill.Focus()
    End Sub

    Private Sub txtFindSOC_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtFindSOC.KeyDown
        Dim ds, dsA, dsRecord, dsCheck, dsStart As New DataSet
        Dim itemFound As Boolean = False
        Dim intFound As Integer = 0
        Dim intSoc, intdoc As Integer
        Dim strSOC As String

        Try
            If e.KeyCode = Keys.Enter And txtFindSOC.Text <> "" Then
                '--- skip if pure test mode
                If (gTestMode) And (Not gWriteVotesToTest) Then
                    Exit Sub
                End If

                If NToB(txtFindSOC.Text) = "" Then
                    Exit Sub
                End If

                intSoc = InStr(UCase(Trim(txtFindSOC.Text)), "SR")
                intdoc = InStr(UCase(Trim(txtFindSOC.Text)), "-")
                strNewSR = Me.txtFindSOC.Text

                If intdoc = 0 And intSoc > 0 Then
                    strSQL = " SELECT oid_current_document_version, oid, label, id, last_transaction_date " & _
                                " FROM alis_object" & _
                                " WHERE oid_session = " & gSessionID & _
                                " AND label ='" & UCase(Trim(txtFindSOC.Text)) & "'"
                ElseIf intdoc > 0 And intSoc = 0 Then
                    strSQL = " SELECT a.oid_current_document_version, a.oid, a.label, a.id,a.last_transaction_date , d.label " & _
                                " FROM alis_object a, document_version d " & _
                                " WHERE a.oid_session =" & gSessionID & " AND" & _
                                " a.OID_CURRENT_DOCUMENT_VERSION  = d.oid AND" & _
                                " d.label ='" & UCase(Trim(txtFindSOC.Text)) & "'"
                End If
                dsStart = ALIS_DataSet(strSQL, "R")

                '---Case 1: Released Special Order Calendar
                If dsStart.Tables(0).Rows.Count > 0 Then
                    lstSOCBills.Items.Clear()
                    lstSOCCalendar.Items.Clear()
                    unSOC = False
                    chkUSOC.Checked = False

                    '*** 1 check correct input
                    If intSoc = 0 And intdoc = 0 Then
                        MsgBox("It is not right name or document id. (Example: SR23 or 16739-1.)", MsgBoxStyle.Information, msgText)
                        txtFindSOC.Text = ""
                        txtFindSOC.Focus()
                        Exit Sub
                        '*** 2 entered SR name
                    ElseIf intSoc > 0 And intdoc = 0 Then
                        '---first check for this calendar on Access DB; if not found, then go to ALIS;
                        strSQL = "Select CalendarCode From tblCalendars Where ucase(CalendarCode)='" & UCase(Trim(txtFindSOC.Text)) & "'"
                        ds = V_DataSet(strSQL, "R")

                        If ds.Tables(0).Rows.Count > 0 Then
                            lstSOCCalendar.Items.Clear()
                            dsRecord = V_DataSet("Select * From tblSpecialOrderCalendar Where ucase(CalendarCode) ='" & UCase(txtFindSOC.Text) & "'", "R")

                            If dsRecord.Tables(0).Rows.Count > 0 Then
                                For Each drR In dsRecord.Tables(0).Rows
                                    lstSOCCalendar.Items.Add(drR("Bill"))
                                Next
                            End If
                        Else
                            strSOC = UCase(Trim(txtFindSOC.Text))
                            strSOCLabel = strSOC
                            GetSOCBills(strSOC, "", gSessionID)
                        End If
                    ElseIf intdoc > 0 And intSoc = 0 Then
                        strSQL = " SELECT a.oid_current_document_version, a.oid, a.label, a.id,a.last_transaction_date , d.label " & _
                                " FROM alis_object a, document_version d " & _
                                " WHERE a.oid_session =" & gSessionID & " AND" & _
                                " a.OID_CURRENT_DOCUMENT_VERSION  = d.oid AND" & _
                                " d.label ='" & Replace(Trim(txtFindSOC.Text), " ", "") & "'"
                        dsCheck = ALIS_DataSet(strSQL, "R")
                        For Each dr As DataRow In dsCheck.Tables(0).Rows
                            strSOCLabel = dr("Label")
                        Next

                        '---first check for this calendar on Access DB; if not found, then go to ALIS;
                        strSQL = "Select CalendarCode From tblCalendars Where ucase(CalendarCode)='" & UCase(strSOCLabel) & "'"
                        ds = V_DataSet(strSQL, "R")

                        If ds.Tables(0).Rows.Count > 0 Then
                            dsRecord = V_DataSet("Select * From tblSpecialOrderCalendar Where ucase(CalendarCode) ='" & UCase(strSOCLabel) & "'", "R")

                            If dsRecord.Tables(0).Rows.Count > 0 Then
                                For Each drR In dsRecord.Tables(0).Rows
                                    lstSOCCalendar.Items.Add(drR("Bill"))
                                Next
                            End If
                        Else
                            strSOC = UCase(Trim(txtFindSOC.Text))
                            GetSOCBills("", Replace(Trim(strSOC), " ", ""), gSessionID)
                        End If
                    End If
                    Me.lblSR.Text = Me.txtFindSOC.Text
                Else
                    '---Case 2: un-release 
                End If
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Download SR", "S")
            Exit Sub
        End Try
    End Sub

    Private Function GetSOCBills(SRLabel As String, DocID As String, SessionOID As Integer) As DataSet
        Dim ds, dsLocalBills As New DataSet
        Dim da As OleDbDataAdapter
        Dim command As OleDbCommand
        Dim reader As OleDbDataReader
        Dim varQueryI As String = ""
        Dim varQueryB, Title As String
        Dim FieldValue() As Object

        Try
            If cnALIS.State = ConnectionState.Open Then cnALIS.Close()

            '-- load Special Order Calendar bills
            If DocID = "" And SRLabel <> "" Then
                varQueryI = " SELECT oid_current_document_version, oid, label, id, last_transaction_date " & _
                            " FROM alis_object" & _
                            " WHERE oid_session = " & SessionOID & _
                            " AND label ='" & SRLabel & "'"
            ElseIf DocID <> "" And SRLabel = "" Then
                varQueryI = " SELECT a.oid_current_document_version, a.oid, a.label, a.id,a.last_transaction_date , d.label " & _
                            " FROM alis_object a, document_version d " & _
                            " WHERE a.oid_session =" & SessionOID & " AND" & _
                            " a.OID_CURRENT_DOCUMENT_VERSION  = d.oid AND" & _
                            " d.label ='" & DocID & "'"
            End If
            command = New OleDbCommand(varQueryI, cnALIS)
            cnALIS.Open()
            reader = command.ExecuteReader()

            If reader.HasRows Then
                Do While reader.Read()
                    strSOCLabel = reader.Item(2).ToString
                    If added = False Then
                        varQueryB = "SELECT a.oid, a.label, a.id, a.sponsor, a.index_word, a.OID_SPONSOR, soc.calendar_page, soc.sequence_number" & _
                                     " FROM special_order_calendar_item soc, alis_object a, matter m" & _
                                     " WHERE OID_Resolution_Clause IN" & _
                                         "	(SELECT S.OID" & _
                                         "	 FROM   Document_Section S" & _
                                         "	 WHERE S.OID_Document_Version = " & reader.Item(0).ToString & _
                                         "	)" & _
                                         " AND soc.oid_matter = m.oid" & _
                                         " AND m.oid_instrument= a.oid" & _
                                         " AND A.OID_SESSION =" & gSessionID & _
                                     " ORDER BY soc.sequence_number"
                    Else
                        varQueryB = "SELECT distinct (a.oid_current_document_version), a.oid, a.label, a.id, a.sponsor, a.index_word, soc.calendar_page" & _
                                    " FROM alis_object a, special_order_calendar_item soc, Matter m" & _
                                    " WHERE a.oid_session = " & gSessionID & _
                                        " AND a.label ='" & strSOCLabel & "'" & _
                                        " AND soc.oid_matter = m.oid" & _
                                        " AND m.oid_instrument= a.oid"
                    End If
                    da = New OleDbDataAdapter(varQueryB, cnALIS)
                    da.Fill(ds, "Table")
                Loop
                reader.Close()

                ReDim FieldValue(8)

                For Each dr As DataRow In ds.Tables(0).Rows
                    FieldValue(0) = "S"
                    If IsDBNull(dr("Label")) Then
                        FieldValue(1) = ""
                    Else
                        FieldValue(1) = dr("Label")
                    End If
                    If Strings.Left(FieldValue(1), 1) = "S" Then
                        Title = " by Senator "
                    Else
                        Title = " by Rep. "
                    End If
                    If IsDBNull(dr("Sponsor")) Then
                        FieldValue(3) = ""
                    Else
                        FieldValue(3) = dr("Sponsor")
                    End If
                    If IsDBNull(dr("Index_Word")) Then
                        FieldValue(4) = ""
                    Else
                        FieldValue(4) = NToB(Replace(dr("Index_Word"), "'", " "))
                    End If
                    If IsDBNull(FieldValue(5)) Or Not IsDBNull(FieldValue(5)) Then
                        FieldValue(5) = ""
                    End If
                    If IsDBNull(dr("Calendar_Page")) Then
                        FieldValue(6) = 0
                    Else
                        FieldValue(6) = dr("Calendar_Page")
                    End If
                    If FieldValue(6) <> 0 Then
                        FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4) & " p." & FieldValue(6)
                    Else
                        FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4)
                    End If

                    '--- new
                    Dim strTitle As String
                    If Strings.Left(FieldValue(1), 1) = "S" Then
                        strTitle = " by Senator "
                    Else
                        strTitle = " by Rep. "
                    End If


                    If FieldValue(6) <> 0 Then
                        FieldValue(8) = FieldValue(1) & "&nbsp;&nbsp;&nbsp;&nbsp;" & " p." & FieldValue(6)
                    Else
                        FieldValue(7) = strTitle & " " & FieldValue(3) & " - " & FieldValue(4)
                    End If

                    ''-- insert SOCls bill into local tblSpecialOrderCalendar
                    strSQL = "Insert Into tblSpecialOrderCalendar Values ('" & strSOCLabel & "', '" & FieldValue(1) & "','" & FieldValue(2) & "','" & FieldValue(3) & "','" & FieldValue(4) & "', '', '" & FieldValue(6) & "', '" & Replace(FieldValue(7), "'", "") & "', '" & Replace(FieldValue(8), "'", "") & "')"
                    V_DataSet(strSQL, "A")

                    '-- add found SOC's bills into listbox
                    If added = False Then
                        lstSOCCalendar.Items.Add(FieldValue(2))
                    Else
                        If addL And addR = False Then
                            lstSOCCalendar.Items.Add(FieldValue(2))
                        End If
                        If addL = False And addR Then
                            lstSOCBills.Items.Add(FieldValue(2))
                        End If
                    End If

                    If added Then
                        Exit For
                    End If
                Next

                '--insert entered 'SR...' into tblCalendar table
                If added = False Then
                    V_DataSet("Insert Into tblCalendars Values ('" & UCase(strSOCLabel) & "','" & strSOCLabel & "', 0)", "A")
                End If
                cnALIS.Close()
                txtFindSOC.Text = ""
            End If
            GetSOCBills = ds

        Catch ex As Exception
            DisplayMessage(ex.Message, "Download Bills", "S")
        End Try
    End Function
  
    Private Sub TabBills_Click(sender As Object, e As System.EventArgs) Handles TabBills.Click
        If TabBills.SelectedIndex = 0 Then
            LoadCalendar()
            btnDeleteSOC.Enabled = False
        ElseIf TabBills.SelectedIndex = 1 Then
            btnDeleteSOC.Enabled = True
            Me.txtFindSOC.Focus()
        End If
    End Sub

    Private Sub btnRightSOC_Click(sender As System.Object, e As System.EventArgs) Handles btnRightSOC.Click
        If lstSOCCalendar.SelectedIndex <> -1 Then
            If SOCBillExist(Me.lstSOCCalendar.Items(lstSOCCalendar.SelectedIndex).ToString) = False Then
                lstSOCBills.Items.Add(lstSOCCalendar.SelectedItem.ToString)
                If lstSOCBills.Items.Count > 0 Then
                    lstSOCBills.SelectedIndex = 0
                End If
                If lstSOCCalendar.SelectedIndex < lstSOCCalendar.Items.Count - 1 Then
                    lstSOCCalendar.SelectedIndex = lstSOCCalendar.SelectedIndex + 1
                End If
            End If
        End If
    End Sub

    Private Sub btnLeftSOC_Click(sender As System.Object, e As System.EventArgs) Handles btnLeftSOC.Click
        If lstSOCBills.SelectedIndex <> -1 Then
            lstSOCBills.Items.Remove(lstSOCBills.SelectedItem.ToString)
        End If
    End Sub

    Private Sub LoadStoredSpecialOrderCalendar()
        Dim ds As New DataSet

        Try
            ds = V_DataSet("Select * From tblSpecialOrderCalendar", "R")
            If ds.Tables(0).Rows.Count > 0 Then
                socExist = True
                lstSOCCalendar.Items.Clear()
                For Each dr As DataRow In ds.Tables(0).Rows
                    lstSOCCalendar.Items.Add(dr("Bill"))
                Next
            Else
                socExist = False
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Down Load Bills", "S")
        End Try
    End Sub

    Private Sub btnDeleteSOC_Click(sender As System.Object, e As System.EventArgs)
        Dim sw As New Stopwatch
        Dim ds As New DataSet

        Try
            If MsgBox("Are you sure want to delete all of the today stored Special Order Calendar?", MsgBoxStyle.YesNo, msgText) = MsgBoxResult.Yes Then
                V_DataSet("Delete From tblSpecialOrderCalendar", "D")
                V_DataSet("DELETE FROM tblCalendars WHERE LEFT(CalendarCode, 2)='SR'", "D")
                V_DataSet("DELETE FROM tblCalendars WHERE CalendarCode='SOC'", "D")
                lstSOCCalendar.Items.Clear()
                lstSOCBills.Items.Clear()
                txtFindSOC.Focus()

                sw.Start()
                For i As Int16 = 0 To My.Application.OpenForms.Count - 1
                    If My.Application.OpenForms.Item(i).Text = "Senate Voting System - Order Of Business" Then
                        frmOpen = True
                    End If
                Next
                sw.Stop()

                '--- if 'Order Of Business' window is close, open it first, then remove specific "SR"
                If frmOpen = False Then
                    frmChamberDisplay.Show()
                    frmChamberDisplay.BringToFront()
                Else
                    For i As Integer = 0 To frmChamberDisplay.Calendar.Items.Count - 1
                        If Strings.Left(frmChamberDisplay.Calendar.Items(i).ToString, 2) = "SR" Or Strings.Left(frmChamberDisplay.Calendar.Items(i).ToString, 2) = "sr" Then
                            frmChamberDisplay.Calendar.Items.RemoveAt(i)
                            Exit For
                        End If
                    Next
                End If

                '--After removed "SR", initialize calendar as 'Regular Order' bills
                ds = GetBills("Regular Order")
                If ds.Tables(0).Rows.Count > 0 Then
                    frmChamberDisplay.Bill.Items.Clear()
                    For Each dr As DataRow In ds.Tables(0).Rows
                        frmChamberDisplay.Bill.Items.Add(dr("Bill"))
                    Next
                End If
                frmChamberDisplay.Calendar.SelectedIndex = 0
                unSOC = False
                chkUSOC.Checked = False
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Delete SR Bills", "S")
        End Try
    End Sub

    Private Sub txtAddL_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAddL.KeyDown
        If lstSOCCalendar.Items.Count = 0 Then
            If e.KeyCode = Keys.Enter Then
                added = True
                addL = True
                addR = False
                GetSOCBills(Trim(UCase(txtAddL.Text)), "", gSessionID)
                txtAddL.Text = ""
            End If
        Else
            lstSOCCalendar.Items.Clear()
            Exit Sub
        End If
    End Sub

    Private Sub txtAddR_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtAddR.KeyDown
        If e.KeyCode = Keys.Enter Then
            added = True
            addL = False
            addR = True
            GetSOCBills(Trim(UCase(txtAddR.Text)), "", gSessionID)
            txtAddR.Text = ""
        End If
    End Sub

    Private Sub tbnClearL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbnClearL.Click
        lstSOCCalendar.Items.Clear()
        lblSR.Text = ""
    End Sub

    Private Sub tbnClearR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbnClearR.Click
        lstSOCBills.Items.Clear()
        lblSR.Text = ""
    End Sub

    Private Sub chkUSOC_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUSOC.CheckedChanged
        Me.chkUSOC.Checked = True
        lblSR.Text = ""
        unSOC = True
    End Sub

    Private Sub btnDeleteSOC_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteSOC.Click
        Dim sw As New Stopwatch
        Dim newSR As String = ""
        Dim dsBill As New DataSet

        If MsgBox("Are you sure to want delete all of the special order calendars?", MsgBoxStyle.YesNo, "Delete Specical Order Calendars") = MsgBoxResult.Yes Then
            'V_DataSet("Delete From tblCalendars Where LEFT(CalendarCode, 'SR')", "D")
            V_DataSet("Delete From tblCalendars Where CalendarCode= 'SOC'", "D")
            V_DataSet("Delete From tblSpecialOrderCalendar", "D")

            '---Finding out 'Order Of Business' window does open or close 
            sw.Start()
            For i As Int16 = 0 To My.Application.OpenForms.Count - 1
                If My.Application.OpenForms.Item(i).Text = "Senate Voting System - Order Of Business" Then
                    frmOpen = True
                    Exit For
                Else
                    frmOpen = False
                End If
            Next
            sw.Stop()


            '---set foucse on passed SR
            For i As Integer = 0 To frmChamberDisplay.Calendar.Items.Count - 1
                If frmChamberDisplay.Calendar.Items(i).ToString = "SPECIAL ORDER CALENDAR" Then
                    frmChamberDisplay.Calendar.Items.Remove(frmChamberDisplay.Calendar.Items(i).ToString)
                End If
            Next

            '--- if 'Chamber Display' window is close, open it first         
            If frmOpen = False Then
                Me.MdiParent = frmMain
                frmChamberDisplay.Show()
            Else
                Me.MdiParent = frmMain
                frmChamberDisplay.Close()
                frmChamberDisplay.Show()
            End If

            Me.lstSOCBills.Items.Clear()
            Me.Close()
        End If
    End Sub

    Private Sub btnClearAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAll.Click
        lstSOCBills.Items.Clear()
    End Sub
End Class