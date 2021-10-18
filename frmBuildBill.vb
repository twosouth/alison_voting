Imports System
Imports System.Data
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Messaging

Public Class frmBuildBill
    Public frmVote As New frmVote
    Private upCase As Boolean = False
    Private entered As Boolean = False
    Private bClick, mClick As Boolean

    Private Sub frmBuildBill_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Me.txtBill.Text = ""
        Me.txtBill.Focus()
        Me.txtBill.SelectionStart = 0
        Me.txtBill.TabIndex = 0
    End Sub

    Private Sub frmBuildBill_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.MdiParent = frmMain

        bClick = True
        mClick = False
        Me.txtBill.Focus()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        'If Me.rdoBill.Checked Then
        '    Me.txtBill.Text = ""
        '    Me.txtBill.Focus()
        'End If
        'If Me.rdoMotion.Checked Then
        '    Me.txtMotion.Text = ""
        '    Me.txtMotion.Focus()
        'End If
        If bClick Then
            Me.txtBill.Text = ""
            Me.txtBill.Focus()
        End If
        If mClick Then
            Me.txtMotion.Text = ""
            Me.txtMotion.Focus()
        End If
        entered = False
    End Sub

    Private Sub btnKeep_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKeep.Click
        Dim sw As New Stopwatch
        Dim newSR As String = ""
        Dim frmVote As New frmVote

        Try
            'If Me.txtBill.Text = "" And rdoBill.Checked Then
            '    MsgBox("Bill can not be empty!", vbCritical, "Invalid Bill")
            '    Me.txtBill.Text = ""
            '    Me.txtBill.Focus()
            '    Exit Sub
            'End If

            'If Me.txtMotion.Text = "" And rdoMotion.Checked Then
            '    MsgBox("Motion can not be empty!", vbCritical, "Invalid Motion")
            '    Me.txtMotion.Text = ""
            '    Me.txtMotion.Focus()
            '    Exit Sub
            'End If

            V_DataSet("Delete From tblOnlyOnePC", "D")
            V_DataSet("Insert Into tblOnlyOnePC Values (1, '" & SVOTE & "','" & Me.txtBill.Text & "', '" & Me.txtMotion.Text & "')", "A")
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.GetBaseException, MsgBoxStyle.Critical, "Build Vote ID")
            Exit Sub
        End Try
    End Sub

    Private Sub btnNumber(ByVal index As String)
        'If rdoBill.Checked Then
        '    Me.txtBill.Text = Me.txtBill.Text & Format(index)
        'Else
        '    Me.txtMotion.Text = Me.txtMotion.Text & Format(index)
        'End If
        If bClick Then
            Me.txtBill.Text = Me.txtBill.Text & Format(index)
        Else
            Me.txtMotion.Text = Me.txtMotion.Text & Format(index)
        End If
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

    Private Sub btnHR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHR.Click
        btnNumber("HR")
    End Sub

    Private Sub btnHJR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHJR.Click
        btnNumber("HJR")
    End Sub

    Private Sub btnBIR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBIR.Click
        btnNumber("BIR")
    End Sub

    Private Sub btnRecess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecess.Click
        btnNumber("RECESS")
    End Sub

    Private Sub btnAdj_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdj.Click
        btnNumber("AKJOURN")
    End Sub

    Private Sub btnA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnA.Click
        If upCase Then
            btnNumber("A")
        Else
            btnNumber("a")
        End If
    End Sub

    Private Sub btnB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnB.Click
        If upCase Then
            btnNumber("B")
        Else
            btnNumber("b")
        End If
    End Sub

    Private Sub btnC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnC.Click
        If upCase Then
            btnNumber("C")
        Else
            btnNumber("c")
        End If
    End Sub

    Private Sub btnD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnD.Click
        If upCase Then
            btnNumber("D")
        Else
            btnNumber("d")
        End If
    End Sub

    Private Sub btnE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnE.Click
        If upCase Then
            btnNumber("E")
        Else
            btnNumber("e")
        End If
    End Sub

    Private Sub btnF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnF.Click
        If upCase Then
            btnNumber("F")
        Else
            btnNumber("f")
        End If
    End Sub

    Private Sub btnG_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnG.Click
        If upCase Then
            btnNumber("G")
        Else
            btnNumber("g")
        End If
    End Sub

    Private Sub btnH_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnH.Click
        If upCase Then
            btnNumber("H")
        Else
            btnNumber("h")
        End If
    End Sub

    Private Sub btnI_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnI.Click
        If upCase Then
            btnNumber("I")
        Else
            btnNumber("i")
        End If
    End Sub

    Private Sub btnJ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnJ.Click
        If upCase Then
            btnNumber("J")
        Else
            btnNumber("j")
        End If
    End Sub

    Private Sub btnK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnK.Click
        If upCase Then
            btnNumber("K")
        Else
            btnNumber("k")
        End If
    End Sub

    Private Sub btnL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnL.Click
        If upCase Then
            btnNumber("L")
        Else
            btnNumber("l")
        End If
    End Sub

    Private Sub btnM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnM.Click
        If upCase Then
            btnNumber("M")
        Else
            btnNumber("m")
        End If
    End Sub

    Private Sub btnN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnN.Click
        If upCase Then
            btnNumber("N")
        Else
            btnNumber("n")
        End If
    End Sub

    Private Sub btnO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnO.Click
        If upCase Then
            btnNumber("O")
        Else
            btnNumber("o")
        End If
    End Sub

    Private Sub btnP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnP.Click
        If upCase Then
            btnNumber("P")
        Else
            btnNumber("p")
        End If
    End Sub

    Private Sub btnQ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnQ.Click
        If upCase Then
            btnNumber("Q")
        Else
            btnNumber("q")
        End If
    End Sub

    Private Sub btnR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnR.Click
        If upCase Then
            btnNumber("R")
        Else
            btnNumber("r")
        End If
    End Sub

    Private Sub btnS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnS.Click
        If upCase Then
            btnNumber("S")
        Else
            btnNumber("s")
        End If
    End Sub

    Private Sub btnT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnT.Click
        If upCase Then
            btnNumber("T")
        Else
            btnNumber("t")
        End If
    End Sub

    Private Sub btnU_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnU.Click
        If upCase Then
            btnNumber("U")
        Else
            btnNumber("u")
        End If
    End Sub

    Private Sub btnV_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnV.Click
        If upCase Then
            btnNumber("V")
        Else
            btnNumber("v")
        End If
    End Sub

    Private Sub btnW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnW.Click
        If upCase Then
            btnNumber("W")
        Else
            btnNumber("w")
        End If
    End Sub

    Private Sub btnX_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnX.Click
        If upCase Then
            btnNumber("X")
        Else
            btnNumber("x")
        End If
    End Sub

    Private Sub btnY_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnY.Click
        If upCase Then
            btnNumber("Y")
        Else
            btnNumber("y")
        End If
    End Sub

    Private Sub btnZ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnZ.Click
        If upCase Then
            btnNumber("Z")
        Else
            btnNumber("z")
        End If
    End Sub

    Private Sub btnComma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnComma.Click
        btnNumber(",")
    End Sub

    Private Sub btnPeriod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPeriod.Click
        btnNumber(".")
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'If rdoBill.Checked Then
        '    If Me.txtBill.Text.Length > 0 Then
        '        Me.txtBill.Text = Mid(Me.txtBill.Text, 1, Me.txtBill.Text.Length - 1)
        '    End If
        'Else
        '    If Me.txtMotion.Text.Length > 0 Then
        '        Me.txtMotion.Text = Mid(Me.txtMotion.Text, 1, Me.txtMotion.Text.Length - 1)
        '    End If
        'End If
        If bClick Then
            If Me.txtBill.Text.Length > 0 Then
                Me.txtBill.Text = Mid(Me.txtBill.Text, 1, Me.txtBill.Text.Length - 1)
            End If
        Else
            If Me.txtMotion.Text.Length > 0 Then
                Me.txtMotion.Text = Mid(Me.txtMotion.Text, 1, Me.txtMotion.Text.Length - 1)
            End If
        End If
    End Sub

    Private Sub btnSpace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSpace.Click
        'If rdoBill.Checked Then
        '    If entered Then
        '        txtBill_KeyDown(txtBill, Nothing)
        '    Else
        '        Me.txtBill.Text = Me.txtBill.Text & " "
        '    End If
        'Else
        '    If entered Then
        '        txtMotion_KeyDown(txtMotion, Nothing)
        '    Else
        '        Me.txtMotion.Text = Me.txtMotion.Text & " "
        '    End If
        'End If
        If bClick Then
            If entered Then
                txtBill_KeyDown(txtBill, Nothing)
            Else
                Me.txtBill.Text = Me.txtBill.Text & " "
            End If
        Else
            If entered Then
                txtMotion_KeyDown(txtMotion, Nothing)
            Else
                Me.txtMotion.Text = Me.txtMotion.Text & " "
            End If
        End If
    End Sub

    Private Sub txtBill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBill.Click
        bClick = True
        mClick = False
    End Sub

    Private Sub txtBill_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBill.Enter
        entered = True
    End Sub

    Private Sub txtBill_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBill.KeyDown
        'If e.KeyCode = Keys.Return Then
        Dim MyTextBox As TextBox = DirectCast(sender, TextBox)
        MyTextBox.SelectedText = " "
        MyTextBox.SelectionStart = MyTextBox.Text.Length
        '  Me.txtBill.Text = MyTextBox.Text
        'e.SuppressKeyPress = True
        ' End If
        entered = False
    End Sub

    Private Sub txtMotion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMotion.Click
        bClick = False
        mClick = True
    End Sub

    Private Sub txtMotion_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMotion.Enter
        entered = True
    End Sub

    Private Sub txtMotion_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMotion.KeyDown
        'If e.KeyCode = Keys.Return Then
        Dim MyTextBox As TextBox = DirectCast(sender, TextBox)
        MyTextBox.SelectedText = " "
        MyTextBox.SelectionStart = MyTextBox.Text.Length
        '  Me.txtMotion.Text = MyTextBox.Text
        'e.SuppressKeyPress = True
        ' End If
        entered = False
    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        If Me.btnUp.BackColor = Color.White Then
            Me.btnUp.BackColor = Color.Red
            upCase = True
        ElseIf Me.btnUp.BackColor = Color.Red Then
            Me.btnUp.BackColor = Color.White
            upCase = False
        End If
    End Sub



End Class