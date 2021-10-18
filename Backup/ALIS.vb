Option Strict Off
Option Explicit On

Imports System
Imports System.Data.OleDb
Imports System.IO
Imports System.Timers
Imports System.Windows.Forms
Imports System.ComponentModel

Module ALIS
    Public Function A_DataSet(ByVal strSql As String, ByVal flag As String) As DataSet
        Dim thisTransaction As OleDbTransaction = Nothing
        Dim da As New OleDbDataAdapter
        Dim ds As New DataSet
        a_Rows = 0

        If cnALIS.State = ConnectionState.Closed Then cnALIS.Open()

        Try
            If strSql <> "" Then
                da = New OleDbDataAdapter(strSql, cnALIS)
                Select Case flag
                    Case "R"
                        da.SelectCommand = New OleDbCommand(strSql, cnALIS)
                    Case "A"
                        thisTransaction = cnALIS.BeginTransaction
                        da.InsertCommand = New OleDbCommand(strSql, cnALIS)
                        da.InsertCommand.Transaction = thisTransaction
                        thisTransaction.Commit()
                    Case "U"
                        thisTransaction = cnALIS.BeginTransaction
                        da.UpdateCommand = New OleDbCommand(strSql, cnALIS)
                        da.UpdateCommand.Transaction = thisTransaction
                        thisTransaction.Commit()
                    Case "D"
                        thisTransaction = cnALIS.BeginTransaction
                        da.DeleteCommand = New OleDbCommand(strSql, cnALIS)
                        da.DeleteCommand.Transaction = thisTransaction
                        thisTransaction.Commit()
                End Select

                da.Fill(ds, "Table")
                a_Rows = ds.Tables(0).Rows.Count
                'gNbrSenators = v_Rows
                da.Update(ds)

                A_DataSet = ds
            End If
            cnALIS.Close()
        Catch ex As OleDbException
            thisTransaction.Rollback()
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Voting System")
        Finally
            cnALIS.Close()
        End Try
    End Function
End Module
