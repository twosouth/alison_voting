Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports System.Data.Common
Imports System.Data.OleDb

Public Class frmVotingParameterMaintenance
    Private sqlQuery As String
    Private Connection As OleDb.OleDbConnection
    Private Command As OleDbCommand
    Private Adapter As OleDbDataAdapter
    Private Builder As OleDbCommandBuilder
    Private ds As DataSet
    Private tempDataSet As DataSet
    Private userTable As DataTable
    Private dc As New DataColumn
    Private currentIndex As Integer
    Private isLastPage As Boolean
    Private totalRecords, startrecord, endrecord As Integer
    Private currentPageStartRecord As Integer
    Private currentPageEndRecord As Integer
    Private origValue1, origValue2 As String
    Private RetValue As VariantType, WriteVotesToTestStart, OK As Boolean
    Private mintTotalRecords As Integer = 0
    Private mintPageSize As Integer = 0
    Private mintPageCount As Integer = 0
    Private mintCurrentPage As Integer = 1
    Protected mcnSample As OleDb.OleDbConnection

    Private Sub frmVotingParameterMaintenance_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        WriteVotesToTestStart = gWriteVotesToTest
    End Sub

    Private Sub frmVotingParameterMaintenanceNew_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CDWindowOpen()
        mintCurrentPage = 1
        Me.openConnection()

        loadPage()
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

    Private Sub userDataGridView_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
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
                MsgBox("Parameter name can not be empty!", MsgBoxStyle.Critical, "Add New or Update Parameter")
                Exit Sub
            End If

            If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
                Dim parameterName As String = userDataGridView.CurrentRow.Cells(0).Value
                Dim parameterValue As String = userDataGridView.CurrentRow.Cells(1).Value

                If UCase(userDataGridView.CurrentRow.Cells(0).Value.ToString) = "NUMBEROFSENATOR" Then
                    If UCase(userDataGridView.CurrentRow.Cells(1).Value.ToString) <> "35" Then
                        MsgBox("Senators number can not greater 35 or less than 35!", MsgBoxStyle.Critical, "Add or Update Parameter")
                        Exit Sub
                    End If
                End If

                '--- check parameter is exist or not first
                ds = V_DataSet("Select * From tblVotingParameters Where Parameter ='" & origValue1 & "' AND ParameterValue ='" & origValue2 & "'", "R")
                If ds.Tables(0).Rows.Count > 0 Then
                    '---doing update
                    V_DataSet("Update tblVotingParameters Set ParameterValue='" & parameterValue & "' WHERE Parameter = '" & Trim(origValue1) & "'", "U")
                Else
                    '--- add new 
                    V_DataSet("Insert Into tblVotingParameters values ('" & parameterName & "', '" & parameterValue & "')", "A")
                End If
                userDataGridView.CurrentCell = userDataGridView.Rows(0).Cells(0)
                Get_Parameters()
                Check_ALIS_Database_Accessible()
                MsgBox("Parameter value has been changed! Voting System will close. Please restart it to reload new parameters.", MsgBoxStyle.Critical, "Add or Update Paramemter")
                End
            Else
                MsgBox("Please enter parameter value!", MsgBoxStyle.Critical, "Add or Update Parameter")
                Exit Sub
            End If
        Catch ex As Exception
            '  MsgBox(ex.Message, MsgBoxStyle.Critical, "Add New or Update Voting System Parameter")
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        origValue1 = ""
        origValue2 = ""
    End Sub

    Private Sub loadPage()
        Dim strSql As String = ""
        Dim intSkip As Integer = 0

        Try
            strSql = "SELECT  * FROM tblVotingParameters Order By Parameter"
            Dim cmd As OleDb.OleDbCommand = Me.mcnSample.CreateCommand()
            cmd.CommandText = strSql

            Dim da As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(cmd)

            Dim ds As DataSet = New DataSet()
            da.Fill(ds, "tblVotingParameters")

            '-- Populate Data Grid
            Me.userDataGridView.DataSource = ds.Tables("tblVotingParameters").DefaultView
            Me.userDataGridView.Columns(0).Width = 300
            Me.userDataGridView.Columns(1).Width = 650
            Me.userDataGridView.Columns(1).HeaderText = "Parameter Value"

            cmd.Dispose()
            da.Dispose()
            ds.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Vote System Parameters")
        End Try
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
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    Private Sub userDataGridView_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles userDataGridView.CellBeginEdit
        Static j As Integer = 1

        If (Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Or Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value)) Then
            If Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Then
                origValue1 = UCase(Trim(userDataGridView.CurrentRow.Cells(0).Value))
            Else
                origValue1 = ""
            End If
            If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
                origValue2 = UCase(Trim(userDataGridView.CurrentRow.Cells(1).Value))
            Else
                origValue2 = ""
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btndelete.Click
        Dim selectedCellCount As Integer = userDataGridView.GetCellCount(DataGridViewElementStates.Selected)

        If selectedCellCount > 0 Then
            Dim PName As String = userDataGridView.CurrentRow.Cells(0).Value
            Dim PValue As String = userDataGridView.CurrentRow.Cells(1).Value

            If MsgBox("Are you sure want to delete the record?", vbYesNo, "Delete Parameter") = vbYes Then
                V_DataSet("Delete From tblVotingParameters Where Parameter ='" & PName & "' AND ParameterValue ='" & PValue & "'", "D")
                loadPage()
                Get_Parameters()
                Check_ALIS_Database_Accessible()
                userDataGridView.CurrentCell = userDataGridView.Rows(0).Cells(0)
            End If
        End If
    End Sub

#Region "Un-using codes"
    'Private Sub frmVotingParameterMaintenance_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
    '    WriteVotesToTestStart = gWriteVotesToTest
    'End Sub

    'Private Sub frmVotingParameterMaintenanceNew_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

    'Private Sub userDataGridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles userDataGridView.CellEndEdit
    '    If IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Then
    '        MsgBox("Code number can not be empty!", MsgBoxStyle.Critical, "Add New Phrase")
    '        Exit Sub
    '    ElseIf IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
    '        MsgBox("Phrase can not be empty!", MsgBoxStyle.Critical, "Add New Phrase")
    '        Exit Sub
    '    End If

    '    If UCase(userDataGridView.CurrentRow.Cells(0).Value.ToString) = "NUMBEROFSENATOR" Then
    '        If UCase(userDataGridView.CurrentRow.Cells(1).Value.ToString) <> "35" Then
    '            MsgBox("Senators number can not greater 35 or less than 35!", MsgBoxStyle.Critical, "Add New Phrase")
    '            Exit Sub
    '        End If
    '    End If

    '    V_DataSet("Update tblVotingParameters Set ParameterValue='" & userDataGridView.CurrentRow.Cells(1).Value.ToString & "' WHERE ucase(Parameter) = '" & UCase(userDataGridView.CurrentRow.Cells(0).Value.ToString) & "'", "U")
    '    Get_Parameters()
    '    Check_ALIS_Database_Accessible()

    'End Sub

    'Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

    '    Get_Parameters()
    '    Check_ALIS_Database_Accessible()
    '    'Dim daU As New OleDbDataAdapter
    '    'Dim strU As String = ""
    '    'Dim txtParameter, txtValue As String

    '    'Try
    '    '    If MsgBox("Are you sure to update parameter?", MsgBoxStyle.YesNo, "Update Voting Parameter") = MsgBoxResult.Yes Then
    '    '        'connLocal = New OleDbConnection(strLocal)
    '    '        'connLocal.Open()
    '    '        txtParameter = UCase(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value)
    '    '        txtValue = UCase(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value)

    '    '        If IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
    '    '            If IsNumeric(origValue2) Then
    '    '                RetValue = DisplayMessage("Parameter Value must be > 0", "Invalid Entry", "S")
    '    '            End If
    '    '            If origValue2 <> "" Then
    '    '                RetValue = DisplayMessage("Parameter value is required", "Missing Entry", "S")
    '    '            End If
    '    '            btnCancel_Click(btnCancel, Nothing)
    '    '            Exit Sub
    '    '        End If

    '    '        If origValue1 = "" Or origValue2 = "" Then
    '    '            MsgBox("Please re-select and re-enter parameter value to update.", MsgBoxStyle.OkOnly, "Update Voting Parameter")
    '    '            btnCancel_Click(btnCancel, Nothing)
    '    '        Else
    '    '            '-- Case 1. not change "TestMode" and "WriteVotesToAlisTest"
    '    '            If (UCase(txtParameter) <> UCase("TESTMODE")) And (UCase(txtParameter) <> UCase("WRITEVOTESTOALISTEST")) Then
    '    '                strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where ucase(Parameter) ='" & UCase(origValue1) & "'"
    '    '                V_DataSet(strU, "U")
    '    '                Get_Parameters()

    '    '                gTestMode = False
    '    '                gWriteVotesToTest = False
    '    '                origValue1 = ""
    '    '                origValue2 = ""

    '    '                '-- if changed senators number, has to resize senator array size
    '    '                If Trim(UCase(txtParameter)) = "NUMBEROFSENATOR" Then
    '    '                    If txtValue <> 35 Then
    '    '                        gSenatorSplit = Int((gNbrSenators / 2) + 0.5)
    '    '                        ReDim gSenatorName(gNbrSenators)
    '    '                        ReDim gSalutation(gNbrSenators)
    '    '                        ReDim gSenatorNameOrder(gNbrSenators)
    '    '                        ReDim gSenatorOID(gNbrSenators)
    '    '                        ReDim gDistrictOID(gNbrSenators)
    '    '                        LoadSenatorsIntoArray()
    '    '                    End If
    '    '                End If

    '    '                If Trim(UCase(txtParameter)) = "CREATEHTMLPAGE" Then
    '    '                    If txtValue = "Y" Then
    '    '                        gCreateHTMLPage = True
    '    '                    ElseIf txtValue = "N" Then
    '    '                        gCreateHTMLPage = False
    '    '                    End If
    '    '                End If
    '    '                If Trim(UCase(txtParameter)) = "CHAMBERHILP" Then
    '    '                    If txtValue = "Y" Then
    '    '                        gChamberHelp = True
    '    '                    ElseIf txtValue = "N" Then
    '    '                        gChamberHelp = False
    '    '                    End If
    '    '                End If
    '    '                If Trim(UCase(txtParameter)) = "VOTINGHELP" Then
    '    '                    If txtValue = "Y" Then
    '    '                        gVotingHelp = True
    '    '                    ElseIf txtValue = "N" Then
    '    '                        gVotingHelp = False
    '    '                    End If
    '    '                End If
    '    '                If Trim(UCase(txtParameter)) = "PRINTVOTEREPORT" Then
    '    '                    If txtValue = "Y" Then
    '    '                        gPrintVoteRpt = True
    '    '                    ElseIf txtValue = "N" Then
    '    '                        gPrintVoteRpt = False
    '    '                    End If
    '    '                End If
    '    '                If Trim(UCase(txtParameter)) = "DELETETESTVOTESONSTART" Then
    '    '                    If txtValue = "Y" Then
    '    '                        gDeleteTestVotesOnStartUp = True
    '    '                    ElseIf txtValue = "N" Then
    '    '                        gDeleteTestVotesOnStartUp = False
    '    '                    End If
    '    '                End If
    '    '                If Trim(UCase(txtParameter)) = "WRITEVOTESTOALISTEST" Then
    '    '                    If txtValue = "Y" Then
    '    '                        gPrintVoteRpt = True
    '    '                    ElseIf txtValue = "N" Then
    '    '                        gPrintVoteRpt = False
    '    '                    End If
    '    '                End If
    '    '                If Trim(UCase(txtParameter)) = "TESTMODE" Then
    '    '                    If txtValue = "Y" Then
    '    '                        gTestMode = True
    '    '                    ElseIf txtValue = "N" Then
    '    '                        gTestMode = False
    '    '                    End If
    '    '                End If


    '    '                ' MsgBox("Update finished", MsgBoxStyle.OkOnly, "Update Voting Parameter")
    '    '            End If

    '    '            '-- Case 2. Change "TestMode"
    '    '            If txtParameter = "TESTMODE" Then
    '    '                If gWriteVotesToTest = False Then
    '    '                    If origValue2 = "N" And txtValue = "Y" Then
    '    '                        gTestMode = True
    '    '                    ElseIf origValue2 = "Y" And txtValue = "N" Then
    '    '                        gTestMode = False
    '    '                    End If
    '    '                    strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where ucase(Parameter)='" & UCase(origValue1) & "'"
    '    '                    V_DataSet(strU, "U")
    '    '                    RetValue = DisplayMessage("Update finished. Since you changed the test mode parameter, the system must be restarted " & _
    '    '                               "so that the correct parameters can be imported.", "System Restart", "S")
    '    '                    End
    '    '                Else
    '    '                    If origValue2 = "N" And txtValue = "Y" Then
    '    '                        gTestMode = True
    '    '                        gWriteVotesToTest = True
    '    '                        strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where UCase(Parameter)='" & UCase(origValue1) & "'"
    '    '                        V_DataSet(strU, "U")
    '    '                        RetValue = DisplayMessage("Update finished. Since you changed the 'WriteVoteToAlis' parameter, the system must be restarted " & _
    '    '                                   "so that the correct parameters can be imported.", "System Restart", "S")
    '    '                        End
    '    '                    ElseIf origValue2 = "Y" And txtValue = "N" Then
    '    '                        gTestMode = False
    '    '                        gWriteVotesToTest = False

    '    '                        strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where UCase(Parameter)='" & UCase(origValue1) & "'"
    '    '                        V_DataSet(strU, "U")

    '    '                        strU = "Update  tblVotingParameters Set Parameter='WriteVotesToAlisTest', ParameterValue='N' Where UCase(Parameter)='WRITEVOTESTOALISTEST'"
    '    '                        V_DataSet(strU, "U")

    '    '                        RetValue = DisplayMessage("Update finished. Since you changed the Test Mode To Production, the system must be restarted " & _
    '    '                        "so that the correct parameters can be imported.", "System Restart", "S")
    '    '                        End
    '    '                    End If
    '    '                End If
    '    '            End If

    '    '            '-- Case 3. Change "WriteVotesToAlisTest" parameter
    '    '            If UCase(txtParameter) = UCase("WRITEVOTESTOALISTEST") Then
    '    '                If gTestMode Then
    '    '                    If origValue2 = "N" And txtValue = "Y" Then
    '    '                        gWriteVotesToTest = True
    '    '                    ElseIf origValue2 = "Y" And txtValue = "N" Then
    '    '                        gWriteVotesToTest = False
    '    '                    End If
    '    '                    strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where UCase(Parameter)='" & UCase(origValue1) & "'"
    '    '                    V_DataSet(strU, "U")
    '    '                    RetValue = DisplayMessage("Update finished. Since you changed the Test Mode To Production, the system must be restarted " & _
    '    '                 "so that the correct parameters can be imported.", "System Restart", "S")
    '    '                    End
    '    '                Else
    '    '                    If origValue2 = "Y" And txtValue = "N" Then
    '    '                        gWriteVotesToTest = False
    '    '                        strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where UCase(Parameter)='" & UCase(origValue1) & "'"
    '    '                        V_DataSet(strU, "U")
    '    '                        RetValue = DisplayMessage("Update finished. Since you changed the Test Mode To Production, the system must be restarted " & _
    '    '                     "so that the correct parameters can be imported.", "System Restart", "S")
    '    '                        End
    '    '                    ElseIf origValue2 = "N" And txtValue = "Y" Then
    '    '                        gWriteVotesToTest = False
    '    '                        RetValue = DisplayMessage("Waring! Since 'TestMode' turnned off, disallowd change 'WrinteVotesToAlis' to on.", "System Restart", "S")
    '    '                        btnCancel_Click(btnCancel, Nothing)
    '    '                    End If
    '    '                End If
    '    '            End If
    '    '        End If
    '    '        DisplayMessage("Vote Parameter has been changed. System will shut down. Re-Start program to get new parameters.", "Vote Parameter Changed", "S")
    '    '        End
    '    '    Else
    '    '        btnCancel_Click(btnCancel, Nothing)
    '    '        MsgBox("Update cancelled", MsgBoxStyle.OkOnly, "Update Voting Parameter")
    '    '    End If
    '    'Catch ex As Exception
    '    '    MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Vote System Parameters")
    '    'End Try
    'End Sub

    'Private Sub btnFillGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFillGrid.Click
    '    userDataGridView.DataSource = Nothing
    '    Me.fillGrid()
    '    Me.btnFirst.Enabled = True
    '    Me.btnPrevious.Enabled = True
    '    Me.lblStatus.Enabled = True
    '    Me.btnNext.Enabled = True
    '    Me.btnLast.Enabled = True
    'End Sub

    'Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
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
    '    ''-- me select statement is very fast compare to SELECT COUNT(*)
    '    Dim strSql As String = "Select * FROM tblVotingParameters Order By Parameter"
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
    '        MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Vote System Parameters")
    '    End Try
    'End Function

    'Private Sub loadPage()
    '    Dim strSql As String = ""
    '    Dim intSkip As Integer = 0

    '    Try
    '        intSkip = (Me.mintCurrentPage * Me.mintPageSize)

    '        '-- Select only the n records.
    '        If intSkip <> 0 Then
    '            strSql = "SELECT TOP " & Me.mintPageSize & " * FROM tblVotingParameters WHERE Parameter NOT IN " & "(SELECT TOP " & intSkip & " Parameter FROM tblVotingParameters )"
    '        Else
    '            strSql = "SELECT TOP " & Me.mintPageSize & " * FROM tblVotingParameters"
    '        End If
    '        Dim cmd As OleDb.OleDbCommand = Me.mcnSample.CreateCommand()
    '        cmd.CommandText = strSql

    '        Dim da As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(cmd)

    '        Dim ds As DataSet = New DataSet()
    '        da.Fill(ds, "tblVotingParameters")

    '        '-- Populate Data Grid
    '        Me.userDataGridView.DataSource = ds.Tables("tblVotingParameters").DefaultView
    '        Me.userDataGridView.Columns(0).Width = 400
    '        Me.userDataGridView.Columns(1).Width = 700
    '        Me.userDataGridView.Columns(1).HeaderText = "Parameter Value"

    '        '-- Show Status
    '        ' Me.lblStatus.Text = (Me.mintCurrentPage + 1).ToString() & " / " & Me.mintPageCount

    '        cmd.Dispose()
    '        da.Dispose()
    '        ds.Dispose()
    '    Catch ex As Exception
    '        MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Vote System Parameters")
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

    'Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
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

    'Private Sub userDataGridView_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles userDataGridView.CellBeginEdit
    '    Static j As Integer = 1
    '    ' If (Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Or Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value)) And j = 1 Then
    '    If (Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Or Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value)) Then
    '        If Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Then
    '            origValue1 = UCase(Trim(userDataGridView.CurrentRow.Cells(0).Value))
    '        Else
    '            origValue1 = ""
    '        End If
    '        If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
    '            origValue2 = UCase(Trim(userDataGridView.CurrentRow.Cells(1).Value))
    '        Else
    '            origValue2 = ""
    '        End If
    '        '  j += 1
    '    End If
    'End Sub


#End Region

#Region "Un-using codes"

    'Private Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
    '    Dim daU As New OleDbDataAdapter
    '    Dim strU, txtParameter, txtValue As String

    '    Try
    '        If MsgBox("Are you sure to update parameter?", MsgBoxStyle.YesNo, "Update Voting Parameter") = MsgBoxResult.Yes Then
    '            connLocal = New OleDbConnection(strLocal)
    '            connLocal.Open()
    '            txtParameter = UCase(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value)
    '            txtValue = UCase(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value)

    '            If IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
    '                If IsNumeric(origValue2) Then
    '                    RetValue = DisplayMessage("Parameter Value must be > 0", "Invalid Entry", "S")
    '                End If
    '                If origValue2 <> "" Then
    '                    RetValue = DisplayMessage("Parameter value is required", "Missing Entry", "S")
    '                End If
    '                btnCancel_Click(btnCancel, Nothing)
    '                Exit Sub
    '            End If

    '            If origValue1 = "" Or origValue2 = "" Then
    '                MsgBox("Please re-select and re-enter parameter value to update.", MsgBoxStyle.OkOnly, "Update Voting Parameter")
    '                btnCancel_Click(btnCancel, Nothing)
    '            Else
    '                If UCase(txtParameter) <> UCase("WRITEVOTESTOALISTEST") Then
    '                    '' strU = "Update  tblVotingParameters Set Parameter='" & userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value & "', ParameterValue='" & userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value & "' Where Parameter='" & origValue1 & "'"
    '                    If txtParameter <> "TESTMODE" Then
    '                        strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where Parameter='" & origValue1 & "'"
    '                        daU.UpdateCommand = connLocal.CreateCommand
    '                        daU.UpdateCommand.CommandText = strU
    '                        daU.UpdateCommand.ExecuteNonQuery()

    '                        Get_Parameters()

    '                        origValue1 = ""
    '                        origValue2 = ""

    '                        '-- if changed senators number, has to resize senator array size
    '                        If Trim(UCase(txtParameter)) = "NUMBEROFSENATOR" Then
    '                            If txtValue <> 35 Then
    '                                gSenatorSplit = Int((gNbrSenators / 2) + 0.5)
    '                                ReDim gSenatorName(gNbrSenators)
    '                                ReDim gSalutation(gNbrSenators)
    '                                ReDim gSenatorNameOrder(gNbrSenators)
    '                                ReDim gSenatorOID(gNbrSenators)
    '                                ReDim gDistrictOID(gNbrSenators)
    '                                LoadSenatorsIntoArray()
    '                            End If
    '                        End If
    '                        MsgBox("Update finished", MsgBoxStyle.OkOnly, "Update Voting Parameter")
    '                    Else
    '                        If gWriteVotesToTest = False Then
    '                            strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where Parameter='" & origValue1 & "'"
    '                            daU.UpdateCommand = connLocal.CreateCommand
    '                            daU.UpdateCommand.CommandText = strU
    '                            daU.UpdateCommand.ExecuteNonQuery()
    '                            RetValue = DisplayMessage("Update finished. Since you changed the test mode parameter, the system must be restarted " & _
    '                            "so that the correct parameters can be imported.", "System Restart", "S")
    '                            End
    '                        Else
    '                            If txtParameter = "TESTMODE" And origValue2 = "Y" And txtValue = "N" Then
    '                                strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where Parameter='" & origValue1 & "'"
    '                                daU.UpdateCommand = connLocal.CreateCommand
    '                                daU.UpdateCommand.CommandText = strU
    '                                daU.UpdateCommand.ExecuteNonQuery()

    '                                strU = "Update  tblVotingParameters Set Parameter='WriteVotesToAlisTest', ParameterValue='N' Where Parameter='WriteVotesToAlisTest'"
    '                                daU.UpdateCommand = connLocal.CreateCommand
    '                                daU.UpdateCommand.CommandText = strU
    '                                daU.UpdateCommand.ExecuteNonQuery()
    '                                RetValue = DisplayMessage("Update finished. Since you changed the Test Mode To Production, the system must be restarted " & _
    '                                "so that the correct parameters can be imported.", "System Restart", "S")
    '                                End
    '                            End If
    '                        End If
    '                    End If
    '                Else
    '                    '--cause 1
    '                    If gTestMode And gWriteVotesToTest Then
    '                        If txtParameter = "TESTMODE" And origValue2 = "Y" And txtValue = "N" Then
    '                            strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where Parameter='" & origValue1 & "'"
    '                            daU.UpdateCommand = connLocal.CreateCommand
    '                            daU.UpdateCommand.CommandText = strU
    '                            daU.UpdateCommand.ExecuteNonQuery()

    '                            strU = "Update  tblVotingParameters Set Parameter='WriteVotesToAlisTest', ParameterValue='N' Where Parameter='WriteVotesToAlisTest'"
    '                            daU.UpdateCommand = connLocal.CreateCommand
    '                            daU.UpdateCommand.CommandText = strU
    '                            daU.UpdateCommand.ExecuteNonQuery()
    '                            RetValue = DisplayMessage("Update finished. Since you changed the Test Mode To Production, the system must be restarted " & _
    '                          "so that the correct parameters can be imported.", "System Restart", "S")
    '                            End
    '                        End If

    '                        If txtParameter = "WRITEVOTESTOALISTEST" And origValue2 = "Y" And txtValue = "N" Then
    '                            strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where Parameter='" & origValue1 & "'"
    '                            daU.UpdateCommand = connLocal.CreateCommand
    '                            daU.UpdateCommand.CommandText = strU
    '                            daU.UpdateCommand.ExecuteNonQuery()
    '                            RetValue = DisplayMessage("Update finished. Since you changed the Write Votes To Test parameter, the system must be restarted " & _
    '                           "so that the correct bills can be imported.", "System Restart", "S")
    '                            End
    '                        End If
    '                    End If

    '                    '--cause 2
    '                    If gTestMode = False And gWriteVotesToTest = False Then
    '                        If txtParameter = "TESTMODE" And origValue2 = "N" And txtValue = "Y" Then
    '                            strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where Parameter='" & origValue1 & "'"
    '                            daU.UpdateCommand = connLocal.CreateCommand
    '                            daU.UpdateCommand.CommandText = strU
    '                            daU.UpdateCommand.ExecuteNonQuery()
    '                            RetValue = DisplayMessage("Update finished. Since you changed the Test Mode parameter, the system must be restarted " & _
    '                           "so that the correct bills can be imported.", "System Restart", "S")
    '                            End
    '                        End If

    '                        If txtParameter = "WRITEVOTESTOALISTEST" And origValue2 = "N" And txtValue = "Y" Then
    '                            MsgBox("Votes can only be written to ALIS test if the test mode is on. You have to change test Mode to on first", MsgBoxStyle.Critical, msgText)
    '                            btnCancel_Click(btnCancel, Nothing)
    '                        End If
    '                    End If

    '                    '--cause 3
    '                    If gTestMode And gWriteVotesToTest = False Then
    '                        If txtParameter = "TESTMODE" And origValue2 = "Y" And txtValue = "N" Then
    '                            strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where Parameter='" & origValue1 & "'"
    '                            daU.UpdateCommand = connLocal.CreateCommand
    '                            daU.UpdateCommand.CommandText = strU
    '                            daU.UpdateCommand.ExecuteNonQuery()
    '                            RetValue = DisplayMessage("Update finished. Since you changed the Test Mode parameter, the system must be restarted " & _
    '                           "so that the correct bills can be imported.", "System Restart", "S")
    '                            End
    '                        End If
    '                        If txtParameter = "WRITEVOTESTOALISTEST" And origValue2 = "N" And txtValue = "Y" Then
    '                            strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where Parameter='" & origValue1 & "'"
    '                            daU.UpdateCommand = connLocal.CreateCommand
    '                            daU.UpdateCommand.CommandText = strU
    '                            daU.UpdateCommand.ExecuteNonQuery()
    '                            RetValue = DisplayMessage("Update finished. Since you changed the write votes parameter, the system must be restarted " & _
    '                           "so that the correct bills can be imported.", "System Restart", "S")
    '                            End
    '                        End If
    '                    End If

    '                    '--case 4 if case 1, case 2, case 3 checked unporperly, case 4 will happened.
    '                    If gTestMode = False And gWriteVotesToTest Then
    '                        If txtParameter = "TESTMODE" And origValue2 = "N" And txtValue = "Y" Then
    '                            strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where Parameter='" & origValue1 & "'"
    '                            daU.UpdateCommand = connLocal.CreateCommand
    '                            daU.UpdateCommand.CommandText = strU
    '                            daU.UpdateCommand.ExecuteNonQuery()
    '                            RetValue = DisplayMessage("Update finished. Since you changed the Test Mode parameter, the system must be restarted " & _
    '                           "so that the correct bills can be imported.", "System Restart", "S")
    '                            End
    '                        End If
    '                        If txtParameter = "WRITEVOTESTOALISTEST" And origValue2 = "Y" And txtValue = "N" Then
    '                            strU = "Update  tblVotingParameters Set Parameter='" & txtParameter & "', ParameterValue='" & UCase(txtValue) & "' Where Parameter='" & origValue1 & "'"
    '                            daU.UpdateCommand = connLocal.CreateCommand
    '                            daU.UpdateCommand.CommandText = strU
    '                            daU.UpdateCommand.ExecuteNonQuery()
    '                            RetValue = DisplayMessage("Update finished. Since you changed the write votes parameter, the system must be restarted " & _
    '                           "so that the correct bills can be imported.", "System Restart", "S")
    '                            End
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        Else
    '            btnCancel_Click(btnCancel, Nothing)
    '            MsgBox("Update cancelled", MsgBoxStyle.OkOnly, "Update Voting Parameter")
    '        End If
    '    Catch ex As Exception
    '        ERHHandler()
    '    End Try
    'End Su
#End Region
    
End Class