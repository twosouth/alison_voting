Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports System.Data.Common
Imports System.Data.OleDb

Public Class frmBoardAndCommissionListNew
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
    Private currentPageStartRecord, currentPageEndRecord As Integer
    Private origValue1, origValue2 As String
    Private Retvalre As New Object


    Private Sub frmBoardAndCommissionListNew_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Dim dc As New DataColumn()
        Try
            userDataGridView.DataSource = Nothing
            userDataGridView.Rows.Clear()
            userDataGridView.Refresh()
            sqlQuery = "Select ALISName as [ALIS Name], SenateVotingName as [Senate Voting Name] FROM tblBoardsAndCommissions ORDER BY ALISName"
            SetDataObjects()
            Connection.Open()

            Adapter.Fill(tempDataSet)
            totalRecords = tempDataSet.Tables(0).Rows.Count
            tempDataSet.Clear()
            tempDataSet.Dispose()
            Adapter.Fill(tempDataSet, 0, totalRecords, "tblBoardsAndCommissions")
            userTable = New DataTable("tblBoardsAndCommissions")

            For Each Me.dc In userTable.Columns
                Dim column As DataGridViewTextBoxColumn = New DataGridViewTextBoxColumn()
                column.DataPropertyName = dc.ColumnName
                column.HeaderText = dc.ColumnName
                column.Name = dc.ColumnName
                column.SortMode = DataGridViewColumnSortMode.Automatic
                column.ValueType = dc.DataType
                userDataGridView.Columns.Add(column)
            Next

            CreateTempTable(0, 0)

        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        Finally
            Connection.Close()
        End Try
    End Sub

    Private Sub SetDataObjects()
        Connection = New OleDbConnection(strVoting)
        Command = New OleDbCommand(sqlQuery, Connection)
        Adapter = New OleDbDataAdapter(Command)
        Builder = New OleDbCommandBuilder(Adapter)
        ds = New DataSet("MainDataSet")
        tempDataSet = New DataSet("TempDataSet")
    End Sub

    Private Sub CreateTempTable(ByVal startRecord As Integer, ByVal noOfRecords As Integer)
        Dim endRecord As Integer

        If (startRecord = 0 Or startRecord < 0) Then
            btnPrevious.Enabled = False
            startRecord = 0
        End If

        endRecord = startRecord + noOfRecords

        If (endRecord >= totalRecords) Then
            btnNext.Enabled = False
            isLastPage = True
            endRecord = totalRecords
        End If

        currentPageStartRecord = startRecord
        currentPageEndRecord = endRecord
        currentIndex = endRecord

        Try
            userTable.Rows.Clear()
            If (Connection.State = ConnectionState.Closed) Then
                Connection.Open()
            End If

            If startRecord >= 0 Then
                Adapter.Fill(ds, startRecord, noOfRecords, "tblBoardsAndCommissions")
                userTable = ds.Tables("tblBoardsAndCommissions")
            End If

            lblPageNums.Text = "Records from " & startRecord & " to " & endRecord & " of " & totalRecords

        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        Finally
            Connection.Close()
        End Try

        userDataGridView.DataSource = userTable.DefaultView
        userDataGridView.AllowUserToResizeColumns = True
        userDataGridView.Columns(0).Width = 330
        userDataGridView.Columns(1).Width = 448
    End Sub

    Private Sub btnPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrevious.Click
        If (isLastPage) Then
            Dim noc As Integer = FindLastPageRecords()
            CreateTempTable(totalRecords - noc - 16, 16)
        Else
            CreateTempTable(CInt(currentIndex - 2 * 16), 16)
        End If
        btnNext.Enabled = True
        btnLast.Enabled = True
        isLastPage = False
    End Sub

    Private Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
        CreateTempTable(currentIndex, 16)
        btnPrevious.Enabled = True
    End Sub

    Private Sub btnLast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLast.Click
        Dim totPages As Integer = CInt(totalRecords / 16)
        Dim remainingRecs As Integer = FindLastPageRecords()

        CreateTempTable(totalRecords - remainingRecs, 16)
        btnPrevious.Enabled = True
        btnNext.Enabled = False
        isLastPage = True
    End Sub

    Private Function FindLastPageRecords() As Integer
        Return (totalRecords / CInt(16))
    End Function

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        userDataGridView.ReadOnly = False

        Try
            ds = V_DataSet("Select * FROM tblBoardsAndCommissions Where AlisName='" & userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value & "'", "S")
            If ds.Tables(0).Rows.Count > 0 Then
                Retvalre = DisplayMessage("This code already exists. Please try again.", "DuplicateNameException Code", "S")
                userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value = ""
                userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value = ""
                Exit Sub
            End If

            Connection.Open()
            Adapter.Update(userTable)
            userDataGridView.ReadOnly = False

            lblPageNums.Text = "Records from " & startRecord & " to " & endRecord & " of " & totalRecords + 1

            origValue1 = ""
            origValue2 = ""
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
            btnCancel.Enabled = True
        Finally
            btnAdd.Enabled = True
            Connection.Close()
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MessageBox.Show("Do you really want to delete selected record(s)?", "Delete Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, 0, False) = DialogResult.Yes Then
            Try
                V_DataSet("Delete from tblBoardsAndCommissions Where AlisName='" & userDataGridView.CurrentRow.Cells(0).Value & "'", "D")
                lblPageNums.Text = "Records from " & startRecord & " to " & endRecord & " of " & totalRecords - 1
                origValue1 = ""
                origValue2 = ""
                Me.Close()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Delete Board and Committee")
            Finally
                Connection.Close()
            End Try
        End If
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        CreateTempTable(0, 16)
        btnPrevious.Enabled = False
        btnNext.Enabled = True
        btnLast.Enabled = True
        isLastPage = False
    End Sub

    Private Sub lblBoardAndCommissionList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        btnLast.Enabled = True
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub userDataGridView_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles userDataGridView.CellBeginEdit
        Static j As Integer = 1
        If (Not IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Or Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value)) And j = 1 Then
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
            j += 1
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Connection.Open()
        CreateTempTable(0, 0)
        Connection.Close()
        origValue1 = ""
        origValue2 = ""
    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        userDataGridView.ReadOnly = False

        Try
            Connection.Open()
            Adapter.Update(userTable)
            userDataGridView.ReadOnly = False
            origValue1 = ""
            origValue2 = ""
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Update Board and Committee")
        Finally
            Connection.Close()
        End Try
    End Sub

End Class

