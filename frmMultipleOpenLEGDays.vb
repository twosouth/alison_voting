Imports System.Windows
Imports System.Text

Public Class frmMultipleOpenLEGDays

    Private Sub frmMultipleOpenLEGDays_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.MdiParent = frmMain

        If gWriteVotesToTest Then
            Me.lblTestSessions.Visible = True
        End If

        lblOpenLegislativeDay_1.Visible = True
        lblOpenLegislativeDay_2.Visible = True
        lblOpenLegislativeDay_3.Visible = True
        OpenLegislativeDay_1.Visible = True
        OpenLegislativeDay_2.Visible = True
        OpenLegislativeDay_3.Visible = True
        Session_1.Visible = True
        Session_2.Visible = True
        Session_3.Visible = True

        Select Case gCalendarDayTmp.Length
            Case 1
                lblOpenLegislativeDay_1.Text = gCalendarDayTmp(0)
            Case 2
                lblOpenLegislativeDay_1.Text = gCalendarDayTmp(0)
                lblOpenLegislativeDay_2.Text = gCalendarDayTmp(1)
            Case 3
                lblOpenLegislativeDay_1.Text = gCalendarDayTmp(0)
                lblOpenLegislativeDay_2.Text = gCalendarDayTmp(1)
                lblOpenLegislativeDay_1.Text = gCalendarDayTmp(2)
        End Select

        Select Case gSessionNameTmp.Length
            Case 1
                Session_1.Text = gSessionNameTmp(0)
            Case 2
                Session_1.Text = gSessionNameTmp(0)
                Session_2.Text = gSessionNameTmp(1)
            Case 3
                Session_1.Text = gSessionNameTmp(0)
                Session_2.Text = gSessionNameTmp(1)
                Session_3.Text = gSessionNameTmp(2)
        End Select

        '--- set the first day to true since it will be the most current one
        Me.OpenLegislativeDay_1.Checked = True
    End Sub

    Private Sub btnCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnCancel.Click
        If MsgBox("There are more than one LEG day open. If you selected Cancel, System will shutdown, otherwise select a specific LEG day to continue work.", MsgBoxStyle.YesNo, msgText) = MsgBoxResult.Yes Then
            End
        End If
    End Sub

    Private Sub btnContinue_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnContinue.Click
        k = 0
        If OpenLegislativeDay_1.Checked Then
            k = 0
        ElseIf OpenLegislativeDay_2.Checked Then
            k = 1
        ElseIf OpenLegislativeDay_3.Checked Then
            k = 2
        End If

        gSessionID = gSessionIDTmp(k)
        gLegislativeDay = CStr(gLegislativeDayTmp(k))
        gLegislativeDayOID = gLegislativeDayOIDTmp(k)
        gCalendarDay = gCalendarDayTmp(k)
        gSessionName = gSessionNameTmp(k)
        gSessionAbbrev = gSessionAbbrevTmp(k)

        Last_VoteID()
        If UCase(gComputerName) = UCase(SDIS) Then
            DisplayMessage("System will re-download Bills from ALIS.", "Re-download Bills", "I")
            DownloadBillsFromALIS()
        End If
        MoreThanOneLEGDay = False
        frmMain.Focus()
        Me.Close()
    End Sub
End Class