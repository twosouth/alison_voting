Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class frmPhrasesList
    Private sqlQuery As String
    Private Connection As OleDb.OleDbConnection
    Private Command As OleDbCommand
    Private Adapter As OleDbDataAdapter
    Private Builder As OleDbCommandBuilder
    Private ds, tempDataSet As DataSet
    Private userTable As DataTable
    Private dc As New DataColumn
    Private currentIndex As Integer
    Private isLastPage As Boolean
    Private totalRecords, startRecord, endRecord As Integer
    Private currentPageStartRecord, currentPageEndRecord, i As Integer
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


    Private Sub loadPage()
        Dim strSql As String = ""
        Dim intSkip As Integer = 0

        Try
            intSkip = (Me.mintCurrentPage * Me.mintPageSize)
           
            strSql = "SELECT  * FROM tblPhrases Order By code"

            Dim cmd As OleDb.OleDbCommand = Me.mcnSample.CreateCommand()
            cmd.CommandText = strSql
            Dim da As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(cmd)
            Dim ds As DataSet = New DataSet()
            da.Fill(ds, "tblPhrases")

            '--- Populate Data Grid
            Me.userDataGridView.DataSource = ds.Tables("tblPhrases").DefaultView

            With userDataGridView
                '---  Widths and Text
                .Columns(0).Width = 150
                .Columns(0).HeaderText = "Code"
                .Columns(1).Width = 740
                .Columns(1).HeaderText = "Phrase"
            End With
         
            cmd.Dispose()
            da.Dispose()
            ds.Dispose()
        Catch ex As Exception
            MsgBox(ex.GetBaseException, MsgBoxStyle.Critical, "Phrases List")
            Exit Sub
        End Try
    End Sub

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

    'Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
    '    mintCurrentPage = 0
    '    goFirst()
    'End Sub

    'Private Sub btnPrevious_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevious.Click
    '    goPrevious()
    'End Sub

    'Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
    '    goNext()
    'End Sub

    'Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
    '    goLast()
    'End Sub

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
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    Private Sub frmPhrasesList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CDWindowOpen()
        mintCurrentPage = 1
        Me.openConnection()
        loadPage()
        frmChamberDisplay.Close()
        frmVote.Close()
    End Sub

    Private Sub frmPhrasesList_Initi()
        InitializeComponent()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        origValue1 = ""
        origValue2 = ""
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MsgBox("Are you sure want to delete selected record?", MessageBoxButtons.YesNo, "Delete Warning") = MsgBoxResult.Yes Then
            Try
                If Not IsDBNull(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value) And Not IsDBNull(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value) Then
                    sqlQuery = "Delete From tblPhrases Where Code=" & CInt(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value) & " AND ucase(Phrase) ='" & UCase(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value) & "'"
                    V_DataSet(sqlQuery, "D")
                    loadPage()
                    Get_Parameters()
                    Check_ALIS_Database_Accessible()

                    userDataGridView.CurrentCell = userDataGridView.Rows(0).Cells(0)
                Else
                    MsgBox("Please select phrase to delete.", MsgBoxStyle.Information, "Delete The Phrase")
                End If
                origValue1 = ""
                origValue2 = ""
            Catch ex As Exception
                DisplayMessage(ex.Message, "Failed Delete", "S")
                Exit Sub
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
                MsgBox("Code number can not be empty!", MsgBoxStyle.Critical, "Add New or Update Phrase")
                Exit Sub
            End If

            If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
                Dim code As Integer = CInt(userDataGridView.CurrentRow.Cells(0).Value)
                Dim Phrase As String = userDataGridView.CurrentRow.Cells(1).Value

                '--- check phrase is exist or not first
                ds = V_DataSet("Select * From tblPhrases Where Code =" & origValue1 & " AND Phrase ='" & origValue2 & "'", "R")
                If ds.Tables(0).Rows.Count > 0 Then
                    '---doing update
                    V_DataSet("Update tblPhrases Set Phrase='" & Phrase & "' WHERE code = " & origValue1, "U")
                Else
                    '--- add new 
                    V_DataSet("Insert Into tblPhrases values (" & code & ", '" & Phrase & "')", "A")
                End If
                userDataGridView.CurrentCell = userDataGridView.Rows(0).Cells(0)
                Get_Parameters()
                Check_ALIS_Database_Accessible()
            Else
                MsgBox("Please enter phrase!", MsgBoxStyle.Critical, "Add New Phrase")
                Exit Sub
            End If
        Catch ex As Exception
            ' MsgBox(ex.Message, MsgBoxStyle.Critical, "Add New or Update Phrase")
        End Try
    End Sub

    Private Sub userDataGridView_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs)
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

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

    Private Sub ReloadPhrases()
        Try
            LoadPhrasesIntoArray()
        Catch ex As Exception
            DisplayMessage(ex.Message, "Load Phases", "S")
            Exit Sub
        End Try
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

#Region "Un-using codes"

    'Private Sub btnFillGrid_Click(sender As System.Object, e As System.EventArgs) Handles btnFillGrid.Click
    '    userDataGridView.DataSource = Nothing
    '    Me.fillGrid()
    '    Me.btnFirst.Enabled = True
    '    Me.btnPrevious.Enabled = True
    '    Me.lblStatus.Enabled = True
    '    Me.btnNext.Enabled = True
    '    Me.btnLast.Enabled = True
    'End Sub

    'Private Sub fillGrid()
    '    ' For Page view.
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
    '        '-- me select statement is very fast compare to SELECT COUNT(*)
    '        Dim strSql As String = "SELECT count(*) from tblPhrases"
    '        Dim intCount As Integer = 0

    '        Dim cmd As OleDb.OleDbCommand = Me.mcnSample.CreateCommand()
    '        cmd.CommandText = strSql

    '        intCount = cmd.ExecuteScalar()

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

    '        cmd.Dispose()

    '        Return intCount
    '    Catch ex As Exception
    '        MsgBox(ex.GetBaseException, MsgBoxStyle.Critical, "Phrases List")
    '        Exit Function
    '    End Try
    'End Function

    'Private Sub loadPage()
    '    Dim strSql As String = ""
    '    Dim intSkip As Integer = 0

    '    Try
    '        intSkip = (Me.mintCurrentPage * Me.mintPageSize)

    '        '-- Select only the n records.
    '        If intSkip <> 0 Then
    '            strSql = "SELECT TOP " & Me.mintPageSize & " * FROM tblPhrases WHERE code NOT IN " & "(SELECT TOP " & intSkip & " code FROM tblPhrases)"
    '        Else
    '            strSql = "SELECT TOP " & Me.mintPageSize & " * FROM tblPhrases"
    '        End If
    '        Dim cmd As OleDb.OleDbCommand = Me.mcnSample.CreateCommand()
    '        cmd.CommandText = strSql

    '        Dim da As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(cmd)

    '        Dim ds As DataSet = New DataSet()
    '        da.Fill(ds, "tblPhrases")

    '        '// Populate Data Grid
    '        Me.userDataGridView.DataSource = ds.Tables("tblPhrases").DefaultView
    '        Me.userDataGridView.Columns(0).Width = 150
    '        Me.userDataGridView.Columns(1).Width = 670

    '        '// Show Status
    '        '  Me.lblStatus.Text = (Me.mintCurrentPage + 1).ToString() & " / " & Me.mintPageCount

    '        cmd.Dispose()
    '        da.Dispose()
    '        ds.Dispose()
    '    Catch ex As Exception
    '        MsgBox(ex.GetBaseException, MsgBoxStyle.Critical, "Phrases List")
    '        Exit Sub
    '    End Try
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

    'Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click, Button1.Click
    '    mintCurrentPage = 0
    '    goFirst()
    'End Sub

    'Private Sub btnPrevious_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevious.Click, Button2.Click
    '    goPrevious()
    'End Sub

    'Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click, Button3.Click
    '    goNext()
    'End Sub

    'Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click, Button4.Click
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
    '        MessageBox.Show(ex.ToString())
    '    End Try
    'End Sub

    'Private Sub frmPhrasesList_Load(sender As Object, e As System.EventArgs) Handles Me.Load
    ' CDWindowOpen

    '    mintCurrentPage = 1
    '    Me.openConnection()
    '    btnFillGrid_Click(btnFillGrid, Nothing)
    '    frmChamberDisplay.Close()
    '    frmVote.Close()
    'End Sub

    'Private Sub frmPhrasesList_Initi()
    '    InitializeComponent()
    'End Sub

    'Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
    '    Dim frmCD As New frmChamberDisplay
    '    If MsgBox("Are you sure want to add?", MsgBoxStyle.YesNo, "Add The New Phrase") = MsgBoxResult.Yes Then
    '        Try
    '            If Not IsDBNull(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value) And Not IsDBNull(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value) Then
    '                ds = V_DataSet("Select * From tblPhrases Where code=" & UCase(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value), "R")
    '                If ds.Tables(0).Rows.Count > 0 Then
    '                    MsgBox("This code already exists. Please try again.", MsgBoxStyle.Critical, "Duplicate Code")
    '                    btnCancel_Click(btnCancel, Nothing)
    '                    Exit Sub
    '                Else
    '                    ds = V_DataSet("Select * From tblPhrases Where ucase(phrase) ='" & UCase(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value) & "'", "R")
    '                    If ds.Tables(0).Rows.Count = 0 Then
    '                        sqlQuery = "Insert Into tblPhrases Values (" & CInt(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value) & ", '" & Replace(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value, "'", "''") & "')"
    '                        V_DataSet(sqlQuery, "A")
    '                        userDataGridView.ReadOnly = False
    '                        btnCancel_Click(btnCancel, Nothing)
    '                        ReloadPhrases()
    '                        Me.Close()
    '                    Else
    '                        MsgBox("The phrase already exists. Please try again.", MsgBoxStyle.Critical, "Duplicate Code")
    '                        btnCancel_Click(btnCancel, Nothing)
    '                        Exit Sub
    '                    End If
    '                End If
    '            Else
    '                If IsDBNull(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value) Then
    '                    MsgBox("Code number can not be empty!", MsgBoxStyle.Critical, "Add New Phrase")
    '                    Exit Sub
    '                End If
    '                If IsDBNull(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value) Then
    '                    MsgBox("Phrase can not be empty!", MsgBoxStyle.Critical, "Add New Phrase")
    '                    Exit Sub
    '                End If
    '            End If
    '        Catch ex As Exception
    '            MsgBox(ex.Message, MsgBoxStyle.Critical, "Add New Phrase")
    '        End Try
    '    End If
    'End Sub

    'Private Sub btnEdit_Click(sender As System.Object, e As System.EventArgs) Handles btnEdit.Click
    '    Dim newCode As Integer
    '    Dim newPhrase As String

    '    Try
    '        If MsgBox("Are you sure want to update?", MsgBoxStyle.YesNo, "Update The Phrase") = MsgBoxResult.Yes Then
    '            If Not IsDBNull(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value) And Not IsDBNull(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value) Then
    '                newCode = userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value
    '                newPhrase = userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value

    '                ds = V_DataSet("Select * From tblPhrases Where code=" & origValue1, "R")
    '                If ds.Tables(0).Rows.Count > 0 And origValue2 = newPhrase Then
    '                    MsgBox("This code already exists. Please try again.", MsgBoxStyle.Critical, "Duplicate Code")
    '                    btnCancel_Click(btnCancel, Nothing)
    '                    Exit Sub
    '                End If
    '                ds = V_DataSet("Select * From tblPhrases Where ucase(phrase) ='" & UCase(newPhrase) & "'", "R")
    '                If ds.Tables(0).Rows.Count = 0 Then
    '                    sqlQuery = "Update tblPhrases Set  Phrase='" & newPhrase & "' Where Code=" & CInt(origValue1)
    '                    V_DataSet(sqlQuery, "U")
    '                    userDataGridView.ReadOnly = False
    '                    '  MsgBox("Update finished.", MsgBoxStyle.Information, "Update The Phrase")
    '                    btnCancel_Click(btnCancel, Nothing)
    '                    ReloadPhrases()
    '                    Me.Close()
    '                    Exit Sub
    '                Else
    '                    MsgBox("This phrase already exists. Please try again.", MsgBoxStyle.Critical, "Duplicate Code")
    '                    btnCancel_Click(btnCancel, Nothing)
    '                    Exit Sub
    '                End If

    '            Else
    '                If IsDBNull(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value) Then
    '                    MsgBox("Code number can not be empty!", MsgBoxStyle.Critical, "Update The Phrase")
    '                    Exit Sub
    '                End If
    '                If IsDBNull(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value) Then
    '                    MsgBox("Phrace can not be empty!", MsgBoxStyle.Critical, "Update The Phrase")
    '                    Exit Sub
    '                End If
    '            End If
    '        Else
    '            btnCancel_Click(btnCancel, Nothing)
    '        End If

    '    Catch ex As Exception
    '        MsgBox(ex.Message, MsgBoxStyle.Critical, "Update The Phrase")
    '    End Try
    'End Sub

    'Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
    '    btnFillGrid_Click(btnFillGrid, Nothing)
    '    origValue1 = ""
    '    origValue2 = ""
    'End Sub

    'Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click
    '    If MsgBox("Are you sure want to delete selected record(s)?", MessageBoxButtons.YesNo, "Delete Warning") = MsgBoxResult.Yes Then
    '        Try
    '            If Not IsDBNull(origValue1) And Not IsDBNull(origValue2) Then
    '                sqlQuery = "Delete From tblPhrases Where Code=" & CInt(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value) & " AND ucase(Phrase) ='" & UCase(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value) & "'"
    '                V_DataSet(sqlQuery, "D")
    '                btnCancel_Click(btnCancel, Nothing)
    '                ReloadPhrases()
    '            Else
    '                MsgBox("Please select phrase to delete.", MsgBoxStyle.Information, "Delete The Phrase")
    '            End If
    '            origValue1 = ""
    '            origValue2 = ""
    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Failed Delete", "S")
    '            Exit Sub
    '        End Try
    '    End If
    'End Sub

    'Private Sub userDataGridView_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles userDataGridView.CellBeginEdit
    '    ' Static j As Integer = 1
    '    'If (Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Or Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value)) And j = 1 Then
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
    '        'j += 1
    '    End If
    'End Sub

    'Private Sub userDataGridView_CellMouseClick(sender As Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs)
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

    'Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    '    Try
    '        Me.Cursor = Cursors.WaitCursor
    '        LoadReport("rptPhrases.rpt")
    '        Me.Cursor = Cursors.Default
    '    Catch ex As Exception
    '        DisplayMessage("Failed to print Phrases report", "Print Phrases Report", "S")
    '        Me.Cursor = Cursors.Default
    '        Exit Sub
    '    End Try
    'End Sub

    'Private Sub LoadReport(ByVal rptName As String, Optional ByVal intVoteID As Object = 0)
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


    '        report1.Load(gVotingPath & rptName)
    '        report1.PrintToPrinter(1, True, 0, 0)
    '        report1.Close()

    '    Catch ex As Exception
    '        DisplayMessage("An error has occurred while tying to print the vote report. Please make " & _
    '                                   "sure this PC is attached to a printer.", "Print Problem", "S")
    '        Exit Sub
    '    End Try
    'End Sub

    'Private Sub SetDefaultPrinter(ByVal PrinterName As String, ByVal DrivaerName As String, ByVal PrinterPort As String)
    '    Dim DeviceLine As String
    '    Dim r As Long

    '    DeviceLine = PrinterName & "," & DrivaerName & "," & PrinterPort

    '    '--- Store the new printer informationin the [WINDOWS] section of
    '    '--- the WIN.INI file for the DEVICE = item
    '    r = WriteProfileString("windows", "Device", DeviceLine)
    '    '--- Cause all applications to reload the INI file:
    '    '--- l = SendMessage(HWND_BROADCAST, WM_WININICHANGE, 0, "windows")
    'End Sub

    'Private Sub ReloadPhrases()
    '    Dim frmCD As New frmChamberDisplay
    '    Dim dsPH As New DataSet
    '    Dim p As Integer = 0

    '    Try
    '        LoadPhrasesIntoArray()

    '        'dsPH = V_DataSet("Select Cstr(Code)  + ' - ' + Phrase  AS ThePhrase, Code, Phrase From tblPhrases Order By Code", "R")
    '        'If dsPH.Tables(0).Rows.Count > 0 Then

    '        '    For Each dr As DataRow In dsPH.Tables(0).Rows

    '        '        frmCD.Phrases.Items.Add(dr("ThePhrase"))

    '        '    Next

    '        'End If
    '    Catch ex As Exception
    '        DisplayMessage(ex.Message, "Load Phases", "S")
    '        Exit Sub
    '    End Try
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
#End Region

End Class