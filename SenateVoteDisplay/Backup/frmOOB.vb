Option Explicit Off

Imports System
Imports System.Text
Imports System.DateTime
Imports System.IO
Imports System.Net
Imports System.ComponentModel
Imports System.Data
Imports System.Data.OleDb
Imports Microsoft.VisualBasic
Imports System.DBNull
Imports System.Net.NetworkInformation
Imports System.Messaging
Imports System.Timers
Imports System.Net.Sockets
Imports System.Runtime.Serialization
Imports System.Drawing

Public Class frmOOB
    Private askVoteID As Boolean = False

    Private Sub getQueueTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReceiveTimer.Tick
        If SPrimary_On Or SSecondary_On Or SDisplay_On Then
            ReceiveMessageFromQueue()
        End If
    End Sub

    Private Sub btnVote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVote.Click
        If SPrimary_On Or SSecondary_On Or SDisplay_On Then
            askVoteID = False
            lblChamberLight.Visible = True
            lblChamberLight.BackColor = Color.Red
            lblChamberLight.Text = "Waiting for Voting"
            ReceiveTimer.Enabled = False
            ReceiveTimer.Stop()
            'ReceiveTimer.Interval = 1000


            If cboDisplay.Text <> "Start Vote" Then
                cboDisplay.Text = "Start Vote"
            End If

            Select Case Trim(cboCalendar.Text)
                Case "Regular Order"
                    bodyText = Mid(cboBill.Text, 1, InStr(cboBill.Text, " ") - 1)
                Case "Local Bills"
                    bodyText = Mid(cboBill.Text, 1, InStr(cboBill.Text, " ") - 1)
                Case "Other"
                    bodyText = Mid(cboBill.Text, 1, InStr(cboBill.Text, " ") - 1)
                Case "Confirmations"
                    bodyText = Mid(cboBill.Text, 1, InStr(cboBill.Text, "-") - 2)
                Case "Motions"
                    bodyText = cboBill.Text
            End Select

            '  If display PC lost voting information, it will request SOOB1 or SOOB2 resend information to display. 
            '  so write to text file first. SOOB PC is able to retreive voting infromation from the text file and respons the display PC request

            bodyParamters = "gCalendar - " & Me.cboCalendar.Text & "||" & "gBill - " & Me.cboBill.Text & "||" & "gCurrentPhrase - " & cboPhrase.Text
            If File.Exists(gVotingPath & gDisplayTextFile) = False Then
                Dim fs As New FileStream(gVotingPath & gDisplayTextFile, FileMode.CreateNew)
                fs.Close()
            End If
            FileOpen(1, gVotingPath & gDisplayTextFile, OpenMode.Output)
            Print(1, bodyParamters)
            FileClose(1)


            '   send voting information to SPrimary or SSecondary PC
            bodyParamters = ""
            bodyParamters = "gVoteID - " & Me.txtVoteID.Text & "||" & "gCalendar - " & Me.cboCalendar.Text & "||" & "gBill - " & Me.cboBill.Text & "||" & "gCurrentPhrase - " & cboPhrase.Text
            If SPrimary_On Or SSecondary_On Then
                'SendMessageToQueue("STARTVOTE", bodyParamters)
                SendMessageToQueue(DisplayTitle(cboDisplay.Text), bodyParamters)
            End If



            '   send voting information to display PC 
            'bodyParamters = ""
            'bodyParamters = "gCalendar - " & Me.cboCalendar.Text & "||" & "gBill - " & Me.cboBill.Text & "||" & "gCurrentPhrase - " & cboPhrase.Text
            If SDisplay_On Then
                'SendMessageToDisplay(UCase(cboDisplay.Text), bodyParamters, gSendVoteQueueToDisplay)
                SendMessageToDisplay(DisplayTitle(cboDisplay.Text), bodyParamters, gSendVoteQueueToDisplay)
            End If


            If gNetwork Then
                '  write to server and local
                WriteHTML(gHTMLFile)
                WriteHTML(gVotingPath & "CurrentMetter.htm")
            Else
                '  write to local only
                WriteHTML(gVotingPath & "CurrentMetter.htm")
            End If
            ''btnVote.Enabled = False
            ''lblChamberLight.BackColor = Color.Red
            ''lblChamberLight.Text = "Waiting Vote!"

            '   Update LastVoteID
            ' str = "Update tblVotingParameters Set ParameterValue='" & gVoteID & "' Where Parameter='LastVoteIDForThisSession'"
            str = "Update tblVotingParameters Set ParameterValue='" & txtVoteID.Text & "' Where Parameter='LastVoteIDForThisSession'"
            V_DataSet(str, "U")


           
        Else
            If SPrimary_On = False And SSecondary_On = False Then
                MsgBox("Senate Voting Parimary and Secondary PCs are offline. Unable send voting information to it. Please contact to Administrator.", MsgBoxStyle.Critical, "Senate Voting System")
                '!!! write vote information on local 
            ElseIf SDisplay_On = False Then
                MsgBox("Senate voting display borad computer is off. Unable to change the background.", MsgBoxStyle.Critical, msgText)
            End If
        End If
    End Sub

    Private Sub frmOOB_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim str As String
        Dim ds As New DataSet
        Dim dsC As New DataSet
        Dim dsB As New DataSet
        Dim dr As DataRow

        Main()

        If gTestMode Then
            lblTestMode.Visible = True
        End If


        '--- initialize calendar and bills   
        '--- str = "Select Calendar, CalendarCode From tblCalendars Where CalendarCode='1'"
        str = "Select Calendar, CalendarCode From tblCalendars  Order By CalendarCode"
        ds = V_DataSet(str, "R")
        For Each dr In ds.Tables(0).Rows
            cboCalendar.Items.Add(dr("Calendar"))
        Next

        ' --- Load Senators from gSenatorName array in to SenatorName comboBox
        For i As Integer = 1 To gSenatorName.Length - 1
            cboSenator.Items.Add(gSenatorName(i))
        Next


        '--- Load  Phases from gThePhrases(i) array in to Phases comboBox. Notic: gThePhrases(i) array included PhraseCode and Phrase, gPhrases has only Phrases string in array
        LoadPhrasesIntoArray()
        For k As Integer = 1 To gThePhrases.Length - 1
            cboPhrase.Items.Add(gThePhrases(k))
        Next

        If gTestMode = False Then
            '--- Put last vote id + 1
            txtVoteID.Text = gVoteID + 1
        Else
            txtVoteID.Text = 0
        End If


        ' clear all of messages from senatevotequeue
        Dim mq As New MessageQueue(gSendQueueName)
        Dim msg As New Message
        mq.Purge()

        ' clear all of messages from senatevotequeue
        Dim mq2 As New MessageQueue(gReceiveQueueName)
        Dim msg2 As New Message
        mq2.Purge()

        If SPrimary_On Or SSecondary_On Then
            ReceiveTimer.Enabled = True
            ReceiveTimer.Start()
            ReceiveTimer.Interval = CInt(gReceiverQueueTimer)

            PingTimer.Enabled = False
            PingTimer.Stop()
        ElseIf SPrimary_On = False And SSecondary_On = False Then

            ReceiveTimer.Enabled = False
            ReceiveTimer.Stop()

            '!!! Using threading to ping to avoid lock process
            'PingTimer.Enabled = True
            'PingTimer.Start()
            'PingTimer.Interval = 1000000
            'PingComputer()
        End If

        cboDisplay.Text = "START VOTE"
    End Sub


    Private Sub PingComputer()
        Dim RetValue As New Object
        Dim Ping As Ping
        Dim pReply As PingReply
        Dim mes As String = "Ping Computers"

        Ping = New Ping


        '   ping Voting System Paramery Computer
        pReply = Ping.Send(gPrimary_IPAddress)
        If pReply.Status = IPStatus.Success Then
            SPrimary_On = True
            ' RetValue = DisplayMessage("Senate Voting Primary Computer Is On Again.", mes, "I")
        Else
            SPrimary_On = False
            ' RetValue = DisplayMessage("Senate Voting Primary Computer Is Off.", mes, "I")
        End If

        '   ping Voting System Secondary Computer
        pReply = Ping.Send(gSecondary_IPAddress)
        If pReply.Status = IPStatus.Success Then
            SSecondary_On = True
            ' RetValue = DisplayMessage("Senate Voting Secondary Computer Is On Again.", mes, "I")
        Else
            SSecondary_On = False
            ' RetValue = DisplayMessage("Senate Voting Seconday Computer Is Off.", mes, "I")
        End If

        If SPrimary_On Or SSecondary_On Then
            lblChamberLight.Visible = True
            lblChamberLight.BackColor = Color.Red
            lblChamberLight.Text = "Waiting to Vote"
            btnVote.Enabled = True

            PingTimer.Enabled = False
            PingTimer.Stop()

            ReceiveTimer.Enabled = True
            ReceiveTimer.Start()
            ReceiveTimer.Interval = gReceiverQueueTimer
        ElseIf SPrimary_On = False And SSecondary_On = False Then
            PingTimer.Enabled = True
            PingTimer.Start()
            PingTimer.Interval = 10000000

            ReceiveTimer.Enabled = False
            ReceiveTimer.Stop()

            lblChamberLight.Visible = True
            lblChamberLight.BackColor = Color.Red
            lblChamberLight.Text = "Unable Communicate to Senate Voting Computers."
            ' RetValue = DisplayMessage("Unable Communicate to Senate Voting Computers.", mes, "I")
            btnVote.Enabled = False
        End If
    End Sub

    Private Sub btnHTML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHTML.Click
        If gNetwork Then
            WriteHTML(gHTMLFile)                                'write to serever
            WriteHTML(gVotingPath & "CurrentMatter.htm")        'write to local
        Else
            WriteHTML(gVotingPath & "CurrentMatter.htm")        'write to local only
        End If
    End Sub

    Private Sub WriteHTML(ByVal HTMLFile As String)
        Dim ds As New DataSet
        Dim WrkFld, str As String, RetValue As Object

        Try

            str = "Select * From tblOrderOfBusiness"
            ds = V_DataSet(str, "R")


            FileOpen(1, HTMLFile, OpenMode.Output)
            Print(1, "<HTML>")
            Print(1, "<meta http-equiv='refresh' content='4'/>")
            Print(1, "<Body><p>&nbsp;</p> ")

            Print(1, "<table align='center' width=100%>")
            Print(1, "<tr align='center'><td></td>")
            Print(1, "<td align='center'>")

            Print(1, "<H1><Center><B><U><FONT FACE=" & Chr(34) & "Arial" & Chr(34) & " SIZE=" & Chr(34) & "4" & Chr(34) & " COLOR=" & Chr(34) & "Black" & Chr(34) & ">Order Of Business</U></B><BR><BR>")

            Print(1, "</td></tr></table>")

            If ClearDisplayFlag = False Then
                Print(1, "</Center></H1>-------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
                Print(1, "<Font Color=" & Chr(34) & "Blue" & Chr(34) & ">" & Me.cboBill.Text & "</Font><BR>")
                Print(1, "<BR>")

                Print(1, "<BR>-------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
                Print(1, "<BR><font size='3' face='Arial' ><b>")
                WrkFld = Me.cboPhrase.Text

                If Strings.Right(WrkFld, 2) <> vbCrLf Then
                    WrkFld = WrkFld & vbCrLf
                End If
                Do
                    Print(1, Mid(WrkFld, 1, InStr(WrkFld, vbCrLf) - 1))
                    Print(1, "<BR>")
                    If InStr(WrkFld, vbCrLf) = Len(WrkFld) - 1 Then
                        Exit Do
                    End If
                    WrkFld = Mid$(WrkFld, InStr(WrkFld, vbCrLf) + 2)
                Loop
                Print(1, "</b></font><BR><BR><BR>")
                Print(1, "<BR><BR><BR>")
                Print(1, "<Font Size=" & Chr(34) & "2" & Chr(34) & " face='Arial'> Current Date/Time:  " & Now & "  " & Now.ToShortTimeString & "</Font>")
                Print(1, "</Body>")
                Print(1, "</HTML>")
            Else
                Print(1, "</Center></H1>------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
                Print(1, "<BR>")
                Print(1, "<Font Color=" & Chr(34) & "Blue" & Chr(34) & ">" & Me.cboBill.Text & "</Font><BR>")
                Print(1, "<Font Color=" & Chr(34) & "Blue" & Chr(34) & ">" & "" & "</Font><BR>")
                Print(1, "<BR>----------------------------------------------------------------------------------------------------------------------------------------------------------<BR>")
                Print(1, "<BR>")
                WrkFld = Me.cboPhrase.Text
                WrkFld = ""

                Print(1, WrkFld)
                Print(1, "<BR>")


                Print(1, "<BR><BR><BR>")
                Print(1, "<BR><BR><BR>")
                Print(1, "<Font Size=" & Chr(34) & "2" & Chr(34) & " face='Arial'> Current Date/Time:  " & Now & "  " & Now.ToShortTimeString & "</Font>")
                Print(1, "</Body>")
                Print(1, "</HTML>")
                ClearDisplayFlag = False
            End If
            FileClose(1)

            Exit Sub

        Catch ex As Exception

            'If ex.Source = 76 Then
            RetValue = DisplayMessage("The folder for this session " & gHTMLFile & " needs to be created before an HTML " & _
               "can be written.  Please create the folder.", "Missing Folder For HTML Document", "S")
            Exit Sub
            'End If
        End Try
    End Sub

    Private Sub cboCalendar_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCalendar.SelectedIndexChanged
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim str As String


        UpdateWorkDataSW = False
        gCalendar = Me.cboCalendar.Text


        str = "Select Bill From tblBills Where CalendarCode = (Select CalendarCode From tblCalendars Where Calendar='" & gCalendar & "') Order By Bill"
        ds = V_DataSet(str, "R")
        cboBill.Items.Clear()
        For Each dr In ds.Tables(0).Rows
            cboBill.Items.Add(dr("Bill"))
        Next

    End Sub

    Private Sub SendMessageToQueue(ByVal label As String, ByVal strBody As String)
        Try
            Dim mq As New MessageQueue
            Dim msg As New Message
            Dim ds As New DataSet

            Try
                If (SPrimary_On And SSecondary_On = False) Then
                    mq.Path = gSendQueueName
                ElseIf (SPrimary_On = False And SSecondary_On) Then
                    mq.Path = gSendQueueNameForSecondary
                End If
                'queueName = ".\PRIVATE$\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE$\SenateVoteQueue"
                msg.Priority = MessagePriority.Normal
                msg.Label = label
                msg.Body = strBody
                mq.Send(msg)

                ''btnVote.Enabled = False
                ''lblChamberLight.BackColor = Color.Red
                ''lblChamberLight.Text = "Waiting Vote!"

                ReceiveTimer.Enabled = True
                ReceiveTimer.Start()
                ReceiveTimer.Interval = gReceiverQueueTimer

                If SDisplay_On Then
                    mq.Path = gSendQueueToDisplay
                    msg.Priority = MessagePriority.Normal
                    msg.Label = label
                    msg.Body = strBody
                    mq.Send(msg)
                End If


                V_DataSet("Update tblVotingParameters Set ParameterValue='" & Me.txtVoteID.Text & "' Where Parameter ='LastVoteIDForThisSession'", "U")
            Catch ex As Exception
                btnVote.Enabled = True
                btnVote.BackColor = System.Drawing.Color.Silver
                btnVote.Text = "Vote"
                Throw New Exception("Error Getting Quequ")
            End Try
            mq.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Voting System")

            ' if Message Queue had exception, write the voting data to text file for writing to ALIS database late
            If File.Exists(gVotingPath & "Vote-" & gVoteID & ".txt") = False Then
                Dim fs As New FileStream(gVotingPath & "Vote-" & gVoteID & ".txt", FileMode.CreateNew)
                fs.Close()
            End If
            FileOpen(1, gVotingPath & "Vote-" & gVoteID & ".txt", OpenMode.Output)
            Print(1, strBody)
        End Try

    End Sub

    Private Sub SendMessageToDisplay(ByVal label As String, ByVal strBody As String, ByVal QueueName As String)
        Try
            Dim mq As New MessageQueue
            Dim msg As New Message
            Dim ds As New DataSet

            Try

                'mq.Path = gSendQueueToDisplay
                mq.Path = QueueName

                'queueName = ".\PRIVATE$\SenateVoteQueue" ;   mq.Path = "FormatName:DIRECT=OS:LEG13\PRIVATE$\SenateVoteQueue"
                msg.Priority = MessagePriority.Normal
                msg.Label = label
                msg.Body = strBody
                mq.Send(msg)

                '  MsgBox("Resend voting data has finished for display request.", MsgBoxStyle.Information, "Senate Voting System")
            Catch ex As Exception
                Throw New Exception("Error Getting Quequ")
            End Try
            mq.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Voting System")
        End Try

    End Sub

    Private Sub ReceiveMessageFromQueue()
        Dim strText As String = ""

        Try
            Dim queue As New MessageQueue(gReceiveQueueName)
            queue.Formatter = New XmlMessageFormatter(New String() {"System.String"})
            Dim qenum As MessageEnumerator

            qenum = queue.GetMessageEnumerator2
            While qenum.MoveNext
                Dim m As Message = qenum.Current
                If m.Label = "CLOSEDVOTE" Then
                    msgText = m.Body
                    m = queue.Receive(New TimeSpan(1000))
                    If msgText <> "" Then
                        txtVoteID.Text = m.Body

                        btnVote.Enabled = True
                        lblChamberLight.BackColor = Color.PaleGreen
                        lblChamberLight.Text = "Order Of Busniess "
                        cboBill.Text = ""
                        cboPhrase.Text = ""
                        cboSenator.Text = ""
                        cboCalendar.Text = ""
                    Else
                        MsgBox("Incorrect voting data! Please request correct voting data.", MsgBoxStyle.OkOnly, "Request correct voting data")
                        SendMessageToQueue("RECALL", "")
                    End If
                ElseIf m.Label = "VOTEID" Then
                    m = queue.Receive(New TimeSpan(1000))

                    MessageBox.Show("Request Last Vote ID Is: " & m.Body)
                    txtVoteID.Text = m.Body

                    btnVote.Enabled = True
                    lblChamberLight.BackColor = Color.PaleGreen
                    lblChamberLight.Text = "Order Busniess "
                    cboBill.Text = ""
                    cboPhrase.Text = ""
                    cboSenator.Text = ""
                    cboCalendar.Text = ""
                ElseIf m.Label = "DISPLAYASK" Then
                    MessageBox.Show("System does not receive voted data to display. Please resend it." & m.Body)

                    If File.Exists(gVotingPath & gDisplayTextFile) = False Then
                        Dim fs As New FileStream(gVotingPath & gDisplayTextFile, FileMode.Open, FileAccess.Read)
                        Dim rd As New StreamReader(fs)

                        rd.BaseStream.Seek(0, SeekOrigin.Begin)
                        While (rd.Peek > -1)
                            strText = rd.ReadLine
                        End While
                        SendMessageToDisplay("STARTDISPLAY", strText, gSendQueueToDisplay)
                    Else
                        MessageBox.Show("System does not retreive voting data for displying!" & m.Body)
                    End If

                End If
            End While

            queue.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        'Try
        '    Dim queue As New MessageQueue(gReceiveQueueName)
        '    Dim qenum As MessageEnumerator

        '    queue.Formatter = New XmlMessageFormatter(New String() {"System.String"})
        '    qenum = queue.GetMessageEnumerator2         'retrive more than one message

        '    While qenum.MoveNext
        '        Dim m As Message = qenum.Current
        '        If m.Label = "CLOSEDVOTE" Then
        '            msgText = m.Body
        '            If msgText <> "" Then
        '                gVotingStarted = True
        '                gVoteID = msgText
        '                txtVoteID.Text = msgText

        '                Me.btnVote.Enabled = True
        '                Me.btnVote.BackColor = Color.Silver
        '                Me.btnVote.Text = "Vote"
        '                Me.lblChamberLight.Text = "Order Businiess"
        '                Me.lblChamberLight.BackColor = System.Drawing.Color.PaleGreen
        '            Else
        '                MsgBox("Incorrect voting data! Please request correct voting data.", MsgBoxStyle.OkOnly, "Request correct voting data")
        '                SendMessageToQueue("RECALL", "")
        '            End If
        '        ElseIf m.Label = "VOTEID" Then
        '            msgText = m.Body
        '            MsgBox("Last Vote ID is: " & msgText, MsgBoxStyle.Information, "You got last vote id")
        '            gVotingStarted = False
        '            gVoteID = m.Body
        '            Me.txtVoteID.Text = gVoteID
        '        End If
        '    End While
        '    queue.Close()
        'Catch

        'End Try
    End Sub

    Private Sub btnVoteID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVoteID.Click
        If SPrimary_On Or SSecondary_On Then
            askVoteID = True
            lblChamberLight.BackColor = Color.Yellow
            lblChamberLight.Text = "Waiting for Last Vote ID"
            btnVote.Enabled = False

            SendMessageToQueue("ASKVOTEID", "")
            clear()
        End If
    End Sub


    Private Sub btnUnlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnlock.Click
        btnVote.Enabled = True
        btnVote.Text = "Vote"
        btnVote.BackColor = System.Drawing.Color.Silver
        lblChamberLight.BackColor = System.Drawing.Color.PaleGreen
        lblChamberLight.Text = "Order Business"
        askVoteID = False
        cboDisplay.Text = "STARTVOTE"
        clear()

    End Sub

    Private Sub clear()
        cboSenator.Text = ""
        cboCalendar.Text = ""
        cboBill.Text = ""
        cboPhrase.Text = ""
        txtVoteID.Text = ""
    End Sub

    Private Sub PingTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PingTimer.Tick
        PingComputer()
    End Sub

    Private Sub bntChang_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bntChang.Click
        ''   send voting information to display PC 
        bodyParamters = ""
        bodyParamters = "gCalendar - " & Me.cboCalendar.Text & "||" & "gBill - " & Me.cboBill.Text & "||" & "gCurrentPhrase - " & cboPhrase.Text
        If SDisplay_On Then
            'SendMessageToDisplay(UCase(cboDisplay.Text), bodyParamters, gSendQueueToDisplay)
            SendMessageToDisplay(DisplayTitle(cboDisplay.Text), bodyParamters, gSendQueueToDisplay)
        Else
            MsgBox("Senate voting display borad computer is off. Unable to change the background.", MsgBoxStyle.Critical, msgText)
        End If
    End Sub

    Private Function DisplayTitle(ByVal disTitle As String) As String
        Select Case UCase(disTitle)
            Case "WELCOME"
                DisplayTitle = "WELCOME"
            Case "START VOTE"
                DisplayTitle = "START VOTE"
            Case "END SESSION"
                DisplayTitle = "END SESSION"
        End Select
    End Function
End Class
