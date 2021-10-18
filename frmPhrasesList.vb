Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class frmPhrasesList
    Private origValue1, origValue2 As String

    Declare Function WriteProfileString Lib "kernel32" _
             Alias "WriteProfileStringA" _
             (ByVal lpszSection As String, _
             ByVal lpszKeyName As String, _
             ByVal lpszString As String) As Long

    Private Sub frmPhrasesList_Initi()
        InitializeComponent()
    End Sub

    'Private Sub frmPhrasesList_FormClosing1(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    '    CDWindowOpen()
    '    e.Cancel = True
    '    Me.Hide()
    '    ChangeSize(frmChamberDisplay)
    'End Sub

    Private Sub frmPhrasesList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CDWindowOpen()
        Me.MdiParent = frmMain
        loadPage()
    End Sub

    Private Sub loadPage()
        Dim ds As New DataSet

        Try
            '--- Select only the n records.
            strSQL = "SELECT  * FROM tblPhrases Order By code"
            ds = V_DataSet(strSQL, "R")
            '--- Populate Data Grid
            Me.userDataGridView.DataSource = ds.Tables(0).DefaultView

            With userDataGridView
                .Columns(0).Width = 100
                .Columns(0).HeaderText = "Code"
                '  .Columns(1).Width = 740
                .Columns(1).HeaderText = "Phrase"
            End With
            ds.Dispose()
        Catch ex As Exception
            MsgBox(ex.GetBaseException, MsgBoxStyle.Critical, "Phrases List")
            Exit Sub
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        loadPage()
        origValue1 = ""
        origValue2 = ""
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MsgBox("Are you sure want to delete selected record?", MessageBoxButtons.YesNo, "Delete Warning") = MsgBoxResult.Yes Then
            Try
                If Not IsDBNull(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value) And Not IsDBNull(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value) Then
                    strSQL = "Delete From tblPhrases Where Code=" & CInt(userDataGridView.Item(0, userDataGridView.CurrentRow.Index).Value) & " AND ucase(Phrase) ='" & UCase(userDataGridView.Item(1, userDataGridView.CurrentRow.Index).Value) & "'"
                    V_DataSet(strSQL, "D")
                    loadPage()
                    ReloadPhrases()
                Else
                    MsgBox("Please select phrase to delete.", MsgBoxStyle.Information, "Delete The Phrase")
                End If
                origValue1 = ""
                origValue2 = ""
            Catch ex As Exception
                DisplayMessage(ex.Message, "Failed Delete", "S")
                Exit Sub
            End Try
        End If
    End Sub

    Private Sub userDataGridView_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles userDataGridView.CellBeginEdit
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
        Dim ds As New DataSet
        Dim i, k As Integer

        Try
            If IsDBNull(userDataGridView.CurrentRow.Cells(0).Value) Then
                MsgBox("Code number can not be empty!", MsgBoxStyle.Critical, "Add New or Update Phrase")
                Exit Sub
            End If

            If Not IsDBNull(userDataGridView.CurrentRow.Cells(1).Value) Then
                Dim code As Integer = CInt(userDataGridView.CurrentRow.Cells(0).Value)
                Dim Phrase As String = userDataGridView.CurrentRow.Cells(1).Value

                '--- check phrase is exist or not first
                ds = V_DataSet("Select * From tblPhrases Where Code =" & origValue1 & " AND Phrase ='" & origValue2 & "'", "R")
                If ds.Tables(0).Rows.Count > 0 Then
                    '---doing update
                    V_DataSet("Update tblPhrases Set Phrase='" & Phrase & "' WHERE code = " & origValue1, "U")
                    loadPage()
                Else

                    Dim dsC As New DataSet
                    dsC = V_DataSet("Select * From tblPhrases Where Code =" & origValue1, "R")
                    If dsC.Tables(0).Rows.Count = 0 Then
                        '--- add new 
                        V_DataSet("Insert Into tblPhrases values (" & code & ", '" & Phrase & "')", "A")
                        loadPage()
                    Else
                        For i = 0 To userDataGridView.Rows.Count - 1
                            If userDataGridView.Rows(i).Cells(0).Value = code Then
                                K = userDataGridView.Rows(i).Index
                                MsgBox("Phrases " & code & " already exist.", vbInformation, "Phrases Maintenance")
                                loadPage()
                                userDataGridView.Rows(K).Selected = True

                                Exit For
                            End If
                        Next
                    End If
                End If
                LoadPhrasesIntoArray()
            Else
                Exit Sub
            End If
            ds.Dispose()
        Catch ex As Exception
            ' MsgBox(ex.Message, MsgBoxStyle.Critical, "Add New or Update Phrase")
        End Try
    End Sub

    Private Sub userDataGridView_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles userDataGridView.CellMouseClick
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

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            LoadReport("rptPhrases.rpt")
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            DisplayMessage("Failed to print Phrases report", "Print Phrases Report", "S")
            Me.Cursor = Cursors.Default
            Exit Sub
        End Try
    End Sub

    Private Sub LoadReport(ByVal rptName As String, Optional ByVal intVoteID As Object = 0)
        '--- You can change more print options via PrintOptions property of ReportDocument
        Dim report1 As New ReportDocument()

        Try
            '--- use the proper report to pinter based upon who is alive
            '--- print rpt to another printer if one selected

            Dim p As Printing.PrinterSettings
            p = New Printing.PrinterSettings()
            gDefaultPrinter = p.PrinterName

            For x As Integer = 0 To Printing.PrinterSettings.InstalledPrinters.Count - 1
                If UCase(Printing.PrinterSettings.InstalledPrinters.Item(x)) = UCase(gSecondaryPrinter.Trim) Then
                    '--- now change default printe to secondary printer
                    SetDefaultPrinter(Printing.PrinterSettings.InstalledPrinters.Item(x), "", "")
                End If
            Next

            report1.Load(gVotingPath & rptName)
            report1.PrintToPrinter(1, True, 0, 0)
            report1.Close()
        Catch ex As Exception
            DisplayMessage("An error has occurred while tying to print the vote report. Please make " & _
                                       "sure this PC is attached to a printer.", "Print Problem", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub SetDefaultPrinter(ByVal PrinterName As String, ByVal DrivaerName As String, ByVal PrinterPort As String)
        Dim DeviceLine As String
        Dim r As Long

        DeviceLine = PrinterName & "," & DrivaerName & "," & PrinterPort

        '--- Store the new printer informationin the [WINDOWS] section of
        '--- the WIN.INI file for the DEVICE = item
        r = WriteProfileString("windows", "Device", DeviceLine)
        '--- Cause all applications to reload the INI file:
        '--- l = SendMessage(HWND_BROADCAST, WM_WININICHANGE, 0, "windows")
    End Sub

    Private Sub ReloadPhrases()
        Dim frmCD As New frmChamberDisplay
        Dim p As Integer = 0

        Try
            LoadPhrasesIntoArray()
        Catch ex As Exception
            DisplayMessage(ex.Message, "Load Phases", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        '   CDWindowOpen()
        Me.Close()
        ' ChangeSize(frmChamberDisplay)
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

#Region "Old-codes"
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
#End Region
  
End Class