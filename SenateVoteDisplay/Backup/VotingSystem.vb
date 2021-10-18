Option Strict Off
Option Explicit On

Imports System
Imports System.Data.OleDb
Imports System.IO
Imports System.ComponentModel
Imports System.Net
Imports System.Net.Sockets
Imports Microsoft.VisualBasic.ApplicationServices

Module VotingSystem
    Public Function V_DataSet(ByVal strSql As String, ByVal flag As String) As DataSet
        Dim da As New OleDbDataAdapter
        Dim daSec As New OleDbDataAdapter
        Dim daDisplay As OleDbDataAdapter
        Dim command, commandSec, commandDisplay As OleDbCommand
        Dim ds As New DataSet
        Dim dsSec As New DataSet
        Dim dsDisplay As New DataSet
      
        v_Rows = 0

        If gOnlyOnePC = False Then
            If cnVoting.State = ConnectionState.Closed Then cnVoting.Open()
            If cnSecVoting.State = ConnectionState.Closed Then cnSecVoting.Open()
        Else
            If (SOOB1_On And SOOB2_On = False) Then
                If cnVoting.State = ConnectionState.Closed Then cnVoting.Open()
            ElseIf SOOB1_On = False And SOOB2_On Then
                If cnSecVoting.State = ConnectionState.Closed Then cnSecVoting.Open()
            End If

            If SDisplay_On Then
                If cnDisplay.State = ConnectionState.Closed Then cnDisplay.Open()
            End If
        End If

        Try
            If strSql <> "" Then
                If gOnlyOnePC = False Then
                    da = New OleDbDataAdapter(strSql, cnVoting)
                    daSec = New OleDbDataAdapter(strSql, cnSecVoting)
                Else
                    If SOOB1_On And SOOB2_On = False Then
                        da = New OleDbDataAdapter(strSql, cnVoting)
                    ElseIf SOOB1_On = False And SOOB2_On Then
                        daSec = New OleDbDataAdapter(strSql, cnSecVoting)
                    End If
                End If

                Select Case flag
                    Case "R"
                        If gOnlyOnePC = False Then
                            da.SelectCommand = New OleDbCommand(strSql, cnVoting)
                            da.Fill(ds, "Table")

                            V_DataSet = ds
                            Return ds
                        Else
                            If SOOB1_On And SOOB2_On = False Then
                                da.SelectCommand = New OleDbCommand(strSql, cnVoting)
                                da.Fill(ds, "Table")

                                V_DataSet = ds
                                Return ds
                            ElseIf SOOB1_On = False And SOOB2_On Then
                                daSec.SelectCommand = New OleDbCommand(strSql, cnSecVoting)
                                daSec.Fill(dsSec, "Table")

                                V_DataSet = dsSec
                                Return dsSec
                            End If
                        End If
                    Case "A"
                        If gOnlyOnePC = False Then
                            command = New OleDbCommand(strSql, cnVoting)
                            da.InsertCommand = command
                            da.Fill(ds, "Table")

                            commandSec = New OleDbCommand(strSql, cnSecVoting)
                            daSec.InsertCommand = commandSec
                            daSec.Fill(dsSec, "Table")

                            V_DataSet = ds
                            Return ds
                        Else
                            If DisplayImage Then
                                daDisplay = New OleDbDataAdapter(strSql, gDisplay_Conn)
                                commandDisplay = New OleDbCommand(strSql, cnDisplay)
                                daDisplay.InsertCommand = commandDisplay
                                daDisplay.Fill(dsDisplay, "DisplayImage")
                            End If

                            If SOOB1_On And SOOB2_On = False Then
                                command = New OleDbCommand(strSql, cnVoting)
                                da.InsertCommand = command
                                da.Fill(ds, "Table")

                                V_DataSet = ds
                                Return ds
                            ElseIf SOOB1_On = False And SOOB2_On Then
                                commandSec = New OleDbCommand(strSql, cnSecVoting)
                                daSec.InsertCommand = commandSec
                                daSec.Fill(dsSec, "Table")

                                V_DataSet = dsSec
                                Return dsSec
                            End If
                        End If
                    Case "U"
                        If gOnlyOnePC = False Then
                            command = New OleDbCommand(strSql, cnVoting)
                            da.UpdateCommand = command
                            command.ExecuteNonQuery()

                            commandSec = New OleDbCommand(strSql, cnSecVoting)
                            daSec.UpdateCommand = commandSec
                            daSec.UpdateCommand = commandSec
                            commandSec.ExecuteNonQuery()

                            V_DataSet = ds
                            Return ds
                        Else
                            If DisplayImage Then
                                daDisplay = New OleDbDataAdapter(strSql, gDisplay_Conn)
                                commandDisplay = New OleDbCommand(strSql, cnDisplay)
                                daDisplay.UpdateCommand = commandDisplay
                                daDisplay.Fill(ds, "DisplayImage")
                                commandDisplay.ExecuteNonQuery()
                            End If

                            If SOOB1_On And SOOB2_On = False Then
                                command = New OleDbCommand(strSql, cnVoting)
                                da.UpdateCommand = command
                                da.Fill(ds, "Table")
                                command.ExecuteNonQuery()

                                V_DataSet = ds
                                Return ds
                            ElseIf SOOB1_On = False And SOOB2_On Then
                                commandSec = New OleDbCommand(strSql, cnSecVoting)
                                daSec.UpdateCommand = commandSec
                                daSec.Fill(dsSec, "Table")
                                commandSec.ExecuteNonQuery()

                                V_DataSet = dsSec
                                Return dsSec
                            End If
                        End If
                    Case "D"
                        If gOnlyOnePC = False Then
                            command = New OleDbCommand(strSql, cnVoting)
                            da.DeleteCommand = command
                            da.DeleteCommand.ExecuteNonQuery()

                            commandSec = New OleDbCommand(strSql, cnSecVoting)
                            daSec.DeleteCommand = commandSec
                            daSec.DeleteCommand.ExecuteNonQuery()

                            V_DataSet = ds
                            Return ds
                        Else
                            If DisplayImage Then
                                daDisplay = New OleDbDataAdapter(strSql, gDisplay_Conn)
                                commandDisplay = New OleDbCommand(strSql, cnDisplay)
                                daDisplay.DeleteCommand = commandDisplay
                                daDisplay.DeleteCommand.ExecuteNonQuery()
                            End If
                            If SOOB1_On And SOOB2_On = False Then
                                command = New OleDbCommand(strSql, cnVoting)
                                da.DeleteCommand = command
                                da.DeleteCommand.ExecuteNonQuery()

                                V_DataSet = ds
                                Return ds
                            ElseIf SOOB1_On = False And SOOB2_On Then
                                commandSec = New OleDbCommand(strSql, cnSecVoting)
                                daSec.DeleteCommand = commandSec
                                daSec.DeleteCommand.ExecuteNonQuery()
                                V_DataSet = dsSec
                                Return dsSec
                            End If
                        End If
                            dataProcess = True
                End Select
            End If
        Catch ex As OleDb.OleDbException
            dataProcess = False
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Voting System")
        Finally
            If SOOB1_On And SOOB2_On = False Then
                cnVoting.Close()
            ElseIf SOOB1_On = False And SOOB2_On Then
                cnSecVoting.Close()
            End If
            If DisplayImage Then
                cnDisplay.Close()
            End If
        End Try
    End Function
End Module
