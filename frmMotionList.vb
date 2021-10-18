Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Text
Imports System.DateTime
Imports System.DBNull
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class frmMotionList
    Private origValue1 As String
    Private origValue2 As Integer

    Declare Function WriteProfileString Lib "kernel32" _
              Alias "WriteProfileStringA" _
              (ByVal lpszSection As String, _
              ByVal lpszKeyName As String, _
              ByVal lpszString As String) As Long

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub frmMotionList_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        e.Cancel = True
        Me.Hide()
    End Sub

    Private Sub frmMotionList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.MdiParent = frmMain
        loadPage()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MsgBox("Are you sure want to delete selected record?", vbYesNo, "Delete Warning") = MsgBoxResult.Yes Then
            Try
                strSQL = "Delete from tblBills Where ucase(BillNbr) ='" & UCase(userDataGridView.CurrentRow.Cells(0).Value) & "' AND CalendarCode='M'"
                V_DataSet(strSQL, "D")
                loadPage()
                Exit Sub
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Delete The Motion")
            End Try
        End If
    End Sub

    Private Sub userDataGridView_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles userDataGridView.CellBeginEdit
        If (Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Or Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value)) Then
            If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
                origValue1 = Trim(userDataGridView.CurrentRow.Cells(1).Value)
            Else
                origValue1 = ""
            End If
            If Not IsDBNull(userDataGridView.CurrentRow.Cells(2).Value) Then
                origValue2 = userDataGridView.CurrentRow.Cells(2).Value
            Else
                origValue2 = 0
            End If
        End If
    End Sub

    Private Sub userDataGridView_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles userDataGridView.CellClick
        If (Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Or Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value)) Then
            If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
                origValue1 = Trim(userDataGridView.CurrentRow.Cells(1).Value)
            Else
                origValue1 = ""
            End If
            If Not IsDBNull(userDataGridView.CurrentRow.Cells(2).Value) Then
                origValue2 = userDataGridView.CurrentRow.Cells(2).Value
            Else
                origValue2 = 0
            End If
        End If
    End Sub

    Private Sub userDataGridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles userDataGridView.CellEndEdit
        Dim ds As New DataSet
        Dim i, k As Integer
        Try
            If IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Then
                MsgBox("Code number can not be empty!", MsgBoxStyle.Critical, "Add New or Update Motion")
                Exit Sub
            End If

            If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
                Dim code As String = userDataGridView.CurrentRow.Cells(0).Value
                Dim motion As String = userDataGridView.CurrentRow.Cells(1).Value

                '--- check motion is exist or not first
                ds = V_DataSet("Select * From tblBills Where CalendarCode ='M' AND ltrim(Bill) ='" & LTrim(origValue1) & "' AND oid =" & origValue2, "R")

                If ds.Tables(0).Rows.Count > 0 Then
                    '---doing update
                    V_DataSet("Update tblBills  Set  Bill ='" & motion & "' WHERE CalendarCode ='M' and oid = " & origValue2, "U")
                Else

                    Dim dsC As New DataSet
                    dsC = V_DataSet("Select * From tblBills Where CalendarCode ='M' AND ltrim(BillNbr) ='" & code & "'", "R")

                    '--- check duplicte
                    If dsC.Tables(0).Rows.Count > 0 Then
                        For i = 0 To userDataGridView.Rows.Count - 1
                            If userDataGridView.Rows(i).Cells(0).Value = code Then
                                k = userDataGridView.Rows(i).Index
                                MsgBox("Motion " & code & " already exist.", vbInformation, "Motion Maintenance")
                                loadPage()
                                userDataGridView.Rows(k).Selected = True

                                Exit For
                            End If
                        Next
                    Else
                        '--- add new 
                        V_DataSet("Insert Into tblBills (CalendarCode, BillNbr, Bill) values ('M', '" & code & "', '" & motion & "')", "A")
                        loadPage()
                    End If
                End If

            Else
                ' MsgBox("Please enter motion.", MsgBoxStyle.Critical, "Add New or Update Motion")
                Exit Sub
            End If
        Catch ex As Exception
            'MsgBox(ex.Message, MsgBoxStyle.Critical, "Add New or Update Motion")
        End Try
    End Sub

    Private Sub userDataGridView_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles userDataGridView.CellMouseClick
        If (Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Or Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value)) Then
            If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
                origValue1 = Trim(userDataGridView.CurrentRow.Cells(1).Value)
            Else
                origValue1 = ""
            End If
            If Not IsDBNull(userDataGridView.CurrentRow.Cells(2).Value) Then
                origValue2 = userDataGridView.CurrentRow.Cells(2).Value
            Else
                origValue2 = 0
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        loadPage()
        origValue1 = ""
        origValue2 = 0
    End Sub

    Private Sub loadPage()
        Dim strSql As String = ""
        Dim intSkip As Integer = 0
        Dim ds As New DataSet

        strSql = "SELECT  BillNbr, Bill, OID FROM tblBills  WHERE CalendarCode = 'M' ORDER BY BillNbr"
        ds = V_DataSet(strSql, "R")

        '-- Populate Data Grid
        Me.userDataGridView.DataSource = Nothing
        Me.userDataGridView.DataSource = ds.Tables(0).DefaultView
        Me.userDataGridView.Columns(0).Width = 120
        Me.userDataGridView.Columns(1).Width = 870
        Me.userDataGridView.Columns(2).Width = 1
        Me.userDataGridView.Columns(0).HeaderText = "Code"
        Me.userDataGridView.Columns(1).HeaderText = "Motion"
        Me.userDataGridView.Columns(2).HeaderText = "oid"
        ds.Dispose()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnDeleteAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteAll.Click
        If MsgBox("Are you sure want to delete all of the record(s)?", vbYesNo, "Delete Warning") = MsgBoxResult.Yes Then
            Try
                strSQL = "Delete from tblBills Where CalendarCode = 'M'"
                V_DataSet(strSQL, "D")
                loadPage()
            Catch ex As Exception
                DisplayMessage(ex.Message, "Delete The Motions", "S")
                Exit Sub
            End Try
        End If
    End Sub

    Private Sub ChangeSize(ByVal frm As Windows.Forms.Form)
        Me.Width = 1024
        Me.Height = 745
        Me.Top = 0
        Me.Left = 0
        frm.MdiParent = frmMain
        frm.Show()
        frm.BringToFront()
    End Sub

    Private Sub CDWindowOpen()
        Dim sw As New Stopwatch

        sw.Start()
        For i As Int16 = 0 To My.Application.OpenForms.Count - 1
            If My.Application.OpenForms.Item(i).Text = "Senate Voting System - Order Of Business" Then
                My.Application.OpenForms.Item(i).Close()
                Exit For
            ElseIf My.Application.OpenForms.Item(i).Text = "Senate Vote Window" Then
                My.Application.OpenForms.Item(i).Close()
                Exit For
            End If
        Next
        sw.Stop()
    End Sub

    'Private Sub closeConnection()
    '    Try
    '        If (Me.mcnSample.State = ConnectionState.Open) Then
    '            Me.mcnSample.Close()
    '        End If
    '        Me.mcnSample.Dispose()
    '    Catch ex As Exception
    '        MessageBox.Show(ex.ToString())
    '    End Try
    'End Sub

    'Private Sub openConnection()
    '    Try
    '        Me.mcnSample = New OleDbConnection(strLocal)
    '        Me.mcnSample.Open()
    '    Catch ex As Exception
    '        DisplayMessage(ex.Message, "Set Local Database Connection", "S")
    '        Exit Sub
    '    End Try
    'End Sub
  
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            LoadReport("rptMotions.rpt")
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            DisplayMessage("Failed to print Montions report", "Print Motions Report", "S")
            Exit Sub
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub LoadReport(ByVal rptName As String, Optional ByVal intVoteID As Object = 0)
        '--- You can change more print options via PrintOptions property of ReportDocument
        Dim report1 As New ReportDocument()

        Try
            '--- use the proper report to pinter based upon who is alive
            '--- print rpt to another printer if one selected

            Dim p As Printing.PrinterSettings
            p = New Printing.PrinterSettings()
            gDefaultPrinter = p.PrinterName

            For x As Integer = 0 To Printing.PrinterSettings.InstalledPrinters.Count - 1
                If UCase(Printing.PrinterSettings.InstalledPrinters.Item(x)) = UCase(gSecondaryPrinter.Trim) Then
                    '--- now change default printe to secondary printer
                    SetDefaultPrinter(Printing.PrinterSettings.InstalledPrinters.Item(x), "", "")
                End If
            Next

            report1.Load(gVotingPath & rptName)
            report1.PrintToPrinter(1, True, 0, 0)
            report1.Close()
        Catch ex As Exception
            DisplayMessage("An error has occurred while tying to print the vote report. Please make " & _
                                       "sure this PC is attached to a printer.", "Print Problem", "S")
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
End Class