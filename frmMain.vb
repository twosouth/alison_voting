Option Strict Off
Option Explicit Off

Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.ComponentModel
Imports System.Net
Imports System.Messaging
Imports System.Runtime.Serialization
Imports System.Text
Imports System.DateTime
Imports System.DBNull
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class frmMain

    Declare Function WriteProfileString Lib "kernel32" _
                 Alias "WriteProfileStringA" _
                 (ByVal lpszSection As String, _
                 ByVal lpszKeyName As String, _
                 ByVal lpszString As String) As Long

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        accessMenu()
    End Sub

    Private Sub CDWindowOpen()
        Dim sw As New Stopwatch

        sw.Start()
        For i As Int16 = 0 To My.Application.OpenForms.Count - 1
            If My.Application.OpenForms.Item(i).Text = "Senate Voting System - Order Of Business" Then
                straightSOC = False
                Exit For
            Else
                straightSOC = True
                isDownLoadBillorSOC = False
            End If
            If My.Application.OpenForms.Item(i).Text = "Senate Vote Window" Then
                Exit For
            End If
        Next
        sw.Stop()
    End Sub

    Private Sub accessMenu()
        If UCase(gDataComputerName) = UCase(SVOTE) Then
            Me.StartTS.Text = "Voting System"
            Me.PhraseList.Visible = False
            Me.BuildVoteIDTS.Visible = True
            Me.SendVotesToAlisTS.Visible = True
            Me.SendVotesToAlisTS.Enabled = True
            Me.SenatorList.Enabled = True
            Me.reportTSroll.Visible = True
            Me.reportTSvoteID.Visible = True
            Me.reportTSphrases.Visible = False
            'If SDisplay_On Then
            '    Me.uTSsenator.Enabled = False
            '    Me.uTSdownloadB.Enabled = False
            'Else
            Me.uTSsenator.Enabled = True
            Me.uTSdownloadB.Enabled = True
            'End If
            Me.uTSdownloadS.Enabled = True
            Me.uTSdownloadC.Enabled = False
            Me.uTScpm.Enabled = False
            Me.uTSdelete.Enabled = False
            Me.uTSboard.Enabled = False
            Me.uTSMotion.Enabled = False
        ElseIf (UCase(gDataComputerName) = UCase(SDIS)) Then
            Me.PhraseList.Visible = True
            Me.SendVotesToAlisTS.Enabled = False
            Me.SendVotesToAlisTS.Visible = False
            Me.StartTS.Text = "Chamber Display"
            Me.reportTSroll.Visible = True
            Me.reportTSvoteID.Visible = True
            Me.BuildVoteIDTS.Visible = False
            Me.reportTSroll.Visible = True
            Me.reportTSvoteID.Visible = True
            Me.reportTSphrases.Visible = True
            Me.uTSdownloadS.Enabled = True
            Me.uTSdownloadC.Enabled = True
            Me.uTSdownloadB.Enabled = True
            Me.uTScpm.Enabled = True
            Me.uTSsenator.Enabled = True
            Me.uTScpm.Enabled = True
            Me.SenatorList.Enabled = True
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub frmStart_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Me.MaximizeBox = True
        Me.IsMdiContainer = True
        Me.BringToFront()
    End Sub

    Private Sub ChangeSize(ByVal frm As Windows.Forms.Form)
        Me.Width = 1024
        Me.Height = 745
        Me.Top = 0
        Me.Left = 0
        frm.MdiParent = Me
        frm.Show()
        frm.BringToFront()
        accessMenu()
    End Sub

    Private Sub uTScpm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTScpm.Click
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmPhrasesList
        ChangeSize(frm)
    End Sub

    Private Sub uTSsenator_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSsenator.Click
        If gNbrSenators > 0 Then
            CDWindowOpen()
            frmVote.Close()
            Me.WindowState = FormWindowState.Maximized
            Dim frm As New frmSenatorList
            ChangeSize(frm)
        Else
            DisplayMessage("Please download Senators from ALIS first.", "Senate Voting System", "S")
        End If
    End Sub

    Private Sub uTSparameter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSparameter.Click
      CDWindowOpen
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmVotingParameterMaintenance
        ChangeSize(frm)
    End Sub

    Private Sub uTSboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSboard.Click
        CDWindowOpen()
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmBoardAndCommissionList
        ChangeSize(frm)
    End Sub

    Private Sub uTSMotion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSMotion.Click
        CDWindowOpen()
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmMotionList
        ChangeSize(frm)
    End Sub

    Private Sub reportTSroll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles reportTSroll.Click
        Dim v_id As New Object
        v_id = Trim(Mid(Me.reportTSvoteID.Text, 9))
        If v_id = "" Or Val(v_id) = 0 Then
            DisplayMessage("Invalid Vote ID", "Invalid Vote ID", "S")
            Me.reportTSvoteID.Text = "Vote ID:"
            Exit Sub
        Else
            RollOfTheSenateByVoteIDReport(v_id)
        End If
    End Sub

    Private Sub ReportsTS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReportsTS.Click
        reportTSvoteID.Text = "Vote ID:"
        If UCase(gComputerName) = UCase(SVOTE) Then
            reportTSvoteID.Focus()
        End If
    End Sub

    Private Sub StartTS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartTS.Click
        '--- if current session changed by somehow reason, system has be re-download Bills from ALIS and parameters
        If gNetwork Then
            '--- get Legislative day and Session infor
            GetLegDay()

            If gSessionName <> gLastSessionName Then
                DisplayMessage("Current session has been changed! System will re-download bills and parameters.", "Re-download Bills and Parameters", "I")
                Main()
            End If
        End If

        V_DataSet("Delete From tblBuiltVoteID", "D")

        Me.WindowState = FormWindowState.Maximized
        If gNbrSenators > 0 Then
            If UCase(gComputerName) = UCase(SDIS) Then
                Dim frm As New frmChamberDisplay
                ChangeSize(frm)
                frm.TC.SelectedIndex = 0
                tPage0 = True
                tPage1 = False
                tPage2 = False
            ElseIf UCase(gComputerName) = UCase(SVOTE) Then
                Dim frm As New frmVote
                ChangeSize(frm)
            End If
        Else
            DisplayMessage("Please download Senators from ALIS first.", "Senate Voting System", "S")
        End If
    End Sub

    Private Sub uTSdownloadS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSdownloadS.Click
        If gNetwork Then
            DownloadSenatorsFromALIS()
        Else
            DisplayMessage("Connect to ALIS database is failed! Unable download Senators from ALIS. Please contact to Administrator.", "Download Senators Form ALIS", "I")
        End If
    End Sub

    Private Sub uTSdownloadC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSdownloadC.Click
        Dim dsALIS As New DataSet
        Dim strSql As String
        Dim dr As DataRow
        Dim ds As New DataSet

        If Not DisplayMessage("You are about to download committees from ALIS.  " & "Do you want to continue?", "Download Committees From ALIS", "Y") Then
            Exit Sub
        End If

        If gNetwork Then
            V_DataSet("Delete From tblCommittees", "D")

            strSql = "SELECT COMMITTEE.NAME AS NAME, COMMITTEE.ABBREVIATION AS ABBREV " & _
                    " FROM ORGANIZATION COMMITTEE, CODE_VALUES CV, ORGANIZATION HOUSE " & _
                    " WHERE COMMITTEE.TYPE_CODE = CV.CODE AND COMMITTEE.OID_PARENT_ORGANIZATION = HOUSE.OID AND (COMMITTEE.COMMITTEE = 'Y') " & _
                    " AND (CV.VALUE = 'Standing Committee') AND (HOUSE.NAME = 'Senate') AND (COMMITTEE.EXPIRATION_DATE IS NULL OR COMMITTEE.EXPIRATION_DATE > (SELECT MIN(D.CALENDAR_DATE) " & _
                    " FROM LEGISLATIVE_DAY D WHERE D.OID_SESSION = " & gSessionID & ")) ORDER BY COMMITTEE.ABBREVIATION"
            dsALIS = ALIS_DataSet(strSql, "R")

            For Each dr In dsALIS.Tables(0).Rows
                V_DataSet("Insert Into tblCommittees Values('" & dr("ABBREV") & "', '" & dr("NAME") & "')", "A")
            Next

            LoadCommitteesIntoArray()

            RetValue = DisplayMessage("Committee download complete", "", "I")
        Else
            DisplayMessage("Connect to ALIS database is failed! Unable download Committees from ALIS. Please contact to Administrator.", "Download Committees from ALIS", "I")
        End If
    End Sub

    Private Sub uTSdelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSdelete.Click
        Dim dsC As New DataSet
        Dim dsCG As New DataSet

        If DisplayMessage("You are about to delete all special order calendars.  Do you want to continue?", "Delete Special Order Calendars", "Y") Then
            V_DataSet("Delete From tblCalendars Where LEFT(CalendarCode, 2) ='SR' OR CalendarCode ='SOC'", "D")
            V_DataSet("Delete From tblSpecialOrderCalendar", "D")
        End If
    End Sub

    Private Sub uTSdownloadB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSdownloadB.Click
        Try
            If gNetwork Then
                Me.Cursor = Cursors.WaitCursor
                isDownLoadBillorSOC = True
                RetValue = DownloadBillsFromALIS()
                Me.Cursor = Cursors.Default
            Else
                DisplayMessage("Connect to ALIS database failed! Unable download Bills from ALIS. Please contact to Administrator.", "Download Bills from ALIS", "I")
            End If
        Catch ex As Exception
            MsgBox("Re-download Bills from ALIS is Failed! please contact to Administrator.", vbCritical, "Download Bills From ALIS")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As Windows.Forms.Form

        '---  close all of the opening child windows
        For Each f In Me.MdiChildren
            Dim pcount As Integer = Me.MdiChildren.GetLength(0)
            f.Close()
            If pcount = Me.MdiChildren.GetLength(0) Then
                Exit For
            Else
                f.Dispose()
            End If
        Next

        '--- set frmAbout form back to centre of screen
        Me.Width = 628
        Me.Height = 451
        Me.Left = 200
        Me.Top = 150
    End Sub

    Private Sub uTSVPMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim frm As New frmVotingParameterMaintenance
        ChangeSize(frm)
    End Sub

    Private Sub AppExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AppExit.Click
        Application.Exit()
    End Sub

    Private Sub SenatorList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SenatorList.Click
        If gNbrSenators > 0 Then
            CDWindowOpen()
            Me.WindowState = FormWindowState.Maximized
            Dim frm As New frmSenatorList()
            ChangeSize(frm)
        Else
            DisplayMessage("Please download Senators from ALIS first.", "Senate Voting System", "S")
        End If
    End Sub

    Private Sub PhraseList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PhraseList.Click
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmPhrasesList
        ChangeSize(frm)
    End Sub

    'Private Sub SpecialOCalendar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SpecialOCalendar.Click
    '    straightSOC = False
    '    isDownLoadBillorSOC = False
    '    CDWindowOpen()
    '    Me.WindowState = FormWindowState.Maximized
    '    Dim frm As New frmPreSOC
    '    ChangeSize(frm)
    'End Sub

    Private Sub SendVotesToAlisTS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SendVotesToAlisTS.Click
        If DisplayMessage("You are about to process all of UNSENT votes to ALIS. Do you want to continue?", "Send Votes To ALIS", "Y") Then
            gReSendALIS = True
            Try
                If gNetwork Then
                    SendVotesToALIS()
                Else
                    DisplayMessage("Connect to ALIS database failed! Unable send votes to ALIS. You can send votes late or contact to Administrator.", "Send Votes To ALIS", "I")
                End If
            Catch ex As Exception
                If DisplayMessage("Failed send votes to ALIS. Do you want to try it again?", "Send Votes To ALIS", "Y") Then
                    If gNetwork Then
                        SendVotesToALIS()
                    Else
                        DisplayMessage("Connect to ALIS database failed! Unable send votes to ALIS. You can send votes late or contact to Administrator.", "Send Votes To ALIS", "I")
                    End If
                Else
                    Exit Try
                End If
            End Try
        End If
    End Sub

    Private Sub BuildVoteIDTS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuildVoteIDTS.Click
        gIndex = 4
        index = gIndex
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmBuildVoteIDnew
        ChangeSize(frm)
    End Sub

    Private Sub reportTSphrases_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles reportTSphrases.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            LoadReport("rptPhrases.rpt")
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            DisplayMessage("Failed to print Phrases report", "Print Phrases Report", "S")
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try
    End Sub

    Private Sub reportTMotions_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles reportTMotions.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            LoadReport("rptMotions.rpt")
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            DisplayMessage("Failed to print Motions report", "Print Motions Report", "S")
            Exit Sub
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub LoadReport(ByVal rptName As String, Optional ByVal SessionOID As Object = 0)
        Dim report1 As New ReportDocument()
        Dim report2 As New ReportDocument()

        Try
            ' --- use the proper report to pinter based upon who is alive
            ' SetDefaultPrinter("HP LaserJet P3010 Series UPD PCL 6 (Copy 1)", "", "")
            ' SetDefaultPrinter(gDefaultPrinter, "", "")
            SetDefaultPrinter(gPrimaryPrinter, "", "")
            report1.Load(gVotingPath & rptName)
            report1.PrintToPrinter(1, True, 0, 0)
            report1.Close()

            '--- print rpt to another printer if one selected
            If gSecondaryPrinter <> "" Then
                Dim p As Printing.PrinterSettings
                p = New Printing.PrinterSettings()

                For x As Integer = 0 To Printing.PrinterSettings.InstalledPrinters.Count - 1
                    If UCase(Printing.PrinterSettings.InstalledPrinters.Item(x)) = UCase(gSecondaryPrinter.Trim) Then
                        '--- now change default printe to secondary printer
                        SetDefaultPrinter(Printing.PrinterSettings.InstalledPrinters.Item(x), "", "")
                        report1.Load(gVotingPath & rptName)
                        report1.PrintToPrinter(1, True, 0, 0)
                        report1.Close()
                    End If
                Next
            End If
        Catch ex As Exception
            RetValue = DisplayMessage("An error has occurred while tying to print the vote report. Please make " & _
                                       "sure this PC is attached to a printer.", "Execute: LoadReport()", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub SetDefaultPrinter(ByVal PrinterName As String, ByVal DrivaerName As String, ByVal PrinterPort As String)
        Dim DeviceLine As String
        Dim r As Long

        DeviceLine = PrinterName & "," & DrivaerName & "," & PrinterPort

        '--- Store the new printer informationin the [WINDOWS] section of
        '--- the WIN.INI file for the DEVICE = item
        r = WriteProfileString("windows", "Device", DeviceLine)
        '--- Cause all applications to reload the INI file:
        '--- l = SendMessage(HWND_BROADCAST, WM_WININICHANGE, 0, "windows")
    End Sub

    Private Sub RollOfTheSenateByVoteIDReport(ByVal intVoteID As Integer)
        '--- this routine is very similar to the one for the vote button on
        '--- the voting display PC
        Dim PrintFieldName() As Object, PrintFieldValue() As Object, k As Integer
        Dim SenatorName() As String, Vote() As String
        Dim i As Integer
        Dim WrkFld As String = ""
        Dim ds, dsRollCallV, dsR, dsPD, dsRC As New DataSet

        If intVoteID = 0 Then
            DisplayMessage("Invalid Vote ID", "", "S")
            Exit Sub
        End If

        ds = V_DataSet("Select * From tblRollCallVotes WHERE SessionID =" & gSessionID & " AND VoteID =" & intVoteID, "R")
        If ds.Tables(0).Rows.Count = 0 Then
            RetValue = DisplayMessage("Vote ID not found for this session", "Invalid Vote ID", "S")
            Exit Sub
        End If

        '---clear PrintVoteHeader and PrintVoteDetail, before record them
        V_DataSet("Delete From tblPrintVoteHeader", "D")
        V_DataSet("Delete From tblPrintVoteDetail", "D")

        ReDim SenatorName(gNbrSenators)
        ReDim Vote(gNbrSenators)
        ReDim PrintFieldName(9)
        ReDim PrintFieldValue(9)

        '--- first, write print vote header rec     
        strSQL = "Select * From tblRollCallVotes Where SessionID =" & gSessionID
        dsR = V_DataSet(strSQL, "R")
        gAction = "Writing"
        For k = 0 To 9
            PrintFieldName(k) = dsR.Tables(0).Columns.Item(k).ColumnName
        Next k

        strSQL = "Select * From tblRollCallVotes WHERE  VoteID =" & intVoteID & " AND SessionID = " & gSessionID
        dsRollCallV = V_DataSet(strSQL, "R")
        For Each dr As DataRow In dsRollCallV.Tables(0).Rows
            PrintFieldValue(0) = intVoteID
            PrintFieldValue(1) = dr("Bill")
            If Not IsDBNull(dr("Motion")) Then
                PrintFieldValue(2) = Replace(dr("Motion"), "'", "''")
            Else
                PrintFieldValue(2) = ""
            End If
            PrintFieldValue(3) = dr("LegislativeDay")
            PrintFieldValue(4) = dr("VoteDate")
            PrintFieldValue(5) = dr("VoteTime")
            PrintFieldValue(6) = dr("TotalYea")
            PrintFieldValue(7) = dr("TotalNay")
            PrintFieldValue(8) = dr("TotalAbstain")
            PrintFieldValue(9) = gSessionName
            strSQL = "Insert Into tblPrintVoteHeader Values (" & intVoteID & ", '" & dr("Bill") & "', '" & dr("Motion") & "', '" & dr("LegislativeDay") & "', '" & dr("VoteDate") & "', '" & dr("VoteTime") & "', " & dr("TotalYea") & ", " & dr("TotalNay") & ", " & dr("TotalAbstain") & ", '" & gSessionName & "')"
            V_DataSet(strSQL, "A")
        Next

        '--- Now set up for print vote detail recs
        ReDim PrintFieldName(8)
        ReDim PrintFieldValue(8)
     
        dsPD = V_DataSet("Select * From tblPrintVoteDetail", "R")
        gAction = "Writing"
        For k = 0 To 8
            PrintFieldName(k) = dsPD.Tables(0).Columns.Item(k).ColumnName
        Next k

        ' ---pick out senators and their votes from this string; names
        ' ---in the vote rec are in district order, so must xref them to
        ' ---name order so they will print in name order
        dsRC = V_DataSet("Select * From tblRollCallVotes Where  SessionID =" & gSessionID & " AND VoteID =" & intVoteID, "R")
        For Each dr As DataRow In dsRC.Tables(0).Rows
            WrkFld = dr("SenatorVotes")         '---Example: SenatorVotes = Allen*N*1835*69840;Beasley*Y*1857*69847;Beason
        Next
        k = 0
        Do
            k = k + 1
            For i = 0 To gNbrSenators - 1
                If gSenatorNameOrder(i) = Mid(WrkFld, 1, InStr(WrkFld, "*") - 1) Then
                    Exit For
                End If
            Next i
            SenatorName(i) = Mid(WrkFld, 1, InStr(WrkFld, "*") - 1)
            WrkFld = Mid(WrkFld, InStr(WrkFld, "*") + 1)
            Vote(i) = WrkFld.Substring(0, 1)
            If InStr(WrkFld, ";") = Len(WrkFld) Then
                Exit Do
            End If
            WrkFld = Mid(WrkFld, InStr(WrkFld, ";") + 1)
        Loop

        '--- add senators/votes to print detail
        For i = 0 To gNbrSenators - 1
            PrintFieldValue(0) = intVoteID
            PrintFieldValue(1) = SenatorName(i)
            PrintFieldValue(2) = ""     '--- init all fields
            PrintFieldValue(3) = ""
            PrintFieldValue(4) = ""
            PrintFieldValue(5) = ""
            PrintFieldValue(6) = ""
            PrintFieldValue(7) = ""
            PrintFieldValue(8) = ""
            Select Case Vote(i)
                Case "Y" : PrintFieldValue(3) = "Y"
                Case "N" : PrintFieldValue(2) = "N"
                Case "A" : PrintFieldValue(4) = "A"
            End Select

            If i < gSenatorSplit - 1 Then
                PrintFieldValue(5) = gSenatorNameOrder(i + gSenatorSplit)
                Select Case Vote(i + gSenatorSplit)
                    Case "Y" : PrintFieldValue(7) = "Y"
                    Case "N" : PrintFieldValue(6) = "N"
                    Case "A" : PrintFieldValue(8) = "A"
                End Select
                strSQL = "Insert Into tblPrintVoteDetail Values (" & PrintFieldValue(0) & ", '" & PrintFieldValue(1) & "', '" & PrintFieldValue(2) & "', '" & PrintFieldValue(3) & "', '" & _
                       PrintFieldValue(4) & "', '" & PrintFieldValue(5) & "', '" & PrintFieldValue(6) & "', '" & PrintFieldValue(7) & "', '" & PrintFieldValue(8) & "')"
                V_DataSet(strSQL, "A")
            ElseIf i = gSenatorSplit - 1 Then
                strSQL = "Insert Into tblPrintVoteDetail Values (" & PrintFieldValue(0) & ", '" & PrintFieldValue(1) & "', '" & PrintFieldValue(2) & "', '" & PrintFieldValue(3) & "', '" & _
                        PrintFieldValue(4) & "', '" & PrintFieldValue(5) & "', '" & PrintFieldValue(6) & "', '" & PrintFieldValue(7) & "', '" & PrintFieldValue(8) & "')"
                V_DataSet(strSQL, "A")
            End If
        Next i

        '--- load Crystal Report and Print out
        Try
            If gPrintVoteRpt Then
                If gTestMode = True Then
                    LoadReport("rptVoteTEST.rpt")
                Else
                    LoadReport("rptVote.rpt")
                End If
            End If
        Catch ex As Exception
            DisplayMessage("Print Votes Report Failed! Do you want to vote continuely without print report?", "Failed Print Report", "Y")
            Exit Sub
        End Try
    End Sub

#Region "Un-using Codes"
    'Private Sub LoadVoteIDReport(ByVal rptName As String, voteID As Integer, sessionOID As Integer)
    '    '--- You can change more print options via PrintOptions property of ReportDocument
    '    Dim report1 As New ReportDocument()

    '    Try
    '        '--- use the proper report to pinter based upon who is alive
    '        report1.Load(gVotingPath & rptName)
    '        report1.PrintToPrinter(1, True, 0, 0)
    '        report1.Close()
    '    Catch ex As Exception
    '        RetValue = DisplayMessage("An error has occurred while tying to print the vote report. Please make " & _
    '                                   "sure this PC is attached to a printer.", "Print Problem", "S")
    '        Exit Sub
    '    End Try
    'End Sub

    'Private Sub LoadReport(ByVal rptName As String, Optional ByVal intVoteID As Object = 0)
    '    '--- You can change more print options via PrintOptions property of ReportDocument
    '    Dim report1 As New ReportDocument()

    '    Try
    '        ''--- use the proper report to pinter based upon who is alive
    '        '--- print rpt to another printer if one selected

    '        Dim p As Printing.PrinterSettings
    '        p = New Printing.PrinterSettings()
    '        gDefaultPrinter = p.PrinterName

    '        For x As Integer = 0 To Printing.PrinterSettings.InstalledPrinters.Count - 1
    '            If UCase(Printing.PrinterSettings.InstalledPrinters.Item(x)) = UCase(gSecondaryPrinter.Trim) Then
    '                '--- now change default printe to secondary printer
    '                SetDefaultPrinter(Printing.PrinterSettings.InstalledPrinters.Item(x), "", "")
    '            End If
    '        Next

    '        If UCase(rptName) = "RPTPASSVOTEID.RPT" Then
    '            '    Dim reportDocument As New ReportDocument()
    '            '    Dim paramFields As New ParameterFields()
    '            '    Dim paramField1 As New ParameterField()
    '            '    Dim paramDiscreteValue1 As New ParameterDiscreteValue()
    '            '    Dim paramField2 As New ParameterField()
    '            '    Dim paramDiscreteValue2 As New ParameterDiscreteValue()

    '            '    paramField1.Name = "Vote ID"
    '            '    paramDiscreteValue1.Value = 1278
    '            '    paramField1.CurrentValues.Add(paramDiscreteValue1)
    '            '    paramFields.Add(paramField1)


    '            '    paramField2.Name = "Session Name"
    '            '    paramDiscreteValue2.Value = gSessionName
    '            '    paramField2.CurrentValues.Add(paramDiscreteValue2)
    '            '    paramFields.Add(paramField2)

    '            '    CRView.ParameterFieldInfo = paramFields
    '            '    Dim reportPath As String = gVotingPath & rptName
    '            '    ' Dim reportPath As String = Server.MapPath("~/CrystalReport.rpt")

    '            '    reportDocument.Load(reportPath)


    '            '    CRView.ReportSource = reportDocument
    '            v_id = intVoteID
    '            frmReport.Show()
    '        Else
    '            report1.Load(gVotingPath & rptName)
    '            report1.PrintToPrinter(1, True, 0, 0)
    '            report1.Close()
    '        End If
    '    Catch ex As Exception
    '        RetValue = DisplayMessage("An error has occurred while tying to print the vote report. Please make " & _
    '                                   "sure this PC is attached to a printer.", "Print Problem", "S")
    '        Exit Sub
    '    End Try
    'End Sub

    '' Reg Key Security Options...
    'Const READ_CONTROL As Integer = &H20000
    'Const KEY_QUERY_VALUE As Short = &H1S
    'Const KEY_SET_VALUE As Short = &H2S
    'Const KEY_CREATE_SUB_KEY As Short = &H4S
    'Const KEY_ENUMERATE_SUB_KEYS As Short = &H8S
    'Const KEY_NOTIFY As Short = &H10S
    'Const KEY_CREATE_LINK As Short = &H20S
    'Const KEY_ALL_ACCESS As Double = KEY_QUERY_VALUE + KEY_SET_VALUE + KEY_CREATE_SUB_KEY + KEY_ENUMERATE_SUB_KEYS + KEY_NOTIFY + KEY_CREATE_LINK + READ_CONTROL

    '' Reg Key ROOT Types...
    'Const HKEY_LOCAL_MACHINE As Integer = &H80000002
    'Const ERROR_SUCCESS As Short = 0
    'Const REG_SZ As Short = 1 ' Unicode nul terminated string
    'Const REG_DWORD As Short = 4 ' 32-bit number

    'Const gREGKEYSYSINFOLOC As String = "SOFTWARE\Microsoft\Shared Tools Location"
    'Const gREGVALSYSINFOLOC As String = "MSINFO"
    'Const gREGKEYSYSINFO As String = "SOFTWARE\Microsoft\Shared Tools\MSINFO"
    'Const gREGVALSYSINFO As String = "PATH"


    'Private Declare Function RegOpenKeyEx Lib "advapi32" Alias "RegOpenKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
    'Private Declare Function RegQueryValueEx Lib "advapi32" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
    'Private Declare Function RegCloseKey Lib "advapi32" (ByVal hKey As Integer) As Integer


    '    Public Function GetKeyValue(ByRef KeyRoot As Integer, ByRef KeyName As String, ByRef SubKeyRef As String, ByRef KeyVal As String) As Boolean
    '        Dim i As Integer                                        ' Loop Counter
    '        Dim rc As Integer                                       ' Return Code
    '        Dim hKey As Integer                                     ' Handle To An Open Registry Key
    '        Dim KeyValType As Integer                               ' Data Type Of A Registry Key
    '        Dim tmpVal As String                                    ' Tempory Storage For A Registry Key Value
    '        Dim KeyValSize As Integer                               ' Size Of Registry Key Variable

    '        Try
    '            '------------------------------------------------------------
    '            ' Open RegKey Under KeyRoot {HKEY_LOCAL_MACHINE...}
    '            '------------------------------------------------------------
    '            rc = RegOpenKeyEx(KeyRoot, KeyName, 0, KEY_ALL_ACCESS, hKey) ' Open Registry Key

    '            If (rc <> ERROR_SUCCESS) Then GoTo GetKeyError ' Handle Error...

    '            tmpVal = New String(Chr(0), 1024)                   ' Allocate Variable Space
    '            KeyValSize = 1024                                   ' Mark Variable Size

    '            '------------------------------------------------------------
    '            ' Retrieve Registry Key Value...
    '            '------------------------------------------------------------
    '            rc = RegQueryValueEx(hKey, SubKeyRef, 0, KeyValType, tmpVal, KeyValSize) ' Get/Create Key Value

    '            If (rc <> ERROR_SUCCESS) Then GoTo GetKeyError ' Handle Errors

    '            If (Asc(Mid(tmpVal, KeyValSize, 1)) = 0) Then       ' Win95 Adds Null Terminated String...
    '                tmpVal = Microsoft.VisualBasic.Left(tmpVal, KeyValSize - 1) ' Null Found, Extract From String
    '            Else                                                ' WinNT Does NOT Null Terminate String...
    '                tmpVal = Microsoft.VisualBasic.Left(tmpVal, KeyValSize) ' Null Not Found, Extract String Only
    '            End If
    '            '------------------------------------------------------------
    '            ' Determine Key Value Type For Conversion...
    '            '------------------------------------------------------------
    '            Select Case KeyValType                              ' Search Data Types...
    '                Case REG_SZ                                     ' String Registry Key Data Type
    '                    KeyVal = tmpVal                             ' Copy String Value
    '                Case REG_DWORD                                  ' Double Word Registry Key Data Type
    '                    For i = Len(tmpVal) To 1 Step -1            ' Convert Each Bit
    '                        KeyVal = KeyVal & Hex(Asc(Mid(tmpVal, i, 1))) ' Build Value Char. By Char.
    '                    Next
    '                    KeyVal = Format("&h" & KeyVal)              ' Convert Double Word To String
    '            End Select

    '            GetKeyValue = True                                  ' Return Success
    '            rc = RegCloseKey(hKey)                              ' Close Registry Key
    '            Exit Function                                       ' Exit

    'GetKeyError:
    '        Catch                                                   ' Cleanup After An Error Has Occured...
    '            KeyVal = ""                                         ' Set Return Val To Empty String
    '            GetKeyValue = False                                 ' Return Failure
    '            rc = RegCloseKey(hKey)                              ' Close Registry Key
    '        End Try

    '    End Function

    'Public Sub StartSysInfo()
    '        On Error GoTo SysInfoErr

    '        Dim rc As Integer
    '        Dim SysInfoPath As String = ""

    '        ' Try To Get System Info Program Path\Name From Registry...
    '        If GetKeyValue(HKEY_LOCAL_MACHINE, gREGKEYSYSINFO, gREGVALSYSINFO, SysInfoPath) Then
    '            ' Try To Get System Info Program Path Only From Registry...
    '        ElseIf GetKeyValue(HKEY_LOCAL_MACHINE, gREGKEYSYSINFOLOC, gREGVALSYSINFOLOC, SysInfoPath) Then
    '            ' Validate Existance Of Known 32 Bit File Version
    '            If Directory.Exists(SysInfoPath & "\MSINFO32.EXE") = True Then
    '                SysInfoPath = SysInfoPath & "\MSINFO32.EXE"
    '                ' Error - File Can Not Be Found...
    '            Else
    '                GoTo SysInfoErr
    '            End If
    '            ' Error - Registry Entry Can Not Be Found...
    '        Else
    '            GoTo SysInfoErr
    '        End If

    '        Call Shell(SysInfoPath, AppWinStyle.NormalFocus)

    '        Exit Sub
    'SysInfoErr:
    '        MsgBox("System Information Is Unavailable At This Time", MsgBoxStyle.OkOnly)
    '    End Sub

    'Private Sub helpTSabout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles helpTSabout.Click
    '    Me.WindowState = FormWindowState.Maximized
    '    Dim frm As New frmAboutNew
    '    ChangeSize(frm)
    '    frmAboutNew.Close()
    'End Sub

    'Private Sub helpTSsystem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles helpTSsystem.Click
    '    Me.WindowState = FormWindowState.Maximized
    '    StartSysInfo()
    'End Sub

    'Private Sub uTSupdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSupdate.Click
    '    Me.WindowState = FormWindowState.Maximized
    '    Dim frm As New frmDisplayBackGround
    '    ChangeSize(frm)
    '    frmAboutNew.Close()
    'End Sub
#End Region

End Class