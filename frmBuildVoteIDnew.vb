Imports System
Imports System.Data
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Messaging

Public Class frmBuildVoteIDnew
    Public frmVote As New frmVote

    Private Sub frmBuildVoteIDnew_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Me.VoteID.Text = ""
    End Sub

    Private Sub frmBuildVoteIDnew_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.MdiParent = frmMain
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'gIndex = 0
        'Me.Close()
  
        If index > 0 Then
            If index = 1 Then
                frmVote.VoteID_1.Text = Val(Me.VoteID.Text)
                UpdateVotingDisplayParameters(0, setR2, setR3)

            ElseIf index = 2 Then
                frmVote.VoteID_2.Text = Me.VoteID.Text
                UpdateVotingDisplayParameters(setR1, 0, setR3)

            ElseIf index = 3 Then
                frmVote.VoteID_3.Text = Me.VoteID.Text
                UpdateVotingDisplayParameters(setR1, setR2, 0)
            ElseIf index = 4 Then
                V_DataSet("Delete From tblBuiltVoteID", "D")
            End If
        End If
        Me.Close()
    End Sub


    Private Sub btnKeep_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKeep.Click
        Dim setVoteID As Integer
        Try
            If (Val(Me.VoteID.Text) >= gVoteID) And index <> 4 Then
                DisplayMessage("The vote ID you create must be < " & gVoteID, "Invalid Vote ID", "S")
                Me.VoteID.Text = ""
            Else
                setVoteID = Val(Me.VoteID.Text)
                If index > 0 Then
                    If index = 1 Then
                        frmVote.VoteID_1.Text = Val(Me.VoteID.Text)
                        UpdateVotingDisplayParameters(setVoteID, setR2, setR3)
                        Exit Try
                    ElseIf index = 2 Then
                        frmVote.VoteID_2.Text = Me.VoteID.Text
                        UpdateVotingDisplayParameters(setR1, setVoteID, setR3)
                        Exit Try
                    ElseIf index = 3 Then
                        frmVote.VoteID_3.Text = Me.VoteID.Text
                        UpdateVotingDisplayParameters(setR1, setR2, setVoteID)
                        Exit Try
                    ElseIf index = 4 Then
                        frmVote.CurrentVoteID.Text = Me.VoteID.Text
                        V_DataSet("Delete From tblBuiltVoteID", "D")
                        V_DataSet("Insert Into tblBuiltVoteID Values(" & Me.VoteID.Text & ")", "A")
                        Exit Try
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.GetBaseException, MsgBoxStyle.Critical, "Build Vote ID")
            Exit Sub
        Finally
            index = 0
            Me.Close()
        End Try
    End Sub

    Private Sub SendVoteSet(ByVal label As String, ByVal strBody As String)
        Try
            Dim mq As New MessageQueue
            Dim msg As New Message
            Dim bodyText As String = ""

            Try
                mq.Path = "FormatName:DIRECT=OS:SENATEVOTING\PRIVATE$\requestvoteidqueue"
                msg.Priority = MessagePriority.Normal
                msg.Label = label
                msg.Body = strBody
                mq.Send(msg)
                mq.Close()
            Catch ex As Exception
                Throw New Exception("Error Getting Quequ")
                mq.Close()
                Exit Try
            End Try
        Catch ex As Exception
            Dim p As New Process
            p = Process.Start(gCmdFile)
            p.WaitForExit()
            DisplayMessage("Message queue does not work. Unable send the Close Voting Information." & vbCrLf & "System will write the vote records on local." & vbCrLf & " Please contact to Administrator.", "Close Voting", "S")
            Exit Sub
        End Try
    End Sub

    Private Sub btnNumber(ByVal index As String)
        Me.VoteID.Text = Me.VoteID.Text & Format(index)
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
End Class