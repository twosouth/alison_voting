Imports System
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Data.OleDb

Public Class frmSenatorList
    Private frmOpen As Boolean

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        'CDWindowOpen()
        Me.Close()
        'If UCase(System.Environment.MachineName) = UCase(SVOTE) Then
        '    ChangeSize(frmVote)
        'ElseIf UCase(System.Environment.MachineName) = UCase(SDIS) Then
        '    ChangeSize(frmChamberDisplay)
        'End If
    End Sub

    'Private Sub frmSenatorList_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    '    Dim sw As New Stopwatch

    '    CDWindowOpen()
    '    If UCase(System.Environment.MachineName) = UCase(SVOTE) Then
    '        e.Cancel = True
    '        Me.Hide()

    '        sw.Start()
    '        For i As Int16 = 0 To My.Application.OpenForms.Count - 1
    '            If My.Application.OpenForms.Item(i).Text = "Senate Voting System - Senate Vote Window" Then
    '                frmOpen = True
    '                Exit For
    '            Else
    '                frmOpen = False
    '            End If
    '        Next
    '        sw.Stop()

    '        ChangeSize(frmVote)
    '    ElseIf UCase(System.Environment.MachineName) = UCase(SDIS) Then
    '        e.Cancel = True
    '        Me.Hide()
    '        ChangeSize(frmChamberDisplay)

    '        sw.Start()
    '        For i As Int16 = 0 To My.Application.OpenForms.Count - 1
    '            If My.Application.OpenForms.Item(i).Text = "Senate Voting System - Order Of Business" Then
    '                frmOpen = True
    '                Exit For
    '            Else
    '                frmOpen = False
    '            End If
    '        Next
    '        sw.Stop()

    '    End If
    'End Sub

    Private Sub frmSenatorList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cn As OleDbConnection
        Dim command As OleDbCommand
        Dim da As OleDbDataAdapter
        Dim Builder As New OleDbCommandBuilder
        Dim ds As DataSet
        Dim dt As New DataTable("Senators")
        Dim dc As New DataColumn
        cn = New OleDbConnection(strLocal)

        Try
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

            ds.Dispose()
            da.Dispose()
            cn.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Senators List")
        Finally
            cn.Close()
        End Try
    End Sub

    Private Sub Salutation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Salutation.SelectedIndexChanged
        Try
            Dim i As Integer = 0
            '--- assign Salutation to  selected multiple senators row
            For Each dr As DataGridViewRow In DGViewSenators.Rows
                If dr.Selected = True Then
                    If UCase(DGViewSenators.Item(1, i).Value) <> Nothing Then
                        V_DataSet("Update tblSenators Set Salutation ='" & Me.Salutation.Text & "' WHERE ucase(SenatorName) ='" & UCase(DGViewSenators.Item(1, i).Value) & "'", "U")
                        DGViewSenators.Item(0, i).Value = Salutation.Text
                    End If
                End If
                i += 1
            Next

            loadSenatorsDon = True
            LoadSenatorsIntoArray()
            loadSenatorsDon = False
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Salutation Of Senators List")
        End Try
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

    Private Sub btnAll_Click(sender As System.Object, e As System.EventArgs) Handles btnAll.Click
        For i As Integer = 0 To Me.DGViewSenators.RowCount - 1
            Me.DGViewSenators.Item(0, i).Selected = True
        Next
    End Sub
End Class