Option Strict Off
Option Explicit On

Imports System
Imports Microsoft.VisualBasic.CompareMethod

Public Class frmMessage

    Private Sub frmMessage_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        ' Me.SetBounds(TwipsToPixelsX(2880), TwipsToPixelsY(2880), 0, 0, Windows.Forms.BoundsSpecified.X Or Windows.Forms.BoundsSpecified.Y)
        Me.lblTitle.Text = gMessageTitle
        Me.Message.Text = gMessage

        If gMessageType = "" Then ' do nothing - just let processing continue
        ElseIf gMessageType = "C" Then
            Me.btnYes.Visible = False
            Me.btnNo.Visible = False
            Me.btnOK.Visible = False
            btnClose.Visible = True
        ElseIf gMessageType = "Y" Then
            Me.btnYes.Visible = True
            Me.btnNo.Visible = True
            Me.btnOK.Visible = False
            ' btnClose.Visible = True
        Else
            Me.btnYes.Visible = False
            Me.btnNo.Visible = False
            Me.btnOK.Visible = True
            ' btnClose.Visible = True
        End If

    End Sub


    Private Sub btnNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNo.Click
        gMessage = False
        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        gMessage = True
        Me.Close()
    End Sub

    Private Sub btnYes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnYes.Click

        gMessage = True
        Me.Close()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        If MsgBox("Are you sure want to close?", vbYesNo, "Close Senate Voting System") = vbYes Then
            End
        End If

    End Sub
End Class