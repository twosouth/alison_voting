Option Strict Off
Option Explicit On

Imports System
Imports System.Data.OleDb
Imports System.IO
Imports System.ComponentModel
Imports System.Net

Module VotingSystem
    Public Function V_DataSet(ByVal strSql As String, ByVal flag As String) As DataSet
        Dim da As New OleDbDataAdapter
        Dim daSec As New OleDbDataAdapter
        Dim command As OleDbCommand
        Dim ds As New DataSet
        Dim dsSec As New DataSet
        Dim dsDisplay As New DataSet

        v_Rows = 0

        If connLocal.State = ConnectionState.Closed Then connLocal.Open()

        Try
            If strSql <> "" Then
                da = New OleDbDataAdapter(strSql, connLocal)

                Select Case flag
                    Case "R"
                        da.SelectCommand = New OleDbCommand(strSql, connLocal)
                        da.Fill(ds, "Table")

                        V_DataSet = ds
                        dataProcess = True
                        Return ds
                    Case "A"
                        command = New OleDbCommand(strSql, connLocal)
                        da.InsertCommand = command
                        da.Fill(ds, "Table")
                        dataProcess = True
                    Case "U"
                        command = New OleDbCommand(strSql, connLocal)
                        da.UpdateCommand = command
                        da.Fill(ds, "Table")
                        command.ExecuteNonQuery()

                        V_DataSet = ds
                        dataProcess = True
                        Return ds
                    Case "D"
                        command = New OleDbCommand(strSql, connLocal)
                        da.DeleteCommand = command
                        da.DeleteCommand.ExecuteNonQuery()

                        V_DataSet = ds
                        Return ds
                        dataProcess = True
                    Case "DROP"
                        command = New OleDbCommand(strSql, connLocal)
                        command.ExecuteNonQuery()
                        dataProcess = True
                End Select
            End If
            connLocal.Close()
        Catch ex As OleDb.OleDbException
            dataProcess = False
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Voting System")
        Finally
            da.Dispose()
            ds.Dispose()
            connLocal.Close()
            If DisplayImage Then
                cnOOB.Close()
            End If
        End Try
    End Function

    Public Function ShowBillsByCalendar(calendar As String) As DataSet
        Dim da As New OleDbDataAdapter
        Dim ds As New DataSet
        Dim strsql As String = "Select tblBills.BillNbr, tblBills.Bill FROM tblBills, tblCalendars " & _
                    " WHERE tblBills.CalendarCode = tblCalendars.CalendarCode AND " & _
                    " tblCalendars.Calendar ='" & calendar & "' ORDER BY tblBills.BillNbr"

        v_Rows = 0

        If gOnlyOnePC = False Then
            If connLocal.State = ConnectionState.Closed Then connLocal.Open()
        End If

        Try
            da = New OleDbDataAdapter(strsql, connLocal)
            da.SelectCommand = New OleDbCommand(strsql, connLocal)
            da.Fill(ds, "Table")

            ShowBillsByCalendar = ds
            Return ds

            dataProcess = True

            ds.Dispose()
            da.Dispose()
            connLocal.Close()
        Catch ex As OleDb.OleDbException
            ds.Dispose()
            da.Dispose()
            connLocal.Close()
            dataProcess = False
            MsgBox(ex.Message, MsgBoxStyle.Critical, msgText)
            Return Nothing
        End Try
    End Function

    Public Sub UpdateParameter(value As String, operate As String)
        Try
            If UCase(operate) = "CREATEHTMLPAGE" Then
                strSQL = "Update tblVotingParameters SET ParameterValue = 'Y' Where ucase(Parameter) ='CREATEHEMLPAGE'"
                V_DataSet(strSQL, "U")
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, msgText)
        End Try
    End Sub
End Module
