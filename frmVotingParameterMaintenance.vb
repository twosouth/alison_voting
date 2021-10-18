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
    Private origValue1, origValue2 As String
    Private WriteVotesToTestStart, OK As Boolean
    Private mcnSample As OleDb.OleDbConnection

    Private Sub frmVotingParameterMaintenance_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        WriteVotesToTestStart = gWriteVotesToTest
    End Sub

    'Private Sub frmVotingParameterMaintenance_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    '    CDWindowOpen()
    '    If UCase(System.Environment.MachineName) = UCase(SVOTE) Then
    '        e.Cancel = True
    '        Me.Hide()
    '        ChangeSize(frmVote)
    '    ElseIf UCase(System.Environment.MachineName) = UCase(SDIS) Then
    '        e.Cancel = True
    '        Me.Hide()
    '        ChangeSize(frmChamberDisplay)
    '    End If
    'End Sub

    Private Sub frmVotingParameterMaintenanceNew_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CDWindowOpen()
        Me.openConnection()
        loadPage()

        'For i As Integer = 0 To userDataGridView.RowCount - 1
        '    With userDataGridView
        '        .Rows(i).Cells(1)
        '    End With
        'Next
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

    Private Sub userDataGridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles userDataGridView.CellEndEdit
        Dim ds, dsC As New DataSet
        Dim i, k As Integer

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
                    loadPage()
                Else
                    dsC = V_DataSet("Select * From tblVotingParameters Where Parameter ='" & origValue1 & "'", "R")
                    If dsC.Tables(0).Rows.Count = 0 Then
                        '--- add new 
                        V_DataSet("Insert Into tblVotingParameters values ('" & parameterName & "', '" & parameterValue & "')", "A")
                        loadPage()
                    Else
                        For i = 0 To userDataGridView.Rows.Count - 1
                            If userDataGridView.Rows(i).Cells(0).Value = parameterName Then
                                k = userDataGridView.Rows(i).Index
                                MsgBox(parameterName & " name already exist.", vbInformation, "Senate Voting System Parameters Maintenance")
                                loadPage()
                                userDataGridView.Rows(k).Selected = True

                                Exit For
                            End If
                        Next
                    End If
                End If
                userDataGridView.CurrentCell = userDataGridView.Rows(0).Cells(0)
                Get_Parameters()
                ds.Dispose()
                dsC.Dispose()
            Else
                ds.Dispose()
                dsC.Dispose()
                ' MsgBox("Please enter parameter value!", MsgBoxStyle.Critical, "Add or Update Parameter")
                Exit Sub
            End If
        Catch ex As Exception
            ds.Dispose()
            dsC.Dispose()
            '  MsgBox(ex.Message, MsgBoxStyle.Critical, "Add New or Update Voting System Parameter")
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        loadPage()
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

            '--- Populate Data Grid
            Me.userDataGridView.DataSource = ds.Tables("tblVotingParameters").DefaultView

            With userDataGridView
                .Columns(0).Width = 250
                .Columns(0).HeaderText = "Paramete Name"
                ' .Columns(1).Width = 650
                .Columns(1).HeaderText = "Parameter Value"
            End With
            cmd.Dispose()
            da.Dispose()
            ds.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Vote System Parameters")
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'CDWindowOpen()
        Me.Close()
        'If UCase(System.Environment.MachineName) = UCase(SVOTE) Then
        '    ChangeSize(frmVote)
        'ElseIf UCase(System.Environment.MachineName) = UCase(SDIS) Then
        '    ChangeSize(frmChamberDisplay)
        'End If
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