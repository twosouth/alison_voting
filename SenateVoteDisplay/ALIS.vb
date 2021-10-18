Option Strict Off
Option Explicit On

Imports System
Imports System.Data.OleDb
Imports System.IO
Imports System.Timers
Imports System.Windows.Forms
Imports System.ComponentModel

Module ALIS

    Public Function ALIS_DataSet(ByVal strSql As String, ByVal flag As String) As DataSet
        Dim thisTransaction As OleDbTransaction = Nothing
        Dim da As New OleDbDataAdapter
        Dim ds As New DataSet

        a_Rows = 0

        Try
            If strSql <> "" Then
                '--- start data trancaction
                If cnALIS.State = ConnectionState.Closed Then
                    cnALIS.Open()
                End If
                da = New OleDbDataAdapter(strSql, cnALIS)
                Select Case flag
                    Case "R"
                        da.SelectCommand = New OleDbCommand(strSql, cnALIS)
                        da.Fill(ds, "Table")
                        ' da.Update(ds)

                        a_Rows = ds.Tables(0).Rows.Count
                        ALIS_DataSet = ds
                    Case "A"
                        If gTestMode = False Then
                            thisTransaction = cnALIS.BeginTransaction
                            da.InsertCommand = New OleDbCommand(strSql, cnALIS)
                            da.InsertCommand.Transaction = thisTransaction
                            thisTransaction.Commit()
                        Else
                            da.InsertCommand = New OleDbCommand(strSql, cnALIS)
                        End If
                        da.Fill(ds, "Table")
                        ' da.Update(ds)
                        ALIS_DataSet = ds
                    Case "U"
                        If gTestMode = False Then
                            thisTransaction = cnALIS.BeginTransaction
                            da.UpdateCommand = New OleDbCommand(strSql, cnALIS)
                            da.UpdateCommand.Transaction = thisTransaction
                            thisTransaction.Commit()
                        Else
                            da.UpdateCommand = New OleDbCommand(strSql, cnALIS)
                        End If
                        da.Fill(ds, "Table")
                        ' da.Update(ds)
                        ALIS_DataSet = ds
                    Case "D"
                        If gTestMode = False Then
                            thisTransaction = cnALIS.BeginTransaction
                            da.DeleteCommand = New OleDbCommand(strSql, cnALIS)
                            da.DeleteCommand.Transaction = thisTransaction
                            thisTransaction.Commit()
                        Else
                            da.DeleteCommand = New OleDbCommand(strSql, cnALIS)
                        End If
                        da.Fill(ds, "Table")
                        '  da.Update(ds)
                        ALIS_DataSet = ds
                End Select
            End If
            cnALIS.Close()

        Catch ex As OleDbException
            '--- if error occurred RollBack data transaction
            If gTestMode = False Then
                thisTransaction.Rollback()
            End If
            DisplayMessage(ex.Message & "An error occurred while sending the vote to ALIS. If the connection to ALIS is down, you should exit the system " & _
            "and then restart it again. If the connction is OK, this vote will be sent to ALIS when the next one is sent.", "Vote Not Sent To ALIS", "S")
        Finally
            '  a_Rows = ds.Tables(0).Rows.Count
            ' ALIS_DataSet = ds

            ds.Dispose()
            cnALIS.Close()
        End Try
    End Function

    'Public Function ALIS_DataSet(ByVal strSql As String, ByVal flag As String) As DataSet
    '    Dim thisTransaction As OleDbTransaction = Nothing
    '    Dim da As New OleDbDataAdapter
    '    Dim ds As New DataSet

    '    a_Rows = 0

    '    Try
    '        If strSql <> "" Then
    '            '--- start data trancaction

    '            da = New OleDbDataAdapter(strSql, cnALIS)
    '            Select Case flag
    '                Case "R"
    '                    da.SelectCommand = New OleDbCommand(strSql, cnALIS)
    '                    da.Fill(ds, "Table")
    '                    da.Update(ds)

    '                    a_Rows = ds.Tables(0).Rows.Count
    '                    ALIS_DataSet = ds
    '                Case "A"
    '                    If gTestMode = False Then
    '                        thisTransaction = cnALIS.BeginTransaction
    '                        da.InsertCommand = New OleDbCommand(strSql, cnALIS)
    '                        da.InsertCommand.Transaction = thisTransaction
    '                        thisTransaction.Commit()
    '                    Else
    '                        da.InsertCommand = New OleDbCommand(strSql, cnALIS)
    '                    End If
    '                    da.Fill(ds, "Table")
    '                    da.Update(ds)
    '                    ALIS_DataSet = ds
    '                Case "U"
    '                    If gTestMode = False Then
    '                        thisTransaction = cnALIS.BeginTransaction
    '                        da.UpdateCommand = New OleDbCommand(strSql, cnALIS)
    '                        da.UpdateCommand.Transaction = thisTransaction
    '                        thisTransaction.Commit()
    '                    Else
    '                        da.UpdateCommand = New OleDbCommand(strSql, cnALIS)
    '                    End If
    '                    da.Fill(ds, "Table")
    '                    da.Update(ds)
    '                    ALIS_DataSet = ds
    '                Case "D"
    '                    If gTestMode = False Then
    '                        thisTransaction = cnALIS.BeginTransaction
    '                        da.DeleteCommand = New OleDbCommand(strSql, cnALIS)
    '                        da.DeleteCommand.Transaction = thisTransaction
    '                        thisTransaction.Commit()
    '                    Else
    '                        da.DeleteCommand = New OleDbCommand(strSql, cnALIS)
    '                    End If
    '                    da.Fill(ds, "Table")
    '                    da.Update(ds)
    '                    ALIS_DataSet = ds
    '            End Select

    '        End If
    '        cnALIS.Close()
    '    Catch ex As OleDbException
    '        '--- if error occurred RollBack data transaction
    '        If gTestMode = False Then
    '            thisTransaction.Rollback()
    '        End If
    '        ' DisplayMessage("An error occurred while sending the vote to ALIS. If the connection to ALIS is down, you should exit the system " & _
    '        ' "and then restart it again. If the connction is OK, this vote will be sent to ALIS when the next one is sent.", "Vote Not Sent To ALIS", "S")
    '    Finally
    '        ds.Dispose()
    '        cnALIS.Close()
    '    End Try
    'End Function
End Module
