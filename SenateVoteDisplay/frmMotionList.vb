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
    Private sqlQuery As String
    Private Connection As OleDb.OleDbConnection
    Private Command As OleDbCommand
    Private Adapter As OleDbDataAdapter
    Private Builder As OleDbCommandBuilder
    Private ds, tempDataSet As DataSet
    Private userTable As DataTable
    Private dc As New DataColumn
    Private currentIndex, i As Integer
    Private isLastPage, isFirstPage As Boolean
    Private totalRecords, startRecord, endRecord As Integer
    Private currentPageStartRecord, currentPageEndRecord As Integer
    Private Retvalre As New Object
    Private origValue1, origValue2 As String
    Private mintTotalRecords As Integer = 0
    Private mintPageSize As Integer = 0
    Private mintPageCount As Integer = 0
    Private mintCurrentPage As Integer = 1
    Protected mcnSample As OleDb.OleDbConnection

    Declare Function WriteProfileString Lib "kernel32" _
              Alias "WriteProfileStringA" _
              (ByVal lpszSection As String, _
              ByVal lpszKeyName As String, _
              ByVal lpszString As String) As Long

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
    End Sub

    Public Sub CDWindowOpen()
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

    Private Sub frmMotionList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CDWindowOpen()
        Me.openConnection()
        loadPage()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MsgBox("Are you sure want to delete selected record?", vbYesNo, "Delete Warning") = MsgBoxResult.Yes Then
            Try
                sqlQuery = "Delete from tblBills Where ucase(BillNbr) ='" & UCase(userDataGridView.CurrentRow.Cells(0).Value) & "' AND CalendarCode='M'"
                V_DataSet(sqlQuery, "D")

                loadPage()
                Me.Close()
                Exit Sub
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Delete The Motion")
            End Try
        End If
    End Sub

    Private Sub userDataGridView_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles userDataGridView.CellBeginEdit
        If (Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Or Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value)) Then
            If Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Then
                origValue1 = Trim(userDataGridView.CurrentRow.Cells(0).Value)
            Else
                origValue1 = ""
            End If
            If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
                origValue2 = Trim(userDataGridView.CurrentRow.Cells(1).Value)
            Else
                origValue2 = ""
            End If
        End If
    End Sub

    Private Sub userDataGridView_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles userDataGridView.CellClick
        If (Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Or Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value)) Then
            If Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Then
                origValue1 = Trim(userDataGridView.CurrentRow.Cells(0).Value)
            Else
                origValue1 = ""
            End If
            If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
                origValue2 = Trim(userDataGridView.CurrentRow.Cells(1).Value)
            Else
                origValue2 = ""
            End If
        End If
    End Sub

    Private Sub userDataGridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles userDataGridView.CellEndEdit
        Dim ds As New DataSet

        Try
            If IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Then
                MsgBox("Code number can not be empty!", MsgBoxStyle.Critical, "Add New or Update Motion")
                Exit Sub
            End If

            If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
                Dim code As String = userDataGridView.CurrentRow.Cells(0).Value
                Dim motion As String = userDataGridView.CurrentRow.Cells(1).Value

                '--- check motion is exist or not first
                ds = V_DataSet("Select * From tblBills Where CalendarCode ='M' AND BillNbr ='" & origValue1 & "' AND Bill ='" & origValue2 & "'", "R")
                If ds.Tables(0).Rows.Count > 0 Then
                    '---doing update
                    V_DataSet("Update tblBills  Set BillNbr='" & code & "' And Bill ='" & motion & "' WHERE CalendarCode ='M' and BillNbr = '" & origValue1 & "'", "U")
                Else
                    '--- add new 
                    V_DataSet("Insert Into tblBills (CalendarCode, BillNbr, Bill) values ('M', '" & code & "', '" & motion & "')", "A")
                End If
                userDataGridView.CurrentCell = userDataGridView.Rows(0).Cells(0)
                Get_Parameters()
                Check_ALIS_Database_Accessible()
            Else
                MsgBox("Please enter motion.", MsgBoxStyle.Critical, "Add New or Update Motion")
                Exit Sub
            End If
        Catch ex As Exception
            'MsgBox(ex.Message, MsgBoxStyle.Critical, "Add New or Update Motion")
        End Try
    End Sub

    Private Sub userDataGridView_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles userDataGridView.CellMouseClick
        If (Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Or Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value)) Then
            If Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Then
                origValue1 = Trim(userDataGridView.CurrentRow.Cells(0).Value)
            Else
                origValue1 = ""
            End If
            If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
                origValue2 = Trim(userDataGridView.CurrentRow.Cells(1).Value)
            Else
                origValue2 = ""
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        origValue1 = ""
        origValue2 = ""
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
        frmMain.Show()
    End Sub

    Private Sub loadPage()
        Dim strSql As String = ""
        Dim intSkip As Integer = 0

        strSql = "SELECT  BillNbr, Bill FROM tblBills  WHERE CalendarCode = 'M' ORDER BY BillNbr"

        Dim cmd As OleDb.OleDbCommand = Me.mcnSample.CreateCommand()
        cmd.CommandText = strSql
        Dim da As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(cmd)
        Dim ds As DataSet = New DataSet()
        da.Fill(ds, "tblMotion")

        '-- Populate Data Grid
        Me.userDataGridView.DataSource = ds.Tables("tblMotion").DefaultView
        Me.userDataGridView.Columns(0).Width = 120
        Me.userDataGridView.Columns(1).Width = 860
        Me.userDataGridView.Columns(0).HeaderText = "Code"
        Me.userDataGridView.Columns(1).HeaderText = "Motion"

        cmd.Dispose()
        da.Dispose()
        ds.Dispose()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.closeConnection()
        Me.Close()
    End Sub

    Private Sub closeConnection()
        Try
            If (Me.mcnSample.State = ConnectionState.Open) Then
                Me.mcnSample.Close()
            End If
            Me.mcnSample.Dispose()

        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    Private Sub openConnection()
        Try
            Me.mcnSample = New OleDbConnection(strLocal)
            Me.mcnSample.Open()
        Catch ex As Exception
            DisplayMessage(ex.Message, "Set Local Database Connection", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub btnDeleteAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteAll.Click
        If MsgBox("Are you sure want to delete all of the record(s)?", vbYesNo, "Delete Warning") = MsgBoxResult.Yes Then
            Try
                sqlQuery = "Delete from tblBills Where CalendarCode = 'M'"
                V_DataSet(sqlQuery, "D")
                loadPage()
                Me.Close()
            Catch ex As Exception
                DisplayMessage(ex.Message, "Delete The Motions", "S")
                Exit Sub
            End Try
        End If
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor
            LoadReport("rptMotions.rpt")
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            DisplayMessage("Failed to print Montions report", "Print Motions Report", "S")
            Me.Cursor = Cursors.Default
            Exit Sub
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

#Region "Old-codes"
    'Public Sub New()

    '    ' This call is required by the designer.
    '    InitializeComponent()

    '    ' Add any initialization after the InitializeComponent() call.

    'End Sub

    'Public Sub CDWindowOpen()
    '    Dim sw As New Stopwatch

    '    sw.Start()
    '    For i As Int16 = 0 To My.Application.OpenForms.Count - 1
    '        If My.Application.OpenForms.Item(i).Text = "Senate Voting System - Order Of Business" Then
    '            My.Application.OpenForms.Item(i).Close()
    '            Exit For
    '        ElseIf My.Application.OpenForms.Item(i).Text = "Senate Vote Window" Then
    '            My.Application.OpenForms.Item(i).Close()
    '            Exit For
    '        End If
    '    Next
    '    sw.Stop()
    'End Sub

    'Private Sub frmMotionList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    CDWindowOpen()
    '    mintCurrentPage = 1
    '    Me.openConnection()
    '    btnFillGrid_Click(btnFillGrid, Nothing)
    'End Sub

    'Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
    '    If Not IsDBNull(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value) And Not IsDBNull(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value) Then
    '        If MsgBox("Are you sure want to add?", vbYesNo, "Add New Motion") = MsgBoxResult.Yes Then
    '            Try
    '                ds = V_DataSet("Select * From tblBills Where ucase(BillNbr) ='" & UCase(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value) & "' AND CalendarCode='M' ORDER BY BILLNBR", "R")
    '                If ds.Tables(0).Rows.Count > 0 Then
    '                    Retvalre = DisplayMessage("This code already exists. Please try again.", "DuplicateNameException Code", "S")
    '                    Exit Sub
    '                End If
    '                sqlQuery = "Insert Into tblBills Values ('M', '" & Replace(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value, "'", "''") & "', '" & userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value & "', '', '', '','','','')"
    '                V_DataSet(sqlQuery, "A")
    '                userDataGridView.ReadOnly = False
    '                '  btnCancel_Click(btnCancel, Nothing)
    '                loadPage()
    '                Me.Close()
    '            Catch ex As Exception
    '                MsgBox(ex.Message, MsgBoxStyle.Critical, "Add New Motion")
    '            End Try
    '        Else
    '            btnCancel_Click(btnCancel, Nothing)
    '        End If
    '    Else
    '        MsgBox("Code and Motion are required fielders.", MsgBoxStyle.Critical, "Add The New Motion")
    '    End If
    'End Sub

    'Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
    '    If Not IsDBNull(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value) And Not IsDBNull(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value) Then
    '        If MsgBox("Are you sure want to change selected record(s)?", vbYesNo, "Update The Motion") = MsgBoxResult.Yes Then
    '            Try

    '                ds = V_DataSet("Select * From tblBills Where ucase(Bill) ='" & UCase(origValue1) & "' AND CalendarCode='M'", "R")
    '                If ds.Tables(0).Rows.Count > 0 Then
    '                    MsgBox("This code already exists. Please try again.", MsgBoxStyle.Critical, "Duplicate Code")
    '                    btnCancel_Click(btnCancel, Nothing)
    '                    Exit Sub
    '                End If
    '                V_DataSet("Update tblBills Set CalendarCode='M', BillNbr='" & origValue1 & "', Bill='" & userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value & "', Sponsor='', Subject= '', WorkData='', CalendarPage='', SenatorSubject='', BillCalendarPage='' Where ucase(BillNbr) ='" & UCase(origValue1) & "' AND CalendarCode='M'", "U")
    '                ' btnCancel_Click(btnCancel, Nothing)
    '                loadPage()
    '                '  Me.MdiParent = frmMain
    '                Me.Close()
    '                '  Me.Show()
    '            Catch ex As Exception
    '                MsgBox(ex.Message, MsgBoxStyle.Critical, "Update The Motion")
    '            End Try
    '        End If
    '    Else
    '        If IsDBNull(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value) Then
    '            MsgBox("Code number can not be empty!", MsgBoxStyle.Critical, "Add New Phrase")
    '            Exit Sub
    '        End If
    '        If IsDBNull(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value) Then
    '            MsgBox("Motion can not be empty!", MsgBoxStyle.Critical, "Add New Phrase")
    '            Exit Sub
    '        End If
    '    End If
    'End Sub

    'Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
    '    If MsgBox("Are you sure want to delete selected record(s)?", vbYesNo, "Delete Warning") = MsgBoxResult.Yes Then
    '        Try
    '            sqlQuery = "Delete from tblBills Where ucase(BillNbr) ='" & UCase(userDataGridView.CurrentRow.Cells(0).Value) & "' AND CalendarCode='M'"
    '            V_DataSet(sqlQuery, "D")
    '            '    Me.btnFirst_Click(sender, e)
    '            ' btnCancel_Click(btnCancel, Nothing)
    '            loadPage()
    '            Me.Close()
    '            Exit Sub
    '        Catch ex As Exception
    '            MsgBox(ex.Message, MsgBoxStyle.Critical, "Delete The Motion")
    '        End Try
    '    End If
    'End Sub

    'Private Sub userDataGridView_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles userDataGridView.CellBeginEdit
    '    If (Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Or Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value)) Then
    '        If Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Then
    '            origValue1 = Trim(userDataGridView.CurrentRow.Cells(0).Value)
    '        Else
    '            origValue1 = ""
    '        End If
    '        If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
    '            origValue2 = Trim(userDataGridView.CurrentRow.Cells(1).Value)
    '        Else
    '            origValue2 = ""
    '        End If
    '    End If
    'End Sub

    'Private Sub userDataGridView_CellMouseClick(sender As Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles userDataGridView.CellMouseClick
    '    If (Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Or Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value)) Then
    '        If Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Then
    '            origValue1 = Trim(userDataGridView.CurrentRow.Cells(0).Value)
    '        Else
    '            origValue1 = ""
    '        End If
    '        If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
    '            origValue2 = Trim(userDataGridView.CurrentRow.Cells(1).Value)
    '        Else
    '            origValue2 = ""
    '        End If
    '    End If
    'End Sub

    'Private Sub btnFillGrid_Click(sender As System.Object, e As System.EventArgs) Handles btnFillGrid.Click
    '    userDataGridView.DataSource = Nothing
    '    Me.fillGrid()
    '    Me.btnFirst.Enabled = True
    '    Me.btnPrevious.Enabled = True
    '    Me.lblStatus.Enabled = True
    '    Me.btnNext.Enabled = True
    '    Me.btnLast.Enabled = True
    'End Sub

    'Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
    '    btnFillGrid_Click(btnFillGrid, Nothing)
    '    origValue1 = ""
    '    origValue2 = ""
    'End Sub

    'Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Me.Close()
    '    frmMain.Show()
    'End Sub

    'Private Sub fillGrid()
    '    '--- For Page view.
    '    Me.mintPageSize = Integer.Parse(Me.tbPageSize.Text)
    '    Me.mintTotalRecords = getCount()
    '    Me.mintPageCount = Me.mintTotalRecords / Me.mintPageSize

    '    '-- Adjust page count if the last page contains partial page.
    '    If (Me.mintTotalRecords Mod Me.mintPageSize > 0) Then
    '        Me.mintPageCount += 1
    '    End If
    '    Me.mintCurrentPage = 0
    '    loadPage()
    'End Sub

    'Private Function getCount() As Integer
    '    Try
    '        '--- me select statement is very fast compare to SELECT COUNT(*)
    '        Dim strSql As String = "Select * FROM tblBills WHERE CalendarCode='M' ORDER BY BillNbr"
    '        Dim intCount As Integer = 0
    '        Dim ds As New DataSet
    '        Dim da As OleDbDataAdapter

    '        da = New OleDbDataAdapter(strSql, strLocal)
    '        da.Fill(ds, "Table")
    '        intCount = ds.Tables(0).Rows.Count

    '        If intCount < Me.mintPageSize Then
    '            btnFirst.Visible = False
    '            btnLast.Visible = False
    '            btnPrevious.Visible = False
    '            btnNext.Visible = False
    '            lblStatus.Visible = False
    '        Else
    '            btnFirst.Visible = True
    '            btnLast.Visible = True
    '            btnPrevious.Visible = True
    '            btnNext.Visible = True
    '            lblStatus.Visible = True
    '        End If

    '        da.Dispose()
    '        ds.Dispose()

    '        Return intCount
    '    Catch ex As Exception
    '        DisplayMessage(ex.Message, "Get Motions", "S")
    '        Exit Function
    '    End Try
    'End Function

    'Private Sub loadPage()
    '    Dim strSql As String = ""
    '    Dim intSkip As Integer = 0

    '    intSkip = (Me.mintCurrentPage * Me.mintPageSize)

    '    '-- Select only the n records.
    '    If intSkip <> 0 Then
    '        strSql = "SELECT TOP " & Me.mintPageSize & " BillNbr , Bill  FROM tblBills WHERE BillNbr NOT IN " & "(SELECT TOP " & intSkip & " BillNbr FROM tblBills WHERE CalendarCode='M' ORDER BY BillNbr)"
    '    Else
    '        strSql = "SELECT TOP " & Me.mintPageSize & "  BillNbr , Bill  FROM tblBills WHERE CalendarCode='M' ORDER BY BillNbr"
    '    End If
    '    Dim cmd As OleDb.OleDbCommand = Me.mcnSample.CreateCommand()
    '    cmd.CommandText = strSql

    '    Dim da As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(cmd)

    '    Dim ds As DataSet = New DataSet()
    '    da.Fill(ds, "tblVotingParameters")

    '    '-- Populate Data Grid
    '    Me.userDataGridView.DataSource = ds.Tables("tblVotingParameters").DefaultView
    '    Me.userDataGridView.Columns(0).Width = 250
    '    Me.userDataGridView.Columns(1).Width = 700
    '    Me.userDataGridView.Columns(0).HeaderText = "Code"
    '    Me.userDataGridView.Columns(1).HeaderText = "Motion"

    '    '-- Show Status
    '    '  Me.lblStatus.Text = (Me.mintCurrentPage + 1).ToString() & " / " & Me.mintPageCount

    '    cmd.Dispose()
    '    da.Dispose()
    '    ds.Dispose()
    'End Sub

    'Private Sub goFirst()
    '    Me.mintCurrentPage = 0
    '    loadPage()
    'End Sub

    'Private Sub goPrevious()
    '    If (Me.mintCurrentPage = Me.mintPageCount) Then
    '        Me.mintCurrentPage = Me.mintPageCount - 1
    '    End If

    '    Me.mintCurrentPage -= 1

    '    If (Me.mintCurrentPage < 1) Then
    '        Me.mintCurrentPage = 0
    '    End If
    '    loadPage()
    'End Sub

    'Private Sub goNext()
    '    Me.mintCurrentPage += 1

    '    If (Me.mintCurrentPage > (Me.mintPageCount - 1)) Then
    '        Me.mintCurrentPage = Me.mintPageCount - 1
    '    End If
    '    loadPage()
    'End Sub

    'Private Sub goLast()
    '    Me.mintCurrentPage = Me.mintPageCount - 1
    '    loadPage()
    'End Sub

    'Private Sub btnFirst_Click(sender As System.Object, e As System.EventArgs) Handles btnFirst.Click
    '    mintCurrentPage = 0
    '    goFirst()
    'End Sub

    'Private Sub btnPrevious_Click(sender As System.Object, e As System.EventArgs) Handles btnPrevious.Click
    '    goPrevious()
    'End Sub

    'Private Sub btnNext_Click(sender As System.Object, e As System.EventArgs) Handles btnNext.Click
    '    goNext()
    'End Sub

    'Private Sub btnLast_Click(sender As System.Object, e As System.EventArgs) Handles btnLast.Click
    '    goLast()
    'End Sub

    'Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
    '    Me.closeConnection()
    '    Me.Close()
    'End Sub

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

    'Private Sub btnDeleteAll_Click(sender As System.Object, e As System.EventArgs) Handles btnDeleteAll.Click
    '    If MsgBox("Are you sure want to delete all of the record(s)?", vbYesNo, "Delete Warning") = MsgBoxResult.Yes Then
    '        Try
    '            sqlQuery = "Delete from tblBills Where  CalendarCode='M'"
    '            V_DataSet(sqlQuery, "D")
    '            '    Me.btnFirst_Click(sender, e)
    '            ' btnCancel_Click(btnCancel, Nothing)
    '            loadPage()
    '            Me.Close()
    '            ' Exit Sub
    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Delete The Motions", "S")
    '            Exit Sub
    '        End Try
    '    End If
    'End Sub

    'Private Sub btnPrint_Click(sender As System.Object, e As System.EventArgs) Handles btnPrint.Click
    '    Try
    '        Me.Cursor = Cursors.WaitCursor
    '        LoadReport("rptMotions.rpt")
    '        Me.Cursor = Cursors.Default
    '    Catch ex As Exception
    '        DisplayMessage("Failed to print Montions report", "Print Motions Report", "S")
    '        Me.Cursor = Cursors.Default
    '        Exit Sub
    '    End Try
    'End Sub

    'Private Sub LoadReport(rptName As String, Optional intVoteID As Object = 0)
    '    '--- You can change more print options via PrintOptions property of ReportDocument
    '    Dim report1 As New ReportDocument()

    '    Try
    '        '--- use the proper report to pinter based upon who is alive
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


    '            report1.Load(gVotingPath & rptName)
    '            report1.PrintToPrinter(1, True, 0, 0)
    '            report1.Close()

    '    Catch ex As Exception
    '        DisplayMessage("An error has occurred while tying to print the vote report. Please make " & _
    '                                   "sure this PC is attached to a printer.", "Print Problem", "S")
    '        Exit Sub
    '    End Try
    'End Sub

    'Private Sub SetDefaultPrinter(PrinterName As String, DrivaerName As String, PrinterPort As String)
    '    Dim DeviceLine As String
    '    Dim r As Long

    '    DeviceLine = PrinterName & "," & DrivaerName & "," & PrinterPort

    '    '--- Store the new printer informationin the [WINDOWS] section of
    '    '--- the WIN.INI file for the DEVICE = item
    '    r = WriteProfileString("windows", "Device", DeviceLine)
    '    '--- Cause all applications to reload the INI file:
    '    '--- l = SendMessage(HWND_BROADCAST, WM_WININICHANGE, 0, "windows")
    'End Sub
#End Region

End Class