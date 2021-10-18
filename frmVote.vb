Imports System
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports System.Timers
Imports System.Messaging
Imports System.Text
Imports System.DateTime
Imports System.DBNull
Imports System.Net.NetworkInformation
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class frmVote
    Private i, j, k, SenatorIndex, VoteOrderIndex, NbrQuorum As Integer
    Private WrkFld, strMotion As String
    Private ShortVote, DisplayPC_No_Now, rollcall As Boolean
    Private arrSenator, arrRecallVote, arrSet, arrVoteID, arrBox As New ArrayList
    Private senatorBoxes(34) As TextBox
    Private labelBoxes(34) As Label
    Private err As Microsoft.VisualBasic.ErrObject
    Private deleteError As Boolean = False
    Private reCall As Boolean = False
    
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

    Private Sub frmVote_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ds As New DataSet
        Dim dsR As New DataSet

        Try
            senatorBoxes = New TextBox() {Senator_1, Senator_2, Senator_3, Senator_4, Senator_5, Senator_6, Senator_7, Senator_8, Senator_9, Senator_10, _
                                        Senator_11, Senator_12, Senator_13, Senator_14, Senator_15, Senator_16, Senator_17, Senator_18, Senator_19, Senator_20, Senator_21, Senator_22, Senator_23, _
                                        Senator_24, Senator_25, Senator_26, Senator_27, Senator_28, Senator_29, Senator_30, Senator_31, Senator_32, Senator_33, Senator_34, Senator_35}
            labelBoxes = New Label() {_Box_1, _Box_2, _Box_3, _Box_4, _Box_5, _Box_6, _Box_7, _Box_8, _Box_9, _Box_10, _
                                        _Box_11, _Box_12, _Box_13, _Box_14, _Box_15, _Box_16, _Box_17, _Box_18, _Box_19, _Box_20, _Box_21, _Box_22, _Box_23, _
                                        _Box_24, _Box_25, _Box_26, _Box_27, _Box_28, _Box_29, _Box_30, _Box_31, _Box_32, _Box_33, _Box_34, _Box_35}

            '--- clear chamber display PC queue
            Me.lblVotingLight.BackColor = Color.LightGreen
            Me.lblVotingLight.Text = "Waiting For Next Bill"

            Me.VoteID_1.Text = 0
            Me.VoteID_2.Text = 0
            Me.VoteID_3.Text = 0

            If SDisplay_On Then
                gOnlyOnePC = False
                ReceiveTimer.Enabled = True
                ReceiveTimer.Start()
                ReceiveTimer.Interval = gReceiveQueueTimer
                SendMessageToDisplayPC("LASTVOTEDID", Trim(CStr(gVoteID)))
                Me.Bill.Visible = False
                Me.CurrentBill.ReadOnly = True
                Me.CurrentMotion.ReadOnly = True
                Me.CurrentVoteID.ReadOnly = True
                Me.btnClear.Visible = False
                Me.btnClearMotion.Visible = False
                Me.btnBill.Visible = False
                Me.Phrases.Visible = False
                Me.Senators.Visible = False
                Me.lblBill.Visible = False
                Me.lblSenator.Visible = False
                Me.lblPhrase.Visible = False
            Else
                gOnlyOnePC = True
                ReceiveTimer.Enabled = False
                ReceiveTimer.Stop()
                Me.Bill.Visible = True
                Me.CurrentBill.ReadOnly = False
                Me.CurrentMotion.ReadOnly = False
                Me.CurrentVoteID.ReadOnly = False
                Me.CurrentBill.Visible = True
                Me.CurrentMotion.Visible = True
                Me.CurrentVoteID.Visible = True
                Me.btnClear.Visible = True
                Me.btnClearMotion.Visible = True
                Me.btnBill.Visible = True
                Me.Phrases.Visible = True
                Me.Senators.Visible = True
                Me.lblBill.Visible = True
                Me.lblSenator.Visible = True
                Me.lblPhrase.Visible = True
                Load_Bill_Phrases_Senators()
                DisplayMessage("Chamber Display PC is offline. Please click 'Unlock Button' to continue voting job.", "Continue Voting Job", "S")
            End If

            '--- get Last Voted ID
            ds = V_DataSet("Select ParameterValue From tblVotingParameters Where Parameter ='LastVoteIDForThisSession'", "R")
            If gTestMode Then
                lblTestMode.Visible = True
                If SDisplay_On = False Then
                    gVoteID = ds.Tables(0).Rows(0).Item(0) + 1
                    Me.CurrentVoteID.Text = gVoteID
                End If
            Else
                lblTestMode.Visible = False
                If SDisplay_On = False Then
                    Me.CurrentVoteID.Text = gVoteID + 1
                End If
            End If

            '--- load RecallVote1, RecallVote2, RecallVote3 ID 
            setRTimer.Enabled = True
            setRTimer.Start()
            LoadRecallVote()

            '--- initial window controls
            InitializeForm("StartUp")
            InitializeFormForOnlyOnePC()

            V_DataSet("Update tblVotingParameters Set ParameterValue='" & gSessionName & "' WHERE UCASE(Parameter) = 'LASTSESSIONNAME'", "U")
            V_DataSet("Update tblVotingParameters Set ParameterValue='" & gSessionID & "' WHERE UCASE(Parameter) = 'LASTSESSIONID'", "U")

            gVoteID = Me.CurrentVoteID.Text

            ds.Dispose()
            dsR.Dispose()
        Catch ex As Exception
            ds.Dispose()
            dsR.Dispose()
            DisplayMessage("Open vote system window failed. Please try open it again. If error consistent occrrues, please contact to Administrator.", "Member Voting Window", "S")
            End
        End Try
    End Sub

    Private Sub InitializeFormForOnlyOnePC()
        If SDisplay_On = False Or gOnlyOnePC Then
            Me.CurrentBill.ReadOnly = False
            Me.CurrentVoteID.ReadOnly = False
        Else
            CurrentBill.ReadOnly = True
            CurrentVoteID.ReadOnly = True
            Me.lblSenator.Visible = False
            Me.lblPhrase.Visible = False
            Me.btnClear.Visible = False
        End If
    End Sub

    Private Sub Load_Bill_Phrases_Senators()
        Dim dsBill As New DataSet

        '--- Load Senators from gSenatorName array in to SenatorName comboBox
        Try
            Me.Senators.Items.Clear()
            Me.Senators.Items.Add("")
            For i As Integer = 0 To gSenatorName.Length - 1
                If gSenatorName(i) <> Nothing Then
                    Me.Senators.Items.Add(gSenatorName(i))
                End If
            Next

            '--- Load  Phases from gThePhrases(i) array in to Phases comboBox. Notic: gThePhrases(i) array included PhraseCode and Phrase, gPhrases has only Phrases string in array
            Me.Phrases.Items.Clear()
            Me.Phrases.Items.Add("")
            For k As Integer = 0 To gPhraseForVotePC.Length - 1
                If gPhraseForVotePC(k) <> Nothing Then
                    Me.Phrases.Items.Add(gPhraseForVotePC(k))
                End If
            Next

            Try
                '--- download Bills from ALIS first, then load it to Bill dropdown box
                If gNetwork Then
                    isDownLoadBillorSOC = True

                    DownloadBillsFromALIS()

                    Me.Bill.Items.Clear()
                    Me.Bill.Items.Add("")

                    dsBill = V_DataSet("Select * From tblBills Where CalendarCode ='1' Order By BillNbr", "R")
                    For Each drBill As DataRow In dsBill.Tables(0).Rows
                        Me.Bill.Items.Add(drBill("Bill"))
                    Next
                End If
                dsBill.Dispose()
            Catch ex As Exception
                dsBill.Dispose()
            End Try
        Catch ex As Exception
            If ex.Message <> "Index was outside the bounds of the array" Then
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Load Phrases and Senators")
            End If
        End Try
    End Sub

    Private Sub SetColorAndFocus(ByVal index As Integer, ByVal backColor As System.Drawing.Color)
        Dim obj As New Object

        '--- if short vote, then only allow so many votes; if pass vote then
        '--- let it continue since not counted in vote total
        If ShortVote And (backColor <> Color.Yellow) Then
            If GetCounts() = gAllowedShortVoteCnt Then
                DisplayMessage("You are only allowed " & gAllowedShortVoteCnt & " with a short vote. If you want to vote this person, " & _
                                           "then you must 'Pass' a Senator who you have previously voted.", "Vote Not Counted", "S")
                Exit Sub
            End If
        End If

        If gVotingStarted Then
        Else
            gVotingStarted = True
            gPutCalendarCode = gCalendarCode
            gPutCalendar = gCalendar
            gPutBill = gBill
            gPutBillNbr = gBillNbr
            gPutPhrase = gCurrentPhrase
            gPutChamberLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
            gPutVotingLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green)
        End If

        '---set senator vote to color
        For y As Integer = 0 To gNbrSenators - 1
            If y = index Then
                labelBoxes(y).Visible = False
                senatorBoxes(y).BackColor = backColor
            End If
        Next

        For y As Integer = 0 To gNbrSenators - 1
            labelBoxes(y).Visible = False
        Next

        '--- found out skip one to get focus
        For b As Integer = 0 To gNbrSenators - 1
            If senatorBoxes(b).BackColor = System.Drawing.Color.White Then
                labelBoxes(b).Visible = True
                senatorBoxes(b).Focus()
                Exit Sub
            End If
        Next

        RetValue = GetCounts()
    End Sub

    Private Sub btnAbstain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbstain.Click
        'If rollcall Then
        '    SenatorIndex = 0
        'End If
        SetColorAndFocus(SenatorIndex, System.Drawing.Color.Blue)
        GetCounts()
        rollcall = False
    End Sub

    Private Sub btnYea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnYea.Click
        'If rollcall Then
        '    SenatorIndex = 0
        'End If
        SetColorAndFocus(SenatorIndex, System.Drawing.Color.Green)
        GetCounts()
        rollcall = False
    End Sub

    Private Sub btnNay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNay.Click
        'If rollcall Then
        '    SenatorIndex = 0
        'End If
        SetColorAndFocus(SenatorIndex, System.Drawing.Color.Red)
        GetCounts()
        rollcall = False
    End Sub

    Private Sub btnPass_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPass.Click
        'If rollcall Then
        '    SenatorIndex = 0
        'End If
        SetColorAndFocus(SenatorIndex, System.Drawing.Color.Yellow)
        GetCounts()
        rollcall = False
    End Sub

    Private Sub ReceiveTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReceiveTimer.Tick
        ReceiveMessageFromDisplayPC()
    End Sub

    Private Sub ReceiveMessageFromDisplayPC()
        Dim Paramters() As String
        Dim i As Integer
        Dim txt, tmpText As String

        Try
            If (MessageQueue.Exists(gReceiveQueueName)) = False Then
                MessageQueue.Create(gReceiveQueueName)
            End If

            Dim queue As New MessageQueue(gReceiveQueueName)
            queue.Formatter = New XmlMessageFormatter(New String() {"System.String"})
            Dim qenum As MessageEnumerator

            qenum = queue.GetMessageEnumerator2
            While qenum.MoveNext
                Dim m As Message = qenum.Current
                If m.Label = "STARTVOTE" Then
                    Me.CurrentBill.Text = ""
                    Me.CurrentMotion.Text = ""
                    Me.btnClearAndCancel.Enabled = True
                    Me.btnCloseVote.Enabled = True
                    msgText = m.Body
                    m = queue.Receive(New TimeSpan(1000))
                    If msgText <> "" Then
                        Paramters = Split(msgText, "||")
                        For i = 0 To Paramters.Length - 1
                            txt = Mid(Paramters(i), 1, InStr(Paramters(i), " - ") - 1)
                            tmpText = Mid(Paramters(i), InStr(Paramters(i), " - ") + 3)
                            Select Case txt
                                Case "gVoteID"
                                    CurrentVoteID.Text = tmpText
                                    gVoteID = CInt(tmpText)
                                    lblVotingLight.BackColor = Color.Red
                                    lblVotingLight.Text = "Now Voting"
                                Case "gBillNbr"
                                    gBillNbr = tmpText
                                Case "gBill"
                                    gBill = tmpText
                                    If InStrRev(Trim(tmpText), " ") > 0 Then
                                        Me.CurrentBill.Text = Mid(Trim(tmpText), 1, InStrRev(Trim(tmpText), " "))
                                    Else
                                        Me.CurrentBill.Text = gBill
                                    End If
                                Case "gCurrentPhrase"
                                    gCurrentPhrase = tmpText
                                    Me.CurrentMotion.Text = gCurrentPhrase
                                Case "gCalendarCode"
                                    gCalendarCode = tmpText
                                    If gCalendarCode = "M" Then
                                        gBill = gBillNbr
                                        Me.CurrentBill.Text = gBillNbr
                                    End If
                            End Select
                        Next
                        EnableButtons()
                        InitializeFormForOnlyOnePC()
                    Else
                        DisplayMessage("Incorrect voting data! Please request correct voting data.", "Request correct voting data", "S")
                        SendMessageToDisplayPC("RECALL", "")
                    End If
                ElseIf m.Label = "VOTEID" Then
                    Me.btnCloseVote.Enabled = True
                    m = queue.Receive(New TimeSpan(1000))
                    DisplayMessage("Received Last VoteID Is: " & m.Body, "Received Last Vote ID", "S")
                ElseIf m.Label = "CANCEL" Then
                    m = queue.Receive(New TimeSpan(1000))
                    lblVotingLight.BackColor = Color.LightGreen
                    lblVotingLight.Text = "Waiting Next Bill"
                    EnableButtons()
                    If SDisplay_On = False Then
                        Me.CurrentBill.Text = ""
                        btnClear_Click(btnClear, Nothing)
                    End If
                    gAction = ""
                    InitializeForm("")
                End If
                SDisplay_On = True
            End While

            queue.Close()
        Catch
            Try
                Dim p As New Process
                p = Process.Start(gCmdFile)
                p.WaitForExit()
                ReceiveMessageFromDisplayPC()
            Catch ex As Exception
                If DisplayMessage(ex.Message & " System can not communicate to Chamber Display PC! Would you want to continue work?", "Voting Systme", "Y") Then
                    Exit Sub
                Else
                    End
                End If
                SDisplay_On = False
            End Try
        End Try
    End Sub

    Private Sub EnableButtons()
        For Each cntrl As Control In Me.Controls
            If TypeOf cntrl Is Button Then
                If InStr(UCase(cntrl.Name), "BTN") > 0 Then
                    cntrl.Enabled = True
                End If
            End If
        Next
    End Sub

    Private Sub DisableButtons()
        For Each cntrl As Control In Me.Controls
            If TypeOf cntrl Is Button Then
                If InStr(UCase(cntrl.Name), "BTN") > 0 Then
                    cntrl.Enabled = False
                End If
            End If
        Next
    End Sub

    Private Function GetCounts() As Integer
        Dim YeaNayAbstainCnt, j As Integer

        '--- initial controls
        Me.YeaCount.Text = 0
        Me.NayCount.Text = 0
        Me.AbstainCount.Text = 0
        Me.PassCount.Text = 0
        Me.PercentYea.Text = 0
        Me.PercentNay.Text = 0
        Me.PercentAbstain.Text = 0

        For j = 0 To gNbrSenators - 1
            Select Case System.Drawing.ColorTranslator.ToOle(Me.senatorBoxes(j).BackColor)
                Case System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green) : Me.YeaCount.Text = CStr(CDbl(Me.YeaCount.Text) + 1)
                Case System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red) : Me.NayCount.Text = CStr(CDbl(Me.NayCount.Text) + 1)
                Case System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue) : Me.AbstainCount.Text = CStr(CDbl(Me.AbstainCount.Text) + 1)
                Case System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow) : Me.PassCount.Text = CStr(CDbl(Me.PassCount.Text) + 1)
            End Select
        Next j

        YeaNayAbstainCnt = Val(Me.YeaCount.Text) + Val(Me.NayCount.Text) + Val(Me.AbstainCount.Text)

        If YeaNayAbstainCnt < gSenatorSplit Then
            Me.chkQuorum.Checked = False
            Me.lblQuorumPresNotVoting.BackColor = Color.Red
        Else
            Me.lblQuorumPresNotVoting.BackColor = Color.Green
            Me.chkQuorum.Checked = True
        End If

        '--- if pass is the first one, then this count will still be 0, so skip
        If YeaNayAbstainCnt > 0 Then
            Me.PercentYea.Text = CStr(Int(((CDbl(Me.YeaCount.Text) / YeaNayAbstainCnt) * 100) + 0.9))
            Me.PercentNay.Text = CStr(Int(((CDbl(Me.NayCount.Text) / YeaNayAbstainCnt) * 100) + 0.9))
            Me.PercentAbstain.Text = CStr(Int(((CDbl(Me.AbstainCount.Text) / YeaNayAbstainCnt) * 100) + 0.9))
        End If

        GetCounts = YeaNayAbstainCnt
    End Function

    Private Sub SendMessageToDisplayPC(ByVal label As String, ByVal strBody As String)

        Dim mq, mq1 As New MessageQueue
        Dim msg, msg1 As New Message
        Dim bodyText As String = ""

        Try
            If SDisplay_On Then
                mq.Path = gSendQueueToDisplay
            Else
                Exit Sub
            End If
            msg.Priority = MessagePriority.Normal
            msg.Label = label
            msg.Body = strBody
            mq.Send(msg)

            If label = "ASKVOTEID" Then
                DisplayMessage("Send the request last vote id has finished.", "Send request last vote id", "S")
            End If
            mq.Close()
        Catch
            Try
                Dim p As New Process
                p = Process.Start(gCmdFile)
                p.WaitForExit()

                If SDisplay_On Then
                    mq1.Path = gSendQueueToDisplay
                Else
                    Exit Sub
                End If
                msg1.Priority = MessagePriority.Normal
                msg1.Label = label
                msg1.Body = strBody
                mq1.Send(msg)

                If label = "ASKVOTEID" Then
                    DisplayMessage("Send the request last vote id has finished.", "Send request last vote id", "S")
                End If
                mq1.Close()
                msg1.Dispose()
            Catch ex As Exception
                DisplayMessage("Unable send the Close Voting Information to Display PC." & vbCrLf & "System will write the vote records on local." & vbCrLf & " Please contact to Administrator.", "Close Voting", "S")
                SendVotesToLocal()
            End Try
        Finally
            mq.Close()
            mq1.Close()
            msg.Dispose()
            msg1.Dispose()
        End Try
       
    End Sub

    Private Sub InitializeForm(ByVal Source As String)
        Dim j As Integer = 1

        Try

            '--- this routine is done from a couple of places and we dont want to write any passed params
            '--- if the source = startup or clearonly
            gReSendALIS = False
            SenatorIndex = 1
            VoteOrderIndex = 1
            Me.YeaCount.Text = 0
            Me.NayCount.Text = 0
            Me.AbstainCount.Text = 0
            Me.PassCount.Text = 0
            Me.PercentYea.Text = 0
            Me.PercentNay.Text = 0
            Me.PercentAbstain.Text = 0
            Me.btnCloseVote.BackColor = Color.Violet
            Me.btnCloseVote.Enabled = True
            Me.chkQuorum.Checked = False
            Me.lblQuorumPresNotVoting.BackColor = Color.Red
            rollcall = False

            If gNbrSenators > 0 Then
                For j = 0 To gNbrSenators - 1
                    senatorBoxes(j).Visible = True
                    If gSalutation(j) <> "" Then
                        senatorBoxes(j).Text = gSalutation(j) & " " & gSenatorNameOrder(j)
                    Else
                        senatorBoxes(j).Text = gSenatorNameOrder(j)
                    End If
                    senatorBoxes(j).BackColor = Color.White
                    labelBoxes(i).Visible = False
                Next
            End If
            If Source <> "ClearOnly" Then
                Me.CurrentBill.Text = ""
                Me.CurrentMotion.Text = ""
                gVotingStarted = False
                Me.lblVotingLight.Text = "Waiting For Next Bill"
                Me.lblVotingLight.BackColor = Color.LightGreen
                gVoteID = GetLastLocalVoteID()
                Me.CurrentVoteID.Text = gVoteID + 1
            End If

            '--- send back params to chamber, but reverse colors - done here to send vote id - not done on startup
            If (Source = "StartUp") Or (Source = "ClearOnly") Then
                SenatorIndex = 0
            Else
                gPutCalendarCode = gCalendarCode
                gPutCalendar = gCalendar
                gPutBill = gBill
                gPutBillNbr = gBillNbr
                gPutPhrase = gCurrentPhrase
                gPutChamberLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green)
                gPutVotingLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
            End If

            Me._Box_1.Visible = True
            Me.Senator_1.Focus()
            vSet = False

        Catch ex As Exception
            DisplayMessage(ex.Message, "Initialize Form", "S")
        End Try
    End Sub

    Private Sub btnBill_Click(sender As System.Object, e As System.EventArgs) Handles btnBill.Click
        frmBuildBill.Show()
    End Sub

    Private Sub btnClearAndCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAndCancel.Click
        Dim mq As New MessageQueue(gSendQueueFromVotePC)
        Dim msg As New Message

        Try
            rollcall = False
            If gVotingHelp Then
                If DisplayMessage("You are about to clear the votes. Do you want to continue?", "Cancel And Clear", "Y") Then
                    InitializeForm("")
                End If
            Else
                InitializeForm("")
            End If
            If LockReadOnly Then
                CurrentBill.ReadOnly = True
                CurrentVoteID.ReadOnly = True
                CurrentMotion.ReadOnly = True
            Else
                CurrentBill.ReadOnly = False
                CurrentVoteID.ReadOnly = False
                CurrentMotion.ReadOnly = False
            End If

            '--- clear all of messages on myself Queue
            mq.Purge()

            SendMessageToDisplayPC("CANCEL", "")

            msg.Dispose()
            mq.Close()
        Catch
            Dim mq1 As New MessageQueue(gSendQueueFromVotePC)
            Dim msg1 As New Message

            Try
                Dim p As New Process
                p = Process.Start(gCmdFile)
                p.WaitForExit()

                '--- clear all of messages on myself Queue
                mq1.Purge()
                SendMessageToDisplayPC("CANCEL", "")

                msg1.Dispose()
                mq1.Dispose()
            Catch ex As Exception
                MsgBox("Unable send 'Cancel Vote' Message to 'Display PC. Please contact to Administrator.", MsgBoxStyle.Critical, "Unabel Send 'Cancel Vote' Message")
            Finally
                msg1.Dispose()
                mq1.Close()
            End Try
        Finally
            msg.Dispose()
            mq.Close()
        End Try
    End Sub

    Private Sub btnClearOnly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearOnly.Click
        If gVotingHelp Then
            If DisplayMessage("You are about to clear the votes. Do you want to continue?", "Cancel And Clear", "Y") Then
                InitializeForm("ClearOnly")
            End If
        Else
            InitializeForm("ClearOnly")
        End If
        If LockReadOnly Then
            CurrentBill.ReadOnly = True
            CurrentVoteID.ReadOnly = True
        Else
            CurrentBill.ReadOnly = False
            CurrentVoteID.ReadOnly = False
        End If
        rollcall = False
    End Sub

    Private Sub btnUnlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnlock.Click
        lblVotingLight.Text = "Continue Next Bill"
        lblVotingLight.BackColor = Color.LightGreen
        Me.CurrentMotion.Enabled = True
        Me.CurrentVoteID.Enabled = True
        Me.CurrentBill.Enabled = True
        Me.CurrentBill.ReadOnly = False
        Me.CurrentMotion.ReadOnly = False
        Me.CurrentVoteID.ReadOnly = False
        EnableButtons()
        If SDisplay_On Then
            Me.CurrentBill.ReadOnly = True
        Else
            Me.CurrentBill.ReadOnly = False
            frmBuildBill.Show()
        End If
        rollcall = False
    End Sub

    Private Sub PingComputers()
        Dim Ping As Ping
        Dim pReply As PingReply
        Dim mes As String = "Ping Computers"
        SVotePC_On = True

        Try
            Ping = New Ping
            pReply = Ping.Send(gDisplay_IPAddress)
            If pReply.Status = IPStatus.Success Then
                SDisplay_On = True
                gOnlyOnePC = False
            Else
                SDisplay_On = False
                gOnlyOnePC = True
                V_DataSet("Delete From tblOnlyOnePC", "D")
                V_DataSet("Insert Into tblOnlyOnePC Values (1, '" & SVOTE & "','', '')", "A")

                If DisplayMessage("Senate Chamber Display Computer is offline. Would you want to continue to vote?", "Chamber Display PC is Offline", "Y") Then
                    LockReadOnly = False
                Else
                    '--- before shotdown update work area value to database
                    '!!!! do I need??? - UpdateLastVoteIDAndLastLegDay(Me.CurrentVoteID.Text)
                    Application.Exit()
                End If
            End If
        Catch
            Exit Sub
        End Try
    End Sub

    Public Function SendVotesToLocal() As Integer
        '--- if woody and buzz are dead, write votes data to local for late write to ALIS
        Dim k As Integer
        Dim VoteOID As Long, WrkFld As String, SenatorOID As Long, Vote As String
        Dim DistrictOID As Long
        Dim strAdd, txt As String
        Dim ds As New DataSet
        Dim dr As DataRow

        Try
            FileOpen(1, gWriteToAlisVoteTable & "-" & gVoteID & ".txt", OpenMode.Output)
            FileOpen(2, gWriteToAlisIndividualVoteTable & "-" & gVoteID & ".txt", OpenMode.Output)

            strSQL = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & "  And AlisVoteOID=0"
            ds = V_DataSet(strSQL, "R")

            For Each dr In ds.Tables(0).Rows
                '--- add vote header and retrieve the generated OID
                strAdd = gSenateID & "|" & dr("LegislativeDayOID") & "|" & dr("VoteID") & "|" & dr("TotalNay") & "|" & dr("TotalYea") & "|" & dr("TotalAbstain") & "|" & dr("TotalPass")
                Print(1, strAdd & vbCrLf)

                '--- beacuse wood and buzz are down, can't retreive ALISVoteID. Set VoteOID = 0 first 
                VoteOID = 0

                '--- now add senator votes, pick out vote, districtOID, and senatorOID from this string
                WrkFld = dr("SenatorVotes")
                k = 0
                Do
                    k = k + 1
                    WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1) ' skip senator name
                    Vote = Microsoft.VisualBasic.Left(WrkFld, 1)
                    WrkFld = Mid$(WrkFld, 3)
                    DistrictOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, "*") - 1))
                    WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1)
                    SenatorOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, ";") - 1))
                    txt = SenatorOID & "||" & VoteOID & "|" & DistrictOID & ", " & Vote
                    Print(2, txt & vbCrLf)

                    If InStr(WrkFld, ";") = Len(WrkFld) Then
                        Exit Do
                    End If
                    WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
                Loop
                V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & " WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")
            Next
            FileClose(1)
            FileClose(2)
            ds.Dispose()
        Catch ex As Exception
            DisplayMessage(ex.Message, "Write Votes On Local", "S")
            Exit Function
        Finally
            FileClose(1)
            FileClose(2)
            ds.Dispose()
        End Try
    End Function

    Private Sub btnShortVote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShortVote.Click
        '--- disable recall votes since that would give more votes than allowed
        '--- (set in params) for a short vote

        If Me.btnShortVote.BackColor = Color.Silver Then
            '--- see if already too many votes taken
            If GetCounts() > gAllowedShortVoteCnt Then
                If Not DisplayMessage("More than the allowed short vote count of " & gAllowedShortVoteCnt & " votes have been taken. Do you want to " & _
                                      "clear the vote panel so a short vote can be taken? if not, then you must manually clear the votes.", "Too Many Votes Taken", "Y") Then
                    Exit Sub
                Else
                    InitializeForm("Clear Only")
                End If
            End If

            Me.btnShortVote.BackColor = Color.Green
            ShortVote = True
            Me.btnRecall_1.Enabled = False
            Me.btnRecall_2.Enabled = False
            Me.btnRecall_3.Enabled = False

            Me.btnSet_1.Enabled = False
            Me.btnSet_2.Enabled = False
            Me.btnSet_3.Enabled = False
        Else
            Me.btnShortVote.BackColor = Color.Silver
            ShortVote = False
            Me.btnRecall_1.Enabled = True
            Me.btnRecall_2.Enabled = True
            Me.btnRecall_3.Enabled = True

            Me.btnSet_1.Enabled = True
            Me.btnSet_2.Enabled = True
            Me.btnSet_3.Enabled = True
        End If
    End Sub

    Private Sub btnRecallPreviousVote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecallPreviousVote.Click
        rollcall = True
        RecallVote(CInt(Me.CurrentVoteID.Text) - 1)
    End Sub

    Private Sub Bill_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles Bill.SelectedIndexChanged
        Me.CurrentBill.Text = ""
        Me.CurrentBill.Text = Me.Bill.Text
    End Sub

    Private Sub Phrases_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Phrases.SelectedIndexChanged
        If Strings.Right(Me.CurrentMotion.Text, 2) <> vbCrLf And Me.CurrentMotion.Text <> "" Then
            Me.CurrentMotion.Text = Me.CurrentMotion.Text & vbCrLf
        End If
      
        If InStr(Me.Phrases.Text, "-") > 0 Then
            InsertIntoWorkData(Mid(Me.Phrases.Text, InStr(Me.Phrases.Text, "-") + 2), gPhraseInsertionPoint)
        Else
            InsertIntoWorkData(Mid(Me.Phrases.Text, 1), gPhraseInsertionPoint)
        End If
    End Sub

    Public Sub InsertIntoWorkData(ByVal Data As String, ByVal InsertionPoint As String)
        Dim i As Integer

        '-- put stuff into the CurrentMotion.text at the insertion point; if this is
        '-- a phrase then append if no insertion point found; if the current
        '-- calendar is the motion calendar, then insert into the current bill area,
        '-- then try the work area

        '--- insert into CurrentMotion textbox
        i = InStr(NToB(Me.CurrentMotion.Text), InsertionPoint)

        If i > 0 Then
            Me.CurrentMotion.Text = Mid(Me.CurrentMotion.Text, 1, i - 1) & Data & Mid(Me.CurrentMotion.Text, i + 1)
        ElseIf InsertionPoint = gPhraseInsertionPoint Then      '--- append the phrase
            Me.CurrentMotion.Text = Me.CurrentMotion.Text & Data
            If Len(Me.CurrentMotion.Text) > 1 Then
                If Strings.Right(Me.CurrentMotion.Text, 2) <> vbCrLf Then
                    Me.CurrentMotion.Text = Me.CurrentMotion.Text & vbCrLf
                End If
            End If
        End If

        '--- if senator insertion point is in the phrase, or bill area for the motion calendar, then pull down the senator list
        If (InsertionPoint = gPhraseInsertionPoint) And (InStr(Data, gSenatorInsertionPoint)) Then
            Me.Senators.Focus()
            SendKeys.Send("{F4}")
        End If
    End Sub

    Private Sub Senators_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Senators.SelectedIndexChanged
        InsertIntoWorkData("Senator " & Me.Senators.Text, gSenatorInsertionPoint)
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        V_DataSet("Update tblOnlyOnePC set tmpBill=''", "U")
        Me.Bill.SelectedIndex = 0
        Me.CurrentBill.Text = ""
        Me.CurrentBill.Focus()
    End Sub

    Private Sub btnClearMotion_Click(sender As System.Object, e As System.EventArgs) Handles btnClearMotion.Click
        V_DataSet("Update tblOnlyOnePC set tmpMotion=''", "U")
        Me.Phrases.SelectedIndex = 0
        Me.Senators.SelectedIndex = 0
        Me.CurrentMotion.Text = ""
        Me.CurrentMotion.Focus()
    End Sub

    Private Sub btnStartService_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartService.Click
        Dim p As New Process
        p = Process.Start(gCmdFile)
        p.WaitForExit()
    End Sub

    Private Sub RecallVote(ByVal VoteID As Integer)
        Dim boxControl As Control
        Dim senatorControl As Control
        Dim ds As New DataSet
        Dim tmpBill As String = ""
        Dim tmpMotion As String = ""

        Try

            '--- Signal chamber that voting has stared
            If gVotingStarted Then
            Else
                gVotingStarted = True
                gPutCalendarCode = gCalendarCode
                gPutCalendar = gCalendar
                gPutBill = gBill
                gPutPhrase = gCurrentPhrase
                gPutChamberLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red)
                gPutVotingLight = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green)
            End If

            For Each boxControl In Controls
                If boxControl.BackColor = Color.Yellow Then
                    SenatorIndex = 1
                    VoteOrderIndex = 1
                End If
            Next

            For Each boxControl In Controls
                For j As Integer = 0 To gNbrSenators - 1
                    If boxControl.Name = "_Box_" & j Then
                        boxControl.Visible = False
                    End If
                Next
            Next
            ' Me._Box_1.Visible = True

            ds = V_DataSet("Select * From tblRollCallVotes WHERE SessionID =" & gSessionID & " AND VoteID = " & VoteID, "R")
            If ds.Tables(0).Rows.Count = 0 Then
                DisplayMessage("This Vote ID does not exist. Please set this Vote ID to different #.", "", "S")
                Exit Sub
            Else
                For Each dr As DataRow In ds.Tables(0).Rows
                    WrkFld = dr("SenatorVotes")
                    tmpMotion = dr("Motion")
                    tmpBill = dr("Bill")
                    gVoteID = dr("VoteID")
                Next
            End If

            '---pick out senators and their votes from this "WrkFkd" string; names in the vote rec are in district order,
            '---do must xref them to name order so they will print in name order
            k = 0
            Dim x As Integer
            Do
                k += 1
                For i As Integer = 0 To gNbrSenators - 1
                    If gSenatorNameOrder(i) = Mid(WrkFld, 1, InStr(WrkFld, "*") - 1) Then
                        x = i
                        Exit For
                    End If
                Next

                Select Case Mid(WrkFld, InStr(WrkFld, "*") + 1, 1)
                    Case "A"
                        For Each senatorControl In Controls
                            If senatorControl.Name = "Senator_" & x + 1 Then
                                senatorControl.BackColor = Color.Blue
                            End If
                        Next
                    Case "N"
                        For Each senatorControl In Controls
                            If senatorControl.Name = "Senator_" & x + 1 Then
                                senatorControl.BackColor = Color.Red
                            End If
                        Next
                    Case "Y"
                        For Each senatorControl In Controls
                            If senatorControl.Name = "Senator_" & x + 1 Then
                                senatorControl.BackColor = Color.Green
                            End If
                        Next
                    Case "P"
                        For Each senatorControl In Controls
                            If senatorControl.Name = "Senator_" & x + 1 Then
                                If reCall = False Then
                                    senatorControl.BackColor = Color.Yellow
                                End If
                                '  senatorControl.BackColor = Color.Yellow
                            End If
                        Next
                End Select

                If (InStr(WrkFld, ";") = WrkFld.Length) Or (k = gNbrSenators) Then
                    Exit Do
                End If
                WrkFld = Mid(WrkFld, InStr(WrkFld, ";") + 1)
            Loop

            gVoteID = GetLastLocalVoteID() + 1
            RetValue = GetCounts()

            reCall = False
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Recall Vote was Failed")
            Exit Sub
        Finally
            ds.Dispose()
        End Try
    End Sub

    Private Sub btnSet_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSet_1.Click
        index = 1
        gIndex = index
        vSet = True

        setR1 = Me.VoteID_1.Text
        setR2 = Me.VoteID_2.Text
        setR3 = Me.VoteID_3.Text
        frmBuildVoteIDnew.Close()
        frmBuildVoteIDnew.MdiParent = frmMain
        frmBuildVoteIDnew.Show()
        frmBuildVoteIDnew.BringToFront()
    End Sub

    Private Sub btnSet_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSet_2.Click
        index = 2
        gIndex = index
        vSet = True

        setR1 = Me.VoteID_1.Text
        setR2 = Me.VoteID_2.Text
        setR3 = Me.VoteID_3.Text
        frmBuildVoteIDnew.Close()
        frmBuildVoteIDnew.MdiParent = frmMain
        frmBuildVoteIDnew.Show()
        frmBuildVoteIDnew.BringToFront()
    End Sub

    Private Sub btnSet_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSet_3.Click
        index = 3
        gIndex = index
        vSet = True

        setR1 = Me.VoteID_1.Text
        setR2 = Me.VoteID_2.Text
        setR3 = Me.VoteID_3.Text
        frmBuildVoteIDnew.Close()
        frmBuildVoteIDnew.MdiParent = frmMain
        frmBuildVoteIDnew.Show()
        frmBuildVoteIDnew.BringToFront()
    End Sub

    Private Sub btnRecall_1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecall_1.Click
        If Me.VoteID_1.Text = Me.CurrentVoteID.Text Then
        ElseIf Me.VoteID_1.Text > 0 Then
            RecallVote(CInt(Me.VoteID_1.Text))
        End If
    End Sub

    Private Sub btnRecall_2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecall_2.Click
        If Me.VoteID_2.Text = Me.CurrentVoteID.Text Then
        ElseIf Me.VoteID_2.Text > 0 Then
            RecallVote(CInt(Me.VoteID_2.Text))
        End If
    End Sub

    Private Sub btnRecall_3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRecall_3.Click
        If Me.VoteID_3.Text = Me.CurrentVoteID.Text Then
        ElseIf Me.VoteID_3.Text > 0 Then
            RecallVote(CInt(Me.VoteID_3.Text))
        End If
    End Sub

    Private Sub RequestVoteIDTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RequestVoteIDTimer.Tick
        Try
            Dim mPath As String = gRequestVoteIDQueue
            If (MessageQueue.Exists(mPath)) = False Then
                MessageQueue.Create(mPath)
            End If

            Dim queue As New MessageQueue(mPath)
            queue.Formatter = New XmlMessageFormatter(New String() {"System.String"})
            Dim qenum As MessageEnumerator

            qenum = queue.GetMessageEnumerator2
            While qenum.MoveNext
                Dim m As Message = qenum.Current
                gVoteID = GetLastLocalVoteID() + 1
                If m.Label = "REQUESTVOTEID" Then
                    m = queue.Receive(New TimeSpan(1000))
                    SendMessageToDisplayPC("ANSWERVOTEID", gVoteID)
                End If
                SDisplay_On = True
            End While

            queue.Close()
        Catch ex As Exception
            If ex.Message <> "Access to Message Queuing system is denied." Then
                Try
                    Dim p As New Process
                    p = Process.Start(gCmdFile)
                    p.WaitForExit()
                    ReceiveMessageFromDisplayPC()
                Catch
                    If DisplayMessage(ex.Message & " System can not communicate to Chamber Display PC! Would you want to continue work?", "Voting Systme", "Y") Then
                        SDisplay_On = False
                        Exit Sub
                    Else
                        End
                    End If
                    SDisplay_On = False
                End Try
            End If
        End Try
    End Sub

    Private Sub setRTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles setRTimer.Tick
        LoadRecallVote()
        If SDisplay_On = False Then
            LoadVoteBill()
        End If
    End Sub

    Private Sub LoadRecallVote()
        Dim recallvote As Integer
        Dim ds As New DataSet
        strSQL = "Select * From tblVotingParameters Where ucase(Parameter) = "

        Try
            Select Case gIndex
                Case 1
                    recallvote = QueryParameters(strSQL & "'RECALLVOTE1'").Tables(0).Rows(0).Item("ParameterValue")
                    Me.VoteID_1.Text = recallvote
                Case 2
                    recallvote = QueryParameters(strSQL & "'RECALLVOTE2'").Tables(0).Rows(0).Item("ParameterValue")
                    Me.VoteID_2.Text = recallvote
                Case 3
                    recallvote = QueryParameters(strSQL & "'RECALLVOTE3'").Tables(0).Rows(0).Item("ParameterValue")
                    Me.VoteID_3.Text = recallvote
                Case 4
                    ds = V_DataSet("Select * From tblBuiltVoteID", "R")
                    For Each dr As DataRow In ds.Tables(0).Rows
                        Me.CurrentVoteID.Text = dr("BuiltVoteID")
                        Exit For
                    Next
            End Select

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub LoadVoteBill()
        Dim dsB As New DataSet
        Try
            dsB = V_DataSet("Select tmpBill, tmpMotion From tblOnlyOnePC", "R")
            If dsB.Tables(0).Rows.Count > 0 Then
                For Each drB In dsB.Tables(0).Rows
                    If Me.CurrentBill.Text = "" Then
                        Me.CurrentBill.Text = drB("tmpBill")
                    End If
                    If Me.CurrentMotion.Text = "" Then
                        Me.CurrentMotion.Text = drB("tmpMotion")
                    End If
                    Exit For
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Failed Load Bill/Motion")
        Finally
            dsB.Dispose()
        End Try
    End Sub

    Private Sub btnCloseVote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseVote.Click
        Dim k, j, i As Integer
        Dim WrkFld, SenatorVotes As String
        Dim FieldName(15), FieldValue(15) As Object
        Dim PrintFieldValue(), PrintFieldName() As Object
        Dim VoteDate, VoteTime As Object
        Dim tmpVoteID As Object = Nothing
        Dim ds As New DataSet
        Dim strMotions As String = ""
        Dim strQ As String = ""

        Try
            gReSendALIS = False
            If Me.CurrentMotion.Text <> "" Then
                strMotions = Me.CurrentMotion.Text
            Else
                strMotions = ""
            End If


            '### 1 --- check it has current Vote ID and Bill
            If Me.CurrentVoteID.Text = "" Then
                DisplayMessage("Vote ID can not be empty.", "Close Vote", "S")
                gVoteID = Me.CurrentVoteID.Text
                tmpVoteID = Me.CurrentVoteID.Text
                Me.CurrentVoteID.Focus()
                Exit Sub
            End If

            If SDisplay_On Then
                Me.CurrentBill.ReadOnly = True
            Else
                Me.CurrentBill.ReadOnly = False
            End If
            lblVotingLight.BackColor = Color.Red
            lblVotingLight.Text = "Recording Vote"


            '### 2 ---  check all of senators voted
            If Val(Me.YeaCount.Text) + Val(Me.NayCount.Text) + Val(Me.AbstainCount.Text) + Val(Me.PassCount.Text) = 0 Then
                RetValue = DisplayMessage("The vote cannot be closed.  No votes have been taken.", "No Voting", "S")
                Exit Sub
            ElseIf ShortVote Then
                ShortVote = False
                btnShortVote.BackColor = Color.Silver
                Me.YeaCount.Text = CStr(25)         '--- set this to 25
                btnRecall_1.Enabled = True
                btnRecall_2.Enabled = True
                btnRecall_3.Enabled = True
            ElseIf Val(Me.YeaCount.Text) + Val(Me.NayCount.Text) + Val(Me.AbstainCount.Text) + Val(Me.PassCount.Text) < gNbrSenators Then
                If gVotingHelp Then
                    If Not DisplayMessage("Not All Senators Have Voted.  Should Those Votes Be Recorded As 'Pass'?", "Missing Votes", "Y") Then
                        Exit Sub
                    End If
                End If
                Me.PassCount.Text = CStr(gNbrSenators - CDbl(Me.YeaCount.Text) - CDbl(Me.NayCount.Text) - CDbl(Me.AbstainCount.Text))
            End If


            '### 3 --- now lock machine, get Vote date and time
            '---lock the form so nothing can be pushed
            Me.Cursor = Cursors.WaitCursor
            Me.ReceiveTimer.Stop()
            Me.ReceiveTimer.Enabled = False
            Me.lblVotingLight.Text = "Recording Vote"
            Me.lblVotingLight.BackColor = System.Drawing.Color.LimeGreen
            DisableButtons()
            Me.Refresh()
            VoteDate = Date.Now.ToShortDateString
            VoteTime = Date.Now.ToShortTimeString


            '### 4 --- set print and write vote detail
            '  --- clear PrintVoteHeader and PrintVoteDetail, before record them
            V_DataSet("Delete From tblPrintVoteHeader", "D")
            V_DataSet("Delete From tblPrintVoteDetail", "D")


            '### 5 --- Try save votes to local database for report
            Try
                '--- avoid the duplicate VoteID in tblRollCallVotes table
                Dim dsCheck As New DataSet
                strSQL = "Select * From tblRollCallVotes Where  SessionID =" & gSessionID & " AND VoteID =" & gVoteID
                dsCheck = V_DataSet(strSQL, "R")
                If dsCheck.Tables(0).Rows.Count > 0 Then
                    gVoteID = gVoteID + 1
                Else
                    gVoteID = Me.CurrentVoteID.Text
                End If

                '--- write print vote header first 
                If SDisplay_On Then
                    strSQL = "Insert Into tblPrintVoteHeader Values (" & CInt(gVoteID) & ", '" & gBill & "', '" & strMotions & "', '" & gLegislativeDay & "', '" & VoteDate & "', '" & VoteTime & "', " & CInt(Me.YeaCount.Text) & ", " & CInt(Me.NayCount.Text) & ", " & CInt(Me.AbstainCount.Text) & ", '" & gSessionName & "')"
                Else
                    gVoteID = CInt(Me.CurrentVoteID.Text)
                    If InStr(Me.CurrentBill.Text, " ") > 0 Then
                        gBill = Mid(Me.CurrentBill.Text, 1, InStr(Me.CurrentBill.Text, " ") - 1)
                    Else
                        gBill = Me.CurrentBill.Text
                    End If
                    strSQL = "Insert Into tblPrintVoteHeader Values (" & gVoteID & ", '" & UCase(gBill) & "', '" & strMotions & "', '" & gLegislativeDay & "', '" & VoteDate & "', '" & VoteTime & "', " & CInt(Me.YeaCount.Text) & ", " & CInt(Me.NayCount.Text) & ", " & CInt(Me.AbstainCount.Text) & ", '" & gSessionName & "')"
                End If
                ds = V_DataSet(strSQL, "A")

                ReDim PrintFieldValue(9)
                ReDim PrintFieldName(9)
                strSQL = "Select * From tblPrintVoteDetail"
                ds = V_DataSet(strSQL, "R")
                For k = 0 To 8
                    PrintFieldName(k) = ds.Tables(0).Columns.Item(k).ColumnName
                Next k

                If gBillNbr = "" Then
                    gBillNbr = 0
                End If

                FieldValue(0) = gSessionID
                If SDisplay_On Then
                    FieldValue(1) = gVoteID
                    FieldValue(2) = gBillNbr
                    FieldValue(3) = gBill
                    FieldValue(4) = Replace(strMotions, "'", "''")
                Else
                    FieldValue(1) = Me.CurrentVoteID.Text
                    If Me.CurrentBill.Text = "" Then
                        FieldValue(2) = ""
                        FieldValue(3) = ""
                    Else
                        If InStr(Me.CurrentBill.Text, " ") > 0 Then
                            FieldValue(2) = Mid(Me.CurrentBill.Text, 1, InStr(Me.CurrentBill.Text, " ") - 1)
                        Else
                            FieldValue(2) = Me.CurrentBill.Text
                        End If

                        FieldValue(3) = Me.CurrentBill.Text
                    End If
                    If Me.CurrentMotion.Text = "" Then
                        FieldValue(4) = ""
                    Else
                        FieldValue(4) = Replace(strMotions, "'", "''")
                    End If
                End If
                FieldValue(5) = gLegislativeDay
                FieldValue(6) = Me.YeaCount.Text
                FieldValue(7) = Me.NayCount.Text
                FieldValue(8) = Me.AbstainCount.Text
                FieldValue(9) = Me.PassCount.Text
                FieldValue(10) = VoteDate
                FieldValue(11) = VoteTime
                FieldValue(13) = gLegislativeDayOID
                FieldValue(14) = 0

                SenatorVotes = ""
                For j = 0 To gNbrSenators - 1
                    '--- j is the district # of the senator in gSenatorName, so find
                    '--- where the name matches in the alphabetic name order and that
                    '--- match (i) will be the index to the screen position of that senator
                    '--- so that i represets the location of the senator name and vote associated with district j
                    For i = 0 To gNbrSenators - 1
                        If gSenatorName(j) = gSenatorNameOrder(i) Then
                            Exit For
                        End If
                    Next i

                    Select Case senatorBoxes(i).BackColor
                        Case System.Drawing.Color.Green() : WrkFld = "Y"
                        Case System.Drawing.Color.Red() : WrkFld = "N"
                        Case System.Drawing.Color.Blue : WrkFld = "A"
                        Case System.Drawing.Color.Yellow : WrkFld = "P"
                        Case Else
                            WrkFld = "P"       '--- default to pass if not voting as set by ? above
                    End Select
                    SenatorVotes = SenatorVotes & gSenatorNameOrder(i) & "*" & WrkFld & "*" & Format(gDistrictOID(i)) & "*" & Format(gSenatorOID(i)) & ";"

                    '--- add senators/votes to print detail; use index j to keep the
                    '--- report in name order
                    PrintFieldValue(0) = gVoteID
                    PrintFieldValue(1) = gSenatorNameOrder(j)
                    PrintFieldValue(2) = ""   ' init all fields
                    PrintFieldValue(3) = ""
                    PrintFieldValue(4) = ""
                    PrintFieldValue(5) = ""
                    PrintFieldValue(6) = ""
                    PrintFieldValue(7) = ""
                    PrintFieldValue(8) = ""
                    Select Case senatorBoxes(j).BackColor
                        Case System.Drawing.Color.Green() : PrintFieldValue(3) = "Y"
                        Case System.Drawing.Color.Red : PrintFieldValue(2) = "N"
                        Case System.Drawing.Color.Blue : PrintFieldValue(4) = "A"
                    End Select

                    If j < gSenatorSplit - 1 Then
                        PrintFieldValue(5) = gSenatorNameOrder(j + gSenatorSplit)
                        Select Case senatorBoxes(j + gSenatorSplit).BackColor
                            Case System.Drawing.Color.Green() : PrintFieldValue(7) = "Y"
                            Case System.Drawing.Color.Red : PrintFieldValue(6) = "N"
                            Case System.Drawing.Color.Blue : PrintFieldValue(8) = "A"
                        End Select
                        strSQL = "Insert Into tblPrintVoteDetail Values (" & PrintFieldValue(0) & ", '" & PrintFieldValue(1) & "', '" & PrintFieldValue(2) & "', '" & PrintFieldValue(3) & "', '" & _
                               PrintFieldValue(4) & "', '" & PrintFieldValue(5) & "', '" & PrintFieldValue(6) & "', '" & PrintFieldValue(7) & "', '" & PrintFieldValue(8) & "')"
                        V_DataSet(strSQL, "A")
                    ElseIf j = gSenatorSplit - 1 Then
                        strSQL = "Insert Into tblPrintVoteDetail Values (" & PrintFieldValue(0) & ", '" & PrintFieldValue(1) & "', '" & PrintFieldValue(2) & "', '" & PrintFieldValue(3) & "', '" & _
                                PrintFieldValue(4) & "', '" & PrintFieldValue(5) & "', '" & PrintFieldValue(6) & "', '" & PrintFieldValue(7) & "', '" & PrintFieldValue(8) & "')"
                        V_DataSet(strSQL, "A")
                    End If
                Next j

                '### 6 --- write votes to local tblRollCallVotes table 
                If Me.chkQuorum.Checked = True Then
                    strQ = "Yes"
                Else
                    strQ = "No"
                End If
                FieldValue(12) = SenatorVotes
                strSQL = "Insert Into tblRollCallVotes  (SessionID, VoteID, BillNbr, Bill, Motion, LegislativeDay, TotalYea, Totalnay, TotalAbstain, TotalPass, VoteDate, VoteTime, SenatorVotes, legislativeDayOID,  ALISVoteOID, Quorum ) Values (" & FieldValue(0) & ", " & FieldValue(1) & ", '" & FieldValue(2) & "', '" & FieldValue(3) & "', '" & _
                            FieldValue(4) & "' , '" & FieldValue(5) & "', " & FieldValue(6) & ", " & FieldValue(7) & ", " & FieldValue(8) & ", " & _
                            FieldValue(9) & ", '" & FieldValue(10) & "',  '" & FieldValue(11) & "', '" & FieldValue(12) & "', " & FieldValue(13) & ", " & FieldValue(14) & ", '" & strQ & "')"
                V_DataSet(strSQL, "A")
            Catch ex As Exception
                DisplayMessage("Save votes to local database was failed!" & vbNewLine & "Voting System will shutdown. Please contact to Administrator.", "Voting System Shutdown", "S")
                Application.Exit()          '!!! --- save votes was failed. Voting process can not be continu. System will shutdown here
            End Try


            '### 7 --- update last voteID, current sessionID, sessionName and current LEG day             
            V_DataSet("Update  tblVotingParameters Set ParameterValue ='" & gVoteID & "' Where UCase(Parameter)='LASTVOTEIDFORTHISSESSION'", "U")
            V_DataSet("Update tblVotingParameters Set ParameterValue='" & gLegislativeDay & "' Where ucase(Parameter) ='LASTLEGISLATIVEDAY'", "U")
            V_DataSet("Update tblVotingParameters Set ParameterValue ='" & gSessionName & "' Where Parameter = 'LastSessionName'", "U")
            V_DataSet("Update tblVotingParameters Set ParameterValue ='" & gSessionID & "' Where Parameter = 'LastSessionID'", "U")


            '### 8 --- write vote detail to text file first incase needs to retreive it
            Try
                ' Dim txtContent As String = "LEGDay = " & FieldValue(5) & "; " & "SessionName = " & gSessionName & "; " & "VoteID = " & gVoteID & "; " & "Bill = " & Me.CurrentBill.Text & "; " & "Motion = " & Me.CurrentMotion.Text & "; " & vbCrLf & "VoteDate = " & VoteDate & "; " & "VoteTime = " & VoteTime & "; " & "TotalYea = " & Me.YeaCount.Text & "; " & "TotalNay = " & Me.NayCount.Text & "; " & "TotalAbstain = " & Me.AbstainCount.Text & "; " & vbNewLine & "SenatorsVote=" & FieldValue(12) & vbNewLine & vbNewLine
                Dim txtContent As String = "Session Name= " & gSessionName & "; " & "SessionID= " & FieldValue(0) & "; " & "VoteID= " & FieldValue(1) & "; " & "BillNbr= " & FieldValue(2) & "; " & "Bill= " & FieldValue(3) & "; " & _
                                       "Motion= " & FieldValue(4) & "; " & "Legislative Day= " & FieldValue(5) & "; " & "Total Yea= " & FieldValue(6) & "; " & "Total Nay= " & FieldValue(7) & "; " & "Total Abstain= " & FieldValue(8) & "; " & _
                                       "Total Pass= " & FieldValue(9) & "; " & "Vote Date= " & FieldValue(10) & ";" & "Vote Time= " & FieldValue(11) & "; " & "LegislativeDayID= " & FieldValue(13) & "; " & _
                                       "TEST= " & FieldValue(14) & "; " & "ALISVoteOID= " & FieldValue(15) & "; " & "Quorum= " & strQ & vbNewLine & "SenateVotes= " & FieldValue(12) & "; " & vbNewLine & vbNewLine
                If File.Exists(gVotingPath & "Vote.txt") = False Then
                    Dim fs As New FileStream(gVotingPath & "Vote.txt", FileMode.CreateNew)
                    fs.Close()
                End If
                FileOpen(1, gVotingPath & "Vote.txt", OpenMode.Append)
                Print(1, txtContent)
                FileClose(1)
            Catch ex As Exception
                FileClose(1)
                GoTo continueReport
            Finally
                FileClose(1)
            End Try


continueReport:
            '### 9 --- write Crystal Report, print out hard copy 
            Try
                If gPrintVoteRpt Then
                    If gTestMode = False Then
                        LoadReport("rptVote.rpt")
                    Else
                        LoadReport("rptVoteTEST.rpt")
                    End If
                End If
            Catch ex As Exception
                '---try print report again
                Try
                    If gPrintVoteRpt Then
                        If gTestMode = False Then
                            LoadReport("rptVote.rpt")
                        Else
                            LoadReport("rptVoteTEST.rpt")
                        End If
                    End If
                Catch exS As Exception
                    DisplayMessage("Print Votes Report is failed! " & vbNewLine & "System will continue vote process without printing report.", "Failed Print Report", "I")
                    GoTo continueWriteVoteToAlis
                End Try
                GoTo continueWriteVoteToAlis
            End Try


continueWriteVoteToAlis:

            '### 10 --- write vots data to ALIS
            If gNetwork Then
                Try
                    '---send votes to ALIS production
                    If gTestMode = False Then
                        SendVotesToALIS()
                    Else
                        '---send votes to ALIS_TEST
                        If gWriteVotesToTest Then
                            SendVotesToALIS()
                        End If
                    End If
                Catch
                    Try
                        '---try send votes to ALIS production again
                        If gTestMode = False Then
                            SendVotesToALIS()
                        Else
                            If gWriteVotesToTest Then           '---send votes to ALIS_TEST
                                SendVotesToALIS()
                            End If
                        End If
                    Catch ex As Exception
                        gNetwork = False
                        DisplayMessage(ex.Message & vbCrLf & "Write Votes To ALIS Was Abbored!", "Write Votes To ALIS", "S")
                        GoTo continueSendClosedVoteMess
                    End Try
                    GoTo continueSendClosedVoteMess
                End Try
            Else
                DisplayMessage("Unable Connecte to ALIS database! Write Votes to ALIS Was Abbored. Please remember to run 'Send Vote to ALIS' late.", "Write Votes To ALIS", "S")
            End If


continueSendClosedVoteMess:

            '### 11 --- SEND Closed Vote message to Chamber Display PC. If it is on
            If SDisplay_On Or gOnlyOnePC = False Then
                Try
                    SendMessageToDisplayPC("CLOSEDVOTE", Trim(CStr(gVoteID)) & "||" & Me.CurrentBill.Text)
                Catch ex As Exception
                    '---  re-run Queue service, assolate message queue problem
                    Dim p As New Process
                    p = Process.Start(gCmdFile)
                    p.WaitForExit()
                    Try
                        '---  ping: Display Computer - Sara's PC, assolate network communication problem
                        Dim PingCD As Ping = Nothing
                        Dim pReply As PingReply

                        pReply = PingCD.Send(gDisplay_IPAddress)
                        If pReply.Status = IPStatus.Success Then
                            SDisplay_On = True
                            SendMessageToDisplayPC("CLOSEDVOTE", Trim(CStr(gVoteID)) & "||" & Me.CurrentBill.Text)
                            Exit Try
                        Else
                            SDisplay_On = False
                            If Not DisplayMessage("Chamber Display PC is offline. Would you like continue voting job without 'Chamber Display'?", "Chamber Display PC Is Offline", "Y") Then
                                End
                            End If
                        End If
                    Catch
                        GoTo continueInitialize
                    End Try
                    GoTo continueInitialize
                End Try
            End If


continueInitialize:

            '### 12 --- Initialize vote window   
            Me.lblVotingLight.BackColor = Color.LightGreen
            Me.lblVotingLight.Text = "Waiting Next Bill"
            EnableButtons()

            Try
                If SDisplay_On = False Then
                    Me.CurrentBill.Text = ""
                    Me.CurrentMotion.Text = ""
                    Me.Senators.Text = ""
                    Me.Phrases.Text = ""
                    btnClear_Click(btnClear, Nothing)
                End If
                V_DataSet("Delete From tblOnlyOnePc", "D")
                gAction = ""
                InitializeForm("")

                '--- set next vote ID by increased 1
                If SDisplay_On = False Then
                    gVoteID = GetLastLocalVoteID() + 1
                End If

                Me.CurrentVoteID.Text = gVoteID
                Me.Cursor = Cursors.Default
            Catch ex As Exception
                Try
                    GoTo continueInitialize
                Catch exI As Exception
                    tmpVoteID = tmpVoteID + 1
                End Try
            Finally
                Me.Cursor = Cursors.Default
                Me.ReceiveTimer.Enabled = True
                Me.ReceiveTimer.Interval = gReceiveQueueTimer
                Me.ReceiveTimer.Start()
            End Try

        Catch ex As Exception
            DisplayMessage(ex.Message, "Close Vote", "I")
        Finally
            gAction = ""
            rollcall = False
            InitializeForm("")
            SenatorIndex = 0
            ds.Dispose()
            Me.Cursor = Cursors.Default
            Me.ReceiveTimer.Enabled = True
            Me.ReceiveTimer.Interval = gReceiveQueueTimer
            Me.ReceiveTimer.Start()
        End Try
    End Sub

    Private Sub LoadReport(rptName As String)
        '--- You can change more print options via PrintOptions property of ReportDocument
        Dim report1 As New ReportDocument()
        Dim report2 As New ReportDocument()

        Try
            '--- Production
            ' ''--- use the proper report to pinter based upon who is alive
            ' SetDefaultPrinter("HP LaserJet P3010 Series UPD PCL 6 (Copy 1)", "", "")
            ' SetDefaultPrinter(gDefaultPrinter, "", "")
            SetDefaultPrinter(gPrimaryPrinter, "", "")
            report1.Load(gVotingPath & rptName)
            report1.PrintToPrinter(1, True, 0, 0)
            report1.Close()

            '--- print rpt to another printer if one selected
            If gSecondaryPrinter <> "" Then
                Dim p As Printing.PrinterSettings
                p = New Printing.PrinterSettings()

                For x As Integer = 0 To Printing.PrinterSettings.InstalledPrinters.Count - 1
                    If UCase(Printing.PrinterSettings.InstalledPrinters.Item(x)) = UCase(gSecondaryPrinter.Trim) Then
                        '--- now change default printe to secondary printer
                        SetDefaultPrinter(Printing.PrinterSettings.InstalledPrinters.Item(x), "", "")
                        report1.Load(gVotingPath & rptName)
                        report1.PrintToPrinter(1, True, 0, 0)
                        report1.Close()
                    End If
                Next
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message & vbNewLine & "Or please make sure this PC is attached to a printer.", "Execute: LoadReport()", "S")
            Exit Sub
        End Try
    End Sub

#Region "Set controles color"

    Private Sub setBox(ByVal index As Integer)
        For j As Integer = 0 To gNbrSenators - 1
            labelBoxes(j).Visible = False
        Next
        labelBoxes(index).Visible = True
    End Sub

    Private Sub Senator_1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_1.GotFocus
        SenatorIndex = 0
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_2.GotFocus
        SenatorIndex = 1
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_3_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_3.GotFocus
        SenatorIndex = 2
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_4_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_4.GotFocus
        SenatorIndex = 3
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_5_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_5.GotFocus
        SenatorIndex = 4
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_6_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_6.GotFocus
        SenatorIndex = 5
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_7_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_7.GotFocus
        SenatorIndex = 6
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_8_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_8.GotFocus
        SenatorIndex = 7
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_9_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_9.GotFocus
        SenatorIndex = 8
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_10_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_10.GotFocus
        SenatorIndex = 9
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_11_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_11.GotFocus
        SenatorIndex = 10
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_12_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_12.GotFocus
        SenatorIndex = 11
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_13_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_13.GotFocus
        SenatorIndex = 12
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_14_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_14.GotFocus
        SenatorIndex = 13
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_15_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_15.GotFocus
        SenatorIndex = 14
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_16_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_16.GotFocus
        SenatorIndex = 15
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_17_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_17.GotFocus
        SenatorIndex = 16
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_18_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_18.GotFocus
        SenatorIndex = 17
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_19_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_19.GotFocus
        SenatorIndex = 18
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_20_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_20.GotFocus
        SenatorIndex = 19
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_21_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_21.GotFocus
        SenatorIndex = 20
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_22_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_22.GotFocus
        SenatorIndex = 21
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_23_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_23.GotFocus
        SenatorIndex = 22
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_24_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_24.GotFocus
        SenatorIndex = 23
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_25_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_25.GotFocus
        SenatorIndex = 24
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_26_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_26.GotFocus
        SenatorIndex = 25
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_27_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_27.GotFocus
        SenatorIndex = 26
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_28_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_28.GotFocus
        SenatorIndex = 27
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_29_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_29.GotFocus
        SenatorIndex = 28
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_30_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_30.GotFocus
        SenatorIndex = 29
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_31_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_31.GotFocus
        SenatorIndex = 30
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_32_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_32.GotFocus
        SenatorIndex = 31
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_33_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_33.GotFocus
        SenatorIndex = 32
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_34_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_34.GotFocus
        SenatorIndex = 33
        setBox(SenatorIndex)
    End Sub

    Private Sub Senator_35_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Senator_35.GotFocus
        SenatorIndex = 34
        setBox(SenatorIndex)
    End Sub

#End Region
   
End Class