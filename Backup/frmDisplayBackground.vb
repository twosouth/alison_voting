Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.IO
Imports System.Data.OleDb
Imports System.ComponentModel
Imports System.Net
Imports System.Net.Sockets
Imports System.Messaging
Imports System.Runtime.Serialization

Public Class frmDisplayBackGround
    Private sqlQuery As String
    Private Connection As OleDb.OleDbConnection
    Private Command As OleDbCommand
    Private Adapter As OleDbDataAdapter
    Private Builder As OleDbCommandBuilder
    Private ds, tempDataSet As DataSet
    Private userTable As DataTable
    Private dc As New DataColumn
    Private currentIndex, i As Integer
    Private isLastPage, bUpdate As Boolean
    Private totalRecords, startRecord, endRecord As Integer
    Private currentPageStartRecord, currentPageEndRecord As Integer
    Private Retvalre As New Object
    Private origValue1, origValue2, fPath, fName As String

    Private Sub frmDisplayBackGround_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dc As New DataColumn()

        Try
            userDataGridView.DataSource = Nothing
            userDataGridView.Rows.Clear()
            userDataGridView.Refresh()
            sqlQuery = "Select [File Name], OID FROM tblDisplayBoradImages"
            SetDataObjects()
            Connection.Open()

            Adapter.Fill(tempDataSet)
            totalRecords = tempDataSet.Tables(0).Rows.Count
            tempDataSet.Clear()
            tempDataSet.Dispose()
            Adapter.Fill(tempDataSet, 0, totalRecords, "tblDisplayBoradImages")
            userTable = New DataTable("tblDisplayBoradImages")

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
            startRecord = 0
        End If

        endRecord = startRecord + noOfRecords
        If (endRecord >= totalRecords) Then
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
                Adapter.Fill(ds, startRecord, noOfRecords, "tblDisplayBoradImages")
                userTable = ds.Tables("tblDisplayBoradImages")
            End If

         
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Load Motion List")
        Finally
            Connection.Close()
        End Try

        userDataGridView.DataSource = userTable.DefaultView
        userDataGridView.AllowUserToResizeColumns = True
        userDataGridView.Columns(0).Width = 238
        userDataGridView.Columns(1).Width = 2
    End Sub


    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            ds = V_DataSet("Select * From tblDisplayBoradImages Where [File Name]='" & userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value & "'", "R")
            If ds.Tables(0).Rows.Count > 0 Then
                Retvalre = DisplayMessage("This file already exists. Please try again.", "DuplicateNameException Code", "S")
                userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value = ""
                userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value = ""
                Exit Sub
            End If

            If MsgBox("Before add new file, make sure you have the new PowerPoint file created and saved to Display PC to right location.", MsgBoxStyle.YesNo, "Add File") = MsgBoxResult.Yes Then
                ' Copy New PowerPoint file to Display PC.
                If File.Exists(gPowerPointFilePath & fName) Then
                    File.Copy(gPowerPointFilePath & fName, "Z:\" & fName)
                End If

                DisplayImage = True
                V_DataSet("Insert Into tblDisplayBoradImages ( [File Name]) VALUES ('" & userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value & "')", "A")
                If dataProcess = False Then
                    frmDisplayBackGround_Load(sender, e)
                End If
                userDataGridView.ReadOnly = False
            End If

            origValue1 = ""
            origValue2 = ""
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Add New Background File")
            frmDisplayBackGround_Load(sender, e)
        End Try
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MessageBox.Show("Do you really want to delete selected record(s)?", "Delete Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, 0, False) = DialogResult.Yes Then
            Try
                DisplayImage = True
                V_DataSet("Delete from tblDisplayBoradImages Where [File Name]='" & userDataGridView.CurrentRow.Cells(0).Value & "'", "D")
                If dataProcess = False Then
                    frmDisplayBackGround_Load(sender, e)
                End If
                origValue1 = ""
                origValue2 = ""
                Me.Close()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Delete The Background File")
                frmDisplayBackGround_Load(sender, e)
            Finally
                Connection.Close()
            End Try
        End If
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

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click
        '--- send voting information to display PC 
        Dim bodyParamters As String = ""
        ' bodyParamters = "gCalendar - " & Me.cboCalendar.Text & "||" & "gBill - " & Me.cboBill.Text & "||" & "gCurrentPhrase - " & cboPhrase.Text
        If SDisplay_On Then
            DisplayImage = True
            SendMessageToDisplay(UCase(userDataGridView.CurrentCell.Value), bodyParamters, gSendQueueToDisplay)
            'SendMessageToDisplay(DisplayTitle(cboDisplay.Text), bodyParamters, gSendQueueToDisplay)
        Else
            DisplayImage = False
            MsgBox("Senate voting display borad computer is off. Unable to change the background.", MsgBoxStyle.Critical, msgText)
        End If
    End Sub

    Private Sub userDataGridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles userDataGridView.CellEndEdit
        fName = userDataGridView.CurrentCell.Value
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
                '  MsgBox("Resend voting data has finished for display request.", MsgBoxStyle.Information, "Senate Voting System")
            Catch ex As Exception
                Throw New Exception("Error Getting Quequ")
            End Try
            mq.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Voting System")
        End Try

    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim daU As New OleDbDataAdapter
        Dim strU, txtFileName, txtValue As String

        cnVoting = New OleDbConnection(strVoting)
        cnVoting.Open()
        txtFileName = userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value
        txtValue = userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value

        If MsgBox("Before update the file name, make sure you have it has created and saved to Display PC to right location.", MsgBoxStyle.YesNo, "Update File") = MsgBoxResult.Yes Then
            strU = "Update  tblDisplayBoradImages Set [File Name]='" & txtFileName & "' Where [File Name]='" & origValue1 & "'"
            daU.UpdateCommand = cnVoting.CreateCommand
            daU.UpdateCommand.CommandText = strU
            daU.UpdateCommand.ExecuteNonQuery()
            origValue1 = ""

            '--- Copy Updated PowerPoint file to Display PC.
            If File.Exists(gPowerPointFilePath & txtFileName) Then
                File.Copy(gPowerPointFilePath & txtFileName, "Z:\" & txtFileName)
            End If
        End If
    End Sub
End Class