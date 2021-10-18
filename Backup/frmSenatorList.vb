Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports System.Data.Common
Imports System.Data.OleDb

Public Class frmSenatorList

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
        'frmStart.Show()
    End Sub

    Private Sub frmSenatorList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cn As OleDbConnection
        Dim command As OleDbCommand
        Dim da As OleDbDataAdapter
        Dim Builder As New OleDbCommandBuilder
        Dim ds As DataSet
        Dim dt As New DataTable("Senators")
        Dim dc As New DataColumn

        Try
            cn = New OleDbConnection(strVoting)
            command = New OleDbCommand("Select Salutation, SenatorName as [Senator Name] FROM tblSenators ORDER BY SenatorName", cn)
            da = New OleDbDataAdapter(command)
            Builder = New OleDbCommandBuilder(da)
            ds = New DataSet("Senators")
            dt.Rows.Clear()
            cn.Open()
            da.Fill(ds, "Senators")
            dt = ds.Tables("Senators")

            DGViewSenators.DataSource = Nothing
            DGViewSenators.Rows.Clear()
            DGViewSenators.Refresh()

            For Each dc In dt.Columns
                Dim cloumn As DataGridViewTextBoxColumn = New DataGridViewTextBoxColumn
                cloumn.DataPropertyName = dc.ColumnName
                cloumn.HeaderText = dc.ColumnName
                cloumn.Name = dc.ColumnName
                cloumn.SortMode = DataGridViewColumnSortMode.Automatic
                cloumn.ValueType = dc.DataType
                DGViewSenators.Columns.Add(cloumn)
            Next

            If ds.Tables(0).Rows.Count <> 0 Then
                DGViewSenators.DataSource = dt.DefaultView
                DGViewSenators.Columns(0).Width = 100
                DGViewSenators.Columns(1).Width = 150
            End If


            cn.Close()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Voting System")
        End Try
    End Sub

    Private Sub Salutation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Salutation.SelectedIndexChanged
        ' Update database tblSenator table and cell
        V_DataSet("Update tblSenators Set Salutation ='" & Me.Salutation.Text & "' WHERE SenatorName='" & DGViewSenators.Item(1, DGViewSenators.CurrentRow.Index).Value & "'", "U")
        DGViewSenators.Item(0, DGViewSenators.CurrentRow.Index).Value = Salutation.Text
    End Sub
End Class