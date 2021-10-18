Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports System.Data.Common
Imports System.Data.OleDb
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared


Public Class frmBoardAndCommissionList
   
    Private origValue1, origValue2 As String
    Private Retvalre As New Object
    Private ds, tempDataSet As DataSet

    'Private Sub frmBoardAndCommissionList_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    '    CDWindowOpen()
    '    e.Cancel = True
    '    Me.Hide()
    '    ChangeSize(frmChamberDisplay)
    'End Sub

    Private Sub frmBoardAndCommissionList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CDWindowOpen()
        loadPage()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MsgBox("Are you sure want to delete selected record?", MessageBoxButtons.YesNo, "Delete The Commission Warning") = MsgBoxResult.Yes Then
            Try
                strSQL = "Delete from tblBoardsAndCommissions Where ucase(AlisName) ='" & UCase(userDataGridView.CurrentRow.Cells(0).Value) & "'"
                V_DataSet(strSQL, "D")
                loadPage()
                LoadCommitteesIntoArray()
                userDataGridView.CurrentCell = userDataGridView.Rows(0).Cells(0)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Delete Board and Committee")
            End Try
        End If
    End Sub

    Private Sub userDataGridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles userDataGridView.CellEndEdit
        Dim ds As New DataSet
        Dim i, k As Integer

        Try
            If IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Then
                MsgBox("Alis committee name can not be empty!", MsgBoxStyle.Critical, "Add New Phrase")
                Exit Sub
            End If

            If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
                Dim AlisName As String = Trim(userDataGridView.CurrentRow.Cells(0).Value)
                Dim SenateVotingName As String = Trim(userDataGridView.CurrentRow.Cells(1).Value)

                '--- check parameter is exist or not first
                ds = V_DataSet("Select * From tblBoardsAndCommissions Where ltrim(AlisName) ='" & LTrim(origValue1) & "' AND ltrim(SenateVotingName) ='" & LTrim(origValue2) & "'", "R")
                If ds.Tables(0).Rows.Count > 0 Then
                    '--- doing update
                    V_DataSet("Update tblBoardsAndCommissions Set AlisName='" & AlisName & "', SenateVotingName ='" & SenateVotingName & "' WHERE AlisName = '" & Trim(origValue1) & "'", "U")
                    loadPage()
                Else
                    Dim dsC As New DataSet
                    dsC = V_DataSet("Select * From tblBoardsAndCommissions Where ltrim(AlisName) ='" & LTrim(origValue1) & "'", "R")
                    If dsC.Tables(0).Rows.Count = 0 Then
                        '--- add new 
                        V_DataSet("Insert Into tblBoardsAndCommissions values ('" & AlisName & "', '" & SenateVotingName & "')", "A")
                        loadPage()
                    Else
                        For i = 0 To userDataGridView.Rows.Count - 1
                            If userDataGridView.Rows(i).Cells(0).Value = AlisName Then
                                K = userDataGridView.Rows(i).Index
                                MsgBox("Boards And Commissions " & AlisName & " already exist.", vbInformation, "Motion Maintenance")
                                loadPage()
                                userDataGridView.Rows(k).Selected = True
                                Exit For
                            End If
                        Next
                    End If
                End If
                LoadCommitteesIntoArray()
            Else
                ' MsgBox("Senate voting committee name can not be empty.", MsgBoxStyle.Critical, "Add New or Update Commission")
                Exit Sub
            End If
        Catch ex As Exception
            '  MsgBox(ex.Message, MsgBoxStyle.Critical, "Add New or Update Commission")
        End Try
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

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        origValue1 = ""
        origValue2 = ""
    End Sub

    Private Sub loadPage()
        Dim strSql As String = ""
        Dim intSkip As Integer = 0

        Try
            strSql = "SELECT * FROM tblBoardsAndCommissions Order By ALISName "
            ds = V_DataSet(strSql, "R")
            Me.userDataGridView.DataSource = ds.Tables(0).DefaultView
            Me.userDataGridView.Columns(0).Width = 500
            Me.userDataGridView.Columns(1).Width = 500
            Me.userDataGridView.Columns(0).HeaderText = "Alis Name"
            Me.userDataGridView.Columns(1).HeaderText = "Senate Voting Name"
            ds.Dispose()
        Catch ex As Exception
            MsgBox(ex.GetBaseException, MsgBoxStyle.Critical, "Board and Commission")
            Exit Sub
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        '  CDWindowOpen()
        Me.Close()
        ' ChangeSize(frmChamberDisplay)
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

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            LoadReport("rptCommitteeBoard.rpt")
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            DisplayMessage("Failed to print Motions report", "Print Motions Report", "S")
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

#Region "Old-codes"
    'Private sqlQuery As String
    'Private Connection As OleDb.OleDbConnection
    'Private Command As OleDbCommand
    'Private Adapter As OleDbDataAdapter
    'Private Builder As OleDbCommandBuilder

    'Private userTable As DataTable
    'Private dc As New DataColumn
    'Private currentIndex As Integer
    'Private isLastPage As Boolean
    'Private totalRecords, startRecord, endRecord As Integer
    'Private currentPageStartRecord, currentPageEndRecord As Integer
    'Private mintTotalRecords As Integer = 0
    'Private mintPageSize As Integer = 0
    'Private mintPageCount As Integer = 0
    'Private mintCurrentPage As Integer = 1
    'Protected mcnSample As OleDb.OleDbConnection
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

    'Private Sub frmBoardAndCommissionList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    CDWindowOpen()
    '    mintCurrentPage = 1
    '    Me.openConnection()
    '    btnFillGrid_Click(btnFillGrid, Nothing)

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

    'Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
    '    Try
    '        userDataGridView.ReadOnly = False
    '        If Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) And Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
    '            If MsgBox("Are you sure want to add new Senate Voting Commission?", MsgBoxStyle.YesNo, "Add The New Commission") = MsgBoxResult.Yes Then
    '                Try
    '                    ds = V_DataSet("Select * FROM tblBoardsAndCommissions Where ucase(AlisName) ='" & Replace(UCase(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value), "'", "''") & "'", "R")
    '                    If ds.Tables(0).Rows.Count > 0 Then
    '                        MsgBox("TheAlis Name already exists. Please try again.", vbInformation, "Duplicate Alis Name waring")
    '                        Exit Sub
    '                    Else
    '                        sqlQuery = "Insert Into tblBoardsAndCommissions values ('" & Replace(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value, "'", "''") & "', '" & userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value & "')"
    '                        V_DataSet(sqlQuery, "A")
    '                        btnCancel_Click(btnCancel, Nothing)
    '                        Me.Close()
    '                    End If
    '                    userDataGridView.ReadOnly = False
    '                Catch ex As Exception
    '                    MessageBox.Show(ex.ToString())
    '                    btnCancel.Enabled = True
    '                Finally
    '                    btnAdd.Enabled = True
    '                End Try
    '            Else
    '                btnCancel_Click(btnCancel, Nothing)
    '            End If
    '        Else
    '            If IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Then
    '                MsgBox("AlisName can not be empty!", MsgBoxStyle.Critical, "Add The New Commission")
    '            End If
    '            If IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
    '                MsgBox("Senate Voting Commission can not be empty!", MsgBoxStyle.Critical, "Add The New Commission")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.GetBaseException, MsgBoxStyle.Critical, "Board and Commission")
    '        Exit Sub
    '    End Try
    'End Sub

    'Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
    '    Dim newAlisName, newSenateVotingName As String

    '    Try
    '        newAlisName = userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value
    '        newSenateVotingName = userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value
    '        userDataGridView.ReadOnly = False

    '        If Not IsDBNull(newAlisName) And Not IsDBNull(newSenateVotingName) Then
    '            If MsgBox("Are you sure want to update?", MsgBoxStyle.YesNo, "Update The Commission") = MsgBoxResult.Yes Then
    '                Try
    '                    If origValue1 <> newAlisName And origValue2 <> newSenateVotingName Then
    '                        sqlQuery = "Update tblBoardsAndCommissions Set AlisName='" & newAlisName & "', SenateVotingName='" & newSenateVotingName & "' Where ucase(AlisName) ='" & UCase(origValue1) & "'"
    '                    End If
    '                    If origValue1 = newAlisName And origValue2 <> newSenateVotingName Then
    '                        sqlQuery = "Update  tblBoardsAndCommissions Set  SenateVotingName='" & newSenateVotingName & "' Where ucase(SenateVotingName) ='" & UCase(origValue2) & "'"
    '                    End If
    '                    If origValue1 <> newAlisName And origValue2 = newSenateVotingName Then
    '                        sqlQuery = "Update  tblBoardsAndCommissions Set AlisName='" & newAlisName & "' Where ucase(AlisName) ='" & UCase(origValue1) & "'"
    '                    End If
    '                    V_DataSet(sqlQuery, "U")

    '                    userDataGridView.ReadOnly = False
    '                    btnCancel_Click(btnCancel, Nothing)
    '                    Me.Close()
    '                Catch ex As Exception
    '                    MsgBox(ex.Message, MsgBoxStyle.Critical, "Update Board and Committee")
    '                End Try
    '            End If
    '        Else
    '            If IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Then
    '                MsgBox("AlisName can not be empty!", MsgBoxStyle.Critical, "Add The New Commission")
    '            End If
    '            If IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
    '                MsgBox("Senate Voting Commission can not be empty!", MsgBoxStyle.Critical, "Add The New Commission")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.GetBaseException, MsgBoxStyle.Critical, "Board and Commission")
    '        Exit Sub
    '    End Try
    'End Sub

    'Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
    '    If MsgBox("Are you sure want to delete selected record(s)?", MessageBoxButtons.YesNo, "Delete The Commission Warning") = MsgBoxResult.Yes Then
    '        Try
    '            sqlQuery = "Delete from tblBoardsAndCommissions Where ucase(AlisName) ='" & UCase(userDataGridView.CurrentRow.Cells(0).Value) & "'"
    '            V_DataSet(sqlQuery, "D")
    '            btnCancel_Click(btnCancel, Nothing)
    '            Me.Close()
    '        Catch ex As Exception
    '            MsgBox(ex.Message, MsgBoxStyle.Critical, "Delete Board and Committee")
    '        End Try
    '    End If
    'End Sub

    ''Private Sub lblBoardAndCommissionList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    ''    btnLast.Enabled = True
    ''End Sub

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

    'Private Sub userDataGridView_CellClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles userDataGridView.CellClick
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
    '    '--- me select statement is very fast compare to SELECT COUNT(*)
    '    Dim strSql As String = "Select * FROM tblBoardsAndCommissions"
    '    Dim intCount As Integer = 0
    '    Dim ds As New DataSet
    '    Dim da As OleDbDataAdapter

    '    Try
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
    '        MsgBox(ex.GetBaseException, MsgBoxStyle.Critical, "Board and Commission")
    '        Exit Function
    '    End Try
    'End Function

    'Private Sub loadPage()
    '    Dim strSql As String = ""
    '    Dim intSkip As Integer = 0

    '    Try
    '        intSkip = (Me.mintCurrentPage * Me.mintPageSize)

    '        '--- Select only the n records.
    '        If intSkip <> 0 Then
    '            strSql = "SELECT TOP " & Me.mintPageSize & " * FROM tblBoardsAndCommissions WHERE AlisName NOT IN " & "(SELECT TOP " & intSkip & " AlisName FROM tblBoardsAndCommissions)"
    '        Else
    '            strSql = "SELECT TOP " & Me.mintPageSize & " * FROM tblBoardsAndCommissions"
    '        End If
    '        Dim cmd As OleDb.OleDbCommand = Me.mcnSample.CreateCommand()
    '        cmd.CommandText = strSql

    '        Dim da As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(cmd)

    '        Dim ds As DataSet = New DataSet()
    '        da.Fill(ds, "tblPhrases")

    '        '--- Populate Data Grid
    '        Me.userDataGridView.DataSource = ds.Tables("tblPhrases").DefaultView
    '        Me.userDataGridView.Columns(0).Width = 500
    '        Me.userDataGridView.Columns(1).Width = 500
    '        Me.userDataGridView.Columns(0).HeaderText = "Alis Name"
    '        Me.userDataGridView.Columns(1).HeaderText = "Senate Voting Name"

    '        '--- Show Status
    '        '  Me.lblStatus.Text = (Me.mintCurrentPage + 1).ToString() & " / " & Me.mintPageCount

    '        cmd.Dispose()
    '        da.Dispose()
    '        ds.Dispose()
    '    Catch ex As Exception
    '        MsgBox(ex.GetBaseException, MsgBoxStyle.Critical, "Board and Commission")
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
    '        MessageBox.Show(ex.ToString())
    '    End Try
    'End Sub
#End Region

   
End Class

