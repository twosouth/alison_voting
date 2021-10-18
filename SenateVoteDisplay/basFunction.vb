'Option Strict Off
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
Imports System.Windows.Forms
Imports System.DBNull
Imports System.Net.NetworkInformation


Module basFunction
#Region "Gobal Variables"
    '*************************************************************************************************************
    ' Copyright @ 2014, Alabama State Senate Data Center

    ' Visual Studio 2010
    '*************************************************************************************************************

    Public SVOTE As String = "ImagePC031"                   '   "LEGI1"      --- Dowe PC - it is a master pc
    Public SDIS As String = "LEGPC39"                  '   "LEGPC39"    --- Sara PC
    Public SOOB As String = "LEGPC39"                   '   "LEGP13"     --- Show HTML on the Wall Projection PC
    'Public SVOTE As String = "SENATEVOTING"             '   "SENATEVOTING"     --- Dowe PC - it is a master pc
    'Public SDIS As String = "SENATEDISPLAY"             '   "SENATEDISPLAY"   --- Sara PC
    'Public SOOB As String = "SENATEOOB"                 '   "SENATEOOB"     --- Show HTML on the Wall Projection PC
    Public Const gPhraseInsertionPoint As String = "*"
    Public Const gSenatorInsertionPoint As String = "^"
    Public Const gCommitteeInsertionPoint As String = "~"
    Public Const gTextInsertionPoint As String = "?"
    Public Const gDesignNbrSenators = 35
    Public Const gBIR As String = "Budget Isolation Resolution."
    Public Const gSenateID As Short = 1753
    Public LocalDatabasePath = ""                        'C:\VotingSystem\VoteSys.mdb"
    Public gCmdFile As String = "c:\VotingSystem\RestartMsgQueue.cmd"


    Public gPingPCTimer As Integer
    Public gSessionID As Integer
    Public gCalendar, gBill, gApplication, gCalendarCode, gBillNbr As String
    Public gPutParams As Boolean
    Public gNbrPhrasesToDisplay As Short
    Public gDefaultPrinter, gSecondaryPrinter As String
    Public gCurrentPhrase, gMessage As New Object
    Public gSenatorName(), gSenatorDistrictName(), gCommittees(), gCommitteeAbbrevs(), gThePhrases(), gPhraseCodes(), gPhrases(), gPhraseForVotePC() As String
    Public gOOBHeadingAttributes, gNotSelectedOOBAttributes As String
    Public gSelectedOOBAttributes, gBillAttributes As String
    Public gIndex As Integer
    Public gPhraseAttributes, gSubjectAttributes As String
    Public gStart, gSVoteID As Integer
    Public gSenator, gVoteID, gSubject As String
    Public gNbrCommittees, gNbrPhrases As Short
    Public gALISTimer As Integer
    Public gSalutation() As String
    Public gSource, gComputerName As String
    Public gBillDisplayed, gCalendarDisplayed, gAction As String
    Public gCalendarCodeDisplayed, gBillNbrDisplayed As String
    Public gFreeFormat As Boolean
    Public gSavePhraseHeight, gVotingTimer As Integer
    Public gSenatorNameOrder() As String
    Public gMessageType, gMessageTitle As String
    Public gTestMode, gCOnly, gChamberHelp, gVotingStarted, gReSendALIS As Boolean
    Public gErrorCount As Short
    Public gPutBill, gPutCalendar, gPutCalendarCode, gPutBillNbr As String
    Public gPutPhrase As Object
    Public gPutChamberLight, gPutVotingLight As Object
    Public gCheckingParams As Boolean
    Public gSessionName, gSessionAbbrev As String
    Public gLegislativeDay, gLastLegislativeDay, gCalendarDay As String
    Public gVotingHelp, gPrintVoteRpt, gSendVotesToALIS As Boolean
    Public gWorkData As Object
    Public gHTMLFile, gNbrSenator As String
    Public gNbrSenators As Integer
    Public gSenatorSplit As Short
    Public gSenatorOID() As Integer
    Public gDistrictOID() As Integer
    Public gWriteVotesToTest As Boolean
    Public gLegislativeDayOID As Integer
    Public gNbrVoteHistoryDays As Short
    Public gSessionIDTmp() As Integer
    Public gLegislativeDayTmp() As Short
    Public gLegislativeDayOIDTmp() As Integer
    Public gCalendarDayTmp() As Object
    Public gSessionNameTmp() As String
    Public gSessionAbbrevTmp() As String
    Public gDeleteTestVotesOnStartUp As Boolean
    Public gDataComputerName As String
    Public SOOBAlive, WoodyAlive As Boolean
    Public gLastVoteIDPerALIS, gLastVoteIDPerLocal As Integer

    Public gOnlyOnePC As Boolean
    Public frmMessage As New frmMessage

    Public connProda As OleDbConnection
    Public connProdb As OleDbConnection
    Public connTest As OleDbConnection
    Public strVoteTest As String                            '   connection string for ALIS_TEST oracle database
    Public strALIS As String                                '   connection string for ALIS Oracle Production database
    Public connLocal As OleDbConnection = Nothing           '   connection local C:\VotingSystem\VoteSys.mdb database
    Public cnOOB As OleDbConnection
    Public cnALIS As OleDbConnection
    Public v_Rows, a_Rows As Integer
    Public strSQL As String
    Public cDateTime As String
    Public gStrALISProd As String
    Public gStrALISTest As String
    Public gDatabasePath As String
    Public strLocal As String
    Public gVotingPath As String
    Public gLogFile, gOOBTextFile As String
    Public msgText As String = "Alabama Senate Voting System"
    Public gSendQueueFromVotePC, gSendQueueFromDisplay, gSendQueueToVotePC, gSendQueueToDisplay, gSendQueueToOOB, gReceiveQueueName, gRequestVoteIDQueue, gSendQueueTimer, gReceiveQueueTimer, gHTMLTimer, tmpWrkFld, Alis_VoteID, gVoteIDTest, gLocalHTMLPage As String
    Public gProduction, LockReadOnly, vSet As Boolean
    Public gDisplay_IPAddress, gVotePC_IPAddress, gOOBPC_IPAddress, gOOBPowerPointFilePath, gLastVoteIDForThisSessionTEST As String
    Public gOOB_Conn, gALIS_Primary_Conn, gALIS_Secondary_Conn, gALIS_TEST_Conn, gWriteToAlisVoteTable, gWriteToAlisIndividualVoteTable As String
    Public SVotePC_On, SDisplay_On, SOOB_On, AlisProda_On, AlisProdb_On, AlisTest_On, Network_On, gNetwork, gWorkingGroup, tableExist, gCreateHTMLPage, rightClick As Boolean
    Public tPage0, tPage1, tPage2, bSenator, bPhrase, dataProcess, DisplayImage, calendarClick As Boolean
    Public setR1, setR2, setR3 As Object

    '-- pass all of values to frmChamberDisplay form for clicking Phrase or Senator from frmPhrase or frmSenator form
    Public pBill, pVoteID, pCSession, pLegDate, pSenator, pCommittee, pBusiness, pCCalendar, pPhrase, pOBusiness, pCalendar, pCBill, pInsertText, pWorkData1, pWorkData2, pWorkData3 As String
    Public K, Index, gAllowedShortVoteCnt, v_id As Integer         '-- if there are more than one LEG day open, k will > 0

    Declare Function MoveWindow Lib "user32" (ByVal hwnd As Integer, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal bRepaint As Integer) As Integer
    Public Declare Function GetAsyncKeyState Lib "user32.dll" (ByVal vKey As Int32) As UShort
    Declare Function WriteProfileString Lib "kernel32" _
                    Alias "WriteProfileStringA" _
                    (ByVal lpszSection As String, _
                    ByVal lpszKeyName As String, _
                    ByVal lpszString As String) As Long

    Public frmChamberDisplay As New frmChamberDisplay
    Public frmPhraseShow As New frmPhraseShow
    Public frmSenatorShow As New frmSenatorShow
    Public frmVote As New frmVote


    Public PhraseDisplayBoard, BillDispalyBoard, OOBDisplayBoard As Integer  '--New
    Public bodyParamters As String
    Public OOBBackgroudChang, DragDrop1, DragDrop2, DragDrop3, askVoteID As Boolean
#End Region

    Function DisplayMessage(ByRef TheMessage As String, ByRef MesgTheTitle As String, ByRef MesgType As String) As Object
        Dim frmMessage As New frmMessage
        Try
            '--- if no message type is passed, then just display and continue
            '--- processing - this is just a status message
            ERHPushStack(("DisplayMessage - " & TheMessage))

            gMessage = TheMessage
            gMessageType = MesgType
            gMessageTitle = NToB(MesgTheTitle)
            frmMessage.Show()
            If gMessageType <> "" Then
                WaitUntilFormCloses("frmMessage")
            End If
            DisplayMessage = gMessage

            ERHPopStack()
            Exit Function
        Catch
            ERHHandler()
        End Try
    End Function

    Public Function FileExistes(ByVal FileName As String) As Integer
        Try
            FileExistes = True
            FileOpen(89, FileName, OpenMode.Input)
            FileClose(89)

            frmVote.setRTimer.Enabled = True
            frmVote.RequestVoteIDTimer.Enabled = True

        Catch ex As Exception
            frmVote.setRTimer.Enabled = False
            frmVote.RequestVoteIDTimer.Enabled = False

            MsgBox(ex.Message & " Please close local database.", vbCritical, "Check Local Database File")
            End         '--- end appliction
        End Try
    End Function

    Public Sub Has_Network_Connectivity()
        Dim hostInfo As System.Net.IPHostEntry
        Try
            '---to look for the logon server
            Dim sServer As String = Environment.GetEnvironmentVariable("logonserver")
            hostInfo = System.Net.Dns.GetHostEntry(sServer.Remove(0, 2))
            Network_On = True
        Catch
            '---there is no network connection
            AlisProda_On = False
            AlisProdb_On = False
            AlisTest_On = False
            gWriteVotesToTest = False
            Network_On = False

            DisplayMessage("Network interruped. Please reset voting system computers to Local Working Group to continue the process.", "Error Occurred: Has_Network_Connectivity()", "S")
            gWorkingGroup = True
            Application.Exit()
        End Try
    End Sub

    Public Sub Check_ALIS_Database_Accessible()
        connProda = New OleDbConnection(gALIS_Secondary_Conn)
        connProdb = New OleDbConnection(gALIS_Primary_Conn)
        connTest = New OleDbConnection(gALIS_TEST_Conn)
        Dim strText As String = "Execute: Check_ALIS_Database_Accessible()"
        Dim RetValue As New Object

        Try
            Try
                '---Check ALIS_PRODB - alis primary
                connProdb.Open()
                If connProdb.State = ConnectionState.Open Then
                    AlisProdb_On = True
                    gNetwork = True
                End If
            Catch
                gNetwork = False
            End Try
            connProdb.Close()

            Try
                '---Check ALIS_PRODA - alis secondary
                connProda.Open()
                If connProda.State = ConnectionState.Open Then
                    gNetwork = True
                    AlisProda_On = True
                End If
            Catch
                AlisProda_On = False
            End Try
            connProda.Close()

            Try
                '---Check ALIS_TEST
                connTest.Open()
                If connTest.State = ConnectionState.Open Then
                    gNetwork = True
                    AlisTest_On = True
                End If
            Catch ex As Exception
                AlisTest_On = False
                MsgBox(ex.Message, vbCritical, "Check_ALIS_Ddatabase_Accessible")
            End Try
            connTest.Close()


            '--- Initial oledbConnection to connect to local VotingSys.mdb database
            '--- 1. Read/Write production to ALIS database
            If gTestMode = False And gWriteVotesToTest = False Then
                If AlisProdb_On And AlisProda_On Then
                    strALIS = gALIS_Primary_Conn
                    cnALIS = New OleDbConnection(strALIS)
                    RetValue = DisplayMessage("ALIS Oracle Databases is accessible", strText, "I")
                ElseIf AlisProdb_On = False And AlisProda_On Then
                    strALIS = gALIS_Secondary_Conn
                    cnALIS = New OleDbConnection(strALIS)
                    RetValue = DisplayMessage("Attention: Only able access the ALIS_PRODA database.", strText, "I")
                ElseIf AlisProdb_On And AlisProda_On = False Then
                    strALIS = gALIS_Primary_Conn
                    cnALIS = New OleDbConnection(strALIS)
                    RetValue = DisplayMessage("Attention: Only able access the ALIS_PRODB database.", strText, "I")
                ElseIf AlisProdb_On = False And AlisProda_On = False Then
                    gNetwork = False
                    If DisplayMessage("Attention: unable write voting data to ALIS production database. Would you want continue process by local 'Working Group'? Otherwise system will shut down.", "Unable Connecte to ALIS", "Y") Then
                        gWorkingGroup = True
                        cnALIS = Nothing
                    Else
                        Application.Exit()
                        '!!! write log file here!
                        '.
                        '.
                    End If
                ElseIf AlisProdb_On = False And AlisProda_On = False And AlisTest_On = False Then
                    gNetwork = False

                    If DisplayMessage("Attention: unable write votes to ALIS Production or ALIS Test database. Would you want continue process by local 'Working Group'? Otherwise system will shut down.", "Unable Connecte to ALIS", "Y") Then
                        gWorkingGroup = True
                        cnALIS = Nothing
                    Else
                        Application.Exit()
                        '!!! write log file here!
                        '.
                        '.
                    End If

                End If
            End If

            '--- 2. No read / no write from ALIS database
            '--- if in text and not writing votes, then there is no interaction
            '--- with ALIS, so set up dummy values; always delete test session votes on startup;
            If gTestMode And gWriteVotesToTest = False Then
                DisplayMessage("Attention: You are in TEST MODE without write vote to ALIS.", "Execute: Check_ALIS_Database_Accessible()", "S")
                V_DataSet("DELETE FROM tblRollCallVotes WHERE SessionID = -1", "D")
                gSessionID = -1
                gLegislativeDayOID = 1
                gLegislativeDay = 1
                gCalendarDay = Now.ToShortDateString
                gVoteID = 0
                V_DataSet("UPDATE tblVotingParameters Set ParameterValue = '0' Where Parameter ='LastVoteIDForThisSession'", "U")
                strALIS = gALIS_TEST_Conn
                cnALIS = New OleDbConnection(strALIS)
            End If

            '--- 3. Read/Write from ALIS_TEST
            If gTestMode And gWriteVotesToTest And AlisTest_On Then
                strVoteTest = gALIS_TEST_Conn
                cnALIS = New OleDbConnection(strVoteTest)
            ElseIf gTestMode And gWriteVotesToTest And AlisTest_On = False Then
                MsgBox("Attention: unable access ALIS_TEST database, system will shutdown. Please contact to your Administrator!", vbCritical, "Check_ALIS_Ddatabase_Accessible")
                End
            End If

            '--- 4. Not Allowed!
            If gTestMode = False And gWriteVotesToTest = True Then
                V_DataSet("UPDATE tblVotingParameters Set ParameterValue ='N'  Where Parameter ='WRITEVOTESTOALISTEST'", "U")

                If AlisProda_On And AlisProda_On Then
                    cnALIS = New OleDbConnection(gALIS_Primary_Conn)
                ElseIf AlisProda_On And AlisProda_On = False Then
                    cnALIS = New OleDbConnection(gALIS_Primary_Conn)
                ElseIf AlisProdb_On = False And AlisProda_On Then
                    cnALIS = New OleDbConnection(gALIS_Secondary_Conn)
                End If
                cnALIS.Open()
                Exit Sub
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: Check_ALIS_Database_Accessible()", "I")
            Exit Sub
        End Try
    End Sub

    Public Sub Get_Parameters()
        Dim ds As New DataSet
        Dim dsP As New DataSet
        Dim parameters As New ArrayList()
        Dim RepValue As New Object
        Dim sqlStr As String = "Select * From tblVotingParameters Where ucase(Parameter) = "
        Dim strFlag As String

        Try

            '--- Builte power on PC's access database connection string to get parameters first
            If gComputerName <> "" Then
                gDataComputerName = gComputerName

                '--- Get Voting system parameters from power on PC

                '*** Dowe's Vote Master PC parameters
                gVotePC_IPAddress = QueryParameters(sqlStr & "'VOTEPCIPADDRESS'").Tables(0).Rows(0).Item("ParameterValue")
                SVOTE = QueryParameters(sqlStr & "'VOTEPCNAME'").Tables(0).Rows(0).Item("ParameterValue")

                '*** Sara's Display PC parameters
                gDisplay_IPAddress = QueryParameters(sqlStr & "'DISPLAYPCIPADDRESS'").Tables(0).Rows(0).Item("ParameterValue")
                SDIS = QueryParameters(sqlStr & "'DISPLAYPCNAME'").Tables(0).Rows(0).Item("ParameterValue")

                '*** OOB show on the wall PC parameters
                gOOBPC_IPAddress = QueryParameters(sqlStr & "'OOBPCIPADDRESS'").Tables(0).Rows(0).Item("ParameterValue")
                gOOB_Conn = QueryParameters(sqlStr & "'OOBPCCONNECTION'").Tables(0).Rows(0).Item("ParameterValue")
                SOOB = QueryParameters(sqlStr & "'OOBPCNAME'").Tables(0).Rows(0).Item("ParameterValue")

                '*** ALIS Production database parameters
                gALIS_Primary_Conn = QueryParameters(sqlStr & "'ALISPRIMARY'").Tables(0).Rows(0).Item("ParameterValue")
                gALIS_Secondary_Conn = QueryParameters(sqlStr & "'ALISSECONDARY'").Tables(0).Rows(0).Item("ParameterValue")
                gALIS_TEST_Conn = QueryParameters(sqlStr & "'ALISTEST'").Tables(0).Rows(0).Item("ParameterValue")

                gVotingPath = QueryParameters(sqlStr & "'VOTINGSYSTEMPATH'").Tables(0).Rows(0).Item("ParameterValue")
                gDatabasePath = QueryParameters(sqlStr & "'DATABASEPATH'").Tables(0).Rows(0).Item("ParameterValue")
                If UCase(gDatabasePath.Trim) <> UCase(LocalDatabasePath.Trim) Then
                    gDatabasePath = strLocal.Trim
                    V_DataSet("Update tblVotingParameters SET ParameterValue='" & LocalDatabasePath.Trim & "' Where ucase(Parameter)='DATABASEPATH'", "U")
                    DisplayMessage("Make sure local database path is: " & vbCrLf & LocalDatabasePath.Trim, msgText, "I")
                End If

                '*** send and receive Queue parameters
                gSendQueueFromVotePC = QueryParameters(sqlStr & "'SENDQUEUEFROMVOTEPC'").Tables(0).Rows(0).Item("ParameterValue")
                gSendQueueFromDisplay = QueryParameters(sqlStr & "'SENDQUEUEFROMDISPLAY'").Tables(0).Rows(0).Item("ParameterValue")
                gSendQueueToOOB = QueryParameters(sqlStr & "'SENDQUEUETOOOB'").Tables(0).Rows(0).Item("ParameterValue")
                gSendQueueToVotePC = QueryParameters(sqlStr & "'SENDQUEUETOVOTEPC'").Tables(0).Rows(0).Item("ParameterValue")
                gSendQueueToDisplay = QueryParameters(sqlStr & "'SENDQUEUETODISPLAY'").Tables(0).Rows(0).Item("ParameterValue")
                gReceiveQueueName = QueryParameters(sqlStr & "'RECEIVEQUEUENAME'").Tables(0).Rows(0).Item("ParameterValue")
                gRequestVoteIDQueue = QueryParameters(sqlStr & "'REQUESTVOTEIDQUEUE'").Tables(0).Rows(0).Item("ParameterValue")


                gLocalHTMLPage = QueryParameters(sqlStr & "'LOCALHTMLPAGE'").Tables(0).Rows(0).Item("ParameterValue")
                gALISTimer = QueryParameters(sqlStr & "'ALISTIMER'").Tables(0).Rows(0).Item("ParameterValue")
                gPingPCTimer = QueryParameters(sqlStr & "'PINGPCTIMER'").Tables(0).Rows(0).Item("ParameterValue")
                gVotingTimer = QueryParameters(sqlStr & "'VOTINGTIMER'").Tables(0).Rows(0).Item("ParameterValue")
                gReceiveQueueTimer = QueryParameters(sqlStr & "'RECEIVEQUEUETIMER'").Tables(0).Rows(0).Item("ParameterValue")
                gOOBTextFile = QueryParameters(sqlStr & "'OOBTEXTFILE'").Tables(0).Rows(0).Item("ParameterValue")
                gNbrVoteHistoryDays = QueryParameters(sqlStr & "'NBRVOTEHISTORYDAYS'").Tables(0).Rows(0).Item("ParameterValue")
                gLastLegislativeDay = QueryParameters(sqlStr & "'LASTLEGISLATIVEDAY'").Tables(0).Rows(0).Item("ParameterValue")
                gNbrPhrasesToDisplay = QueryParameters(sqlStr & "'NUMBEROFPHRASESTODISPLAY'").Tables(0).Rows(0).Item("ParameterValue")
                gNbrSenator = QueryParameters(sqlStr & "'NUMBEROFSENATOR'").Tables(0).Rows(0).Item("ParameterValue").ToString
                gNbrSenators = CType(gNbrSenator, Integer)
                gOOBPowerPointFilePath = QueryParameters(sqlStr & "'OOBPOWERPOINTFILEPATH'").Tables(0).Rows(0).Item("ParameterValue")
                gHTMLFile = QueryParameters(sqlStr & "'HTMLFILE'").Tables(0).Rows(0).Item("ParameterValue")
                gHTMLTimer = QueryParameters(sqlStr & "'HTMLTIMER'").Tables(0).Rows(0).Item("ParameterValue")
                gLogFile = QueryParameters(sqlStr & "'LOGFILE'").Tables(0).Rows(0).Item("ParameterValue")
                gSendQueueTimer = QueryParameters(sqlStr & "'SENDQUEUETIMER'").Tables(0).Rows(0).Item("ParameterValue")
                gWriteToAlisVoteTable = QueryParameters(sqlStr & "'WRITETOALISVOTETABLEFILE'").Tables(0).Rows(0).Item("ParameterValue")
                gWriteToAlisIndividualVoteTable = QueryParameters(sqlStr & "'WRITETOALISINDIVIDUALVOTETABLE'").Tables(0).Rows(0).Item("ParameterValue")
                gAllowedShortVoteCnt = QueryParameters(sqlStr & "'ALLOWEDSHORTVOTECNT'").Tables(0).Rows(0).Item("ParameterValue")
                gSecondaryPrinter = QueryParameters(sqlStr & "'SECONDARYPRINTER'").Tables(0).Rows(0).Item("ParameterValue")
                gCmdFile = QueryParameters(sqlStr & "'COMMANDFILE'").Tables(0).Rows(0).Item("ParameterValue")

                strFlag = UCase(QueryParameters(sqlStr & "'CREATEHTMLPAGE'").Tables(0).Rows(0).Item("ParameterValue"))
                If strFlag = "Y" Then
                    gCreateHTMLPage = True
                Else
                    gCreateHTMLPage = False
                End If
                strFlag = UCase(QueryParameters(sqlStr & "'PRINTVOTEREPORT'").Tables(0).Rows(0).Item("ParameterValue"))
                If strFlag = "Y" Then
                    gPrintVoteRpt = True
                Else
                    gPrintVoteRpt = False
                End If
                strFlag = UCase(QueryParameters(sqlStr & "'TESTMODE'").Tables(0).Rows(0).Item("ParameterValue"))
                If strFlag = "Y" Then
                    gTestMode = True
                    gSVoteID = QueryParameters(sqlStr & "'LASTVOTEIDFORTHISSESSIONTEST'").Tables(0).Rows(0).Item("ParameterValue")
                    gVoteID = CType(gSVoteID, Integer)
                Else
                    gTestMode = False
                    gSVoteID = QueryParameters(sqlStr & "'LASTVOTEIDFORTHISSESSION'").Tables(0).Rows(0).Item("ParameterValue")
                    gVoteID = CType(gSVoteID, Integer)
                End If
                strFlag = UCase(QueryParameters(sqlStr & "'WRITEVOTESTOALISTEST'").Tables(0).Rows(0).Item("ParameterValue"))
                If strFlag = "Y" Then
                    gWriteVotesToTest = True
                Else
                    gWriteVotesToTest = False
                End If
                strFlag = UCase(QueryParameters(sqlStr & "'DELETETESTVOTESONSTART'").Tables(0).Rows(0).Item("ParameterValue"))
                If strFlag = "Y" Then
                    gDeleteTestVotesOnStartUp = True
                Else
                    gDeleteTestVotesOnStartUp = False
                End If
                strFlag = UCase(QueryParameters(sqlStr & "'CHAMBERHELP'").Tables(0).Rows(0).Item("ParameterValue"))
                If strFlag = "Y" Then
                    gChamberHelp = True
                Else
                    gChamberHelp = False
                End If
                strFlag = UCase(QueryParameters(sqlStr & "'VOTINGHELP'").Tables(0).Rows(0).Item("ParameterValue"))
                If strFlag = "Y" Then
                    gVotingHelp = True
                Else
                    gVotingHelp = False
                End If
            End If
            Exit Sub
        Catch ex As Exception
            DisplayMessage(ex.Message & vbCrLf & "Get Voting Parameters Is Failed! Voting System Will Shut Down. Please Try Run It Again.", "Execute: Get_Parameters()", "S")
            Application.Exit()
        End Try
    End Sub

    Public Sub Check_PCs_Status()
        Dim RetValue As New Object
        Dim Ping As Ping
        Dim pReply As PingReply
        Dim mes As String = "Execute: Check_PCs_Status()"

        Try
            Ping = New Ping

            '--- ping: Member Vote Computer - Dowe's PC
            Try
                pReply = Ping.Send(gVotePC_IPAddress)
                If pReply.Status = IPStatus.Success Then
                    SVotePC_On = True
                Else
                    SVotePC_On = False
                End If
            Catch ex As Exception
                GoTo pingDispaly
            End Try

pingDispaly:
            Try
                '--- ping: Display Computer - Sara's PC
                pReply = Ping.Send(gDisplay_IPAddress)
                If pReply.Status = IPStatus.Success Then
                    SDisplay_On = True
                Else
                    SDisplay_On = False
                End If
            Catch ex As Exception
                GoTo pingOOC
            End Try

pingOOC:
            '--- ping: Operate Order of Business Computer - show html on the projection PC
            If UCase(SDIS) = UCase(System.Environment.MachineName) Then
                Try
                    pReply = Ping.Send(gOOBPC_IPAddress)
                    If pReply.Status = IPStatus.Success Then
                        SOOB_On = True
                        cnOOB = New OleDbConnection(gOOB_Conn)
                        RetValue = DisplayMessage("Senate Order of Business Computer Is On.", mes, "I")
                    Else
                        SOOB_On = False
                        cnOOB = Nothing
                        RetValue = DisplayMessage("Senate Order of Business Computer Is Off." & vbCrLf & " Unable Display Vote Informations.", "Check_PCs_Status", "I")
                    End If
                Catch ex As Exception
                    GoTo continueProc
                End Try
            End If

continueProc:
            '--- assign local database connection
            connLocal = New OleDbConnection(strLocal)

            '--- if only single PC On recording it
            gCOnly = False

            '--- Clear stored workarea and onlyOnePC informations
            V_DataSet("Delete From tblOnlyOnePC", "D")

            If SVotePC_On And SDisplay_On Then
                gOnlyOnePC = False

                If UCase(gComputerName) = UCase(SDIS) Then
                    RetValue = DisplayMessage("Senate Voting Computer Is On.", "Check PCs Status", "I")
                End If
                If UCase(gComputerName) = UCase(SVOTE) Then
                    RetValue = DisplayMessage("Senate Chamber Display Computer Is On.", "Check PCs Status", "I")
                End If
            ElseIf SVotePC_On And SDisplay_On = False Then
                V_DataSet("Delete From tblOnlyOnePC", "D")
                V_DataSet("Insert Into tblOnlyOnePC Values (1, '" & SVOTE & "','')", "A")
                If Not DisplayMessage("Senate Chamber Display Computer Is Off. Would You Want Continue Without Chamber Display?", "Chamber Dispaly PC is Offline", "Y") Then
                    End
                End If
                gOnlyOnePC = True
            ElseIf SVotePC_On = False And SDisplay_On Then
                If Not DisplayMessage("Senate Voting Computer Is Off." & vbCrLf & "Would You Want Continue Without Voting?", "Vote PC is Offline", "Y") Then
                    End
                End If
                V_DataSet("Delete From tblOnlyOnePC", "D")
                V_DataSet("Insert Into tblOnlyOnePC Values (1, '" & SDIS & "','')", "A")
                gOnlyOnePC = True
            End If

            If gCOnly Then
                If SVotePC_On Then
                    gDatabasePath = "C:\VotingSystem\SenateVotes.mdb"
                ElseIf SDisplay_On Then
                    gDatabasePath = "C:\VotingSystem\SenateChamberDisplay.mdb"
                End If
                gHTMLFile = "c:\VotingSystem\CurrentMatter.htm"
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Check_PCs_Status()", "S")
            Exit Sub
        End Try
    End Sub

    Public Sub Main()
        Dim ds As New DataSet
        Dim dsP As New DataSet
        Dim RetValue As New Object, CheckDate As Date, x As Long, WrkFld As String

        gNetwork = True
        gWriteVotesToTest = False
        gOnlyOnePC = False
        gWorkingGroup = False
        gTestMode = False

        Try
            '--- get local database connectin string
            If UCase(System.Environment.MachineName) = UCase(SDIS) Then
                strLocal = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\VotingSystem\SenateChamberDisplay.mdb;User ID=admin;Password=;"
                LocalDatabasePath = "C:\VotingSystem\SenateChamberDisplay.mdb"
            ElseIf UCase(System.Environment.MachineName) = UCase(SVOTE) Then
                strLocal = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\VotingSystem\SenateVotes.mdb;User ID=admin;Password=;"
                LocalDatabasePath = "C:\VotingSystem\SenateVotes.mdb"
            End If


            If FileExistes(LocalDatabasePath) Then
                '--- change mouse pointer to waiting pointer
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                '--- get computer name to access local VoteSys.mdb database 
                gComputerName = Trim(System.Environment.MachineName)

                '--- initial connected to local VoteSys.mdb database
                connLocal = New OleDbConnection(strLocal)

                '--- get initial voting parameters from local VoteSys.mdb Access Database
                Get_Parameters()

                '--- initial Application
                If gComputerName = SVOTE Then
                    gApplication = "Voting"
                ElseIf gComputerName = SDIS Then
                    gApplication = "Chamber"
                End If

                '--- check Chamber Display, Voting, and OOB PCs Status
                Check_PCs_Status()

                '--- check Network Connectivity 
                Has_Network_Connectivity()

                '--- check Oracle database 
                Check_ALIS_Database_Accessible()


                '--- get default printer name
                Dim p As Printing.PrinterSettings
                p = New Printing.PrinterSettings()
                gDefaultPrinter = p.PrinterName

                InitialSpecailOrderCalendar()


                '--- get Legislative day 
                GetLegDay()

                '--- download Bills from ALIS, on Chamber Display site always do it, Voting side is optional
                If (gTestMode = False And gWriteVotesToTest = False) Or (gTestMode = True And gWriteVotesToTest = True) Then
                    If (UCase(gComputerName) = UCase(SDIS)) Then
                        Dim dsCheckW As New DataSet
                        dsCheckW = V_DataSet("Select ParameterValue From tblVotingParameters Where UCASE(Parameter) ='LASTLEGISLATIVEDAY'", "R")
                        For Each drCW As DataRow In dsCheckW.Tables(0).Rows
                            If Not drCW("ParameterValue") = gLegislativeDay Then
                                Cursor.Current = Cursors.WaitCursor
                                DownloadBillsFromALIS()
                                Cursor.Current = Cursors.Default
                            End If
                        Next
                    ElseIf (UCase(gComputerName) = UCase(SVOTE)) Then
                        If SDisplay_On = False Then
                            Cursor.Current = Cursors.WaitCursor
                            DownloadBillsFromALIS()
                            Cursor.Current = Cursors.Default
                        Else
                            '--- skip down load bills from alis, beacuse it is a same LEG day
                            '--- if current PC is voting machine, get last voteID for production or test
                            GetVoteIDForVotePC()
                        End If
                    End If
                Else
                    If gTestMode And gWriteVotesToTest = False Then
                        Dim dsTESTVoteID As New DataSet
                        strSQL = "SELECT ParameterValue FROM tblVotingParameters WHERE ucase(Parameter) ='LASTVOTEIDFORTHISSESSIONTEST' "
                        dsTESTVoteID = V_DataSet(strSQL, "R")
                        If dsTESTVoteID.Tables(0).Rows.Count = 0 Then
                            gVoteID = 0
                        Else
                            For Each dr As DataRow In dsTESTVoteID.Tables(0).Rows
                                gVoteID = dr("ParameterValue")
                            Next
                        End If
                    End If
                End If


                '--- for sendisplay, set the paths to files on this PC; for senvote set the paths to files on sendisplay
                '--- NOTE: KEEP COMPUTER NAMES IN CAPS
                x = 40
                WrkFld = Space(40)

                '--- Before download Senator records, resize array. 
                gSenatorSplit = Int((gNbrSenators / 2) + 0.5)
                ReDim gSenatorName(gNbrSenators)
                ReDim gSalutation(gNbrSenators)
                ReDim gSenatorNameOrder(gNbrSenators)
                ReDim gSenatorOID(gNbrSenators)
                ReDim gDistrictOID(gNbrSenators)

                '--- load Senator records from local database
                LoadSenatorsIntoArray()

                '--- these test votes are different from session test votes deleted above;
                If gDeleteTestVotesOnStartUp Then
                    V_DataSet("Delete From tblRollCallVotes Where Test= 1", "D")   ' delete any test votes
                End If

                '--- delete roll call votes per # of history days; if days=999, then no delete
                If gNbrVoteHistoryDays < 999 Then
                    CheckDate = DateAdd(DateInterval.Day, -gNbrVoteHistoryDays, Date.Today)
                    V_DataSet("Delete From tblRollCallVotes Where VoteDate < #" & CheckDate & "#", "D")
                End If

                '--- load the phrases into an array
                LoadPhrasesIntoArray()

                '--- load the committees into an array
                LoadCommitteesIntoArray()
             
                Exit Sub
            Else
                RetValue = DisplayMessage("Since a connection is not available on this PC, try restarting again. If that fails again, please contact your administrator. ", "No Connection On Chamber PC.", "S")
                '!!! write log file
                '.
                '.
                Application.Exit()
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: Main()", "S")
            Exit Sub
        Finally
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Public Function NToB(ByRef var As Object) As Object
        '--- Accepts a variant argument and returns a blank if the argument is Null or the
        '--- argument itself if it is not Null.
        If IsDBNull(var) Then
            NToB = ""
        Else
            NToB = var
        End If
    End Function

    Public Function ReplaceCharacter(ByRef TheFld As String, ByRef FromChar As String, ByRef ToChar As String) As String
        '--- replace all sets of chars to another set of characters
        Dim x, i As Short
        Dim NewFld As String

        Try
            ReplaceCharacter = ""

            NewFld = TheFld
            i = InStr(NewFld, FromChar)
            If i = 0 Then
                GoTo ReplaceCharacterExit
            End If

            Do While InStr(i, NewFld, FromChar) > 0
                x = InStr(i, NewFld, FromChar)
                NewFld = Mid(NewFld, 1, x - 1) & ToChar & Mid(NewFld, x + Len(FromChar))

                '--- move to character following last replacement to continue
                i = x + Len(ToChar)
                If i > Len(NewFld) Then
                    Exit Do
                End If
            Loop

ReplaceCharacterExit:
            ReplaceCharacter = NewFld
            ERHPopStack()
            Exit Function
        Catch ex As Exception
            DisplayMessage(ex.Message, "Replace Character", "S")
        End Try
    End Function

    Public Sub LoadCommitteesIntoArray()
        Dim i As Short
        Dim ds As New DataSet
        Dim dr As DataRow

        Try
            '--- init to blanks
            ReDim gCommittees(0)
            ReDim gCommitteeAbbrevs(0)

            strSQL = "Select Abbrev + '-' + Committee AS TheCommittee, Abbrev, Committee FROM tblCommittees Order By Abbrev"
            ds = V_DataSet(strSQL, "R")

            ReDim Preserve gCommittees(ds.Tables(0).Rows.Count)
            ReDim Preserve gCommitteeAbbrevs(ds.Tables(0).Rows.Count)

            For Each dr In ds.Tables(0).Rows
                i += 1
                gCommittees(i) = dr("Committee")
                gCommitteeAbbrevs(i) = dr("Abbrev")
            Next
            gNbrCommittees = i

            Exit Sub
        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: LoadCommitteesIntoArray()", "I")
            Exit Sub
        End Try
    End Sub

    Public Sub LoadPhrasesIntoArray()
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim i As Short
        ReDim gPhrases(0)           '--- init to blanks
        Dim gPrhaseCodes(0) As Object

        Try
            strSQL = "Select Cstr(Code)  + ' - ' + Phrase  AS ThePhrase, Code, Phrase From tblPhrases Order By Code"
            ds = V_DataSet(strSQL, "R")

            ReDim Preserve gPhrases(ds.Tables(0).Rows.Count)
            ReDim Preserve gThePhrases(ds.Tables(0).Rows.Count)
            ReDim Preserve gPhraseCodes(ds.Tables(0).Rows.Count)

            i = 0
            For Each dr In ds.Tables(0).Rows
                i = i + 1
                gPhrases(i) = dr("Phrase")
                gThePhrases(i) = dr("ThePhrase")
                gPhraseCodes(i) = dr("Code")

            Next
            gNbrPhrases = i

            '--- for Voting PC only
            Dim dsV As New DataSet
            Dim pCount As Integer
            strSQL = "Select * From tblPhrasesForVotePC Order By Phrase"
            dsV = V_DataSet(strSQL, "R")
            ReDim gPhraseForVotePC(dsV.Tables(0).Rows.Count)
            For Each drV As DataRow In dsV.Tables(0).Rows
                pCount += 1
                gPhraseForVotePC(pCount) = drV("Phrase")
            Next

        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: LoadPhrasesIntoArray()", "I")
            Exit Sub
        End Try
    End Sub

    Function IsLoaded(ByRef ThisFormName As String) As Short
        '--- Accepts: a form name
        '--- Purpose: determines if a form is loaded
        '--- Returns: True if specified the form is loaded; False if the specified form is not loaded.

        Dim forms As New Collection
        Dim Ctr, FormFound As Short
        Dim i As Integer

        Try
            Ctr = 0
            FormFound = False
            For i = System.Windows.Forms.Application.OpenForms.Count - 1 To 1 Step -1
                Ctr = Ctr + 1
                Dim form As Form = System.Windows.Forms.Application.OpenForms(i)
                If form.Name = ThisFormName Then
                    FormFound = True
                End If
            Next
            IsLoaded = FormFound

        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: LoadPhrasesIntoArray()", "I")
            Exit Function
        End Try
    End Function

    Sub WaitUntilFormCloses(ByRef FormName As String)
        Do Until Not IsLoaded(FormName)
            System.Windows.Forms.Application.DoEvents()
        Loop
    End Sub

    Public Function DownloadBillsFromALIS() As Integer
        Dim drV, drA, drA1 As DataRow
        Dim dsV As New DataSet
        Dim dsA, dsA1 As New DataSet
        Dim FieldValue() As Object
        Dim Title, WrkFld, str As String, RetValue As Object
        Dim da As New OleDbDataAdapter
        Dim ds As New DataSet

        Try
            ERHPushStack("DownloadBillsFromALIS")

            Try
                DownloadBillsFromALIS = False

                '--- if no ALIS connection, the attempt it again
                If (Not AlisProda_On And Not AlisProdb_On) Or (Not AlisTest_On And gTestMode And gWriteVotesToTest) Then
                    GoTo ProcExit
                End If

                RetValue = DisplayMessage("Please wait while " & IIf(gWriteVotesToTest, "gTest ", "") & "bills are downloaded from ALIS.", "Execute: DownloadBillsFromALIS()", "I")


                '--- get legislative day and session data; check for multiple open legislative days; if
                '--- found, then display and allow user to select or cancel; cancel
                '--- exits the system and assumes someone will fix on the ALIS end; only
                '--- trap 3 open days
                strSQL = "SELECT LD.OID, LD.LEGISLATIVE_DAY, " & _
                        "TO_CHAR(LD.CALENDAR_DATE,  'YYYY-MM-DD') AS CAL_DAY, " & _
                        "UPPER(LD.STATUS_CODE) AS STATUS, LD.OID_SESSION, " & _
                        "LS.LABEL, LS.ABBREVIATION " & _
                        "FROM LEGISLATIVE_DAY LD, LEGISLATIVE_SESSION LS " & _
                        "WHERE (LD.OID_SESSION = LS.OID) AND " & _
                        "(NOT (LD.LEGISLATIVE_DAY IS NULL)) AND " & _
                        "(LD.Status_code = 'O') " & _
                        "ORDER BY LD.LEGISLATIVE_DAY DESC"
                dsA = ALIS_DataSet(strSQL, "R")

                K = -1
                For Each drA In dsA.Tables(0).Rows
                    K = K + 1
                    ReDim Preserve gSessionIDTmp(K)
                    ReDim Preserve gLegislativeDayTmp(K)
                    ReDim Preserve gLegislativeDayOIDTmp(K)
                    ReDim Preserve gCalendarDayTmp(K)
                    ReDim Preserve gSessionNameTmp(K)
                    ReDim Preserve gSessionAbbrevTmp(K)
                    gSessionIDTmp(K) = drA("OID_Session")
                    gLegislativeDayTmp(K) = drA("Legislative_Day")
                    gLegislativeDayOIDTmp(K) = drA("OID")
                    gCalendarDayTmp(K) = FormatDateTime(drA("Cal_Day"), DateFormat.ShortDate)
                    gSessionNameTmp(K) = drA("Label")
                    gSessionAbbrevTmp(K) = drA("Abbreviation")
                    If K = 2 Then
                        Exit For
                    End If
                Next

                If K = -1 Then
                    RetValue = DisplayMessage("There are No open legislative days. Download from ALIS terminated.", "Download Bills From ALIS", "S")
                    Exit Function
                End If

                '--- get current open session id, leg day, and calendar day too, when k=0, if k > 0, there are more than one LEG day open.
                If K = 0 Then
                    gSessionID = gSessionIDTmp(0)
                    gLegislativeDay = gLegislativeDayTmp(0)
                    gLegislativeDayOID = gLegislativeDayOIDTmp(0)
                    gCalendarDay = gCalendarDayTmp(0)
                    gSessionName = gSessionNameTmp(0)
                    gSessionAbbrev = gSessionAbbrevTmp(0)
                Else
                    frmMultipleOpenLegislativeDays.Show()
                    WaitUntilFormCloses("frmMultipleOpenLegislativeDays")
                End If
                dsA.Dispose()

                '--- if different legislatuve day, then update recall votes for Voting PC to 0
                If Val(gLegislativeDay) <> gLastLegislativeDay Then
                    strSQL = "Update tblVotingDisplayParameters SET RecallVote1 = 0,RecallVote2 = 0,RecallVote3 = 0"
                    V_DataSet(strSQL, "U")
                End If

                '--- get last VOTE ID from ALIS for this session and update the
                '--- parameter table
                strSQL = " SELECT MAX(TO_NUMBER(voteid)) AS LastVoteID  " & _
                        " FROM LEGISLATIVE_DAY LD, VOTE " & _
                        " WHERE LD.oid = VOTE.oid_legislative_day " & _
                        " AND (LD.oid_session =" & gSessionID & ") " & _
                        " AND (VOTE.oid_legislative_body = " & gSenateID & ")"
                dsA1 = ALIS_DataSet(strSQL, "R")

                If dsA1.Tables(0).Rows.Count = 0 Then
                    RetValue = DisplayMessage("Cannot find last vote ID for session " & gSessionName & _
                       " in ALIS. The last vote ID will be set to 0.  If this is not correct, you must enter the last vote ID using the Parameter Maintenance form.", "No Vote ID In ALIS", "S")
                    gLastVoteIDPerALIS = 0
                Else
                    For Each drA1 In dsA1.Tables(0).Rows
                        If IsDBNull(drA1("LastVoteID")) Then
                            gLastVoteIDPerALIS = 0
                        Else
                            gLastVoteIDPerALIS = CInt(drA1("LastVoteID"))
                        End If
                    Next
                End If

                '--- now get see if there is a higher local vote id not yet sent to ALIS
                '--- if so, then update the last vote with it
                If gTestMode = False Then
                    strSQL = "SELECT MAX(VoteID) AS LastVoteID FROM tblRollCallVotes WHERE SessionID = " & gSessionID & " AND AlisVoteOID = 0 and Test = 0 "
                Else
                    strSQL = "SELECT MAX(VoteID) AS LastVoteID FROM tblRollCallVotes WHERE SessionID = " & gSessionID & " AND AlisVoteOID = 0 and Test = 1 "
                End If
                dsV = V_DataSet(strSQL, "R")
                If dsV.Tables(0).Rows.Count = 0 Then
                    gLastVoteIDPerLocal = 0
                Else
                    For Each drV In dsV.Tables(0).Rows
                        If IsDBNull(drV("LastVoteID")) Then
                            gLastVoteIDPerLocal = 0
                        Else
                            gLastVoteIDPerLocal = drV("LastVoteID")
                        End If
                    Next
                End If

                '--- now get see if there is a higher local vote id not yet sent to ALIS
                '--- if so, then update the last vote with it
                'If UCase(gComputerName) = UCase(SVOTE) Then
                '    If gTestMode = False Then
                '        strSQL = "SELECT MAX(VoteID) AS LastVoteID FROM tblRollCallVotes WHERE SessionID = " & gSessionID & " AND AlisVoteOID = 0 and Test = 0 "
                '    Else
                '        strSQL = "SELECT MAX(VoteID) AS LastVoteID FROM tblRollCallVotes WHERE SessionID = " & gSessionID & " AND AlisVoteOID = 0 and Test = 1 "
                '    End If

                '    dsV = V_DataSet(strSQL, "R")
                '    If dsV.Tables(0).Rows.Count = 0 Then
                '        gLastVoteIDPerLocal = 0
                '    Else
                '        For Each drV In dsV.Tables(0).Rows
                '            If IsDBNull(drV("LastVoteID")) Then
                '                gLastVoteIDPerLocal = 0
                '            Else
                '                gLastVoteIDPerLocal = drV("LastVoteID")
                '            End If
                '        Next
                '    End If
                'ElseIf UCase(gComputerName) = UCase(SDIS) Then
                '    If gTestMode = False Then
                '        strSQL = "SELECT ParameterValue FROM tblVotingParameters WHERE ucase(Parameter) ='LASTVOTEIDFORTHISSESSION' "
                '    Else
                '        strSQL = "SELECT ParameterValue FROM tblVotingParameters WHERE ucase(Parameter) ='LASTVOTEIDFORTHISSESSIONTEST' "
                '    End If

                '    dsV = V_DataSet(strSQL, "R")
                '    If dsV.Tables(0).Rows.Count = 0 Then
                '        gLastVoteIDPerLocal = 0
                '    Else
                '        For Each drV In dsV.Tables(0).Rows
                '            If IsDBNull(drV("ParameterValue")) Then
                '                gLastVoteIDPerLocal = 0
                '            Else
                '                gLastVoteIDPerLocal = drV("ParameterValue")
                '            End If
                '        Next
                '    End If
                'End If

                If gLastVoteIDPerLocal > gLastVoteIDPerALIS Then
                    gVoteID = gLastVoteIDPerLocal
                Else
                    gVoteID = gLastVoteIDPerALIS
                End If

                'If gTestMode And gLastVoteIDPerLocal = 0 Then
                '    gVoteID = 0
                'End If

                '--- Now update the tblVoteParameters table
                If gTestMode = False Then
                    V_DataSet("Update tblVotingParameters SET ParameterValue='" & gVoteID & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSION'", "U")
                Else
                    V_DataSet("Update tblVotingParameters SET ParameterValue='" & gVoteID & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSIONTEST'", "U")
                End If

                '--- Delete Special Order Calenders  ALWAYS DO THESE!!!
                V_DataSet("DELETE FROM tblSpecialOrderCalendar ", "D")

                '---- if some of error occurred, continue next process
            Catch ex As Exception
                GoTo ProcNext1
            End Try

ProcNext1:
            '--- save any bills that have something in the work area to a temporary table and
            '--- then restore the work areas if the same bills are downloaded again
            Try
                V_DataSet("Delete From tblWork", "D")

                '--- Definition for field() Array
                '--- field 0 - calendar code
                '--- field 1 - bill #
                '--- field 2 - bill description - what will be displayed for the bil - composed of bill #, sponsor, , subject (includes desc for confirmation), and calendar page
                '--- field 3 - sponsor
                '--- field 4 - subject
                '--- field 5 - work area data - always blank when added here - built by chamber display
                '--- field 6 - calendar page  
                '--- field 7 - SenatorSubject
                '--- field 8 - BillCanlendarPage
             

                '---delete all Other bills dynamically retrieved during the day
                V_DataSet("Delete From tblBills Where CalendarCode ='ZZ'", "D")

                ReDim FieldValue(8)


                '### 1 --- Get Regular Order Bills
                Try
                    '---delete Regular Bills first
                    V_DataSet("Delete From tblBills Where CalendarCode ='1'", "D")

                    str = " SELECT label, sponsor, alis_object.index_word, calendar_page " & _
                            " FROM ALIS_OBJECT, MATTER " & _
                            " WHERE matter.oid_instrument = alis_object.oid AND matter.oid_session = " & gSessionID & " AND alis_object.oid_session = " & gSessionID & _
                            " AND matter.matter_status_code = 'Pend' AND matter.oid_legislative_body = '1753' AND alis_object.legislative_body = 'S' " & _
                            " AND matter.calendar_sequence_no > 0 ORDER BY matter.calendar_sequence_no"
                    dsA = ALIS_DataSet(str, "R")
                    If dsA.Tables(0).Rows.Count > 0 Then
                        For Each drA In dsA.Tables(0).Rows
                            FieldValue(0) = "1"
                            If IsDBNull(drA("Label")) Then
                                FieldValue(1) = ""
                            Else
                                FieldValue(1) = drA("Label")
                            End If
                            If Strings.Left(FieldValue(1), 1) = "S" Then
                                Title = " by Senator "
                            Else
                                Title = " by Rep. "
                            End If
                            If IsDBNull(drA("Sponsor")) Then
                                FieldValue(3) = ""
                            Else
                                FieldValue(3) = drA("Sponsor")
                            End If
                            If IsDBNull(drA("Index_Word")) Then
                                FieldValue(4) = ""
                            Else
                                FieldValue(4) = NToB(Replace(drA("Index_Word"), "'", " "))
                            End If
                            If IsDBNull(FieldValue(5)) Or Not IsDBNull(FieldValue(5)) Then
                                FieldValue(5) = ""
                            End If
                            If IsDBNull(drA("Calendar_Page")) Then
                                FieldValue(6) = 0
                            Else
                                FieldValue(6) = drA("Calendar_Page")
                            End If

                            If FieldValue(6) <> 0 Then
                                FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4) & " p." & FieldValue(6)
                            Else
                                FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4)
                            End If

                            '--- new fields added
                            Dim strTitle As String
                            If Strings.Left(FieldValue(1), 1) = "S" Then
                                strTitle = "Senator"
                            Else
                                strTitle = "Representative"
                            End If

                            FieldValue(7) = strTitle & " " & FieldValue(3) & " - " & FieldValue(4)
                            If FieldValue(6) <> 0 Then
                                FieldValue(8) = FieldValue(1) & "&nbsp;&nbsp;&nbsp;&nbsp;" & " p." & FieldValue(6)
                            Else
                                ' FieldValue(7) = FieldValue(1)
                                FieldValue(8) = ""
                                FieldValue(7) = FieldValue(2)
                            End If
                            V_DataSet("INSERT INTO tblBills VALUES ('1', '" & FieldValue(1) & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "', '" & Replace(FieldValue(7), "'", "") & "', '" & Replace(FieldValue(8), "'", "") & "')", "A")
                        Next
                    End If
                Catch ex As Exception
                    DisplayMessage("Failed Download Bills From ALIS! Please Re-try.", "Down Load Bill From ALIS", "Y")
                    End
                End Try

                '### 2 --- Get Local Bills  
                Try
                    '--- delete Local bills first
                    V_DataSet("Delete From tblBills Where CalendarCode ='2'", "D")

                    str = "SELECT label, calendar_page, senate_display_title, sponsor " & _
                            " FROM alis_object, matter WHERE matter.oid_instrument = alis_object.oid " & _
                            " AND matter.oid_session = " & gSessionID & " AND alis_object.oid_session = " & gSessionID & " AND matter.matter_status_code = 'Pend' " & _
                            " AND matter.oid_legislative_body = '1753' AND alis_object.legislative_body = 'S' AND alis_object.local_bill = 'T' " & _
                            " AND matter.calendar_sequence_no > 0 ORDER BY matter.calendar_sequence_no "
                    dsA = ALIS_DataSet(str, "R")

                    If dsA.Tables(0).Rows.Count > 0 Then
                        For Each drA In dsA.Tables(0).Rows
                            FieldValue(0) = "2"
                            FieldValue(1) = NToB(drA("Label"))
                            If Strings.Left(FieldValue(1), 1) = "S" Then
                                Title = " by Senator "
                            Else
                                Title = " by Rep. "
                            End If
                            FieldValue(3) = NToB(drA("Sponsor"))
                            If InStr(NToB(drA("Senate_Display_Title")), " ") > 0 Then
                                FieldValue(4) = Mid(NToB(drA("Senate_Display_Title")), InStr(NToB(drA("Senate_Display_Title")), " ") + 1)
                            Else
                                FieldValue(4) = Mid(NToB(drA("Senate_Display_Title")), 1)
                            End If

                            FieldValue(5) = ""
                            If IsDBNull((drA("Calendar_Page"))) Then
                                FieldValue(6) = 0
                            Else
                                FieldValue(6) = drA("Calendar_Page")
                            End If
                            If CType(FieldValue(6), String) <> 0 Then
                                FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4) & " p." & FieldValue(6)
                            Else
                                FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4)
                            End If

                            Dim strTitle As String = ""
                            If Strings.Left(FieldValue(1), 1) = "S" Then
                                strTitle = "Senator"
                            ElseIf Strings.Left(FieldValue(1), 1) = "H" Then
                                strTitle = "Representative"
                            ElseIf Strings.Left(FieldValue(1), 1) <> "S" And Strings.Left(FieldValue(1), 1) <> "H" Then
                                strTitle = ""
                            End If

                            FieldValue(7) = strTitle & " " & FieldValue(3) & " - " & FieldValue(4)
                            If FieldValue(6) <> 0 Then
                                FieldValue(8) = FieldValue(1) & "&nbsp;&nbsp;&nbsp;&nbsp;" & " p." & FieldValue(6)
                            Else
                                FieldValue(8) = ""
                                FieldValue(7) = FieldValue(2)
                            End If
                            V_DataSet("INSERT INTO tblBills VALUES ('2', '" & FieldValue(1) & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "', '" & Replace(FieldValue(7), "'", "") & "', '" & Replace(FieldValue(8), "'", "") & "')", "A")
                        Next
                    End If
                Catch ex As Exception
                    DisplayMessage("Failed Download Local Bills From ALIS! Please Re-try.", "Down Load Local Bill From ALIS", "Y")
                    End
                End Try


                '###3 ---Get Confirmations
                Try
                    '--- delete confirmatins
                    V_DataSet("Delete From tblBills Where CalendarCode ='3'", "D")

                    str = "SELECT I.OID, I.LABEL, I.TYPE_CODE,   PE.FIRSTNAME, PE.LASTNAME, PE.SUFFIX, O.NAME, CO.ABBREVIATION, TO_CHAR(R1DAY.CALENDAR_DATE, 'MM/DD/YYYY') AS R1DATE, TO_CHAR(MDAY.CALENDAR_DATE, 'MM/DD/YYYY') AS MDATE, " & _
                            " M.TRANSACTION_TYPE_CODE AS MTRAN, M.MATTER_STATUS_CODE AS MSTAT, M.RECOMMENDATION_CODE AS MREC, AU.NAME AS AUTHORITY, CV.Value AS INSTR_VALUE " & _
                        " FROM ALIS_OBJECT I, MATTER C1, MATTER M, ORGANIZATION CO, ORGANIZATION O, POSITION PO, POSITION AU, PERSON PE, LEGISLATIVE_DAY R1DAY, LEGISLATIVE_DAY MDAY, CODE_VALUES CV " & _
                        " WHERE I.INSTRUMENT = 'T' AND I.TYPE_CODE = 'CF' AND I.STATUS_CODE = CV.CODE (+) AND CV.CODE_TYPE = 'InstrumentStatus' AND I.OID_SESSION = " & gSessionID & " AND I.OID_POSITION = PO.OID (+) " & _
                            " AND O.OID (+) = PO.OID_ORGANIZATION AND AU.OID (+) = PO.OID_APPOINTING_AUTHORITY AND I.OID = C1.OID_INSTRUMENT AND M.OID_COMMITTEE = CO.OID (+) AND C1.OID_CANDIDACY = PE.OID (+) " & _
                            " AND C1.OID_CONSIDERED_DAY = R1DAY.OID (+) AND M.OID_CONSIDERED_DAY = MDAY.OID (+) AND M.OID_INSTRUMENT = I.OID AND M.SEQUENCE = I.LAST_MATTER AND I.OID_SESSION = C1.OID_SESSION " & _
                            " AND C1.SEQUENCE = (SELECT MIN(X.SEQUENCE) FROM MATTER X WHERE X.OID_SESSION = C1.OID_SESSION AND X.OID_INSTRUMENT = C1.OID_INSTRUMENT AND X.TRANSACTION_TYPE_CODE = 'R1') ORDER BY SUBSTR(I.LABEL, 1, 1), I.TYPE_CODE, I.ID"
                    dsA = ALIS_DataSet(str, "R")

                    If dsA.Tables(0).Rows.Count > 0 Then
                        For Each drA In dsA.Tables(0).Rows
                            FieldValue(0) = "3"
                            FieldValue(1) = NToB(Mid(drA("Label"), 2))
                            FieldValue(3) = NToB(drA("FirstName")) & " " & NToB(drA("LastName"))
                            FieldValue(4) = ""

                            '--- if boards and commissions exist, then search for the senate voting name and replace if found            
                            If NToB(drA("Name")) = "" Then
                                FieldValue(2) = FieldValue(1) & " - " & FieldValue(3) & " -- "
                            Else
                                WrkFld = NToB(drA("Name"))
                                str = "SELECT SenateVotingName FROM tblBoardsAndCommissions WHERE AlisName = '" & Replace(WrkFld, "'", "''") & "'"
                                dsV = V_DataSet(str, "R")
                                If dsV.Tables(0).Rows.Count = 0 Then
                                    FieldValue(2) = FieldValue(1) & " - " & FieldValue(3) & " -- " & NToB(drA("Name"))
                                Else
                                    FieldValue(2) = FieldValue(1) & " - " & FieldValue(3) & " -- " & dsV.Tables(0).Rows(0).Item("SenateVotingName")
                                End If
                            End If
                            FieldValue(5) = ""
                            FieldValue(6) = 0

                            Dim strTitle As String = ""
                            If Strings.Left(FieldValue(1), 1) = "S" Then
                                strTitle = "Senator"
                            ElseIf Strings.Left(FieldValue(1), 1) = "H" Then
                                strTitle = "Representative"
                            ElseIf Strings.Left(FieldValue(1), 1) <> "S" And Strings.Left(FieldValue(1), 1) <> "H" Then
                                strTitle = ""
                            End If

                            FieldValue(7) = strTitle & " " & FieldValue(3) & " - " & FieldValue(4)

                            If FieldValue(6) <> 0 Then
                                FieldValue(8) = FieldValue(1) & "&nbsp;&nbsp;&nbsp;&nbsp;" & " p." & FieldValue(6)
                            Else
                                FieldValue(7) = FieldValue(1)
                            End If
                            V_DataSet("INSERT INTO tblBills VALUES ('3', '" & FieldValue(1) & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "', '" & Replace(FieldValue(7), "'", "") & "', '" & Replace(FieldValue(8), "'", "") & "')", "A")
                        Next
                    End If
                Catch
                    DisplayMessage("Failed Download Confirmations From ALIS! Please Re-try.", "Down Load Confirmatins From ALIS", "Y")
                    End
                End Try

                '--- restore any saved work areas to the bills downloaded
                dsV = V_DataSet("Select * From tblWork", "R")
                For Each drV In dsV.Tables(0).Rows
                    V_DataSet("UPDATE tblBills SET WorkData ='" & Replace(drV("WorkData"), "'", "''") & "' WHERE CalendarCode ='" & NToB(drV("CalendarCode")) & "' AND ucase(BillNbr) ='" & UCase(NToB(drV("BillNbr"))) & "'", "U")
                Next
                RetValue = DisplayMessage("Download of " & IIf(gWriteVotesToTest, "TEST ", "") & " Bills From ALIS Completed.", "Execute: DownloadBillsFromALIS()", "I")

                DownloadBillsFromALIS = True
            Catch ex As Exception
                DownloadBillsFromALIS = False
                GoTo ProcExit
            End Try

ProcExit:

        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: DownloadBillsFromALIS()", "S")
            Exit Function
        End Try
    End Function

    Public Sub LoadSenatorsIntoArray()
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim i, y As Short
        Dim RetValue As New Object

        '--- load the senators into an array
        Try
            i = 0

            strSQL = "Select * From tblSenators Order By SenatorName"
            ds = V_DataSet(strSQL, "R")
            gNbrSenators = ds.Tables(0).Rows.Count
            ReDim Preserve gSenatorName(gNbrSenators)
            ReDim Preserve gSenatorOID(gNbrSenators)
            ReDim Preserve gDistrictOID(gNbrSenators)
            ReDim Preserve gSenatorDistrictName(gNbrSenators)

            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr In ds.Tables(0).Rows
                    '  i = i + 1
                    If (i > gNbrSenators) Or (Int(ds.Tables(0).Rows.Count > gNbrSenators)) Then
                        RetValue = DisplayMessage("There are more Senators than we have space allocated for.  Either # of Senators in the " & "parameter table is incorrect or you need to download Senators from ALIS.  It is suggested you determine which " & "is correct, but processing will continue.  This message may appear more than once.", "Senator # Miss-Match", "I")
                    Else
                        gSenatorNameOrder(i) = dr("SenatorName")            '--- used to populate senator combo box on chamber display
                        If gApplication = "Voting" Then
                            gSalutation(i) = NToB(dr("Salutation"))         '--- in senator name order
                        End If
                        gSenatorName(i) = dr("SenatorName")
                        gSenatorOID(i) = dr("SenatorOID")                   '--- must follow senator name order
                        gDistrictOID(i) = dr("DistrictOID")
                        i = i + 1
                    End If
                Next
                RetValue = DisplayMessage("Reading Senators Records From Local Database Finished.", "Execute: LoadSenatorsIntoArray()", "I")
            Else
                RetValue = DisplayMessage("Attention: There are not Senators download from ALIS yet!", "Execute: LoadSenatorsIntoArray()", "I")
                DownloadSenatorsFromALIS()
            End If

            ds.Dispose()

            Dim dsD As New DataSet
            Dim k As Integer = 0
            Dim x As Integer = 0
            Dim MemberLname As String = ""
            strSQL = "Select * From tblSenators Order By SenatorNbr"
            dsD = V_DataSet(strSQL, "R")
            For Each drD As DataRow In dsD.Tables(0).Rows
                gSenatorDistrictName(k + 1) = drD("SenatorName")
                k += 1
            Next
            Exit Sub
        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: LoadSenatorsIntoArray()", "I")
            Exit Sub
        End Try
    End Sub

    Public Function QueryParameters(ByVal strSql As String) As DataSet
        Dim da As New OleDbDataAdapter
        Dim ds As New DataSet

        If connLocal.State = ConnectionState.Closed Then connLocal.Open()

        Try
            If strSql <> "" Then
                da = New OleDbDataAdapter(strSql, connLocal)
                da.SelectCommand = New OleDbCommand(strSql, connLocal)
                da.Fill(ds, "Table")
                QueryParameters = ds
                Return ds
            End If
        Catch ex As OleDbException
            DisplayMessage(ex.Message, "Get Voting Parameters Failed", "S")
        Finally
            connLocal.Close()
        End Try
    End Function

    Public Sub InitialSpecailOrderCalendar()
        Dim ds, dsC As New DataSet
        Dim RetValue As New Object

        Try
            strSQL = "Delete From tblSpecialOrderCalendar "
            ds = V_DataSet(strSQL, "D")

            strSQL = "Delete From tblCalendars Where LEFT(CalendarCode, 2)='SR'"
            dsC = V_DataSet(strSQL, "D")

            strSQL = "Delete From tblCalendars Where CalendarCode='SOC'"
            dsC = V_DataSet(strSQL, "D")

            Exit Sub
        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: IintalSpecailOrderCalendar()", "S")
            Exit Sub
        End Try
    End Sub

    Public Function GetBills(ByVal gCalendar As String) As DataSet
        Try
            Dim origCalendar As String = gCalendar
            Dim sr As String = UCase(Strings.Left(gCalendar, 2))
            If sr = "SR" Then
                gCalendar = "SR"
            End If

            If gCalendar = "SR" Then
                strSQL = "Select Bill, CalendarCode From  tblSpecialOrderCalendar Where CalendarCode = (Select CalendarCode From tblCalendars Where ucase(Calendar) ='" & UCase(origCalendar) & "') Order By Bill"
            End If
            If gCalendar <> "SR" And UCase(gCalendar) <> "SPECIAL ORDER CALENDAR" Then
                strSQL = "Select Bill, CalendarCode From tblBills Where CalendarCode = (Select CalendarCode From tblCalendars Where ucase(Calendar) ='" & UCase(gCalendar) & "') Order By Bill"
            End If
            If UCase(gCalendar) = "SPECIAL ORDER CALENDAR" Then
                strSQL = "Select Bill, CalendarCode From  tblSpecialOrderCalendar Where CalendarCode = (Select CalendarCode From tblCalendars Where ucase(CalendarCode) ='SOC') Order By Bill"
            End If
            GetBills = V_DataSet(strSQL, "R")

        Catch ex As Exception
            DisplayMessage(ex.Message, "Get Bills", "S")
        End Try
    End Function

    Public Sub UpdateVotingDisplayParameters(Optional ByVal VoteID1 As Integer = 0, Optional ByVal VoteID2 As Integer = 0, Optional ByVal VoteID3 As Integer = 0)
        Try
            If VoteID1 > 0 Then
                V_DataSet("Update tblVotingDisplayParameters Set RecallVote1 = " & VoteID1, "U")
                V_DataSet("Update tblVotingParameters Set ParameterValue = '" & VoteID1 & "' WHERE Parameter ='RecallVote1'", "U")
            End If
            If VoteID2 > 0 Then
                V_DataSet("Update tblVotingDisplayParameters Set RecallVote2 = " & VoteID2, "U")
                V_DataSet("Update tblVotingParameters Set ParameterValue = '" & VoteID2 & "' WHERE Parameter ='RecallVote2'", "U")
            End If
            If VoteID3 > 0 Then
                V_DataSet("Update tblVotingDisplayParameters Set RecallVote3 = " & VoteID3, "U")
                V_DataSet("Update tblVotingParameters Set ParameterValue = '" & VoteID3 & "' WHERE Parameter ='RecallVote3'", "U")
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: UpdateVotingDisplayParameters", "S")
            Exit Sub
        End Try
    End Sub

    Public Function GetLastLocalVoteID() As Long
        Dim dsP, dsR As New DataSet
        Dim strP, strR As String
        Dim voteidP, voteidR As Integer

        '--- this roution gets the last vote ID for this session from the param table; this
        '--- field is initially updated on startup by finding the last vote ID sent to ALIS, and
        '--- the param table is updated after each vote so that number should always be current
        '--- during this logon
        Try
            If gTestMode = False Then
                strP = "Select ParameterValue From tblVotingParameters Where UCase(Parameter)='LASTVOTEIDFORTHISSESSION'"
                dsP = V_DataSet(strP, "R")
                strR = "Select max(VoteID) as voteID From tblRollCallVotes Where Test =0"
                dsR = V_DataSet(strR, "R")
            Else
                strP = "Select ParameterValue From tblVotingParameters Where UCase(Parameter)='LASTVOTEIDFORTHISSESSIONTEST'"
                dsP = V_DataSet(strP, "R")
                strR = "Select max(VoteID) as voteID From tblRollCallVotes Where Test =1"
                dsR = V_DataSet(strR, "R")
            End If

            If dsP.Tables(0).Rows.Count <> 0 Then
                For Each drP As DataRow In dsP.Tables(0).Rows
                    If Not IsDBNull(drP("ParameterValue")) Then
                        voteidR = drP("ParameterValue")
                    Else
                        voteidR = 0
                    End If
                Next
            Else
                voteidP = 0
            End If

            If dsR.Tables(0).Rows.Count <> 0 Then
                For Each drR As DataRow In dsR.Tables(0).Rows
                    If Not IsDBNull(drR("VoteID")) Then
                        voteidP = drR("VoteID")
                    Else
                        voteidP = 0
                    End If
                Next
            Else
                voteidP = 0
            End If

            If voteidP = voteidR Then
                gVoteID = voteidP
            ElseIf voteidP > voteidR Then
                gVoteID = voteidR
            ElseIf voteidP < voteidR Then
                gVoteID = voteidR
            End If
            UpdateLastVoteIDAndLastLegDay(gVoteID)

            GetLastLocalVoteID = gVoteID
        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: GetLastLocalVoteID()", "S")
            Exit Function
        End Try
    End Function

    Public Sub UpdateLastVoteIDAndLastLegDay(ByVal LastVoteID As String)
        Try
            If gTestMode = False Then
                strSQL = "Update tblVotingParameters Set ParameterValue='" & LastVoteID & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSION'"
                V_DataSet(strSQL, "U")
            Else
                strSQL = "Update tblVotingParameters Set ParameterValue='" & LastVoteID & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSIONTEST'"
                V_DataSet(strSQL, "U")
            End If
            strSQL = "Update tblVotingParameters Set ParameterValue='" & gLegislativeDay & "' Where ucase(Parameter) ='LASTLEGISLATIVEDAY'"
            V_DataSet(strSQL, "U")
        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: UpdateLastVoteIDAndLastLegDay()", "I")
            Exit Sub
        End Try
    End Sub

    Public Sub GetLegDay()
        Dim ds As New DataSet

        Try
            strSQL = "SELECT LD.OID, LD.LEGISLATIVE_DAY, " & _
                      " TO_CHAR(LD.CALENDAR_DATE,  'YYYY-MM-DD') AS CAL_DAY, " & _
                      " UPPER(LD.STATUS_CODE) AS STATUS, LD.OID_SESSION, " & _
                      " LS.LABEL, LS.ABBREVIATION " & _
                      " FROM LEGISLATIVE_DAY LD, LEGISLATIVE_SESSION LS " & _
                      " WHERE (LD.OID_SESSION = LS.OID) AND " & _
                      " (NOT (LD.LEGISLATIVE_DAY IS NULL)) AND " & _
                      " (LD.Status_code = 'O') " & _
                      " ORDER BY LD.LEGISLATIVE_DAY DESC"
            ds = ALIS_DataSet(strSQL, "R")

            Dim openDay As Integer = ds.Tables(0).Rows.Count
            If openDay = 0 Then
                RetValue = DisplayMessage("There are No open legislative days. System will shut down. Restart try again.", "No Legislative Day Open", "S")
                Application.Exit()
            ElseIf openDay > 1 Then
                frmMultipleOpenLegislativeDays.Show()
                WaitUntilFormCloses("frmMultipleOpenLegislativeDays")
            ElseIf openDay = 1 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    gSessionID = dr("OID_SESSION")
                    gLegislativeDay = dr("LEGISLATIVE_DAY")
                    gLegislativeDayOID = dr("OID")
                    gCalendarDay = dr("CAL_DAY")
                    gSessionName = dr("LABEL")
                    gSessionAbbrev = dr("ABBREVIATION")
                Next
            End If
        Catch ex As Exception
            RetValue = DisplayMessage(ex.Message, "Error Occurred: Get Legislative Day", "S")
            Exit Sub
        End Try
    End Sub

    Public Function SendVotesToALIS() As Integer
        Dim k As Integer
        Dim indVoteOID As Long, VoteOID As Long, WrkFld As String, SenatorOID As Long, Vote As String
        Dim DistrictOID As Long
        Dim str, strAdd As String
        Dim ds, dsAdd, dsIndVoteOID, dsVoteOID, dsDetail As New DataSet
        Dim dr As DataRow
        Dim dt As New DataTable

        dt.Clear()
        dt.Columns.Clear()
        dt.Columns.Add("SenatorOID")
        dt.Columns.Add("DistrictOID")
        dt.Columns.Add("Vote")

        Try
            '--- if it is test and not write to ALIS or ALIS database un-accessiable, quite send votes to ALIS
            If (gTestMode And Not gWriteVotesToTest) Then
                GoTo ProcExit
            End If

            '--- check able to connecte ALIS
            '--- Check_ALIS_Database_Accessible()
            If (Not AlisProda_On And Not AlisProdb_On) Then
                DisplayMessage("An error occurred while sending the votes to ALIS.  If the connection to ALIS is down, you should exit the system " & _
                "and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Vote Not Sent To ALIS", "S")
                gSendVotesToALIS = True
                GoTo ProcExit
            End If

            If gTestMode And gWriteVotesToTest Then                '---- 0 gProduction votes ,     1 gTest votes
                If gReSendALIS = False Then
                    strSQL = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 1 And AlisVoteOID=0 AND VoteID =" & gVoteID
                Else
                    strSQL = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 1 And AlisVoteOID=0 "
                End If
            Else
                If gReSendALIS = False Then
                    strSQL = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 0 And AlisVoteOID=0 AND VoteID =" & gVoteID
                Else
                    str = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 0 And AlisVoteOID=0 "
                End If
            End If
            ds = V_DataSet(strSQL, "R")

            For Each dr In ds.Tables(0).Rows
                '--- get parentas VoteID from ALIS
                dsVoteOID = ALIS_DataSet("SELECT ALIS.VOTE_S.NEXTVAL AS VOTEOID FROM DUAL", "R")
                If dsVoteOID.Tables(0).Rows.Count > 0 Then
                    VoteOID = dsVoteOID.Tables(0).Rows(0).Item("VOTEOID")
                End If

                strAdd = "INSERT INTO Vote (oid, oid_legislative_Body, oid_Legislative_day, voteId, nays, yeas, abstains, pass) VALUES (" & VoteOID & ", " & gSenateID & ", " & _
                        dr("LegislativeDayOID") & ", " & dr("VoteID") & ", " & dr("TotalNay") & ", " & dr("TotalYea") & ", " & dr("TotalAbstain") & ", " & dr("TotalPass") & ")"
                ALIS_DataSet(strAdd, "A")

                '--- pick out vote, districtOID, and senatorOID add to tmp tabel
                WrkFld = dr("SenatorVotes")
                k = 0
                Do
                    WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1) ' skip senator name
                    Vote = Microsoft.VisualBasic.Left(WrkFld, 1)
                    WrkFld = Mid$(WrkFld, 3)
                    DistrictOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, "*") - 1))
                    WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1)
                    SenatorOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, ";") - 1))
                    If InStr(WrkFld, ";") = Len(WrkFld) Then
                        WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
                        dt.Rows.Add(SenatorOID, DistrictOID, Vote)
                        Exit Do
                    End If
                    WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
                    dt.Rows.Add(SenatorOID, DistrictOID, Vote)
                Loop

                '--- sort districtOID
                Dim dataView As New DataView(dt)
                dataView.Sort = "DistrictOID"
                Dim dataTable As DataTable = dataView.ToTable

                If dataView.Count > 0 Then
                    For Each drDetail As DataRowView In dataView
                        ' --- get Individual Member VoteID from ALIS
                        dsIndVoteOID = ALIS_DataSet("SELECT ALIS.INDIVIDUAL_VOTE_S.NEXTVAL AS IND_VOTEOID FROM DUAL", "R")
                        indVoteOID = dsIndVoteOID.Tables(0).Rows(0).Item("IND_VoteOID")
                        strSQL = "INSERT INTO Individual_Vote(oid, oid_legislator, oid_vote, oid_organization, vote) VALUES (" & indVoteOID & ", " & drDetail("SenatorOID") & ", " & VoteOID & ", " & drDetail("DistrictOID") & ", '" & drDetail("Vote") & "')"
                        ALIS_DataSet(strSQL, "A")
                    Next
                End If

                If gTestMode Then
                    V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & ", TEST = 1 WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")
                Else
                    V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & ", TEST = 0 WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")
                End If
            Next
            gSendVotesToALIS = False

            If gReSendALIS Then
                DisplayMessage("Process all of UNSEND votes to ALIS finished.", "Send Votes To ALIS", "S")
                Exit Function
            End If

ProcExit:

        Catch ex As Exception
            RetValue = DisplayMessage(ex.Message & vbCrLf & "An error occurred while Re-Send the votes to ALIS.  If the connection to ALIS is down, you should exit the system " & _
            "and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Execute: SendUnSendVotesToALIS()", "S")
            Exit Function
        End Try
    End Function

    Public Sub DownloadSenatorsFromALIS()
        Dim i As Short
        Dim dsSenator As New DataSet
        Dim dsDistrict As New DataSet
        Dim drSenator As DataRow
        Dim dt As New DataTable
        dt.Clear()
        dt.Columns.Clear()
        dt.Columns.Add("SenatorName")
        dt.Columns.Add("SenatorOID")
        dt.Columns.Add("DistrictOID")

        Try

            If Not DisplayMessage("You are about to download senators from ALIS.  " & "Do you want to continue?", "Download Senators From ALIS", "Y") Then
                gNbrSenators = 0
                Exit Sub
            End If

            '--- Clear tblSenators table first
            V_DataSet("Delete From tblSenators", "D")

            '--- senators are read in by their distric #, so, the first one read is
            '--- from district 1 and gets assigned as senator #1;
            '--- also read another ALIS sql stmt to get the oid of the district which
            '--- is used when the votes are recorded; oids are read in the same
            '--- order such that the first oid is assigned to the now senator #1

            i = 0
            Dim strSql As String
            ' strSql = " SELECT A.NAME AS SenatorName, A.OID as SenatorOID, O.OID as DistrictOID" & _
            strSql = " SELECT A.NAME AS SenatorName, A.OID as SenatorOID, O.OID as DistrictOID" & _
                    " FROM ALIS_OBJECT A, POSITION PO,  Organization O " & _
                    " WHERE(A.OID_POSITION = PO.OID) " & _
                        " AND (A.INCUMBENCY = 'T') " & _
                        " AND (A.INCUMBENCY_TYPE_CODE = 'ET') " & _
                        " AND start_date < sysdate  " & _
                        " AND nvl(end_date, sysdate) >= sysdate  " & _
                        " AND (PO.TYPE_CODE = 1) " & _
                        " AND PO.oid_organization = O.OID " & _
                        " ORDER BY O.OID "
            dsSenator = ALIS_DataSet(strSql, "R")

            strSql = ""
            If dsSenator.Tables(0).Rows.Count > 0 Then
                For Each drSenator In dsSenator.Tables(0).Rows
                    strSql = "Insert Into tblSenators Values ('" & Replace(drSenator("SenatorName"), """", "") & "', " & drSenator("SenatorOID") & ", '', " & i + 1 & ", " & drSenator("DistrictOID") & ")"
                    V_DataSet(strSql, "A")
                    i += 1
                Next
                gNbrSenators = dsSenator.Tables(0).Rows.Count

                LoadSenatorsIntoArray()

                RetValue = DisplayMessage("Senator download complete.  Don't forget to add the senator's salutations.", "", "I")
            Else
                DisplayMessage("There are not Senators to download. Please contact to your Administrator.", "Download Senators From ALIS", "S")
                End
            End If

        Catch ex As Exception
            DisplayMessage(ex.Message & " Failed download senators from ALIS. Please contact to your Administrator.", "Download Senators From ALIS", "S")
            End
        End Try
    End Sub

    Public Sub GetVoteIDForVotePC()
        '--- get last VOTE ID from ALIS for this session and update the
        '--- parameter table
        strSQL = " SELECT MAX(TO_NUMBER(voteid)) AS LastVoteID  " & _
                " FROM LEGISLATIVE_DAY LD, VOTE " & _
                " WHERE LD.oid = VOTE.oid_legislative_day " & _
                " AND (LD.oid_session =" & gSessionID & ") " & _
                " AND (VOTE.oid_legislative_body = " & gSenateID & ")"
        dsA1 = ALIS_DataSet(strSQL, "R")

        If dsA1.Tables(0).Rows.Count = 0 Then
            RetValue = DisplayMessage("Cannot find last vote ID for session " & gSessionName & _
               " in ALIS. The last vote ID will be set to 0.  If this is not correct, you must enter the last vote ID using the Parameter Maintenance form.", "No Vote ID In ALIS", "S")
            gLastVoteIDPerALIS = 0
        Else
            For Each drA1 In dsA1.Tables(0).Rows
                If IsDBNull(drA1("LastVoteID")) Then
                    gLastVoteIDPerALIS = 0
                Else
                    gLastVoteIDPerALIS = CInt(drA1("LastVoteID"))
                End If
            Next
        End If

        '--- now get see if there is a higher local vote id not yet sent to ALIS
        '--- if so, then update the last vote with it
        If gTestMode = False Then
            strSQL = "SELECT MAX(VoteID) AS LastVoteID FROM tblRollCallVotes WHERE SessionID = " & gSessionID & " AND AlisVoteOID = 0 and Test = 0 "
        Else
            strSQL = "SELECT MAX(VoteID) AS LastVoteID FROM tblRollCallVotes WHERE SessionID = " & gSessionID & " AND AlisVoteOID = 0 and Test = 1 "
        End If
        dsV = V_DataSet(strSQL, "R")
        If dsV.Tables(0).Rows.Count = 0 Then
            gLastVoteIDPerLocal = 0
        Else
            For Each drV In dsV.Tables(0).Rows
                If IsDBNull(drV("LastVoteID")) Then
                    gLastVoteIDPerLocal = 0
                Else
                    gLastVoteIDPerLocal = drV("LastVoteID")
                End If
            Next
        End If


        If gLastVoteIDPerLocal > gLastVoteIDPerALIS Then
            gVoteID = gLastVoteIDPerLocal
        Else
            gVoteID = gLastVoteIDPerALIS
        End If

        If gTestMode And gLastVoteIDPerLocal = 0 Then
            gVoteID = 0
        End If

        '--- Now update the tblVoteParameters table
        If gTestMode = False Then
            V_DataSet("Update tblVotingParameters SET ParameterValue='" & gVoteID & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSION'", "U")
        Else
            V_DataSet("Update tblVotingParameters SET ParameterValue='" & gVoteID & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSIONTEST'", "U")
        End If
    End Sub

#Region "orig-olds"

    '#Region "Gobal Variables"
    '    '*************************************************************************************************************
    '    ' Copyright @ 2014, Alabama State Senate Data Center

    '    ' Visual Studio 2010
    '    '*************************************************************************************************************

    '    'Public SVOTE As String = "LEGI1"                   '   "LEGI1"      --- Dowe PC - it is a master pc
    '    'Public SDIS As String = "LEGPC39"                  '   "LEGPC39"    --- Sara PC
    '    'Public SOOB As String = "LEGPC39"                   '   "LEGP13"     --- Show HTML on the Wall Projection PC
    '    Public SVOTE As String = "SENATEVOTING"             '   "SENATEVOTING"     --- Dowe PC - it is a master pc
    '    Public SDIS As String = "SENATEDISPLAY"             '   "SENATEDISPLAY"   --- Sara PC
    '    Public SOOB As String = "SENATEOOB"                 '   "SENATEOOB"     --- Show HTML on the Wall Projection PC
    '    Public Const gPhraseInsertionPoint As String = "*"
    '    Public Const gSenatorInsertionPoint As String = "^"
    '    Public Const gCommitteeInsertionPoint As String = "~"
    '    Public Const gTextInsertionPoint As String = "?"
    '    Public Const gDesignNbrSenators = 35
    '    Public Const gBIR As String = "Budget Isolation Resolution."
    '    Public Const gSenateID As Short = 1753
    '    Public LocalDatabasePath = ""                        'C:\VotingSystem\VoteSys.mdb"
    '    Public gCmdFile As String = "c:\VotingSystem\RestartMsgQueue.cmd"


    '    Public gPingPCTimer As Integer
    '    Public gSessionID As Integer
    '    Public gCalendar, gBill, gApplication, gCalendarCode, gBillNbr As String
    '    Public gPutParams As Boolean
    '    Public gNbrPhrasesToDisplay As Short
    '    Public gDefaultPrinter, gSecondaryPrinter As String
    '    Public gCurrentPhrase, gMessage As New Object
    '    Public gSenatorName(), gSenatorDistrictName(), gCommittees(), gCommitteeAbbrevs(), gThePhrases(), gPhraseCodes(), gPhrases(), gPhraseForVotePC() As String
    '    Public gOOBHeadingAttributes, gNotSelectedOOBAttributes As String
    '    Public gSelectedOOBAttributes, gBillAttributes As String
    '    Public gIndex As Integer
    '    Public gPhraseAttributes, gSubjectAttributes As String
    '    Public gStart, gSVoteID As Integer
    '    Public gSenator, gVoteID, gSubject As String
    '    Public gNbrCommittees, gNbrPhrases As Short
    '    Public gALISTimer As Integer
    '    Public gSalutation() As String
    '    Public gSource, gComputerName As String
    '    Public gBillDisplayed, gCalendarDisplayed, gAction As String
    '    Public gCalendarCodeDisplayed, gBillNbrDisplayed As String
    '    Public gFreeFormat As Boolean
    '    Public gSavePhraseHeight, gVotingTimer As Integer
    '    Public gSenatorNameOrder() As String
    '    Public gMessageType, gMessageTitle As String
    '    Public gTestMode, gCOnly, gChamberHelp, gVotingStarted, gReSendALIS As Boolean
    '    Public gErrorCount As Short
    '    Public gPutBill, gPutCalendar, gPutCalendarCode, gPutBillNbr As String
    '    Public gPutPhrase As Object
    '    Public gPutChamberLight, gPutVotingLight As Object
    '    Public gCheckingParams As Boolean
    '    Public gSessionName, gSessionAbbrev As String
    '    Public gLegislativeDay, gLastLegislativeDay, gCalendarDay As String
    '    Public gVotingHelp, gPrintVoteRpt, gSendVotesToALIS As Boolean
    '    Public gWorkData As Object
    '    Public gHTMLFile, gNbrSenator As String
    '    Public gNbrSenators As Integer
    '    Public gSenatorSplit As Short
    '    Public gSenatorOID() As Integer
    '    Public gDistrictOID() As Integer
    '    Public gWriteVotesToTest As Boolean
    '    Public gLegislativeDayOID As Integer
    '    Public gNbrVoteHistoryDays As Short
    '    Public gSessionIDTmp() As Integer
    '    Public gLegislativeDayTmp() As Short
    '    Public gLegislativeDayOIDTmp() As Integer
    '    Public gCalendarDayTmp() As Object
    '    Public gSessionNameTmp() As String
    '    Public gSessionAbbrevTmp() As String
    '    Public gDeleteTestVotesOnStartUp As Boolean
    '    Public gDataComputerName As String
    '    Public SOOBAlive, WoodyAlive As Boolean
    '    Public gLastVoteIDPerALIS, gLastVoteIDPerLocal As Integer

    '    Public gOnlyOnePC As Boolean
    '    Public frmMessage As New frmMessage

    '    Public connProda As OleDbConnection
    '    Public connProdb As OleDbConnection
    '    Public connTest As OleDbConnection
    '    Public strVoteTest As String                            '   connection string for ALIS_TEST oracle database
    '    Public strALIS As String                                '   connection string for ALIS Oracle Production database
    '    Public connLocal As OleDbConnection = Nothing           '   connection local C:\VotingSystem\VoteSys.mdb database
    '    Public cnOOB As OleDbConnection
    '    Public cnALIS As OleDbConnection
    '    Public v_Rows, a_Rows As Integer
    '    Public strSQL As String
    '    Public cDateTime As String
    '    Public gStrALISProd As String
    '    Public gStrALISTest As String
    '    Public gDatabasePath As String
    '    Public strLocal As String
    '    Public gVotingPath As String
    '    Public gLogFile, gOOBTextFile As String
    '    Public msgText As String = "Alabama Senate Voting System"
    '    Public gSendQueueFromVotePC, gSendQueueFromDisplay, gSendQueueToVotePC, gSendQueueToDisplay, gSendQueueToOOB, gReceiveQueueName, gRequestVoteIDQueue, gSendQueueTimer, gReceiveQueueTimer, gHTMLTimer, tmpWrkFld, Alis_VoteID, gVoteIDTest, gLocalHTMLPage As String
    '    Public gProduction, LockReadOnly, vSet As Boolean
    '    Public gDisplay_IPAddress, gVotePC_IPAddress, gOOBPC_IPAddress, gOOBPowerPointFilePath, gLastVoteIDForThisSessionTEST As String
    '    Public gOOB_Conn, gALIS_Primary_Conn, gALIS_Secondary_Conn, gALIS_TEST_Conn, gWriteToAlisVoteTable, gWriteToAlisIndividualVoteTable As String
    '    Public SVotePC_On, SDisplay_On, SOOB_On, AlisProda_On, AlisProdb_On, AlisTest_On, Network_On, gNetwork, gWorkingGroup, tableExist, gCreateHTMLPage, rightClick As Boolean
    '    Public tPage0, tPage1, tPage2, bSenator, bPhrase, dataProcess, DisplayImage, calendarClick As Boolean
    '    Public setR1, setR2, setR3 As Object

    '    '-- pass all of values to frmChamberDisplay form for clicking Phrase or Senator from frmPhrase or frmSenator form
    '    Public pBill, pVoteID, pCSession, pLegDate, pSenator, pCommittee, pBusiness, pCCalendar, pPhrase, pOBusiness, pCalendar, pCBill, pInsertText, pWorkData1, pWorkData2, pWorkData3 As String
    '    Public K, Index, gAllowedShortVoteCnt, v_id As Integer         '-- if there are more than one LEG day open, k will > 0

    '    Declare Function MoveWindow Lib "user32" (ByVal hwnd As Integer, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal bRepaint As Integer) As Integer
    '    Public Declare Function GetAsyncKeyState Lib "user32.dll" (ByVal vKey As Int32) As UShort
    '    Declare Function WriteProfileString Lib "kernel32" _
    '                    Alias "WriteProfileStringA" _
    '                    (ByVal lpszSection As String, _
    '                    ByVal lpszKeyName As String, _
    '                    ByVal lpszString As String) As Long

    '    Public frmChamberDisplay As New frmChamberDisplay
    '    Public frmPhraseShow As New frmPhraseShow
    '    Public frmSenatorShow As New frmSenatorShow
    '    Public frmVote As New frmVote


    '    Public PhraseDisplayBoard, BillDispalyBoard, OOBDisplayBoard As Integer  '--New
    '    Public bodyParamters As String
    '    Public OOBBackgroudChang, DragDrop1, DragDrop2, DragDrop3, askVoteID As Boolean
    '#End Region

    '    Function DisplayMessage(ByRef TheMessage As String, ByRef MesgTheTitle As String, ByRef MesgType As String) As Object
    '        Dim frmMessage As New frmMessage
    '        Try
    '            '--- if no message type is passed, then just display and continue
    '            '--- processing - this is just a status message
    '            ERHPushStack(("DisplayMessage - " & TheMessage))

    '            gMessage = TheMessage
    '            gMessageType = MesgType
    '            gMessageTitle = NToB(MesgTheTitle)
    '            frmMessage.Show()
    '            If gMessageType <> "" Then
    '                WaitUntilFormCloses("frmMessage")
    '            End If
    '            DisplayMessage = gMessage

    '            ERHPopStack()
    '            Exit Function
    '        Catch
    '            ERHHandler()
    '        End Try
    '    End Function

    '    Public Function FileExistes(ByVal FileName As String) As Integer
    '        Try
    '            FileExistes = True
    '            FileOpen(89, FileName, OpenMode.Input)
    '            FileClose(89)

    '            frmVote.setRTimer.Enabled = True
    '            frmVote.RequestVoteIDTimer.Enabled = True

    '        Catch ex As Exception
    '            frmVote.setRTimer.Enabled = False
    '            frmVote.RequestVoteIDTimer.Enabled = False

    '            MsgBox(ex.Message & " Please close local database.", vbCritical, "Check Local Database File")

    '            End
    '        End Try
    '    End Function

    '    Public Sub Has_Network_Connectivity()
    '        Dim hostInfo As System.Net.IPHostEntry
    '        Try
    '            '---to look for the logon server
    '            Dim sServer As String = Environment.GetEnvironmentVariable("logonserver")
    '            hostInfo = System.Net.Dns.GetHostEntry(sServer.Remove(0, 2))
    '            Network_On = True
    '        Catch
    '            '---there is no network connection
    '            AlisProda_On = False
    '            AlisProdb_On = False
    '            AlisTest_On = False
    '            gWriteVotesToTest = False
    '            Network_On = False

    '            DisplayMessage("Network interruped. Please reset voting system computers to Local Working Group to continue the process.", "Error Occurred: Has_Network_Connectivity()", "S")
    '            gWorkingGroup = True
    '            Application.Exit()
    '        End Try
    '    End Sub

    '    Public Sub Check_ALIS_Database_Accessible()
    '        connProda = New OleDbConnection(gALIS_Secondary_Conn)
    '        connProdb = New OleDbConnection(gALIS_Primary_Conn)
    '        connTest = New OleDbConnection(gALIS_TEST_Conn)
    '        Dim strText As String = "Execute: Check_ALIS_Database_Accessible()"
    '        Dim RetValue As New Object

    '        Try
    '            Try
    '                '---Check ALIS_PRODB - alis primary
    '                connProdb.Open()
    '                If connProdb.State = ConnectionState.Open Then
    '                    AlisProdb_On = True
    '                    gNetwork = True
    '                End If
    '            Catch
    '                gNetwork = False
    '            End Try
    '            connProdb.Close()


    '            Try
    '                '---Check ALIS_PRODA - alis secondary
    '                connProda.Open()
    '                If connProda.State = ConnectionState.Open Then
    '                    gNetwork = True
    '                    AlisProda_On = True
    '                End If
    '            Catch
    '                AlisProda_On = False
    '            End Try
    '            connProda.Close()


    '            Try
    '                '---Check ALIS_TEST
    '                connTest.Open()
    '                If connTest.State = ConnectionState.Open Then
    '                    gNetwork = True
    '                    AlisTest_On = True
    '                End If
    '            Catch ex As Exception
    '                AlisTest_On = False
    '                MsgBox(ex.Message, vbCritical, "Check_ALIS_Ddatabase_Accessible")
    '            End Try
    '            connTest.Close()


    '            '--- Initial oledbConnection to connect to local VotingSys.mdb database
    '            '--- 1. Read/Write production to ALIS database
    '            If gTestMode = False And gWriteVotesToTest = False Then
    '                If AlisProdb_On And AlisProda_On Then
    '                    strALIS = gALIS_Primary_Conn
    '                    cnALIS = New OleDbConnection(strALIS)
    '                    RetValue = DisplayMessage("ALIS Oracle Databases is accessible", strText, "I")
    '                ElseIf AlisProdb_On = False And AlisProda_On Then
    '                    strALIS = gALIS_Secondary_Conn
    '                    cnALIS = New OleDbConnection(strALIS)
    '                    RetValue = DisplayMessage("Attention: Only able access the ALIS_PRODA database.", strText, "I")
    '                ElseIf AlisProdb_On And AlisProda_On = False Then
    '                    strALIS = gALIS_Primary_Conn
    '                    cnALIS = New OleDbConnection(strALIS)
    '                    RetValue = DisplayMessage("Attention: Only able access the ALIS_PRODB database.", strText, "I")
    '                ElseIf AlisProdb_On = False And AlisProda_On = False Then
    '                    gNetwork = False
    '                    If DisplayMessage("Attention: unable write voting data to ALIS production database. Would you want continue process by local 'Working Group'? Otherwise system will shut down.", "Unable Connecte to ALIS", "Y") Then
    '                        gWorkingGroup = True
    '                        cnALIS = Nothing
    '                    Else
    '                        Application.Exit()
    '                        '!!! write log file here!
    '                        '.
    '                        '.
    '                    End If
    '                ElseIf AlisProdb_On = False And AlisProda_On = False And AlisTest_On = False Then
    '                    gNetwork = False

    '                    If DisplayMessage("Attention: unable write votes to ALIS Production or ALIS Test database. Would you want continue process by local 'Working Group'? Otherwise system will shut down.", "Unable Connecte to ALIS", "Y") Then
    '                        gWorkingGroup = True
    '                        cnALIS = Nothing
    '                    Else
    '                        Application.Exit()
    '                        '!!! write log file here!
    '                        '.
    '                        '.
    '                    End If

    '                End If
    '            End If

    '            '--- 2. No read / no write from ALIS database
    '            '--- if in text and not writing votes, then there is no interaction
    '            '--- with ALIS, so set up dummy values; always delete test session votes on startup;
    '            If gTestMode And gWriteVotesToTest = False Then
    '                DisplayMessage("Attention: You are in TEST MODE without write vote to ALIS.", "Execute: Check_ALIS_Database_Accessible()", "S")
    '                V_DataSet("DELETE FROM tblRollCallVotes WHERE SessionID = -1", "D")
    '                gSessionID = -1
    '                gLegislativeDayOID = 1
    '                gLegislativeDay = 1
    '                gCalendarDay = Now.ToShortDateString
    '                gVoteID = 0
    '                V_DataSet("UPDATE tblVotingParameters Set ParameterValue = '0' Where Parameter ='LastVoteIDForThisSession'", "U")
    '                strALIS = gALIS_TEST_Conn
    '                cnALIS = New OleDbConnection(strALIS)
    '            End If

    '            '--- 3. Read/Write from ALIS_TEST
    '            If gTestMode And gWriteVotesToTest And AlisTest_On Then
    '                strVoteTest = gALIS_TEST_Conn
    '                cnALIS = New OleDbConnection(strVoteTest)
    '                RetValue = DisplayMessage("Attention: Current is TEST MODE and write votes to ALIS_TEST database.", strText, "I")
    '            ElseIf gTestMode And gWriteVotesToTest And AlisTest_On = False Then

    '                MsgBox("Attention: unable access ALIS_TEST database, system will shutdown. Please contact to your Administrator!", vbCritical, "Check_ALIS_Ddatabase_Accessible")
    '                End
    '            End If

    '            '--- 4. Not Allowed!
    '            '  Dim sw As New Stopwatch
    '            If gTestMode = False And gWriteVotesToTest = True Then
    '                'RetValue = DisplayMessage("Current system is on PRODUCTION MODE." & vbCrLf & "Invalid Parameter WriteVoteToTest = true! Please change it equle to false! ", strText, "I")
    '                V_DataSet("UPDATE tblVotingParameters Set ParameterValue ='N'  Where Parameter ='WRITEVOTESTOALISTEST'", "U")

    '                If AlisProda_On And AlisProda_On Then
    '                    cnALIS = New OleDbConnection(gALIS_Primary_Conn)
    '                ElseIf AlisProda_On And AlisProda_On = False Then
    '                    cnALIS = New OleDbConnection(gALIS_Primary_Conn)
    '                ElseIf AlisProdb_On = False And AlisProda_On Then
    '                    cnALIS = New OleDbConnection(gALIS_Secondary_Conn)
    '                End If
    '                cnALIS.Open()
    '                Exit Sub
    '            End If

    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Execute: Check_ALIS_Database_Accessible()", "I")
    '            Exit Sub
    '        End Try
    '    End Sub

    '    Public Sub Get_Parameters()
    '        Dim ds As New DataSet
    '        Dim dsP As New DataSet
    '        Dim parameters As New ArrayList()
    '        Dim RepValue As New Object
    '        Dim sqlStr As String = "Select * From tblVotingParameters Where ucase(Parameter) = "
    '        Dim strFlag As String

    '        Try

    '            '--- Builte power on PC's access database connection string to get parameters first
    '            If gComputerName <> "" Then
    '                gDataComputerName = gComputerName

    '                '--- Get Voting system parameters from power on PC

    '                '*** Dowe's Vote Master PC parameters
    '                gVotePC_IPAddress = QueryParameters(sqlStr & "'VOTEPCIPADDRESS'").Tables(0).Rows(0).Item("ParameterValue")
    '                SVOTE = QueryParameters(sqlStr & "'VOTEPCNAME'").Tables(0).Rows(0).Item("ParameterValue")

    '                '*** Sara's Display PC parameters
    '                gDisplay_IPAddress = QueryParameters(sqlStr & "'DISPLAYPCIPADDRESS'").Tables(0).Rows(0).Item("ParameterValue")
    '                SDIS = QueryParameters(sqlStr & "'DISPLAYPCNAME'").Tables(0).Rows(0).Item("ParameterValue")

    '                '*** OOB show on the wall PC parameters
    '                gOOBPC_IPAddress = QueryParameters(sqlStr & "'OOBPCIPADDRESS'").Tables(0).Rows(0).Item("ParameterValue")
    '                gOOB_Conn = QueryParameters(sqlStr & "'OOBPCCONNECTION'").Tables(0).Rows(0).Item("ParameterValue")
    '                SOOB = QueryParameters(sqlStr & "'OOBPCNAME'").Tables(0).Rows(0).Item("ParameterValue")

    '                '*** ALIS Production database parameters
    '                gALIS_Primary_Conn = QueryParameters(sqlStr & "'ALISPRIMARY'").Tables(0).Rows(0).Item("ParameterValue")
    '                gALIS_Secondary_Conn = QueryParameters(sqlStr & "'ALISSECONDARY'").Tables(0).Rows(0).Item("ParameterValue")
    '                gALIS_TEST_Conn = QueryParameters(sqlStr & "'ALISTEST'").Tables(0).Rows(0).Item("ParameterValue")

    '                gVotingPath = QueryParameters(sqlStr & "'VOTINGSYSTEMPATH'").Tables(0).Rows(0).Item("ParameterValue")
    '                gDatabasePath = QueryParameters(sqlStr & "'DATABASEPATH'").Tables(0).Rows(0).Item("ParameterValue")
    '                If UCase(gDatabasePath.Trim) <> UCase(LocalDatabasePath.Trim) Then
    '                    gDatabasePath = strLocal.Trim
    '                    V_DataSet("Update tblVotingParameters SET ParameterValue='" & LocalDatabasePath.Trim & "' Where ucase(Parameter)='DATABASEPATH'", "U")
    '                    DisplayMessage("Make sure local database path is: " & vbCrLf & LocalDatabasePath.Trim, msgText, "I")
    '                End If

    '                '*** send and receive Queue parameters
    '                gSendQueueFromVotePC = QueryParameters(sqlStr & "'SENDQUEUEFROMVOTEPC'").Tables(0).Rows(0).Item("ParameterValue")
    '                gSendQueueFromDisplay = QueryParameters(sqlStr & "'SENDQUEUEFROMDISPLAY'").Tables(0).Rows(0).Item("ParameterValue")
    '                gSendQueueToOOB = QueryParameters(sqlStr & "'SENDQUEUETOOOB'").Tables(0).Rows(0).Item("ParameterValue")
    '                gSendQueueToVotePC = QueryParameters(sqlStr & "'SENDQUEUETOVOTEPC'").Tables(0).Rows(0).Item("ParameterValue")
    '                gSendQueueToDisplay = QueryParameters(sqlStr & "'SENDQUEUETODISPLAY'").Tables(0).Rows(0).Item("ParameterValue")
    '                gReceiveQueueName = QueryParameters(sqlStr & "'RECEIVEQUEUENAME'").Tables(0).Rows(0).Item("ParameterValue")
    '                gRequestVoteIDQueue = QueryParameters(sqlStr & "'REQUESTVOTEIDQUEUE'").Tables(0).Rows(0).Item("ParameterValue")


    '                gLocalHTMLPage = QueryParameters(sqlStr & "'LOCALHTMLPAGE'").Tables(0).Rows(0).Item("ParameterValue")
    '                gALISTimer = QueryParameters(sqlStr & "'ALISTIMER'").Tables(0).Rows(0).Item("ParameterValue")
    '                gPingPCTimer = QueryParameters(sqlStr & "'PINGPCTIMER'").Tables(0).Rows(0).Item("ParameterValue")
    '                gVotingTimer = QueryParameters(sqlStr & "'VOTINGTIMER'").Tables(0).Rows(0).Item("ParameterValue")
    '                gReceiveQueueTimer = QueryParameters(sqlStr & "'RECEIVEQUEUETIMER'").Tables(0).Rows(0).Item("ParameterValue")
    '                gOOBTextFile = QueryParameters(sqlStr & "'OOBTEXTFILE'").Tables(0).Rows(0).Item("ParameterValue")
    '                gNbrVoteHistoryDays = QueryParameters(sqlStr & "'NBRVOTEHISTORYDAYS'").Tables(0).Rows(0).Item("ParameterValue")
    '                gLastLegislativeDay = QueryParameters(sqlStr & "'LASTLEGISLATIVEDAY'").Tables(0).Rows(0).Item("ParameterValue")
    '                gNbrPhrasesToDisplay = QueryParameters(sqlStr & "'NUMBEROFPHRASESTODISPLAY'").Tables(0).Rows(0).Item("ParameterValue")
    '                gNbrSenator = QueryParameters(sqlStr & "'NUMBEROFSENATOR'").Tables(0).Rows(0).Item("ParameterValue").ToString
    '                gNbrSenators = CType(gNbrSenator, Integer)
    '                gOOBPowerPointFilePath = QueryParameters(sqlStr & "'OOBPOWERPOINTFILEPATH'").Tables(0).Rows(0).Item("ParameterValue")
    '                gHTMLFile = QueryParameters(sqlStr & "'HTMLFILE'").Tables(0).Rows(0).Item("ParameterValue")
    '                gHTMLTimer = QueryParameters(sqlStr & "'HTMLTIMER'").Tables(0).Rows(0).Item("ParameterValue")
    '                gLogFile = QueryParameters(sqlStr & "'LOGFILE'").Tables(0).Rows(0).Item("ParameterValue")
    '                gSendQueueTimer = QueryParameters(sqlStr & "'SENDQUEUETIMER'").Tables(0).Rows(0).Item("ParameterValue")
    '                gWriteToAlisVoteTable = QueryParameters(sqlStr & "'WRITETOALISVOTETABLEFILE'").Tables(0).Rows(0).Item("ParameterValue")
    '                gWriteToAlisIndividualVoteTable = QueryParameters(sqlStr & "'WRITETOALISINDIVIDUALVOTETABLE'").Tables(0).Rows(0).Item("ParameterValue")
    '                gAllowedShortVoteCnt = QueryParameters(sqlStr & "'ALLOWEDSHORTVOTECNT'").Tables(0).Rows(0).Item("ParameterValue")
    '                gSecondaryPrinter = QueryParameters(sqlStr & "'SECONDARYPRINTER'").Tables(0).Rows(0).Item("ParameterValue")
    '                gCmdFile = QueryParameters(sqlStr & "'COMMANDFILE'").Tables(0).Rows(0).Item("ParameterValue")

    '                strFlag = UCase(QueryParameters(sqlStr & "'CREATEHTMLPAGE'").Tables(0).Rows(0).Item("ParameterValue"))
    '                If strFlag = "Y" Then
    '                    gCreateHTMLPage = True
    '                Else
    '                    gCreateHTMLPage = False
    '                End If
    '                strFlag = UCase(QueryParameters(sqlStr & "'PRINTVOTEREPORT'").Tables(0).Rows(0).Item("ParameterValue"))
    '                If strFlag = "Y" Then
    '                    gPrintVoteRpt = True
    '                Else
    '                    gPrintVoteRpt = False
    '                End If
    '                strFlag = UCase(QueryParameters(sqlStr & "'TESTMODE'").Tables(0).Rows(0).Item("ParameterValue"))
    '                If strFlag = "Y" Then
    '                    gTestMode = True
    '                    gSVoteID = QueryParameters(sqlStr & "'LASTVOTEIDFORTHISSESSIONTEST'").Tables(0).Rows(0).Item("ParameterValue")
    '                    gVoteID = CType(gSVoteID, Integer)
    '                Else
    '                    gTestMode = False
    '                    gSVoteID = QueryParameters(sqlStr & "'LASTVOTEIDFORTHISSESSION'").Tables(0).Rows(0).Item("ParameterValue")
    '                    gVoteID = CType(gSVoteID, Integer)
    '                End If
    '                strFlag = UCase(QueryParameters(sqlStr & "'WRITEVOTESTOALISTEST'").Tables(0).Rows(0).Item("ParameterValue"))
    '                If strFlag = "Y" Then
    '                    gWriteVotesToTest = True

    '                Else
    '                    gWriteVotesToTest = False
    '                End If
    '                strFlag = UCase(QueryParameters(sqlStr & "'DELETETESTVOTESONSTART'").Tables(0).Rows(0).Item("ParameterValue"))
    '                If strFlag = "Y" Then
    '                    gDeleteTestVotesOnStartUp = True
    '                Else
    '                    gDeleteTestVotesOnStartUp = False
    '                End If
    '                strFlag = UCase(QueryParameters(sqlStr & "'CHAMBERHELP'").Tables(0).Rows(0).Item("ParameterValue"))
    '                If strFlag = "Y" Then
    '                    gChamberHelp = True
    '                Else
    '                    gChamberHelp = False
    '                End If
    '                strFlag = UCase(QueryParameters(sqlStr & "'VOTINGHELP'").Tables(0).Rows(0).Item("ParameterValue"))
    '                If strFlag = "Y" Then
    '                    gVotingHelp = True
    '                Else
    '                    gVotingHelp = False
    '                End If
    '            End If
    '            Exit Sub
    '        Catch ex As Exception
    '            DisplayMessage(ex.Message & vbCrLf & "Get Voting Parameters Is Failed! Voting System Will Shut Down. Please Try Run It Again.", "Execute: Get_Parameters()", "S")
    '            Application.Exit()
    '        End Try
    '    End Sub

    '    Public Sub Check_PCs_Status()
    '        Dim RetValue As New Object
    '        Dim Ping As Ping
    '        Dim pReply As PingReply
    '        Dim mes As String = "Execute: Check_PCs_Status()"

    '        Try

    '            Ping = New Ping

    '            '--- ping: Member Vote Computer - Dowe's PC
    '            Try
    '                pReply = Ping.Send(gVotePC_IPAddress)
    '                If pReply.Status = IPStatus.Success Then
    '                    SVotePC_On = True
    '                Else
    '                    SVotePC_On = False
    '                End If
    '            Catch ex As Exception
    '                GoTo pingDispaly
    '            End Try


    'pingDispaly:
    '            Try
    '                '--- ping: Display Computer - Sara's PC
    '                pReply = Ping.Send(gDisplay_IPAddress)
    '                If pReply.Status = IPStatus.Success Then
    '                    SDisplay_On = True
    '                Else
    '                    SDisplay_On = False
    '                End If
    '            Catch ex As Exception
    '                GoTo pingOOC
    '            End Try


    'pingOOC:
    '            '--- ping: Operate Order of Business Computer - show html on the projection PC
    '            If UCase(SDIS) = UCase(System.Environment.MachineName) Then
    '                Try
    '                    pReply = Ping.Send(gOOBPC_IPAddress)
    '                    If pReply.Status = IPStatus.Success Then
    '                        SOOB_On = True
    '                        cnOOB = New OleDbConnection(gOOB_Conn)
    '                        RetValue = DisplayMessage("Senate Order of Business Computer Is On.", mes, "I")
    '                    Else
    '                        SOOB_On = False
    '                        cnOOB = Nothing
    '                        RetValue = DisplayMessage("Senate Order of Business Computer Is Off." & vbCrLf & " Unable Display Vote Informations.", "Check_PCs_Status", "I")
    '                    End If
    '                Catch ex As Exception
    '                    GoTo continueProc
    '                End Try
    '            End If

    'continueProc:
    '            '--- assign local database connection
    '            connLocal = New OleDbConnection(strLocal)


    '            '--- if only single PC On recording it
    '            gCOnly = False

    '            '--- Clear stored workarea and onlyOnePC informations
    '            V_DataSet("Delete From tblOnlyOnePC", "D")

    '            If SVotePC_On And SDisplay_On Then
    '                gOnlyOnePC = False

    '                If UCase(gComputerName) = UCase(SDIS) Then
    '                    RetValue = DisplayMessage("Senate Voting Computer Is On.", "Check PCs Status", "I")
    '                End If
    '                If UCase(gComputerName) = UCase(SVOTE) Then
    '                    RetValue = DisplayMessage("Senate Chamber Display Computer Is On.", "Check PCs Status", "I")
    '                End If
    '            ElseIf SVotePC_On And SDisplay_On = False Then
    '                V_DataSet("Delete From tblOnlyOnePC", "D")
    '                V_DataSet("Insert Into tblOnlyOnePC Values (1, '" & SVOTE & "','')", "A")
    '                If Not DisplayMessage("Senate Chamber Display Computer Is Off. Would You Want Continue Without Chamber Display?", "Chamber Dispaly PC is Offline", "Y") Then
    '                    End
    '                End If
    '                gOnlyOnePC = True
    '            ElseIf SVotePC_On = False And SDisplay_On Then
    '                If Not DisplayMessage("Senate Voting Computer Is Off." & vbCrLf & "Would You Want Continue Without Voting?", "Vote PC is Offline", "Y") Then
    '                    End
    '                End If
    '                V_DataSet("Delete From tblOnlyOnePC", "D")
    '                V_DataSet("Insert Into tblOnlyOnePC Values (1, '" & SDIS & "','')", "A")
    '                gOnlyOnePC = True
    '            End If



    '            If gCOnly Then
    '                If SVotePC_On Then
    '                    gDatabasePath = "C:\VotingSystem\SenateVotes.mdb"
    '                ElseIf SDisplay_On Then
    '                    gDatabasePath = "C:\VotingSystem\SenateChamberDisplay.mdb"
    '                End If
    '                gHTMLFile = "c:\VotingSystem\CurrentMatter.htm"
    '            End If
    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Check_PCs_Status()", "S")
    '            Exit Sub
    '        End Try
    '    End Sub

    '    Public Sub Main()
    '        Dim ds As New DataSet
    '        Dim dsP As New DataSet
    '        Dim RetValue As New Object, CheckDate As Date, x As Long, WrkFld As String

    '        gNetwork = True
    '        gWriteVotesToTest = False
    '        gOnlyOnePC = False
    '        gWorkingGroup = False
    '        gTestMode = False

    '        Try
    '            '--- get local database connectin string
    '            If UCase(System.Environment.MachineName) = UCase(SDIS) Then
    '                strLocal = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\VotingSystem\SenateChamberDisplay.mdb;User ID=admin;Password=;"
    '                LocalDatabasePath = "C:\VotingSystem\SenateChamberDisplay.mdb"
    '                ' strLocal = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\VotingSystem\SenateChamberDisplay.accdb;User ID=admin;Password=;"
    '                ' LocalDatabasePath = "C:\VotingSystem\SenateChamberDisplay.accdb"
    '            ElseIf UCase(System.Environment.MachineName) = UCase(SVOTE) Then
    '                strLocal = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\VotingSystem\SenateVotes.mdb;User ID=admin;Password=;"
    '                LocalDatabasePath = "C:\VotingSystem\SenateVotes.mdb"
    '                ' strLocal = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\VotingSystem\SenateVotes.accdb;User ID=admin;Password=;"
    '                ' LocalDatabasePath = "C:\VotingSystem\SenateVotes.accdb"
    '            End If


    '            If FileExistes(LocalDatabasePath) Then
    '                '--- change mouse pointer to waiting pointer
    '                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

    '                '--- get computer name to access local VoteSys.mdb database 
    '                gComputerName = Trim(System.Environment.MachineName)

    '                '--- initial connected to local VoteSys.mdb database
    '                connLocal = New OleDbConnection(strLocal)

    '                '--- get initial voting parameters from local VoteSys.mdb Access Database
    '                Get_Parameters()

    '                '--- initial Application
    '                If gComputerName = SVOTE Then
    '                    gApplication = "Voting"
    '                ElseIf gComputerName = SDIS Then
    '                    gApplication = "Chamber"
    '                End If

    '                '--- check Chamber Display, Voting, and OOB PCs Status
    '                Check_PCs_Status()

    '                '--- check Network Connectivity 
    '                Has_Network_Connectivity()

    '                '--- check Oracle database 
    '                Check_ALIS_Database_Accessible()


    '                '--- get default printer name
    '                Dim p As Printing.PrinterSettings
    '                p = New Printing.PrinterSettings()
    '                gDefaultPrinter = p.PrinterName

    '                'If gMessage = True Then
    '                InitialSpecailOrderCalendar()
    '                'End If


    '                '--- get Legislative day 
    '                GetLegDay()

    '                '--- download Bills from ALIS, on Chamber Display site always do it, Voting side is optional
    '                If (gTestMode = False And gWriteVotesToTest = False) Or (gTestMode = True And gWriteVotesToTest = True) Then
    '                    If UCase(gComputerName) = UCase(SDIS) Then
    '                        Dim dsCheckW As New DataSet
    '                        dsCheckW = V_DataSet("Select ParameterValue From tblVotingParameters Where UCASE(Parameter) ='LASTLEGISLATIVEDAY'", "R")
    '                        For Each drCW As DataRow In dsCheckW.Tables(0).Rows
    '                            If Not drCW("ParameterValue") = gLegislativeDay Then
    '                                Cursor.Current = Cursors.WaitCursor
    '                                DownloadBillsFromALIS()
    '                                Cursor.Current = Cursors.Default
    '                            End If
    '                        Next
    '                    Else
    '                        '--- skip down load bills from alis, beacuse it is a same LEG day
    '                    End If
    '                Else
    '                    If gTestMode Then
    '                        Dim dsTESTVoteID As New DataSet
    '                        strSQL = "SELECT ParameterValue FROM tblVotingParameters WHERE ucase(Parameter) ='LASTVOTEIDFORTHISSESSIONTEST' "
    '                        dsTESTVoteID = V_DataSet(strSQL, "R")
    '                        If dsTESTVoteID.Tables(0).Rows.Count = 0 Then
    '                            gVoteID = 1
    '                        Else
    '                            For Each dr As DataRow In dsTESTVoteID.Tables(0).Rows
    '                                gVoteID = dr("ParameterValue") + 1
    '                            Next
    '                        End If
    '                    End If
    '                End If


    '                '--- for sendisplay, set the paths to files on this PC; for senvote set the paths to files on sendisplay
    '                '--- NOTE: KEEP COMPUTER NAMES IN CAPS
    '                x = 40
    '                WrkFld = Space(40)

    '                '--- Before download Senator records, resize array. 
    '                gSenatorSplit = Int((gNbrSenators / 2) + 0.5)
    '                ReDim gSenatorName(gNbrSenators)
    '                ReDim gSalutation(gNbrSenators)
    '                ReDim gSenatorNameOrder(gNbrSenators)
    '                ReDim gSenatorOID(gNbrSenators)
    '                ReDim gDistrictOID(gNbrSenators)

    '                '--- load Senator records from local database
    '                LoadSenatorsIntoArray()


    '                '--- these test votes are different from session test votes deleted above;
    '                If gDeleteTestVotesOnStartUp Then
    '                    V_DataSet("Delete From tblRollCallVotes Where Test= 1", "D")   ' delete any test votes
    '                End If

    '                '--- delete roll call votes per # of history days; if days=999, then no delete
    '                If gNbrVoteHistoryDays < 999 Then
    '                    CheckDate = DateAdd(DateInterval.Day, -gNbrVoteHistoryDays, Date.Today)
    '                    V_DataSet("Delete From tblRollCallVotes Where VoteDate < " & "#" & CheckDate & "#", "D")
    '                End If


    '                '--- load the phrases into an array
    '                LoadPhrasesIntoArray()

    '                '--- load the committees into an array
    '                LoadCommitteesIntoArray()

    '                '--- initial tblSenatorsVote

    '                'ERHPopStack()
    '                Exit Sub

    '            Else
    '                RetValue = DisplayMessage("Since a connection is not available on this PC, try restarting again. If that fails again, please contact your administrator. ", "No Connection On Chamber PC.", "S")
    '                '!!! write log file
    '                '.
    '                '.
    '                Application.Exit()
    '            End If
    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Execute: Main()", "S")
    '            Exit Sub
    '        Finally
    '            Cursor.Current = Cursors.Default
    '        End Try
    '    End Sub

    '    Public Function NToB(ByRef var As Object) As Object
    '        '--- Accepts a variant argument and returns a blank if the argument is Null or the
    '        '--- argument itself if it is not Null.
    '        If IsDBNull(var) Then
    '            NToB = ""
    '        Else
    '            NToB = var
    '        End If
    '    End Function

    '    Public Function ReplaceCharacter(ByRef TheFld As String, ByRef FromChar As String, ByRef ToChar As String) As String
    '        '--- replace all sets of chars to another set of characters
    '        Dim x, i As Short
    '        Dim NewFld As String

    '        Try
    '            ReplaceCharacter = ""

    '            NewFld = TheFld
    '            i = InStr(NewFld, FromChar)
    '            If i = 0 Then
    '                GoTo ReplaceCharacterExit
    '            End If

    '            Do While InStr(i, NewFld, FromChar) > 0
    '                x = InStr(i, NewFld, FromChar)
    '                NewFld = Mid(NewFld, 1, x - 1) & ToChar & Mid(NewFld, x + Len(FromChar))

    '                '--- move to character following last replacement to continue
    '                i = x + Len(ToChar)
    '                If i > Len(NewFld) Then
    '                    Exit Do
    '                End If
    '            Loop

    'ReplaceCharacterExit:
    '            ReplaceCharacter = NewFld
    '            ERHPopStack()
    '            Exit Function
    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Replace Character", "S")
    '        End Try
    '    End Function

    '    Public Sub LoadCommitteesIntoArray()
    '        Dim i As Short
    '        Dim ds As New DataSet
    '        Dim dr As DataRow

    '        Try
    '            '--- init to blanks
    '            ReDim gCommittees(0)
    '            ReDim gCommitteeAbbrevs(0)

    '            strSQL = "Select Abbrev + '-' + Committee AS TheCommittee, Abbrev, Committee FROM tblCommittees Order By Abbrev"
    '            ds = V_DataSet(strSQL, "R")

    '            ReDim Preserve gCommittees(ds.Tables(0).Rows.Count)
    '            ReDim Preserve gCommitteeAbbrevs(ds.Tables(0).Rows.Count)

    '            For Each dr In ds.Tables(0).Rows
    '                i += 1
    '                gCommittees(i) = dr("Committee")
    '                gCommitteeAbbrevs(i) = dr("Abbrev")
    '            Next
    '            gNbrCommittees = i

    '            Exit Sub
    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Execute: LoadCommitteesIntoArray()", "I")
    '            Exit Sub
    '        End Try
    '    End Sub

    '    Public Sub LoadPhrasesIntoArray()
    '        Dim ds As New DataSet
    '        Dim dr As DataRow
    '        Dim i As Short
    '        ReDim gPhrases(0)           '--- init to blanks
    '        Dim gPrhaseCodes(0) As Object

    '        Try
    '            strSQL = "Select Cstr(Code)  + ' - ' + Phrase  AS ThePhrase, Code, Phrase From tblPhrases Order By Code"
    '            ds = V_DataSet(strSQL, "R")

    '            ReDim Preserve gPhrases(ds.Tables(0).Rows.Count)
    '            ReDim Preserve gThePhrases(ds.Tables(0).Rows.Count)
    '            ReDim Preserve gPhraseCodes(ds.Tables(0).Rows.Count)

    '            i = 0
    '            For Each dr In ds.Tables(0).Rows
    '                i = i + 1
    '                gPhrases(i) = dr("Phrase")
    '                gThePhrases(i) = dr("ThePhrase")
    '                gPhraseCodes(i) = dr("Code")

    '            Next
    '            gNbrPhrases = i


    '            '--- for Voting PC only
    '            Dim dsV As New DataSet
    '            Dim pCount As Integer
    '            strSQL = "Select * From tblPhrasesForVotePC Order By Phrase"
    '            dsV = V_DataSet(strSQL, "R")
    '            ReDim gPhraseForVotePC(dsV.Tables(0).Rows.Count)
    '            For Each drV As DataRow In dsV.Tables(0).Rows
    '                pCount += 1
    '                gPhraseForVotePC(pCount) = drV("Phrase")
    '            Next

    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Execute: LoadPhrasesIntoArray()", "I")
    '            Exit Sub
    '        End Try
    '    End Sub

    '    Function IsLoaded(ByRef ThisFormName As String) As Short
    '        '--- Accepts: a form name
    '        '--- Purpose: determines if a form is loaded
    '        '--- Returns: True if specified the form is loaded; False if the specified form is not loaded.

    '        Dim forms As New Collection
    '        Dim Ctr, FormFound As Short
    '        Dim i As Integer

    '        On Error GoTo IsLoadedERH
    '        ERHPushStack(("IsLoaded - " & ThisFormName))

    '        Ctr = 0
    '        FormFound = False
    '        For i = System.Windows.Forms.Application.OpenForms.Count - 1 To 1 Step -1
    '            Ctr = Ctr + 1
    '            Dim form As Form = System.Windows.Forms.Application.OpenForms(i)
    '            If form.Name = ThisFormName Then
    '                FormFound = True
    '            End If
    '        Next
    '        IsLoaded = FormFound

    '        ERHPopStack()
    '        Exit Function

    'IsLoadedERH:
    '        ERHHandler()
    '        Exit Function
    '    End Function

    '    Sub WaitUntilFormCloses(ByRef FormName As String)
    '        Do Until Not IsLoaded(FormName)
    '            System.Windows.Forms.Application.DoEvents()
    '        Loop
    '    End Sub

    '    Public Function DownloadBillsFromALIS() As Integer
    '        Dim drV, drA, drA1 As DataRow
    '        Dim dsV As New DataSet
    '        Dim dsA, dsA1 As New DataSet
    '        Dim FieldValue() As Object
    '        Dim Title, WrkFld, str As String, RetValue As Object
    '        Dim da As New OleDbDataAdapter
    '        Dim ds As New DataSet

    '        Try
    '            ERHPushStack("DownloadBillsFromALIS")

    '            Try
    '                DownloadBillsFromALIS = False

    '                '--- if no ALIS connection, the attempt it again
    '                If (Not AlisProda_On And Not AlisProdb_On) Or (Not AlisTest_On And gTestMode And gWriteVotesToTest) Then
    '                    GoTo ProcExit
    '                End If

    '                RetValue = DisplayMessage("Please wait while " & IIf(gWriteVotesToTest, "gTest ", "") & "bills are downloaded from ALIS.", "Execute: DownloadBillsFromALIS()", "I")


    '                '--- get legislative day and session data; check for multiple open legislative days; if
    '                '--- found, then display and allow user to select or cancel; cancel
    '                '--- exits the system and assumes someone will fix on the ALIS end; only
    '                '--- trap 3 open days
    '                strSQL = "SELECT LD.OID, LD.LEGISLATIVE_DAY, " & _
    '                        "TO_CHAR(LD.CALENDAR_DATE,  'YYYY-MM-DD') AS CAL_DAY, " & _
    '                        "UPPER(LD.STATUS_CODE) AS STATUS, LD.OID_SESSION, " & _
    '                        "LS.LABEL, LS.ABBREVIATION " & _
    '                        "FROM LEGISLATIVE_DAY LD, LEGISLATIVE_SESSION LS " & _
    '                        "WHERE (LD.OID_SESSION = LS.OID) AND " & _
    '                        "(NOT (LD.LEGISLATIVE_DAY IS NULL)) AND " & _
    '                        "(LD.Status_code = 'O') " & _
    '                        "ORDER BY LD.LEGISLATIVE_DAY DESC"
    '                dsA = ALIS_DataSet(strSQL, "R")


    '                K = -1
    '                For Each drA In dsA.Tables(0).Rows
    '                    K = K + 1
    '                    ReDim Preserve gSessionIDTmp(K)
    '                    ReDim Preserve gLegislativeDayTmp(K)
    '                    ReDim Preserve gLegislativeDayOIDTmp(K)
    '                    ReDim Preserve gCalendarDayTmp(K)
    '                    ReDim Preserve gSessionNameTmp(K)
    '                    ReDim Preserve gSessionAbbrevTmp(K)
    '                    gSessionIDTmp(K) = drA("OID_Session")
    '                    gLegislativeDayTmp(K) = drA("Legislative_Day")
    '                    gLegislativeDayOIDTmp(K) = drA("OID")
    '                    gCalendarDayTmp(K) = FormatDateTime(drA("Cal_Day"), DateFormat.ShortDate)
    '                    gSessionNameTmp(K) = drA("Label")
    '                    gSessionAbbrevTmp(K) = drA("Abbreviation")
    '                    If K = 2 Then
    '                        Exit For
    '                    End If
    '                Next

    '                If K = -1 Then
    '                    RetValue = DisplayMessage("There are No open legislative days. Download from ALIS terminated.", "Download Bills From ALIS", "S")
    '                    Exit Function
    '                End If

    '                '--- get current open session id, leg day, and calendar day too, when k=0, if k > 0, there are more than one LEG day open.
    '                If K = 0 Then
    '                    gSessionID = gSessionIDTmp(0)
    '                    gLegislativeDay = gLegislativeDayTmp(0)
    '                    gLegislativeDayOID = gLegislativeDayOIDTmp(0)
    '                    gCalendarDay = gCalendarDayTmp(0)
    '                    gSessionName = gSessionNameTmp(0)
    '                    gSessionAbbrev = gSessionAbbrevTmp(0)
    '                Else
    '                    frmMultipleOpenLegislativeDays.Show()
    '                    WaitUntilFormCloses("frmMultipleOpenLegislativeDays")
    '                End If
    '                dsA.Dispose()

    '                '--- if different legislatuve day, then update recall votes for Voting PC to 0
    '                If Val(gLegislativeDay) <> gLastLegislativeDay Then
    '                    strSQL = "Update tblVotingDisplayParameters SET RecallVote1 = 0,RecallVote2 = 0,RecallVote3 = 0"
    '                    V_DataSet(strSQL, "U")
    '                End If


    '                '--- get last VOTE ID from ALIS for this session and update the
    '                '--- parameter table
    '                strSQL = " SELECT MAX(TO_NUMBER(voteid)) AS LastVoteID  " & _
    '                        " FROM LEGISLATIVE_DAY LD, VOTE " & _
    '                        " WHERE LD.oid = VOTE.oid_legislative_day " & _
    '                        " AND (LD.oid_session =" & gSessionID & ") " & _
    '                        " AND (VOTE.oid_legislative_body = " & gSenateID & ")"
    '                dsA1 = ALIS_DataSet(strSQL, "R")

    '                If dsA1.Tables(0).Rows.Count = 0 Then
    '                    RetValue = DisplayMessage("Cannot find last vote ID for session " & gSessionName & _
    '                       " in ALIS. The last vote ID will be set to 0.  If this is not correct, you must enter the last vote ID using the Parameter Maintenance form.", "No Vote ID In ALIS", "S")
    '                    gLastVoteIDPerALIS = 0
    '                Else
    '                    For Each drA1 In dsA1.Tables(0).Rows
    '                        If IsDBNull(drA1("LastVoteID")) Then
    '                            gLastVoteIDPerALIS = 0

    '                        Else
    '                            gLastVoteIDPerALIS = CInt(drA1("LastVoteID"))
    '                        End If
    '                    Next
    '                End If

    '                '--- now get see if there is a higher local vote id not yet sent to ALIS
    '                '--- if so, then update the last vote with it
    '                If UCase(gComputerName) = UCase(SVOTE) Then
    '                    strSQL = "SELECT MAX(VoteID) AS LastVoteID FROM tblRollCallVotes WHERE SessionID = " & gSessionID
    '                    dsV = V_DataSet(strSQL, "R")
    '                    If dsV.Tables(0).Rows.Count = 0 Then
    '                        gLastVoteIDPerLocal = 0
    '                    Else
    '                        For Each drV In dsV.Tables(0).Rows
    '                            If IsDBNull(drV("LastVoteID")) Then
    '                                gLastVoteIDPerLocal = 0
    '                            Else
    '                                gLastVoteIDPerLocal = drV("LastVoteID")
    '                            End If
    '                        Next
    '                    End If
    '                ElseIf UCase(gComputerName) = UCase(SDIS) Then
    '                    If gTestMode = False Then
    '                        strSQL = "SELECT ParameterValue FROM tblVotingParameters WHERE ucase(Parameter) ='LASTVOTEIDFORTHISSESSION' "
    '                    Else
    '                        strSQL = "SELECT ParameterValue FROM tblVotingParameters WHERE ucase(Parameter) ='LASTVOTEIDFORTHISSESSIONTEST' "
    '                    End If

    '                    dsV = V_DataSet(strSQL, "R")
    '                    If dsV.Tables(0).Rows.Count = 0 Then
    '                        gLastVoteIDPerLocal = 0
    '                    Else
    '                        For Each drV In dsV.Tables(0).Rows
    '                            If IsDBNull(drV("ParameterValue")) Then
    '                                gLastVoteIDPerLocal = 0
    '                            Else
    '                                gLastVoteIDPerLocal = drV("ParameterValue")
    '                            End If
    '                        Next
    '                    End If
    '                End If

    '                'If gLastVoteIDPerLocal > gLastVoteIDPerALIS Then
    '                '    ' gVoteID = gLastVoteIDPerLocal
    '                '    gVoteID = gLastVoteIDPerALIS
    '                '    ' RetValue = DisplayMessage("Attention: Local Last Vote ID Is Greater Than ALIS Production Last Vote ID. System Will Assign Local Last Vote ID To Current Vote ID.", "Execute: DownloadBillsFromALIS()", "I")
    '                'End If

    '                If gLastVoteIDPerLocal > gLastVoteIDPerALIS Then
    '                    gVoteID = gLastVoteIDPerLocal
    '                Else
    '                    gVoteID = gLastVoteIDPerALIS
    '                    '  RetValue = DisplayMessage("Attention: Local Last Vote ID Is Less Than ALIS Production Last Vote ID. System Will Assign ALIS Production Last Vote ID To Current Vote ID.", "Execute: DownloadBillsFromALIS()", "I")
    '                End If

    '                If gTestMode And gLastVoteIDPerLocal = 0 Then
    '                    gVoteID = 1
    '                End If


    '                '--- Now update the tblVoteParameters table
    '                If gTestMode = False Then
    '                    V_DataSet("Update tblVotingParameters SET ParameterValue='" & gVoteID & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSION'", "U")
    '                Else
    '                    V_DataSet("Update tblVotingParameters SET ParameterValue='" & gVoteID & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSIONTEST'", "U")
    '                End If

    '                '--- Delete Special Order Calenders  ALWAYS DO THESE!!!
    '                V_DataSet("DELETE FROM tblSpecialOrderCalendar ", "D")

    '                '---- if some of error occurred, continue next process
    '            Catch ex As Exception
    '                GoTo ProcNext1
    '            End Try

    'ProcNext1:
    '            '--- save any bills that have something in the work area to a temporary table and
    '            '--- then restore the work areas if the same bills are downloaded again
    '            Try
    '                V_DataSet("Delete From tblWork", "D")

    '                Dim dsWorkData As New DataSet
    '                dsWorkData = V_DataSet("Select CalendarCode, BillNbr, WorkData FROM tblBills WHERE WorkData <> ''", "R")
    '                If dsWorkData.Tables(0).Rows.Count <> 0 Then
    '                    For Each dr As DataRow In dsWorkData.Tables(0).Rows
    '                        Dim dsW As New DataSet
    '                        'dsW = V_DataSet("Select * From tblWork", "R")
    '                        'If dsW.Tables(0).Rows.Count > 0 Then
    '                        '    strSQL = "Update tblWork Set CalendarCode ='" & dr("CalendarCode") & "', BillNbr ='" & dr("BillNbr") & "', WorkData ='" & dr("WorkData") & "'"
    '                        '    V_DataSet(strSQL, "U")
    '                        'Else
    '                        strSQL = "Insert into tblWork Values ('" & dr("CalendarCode") & "', '" & dr("BillNbr") & "', '" & Replace(dr("WorkData"), "'", "''") & "', " & gLegislativeDay & ")"
    '                        V_DataSet(strSQL, "A")
    '                        'End If
    '                    Next
    '                End If

    '                '--- Definition for field() Array
    '                '--- field 0 - calendar code
    '                '--- field 1 - bill #
    '                '--- field 2 - bill description - what will be displayed for the bil - composed of bill #, sponsor, , subject (includes desc for confirmation), and calendar page
    '                '--- field 3 - sponsor
    '                '--- field 4 - subject
    '                '--- field 5 - work area data - always blank when added here - built by chamber display
    '                '--- field 6 - calendar page  
    '                '--- field 7 - SenatorSubject
    '                '--- field 8 - BillCanlendarPage


    '                '---delete all Other bills dynamically retrieved during the day
    '                V_DataSet("Delete From tblBills Where CalendarCode ='ZZ'", "D")

    '                ReDim FieldValue(8)


    '                '### 1 --- Get Regular Order Bills
    '                Try
    '                    '---delete Regular Bills first
    '                    V_DataSet("Delete From tblBills Where CalendarCode ='1'", "D")

    '                    str = " SELECT label, sponsor, alis_object.index_word, calendar_page " & _
    '                            " FROM ALIS_OBJECT, MATTER " & _
    '                            " WHERE matter.oid_instrument = alis_object.oid AND matter.oid_session = " & gSessionID & " AND alis_object.oid_session = " & gSessionID & _
    '                            " AND matter.matter_status_code = 'Pend' AND matter.oid_legislative_body = '1753' AND alis_object.legislative_body = 'S' " & _
    '                            " AND matter.calendar_sequence_no > 0 ORDER BY matter.calendar_sequence_no"
    '                    dsA = ALIS_DataSet(str, "R")
    '                    If dsA.Tables(0).Rows.Count > 0 Then
    '                        For Each drA In dsA.Tables(0).Rows
    '                            FieldValue(0) = "1"
    '                            If IsDBNull(drA("Label")) Then
    '                                FieldValue(1) = ""
    '                            Else
    '                                FieldValue(1) = drA("Label")
    '                            End If
    '                            If Strings.Left(FieldValue(1), 1) = "S" Then
    '                                Title = " by Senator "
    '                            Else
    '                                Title = " by Rep. "
    '                            End If
    '                            If IsDBNull(drA("Sponsor")) Then
    '                                FieldValue(3) = ""
    '                            Else
    '                                FieldValue(3) = drA("Sponsor")
    '                            End If
    '                            If IsDBNull(drA("Index_Word")) Then
    '                                FieldValue(4) = ""
    '                            Else
    '                                FieldValue(4) = NToB(Replace(drA("Index_Word"), "'", " "))
    '                            End If
    '                            If IsDBNull(FieldValue(5)) Or Not IsDBNull(FieldValue(5)) Then
    '                                FieldValue(5) = ""
    '                            End If
    '                            If IsDBNull(drA("Calendar_Page")) Then
    '                                FieldValue(6) = 0
    '                            Else
    '                                FieldValue(6) = drA("Calendar_Page")
    '                            End If

    '                            If FieldValue(6) <> 0 Then
    '                                FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4) & " p." & FieldValue(6)
    '                            Else
    '                                FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4)
    '                            End If

    '                            '--- new fields added
    '                            Dim strTitle As String
    '                            If Strings.Left(FieldValue(1), 1) = "S" Then
    '                                strTitle = "Senator"
    '                            Else
    '                                strTitle = "Representative"
    '                            End If

    '                            'FieldValue(7) = strTitle & " " & FieldValue(3) & " - " & FieldValue(4)

    '                            'If FieldValue(6) <> 0 Then
    '                            '    FieldValue(8) = FieldValue(1) & "&nbsp;&nbsp;&nbsp;&nbsp;" & " p." & FieldValue(6)
    '                            'Else
    '                            '    FieldValue(7) = FieldValue(1)
    '                            'End If
    '                            FieldValue(7) = strTitle & " " & FieldValue(3) & " - " & FieldValue(4)
    '                            If FieldValue(6) <> 0 Then
    '                                FieldValue(8) = FieldValue(1) & "&nbsp;&nbsp;&nbsp;&nbsp;" & " p." & FieldValue(6)
    '                            Else
    '                                ' FieldValue(7) = FieldValue(1)
    '                                FieldValue(8) = ""
    '                                FieldValue(7) = FieldValue(2)
    '                            End If
    '                            V_DataSet("INSERT INTO tblBills VALUES ('1', '" & FieldValue(1) & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "', '" & Replace(FieldValue(7), "'", "") & "', '" & Replace(FieldValue(8), "'", "") & "')", "A")
    '                        Next
    '                    End If
    '                Catch ex As Exception
    '                    DisplayMessage("Failed Download Bills From ALIS! Please Re-try.", "Down Load Bill From ALIS", "Y")
    '                    End
    '                End Try

    '                '### 2 --- Get Local Bills  
    '                Try
    '                    '--- delete Local bills first
    '                    V_DataSet("Delete From tblBills Where CalendarCode ='2'", "D")

    '                    str = "SELECT label, calendar_page, senate_display_title, sponsor " & _
    '                            " FROM alis_object, matter WHERE matter.oid_instrument = alis_object.oid " & _
    '                            " AND matter.oid_session = " & gSessionID & " AND alis_object.oid_session = " & gSessionID & " AND matter.matter_status_code = 'Pend' " & _
    '                            " AND matter.oid_legislative_body = '1753' AND alis_object.legislative_body = 'S' AND alis_object.local_bill = 'T' " & _
    '                            " AND matter.calendar_sequence_no > 0 ORDER BY matter.calendar_sequence_no "
    '                    dsA = ALIS_DataSet(str, "R")

    '                    If dsA.Tables(0).Rows.Count > 0 Then
    '                        For Each drA In dsA.Tables(0).Rows
    '                            FieldValue(0) = "2"
    '                            FieldValue(1) = NToB(drA("Label"))
    '                            If Strings.Left(FieldValue(1), 1) = "S" Then
    '                                Title = " by Senator "
    '                            Else
    '                                Title = " by Rep. "
    '                            End If
    '                            FieldValue(3) = NToB(drA("Sponsor"))
    '                            If InStr(NToB(drA("Senate_Display_Title")), " ") > 0 Then
    '                                FieldValue(4) = Mid(NToB(drA("Senate_Display_Title")), InStr(NToB(drA("Senate_Display_Title")), " ") + 1)
    '                            Else
    '                                FieldValue(4) = Mid(NToB(drA("Senate_Display_Title")), 1)
    '                            End If

    '                            FieldValue(5) = ""
    '                            If IsDBNull((drA("Calendar_Page"))) Then
    '                                FieldValue(6) = 0
    '                            Else
    '                                FieldValue(6) = drA("Calendar_Page")
    '                            End If
    '                            If CType(FieldValue(6), String) <> 0 Then
    '                                FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4) & " p." & FieldValue(6)
    '                            Else
    '                                FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4)
    '                            End If

    '                            Dim strTitle As String = ""
    '                            If Strings.Left(FieldValue(1), 1) = "S" Then
    '                                strTitle = "Senator"
    '                            ElseIf Strings.Left(FieldValue(1), 1) = "H" Then
    '                                strTitle = "Representative"
    '                            ElseIf Strings.Left(FieldValue(1), 1) <> "S" And Strings.Left(FieldValue(1), 1) <> "H" Then
    '                                strTitle = ""
    '                            End If

    '                            FieldValue(7) = strTitle & " " & FieldValue(3) & " - " & FieldValue(4)
    '                            If FieldValue(6) <> 0 Then
    '                                FieldValue(8) = FieldValue(1) & "&nbsp;&nbsp;&nbsp;&nbsp;" & " p." & FieldValue(6)
    '                            Else
    '                                ' FieldValue(7) = FieldValue(1)
    '                                FieldValue(8) = ""
    '                                FieldValue(7) = FieldValue(2)
    '                            End If
    '                            V_DataSet("INSERT INTO tblBills VALUES ('2', '" & FieldValue(1) & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "', '" & Replace(FieldValue(7), "'", "") & "', '" & Replace(FieldValue(8), "'", "") & "')", "A")
    '                        Next
    '                    End If
    '                Catch ex As Exception
    '                    DisplayMessage("Failed Download Local Bills From ALIS! Please Re-try.", "Down Load Local Bill From ALIS", "Y")
    '                    End
    '                End Try


    '                '###3 ---Get Confirmations
    '                Try
    '                    ' delete confirmatins
    '                    V_DataSet("Delete From tblBills Where CalendarCode ='3'", "D")

    '                    str = "SELECT I.OID, I.LABEL, I.TYPE_CODE,   PE.FIRSTNAME, PE.LASTNAME, PE.SUFFIX, O.NAME, CO.ABBREVIATION, TO_CHAR(R1DAY.CALENDAR_DATE, 'MM/DD/YYYY') AS R1DATE, TO_CHAR(MDAY.CALENDAR_DATE, 'MM/DD/YYYY') AS MDATE, " & _
    '                            " M.TRANSACTION_TYPE_CODE AS MTRAN, M.MATTER_STATUS_CODE AS MSTAT, M.RECOMMENDATION_CODE AS MREC, AU.NAME AS AUTHORITY, CV.Value AS INSTR_VALUE " & _
    '                        " FROM ALIS_OBJECT I, MATTER C1, MATTER M, ORGANIZATION CO, ORGANIZATION O, POSITION PO, POSITION AU, PERSON PE, LEGISLATIVE_DAY R1DAY, LEGISLATIVE_DAY MDAY, CODE_VALUES CV " & _
    '                        " WHERE I.INSTRUMENT = 'T' AND I.TYPE_CODE = 'CF' AND I.STATUS_CODE = CV.CODE (+) AND CV.CODE_TYPE = 'InstrumentStatus' AND I.OID_SESSION = " & gSessionID & " AND I.OID_POSITION = PO.OID (+) " & _
    '                            " AND O.OID (+) = PO.OID_ORGANIZATION AND AU.OID (+) = PO.OID_APPOINTING_AUTHORITY AND I.OID = C1.OID_INSTRUMENT AND M.OID_COMMITTEE = CO.OID (+) AND C1.OID_CANDIDACY = PE.OID (+) " & _
    '                            " AND C1.OID_CONSIDERED_DAY = R1DAY.OID (+) AND M.OID_CONSIDERED_DAY = MDAY.OID (+) AND M.OID_INSTRUMENT = I.OID AND M.SEQUENCE = I.LAST_MATTER AND I.OID_SESSION = C1.OID_SESSION " & _
    '                            " AND C1.SEQUENCE = (SELECT MIN(X.SEQUENCE) FROM MATTER X WHERE X.OID_SESSION = C1.OID_SESSION AND X.OID_INSTRUMENT = C1.OID_INSTRUMENT AND X.TRANSACTION_TYPE_CODE = 'R1') ORDER BY SUBSTR(I.LABEL, 1, 1), I.TYPE_CODE, I.ID"
    '                    dsA = ALIS_DataSet(str, "R")

    '                    If dsA.Tables(0).Rows.Count > 0 Then
    '                        For Each drA In dsA.Tables(0).Rows
    '                            FieldValue(0) = "3"
    '                            FieldValue(1) = NToB(Mid(drA("Label"), 2))
    '                            FieldValue(3) = NToB(drA("FirstName")) & " " & NToB(drA("LastName"))
    '                            FieldValue(4) = ""

    '                            '--- if boards and commissions exist, then search for the senate voting name and replace if found            
    '                            If NToB(drA("Name")) = "" Then
    '                                FieldValue(2) = FieldValue(1) & " - " & FieldValue(3) & " -- "
    '                            Else
    '                                WrkFld = NToB(drA("Name"))
    '                                str = "SELECT SenateVotingName FROM tblBoardsAndCommissions WHERE AlisName = '" & Replace(WrkFld, "'", "''") & "'"
    '                                dsV = V_DataSet(str, "R")
    '                                If dsV.Tables(0).Rows.Count = 0 Then
    '                                    FieldValue(2) = FieldValue(1) & " - " & FieldValue(3) & " -- " & NToB(drA("Name"))
    '                                Else
    '                                    FieldValue(2) = FieldValue(1) & " - " & FieldValue(3) & " -- " & dsV.Tables(0).Rows(0).Item("SenateVotingName")
    '                                End If
    '                            End If
    '                            FieldValue(5) = ""
    '                            FieldValue(6) = 0

    '                            Dim strTitle As String = ""
    '                            If Strings.Left(FieldValue(1), 1) = "S" Then
    '                                strTitle = "Senator"
    '                            ElseIf Strings.Left(FieldValue(1), 1) = "H" Then
    '                                strTitle = "Representative"
    '                            ElseIf Strings.Left(FieldValue(1), 1) <> "S" And Strings.Left(FieldValue(1), 1) <> "H" Then
    '                                strTitle = ""
    '                            End If

    '                            FieldValue(7) = strTitle & " " & FieldValue(3) & " - " & FieldValue(4)

    '                            If FieldValue(6) <> 0 Then
    '                                FieldValue(8) = FieldValue(1) & "&nbsp;&nbsp;&nbsp;&nbsp;" & " p." & FieldValue(6)
    '                            Else
    '                                FieldValue(7) = FieldValue(1)
    '                            End If
    '                            V_DataSet("INSERT INTO tblBills VALUES ('3', '" & FieldValue(1) & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "', '" & Replace(FieldValue(7), "'", "") & "', '" & Replace(FieldValue(8), "'", "") & "')", "A")
    '                        Next
    '                    End If
    '                Catch
    '                    DisplayMessage("Failed Download Confirmations From ALIS! Please Re-try.", "Down Load Confirmatins From ALIS", "Y")
    '                    End
    '                End Try

    '                '--- restore any saved work areas to the bills downloaded
    '                dsV = V_DataSet("Select * From tblWork", "R")
    '                For Each drV In dsV.Tables(0).Rows
    '                    V_DataSet("UPDATE tblBills SET WorkData ='" & Replace(drV("WorkData"), "'", "''") & "' WHERE CalendarCode ='" & NToB(drV("CalendarCode")) & "' AND ucase(BillNbr) ='" & UCase(NToB(drV("BillNbr"))) & "'", "U")
    '                Next
    '                '--- clear tblWork 
    '                V_DataSet("Update tblWork Set CalendarCode = '', BillNbr = '', WorkData = ''", "U")

    '                RetValue = DisplayMessage("Download of " & IIf(gWriteVotesToTest, "TEST ", "") & " Bills From ALIS Completed.", "Execute: DownloadBillsFromALIS()", "I")

    '                DownloadBillsFromALIS = True
    '            Catch ex As Exception
    '                DownloadBillsFromALIS = False
    '                GoTo ProcExit
    '            End Try

    'ProcExit:

    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Execute: DownloadBillsFromALIS()", "S")
    '            Exit Function
    '        End Try
    '    End Function

    '    Public Sub LoadSenatorsIntoArray()
    '        Dim ds As New DataSet
    '        Dim dr As DataRow
    '        Dim i, y As Short
    '        Dim RetValue As New Object

    '        '--- load the senators into an array
    '        Try
    '            ERHPushStack(("LoadSenatorsIntoArray"))

    '            i = 0

    '            strSQL = "Select * From tblSenators Order By SenatorName"
    '            ds = V_DataSet(strSQL, "R")
    '            gNbrSenators = ds.Tables(0).Rows.Count
    '            ReDim Preserve gSenatorName(gNbrSenators)
    '            ReDim Preserve gSenatorOID(gNbrSenators)
    '            ReDim Preserve gDistrictOID(gNbrSenators)
    '            ReDim Preserve gSenatorDistrictName(gNbrSenators)

    '            If ds.Tables(0).Rows.Count > 0 Then
    '                For Each dr In ds.Tables(0).Rows
    '                    '  i = i + 1
    '                    If (i > gNbrSenators) Or (Int(ds.Tables(0).Rows.Count > gNbrSenators)) Then
    '                        RetValue = DisplayMessage("There are more Senators than we have space allocated for.  Either # of Senators in the " & "parameter table is incorrect or you need to download Senators from ALIS.  It is suggested you determine which " & "is correct, but processing will continue.  This message may appear more than once.", "Senator # Miss-Match", "I")
    '                    Else
    '                        gSenatorNameOrder(i) = dr("SenatorName")            '--- used to populate senator combo box on chamber display
    '                        If gApplication = "Voting" Then
    '                            gSalutation(i) = NToB(dr("Salutation"))         '--- in senator name order
    '                        End If
    '                        'If InStr(dr("SenatorName"), ", ") > 0 Then
    '                        '    gSenatorName(i) = Mid(dr("SenatorName"), 1, InStr(dr("SenatorName"), ", ") - 1)
    '                        'Else
    '                        gSenatorName(i) = dr("SenatorName")
    '                        'End If

    '                        gSenatorOID(i) = dr("SenatorOID")                   '--- must follow senator name order
    '                        gDistrictOID(i) = dr("DistrictOID")
    '                        i = i + 1
    '                    End If
    '                Next
    '                RetValue = DisplayMessage("Reading Senators Records From Local Database Finished.", "Execute: LoadSenatorsIntoArray()", "I")
    '            Else
    '                RetValue = DisplayMessage("Attention: There are not Senators download from ALIS yet!", "Execute: LoadSenatorsIntoArray()", "I")
    '                DownloadSenatorsFromALIS()
    '            End If

    '            ds.Dispose()

    '            Dim dsD As New DataSet
    '            Dim k As Integer = 0
    '            Dim x As Integer = 0
    '            Dim MemberLname As String = ""
    '            strSQL = "Select * From tblSenators Order By SenatorNbr"
    '            dsD = V_DataSet(strSQL, "R")
    '            For Each drD As DataRow In dsD.Tables(0).Rows
    '                'If InStr(drD("SenatorName"), ", ") > 0 Then
    '                '    MemberLname = Mid(drD("SenatorName"), 1, InStr(drD("SenatorName"), ", ") - 1)
    '                '    gSenatorDistrictName(k + 1) = MemberLname
    '                'Else
    '                gSenatorDistrictName(k + 1) = drD("SenatorName")
    '                ' End If

    '                k += 1
    '            Next
    '            Exit Sub
    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Execute: LoadSenatorsIntoArray()", "I")
    '            Exit Sub
    '        End Try
    '    End Sub

    '    Public Function QueryParameters(ByVal strSql As String) As DataSet
    '        Dim da As New OleDbDataAdapter
    '        Dim ds As New DataSet

    '        If connLocal.State = ConnectionState.Closed Then connLocal.Open()

    '        Try
    '            If strSql <> "" Then
    '                da = New OleDbDataAdapter(strSql, connLocal)
    '                da.SelectCommand = New OleDbCommand(strSql, connLocal)
    '                da.Fill(ds, "Table")
    '                QueryParameters = ds
    '                Return ds
    '            End If
    '        Catch ex As OleDbException
    '            DisplayMessage(ex.Message, "Get Voting Parameters Failed", "S")
    '        Finally
    '            connLocal.Close()
    '        End Try
    '    End Function

    '    Public Sub InitialSpecailOrderCalendar()
    '        Dim ds, dsC As New DataSet
    '        Dim RetValue As New Object

    '        Try
    '            strSQL = "Delete From tblSpecialOrderCalendar "
    '            ds = V_DataSet(strSQL, "D")

    '            strSQL = "Delete From tblCalendars Where LEFT(CalendarCode, 2)='SR'"
    '            dsC = V_DataSet(strSQL, "D")

    '            strSQL = "Delete From tblCalendars Where CalendarCode='SOC'"
    '            dsC = V_DataSet(strSQL, "D")

    '            Exit Sub
    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Execute: IintalSpecailOrderCalendar()", "S")
    '            Exit Sub
    '        End Try
    '    End Sub

    '    Public Function GetBills(ByVal gCalendar As String) As DataSet
    '        Try
    '            Dim origCalendar As String = gCalendar
    '            Dim sr As String = UCase(Strings.Left(gCalendar, 2))
    '            If sr = "SR" Then
    '                gCalendar = "SR"
    '            End If

    '            If gCalendar = "SR" Then
    '                strSQL = "Select Bill, CalendarCode From  tblSpecialOrderCalendar Where CalendarCode = (Select CalendarCode From tblCalendars Where ucase(Calendar) ='" & UCase(origCalendar) & "') Order By Bill"
    '            End If
    '            If gCalendar <> "SR" And UCase(gCalendar) <> "SPECIAL ORDER CALENDAR" Then
    '                strSQL = "Select Bill, CalendarCode From tblBills Where CalendarCode = (Select CalendarCode From tblCalendars Where ucase(Calendar) ='" & UCase(gCalendar) & "') Order By Bill"
    '            End If
    '            If UCase(gCalendar) = "SPECIAL ORDER CALENDAR" Then
    '                strSQL = "Select Bill, CalendarCode From  tblSpecialOrderCalendar Where CalendarCode = (Select CalendarCode From tblCalendars Where ucase(CalendarCode) ='SOC') Order By Bill"
    '            End If
    '            GetBills = V_DataSet(strSQL, "R")

    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Get Bills", "S")
    '            Exit Function
    '        End Try
    '    End Function

    '    Public Sub UpdateVotingDisplayParameters(Optional ByVal VoteID1 As Integer = 0, Optional ByVal VoteID2 As Integer = 0, Optional ByVal VoteID3 As Integer = 0)
    '        Try
    '            If VoteID1 > 0 Then
    '                V_DataSet("Update tblVotingDisplayParameters Set RecallVote1 = " & VoteID1, "U")
    '                V_DataSet("Update tblVotingParameters Set ParameterValue = '" & VoteID1 & "' WHERE Parameter ='RecallVote1'", "U")
    '            End If
    '            If VoteID2 > 0 Then
    '                V_DataSet("Update tblVotingDisplayParameters Set RecallVote2 = " & VoteID2, "U")
    '                V_DataSet("Update tblVotingParameters Set ParameterValue = '" & VoteID2 & "' WHERE Parameter ='RecallVote2'", "U")
    '            End If
    '            If VoteID3 > 0 Then
    '                V_DataSet("Update tblVotingDisplayParameters Set RecallVote3 = " & VoteID3, "U")
    '                V_DataSet("Update tblVotingParameters Set ParameterValue = '" & VoteID3 & "' WHERE Parameter ='RecallVote3'", "U")
    '            End If
    '            'V_DataSet("Update tblVotingDisplayParameters Set RecallVote1 = " & VoteID1 & ", RecallVote2=" & VoteID2 & ", RecallVote3=" & VoteID3, "U")
    '            'V_DataSet("Update tblVotingParameters Set ParameterValue = '" & VoteID1 & "' WHERE Parameter ='RecallVote1'", "U")
    '            'V_DataSet("Update tblVotingParameters Set ParameterValue = '" & VoteID2 & "' WHERE Parameter ='RecallVote2'", "U")
    '            'V_DataSet("Update tblVotingParameters Set ParameterValue = '" & VoteID3 & "' WHERE Parameter ='RecallVote3'", "U")
    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Execute: UpdateVotingDisplayParameters", "S")
    '            Exit Sub
    '        End Try
    '    End Sub

    '    Public Function GetLastLocalVoteID() As Long
    '        Dim dsP, dsR As New DataSet
    '        Dim strP, strR As String
    '        Dim voteidP, voteidR As Integer

    '        '--- this roution gets the last vote ID for this session from the param table; this
    '        '--- field is initially updated on startup by finding the last vote ID sent to ALIS, and
    '        '--- the param table is updated after each vote so that number should always be current
    '        '--- during this logon
    '        Try
    '            If gTestMode = False Then
    '                strP = "Select ParameterValue From tblVotingParameters Where UCase(Parameter)='LASTVOTEIDFORTHISSESSION'"
    '                dsP = V_DataSet(strP, "R")
    '                strR = "Select max(VoteID) as voteID From tblRollCallVotes Where Test =0"
    '                dsR = V_DataSet(strR, "R")
    '            Else
    '                strP = "Select ParameterValue From tblVotingParameters Where UCase(Parameter)='LASTVOTEIDFORTHISSESSIONTEST'"
    '                dsP = V_DataSet(strP, "R")
    '                strR = "Select max(VoteID) as voteID From tblRollCallVotes Where Test =1"
    '                dsR = V_DataSet(strR, "R")
    '            End If


    '            If dsP.Tables(0).Rows.Count <> 0 Then
    '                For Each drP As DataRow In dsP.Tables(0).Rows
    '                    If Not IsDBNull(drP("ParameterValue")) Then
    '                        voteidR = drP("ParameterValue")
    '                    Else
    '                        voteidR = 0
    '                    End If

    '                Next
    '            Else
    '                voteidP = 0
    '            End If

    '            If dsR.Tables(0).Rows.Count <> 0 Then
    '                For Each drR As DataRow In dsR.Tables(0).Rows
    '                    If Not IsDBNull(drR("VoteID")) Then
    '                        voteidP = drR("VoteID")
    '                    Else
    '                        voteidP = 0
    '                    End If
    '                Next
    '            Else
    '                voteidP = 0
    '            End If

    '            If voteidP = voteidR Then
    '                gVoteID = voteidP
    '            ElseIf voteidP > voteidR Then
    '                gVoteID = voteidR
    '            ElseIf voteidP < voteidR Then
    '                gVoteID = voteidR
    '            End If
    '            UpdateLastVoteIDAndLastLegDay(gVoteID)

    '            GetLastLocalVoteID = gVoteID
    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Execute: GetLastLocalVoteID()", "S")
    '            Exit Function
    '        End Try
    '    End Function



    '    Public Sub UpdateLastVoteIDAndLastLegDay(ByVal LastVoteID As String)
    '        Try
    '            If gTestMode = False Then
    '                strSQL = "Update tblVotingParameters Set ParameterValue='" & LastVoteID & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSION'"
    '                V_DataSet(strSQL, "U")
    '            Else
    '                strSQL = "Update tblVotingParameters Set ParameterValue='" & LastVoteID & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSIONTEST'"
    '                V_DataSet(strSQL, "U")
    '            End If
    '            strSQL = "Update tblVotingParameters Set ParameterValue='" & gLegislativeDay & "' Where ucase(Parameter) ='LASTLEGISLATIVEDAY'"
    '            V_DataSet(strSQL, "U")
    '        Catch ex As Exception
    '            DisplayMessage(ex.Message, "Execute: UpdateLastVoteIDAndLastLegDay()", "I")
    '            Exit Sub
    '        End Try
    '    End Sub

    '    Public Sub GetLegDay()
    '        Dim ds As New DataSet

    '        Try
    '            strSQL = "SELECT LD.OID, LD.LEGISLATIVE_DAY, " & _
    '                      " TO_CHAR(LD.CALENDAR_DATE,  'YYYY-MM-DD') AS CAL_DAY, " & _
    '                      " UPPER(LD.STATUS_CODE) AS STATUS, LD.OID_SESSION, " & _
    '                      " LS.LABEL, LS.ABBREVIATION " & _
    '                      " FROM LEGISLATIVE_DAY LD, LEGISLATIVE_SESSION LS " & _
    '                      " WHERE (LD.OID_SESSION = LS.OID) AND " & _
    '                      " (NOT (LD.LEGISLATIVE_DAY IS NULL)) AND " & _
    '                      " (LD.Status_code = 'O') " & _
    '                      " ORDER BY LD.LEGISLATIVE_DAY DESC"
    '            ds = ALIS_DataSet(strSQL, "R")

    '            Dim openDay As Integer = ds.Tables(0).Rows.Count
    '            If openDay = 0 Then
    '                RetValue = DisplayMessage("There are No open legislative days. System will shut down. Restart try again.", "No Legislative Day Open", "S")
    '                Application.Exit()
    '            ElseIf openDay > 1 Then
    '                frmMultipleOpenLegislativeDays.Show()
    '                WaitUntilFormCloses("frmMultipleOpenLegislativeDays")
    '            ElseIf openDay = 1 Then
    '                For Each dr As DataRow In ds.Tables(0).Rows
    '                    gSessionID = dr("OID_SESSION")
    '                    gLegislativeDay = dr("LEGISLATIVE_DAY")
    '                    gLegislativeDayOID = dr("OID")
    '                    gCalendarDay = dr("CAL_DAY")
    '                    gSessionName = dr("LABEL")
    '                    gSessionAbbrev = dr("ABBREVIATION")
    '                Next
    '            End If
    '        Catch ex As Exception
    '            RetValue = DisplayMessage(ex.Message, "Error Occurred: Get Legislative Day", "S")
    '            Exit Sub
    '        End Try
    '    End Sub



    '    Public Function SendVotesToALIS() As Integer
    '        Dim k As Integer
    '        Dim indVoteOID As Long, VoteOID As Long, WrkFld As String, SenatorOID As Long, Vote As String
    '        Dim DistrictOID As Long
    '        Dim str, strAdd As String
    '        Dim ds, dsAdd, dsIndVoteOID, dsVoteOID, dsDetail As New DataSet
    '        Dim dr As DataRow
    '        Dim dt As New DataTable

    '        dt.Clear()
    '        dt.Columns.Clear()
    '        dt.Columns.Add("SenatorOID")
    '        dt.Columns.Add("DistrictOID")
    '        dt.Columns.Add("Vote")



    '        Try
    '            '--- if it is test and not write to ALIS or ALIS database un-accessiable, quite send votes to ALIS
    '            If (gTestMode And Not gWriteVotesToTest) Then
    '                GoTo ProcExit
    '            End If

    '            '--- check able to connecte ALIS
    '            '--- Check_ALIS_Database_Accessible()
    '            If (Not AlisProda_On And Not AlisProdb_On) Then
    '                DisplayMessage("An error occurred while sending the votes to ALIS.  If the connection to ALIS is down, you should exit the system " & _
    '                "and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Vote Not Sent To ALIS", "S")
    '                gSendVotesToALIS = True
    '                GoTo ProcExit
    '            End If

    '            If gTestMode And gWriteVotesToTest Then                '---- 0 gProduction votes ,     1 gTest votes
    '                If gReSendALIS = False Then
    '                    strSQL = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 1 And AlisVoteOID=0 AND VoteID =" & gVoteID
    '                Else
    '                    strSQL = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 1 And AlisVoteOID=0 "
    '                End If
    '            Else
    '                If gReSendALIS = False Then
    '                    strSQL = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 0 And AlisVoteOID=0 AND VoteID =" & gVoteID
    '                Else
    '                    str = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 0 And AlisVoteOID=0 "
    '                End If
    '            End If
    '            ds = V_DataSet(strSQL, "R")

    '            For Each dr In ds.Tables(0).Rows
    '                '--- get parentas VoteID from ALIS
    '                dsVoteOID = ALIS_DataSet("SELECT ALIS.VOTE_S.NEXTVAL AS VOTEOID FROM DUAL", "R")
    '                If dsVoteOID.Tables(0).Rows.Count > 0 Then
    '                    VoteOID = dsVoteOID.Tables(0).Rows(0).Item("VOTEOID")
    '                End If

    '                strAdd = "INSERT INTO Vote (oid, oid_legislative_Body, oid_Legislative_day, voteId, nays, yeas, abstains, pass) VALUES (" & VoteOID & ", " & gSenateID & ", " & _
    '                        dr("LegislativeDayOID") & ", " & dr("VoteID") & ", " & dr("TotalNay") & ", " & dr("TotalYea") & ", " & dr("TotalAbstain") & ", " & dr("TotalPass") & ")"
    '                ALIS_DataSet(strAdd, "A")


    '                '--- pick out vote, districtOID, and senatorOID add to tmp tabel
    '                WrkFld = dr("SenatorVotes")
    '                k = 0
    '                Do
    '                    ' k = k + 1
    '                    WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1) ' skip senator name
    '                    Vote = Microsoft.VisualBasic.Left(WrkFld, 1)
    '                    WrkFld = Mid$(WrkFld, 3)
    '                    DistrictOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, "*") - 1))
    '                    WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1)
    '                    SenatorOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, ";") - 1))
    '                    If InStr(WrkFld, ";") = Len(WrkFld) Then
    '                        WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '                        dt.Rows.Add(SenatorOID, DistrictOID, Vote)
    '                        Exit Do
    '                    End If
    '                    WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '                    dt.Rows.Add(SenatorOID, DistrictOID, Vote)
    '                    '  k = k + 1
    '                    ' MsgBox("senatorOID: " & SenatorOID, "       DistrictOID: " & DistrictOID & "        vote: " & Vote)
    '                Loop

    '                '--- sort districtOID
    '                Dim dataView As New DataView(dt)
    '                dataView.Sort = "DistrictOID"
    '                Dim dataTable As DataTable = dataView.ToTable

    '                If dataView.Count > 0 Then
    '                    For Each drDetail As DataRowView In dataView
    '                        ' --- get Individual Member VoteID from ALIS
    '                        dsIndVoteOID = ALIS_DataSet("SELECT ALIS.INDIVIDUAL_VOTE_S.NEXTVAL AS IND_VOTEOID FROM DUAL", "R")
    '                        indVoteOID = dsIndVoteOID.Tables(0).Rows(0).Item("IND_VoteOID")
    '                        strSQL = "INSERT INTO Individual_Vote(oid, oid_legislator, oid_vote, oid_organization, vote) VALUES (" & indVoteOID & ", " & drDetail("SenatorOID") & ", " & VoteOID & ", " & drDetail("DistrictOID") & ", '" & drDetail("Vote") & "')"
    '                        '  strSQL = "INSERT INTO Individual_Vote(oid, oid_legislator, oid_vote, oid_organization, vote) VALUES (" & indVoteOID & ", " & SenatorOID & ", " & VoteOID & ", " & DistrictOID & ", '" & Vote & "')"

    '                        ALIS_DataSet(strSQL, "A")
    '                    Next
    '                End If

    '                If gTestMode Then
    '                    V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & ", TEST = 1 WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")
    '                Else
    '                    V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & ", TEST = 0 WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")
    '                End If
    '            Next
    '            gSendVotesToALIS = False

    '            If gReSendALIS Then
    '                DisplayMessage("Process all of UNSEND votes to ALIS finished.", "Send Votes To ALIS", "S")
    '                Exit Function
    '            End If

    'ProcExit:

    '        Catch ex As Exception
    '            RetValue = DisplayMessage(ex.Message & vbCrLf & "An error occurred while Re-Send the votes to ALIS.  If the connection to ALIS is down, you should exit the system " & _
    '            "and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Execute: SendUnSendVotesToALIS()", "S")
    '            Exit Function
    '        End Try
    '    End Function


    '    '    Public Function SendVotesToALIS() As Integer
    '    '        Dim k As Integer
    '    '        Dim indVoteOID As Long, VoteOID As Long, WrkFld As String, SenatorOID As Long, Vote As String
    '    '        Dim DistrictOID As Long
    '    '        Dim str, strAdd As String
    '    '        Dim ds, dsAdd, dsIndVoteOID, dsVoteOID, dsDetail As New DataSet
    '    '        Dim dr As DataRow
    '    '        Dim dt As New DataTable

    '    '        dt.Clear()
    '    '        dt.Columns.Clear()
    '    '        dt.Columns.Add("SenatorOID")
    '    '        dt.Columns.Add("DistrictOID")
    '    '        dt.Columns.Add("Vote")



    '    '        Try
    '    '            '--- if it is test and not write to ALIS or ALIS database un-accessiable, quite send votes to ALIS
    '    '            If (gTestMode And Not gWriteVotesToTest) Then
    '    '                GoTo ProcExit
    '    '            End If

    '    '            '--- check able to connecte ALIS
    '    '            '--- Check_ALIS_Database_Accessible()
    '    '            If (Not AlisProda_On And Not AlisProdb_On) Then
    '    '                DisplayMessage("An error occurred while sending the votes to ALIS.  If the connection to ALIS is down, you should exit the system " & _
    '    '                "and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Vote Not Sent To ALIS", "S")
    '    '                gSendVotesToALIS = True
    '    '                GoTo ProcExit
    '    '            End If

    '    '            If gTestMode And gWriteVotesToTest Then                '---- 0 gProduction votes ,     1 gTest votes
    '    '                If gReSendALIS = False Then
    '    '                    strSQL = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 1 And AlisVoteOID=0 AND VoteID =" & gVoteID
    '    '                Else
    '    '                    strSQL = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 1 And AlisVoteOID=0 "
    '    '                End If
    '    '            Else
    '    '                If gReSendALIS = False Then
    '    '                    strSQL = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 0 And AlisVoteOID=0 AND VoteID =" & gVoteID
    '    '                Else
    '    '                    str = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 0 And AlisVoteOID=0 "
    '    '                End If
    '    '            End If
    '    '            ds = V_DataSet(strSQL, "R")

    '    '            For Each dr In ds.Tables(0).Rows
    '    '                '--- get parentas VoteID from ALIS
    '    '                dsVoteOID = ALIS_DataSet("SELECT ALIS.VOTE_S.NEXTVAL AS VOTEOID FROM DUAL", "R")
    '    '                If dsVoteOID.Tables(0).Rows.Count > 0 Then
    '    '                    VoteOID = dsVoteOID.Tables(0).Rows(0).Item("VOTEOID")
    '    '                End If

    '    '                strAdd = "INSERT INTO Vote (oid, oid_legislative_Body, oid_Legislative_day, voteId, nays, yeas, abstains, pass) VALUES (" & VoteOID & ", " & gSenateID & ", " & _
    '    '                        dr("LegislativeDayOID") & ", " & dr("VoteID") & ", " & dr("TotalNay") & ", " & dr("TotalYea") & ", " & dr("TotalAbstain") & ", " & dr("TotalPass") & ")"
    '    '                ALIS_DataSet(strAdd, "A")


    '    '                '--- pick out vote, districtOID, and senatorOID add to tmp tabel
    '    '                WrkFld = dr("SenatorVotes")
    '    '                k = 0
    '    '                Do
    '    '                    k = k + 1
    '    '                    WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1) ' skip senator name
    '    '                    Vote = Microsoft.VisualBasic.Left(WrkFld, 1)
    '    '                    WrkFld = Mid$(WrkFld, 3)
    '    '                    DistrictOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, "*") - 1))
    '    '                    WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1)
    '    '                    SenatorOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, ";") - 1))
    '    '                    If InStr(WrkFld, ";") = Len(WrkFld) Then
    '    '                        Exit Do
    '    '                    End If
    '    '                    WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '    '                    dt.Rows.Add(SenatorOID, DistrictOID, Vote)
    '    '                Loop

    '    '                '--- sort districtOID
    '    '                Dim dataView As New DataView(dt)
    '    '                dataView.Sort = "DistrictOID"
    '    '                Dim dataTable As DataTable = dataView.ToTable

    '    '                If dataView.Count > 0 Then
    '    '                    For Each drDetail As DataRowView In dataView
    '    '                        '--- get Individual Member VoteID from ALIS
    '    '                        dsIndVoteOID = ALIS_DataSet("SELECT ALIS.INDIVIDUAL_VOTE_S.NEXTVAL AS IND_VOTEOID FROM DUAL", "R")
    '    '                        indVoteOID = dsIndVoteOID.Tables(0).Rows(0).Item("IND_VoteOID")
    '    '                        ALIS_DataSet("INSERT INTO Individual_Vote(oid, oid_legislator, oid_vote, oid_organization, vote) VALUES (" & indVoteOID & ", " & drDetail("SenatorOID") & ", " & VoteOID & ", " & drDetail("DistrictOID") & ", '" & drDetail("Vote") & "')", "A")
    '    '                    Next
    '    '                End If

    '    '                If gTestMode Then
    '    '                    V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & ", TEST = 1 WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")
    '    '                Else
    '    '                    V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & ", TEST = 0 WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")
    '    '                End If
    '    '            Next
    '    '            gSendVotesToALIS = False

    '    '            If gReSendALIS Then
    '    '                DisplayMessage("Process all of UNSEND votes to ALIS finished.", "Send Votes To ALIS", "S")
    '    '                Exit Function
    '    '            End If

    '    'ProcExit:

    '    '        Catch ex As Exception
    '    '            RetValue = DisplayMessage(ex.Message & vbCrLf & "An error occurred while Re-Send the votes to ALIS.  If the connection to ALIS is down, you should exit the system " & _
    '    '            "and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Execute: SendUnSendVotesToALIS()", "S")
    '    '            Exit Function
    '    '        End Try
    '    '    End Function

    '    Public Sub DownloadSenatorsFromALIS()
    '        Dim i As Short
    '        Dim dsSenator As New DataSet
    '        Dim dsDistrict As New DataSet
    '        Dim drSenator, drSDistrict As DataRow
    '        Dim dt As New DataTable
    '        dt.Clear()
    '        dt.Columns.Clear()
    '        dt.Columns.Add("SenatorName")
    '        dt.Columns.Add("SenatorOID")
    '        dt.Columns.Add("DistrictOID")

    '        Try

    '            If Not DisplayMessage("You are about to download senators from ALIS.  " & "Do you want to continue?", "Download Senators From ALIS", "Y") Then
    '                gNbrSenators = 0
    '                Exit Sub
    '            End If

    '            '--- Clear tblSenators table first
    '            V_DataSet("Delete From tblSenators", "D")


    '            '--- senators are read in by their distric #, so, the first one read is
    '            '--- from district 1 and gets assigned as senator #1;
    '            '--- also read another ALIS sql stmt to get the oid of the district which
    '            '--- is used when the votes are recorded; oids are read in the same
    '            '--- order such that the first oid is assigned to the now senator #1

    '            i = 0
    '            Dim strSql As String
    '            ' strSql = " SELECT A.NAME AS SenatorName, A.OID as SenatorOID, O.OID as DistrictOID" & _
    '            strSql = " SELECT A.NAME AS SenatorName, A.OID as SenatorOID, O.OID as DistrictOID" & _
    '                    " FROM ALIS_OBJECT A, POSITION PO,  Organization O " & _
    '                    " WHERE(A.OID_POSITION = PO.OID) " & _
    '                        " AND (A.INCUMBENCY = 'T') " & _
    '                        " AND (A.INCUMBENCY_TYPE_CODE = 'ET') " & _
    '                        " AND start_date < sysdate  " & _
    '                        " AND nvl(end_date, sysdate) >= sysdate  " & _
    '                        " AND (PO.TYPE_CODE = 1) " & _
    '                        " AND PO.oid_organization = O.OID " & _
    '                        " ORDER BY O.OID "
    '            dsSenator = ALIS_DataSet(strSql, "R")

    '            strSql = ""
    '            If dsSenator.Tables(0).Rows.Count > 0 Then
    '                For Each drSenator In dsSenator.Tables(0).Rows
    '                    strSql = "Insert Into tblSenators Values ('" & Replace(drSenator("SenatorName"), """", "") & "', " & drSenator("SenatorOID") & ", '', " & i + 1 & ", " & drSenator("DistrictOID") & ")"
    '                    V_DataSet(strSql, "A")
    '                    i += 1
    '                Next
    '                gNbrSenators = dsSenator.Tables(0).Rows.Count

    '                LoadSenatorsIntoArray()

    '                RetValue = DisplayMessage("Senator download complete.  Don't forget to add the senator's salutations.", "", "I")
    '            Else
    '                DisplayMessage("There are not Senators to download. Please contact to your Administrator.", "Download Senators From ALIS", "S")
    '                End
    '            End If

    '        Catch ex As Exception
    '            DisplayMessage(ex.Message & " Failed download senators from ALIS. Please contact to your Administrator.", "Download Senators From ALIS", "S")
    '            End
    '        End Try
    '    End Sub


#End Region

#Region "Old-Codes"
    ' Public gRequestvoteidqueue As String                '   ".\PRIVATE$\requestvoteidqueue"
    '  Public gCmdFile As String = "c:\VotingSystem\RestartMsgQueue.cmd"

    '    Public Function SendVotesToALIS() As Integer
    '        Dim indVoteOID As Long, VoteOID As Long
    '        Dim str, strAdd As String
    '        Dim ds, dsIndVoteOID, dsVoteOID, dsDetail As New DataSet
    '        Dim dr As DataRow

    '        Try
    '            '--- if it is test and not write to ALIS or ALIS database un-accessiable, quite send votes to ALIS
    '            If (gTestMode And Not gWriteVotesToTest) Then
    '                GoTo ProcExit
    '            End If

    '            '--- check able to connecte ALIS
    '            '--- Check_ALIS_Database_Accessible()
    '            If gTestMode = False Then
    '                If (Not AlisProda_On And Not AlisProdb_On) Then
    '                    DisplayMessage("An error occurred while sending the votes to ALIS.  If the connection to ALIS is down, you should exit the system " & _
    '                    "and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Vote Not Sent To ALIS", "S")
    '                    gSendVotesToALIS = True
    '                    GoTo ProcExit
    '                End If
    '            ElseIf (gTestMode And gWriteVotesToTest) Then
    '                If Not AlisTest_On Then
    '                    DisplayMessage("An error occurred while sending the votes to ALIS_TEST.  If the connection to ALIS_TEST is down, you should exit the system " & _
    '                  "and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Vote Not Sent To ALIS", "S")
    '                    gSendVotesToALIS = True
    '                    GoTo ProcExit
    '                End If
    '            End If


    '            '--- now add senator votes
    '            '--- pick out vote, districtOID, and senatorOID from this string

    '            strSQL = "Select * From tblSenatorsVote Where VoteID =" & gVoteID & " Order By DistrictOID"
    '            dsDetail = V_DataSet(strSQL, "R")

    '            dsVoteOID = ALIS_DataSet("SELECT ALIS.VOTE_S.NEXTVAL AS VOTEOID FROM DUAL", "R")
    '            If dsVoteOID.Tables(0).Rows.Count > 0 Then
    '                VoteOID = dsVoteOID.Tables(0).Rows(0).Item("VOTEOID")
    '            End If

    '            If dsDetail.Tables(0).Rows.Count > 0 Then
    '                For Each drDetail In dsDetail.Tables(0).Rows
    '                    If gTestMode And gWriteVotesToTest Then                     '1 gTest votes
    '                        str = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 1 And AlisVoteOID=0 AND VoteID =" & gVoteID
    '                    Else                                                        '0 gProduction votes
    '                        str = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 0 And AlisVoteOID=0 AND VoteID =" & gVoteID
    '                    End If
    '                    ds = V_DataSet(str, "R")
    '                    If ds.Tables(0).Rows.Count > 0 Then
    '                        For Each dr In ds.Tables(0).Rows
    '                            strAdd = "INSERT INTO Vote (oid, oid_legislative_Body, oid_Legislative_day, voteId, nays, yeas, abstains, pass) VALUES (" & VoteOID & ", " & gSenateID & ", " & _
    '                                    dr("LegislativeDayOID") & ", " & dr("VoteID") & ", " & dr("TotalNay") & ", " & dr("TotalYea") & ", " & dr("TotalAbstain") & ", " & dr("TotalPass") & ")"
    '                            ALIS_DataSet(strAdd, "A")
    '                        Next
    '                    End If

    '                    '---Individual Member Vote ID
    '                    dsIndVoteOID = ALIS_DataSet("SELECT ALIS.INDIVIDUAL_VOTE_S.NEXTVAL AS IND_VOTEOID FROM DUAL", "R")
    '                    indVoteOID = dsIndVoteOID.Tables(0).Rows(0).Item("IND_VoteOID")

    '                    '--- now add senator votes
    '                    '--- pick out parents VoteID districtOID, senatorOID and vote from this string                 
    '                    ALIS_DataSet("INSERT INTO Individual_Vote(oid, oid_legislator, oid_vote, oid_organization, vote) VALUES (" & indVoteOID & ", " & drDetail("SenatorOID") & ", " & VoteOID & ", " & drDetail("DistrictOID") & ", '" & drDetail("Vote") & "')", "A")

    '                    If gTestMode Then
    '                        V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & ", TEST = 1 WHERE SessionID =" & gSessionID & " AND VoteID =" & gVoteID, "U")
    '                    Else
    '                        V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & ", TEST = 0 WHERE SessionID =" & gSessionID & " AND VoteID =" & gVoteID, "U")
    '                    End If
    '                Next
    '                V_DataSet("Delete From tblSenatorsVote", "D")
    '            Else
    '                MsgBox("Can not found " & gVoteID & " votes to send ALIS.", vbInformation, "Senad Votes to ALIS")
    '                Exit Function
    '            End If
    '            gSendVotesToALIS = False
    '            V_DataSet("Delete From tblSenatorsVote Where VoteID =" & gVoteID, "D")
    'ProcExit:

    '        Catch ex As Exception
    '            RetValue = DisplayMessage(ex.Message & vbCrLf & "An error occurred while sending the vote to ALIS.  If the connection to ALIS is down, you should exit the system " & _
    '            "and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Execute: SendVotesToALIS()", "S")
    '            Exit Function
    '        End Try
    '    End Function


    '    Public Function SendVotesToALIS() As Integer
    '        Dim k As Integer
    '        Dim indVoteOID As Long, VoteOID As Long, WrkFld As String, SenatorOID As Long, Vote As String
    '        Dim DistrictOID As Long
    '        Dim str, strAdd As String
    '        Dim ds, dsAdd, dsIndVoteOID, dsVoteOID, dsDetail As New DataSet
    '        Dim dr As DataRow

    '        Try
    '            '--- if it is test and not write to ALIS or ALIS database un-accessiable, quite send votes to ALIS
    '            If (gTestMode And Not gWriteVotesToTest) Then
    '                GoTo ProcExit
    '            End If

    '            '--- check able to connecte ALIS
    '            '--- Check_ALIS_Database_Accessible()
    '            If (Not AlisProda_On And Not AlisProdb_On) Then
    '                DisplayMessage("An error occurred while sending the votes to ALIS.  If the connection to ALIS is down, you should exit the system " & _
    '                "and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Vote Not Sent To ALIS", "S")
    '                gSendVotesToALIS = True
    '                GoTo ProcExit
    '            End If

    '            If gTestMode And gWriteVotesToTest Then                     '1 gTest votes
    '                str = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 1 And AlisVoteOID=0 AND VoteID =" & gVoteID
    '            Else                                                        '0 gProduction votes
    '                str = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 0 And AlisVoteOID=0 AND VoteID =" & gVoteID
    '            End If
    '            ds = V_DataSet(str, "R")

    '            For Each dr In ds.Tables(0).Rows
    '                '--- add vote header and retrieve the generated OID

    '                dsVoteOID = ALIS_DataSet("SELECT ALIS.VOTE_S.NEXTVAL AS VOTEOID FROM DUAL", "R")
    '                If dsVoteOID.Tables(0).Rows.Count > 0 Then
    '                    VoteOID = dsVoteOID.Tables(0).Rows(0).Item("VOTEOID")
    '                End If



    '                strAdd = "INSERT INTO Vote (oid, oid_legislative_Body, oid_Legislative_day, voteId, nays, yeas, abstains, pass) VALUES (" & VoteOID & ", " & gSenateID & ", " & _
    '                        dr("LegislativeDayOID") & ", " & dr("VoteID") & ", " & dr("TotalNay") & ", " & dr("TotalYea") & ", " & dr("TotalAbstain") & ", " & dr("TotalPass") & ")"

    '                '  strAdd = "INSERT INTO Vote (oid, oid_legislative_Body, oid_Legislative_day, voteId, nays, yeas, abstains, pass) VALUES (vote_s.nextval" & ", " & gSenateID & ", " & _
    '                '          dr("LegislativeDayOID") & ", " & dr("VoteID") & ", " & dr("TotalNay") & ", " & dr("TotalYea") & ", " & dr("TotalAbstain") & ", " & dr("TotalPass") & ")"
    '                'strAdd = "INSERT INTO Vote (oid, oid_legislative_Body, oid_Legislative_day, voteId, nays, yeas, abstains, pass) VALUES (vote_s.nextval " & ", " & gSenateID & ", " & _
    '                '       dr("LegislativeDayOID") & ", 13, " & dr("TotalNay") & ", " & dr("TotalYea") & ", " & dr("TotalAbstain") & ", " & dr("TotalPass") & ")"
    '                ALIS_DataSet(strAdd, "A")

    '                'Individual Member Votes
    '                'dsIndVoteOID = ALIS_DataSet("SELECT ALIS.INDIVIDUAL_VOTE_S.NEXTVAL AS IND_VOTEOID FROM DUAL", "R")
    '                'indVoteOID = dsIndVoteOID.Tables(0).Rows(0).Item("IND_VoteOID")

    '                '--- now add senator votes
    '                '--- pick out vote, districtOID, and senatorOID from this string
    '                WrkFld = dr("SenatorVotes")
    '                k = 0
    '                Do
    '                    k = k + 1
    '                    WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1) ' skip senator name
    '                    Vote = Microsoft.VisualBasic.Left(WrkFld, 1)
    '                    WrkFld = Mid$(WrkFld, 3)
    '                    DistrictOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, "*") - 1))
    '                    WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1)
    '                    SenatorOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, ";") - 1))
    '                    ' ALIS_DataSet("INSERT INTO Individual_Vote(oid, oid_legislator, oid_vote, oid_organization, vote) VALUES (individual_vote_s.NEXTVAL, " & SenatorOID & "' " & VoteOID & "' " & DistrictOID & ", " & Vote & ")", "A")
    '                    dsIndVoteOID = ALIS_DataSet("SELECT ALIS.INDIVIDUAL_VOTE_S.NEXTVAL AS IND_VOTEOID FROM DUAL", "R")
    '                    indVoteOID = dsIndVoteOID.Tables(0).Rows(0).Item("IND_VoteOID")
    '                    ALIS_DataSet("INSERT INTO Individual_Vote(oid, oid_legislator, oid_vote, oid_organization, vote) VALUES (" & indVoteOID & ", " & SenatorOID & ", " & VoteOID & ", " & DistrictOID & ", '" & Vote & "')", "A")

    '                    If InStr(WrkFld, ";") = Len(WrkFld) Then
    '                        Exit Do
    '                    End If
    '                    WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '                Loop

    '                If gTestMode Then
    '                    V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & ", TEST = 1 WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")
    '                Else
    '                    V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & ", TEST = 0 WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")
    '                End If
    '            Next
    '            gSendVotesToALIS = False

    'ProcExit:

    '        Catch ex As Exception
    '            RetValue = DisplayMessage(ex.Message & vbCrLf & "An error occurred while sending the vote to ALIS.  If the connection to ALIS is down, you should exit the system " & _
    '            "and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Execute: SendVotesToALIS()", "S")
    '            Exit Function
    '        End Try
    '    End Function

    'Function ALISConnectionOK() As Boolean
    '    '--- will only be here if initial connect to ALIS was unsuccessful and now are trying to
    '    '--- download/upload from/to ALIS; this is an attempt to establish a connection

    '    Dim cn As New OleDbConnection, RetValue As Object

    '    Try
    '        ALISConnectionOK = False
    '        cn = connProdb
    '        If cn.State = ConnectionState.Closed Then
    '            cn.Open()
    '            If cn.State = ConnectionState.Closed Then
    '                RetValue = DisplayMessage("Unable to open the ALIS connection to Woody " & cn.ConnectionString, _
    '                   "Unable To Open Connection", "S")
    '            Else
    '                ALISConnectionOK = True
    '                WoodyAlive = True
    '                RetValue = DisplayMessage("The connection with ALIS has been established. It if depending upon when the ALIS connection initially, you may need to check the correctness of the vote IDs.", "Execute: ALISConnectionOK()", "S")
    '            End If
    '        End If
    '        cn.Close()
    '    Catch ex As Exception
    '        DisplayMessage("Unable access ALIS Database", "Execute: ALISConnectionOK()", "S")
    '        Exit Function
    '    End Try
    'End Function


    '    Public Function DownloadBillsFromALIS() As Integer
    '        Dim drV, drA, drA1 As DataRow
    '        Dim dsV As New DataSet
    '        Dim dsA As New DataSet
    '        Dim dsA1 As New DataSet
    '        Dim FieldValue(), FieldName() As Object
    '        Dim Title, WrkFld, str As String, RetValue As Object

    '        On Error GoTo ProcERH
    '        ERHPushStack("DownloadBillsFromALIS")

    '        DownloadBillsFromALIS = False

    '        '--- if no ALIS connection, the attempt it again
    '        If Not WoodyAlive Then
    '            If Not ALISConnectionOK() Then
    '                GoTo ProcExit
    '            End If
    '        End If

    '        RetValue = DisplayMessage("Please wait while " & IIf(gWriteVotesToTest, "gTest ", "") & "bills are downloaded from ALIS.", "Excute: DownloadBillsFromALIS() function", "I")


    '        '--- get legislative day and session data; check for multiple open legislative days; if
    '        '--- found, then display and allow user to select or cancel; cancel
    '        '--- exits the system and assumes someone will fix on the ALIS end; only
    '        '--- trap 3 open days
    '        str = "SELECT LD.OID, LD.LEGISLATIVE_DAY, " & _
    '                "TO_CHAR(LD.CALENDAR_DATE,  'YYYY-MM-DD') AS CAL_DAY, " & _
    '                "UPPER(LD.STATUS_CODE) AS STATUS, LD.OID_SESSION, " & _
    '                "LS.LABEL, LS.ABBREVIATION " & _
    '                "FROM LEGISLATIVE_DAY LD, LEGISLATIVE_SESSION LS " & _
    '                "WHERE (LD.OID_SESSION = LS.OID) AND " & _
    '                "(NOT (LD.LEGISLATIVE_DAY IS NULL)) AND " & _
    '                "(LD.Status_code = 'O') " & _
    '                "ORDER BY LD.LEGISLATIVE_DAY DESC"
    '        dsA = ALIS_DataSet(str, "R")


    '        K = -1
    '        For Each drA In dsA.Tables(0).Rows
    '            K = K + 1
    '            ReDim Preserve gSessionIDTmp(K)
    '            ReDim Preserve gLegislativeDayTmp(K)
    '            ReDim Preserve gLegislativeDayOIDTmp(K)
    '            ReDim Preserve gCalendarDayTmp(K)
    '            ReDim Preserve gSessionNameTmp(K)
    '            ReDim Preserve gSessionAbbrevTmp(K)
    '            gSessionIDTmp(K) = drA("OID_Session")
    '            gLegislativeDayTmp(K) = drA("Legislative_Day")
    '            gLegislativeDayOIDTmp(K) = drA("OID")
    '            gCalendarDayTmp(K) = FormatDateTime(drA("Cal_Day"), DateFormat.ShortDate)
    '            gSessionNameTmp(K) = drA("Label")
    '            gSessionAbbrevTmp(K) = drA("Abbreviation")
    '            If K = 2 Then
    '                Exit For
    '            End If
    '        Next

    '        If K = -1 Then
    '            RetValue = DisplayMessage("There are No open legislative days. Download from ALIS terminated.", "Download Bills From ALIS", "S")
    '            Exit Function
    '        End If

    '        '--- get current open session id, leg day, and calendar day too, when k=0, if k > 0, there are more than one LEG day open.
    '        If K = 0 Then
    '            gSessionID = gSessionIDTmp(0)
    '            gLegislativeDay = gLegislativeDayTmp(0)
    '            gLegislativeDayOID = gLegislativeDayOIDTmp(0)
    '            gCalendarDay = gCalendarDayTmp(0)
    '            gSessionName = gSessionNameTmp(0)
    '            gSessionAbbrev = gSessionAbbrevTmp(0)
    '        Else
    '            frmMultipleOpenLegislativeDays.Show()
    '            WaitUntilFormCloses("frmMultipleOpenLegislativeDays")
    '        End If
    '        dsA.Dispose()

    '        '--- if different legislatuve day, then update recall votes for Voting PC to 0
    '        If Val(gLegislativeDay) <> glastLegislativeDay Then
    '            strSQL = "Update tblVotingDisplayParameters SET RecallVote1 = 0,RecallVote2 = 0,RecallVote3 = 0"
    '            V_DataSet(strSQL, "U")
    '            Dim ds As DataSet

    '        End If


    '        '--- get last VOTE ID from ALIS for this session and update the
    '        '--- parameter table
    '        str = " SELECT MAX(TO_NUMBER(voteid)) AS LastVoteID  " & _
    '                " FROM LEGISLATIVE_DAY LD, VOTE " & _
    '                " WHERE LD.oid = VOTE.oid_legislative_day " & _
    '                " AND (LD.oid_session =" & gSessionID & ") " & _
    '                " AND (VOTE.oid_legislative_body = " & gSenateID & ")"
    '        dsA1 = ALIS_DataSet(str, "R")

    '        If dsA1.Tables(0).Rows.Count = 0 Then
    '            RetValue = DisplayMessage("Cannot find last vote ID for session " & gSessionName & _
    '               " in ALIS. The last vote ID will be set to 0.  If this is not correct, you must enter the last vote ID using the Parameter Maintenance form.", "No Vote ID In ALIS", "S")
    '            gLastVoteIDPerALIS = 0
    '        Else
    '            For Each drA1 In dsA1.Tables(0).Rows
    '                If IsDBNull(drA1("LastVoteID")) Then
    '                    gLastVoteIDPerALIS = 0
    '                Else
    '                    gLastVoteIDPerALIS = CInt(drA1("LastVoteID"))
    '                End If
    '            Next
    '        End If

    '        '--- now get see if there is a higher local vote id not yet sent to ALIS
    '        '--- if so, then update the last vote with it
    '        str = "SELECT MAX(VoteID) AS LastVoteID FROM tblRollCallVotes WHERE SessionID = " & gSessionID & " AND ALISVoteOID = 0"
    '        dsV = V_DataSet(str, "R")
    '        If dsV.Tables(0).Rows.Count = 0 Then
    '            gLastVoteIDPerLocal = 0
    '        Else
    '            For Each drV In dsV.Tables(0).Rows
    '                If IsDBNull(drV("LastVoteID")) Then
    '                    gLastVoteIDPerLocal = 0
    '                Else
    '                    gLastVoteIDPerLocal = drV("LastVoteID")
    '                End If
    '            Next
    '        End If

    '        If gLastVoteIDPerLocal > gLastVoteIDPerALIS Then
    '            gVoteID = gLastVoteIDPerLocal
    '        Else
    '            gVoteID = gLastVoteIDPerALIS
    '            RetValue = DisplayMessage("Attention: Local Last Vote ID is less than ALIS production Last Vote ID. System will assign ALIS production last vote ID to current vote ID.", "Excute: DownloadBillsFromALIS() function", "I")
    '        End If


    '        '--- Now update the tblVoteParameters table
    '        If gTestMode = False Then
    '            V_DataSet("Update tblVotingParameters SET ParameterValue='" & gVoteID & "' Where Parameter='LastVoteIDForThisSession'", "U")
    '        Else
    '            V_DataSet("Update tblVotingParameters SET ParameterValue='" & gVoteID & "' Where Parameter='LastVoteIDForThisSessionTEST'", "U")
    '        End If

    '        '--- Delete Special Order Calenders  ALWAYS DO THESE!!!
    '        V_DataSet("DELETE FROM tblSpecialOrderCalendar ", "D")


    '        '--- save any bills that have something in the work area to a temporary table and
    '        '--- then restore the work areas if the same bills are downloaded again
    '        On Error Resume Next
    '        V_DataSet("Update tblWork Set CalendarCode ='', BillNbr = '', WorkData =''", "U")
    '        On Error GoTo ProcERH
    '        Dim dsWorkData As New DataSet
    '        dsWorkData = V_DataSet("Select CalendarCode, BillNbr, WorkData FROM tblBills WHERE WorkData <> ''", "R")
    '        If dsWorkData.Tables(0).Rows.Count <> 0 Then
    '            For Each dr As DataRow In dsWorkData.Tables(0).Rows
    '                strSQL = "Update tblWork Set CalendarCode ='" & dr("CalendarCode") & "', BillNbr ='" & dr("BillNbr") & "', WorkData ='" & dr("WorkData") & "'"
    '                V_DataSet(strSQL, "U")
    '            Next
    '        End If
    '        'strSQL = "Select CalendarCode, BillNbr, WorkData INTO tblWork FROM tblBills WHERE (WorkData <> '')"

    '        'V_DataSet(strSQL, "U")


    '        ' gHTMLFile = Mid$(gHTMLFile, 1, InStr(gHTMLFile, "*") - 1) & gSessionAbbrev & Mid$(gHTMLFile, InStr(gHTMLFile, "*") + 1)


    '        '--- field 0 - calendar code
    '        '--- field 1 - bill #
    '        '--- field 2 - bill description - what will be displayed for the bil - composed of bill #, sponsor, , subject (includes desc for confirmation), and calendar page
    '        '--- field 3 - sponsor
    '        '--- field 4 - subject
    '        '--- field 5 - work area data - always blank when added here - built by chamber display
    '        '--- field 6 - calendar page


    '        '---delete all Other bills dynamically retrieved during the day
    '        V_DataSet("Delete From tblBills Where CalendarCode ='ZZ'", "D")


    '        ' 1 --- Get Regular Order Bills
    '        ' delete Regular Bills first
    '        V_DataSet("Delete From tblBills Where CalendarCode ='1'", "D")

    '        ReDim FieldValue(6)
    '        str = " SELECT label, sponsor, alis_object.index_word, calendar_page " & _
    '                " FROM alis_object, matter " & _
    '                " WHERE matter.oid_instrument = alis_object.oid AND matter.oid_session = " & gSessionID & " AND alis_object.oid_session = " & gSessionID & _
    '                " AND matter.matter_status_code = 'Pend' AND matter.oid_legislative_body = '1753' AND alis_object.legislative_body = 'S' " & _
    '                " AND matter.calendar_sequence_no > 0 ORDER BY matter.calendar_sequence_no"
    '        dsA = ALIS_DataSet(str, "R")
    '        If dsA.Tables(0).Rows.Count > 0 Then
    '            For Each drA In dsA.Tables(0).Rows
    '                FieldValue(0) = "1"
    '                If IsDBNull(drA("Label")) Then
    '                    FieldValue(1) = ""
    '                Else
    '                    FieldValue(1) = drA("Label")
    '                End If
    '                If Strings.Left(FieldValue(1), 1) = "S" Then
    '                    Title = " by Senator "
    '                Else
    '                    Title = " by Rep. "
    '                End If
    '                If IsDBNull(drA("Sponsor")) Then
    '                    FieldValue(3) = ""
    '                Else
    '                    FieldValue(3) = drA("Sponsor")
    '                End If
    '                If IsDBNull(drA("Index_Word")) Then
    '                    FieldValue(4) = ""
    '                Else
    '                    FieldValue(4) = NToB(Replace(drA("Index_Word"), "'", " "))
    '                End If
    '                If IsDBNull(FieldValue(5)) Or Not IsDBNull(FieldValue(5)) Then
    '                    FieldValue(5) = ""
    '                End If
    '                If IsDBNull(drA("Calendar_Page")) Then
    '                    FieldValue(6) = 0
    '                Else
    '                    FieldValue(6) = drA("Calendar_Page")
    '                End If

    '                If FieldValue(6) <> 0 Then
    '                    FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4) & " p." & FieldValue(6)
    '                Else
    '                    FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4)
    '                End If
    '                V_DataSet("INSERT INTO tblBills VALUES ('1', '" & FieldValue(1) & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "')", "A")
    '            Next
    '        End If


    '        ' 2 --- Get Local Bills
    '        ' delete Local bills first
    '        V_DataSet("Delete From tblBills Where CalendarCode ='2'", "D")

    '        str = "SELECT label, calendar_page, senate_display_title, sponsor " & _
    '                " FROM alis_object, matter WHERE matter.oid_instrument = alis_object.oid " & _
    '                " AND matter.oid_session = " & gSessionID & " AND alis_object.oid_session = " & gSessionID & " AND matter.matter_status_code = 'Pend' " & _
    '                " AND matter.oid_legislative_body = '1753' AND alis_object.legislative_body = 'S' AND alis_object.local_bill = 'T' " & _
    '                " AND matter.calendar_sequence_no > 0 ORDER BY matter.calendar_sequence_no "
    '        dsA = ALIS_DataSet(str, "R")

    '        If dsA.Tables(0).Rows.Count > 0 Then
    '            For Each drA In dsA.Tables(0).Rows
    '                FieldValue(0) = "2"
    '                FieldValue(1) = NToB(("Label"))
    '                If Strings.Left(FieldValue(1), 1) = "S" Then
    '                    Title = " by Senator "
    '                Else
    '                    Title = " by Rep. "
    '                End If
    '                FieldValue(3) = NToB(drA("Sponsor"))
    '                FieldValue(4) = Mid$(NToB(drA("Senate_Display_Title")), InStr(NToB(drA("Senate_Display_Title")), " ") + 1)
    '                FieldValue(5) = ""
    '                FieldValue(6) = NToB(drA("Calendar_Page"))
    '                If CType(FieldValue(6), String) <> "" Then
    '                    FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4) & " p." & FieldValue(6)
    '                Else
    '                    FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4)
    '                End If
    '                V_DataSet("INSERT INTO tblBills VALUES ('2', '" & drA("Label") & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "')", "A")
    '            Next
    '        End If


    '        '3 ---Get Confirmations
    '        ' delete confirmatins
    '        V_DataSet("Delete From tblBills Where CalendarCode ='3'", "D")

    '        str = "SELECT I.OID, I.LABEL, I.TYPE_CODE, PE.FIRSTNAME, PE.LASTNAME, PE.SUFFIX, O.NAME, CO.ABBREVIATION, TO_CHAR(R1DAY.CALENDAR_DATE, 'MM/DD/YYYY') AS R1DATE, TO_CHAR(MDAY.CALENDAR_DATE, 'MM/DD/YYYY') AS MDATE, " & _
    '                " M.TRANSACTION_TYPE_CODE AS MTRAN, M.MATTER_STATUS_CODE AS MSTAT, M.RECOMMENDATION_CODE AS MREC, AU.NAME AS AUTHORITY, CV.Value AS INSTR_VALUE " & _
    '            " FROM ALIS_OBJECT I, MATTER C1, MATTER M, ORGANIZATION CO, ORGANIZATION O, POSITION PO, POSITION AU, PERSON PE, LEGISLATIVE_DAY R1DAY, LEGISLATIVE_DAY MDAY, CODE_VALUES CV " & _
    '            " WHERE I.INSTRUMENT = 'T' AND I.TYPE_CODE = 'CF' AND I.STATUS_CODE = CV.CODE (+) AND CV.CODE_TYPE = 'InstrumentStatus' AND I.OID_SESSION = " & gSessionID & " AND I.OID_POSITION = PO.OID (+) " & _
    '                " AND O.OID (+) = PO.OID_ORGANIZATION AND AU.OID (+) = PO.OID_APPOINTING_AUTHORITY AND I.OID = C1.OID_INSTRUMENT AND M.OID_COMMITTEE = CO.OID (+) AND C1.OID_CANDIDACY = PE.OID (+) " & _
    '                " AND C1.OID_CONSIDERED_DAY = R1DAY.OID (+) AND M.OID_CONSIDERED_DAY = MDAY.OID (+) AND M.OID_INSTRUMENT = I.OID AND M.SEQUENCE = I.LAST_MATTER AND I.OID_SESSION = C1.OID_SESSION " & _
    '                " AND C1.SEQUENCE = (SELECT MIN(X.SEQUENCE) FROM MATTER X WHERE X.OID_SESSION = C1.OID_SESSION AND X.OID_INSTRUMENT = C1.OID_INSTRUMENT AND X.TRANSACTION_TYPE_CODE = 'R1') ORDER BY SUBSTR(I.LABEL, 1, 1), I.TYPE_CODE, I.ID"
    '        dsA = ALIS_DataSet(str, "R")

    '        If dsA.Tables(0).Rows.Count > 0 Then
    '            For Each drA In dsA.Tables(0).Rows
    '                FieldValue(0) = "3"
    '                FieldValue(1) = NToB(Mid$(drA("Label"), 2))
    '                FieldValue(3) = NToB(drA("FirstName")) & " " & NToB(drA("LastName"))
    '                FieldValue(4) = ""

    '                '--- if boards and commissions exist, then search for the senate voting name and replace if found            
    '                If NToB(drA("Name")) = "" Then
    '                    FieldValue(2) = FieldValue(1) & " - " & FieldValue(3) & " -- "
    '                Else
    '                    WrkFld = NToB(drA("Name"))
    '                    str = "SELECT SenateVotingName FROM tblBoardsAndCommissions WHERE AlisName = '" & Replace(WrkFld, "'", "''") & "'"
    '                    dsV = V_DataSet(str, "R")
    '                    If dsV.Tables(0).Rows.Count = 0 Then
    '                        FieldValue(2) = FieldValue(1) & " - " & FieldValue(3) & " -- " & NToB(drA("Name"))
    '                    Else
    '                        FieldValue(2) = FieldValue(1) & " - " & FieldValue(3) & " -- " & dsV.Tables(0).Rows(0).Item("SenateVotingName")
    '                    End If
    '                End If
    '                FieldValue(5) = ""
    '                FieldValue(6) = ""
    '                V_DataSet("INSERT INTO tblBills VALUES ('3', '" & drA("Label") & "', '" & Replace(FieldValue(2), "'", "''") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "')", "A")
    '            Next
    '        End If

    '        '--- restore any saved work areas to the bills downloaded
    '        dsV = V_DataSet("Select * From tblWork", "R")
    '        For Each drV In dsV.Tables(0).Rows
    '            V_DataSet("UPDATE tblBills SET WorkData ='" & Replace(drV("WorkData"), "'", "''") & "' WHERE CalendarCode ='" & NToB(drV("CalendarCode")) & "' AND BillNbr ='" & NToB(drV("BillNbr")) & "'", "U")
    '        Next
    '        '--- clear tblWork 
    '        V_DataSet("Update tblWork Set CalendarCode = '', BillNbr = '', WorkData = ''", "U")

    '        RetValue = DisplayMessage("Download of " & IIf(gWriteVotesToTest, "TEST ", "") & "bills from ALIS completed.", "Alabama Senate Voting System", "I")

    '        DownloadBillsFromALIS = True

    'ProcExit:
    '        ERHPopStack()
    '        Exit Function

    'ProcERH:
    '        ERHHandler()
    '        Exit Function
    '    End Function



    '    Public Sub ApplySpecificAttributesToDisplayBoard(BoardID As String, Attributes As String)

    '        Dim WrkFld As String, i As Integer


    '        On Error GoTo ProcERH
    '        ERHPushStack("ApplySpecificAttributesToDisplayBoard - " & BoardID)

    '        ' make display board invisible and then visible at end after
    '        ' setting length =0 to eliminate flickering of selected area

    '        If BoardID = "OOB" Then
    '            Pfrm.OOBDisplayBoard.Visible = False")
    '            Pfrm.OOBDisplayBoard.SelectionStart = gStart
    '            Pfrm.OOBDisplayBoard.SelectionLength = gLength

    '            WrkFld = Attributes
    '            Pfrm.OOBDisplayBoard.SelectionFont = New Drawing.Font("Aril", currentFont.Size, newFontStyle)  'Mid$(WrkFld, 1, InStr(WrkFld, ";") - 1)
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '            Pfrm(BoardID & "DisplayBoard").SelFontSize = Val(Mid$(WrkFld, 1, InStr(WrkFld, ";") - 1))
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '            If Left$(WrkFld, 1) = "N" Then
    '                Pfrm(BoardID & "DisplayBoard").SelBold = False
    '            Else
    '                Pfrm(BoardID & "DisplayBoard").SelBold = True
    '            End If
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '            If Left$(WrkFld, 1) = "N" Then
    '                Pfrm(BoardID & "DisplayBoard").SelItalic = False
    '            Else
    '                Pfrm(BoardID & "DisplayBoard").SelItalic = True
    '            End If
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '            If Left$(WrkFld, 1) = "N" Then
    '                Pfrm(BoardID & "DisplayBoard").SelUnderline = False
    '            Else
    '                Pfrm(BoardID & "DisplayBoard").SelUnderline = True
    '            End If
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)

    '            Select Case WrkFld
    '                Case "Black" : Pfrm(BoardID & "DisplayBoard").SelColor = vbBlack
    '                Case "Red" : Pfrm(BoardID & "DisplayBoard").SelColor = vbRed
    '                Case "Green" : Pfrm(BoardID & "DisplayBoard").SelColor = vbGreen
    '                Case "Blue" : Pfrm(BoardID & "DisplayBoard").SelColor = vbBlue
    '                Case "Magenta" : Pfrm(BoardID & "DisplayBoard").SelColor = vbMagenta
    '            End Select

    '        ElseIf BoardID = "Bill" Then
    '            Pfrm(BoardID & "DisplayBoard").Visible = False
    '            Pfrm(BoardID & "DisplayBoard").SelStart = gStart
    '            Pfrm(BoardID & "DisplayBoard").SelLength = gLength

    '            WrkFld = Attributes
    '            Pfrm(BoardID & "DisplayBoard").SelFontName = Mid$(WrkFld, 1, InStr(WrkFld, ";") - 1)
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '            Pfrm(BoardID & "DisplayBoard").SelFontSize = Val(Mid$(WrkFld, 1, InStr(WrkFld, ";") - 1))
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '            If Left$(WrkFld, 1) = "N" Then
    '                Pfrm(BoardID & "DisplayBoard").SelBold = False
    '            Else
    '                Pfrm(BoardID & "DisplayBoard").SelBold = True
    '            End If
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '            If Left$(WrkFld, 1) = "N" Then
    '                Pfrm(BoardID & "DisplayBoard").SelItalic = False
    '            Else
    '                Pfrm(BoardID & "DisplayBoard").SelItalic = True
    '            End If
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '            If Left$(WrkFld, 1) = "N" Then
    '                Pfrm(BoardID & "DisplayBoard").SelUnderline = False
    '            Else
    '                Pfrm(BoardID & "DisplayBoard").SelUnderline = True
    '            End If
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)

    '            Select Case WrkFld
    '                Case "Black" : Pfrm(BoardID & "DisplayBoard").SelColor = vbBlack
    '                Case "Red" : Pfrm(BoardID & "DisplayBoard").SelColor = vbRed
    '                Case "Green" : Pfrm(BoardID & "DisplayBoard").SelColor = vbGreen
    '                Case "Blue" : Pfrm(BoardID & "DisplayBoard").SelColor = vbBlue
    '                Case "Magenta" : Pfrm(BoardID & "DisplayBoard").SelColor = vbMagenta
    '            End Select
    '        ElseIf BoardID = "Phrase" Then
    '            Pfrm(BoardID & "DisplayBoard").Visible = False
    '            Pfrm(BoardID & "DisplayBoard").SelStart = gStart
    '            Pfrm(BoardID & "DisplayBoard").SelLength = gLength

    '            WrkFld = Attributes
    '            Pfrm(BoardID & "DisplayBoard").SelFontName = Mid$(WrkFld, 1, InStr(WrkFld, ";") - 1)
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '            Pfrm(BoardID & "DisplayBoard").SelFontSize = Val(Mid$(WrkFld, 1, InStr(WrkFld, ";") - 1))
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '            If Left$(WrkFld, 1) = "N" Then
    '                Pfrm(BoardID & "DisplayBoard").SelBold = False
    '            Else
    '                Pfrm(BoardID & "DisplayBoard").SelBold = True
    '            End If
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '            If Left$(WrkFld, 1) = "N" Then
    '                Pfrm(BoardID & "DisplayBoard").SelItalic = False
    '            Else
    '                Pfrm(BoardID & "DisplayBoard").SelItalic = True
    '            End If
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '            If Left$(WrkFld, 1) = "N" Then
    '                Pfrm(BoardID & "DisplayBoard").SelUnderline = False
    '            Else
    '                Pfrm(BoardID & "DisplayBoard").SelUnderline = True
    '            End If
    '            WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)

    '            Select Case WrkFld
    '                Case "Black" : Pfrm(BoardID & "DisplayBoard").SelColor = vbBlack
    '                Case "Red" : Pfrm(BoardID & "DisplayBoard").SelColor = vbRed
    '                Case "Green" : Pfrm(BoardID & "DisplayBoard").SelColor = vbGreen
    '                Case "Blue" : Pfrm(BoardID & "DisplayBoard").SelColor = vbBlue
    '                Case "Magenta" : Pfrm(BoardID & "DisplayBoard").SelColor = vbMagenta
    '            End Select
    '        End If

    '        ' now left justify or center the display

    '        If (BoardID = "OOB") Then
    '            Pfrm.OOBDisplayBoard.SelectionStart = 0
    '            Pfrm.OOBDisplayBoard.SelectionLength = Strings.Len("Order Of Business") + 3 + gOOBLength
    '            Pfrm.OOBDisplayBoard.SelectionAlignment = 2      ' center
    '        ElseIf BoardID <> "Subject" Then ' this is on same line with bill so already done
    '            Pfrm(BoardID & "DisplayBoard").SelStart = 0
    '            Pfrm(BoardID & "DisplayBoard").SelLength = Len(Pfrm(BoardID & "DisplayBoard").Text)
    '            Pfrm(BoardID & "DisplayBoard").SelAlignment = 0 ' left
    '        End If
    '        Pfrm(BoardID & "DisplayBoard").SelLength = 0
    '        Pfrm(BoardID & "DisplayBoard").Visible = True

    '        ERHPopStack()
    '        Exit Sub

    'ProcERH:

    '        ERHHandler()
    '        Exit Sub

    '    End Sub


    '    Public Function SendVotesToALIS() As Integer
    '        ' if woody is dead, then there would be no access to ALIS; if woody's initial
    '        ' connection fails, then he is given buzz's connection, but if that connection
    '        ' fails then woody is really dead

    '        Dim k As Integer
    '        Dim VoteOID As Long, WrkFld As String, SenatorOID As Long, Vote As String
    '        Dim DistrictOID, SenatorVoteOID As Long
    '        Dim str, strAdd As String
    '        Dim ds, dsAdd, dsVoteOID As New DataSet
    '        Dim dr As DataRow

    '        On Error GoTo ProcERH
    '        ERHPushStack("SendVogTestoALIS")

    '        If ((gTestMode) And (Not gWriteVotesToTest)) Or (Not WoodyAlive) Then
    '            GoTo ProcExit
    '        End If

    '        'rmChamberDisplay.ALISTimer.Interval = 0    ' make so no entry while it is running

    '        If gWriteVotesToTest Then                   '1 gTest votes
    '            str = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And gTest =1 And AlisVoteOID=0"
    '        Else                                        '0 gProduction votes
    '            str = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And gTest =0 And AlisVoteOID=0"
    '        End If
    '        ds = V_DataSet(str, "R")

    '        For Each dr In ds.Tables(0).Rows
    '            ' add vote header and retrieve the generated OID
    '            strAdd = "INSERT INTO Vote (oid, oid_legislative_Body, oid_Legilative_day, voteId, nays, yeas, abstains, pass) VALUES (vote_s.nextval, " & ", " & gSenateID & "' " & _
    '            dr("LegislativeDayOID") & ", " & dr("VoteID") & ", " & dr("TotalNay") & ", " & dr("TotalYea") & ", " & dr("TotalAbstain") & ", " & dr("TotalPass") & ")"
    '            ALIS_DataSet(strAdd, cnALIS, "A")

    '            dsVoteOID = ALIS_DataSet("SELECT ALIS.VOTE_S.CURRVAL AS VOTEOID FROM DUAL", cnALIS, "S")

    '            VoteOID = dsVoteOID.Tables(0).Rows(0).Item("VoteOID")

    '            ' now add senator votes
    '            ' pick out vote, districtOID, and senatorOID from this string
    '            WrkFld = dr("SenatorVotes")
    '            k = 0
    '            Do
    '                k = k + 1
    '                WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1) ' skip senator name
    '                Vote = Left$(WrkFld, 1)
    '                WrkFld = Mid$(WrkFld, 3)
    '                DistrictOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, "*") - 1))
    '                WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1)
    '                SenatorOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, ";") - 1))
    '                ALIS_DataSet("INSERT INTO Individual_Vote(oid, oid_legislator, oid_vote, oid_organization, vote) VALUES (individual_vote_s.NEXTVAL, " & SenatorOID & "' " & VoteOID & "' " & DistrictOID & ", " & Vote & ")", cnALIS, "A")

    '                If InStr(WrkFld, ";") = Len(WrkFld) Then
    '                    Exit Do
    '                End If
    '                WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '            Loop

    '            V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & " WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")


    '            If (SD2Alive) And (gComputerName <> SD2) Then
    '                '??? VotingSystem.UpdateVoteAsSentToALISRedundant(VoteOID, gSessionID, dr("VoteID"))
    '            End If
    '        Next

    'ProcExit:
    '        ' frmChamberDisplay.ALISTimer.Interval = gALISTimer   ' reset timer
    '        ERHPopStack()
    '        Exit Function
    'ProcERH:
    '        ERHHandler()
    '        Exit Function
    '    End Function


    ' Function GetPassedParameters() As Integer
    '        Dim ctl As Control, ForUser As String
    '        Dim CurrentCalendarCode As String, CurrentCalendar As String, CurrentBillNbr As String
    '        Dim CurrentBill As String, LegislativeDay As String, SessionName As String
    '        Dim CurrentPhrase As Object, ChamberLight As Long, VotingLight As Long
    '        Dim VoteID As Long, VotingStarted As Integer, SessionID As Long, LegislativeDayOID As Long

    '        On Error GoTo ProcERH
    '        ERHPushStack("GetPassedParameters - " & gApplication)

    '        ' if already in this routine, just exit - in case timer sends us here before we are finished

    '        GetPassedParameters = True
    '        If gCheckingParams Then
    '            GoTo ProcExit
    '        End If

    '        ' if this app name = foruser, then the params need to be read; if not then the params are for
    '        ' the other app so just go to put param check to see if this app is putting params for the other app

    '        Open gDataPath & "User.txt" For Input As #1
    '        Line Input #1, ForUser
    '        Close #1

    '        If ForUser = "GoAway" Then
    '            GoTo ProcExit
    '        End If

    '        gCheckingParams = True

    '        If gPutParams Then
    '           Open gDataPath & "User.txt" For Output As #1
    '           Print #1, "GoAway"
    '           Close #1
    '        End If

    '        If ForUser <> gApplication Then
    '            GoTo PutParams
    '        End If

    '        Open gDataPath & "Param.txt" For Input As #2
    '        Input #2, CurrentCalendarCode, CurrentCalendar, CurrentBillNbr, CurrentBill, _
    '           CurrentPhrase, LegislativeDay, SessionName, SessionID, LegislativeDayOID, ChamberLight, VotingLight, VoteID, VotingStarted
    '        If (ForUser = "Voting") And (Not EOF(2)) Then   ' capture the display area for the report
    '              Input #2, gWorkData
    '        End If
    '        Close #2

    '        Select Case gApplication
    '            Case "Chamber"
    '                frm!VoteID = VoteID
    '                gVoteID = VoteID
    '                If ChamberLight = vbRed Then
    '                    frm!lblChamberLight.BackColor = vbRed
    '                    frm!lblChamberLight.Caption = "Now Voting"
    '                    gVotingStarted = VotingStarted
    '                    If gVotingStarted Then
    '                        frm!btnCancelVote.Enabled = False
    '                        frm!btnClearDisplay.Enabled = False

    '                        ' now that voting has started, clear the work area if BIR

    '                        If InStr(frm!WorkData, gBIR) = 1 Then
    '                            frm!WorkData = ""
    '                        End If
    '                    End If

    '                    frm!OrderOfBusiness.Enabled = False
    '                    frm!btnExit.Enabled = False
    '                ElseIf ChamberLight = vbGreen Then
    '                    frm!lblChamberLight.BackColor = vbGreen

    '                    ' if returning from voting to chamber before voting started, leave waiting message on chamber; when
    '                    ' voting starts, the voting PC will send a red light to chamber which will be captured above and will then
    '                    ' change the message

    '                    If frm!lblChamberLight.Caption = "Waiting For Voting" Then
    '                    Else
    '                        frm!lblChamberLight.Caption = "Display Next Bill"
    '                        gVotingStarted = False
    '                        frm!btnVote.Enabled = True
    '                        frm!btnClearDisplay.Enabled = True
    '                        frm!btnUpdateDisplay.Enabled = True
    '                        frm!btnCancelVote.Enabled = True
    '                        frm!OrderOfBusiness.Enabled = True
    '                        frm!btnExit.Enabled = True
    '                    End If

    '                End If

    '            Case "Voting"

    '                ' if chamber display has quit, then set to false as a flag to quit voting display

    '                If CurrentCalendarCode = "Exit" Then
    '                    GetPassedParameters = False
    '                ElseIf VotingLight = vbRed Then
    '                    frm!lblVotingLight.BackColor = vbRed
    '                    frm!lblVotingLight.Caption = "Waiting For Next Bill"
    '                    For Each ctl In frm
    '                        If Left$(ctl.Name, 3) = "btn" Then
    '                            ctl.Enabled = False
    '                        End If
    '                    Next ctl
    '                    If CurrentCalendarCode = "Cancel" Then  ' chamber cancelled the vote, so reset
    '                        gPutCalendarCode = "" ' reset it
    '                        frm!CurrentBill = ""
    '                        frm!CurrentMotion = ""
    '                    End If
    '                ElseIf VotingLight = vbGreen Then
    '                    frm!lblVotingLight.BackColor = vbGreen
    '                    frm!lblVotingLight.Caption = "OK To Vote"
    '                    gCalendar = CurrentCalendar
    '                    gCalendarCode = CurrentCalendarCode
    '                    gBill = CurrentBill
    '                    gBillNbr = CurrentBillNbr
    '                    gCurrentPhrase = CurrentPhrase
    '                    gLegislativeDay = LegislativeDay
    '                    gSessionName = SessionName
    '                    gSessionID = SessionID
    '                    gLegislativeDayOID = LegislativeDayOID
    '                    frm!CurrentBill = gBill
    '                    frm!CurrentMotion = gCurrentPhrase
    '                    For Each ctl In frm
    '                        If Left$(ctl.Name, 3) = "btn" Then
    '                            ctl.Enabled = True
    '                        End If
    '                    Next ctl

    '                End If

    '        End Select

    'PutParams:

    '        If gPutParams Then
    '            gPutParams = False
    '           Open gDataPath & "Param.txt" For Output As #2
    '           Write #2, gPutCalendarCode, gPutCalendar, gPutBillNbr, gPutBill, _
    '              gPutPhrase, gLegislativeDay, gSessionName, gSessionID, gLegislativeDayOID, gPutChamberLight, gPutVotingLight, gVoteID, gVotingStarted
    '            If gApplication = "Chamber" Then
    '              Write #2, frm!WorkData.Text   ' this is used by voting display for report
    '            End If
    '           Close #2
    '           Open gDataPath & "User.txt" For Output As #1
    '            If gApplication = "Chamber" Then
    '              Print #1, "Voting"
    '            Else
    '              Print #1, "Chamber"
    '            End If
    '           Close #1
    '        End If

    '        gCheckingParams = False

    'ProcExit:

    '        ERHPopStack()
    '        Exit Function

    'ProcERH:

    '        If Err() = 62 Then   ' attempt to read past eof
    '           Close #1
    '           Close #2
    '            Resume ProcExit
    '        End If

    '        ERHHandler()
    '        Exit Function

    'End Function


    'Public Sub IsFormAlreadyOpen(ByVal FormType As Type)
    '    For Each OpenForm As Form In Application.OpenForms
    '        If OpenForm.GetType() = FormType Then

    '        End If
    '    Next
    'End Sub

    '    Public Sub Get_Parameters()
    '        Dim ds As New DataSet
    '        Dim dsP As New DataSet
    '        Dim str As String
    '        Dim RepValue As New Object

    '        On Error GoTo ProcERH
    '        ERHPushStack("Excute: Get_Parameters")


    '        '--- Get parameters
    '        str = "Select * From tblParameters"
    '        ds = V_DataSet(str, "R")

    '        ggPingPCTTimer = ds.Tables(0).Rows(0).Item("gPingPCTTimer")
    '        gVotingTimer = ds.Tables(0).Rows(0).Item("VotingTimer")
    '        gALISTimer = ds.Tables(0).Rows(0).Item("ALISTimer")
    '        gSwitchBox = ds.Tables(0).Rows(0).Item("SwitchBox").ToString
    '        gVersionNbr = ds.Tables(0).Rows(0).Item("VersionNbr")
    '        gVoteID = ds.Tables(0).Rows(0).Item("LastVoteIDForThisSession")

    '        ' make switchbox long enough so assignments dont faulter; if for some reason this field is
    '        ' too short in the db, then the way the selections are made below should alert the user that
    '        ' all is not right and they should go to the maintenace form which will fix the problem

    '        Do Until Len(gSwitchBox) = 255
    '            gSwitchBox = gSwitchBox & "."
    '        Loop

    '        If Mid(gSwitchBox, 2, 1) = "N" Then
    '            gTestMode = False
    '        Else
    '            gTestMode = True
    '        End If
    '        If Mid(gSwitchBox, 3, 1) = "N" Then
    '            gWriteVotesToTest = False
    '        Else
    '            gWriteVotesToTest = True
    '        End If
    '        If Mid(gSwitchBox, 5, 1) = "N" Then
    '            gChamberHelp = False
    '        Else
    '            gChamberHelp = True
    '        End If
    '        If Mid(gSwitchBox, 6, 1) = "N" Then
    '            gVotingHelp = False
    '        Else
    '            gVotingHelp = True
    '        End If
    '        If Mid(gSwitchBox, 7, 1) = "N" Then
    '            gPrintVoteRpt = False
    '        Else
    '            gPrintVoteRpt = True
    '        End If
    '        If Mid(gSwitchBox, 8, 1) = "N" Then
    '            gDeletegTestVotesOnStartUp = False
    '        Else
    '            gDeletegTestVotesOnStartUp = True
    '        End If

    '        '   if not in gTest mode, then reset write to test flag to false
    '        If gTestMode = False And gWriteVotesToTest = True Then
    '            gWriteVotesToTest = False
    '            gSwitchBox = Left(gSwitchBox, 3) & "N" & Mid(gSwitchBox, 5)
    '            V_DataSet("UPDATE tblParameters SET SwitchBox='" & gSwitchBox & "'", "U")
    '        ElseIf gTestMode = True And gWriteVotesToTest = False Then
    '            V_DataSet("Delete From tblRollCallVotes WHERE SessionID=-1", "D")
    '            gSessionID = -1
    '            gLegislativeDayOID = 1
    '            gLegislativeDay = CStr(1)
    '            gCalendarDay = Format(Today, "m/d/yyyy")
    '            gVoteIDTest = 0
    '            ' gVoteID = 0
    '            V_DataSet("UPDATE tblParameters Set LastVoteIDForThisSession=0", "U")
    '        End If

    '        gNbrPhrasesToDisplay = Val(Mid(gSwitchBox, 10, 2))
    '        gNbrSenators = Val(Mid(gSwitchBox, 12, 2))
    '        gNbrVoteHistoryDays = Val(Mid(gSwitchBox, 14, 3))

    '        Exit Sub
    'ProcERH:
    '        ERHHandler()
    '        Exit Sub
    '    End Sub

    'Private Sub ReadDisplayBoardAttributes()
    '    Dim ds As New DataSet

    '    Try
    '        ds = V_DataSet("Select * From tblProjectionDisplayAttributes ", "R")
    '        If ds.Tables(0).Rows.Count <> 0 Then
    '            gOOBHeadingAttributes = ds.Tables(0).Rows(0).Item("OrderOfBusinessHeading")
    '            gNotSelectedOOBAttributes = ds.Tables(0).Rows(0).Item("NotSelectedOrdersOfBusiness")
    '            gSelectedOOBAttributes = ds.Tables(0).Rows(0).Item("SelectedOrderOfBusiness")
    '            gBillAttributes = ds.Tables(0).Rows(0).Item("Bill")
    '            gPhraseAttributes = ds.Tables(0).Rows(0).Item("Phrase")
    '            gSubjectAttributes = ds.Tables(0).Rows(0).Item("Subject")
    '        End If

    '    Catch ex As Exception
    '        DisplayMessage(ex.Message, "ReadDisplayBoardAttributes", "S")
    '    End Try
    'End Sub

    '    Public Function SendVotesToALIS() As Integer
    '        Dim k As Integer
    '        Dim VoteOID As Long, WrkFld As String, SenatorOID As Long, Vote As String
    '        Dim DistrictOID As Long
    '        Dim str, strAdd As String
    '        Dim ds, dsAdd, dsVoteOID As New DataSet
    '        Dim dr As DataRow

    '        Try
    '            '--- if it is test and not write to ALIS or ALIS database un-accessiable, quite send votes to ALIS
    '            If (gTestMode And Not gWriteVotesToTest) Then
    '                GoTo ProcExit
    '            End If

    '            '--- check able to connecte ALIS
    '            '--- Check_ALIS_Database_Accessible()
    '            If (Not AlisProda_On And Not AlisProdb_On) Then
    '                DisplayMessage("An error occurred while sending the votes to ALIS.  If the connection to ALIS is down, you should exit the system " & _
    '                "and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Vote Not Sent To ALIS", "S")
    '                gSendVotesToALIS = True
    '                GoTo ProcExit
    '            End If

    '            If gTestMode And gWriteVotesToTest Then                     '1 gTest votes
    '                str = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 1 And AlisVoteOID=0 AND VoteID =" & gVoteID
    '            Else                                                        '0 gProduction votes
    '                str = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And Test = 0 And AlisVoteOID=0 AND VoteID =" & gVoteID
    '            End If
    '            ds = V_DataSet(str, "R")

    '            For Each dr In ds.Tables(0).Rows
    '                '--- add vote header and retrieve the generated OID
    '                strAdd = "INSERT INTO Vote (oid, oid_legislative_Body, oid_Legislative_day, voteId, nays, yeas, abstains, pass) VALUES (vote_s.nextval" & ", " & gSenateID & ", " & _
    '                         dr("LegislativeDayOID") & ", " & dr("VoteID") & ", " & dr("TotalNay") & ", " & dr("TotalYea") & ", " & dr("TotalAbstain") & ", " & dr("TotalPass") & ")"
    '                'strAdd = "INSERT INTO Vote (oid, oid_legislative_Body, oid_Legislative_day, voteId, nays, yeas, abstains, pass) VALUES (vote_s.nextval " & ", " & gSenateID & ", " & _
    '                '       dr("LegislativeDayOID") & ", 13, " & dr("TotalNay") & ", " & dr("TotalYea") & ", " & dr("TotalAbstain") & ", " & dr("TotalPass") & ")"
    '                ALIS_DataSet(strAdd, "A")

    '                dsVoteOID = ALIS_DataSet("SELECT ALIS.VOTE_S.CURRVAL AS VOTEOID FROM DUAL", "R")
    '                VoteOID = dsVoteOID.Tables(0).Rows(0).Item("VoteOID")

    '                '--- now add senator votes
    '                '--- pick out vote, districtOID, and senatorOID from this string
    '                WrkFld = dr("SenatorVotes")



    '                k = 0
    '                Do
    '                    k = k + 1
    '                    WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1) ' skip senator name
    '                    Vote = Microsoft.VisualBasic.Left(WrkFld, 1)
    '                    WrkFld = Mid$(WrkFld, 3)
    '                    DistrictOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, "*") - 1))
    '                    WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1)
    '                    SenatorOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, ";") - 1))
    '                    ALIS_DataSet("INSERT INTO Individual_Vote(oid, oid_legislator, oid_vote, oid_organization, vote) VALUES (individual_vote_s.NEXTVAL, " & SenatorOID & "' " & VoteOID & "' " & DistrictOID & ", " & Vote & ")", "A")

    '                    If InStr(WrkFld, ";") = Len(WrkFld) Then
    '                        Exit Do
    '                    End If
    '                    WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '                Loop

    '                If gTestMode Then
    '                    V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & ", TEST = 1 WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")
    '                Else
    '                    V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & ", TEST = 0 WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")
    '                End If
    '            Next



    '            'For Each dr In ds.Tables(0).Rows
    '            '    '--- add vote header and retrieve the generated OID
    '            '    strAdd = "INSERT INTO Vote (oid, oid_legislative_Body, oid_Legislative_day, voteId, nays, yeas, abstains, pass) VALUES (vote_s.nextval" & ", " & gSenateID & ", " & _
    '            '             dr("LegislativeDayOID") & ", " & dr("VoteID") & ", " & dr("TotalNay") & ", " & dr("TotalYea") & ", " & dr("TotalAbstain") & ", " & dr("TotalPass") & ")"
    '            '    'strAdd = "INSERT INTO Vote (oid, oid_legislative_Body, oid_Legislative_day, voteId, nays, yeas, abstains, pass) VALUES (vote_s.nextval " & ", " & gSenateID & ", " & _
    '            '    '       dr("LegislativeDayOID") & ", 13, " & dr("TotalNay") & ", " & dr("TotalYea") & ", " & dr("TotalAbstain") & ", " & dr("TotalPass") & ")"
    '            '    ALIS_DataSet(strAdd, "A")

    '            '    dsVoteOID = ALIS_DataSet("SELECT ALIS.VOTE_S.CURRVAL AS VOTEOID FROM DUAL", "R")
    '            '    VoteOID = dsVoteOID.Tables(0).Rows(0).Item("VoteOID")

    '            '    '--- now add senator votes
    '            '    '--- pick out vote, districtOID, and senatorOID from this string
    '            '    WrkFld = dr("SenatorVotes")
    '            '    k = 0
    '            '    Do
    '            '        k = k + 1
    '            '        WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1) ' skip senator name
    '            '        Vote = Microsoft.VisualBasic.Left(WrkFld, 1)
    '            '        WrkFld = Mid$(WrkFld, 3)
    '            '        DistrictOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, "*") - 1))
    '            '        WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1)
    '            '        SenatorOID = Val(Mid$(WrkFld, 1, InStr(WrkFld, ";") - 1))
    '            '        ALIS_DataSet("INSERT INTO Individual_Vote(oid, oid_legislator, oid_vote, oid_organization, vote) VALUES (individual_vote_s.NEXTVAL, " & SenatorOID & "' " & VoteOID & "' " & DistrictOID & ", " & Vote & ")", "A")

    '            '        If InStr(WrkFld, ";") = Len(WrkFld) Then
    '            '            Exit Do
    '            '        End If
    '            '        WrkFld = Mid$(WrkFld, InStr(WrkFld, ";") + 1)
    '            '    Loop

    '            '    If gTestMode Then
    '            '        V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & ", TEST = 1 WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")
    '            '    Else
    '            '        V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & ", TEST = 0 WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")
    '            '    End If
    '            'Next
    '            gSendVotesToALIS = False

    'ProcExit:

    '        Catch ex As Exception
    '            'RetValue = DisplayMessage(ex.Message & vbCrLf & "An error occurred while sending the vote to ALIS.  If the connection to ALIS is down, you should exit the system " & _
    '            '"and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Execute: SendVotesToALIS()", "S")
    '            Exit Function
    '        End Try
    '    End Function

#End Region

End Module
