Imports System
Imports System.Data
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Messaging

Public Class frmBuildBill
    Public frmVote As New frmVote

    Private Sub frmBuildBill_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Me.txtBill.Text = ""
    End Sub

    Private Sub frmBuildBill_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.MdiParent = frmMain
        Me.txtBill.Focus()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        '  gIndex = 0
        Me.Close()
    End Sub

    Private Sub btnKeep_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKeep.Click
        Dim sw As New Stopwatch
        Dim newSR As String = ""
        Dim frmVote As New frmVote


        Try
            If Me.txtBill.Text = "" Then
                MsgBox("Bill can not be empty!", vbCritical, "Invalid Bill")
                Me.txtBill.Text = ""
                Me.txtBill.Focus()
                Exit Sub
            End If
            If Mid(Me.txtBill.Text, 1, 1) <> "H" And Mid(Me.txtBill.Text, 1, 1) <> "S" And Mid(Me.txtBill.Text, 1, 1) <> "C" Then
                DisplayMessage("Invalid Bill! Please Re-enter Bill.", "Invalid Bill", "S")
                Me.txtBill.Text = ""
                Me.txtBill.Focus()
                Exit Sub
            End If
            V_DataSet("Delete From tblOnlyOnePC", "D")
            V_DataSet("Insert Into tblOnlyOnePC Values (1, '" & SVOTE & "','" & Me.txtBill.Text & "')", "A")
            ' frmVote.CurrentBill.Text = Me.txtBill.Text
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.GetBaseException, MsgBoxStyle.Critical, "Build Vote ID")
            Exit Sub
        End Try
    End Sub

    Private Sub btnNumber(ByVal index As String)
        Me.txtBill.Text = Me.txtBill.Text & Format(index)
    End Sub

    Private Sub btnNbr_0_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNbr_0.Click
        btnNumber(0)
    End Sub

    Private Sub btnNbr_1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNbr_1.Click
        btnNumber(1)
    End Sub

    Private Sub btnNbr_2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNbr_2.Click
        btnNumber(2)
    End Sub

    Private Sub btnNbr_3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNbr_3.Click
        btnNumber(3)
    End Sub

    Private Sub btnNbr_4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNbr_4.Click
        btnNumber(4)
    End Sub

    Private Sub btnNbr_5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNbr_5.Click
        btnNumber(5)
    End Sub

    Private Sub btnNbr_6_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNbr_6.Click
        btnNumber(6)
    End Sub

    Private Sub btnNbr_7_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNbr_7.Click
        btnNumber(7)
    End Sub

    Private Sub btnNbr_8_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNbr_8.Click
        btnNumber(8)
    End Sub

    Private Sub btnNbr_9_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNbr_9.Click
        btnNumber(9)
    End Sub

    Private Sub btnSB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSB.Click
        btnNumber("SB")
    End Sub

    Private Sub btnSR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSR.Click
        btnNumber("SR")
    End Sub

    Private Sub btnSJR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSJR.Click
        btnNumber("SJR")
    End Sub

    Private Sub btnCF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCF.Click
        btnNumber("CF")
    End Sub

    Private Sub btnHB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHB.Click
        btnNumber("HB")
    End Sub

    Private Sub btnHJR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHJR.Click
        btnNumber("HJR")
    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Me.txtBill.Text = ""
        Me.txtBill.Focus()
    End Sub
End Class