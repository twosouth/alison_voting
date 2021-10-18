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
Imports System.Array

Module basFunction

#Region "Gobal Variables"
    '*************************************************************************************************************
    ' Copyright @ 2014, Alabama State Senate Data Center

    ' Visual Studio 2010
    '*************************************************************************************************************

    'Public SVOTE As String = "ImagePC031"                   '   "LEGI1"      --- Dowe PC - it is a master pc
    'Public SDIS As String = "LEGPC39"                  '   "LEGPC39"    --- Sara PC
    'Public SOOB As String = "LEGPC39"                   '   "LEGP13"     --- Show HTML on the Wall Projection PC
    Public SVOTE As String = "SENATEVOTING"             '   "SENATEVOTING"     --- Dowe PC - it is a master pc
    Public SDIS As String = "SENATEDISPLAY"             '   "SENATEDISPLAY"   --- Sara PC
    Public SOOB As String = "SENATEOOB"                 '   "SENATEOOB"     --- Show HTML on the Wall Projection PC
    Public Const gPhraseInsertionPoint As String = "*"
    Public Const gSenatorInsertionPoint As String = "^"
    Public Const gCommitteeInsertionPoint As String = "~"
    Public Const gTextInsertionPoint As String = "?"
    Public Const gDesignNbrSenators = 35
    Public Const gBIR As String = "Budget Isolation Resolution."
    Public Const gSenateID As Short = 1753
    Public LocalDatabasePath As String = ""                        '---its value depand on select 'Production' or 'Test Mode' form frmAbout window "
    Public gCmdFile As String = "c:\VotingSystem3\RestartMsgQueue.cmd"
    Public gSessionID, gSVoteID, gIndex, gNbrSenators, gNbrVoteHistoryDays As Integer
    Public gCalendar, gBill, gApplication, gCalendarCode, gBillNbr As String
    Public gHTMLFile, gNbrSenator, gDataComputerName As String
    Public gDefaultPrinter, gPrimaryPrinter, gSecondaryPrinter, gMessageType, gMessageTitle As String
    Public gSenatorName(), gSenatorDistrictName(), gCommittees(), gCommitteeAbbrevs(), gThePhrases(), gPhraseCodes(), gPhrases(), gPhraseForVotePC() As String
    Public gComputerName, gSenator, gVoteID, gSubject As String
    Public gBillDisplayed, gCalendarDisplayed, gAction As String
    Public gCalendarCodeDisplayed, gBillNbrDisplayed, bodyParamters As String
    Public gOnlyOnePC, gFreeFormat, gTestMode, gWriteVotesToTest, gCOnly, gChamberHelp, gVotingStarted, gReSendALIS As Boolean
    Public gSenatorSplit, gErrorCount, gNbrCommittees, gNbrPhrases As Short
    Public gLegislativeDayTmp(), gCalendarDayTmp() As Object
    Public gPutBill, gPutCalendar, gPutCalendarCode, gPutBillNbr As String
    Public gPutChamberLight, gPutVotingLight, gPutPhrase, gWorkData, gCurrentPhrase, gMessage As New Object
    Public gSessionName, gSessionAbbrev, gLegislativeDay, gLastLegislativeDay, gCalendarDay As String
    Public SOOBAlive, WoodyAlive, gDeleteTestVotesOnStartUp, gVotingHelp, gPrintVoteRpt As Boolean
    Public gSenatorOID(), gDistrictOID(), gSessionIDTmp(), gLegislativeDayOIDTmp() As Integer
    Public gSalutation(), gSenatorNameOrder(), gSessionNameTmp(), gSessionAbbrevTmp(), gDownloadBills() As String
    Public gLegislativeDayOID, gLastVoteIDPerALIS, gLastVoteIDPerLocal As Integer
    Public loadSenatorsDon As Boolean = False


    Public frmMessage As New frmMessage
    Public connProda, connProdb, connTest As OleDbConnection
    Public strVoteTest As String                            '   connection string for ALIS_TEST oracle database
    Public strALIS As String                                '   connection string for ALIS Oracle Production database
    Public connLocal As OleDbConnection = Nothing           '   connection local C:\VotingSystem\VoteSys.mdb database
    Public cnOOB As OleDbConnection
    Public cnALIS As OleDbConnection
    Public v_Rows, a_Rows As Integer
    Public strSQL, gDatabasePath, strLocal, gVotingPath, gLogFile, gLastSessionName, gVotingSystemPath, gCurrentSessionName As String
    Public msgText As String = "Alabama Senate Voting System"
    Public gSendQueueFromVotePC, gSendQueueFromDisplay, gSendQueueToVotePC, gSendQueueToDisplay, gSendQueueToOOB, gReceiveQueueName, gRequestVoteIDQueue, gSendQueueTimer, gReceiveQueueTimer, tmpWrkFld, Alis_VoteID, gVoteIDTest, gLocalHTMLPage As String
    Public gProduction, LockReadOnly, vSet, LEGDay, MoreThanOneLEGDay As Boolean
    Public gDisplay_IPAddress, gVotePC_IPAddress, gOOBPC_IPAddress, gOOBPowerPointFilePath, gLastVoteIDForThisSessionTEST As String
    Public gOOB_Conn, gALIS_Primary_Conn, gALIS_Secondary_Conn, gALIS_TEST_Conn, gWriteToAlisVoteTable, gWriteToAlisIndividualVoteTable As String
    Public SVotePC_On, SDisplay_On, SOOB_On, AlisProda_On, AlisProdb_On, AlisTest_On, Network_On, gNetwork, gWorkingGroup, tableExist, gCreateHTMLPage, rightClick As Boolean
    Public tPage0, tPage1, tPage2, bSenator, bPhrase, dataProcess, DisplayImage, calendarClick, rightMouse As Boolean
    Public PhraseDisplayBoard, BillDispalyBoard, OOBDisplayBoard As Integer  '--New
    Public DragDrop1, DragDrop2, DragDrop3, askVoteID, isDownLoadBillorSOC, isDeleteSOC, straightSOC, enterSOC As Boolean
    Public setR1, setR2, setR3 As Object
    Public work_bill, work1_bill, work2_bill, work_area_text, work_area1_text, work_area2_text As String
    Public wrkOOB, wrkOOB1, wrkOOB2 As String
    Public wrkText, wrkText1, wrkText2 As String
    Public wrkCal, wrkCal1, wrkCal2, wrkCalCode, wrkCalCode1, wrkCalCode2 As String

    Public Owork_bill, Owork1_bill, Owork2_bill, Owork_area_text, Owork_area1_text, Owork_area2_text As String
    Public OwrkOOB, OwrkOOB1, OwrkOOB2 As String
    Public OwrkText, OwrkText1, OwrkText2 As String
    Public OwrkCal, OwrkCal1, OwrkCal2, OwrkCalCode, OwrkCalCode1, OwrkCalCode2 As String
    Public displaied_work, displaied_work1, displaied_work2 As Boolean

    '-- pass all of values to frmChamberDisplay form for clicking Phrase or Senator from frmPhrase or frmSenator form
    Public pWorkData1, pWorkData2, pWorkData3 As String
    Public K, index, gAllowedShortVoteCnt, v_id As Integer         '-- if there are more than one LEG day open, k will > 0
    Public RetValue As New Object

    Declare Function MoveWindow Lib "user32" (ByVal hwnd As Integer, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal bRepaint As Integer) As Integer
    Public Declare Function GetAsyncKeyState Lib "user32.dll" (ByVal vKey As Int32) As UShort
    Declare Function WriteProfileString Lib "kernel32" _
                    Alias "WriteProfileStringA" _
                    (ByVal lpszSection As String, _
                    ByVal lpszKeyName As String, _
                    ByVal lpszString As String) As Long

    Public frmChamberDisplay As New frmChamberDisplay
    Public frmVote As New frmVote

#End Region

    Public Sub Main()
        Dim dsCheckW As New DataSet
        Dim CheckDate As Date, x As Long, WrkFld As String

        gNetwork = True
        gWriteVotesToTest = False
        gOnlyOnePC = False
        gWorkingGroup = False

        Try
            If FileExistes(LocalDatabasePath) Then
                frmVote.setRTimer.Enabled = True
                frmVote.RequestVoteIDTimer.Enabled = True

                '--- change mouse pointer to waiting pointer
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                '--- get computer name to access local VoteSys.mdb database 
                gComputerName = Trim(System.Environment.MachineName)

                '--- get initial voting parameters from local VoteSys.mdb Access Database
                Get_Parameters()

                '--- initial Application
                If UCase(gComputerName) = UCase(SVOTE) Then
                    gApplication = "Voting"
                ElseIf UCase(gComputerName) = UCase(SDIS) Then
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

                If UCase(gComputerName) = UCase(SDIS) Then
                    If DisplayMessage("Would you want to clear the Special Order Calendar?", "Clear Special Order Calendar", "Y") Then
                        InitialSpecailOrderCalendar()
                    End If
                End If

                If gNetwork Then
                    '--- get Legislative day and Session infor
                    GetLegDay()

                    '--- get Last VoteID
                    Last_VoteID()
                Else
                    LEGDay = False
                End If

                '--- Check New Session? If it is true, copy local database and clear passed session vote records from tblRollCallVotes tabel
                CopyAndClear()

                '--- download Bills from ALIS, on Chamber Display site always do it, Voting side is optional
                dsCheckW = V_DataSet("Select ParameterValue From tblVotingParameters Where UCASE(Parameter) ='LASTLEGISLATIVEDAY'", "R")
                For Each drCW As DataRow In dsCheckW.Tables(0).Rows
                    If LEGDay Then
                        If UCase(System.Environment.MachineName) = UCase(SDIS) Then
                            If (drCW("ParameterValue") <> gLegislativeDay) Or (gSessionName <> gLastSessionName) Then
                                DownloadBillsFromALIS()
                            Else
                                If MoreThanOneLEGDay Then
                                    frmMultipleOpenLEGDays.Show()
                                End If
                            End If
                        ElseIf UCase(System.Environment.MachineName) = UCase(SVOTE) Then
                            DownloadBillsFromALIS()
                        End If
                    Else
                        gLegislativeDay = drCW("ParameterValue")
                        DownloadBillsFromALIS()
                    End If
                Next

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
                Application.Exit()
            End If
            dsCheckW.Dispose()
        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: Main()", "S")
            Exit Sub
        Finally
            dsCheckW.Dispose()
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Public Sub Last_VoteID()
        Dim dsV, dsA1 As New DataSet

        Try
            '--- initail PrintVoteHeader and PrintVoteDetail tables
            V_DataSet("Delete From tblPrintVoteDetail", "D")
            V_DataSet("Delete From tblPrintVoteHeader", "D")

            '--- now get see if there is a higher local vote id not yet sent to ALIS
            '--- if so, then update the last vote with it
            If UCase(gComputerName) = UCase(SVOTE) Then
                strSQL = "SELECT MAX(VoteID) AS LastVoteID FROM tblRollCallVotes WHERE SessionID = " & gSessionID
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
                        gVoteID = gLastVoteIDPerLocal
                        Exit For
                    Next
                End If
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

            If gLastVoteIDPerLocal > gLastVoteIDPerALIS Then
                gVoteID = gLastVoteIDPerLocal
            Else
                gVoteID = gLastVoteIDPerALIS
            End If

            '--- Now update the tblVoteParameters table
            V_DataSet("Update tblVotingParameters SET ParameterValue='" & gVoteID & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSION'", "U")

        Catch ex As Exception
            MsgBox(ex.Message & vbNewLine & "System will shutdown and restart again. If the error consists happened, please contact to Administrator.", MsgBoxStyle.Critical, "Access Database To Get Last VoteID")
            End
        Finally
            dsA1.Dispose()
            dsV.Dispose()
        End Try
    End Sub

    Function DisplayMessage(ByRef TheMessage As String, ByRef MesgTheTitle As String, ByRef MesgType As String) As Object
        Dim frmMessage As New frmMessage

        Try
            '--- if no message type is passed, then just display and continue
            '--- processing - this is just a status message
            ' ERHPushStack(("DisplayMessage - " & TheMessage))

            gMessage = TheMessage
            gMessageType = MesgType
            gMessageTitle = NToB(MesgTheTitle)
            frmMessage.Show()
            If gMessageType <> "" Then
                WaitUntilFormCloses("frmMessage")
            End If
            DisplayMessage = gMessage

            '  ERHPopStack()
            Exit Function
        Catch
            ERHHandler()
        End Try
    End Function

    Public Function FileExistes(ByVal FileName As String) As Boolean
        Try
            FileExistes = True
            FileOpen(89, FileName, OpenMode.Input)
            FileClose(89)
        Catch ex As Exception
            FileClose(89)
            FileExistes = False
            frmVote.setRTimer.Enabled = False
            frmVote.RequestVoteIDTimer.Enabled = False

            If InStr(FileName, ".mdb") = 0 Then
                MsgBox(ex.Message & FileName & " was not found! Please contact to Administrator.", vbCritical, "Check Local File")
            Else
                MsgBox("Local database " & FileName & " was not found! System will shut down. Please contact to Administrator.", vbCritical, "Check Local File")
                Application.Exit()         '---end Application 
            End If
        End Try
    End Function

    Public Sub Has_Network_Connectivity()
        Dim hostInfo As System.Net.IPHostEntry

        Try
            '--- to look for the logon server
            Dim sServer As String = Environment.GetEnvironmentVariable("logonserver")
            hostInfo = System.Net.Dns.GetHostEntry(sServer.Remove(0, 2))
            Network_On = True
        Catch
            '--- there is no network connection
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
        Dim strText As String = "Execute: Check_ALIS_Database_Accessible()"
     
        connProda = New OleDbConnection(gALIS_Secondary_Conn)
        connProdb = New OleDbConnection(gALIS_Primary_Conn)
        connTest = New OleDbConnection(gALIS_TEST_Conn)

        '--- check ALIS PRODUCTION DATABASE
        If gTestMode = False Then
            Try
                '--- Check ALIS_PRODB - alis primary
                connProdb.Open()
                If connProdb.State = ConnectionState.Open Then
                    AlisProdb_On = True
                    gNetwork = True
                End If
                connProdb.Close()
            Catch
                GoTo ALIS_PRODA
            Finally
                connProdb.Close()
            End Try

ALIS_PRODA:
            Try
                '--- Check ALIS_PRODA - alis secondary
                connProda.Open()
                If connProda.State = ConnectionState.Open Then
                    gNetwork = True
                    AlisProda_On = True
                End If
                connProda.Close()
            Catch
                AlisProda_On = False
                GoTo continue1
            Finally
                connProda.Close()
            End Try

continue1:
            If AlisProdb_On And AlisProda_On Then
                strALIS = gALIS_Primary_Conn
                cnALIS = New OleDbConnection(strALIS)
                RetValue = DisplayMessage("ALIS Oracle Databases is accessible", strText, "I")
            ElseIf AlisProdb_On And AlisProda_On = False Then
                strALIS = gALIS_Primary_Conn
                cnALIS = New OleDbConnection(strALIS)
                RetValue = DisplayMessage("Attention: Only able access the ALIS_PRODB database.", strText, "I")
            ElseIf AlisProdb_On = False And AlisProda_On Then
                strALIS = gALIS_Secondary_Conn
                cnALIS = New OleDbConnection(strALIS)
                RetValue = DisplayMessage("Attention: Only able access the ALIS_PRODA database.", strText, "I")
            ElseIf AlisProdb_On = False And AlisProda_On = False Then
                gNetwork = False
                If DisplayMessage("Attention: unable write voting data to ALIS production database. Would you want continue process by local 'Working Group'? Otherwise system will shut down.", "Unable Connecte to ALIS", "Y") Then
                    gWorkingGroup = True
                    cnALIS = Nothing
                Else
                    Application.Exit()
                    '!!! write log file here!                       
                End If
            End If
        Else
            Try
                strALIS = gALIS_TEST_Conn
                cnALIS = New OleDbConnection(strALIS)
                gNetwork = True
                AlisTest_On = True
            Catch ex As Exception
                AlisTest_On = False
                GoTo continue2
            Finally
                connTest.Close()
            End Try

continue2:
            If AlisTest_On Then
                If gTestMode And gWriteVotesToTest Then
                    strVoteTest = gALIS_TEST_Conn
                    cnALIS = New OleDbConnection(strVoteTest)
                ElseIf gTestMode And gWriteVotesToTest = False Then
                    DisplayMessage("Attention: You are in TEST MODE without write vote to ALIS_TEST.", "Execute: Check_ALIS_Database_Accessible()", "S")
                    cnALIS = Nothing
                ElseIf gTestMode = False And gWriteVotesToTest = True Then
                    V_DataSet("UPDATE tblVotingParameters Set ParameterValue ='N'  Where Parameter ='WRITEVOTESTOALISTEST'", "U")
                    cnALIS = Nothing
                End If
            Else
                If DisplayMessage("Attention: unable write TEST voting data to ALIS_TEST database. Would you want continue process by local 'Working Group'? Otherwise system will shut down.", "Unable Connecte to ALIS", "Y") Then
                    gWorkingGroup = True
                    cnALIS = Nothing
                    GoTo continue3
                Else
                    Application.Exit()
                    '!!! write log file here!    
                End If
            End If

continue3:
            '--- with ALIS, so set up dummy values; 
            gSessionID = -1
            gLegislativeDayOID = 1
            gLegislativeDay = 1
            gCalendarDay = Now.ToShortDateString
            gVoteID = 0
            V_DataSet("UPDATE tblVotingParameters Set ParameterValue = '0' Where Parameter ='LastVoteIDForThisSession'", "U")
        End If

        If AlisProda_On = False And AlisProdb_On = False And AlisTest_On = False Then
            gNetwork = False
        End If
    End Sub

    Public Sub Get_Parameters()
        Dim ds As New DataSet
        Dim dsP As New DataSet
        Dim parameters As New ArrayList()
        Dim RepValue As New Object
        Dim strFlag As String

        Try
            strSQL = ""
            strSQL = "Select * From tblVotingParameters Where ucase(Parameter) = "

            '--- Builte power on PC's access database connection string to get parameters first
            If gComputerName <> "" Then
                gDataComputerName = gComputerName

                '*** Dowe's Vote Master PC parameters
                gVotePC_IPAddress = QueryParameters(strSQL & "'VOTEPCIPADDRESS'").Tables(0).Rows(0).Item("ParameterValue")
                SVOTE = QueryParameters(strSQL & "'VOTEPCNAME'").Tables(0).Rows(0).Item("ParameterValue")

                '*** Sara's Display PC parameters
                gDisplay_IPAddress = QueryParameters(strSQL & "'DISPLAYPCIPADDRESS'").Tables(0).Rows(0).Item("ParameterValue")
                SDIS = QueryParameters(strSQL & "'DISPLAYPCNAME'").Tables(0).Rows(0).Item("ParameterValue")

                '*** OOB show on the wall PC parameters
                gOOBPC_IPAddress = QueryParameters(strSQL & "'OOBPCIPADDRESS'").Tables(0).Rows(0).Item("ParameterValue")
                gOOB_Conn = QueryParameters(strSQL & "'OOBPCCONNECTION'").Tables(0).Rows(0).Item("ParameterValue")
                SOOB = QueryParameters(strSQL & "'OOBPCNAME'").Tables(0).Rows(0).Item("ParameterValue")

                '*** ALIS Production database parameters
                gALIS_Primary_Conn = QueryParameters(strSQL & "'ALISPRIMARY'").Tables(0).Rows(0).Item("ParameterValue")
                gALIS_Secondary_Conn = QueryParameters(strSQL & "'ALISSECONDARY'").Tables(0).Rows(0).Item("ParameterValue")
                gALIS_TEST_Conn = QueryParameters(strSQL & "'ALISTEST'").Tables(0).Rows(0).Item("ParameterValue")

                gVotingPath = QueryParameters(strSQL & "'VOTINGSYSTEMPATH'").Tables(0).Rows(0).Item("ParameterValue")
                gDatabasePath = QueryParameters(strSQL & "'DATABASEPATH'").Tables(0).Rows(0).Item("ParameterValue")
                If UCase(gDatabasePath.Trim) <> UCase(LocalDatabasePath.Trim) Then
                    gDatabasePath = strLocal.Trim
                    V_DataSet("Update tblVotingParameters SET ParameterValue='" & LocalDatabasePath.Trim & "' Where ucase(Parameter)='DATABASEPATH'", "U")
                    DisplayMessage("Make sure local database path is: " & vbCrLf & LocalDatabasePath.Trim, msgText, "I")
                End If

                '*** send and receive Queue parameters
                gSendQueueFromVotePC = QueryParameters(strSQL & "'SENDQUEUEFROMVOTEPC'").Tables(0).Rows(0).Item("ParameterValue")
                gSendQueueFromDisplay = QueryParameters(strSQL & "'SENDQUEUEFROMDISPLAY'").Tables(0).Rows(0).Item("ParameterValue")
                gSendQueueToOOB = QueryParameters(strSQL & "'SENDQUEUETOOOB'").Tables(0).Rows(0).Item("ParameterValue")
                gSendQueueToVotePC = QueryParameters(strSQL & "'SENDQUEUETOVOTEPC'").Tables(0).Rows(0).Item("ParameterValue")
                gSendQueueToDisplay = QueryParameters(strSQL & "'SENDQUEUETODISPLAY'").Tables(0).Rows(0).Item("ParameterValue")
                gReceiveQueueName = QueryParameters(strSQL & "'RECEIVEQUEUENAME'").Tables(0).Rows(0).Item("ParameterValue")
                gRequestVoteIDQueue = QueryParameters(strSQL & "'REQUESTVOTEIDQUEUE'").Tables(0).Rows(0).Item("ParameterValue")
                gLocalHTMLPage = QueryParameters(strSQL & "'LOCALHTMLPAGE'").Tables(0).Rows(0).Item("ParameterValue")
                gReceiveQueueTimer = QueryParameters(strSQL & "'RECEIVEQUEUETIMER'").Tables(0).Rows(0).Item("ParameterValue")
                gNbrVoteHistoryDays = QueryParameters(strSQL & "'NBRVOTEHISTORYDAYS'").Tables(0).Rows(0).Item("ParameterValue")
                gLastLegislativeDay = QueryParameters(strSQL & "'LASTLEGISLATIVEDAY'").Tables(0).Rows(0).Item("ParameterValue")
                gNbrSenator = QueryParameters(strSQL & "'NUMBEROFSENATOR'").Tables(0).Rows(0).Item("ParameterValue").ToString
                gNbrSenators = CType(gNbrSenator, Integer)
                gHTMLFile = QueryParameters(strSQL & "'HTMLFILE'").Tables(0).Rows(0).Item("ParameterValue")
                gLogFile = QueryParameters(strSQL & "'LOGFILE'").Tables(0).Rows(0).Item("ParameterValue")
                gSendQueueTimer = QueryParameters(strSQL & "'SENDQUEUETIMER'").Tables(0).Rows(0).Item("ParameterValue")
                gWriteToAlisVoteTable = QueryParameters(strSQL & "'WRITETOALISVOTETABLEFILE'").Tables(0).Rows(0).Item("ParameterValue")
                gWriteToAlisIndividualVoteTable = QueryParameters(strSQL & "'WRITETOALISINDIVIDUALVOTETABLE'").Tables(0).Rows(0).Item("ParameterValue")
                gAllowedShortVoteCnt = QueryParameters(strSQL & "'ALLOWEDSHORTVOTECNT'").Tables(0).Rows(0).Item("ParameterValue")
                gSecondaryPrinter = QueryParameters(strSQL & "'SECONDARYPRINTER'").Tables(0).Rows(0).Item("ParameterValue")
                gCmdFile = QueryParameters(strSQL & "'COMMANDFILE'").Tables(0).Rows(0).Item("ParameterValue")
                gLastSessionName = QueryParameters(strSQL & "'LASTSESSIONNAME'").Tables(0).Rows(0).Item("ParameterValue")
                gSVoteID = QueryParameters(strSQL & "'LASTVOTEIDFORTHISSESSION'").Tables(0).Rows(0).Item("ParameterValue")
                gVotingSystemPath = QueryParameters(strSQL & "'VOTINGSYSTEMPATH'").Tables(0).Rows(0).Item("ParameterValue")
                gPrimaryPrinter = QueryParameters(strSQL & "'PRIMARYPRINTER'").Tables(0).Rows(0).Item("ParameterValue")

                strFlag = UCase(QueryParameters(strSQL & "'CREATEHTMLPAGE'").Tables(0).Rows(0).Item("ParameterValue"))
                If strFlag = "Y" Then
                    gCreateHTMLPage = True
                Else
                    gCreateHTMLPage = False
                End If
                strFlag = UCase(QueryParameters(strSQL & "'PRINTVOTEREPORT'").Tables(0).Rows(0).Item("ParameterValue"))
                If strFlag = "Y" Then
                    gPrintVoteRpt = True
                Else
                    gPrintVoteRpt = False
                End If
                strFlag = UCase(QueryParameters(strSQL & "'WRITEVOTESTOALISTEST'").Tables(0).Rows(0).Item("ParameterValue"))
                If strFlag = "Y" Then
                    gWriteVotesToTest = True
                Else
                    gWriteVotesToTest = False
                End If
                strFlag = UCase(QueryParameters(strSQL & "'DELETETESTVOTESONSTART'").Tables(0).Rows(0).Item("ParameterValue"))
                If strFlag = "Y" Then
                    gDeleteTestVotesOnStartUp = True
                Else
                    gDeleteTestVotesOnStartUp = False
                End If
                strFlag = UCase(QueryParameters(strSQL & "'CHAMBERHELP'").Tables(0).Rows(0).Item("ParameterValue"))
                If strFlag = "Y" Then
                    gChamberHelp = True
                Else
                    gChamberHelp = False
                End If
                strFlag = UCase(QueryParameters(strSQL & "'VOTINGHELP'").Tables(0).Rows(0).Item("ParameterValue"))
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
                        DisplayMessage("Senate Order of Business Computer Is On.", mes, "I")
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
                If Not DisplayMessage("Senate Chamber Display Computer Is Off. " & vbNewLine & "Would You Want Continue Without Chamber Display?", "Chamber Dispaly PC is Offline", "Y") Then
                    End
                End If
                gOnlyOnePC = True
            ElseIf SVotePC_On = False And SDisplay_On Then
                If Not DisplayMessage("Senate Voting Computer Is Off." & vbCrLf & "Would You Want Continue Without Voting?", "Vote PC is Offline", "Y") Then
                    End
                End If
                gOnlyOnePC = True
            End If
        Catch ex As Exception
            DisplayMessage(ex.Message, "Check_PCs_Status()", "S")
            Exit Sub
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
            ds.Dispose()

            Exit Sub
        Catch ex As Exception
            ds.Dispose()
            DisplayMessage(ex.Message, "Execute: LoadCommitteesIntoArray()", "I")
            Exit Sub
        End Try
    End Sub

    Public Sub LoadPhrasesIntoArray()
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim i As Short
        Dim gPrhaseCodes(0) As Object
        ReDim gPhrases(0)           '--- init to blanks

        Try
            strSQL = "Select Cstr(Code)  + ' - ' + Phrase  AS ThePhrase, Code, Phrase From tblPhrases Order By Code"
            ds = V_DataSet(strSQL, "R")

            ReDim Preserve gPhrases(ds.Tables(0).Rows.Count)
            ReDim Preserve gPhraseForVotePC(ds.Tables(0).Rows.Count)
            ReDim Preserve gThePhrases(ds.Tables(0).Rows.Count)
            ReDim Preserve gPhraseCodes(ds.Tables(0).Rows.Count)

            i = 0
            For Each dr In ds.Tables(0).Rows
                i = i + 1
                gPhrases(i) = dr("Phrase")
                gThePhrases(i) = dr("ThePhrase")
                gPhraseCodes(i) = dr("Code")
                '--- for Voting PC only
                gPhraseForVotePC(i) = dr("Phrase")
            Next
            gNbrPhrases = i
            ds.Dispose()
        Catch ex As Exception
            ds.Dispose()
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
            MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical, "Check Window Status")
            Exit Function
        End Try
    End Function

    Sub WaitUntilFormCloses(ByRef FormName As String)
        Do Until Not IsLoaded(FormName)
            System.Windows.Forms.Application.DoEvents()
        Loop
    End Sub

    Public Function DownloadBillsFromALIS() As Integer
        Dim drV, drA As DataRow
        Dim ds, dsV, dsA, dsA1 As New DataSet
        Dim FieldValue() As Object
        Dim Title, WrkFld As String
        Dim da As New OleDbDataAdapter
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim billCount, lBillCount, CBillCount As Integer

        Try
            Try
                DownloadBillsFromALIS = False

                '--- if no ALIS connection, the attempt it again
                If (Not AlisProda_On And Not AlisProdb_On And gProduction) Or (Not AlisTest_On And gTestMode And gWriteVotesToTest) Then
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
                    frmMultipleOpenLEGDays.Show()
                    WaitUntilFormCloses("frmMultipleOpenLegDays")
                End If
                dsA.Dispose()

                '--- if different legislatuve day, then update recall votes for Voting PC to 0
                If Val(gLegislativeDay) <> gLastLegislativeDay Then
                    strSQL = "Update tblVotingDisplayParameters SET RecallVote1 = 0,RecallVote2 = 0,RecallVote3 = 0"
                    V_DataSet(strSQL, "U")
                End If

                '---- if some of error occurred, continue next process
            Catch ex As Exception
                GoTo ProcNext1
            End Try

ProcNext1:
            '--- save any bills that have something in the work area to a temporary table and
            '--- then restore the work areas if the same bills are downloaded again
            Try
                V_DataSet("Delete From tblWork", "D")

                Dim dsWorkData As New DataSet
                dsWorkData = V_DataSet("Select CalendarCode, BillNbr, WorkData FROM tblBills WHERE WorkData <> ''", "R")
                If dsWorkData.Tables(0).Rows.Count <> 0 Then
                    For Each dr As DataRow In dsWorkData.Tables(0).Rows
                        Dim dsW As New DataSet
                        strSQL = "Insert into tblWork Values ('" & dr("CalendarCode") & "', '" & dr("BillNbr") & "', '" & Replace(dr("WorkData"), "'", "''") & "', " & gLegislativeDay & ")"
                        V_DataSet(strSQL, "A")
                    Next
                End If

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
                V_DataSet("Delete From tblBuiltVoteID", "D")

                ReDim FieldValue(9)

                '### 1 --- get Regular Order Bills
                Try
                    '---delete Regular Bills first
                    V_DataSet("Delete From tblBills Where CalendarCode ='1'", "D")

                    strSQL = " SELECT label, sponsor, alis_object.index_word, calendar_page " & _
                            " FROM ALIS_OBJECT, MATTER " & _
                            " WHERE matter.oid_instrument = alis_object.oid AND matter.oid_session = " & gSessionID & " AND alis_object.oid_session = " & gSessionID & _
                            " AND matter.matter_status_code = 'Pend' AND matter.oid_legislative_body = '1753' AND alis_object.legislative_body = 'S' " & _
                            " AND matter.calendar_sequence_no > 0 ORDER BY matter.calendar_sequence_no"
                    dsA = ALIS_DataSet(strSQL, "R")
                    billCount = dsA.Tables(0).Rows.Count
                    If billCount > 0 Then
                        ReDim Preserve gDownloadBills(billCount - 1)
                        For Each drA In dsA.Tables(0).Rows
                            x += 1
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
                                FieldValue(8) = ""
                                FieldValue(7) = FieldValue(2)
                            End If
                            FieldValue(9) = x
                            V_DataSet("INSERT INTO tblBills VALUES ('1', '" & FieldValue(1) & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "', '" & Replace(FieldValue(7), "'", "") & "', '" & Replace(FieldValue(8), "'", "") & "', " & FieldValue(9) & ")", "A")
                            gDownloadBills(y) = FieldValue(2)
                            y = y + 1
                        Next
                    End If
                Catch
                    Try
                        DownloadBillsFromALIS()
                    Catch ex As Exception
                        DisplayMessage("Failed Download Bills From ALIS Again! System will be shutdown. Please Contact to Administrator.", "Down Load Bill From ALIS", "S")
                        End
                    End Try
                End Try


                '### 2 --- Get Local Bills  
                Try
                    '--- delete Local bills first
                    V_DataSet("Delete From tblBills Where CalendarCode ='2'", "D")

                    strSQL = "SELECT label, calendar_page, senate_display_title, sponsor " & _
                            " FROM alis_object, matter WHERE matter.oid_instrument = alis_object.oid " & _
                            " AND matter.oid_session = " & gSessionID & " AND alis_object.oid_session = " & gSessionID & " AND matter.matter_status_code = 'Pend' " & _
                            " AND matter.oid_legislative_body = '1753' AND alis_object.legislative_body = 'S' AND alis_object.local_bill = 'T' " & _
                            " AND matter.calendar_sequence_no > 0 ORDER BY matter.calendar_sequence_no "
                    dsA = ALIS_DataSet(strSQL, "R")

                    lBillCount = dsA.Tables(0).Rows.Count
                    If lBillCount > 0 Then
                        ReDim Preserve gDownloadBills(billCount + lBillCount - 1)
                        For Each drA In dsA.Tables(0).Rows
                            x += 1
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
                            FieldValue(9) = x
                            V_DataSet("INSERT INTO tblBills VALUES ('2', '" & FieldValue(1) & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "', '" & Replace(FieldValue(7), "'", "") & "', '" & Replace(FieldValue(8), "'", "") & "', " & FieldValue(9) & ")", "A")
                            gDownloadBills(y) = FieldValue(2)
                            y = y + 1
                        Next
                    End If
                Catch
                    Try
                        DownloadBillsFromALIS()
                    Catch ex As Exception
                        DisplayMessage("Failed Download Local Bills From ALIS Again! System will be shutdown. Please Contact to Administrator.", "Down Load Bill From ALIS", "S")
                        End
                    End Try
                End Try


                '###3 ---Get Confirmations
                Try
                    '--- delete confirmatins
                    V_DataSet("Delete From tblBills Where CalendarCode ='3'", "D")

                    strSQL = "SELECT I.OID, I.LABEL, I.TYPE_CODE,   PE.FIRSTNAME, PE.LASTNAME, PE.SUFFIX, O.NAME, CO.ABBREVIATION, TO_CHAR(R1DAY.CALENDAR_DATE, 'MM/DD/YYYY') AS R1DATE, TO_CHAR(MDAY.CALENDAR_DATE, 'MM/DD/YYYY') AS MDATE, " & _
                            " M.TRANSACTION_TYPE_CODE AS MTRAN, M.MATTER_STATUS_CODE AS MSTAT, M.RECOMMENDATION_CODE AS MREC, AU.NAME AS AUTHORITY, CV.Value AS INSTR_VALUE " & _
                        " FROM ALIS_OBJECT I, MATTER C1, MATTER M, ORGANIZATION CO, ORGANIZATION O, POSITION PO, POSITION AU, PERSON PE, LEGISLATIVE_DAY R1DAY, LEGISLATIVE_DAY MDAY, CODE_VALUES CV " & _
                        " WHERE I.INSTRUMENT = 'T' AND I.TYPE_CODE = 'CF' AND I.STATUS_CODE = CV.CODE (+) AND CV.CODE_TYPE = 'InstrumentStatus' AND I.OID_SESSION = " & gSessionID & " AND I.OID_POSITION = PO.OID (+) " & _
                            " AND O.OID (+) = PO.OID_ORGANIZATION AND AU.OID (+) = PO.OID_APPOINTING_AUTHORITY AND I.OID = C1.OID_INSTRUMENT AND M.OID_COMMITTEE = CO.OID (+) AND C1.OID_CANDIDACY = PE.OID (+) " & _
                            " AND C1.OID_CONSIDERED_DAY = R1DAY.OID (+) AND M.OID_CONSIDERED_DAY = MDAY.OID (+) AND M.OID_INSTRUMENT = I.OID AND M.SEQUENCE = I.LAST_MATTER AND I.OID_SESSION = C1.OID_SESSION " & _
                            " AND C1.SEQUENCE = (SELECT MIN(X.SEQUENCE) FROM MATTER X WHERE X.OID_SESSION = C1.OID_SESSION AND X.OID_INSTRUMENT = C1.OID_INSTRUMENT AND X.TRANSACTION_TYPE_CODE = 'R1') ORDER BY SUBSTR(I.LABEL, 1, 1), I.TYPE_CODE, I.ID"
                    dsA = ALIS_DataSet(strSQL, "R")
                    CBillCount = dsA.Tables(0).Rows.Count
                    If CBillCount > 0 Then
                        ReDim Preserve gDownloadBills(billCount + lBillCount + CBillCount - 1)
                        For Each drA In dsA.Tables(0).Rows
                            x += 1
                            FieldValue(0) = "3"
                            FieldValue(1) = NToB(Mid(drA("Label"), 2))
                            FieldValue(3) = NToB(drA("FirstName")) & " " & NToB(drA("LastName"))
                            FieldValue(4) = ""

                            '--- if boards and commissions exist, then search for the senate voting name and replace if found            
                            If NToB(drA("Name")) = "" Then
                                FieldValue(2) = FieldValue(1) & " - " & FieldValue(3) & " -- "
                            Else
                                WrkFld = NToB(drA("Name"))
                                strSQL = "SELECT SenateVotingName FROM tblBoardsAndCommissions WHERE AlisName = '" & Replace(WrkFld, "'", "''") & "'"
                                dsV = V_DataSet(strSQL, "R")
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
                            FieldValue(9) = x
                            V_DataSet("INSERT INTO tblBills VALUES ('3', '" & FieldValue(1) & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "', '" & Replace(FieldValue(7), "'", "") & "', '" & Replace(FieldValue(8), "'", "") & "', " & FieldValue(9) & ")", "A")
                            gDownloadBills(y) = FieldValue(2)
                            y = y + 1
                        Next
                    End If
                Catch
                    Try
                        DownloadBillsFromALIS()
                    Catch ex As Exception
                        DisplayMessage("Failed Download Confirmatins From ALIS Again! System will be shutdown. Please Contact to Administrator.", "Down Confirmations From ALIS", "S")
                        End
                    End Try
                End Try


                '--- restore any saved work areas to the bills downloaded
                dsV = V_DataSet("Select * From tblWork", "R")
                For Each drV In dsV.Tables(0).Rows
                    V_DataSet("UPDATE tblBills SET WorkData ='" & Replace(drV("WorkData"), "'", "''") & "' WHERE CalendarCode ='" & NToB(drV("CalendarCode")) & "' AND ucase(BillNbr) ='" & UCase(NToB(drV("BillNbr"))) & "'", "U")
                Next
                '--- Delete tblWork 
                V_DataSet("Delete From tblWork", "D")

                RetValue = DisplayMessage("Download of " & IIf(gWriteVotesToTest, "TEST ", "") & " Bills From ALIS Completed.", "Execute: DownloadBillsFromALIS()", "I")

                DownloadBillsFromALIS = True
            Catch ex As Exception
                DownloadBillsFromALIS = False
                GoTo ProcExit
            End Try

ProcExit:

        Catch ex As Exception
            DisplayMessage(ex.Message & vbNewLine & "Download Bills Form ALIS was Failed! System Will Shutdown. Please Contact to Administrator.", "Execute: DownloadBillsFromALIS()", "S")
            End
        Finally
            ds.Dispose()
            dsA.Dispose()
            dsA1.Dispose()
            dsV.Dispose()
        End Try
    End Function

    Public Sub LoadSenatorsIntoArray()
        Dim ds, dsD As New DataSet
        Dim dr As DataRow
        Dim i As Short
        Dim k As Integer = 0
        Dim x As Integer = 0
        Dim MemberLname As String = ""

        '--- load the senators into an array
        Try
            i = 0

            ds = V_DataSet("Select * From tblSenators Order By SenatorName", "R")
            gNbrSenators = ds.Tables(0).Rows.Count
            ReDim Preserve gSenatorName(gNbrSenators)
            ReDim Preserve gSenatorOID(gNbrSenators)
            ReDim Preserve gDistrictOID(gNbrSenators)
            ReDim Preserve gSenatorDistrictName(gNbrSenators)

            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr In ds.Tables(0).Rows
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
                If loadSenatorsDon = False Then
                    RetValue = DisplayMessage("Reading Senators Records From Local Database Finished.", "Execute: LoadSenatorsIntoArray()", "I")
                End If

            Else
                RetValue = DisplayMessage("Attention: There are not Senators download from ALIS yet!", "Execute: LoadSenatorsIntoArray()", "I")
                If gNetwork Then
                    DownloadSenatorsFromALIS()
                Else
                    DisplayMessage("Conntect to ALIS failed!", "Execute: LoadSenatorsIntoArray()", "I")
                End If
            End If
            ds.Dispose()

            dsD = V_DataSet("Select * From tblSenators Order By SenatorNbr", "R")
            For Each drD As DataRow In dsD.Tables(0).Rows
                gSenatorDistrictName(k + 1) = drD("SenatorName")
                k += 1
            Next
            dsD.Dispose()
            Exit Sub
        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: LoadSenatorsIntoArray()", "I")
            Exit Sub
        Finally
            ds.Dispose()
            dsD.Dispose()
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
            da.Dispose()
            ds.Dispose()
            connLocal.Close()
        End Try
    End Function

    Public Sub InitialSpecailOrderCalendar()
        Try
            V_DataSet("Delete From tblSpecialOrderCalendar ", "D")
            V_DataSet("Delete From tblCalendars Where LEFT(CalendarCode, 2)='SR'", "D")
            V_DataSet("Delete From tblCalendars Where CalendarCode='SOC'", "D")

        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: IintalSpecailOrderCalendar()", "S")
            Exit Sub
        End Try
    End Sub

    Public Function GetBills(ByVal gCalendar As String) As DataSet
        Dim origCalendar As String = gCalendar
        Dim sr As String = UCase(Strings.Left(gCalendar, 2))

        Try
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
            GetBills = Nothing
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
        Dim dsp, dsR As New DataSet
        Dim voteidP, voteidR As Integer

        Try
            strSQL = "Select ParameterValue From tblVotingParameters Where UCase(Parameter)='LASTVOTEIDFORTHISSESSION'"
            dsp = V_DataSet(strSQL, "R")

            strSQL = "Select max(VoteID) as voteID From tblRollCallVotes Where SessionID =" & gSessionID
            dsR = V_DataSet(strSQL, "R")

            If dsp.Tables(0).Rows.Count <> 0 Then
                For Each drP As DataRow In dsp.Tables(0).Rows
                    If Not IsDBNull(drP("ParameterValue")) Then
                        voteidP = drP("ParameterValue")
                    Else
                        voteidP = 0
                    End If
                Next
            Else
                voteidP = 0
            End If

            If dsR.Tables(0).Rows.Count <> 0 Then
                For Each drR As DataRow In dsR.Tables(0).Rows
                    If Not IsDBNull(drR("VoteID")) Then
                        voteidR = drR("VoteID")
                    Else
                        voteidR = 0
                    End If
                Next
            Else
                voteidR = 0
            End If

            If voteidP = voteidR Then
                gVoteID = voteidR
            ElseIf voteidP > voteidR Then
                gVoteID = voteidP
            ElseIf voteidP < voteidR Then
                gVoteID = voteidP
            End If
            UpdateLastVoteIDAndLastLegDay(gVoteID)

            GetLastLocalVoteID = gVoteID
        Catch ex As Exception
            DisplayMessage(ex.Message, "Execute: GetLastLocalVoteID()", "S")
            Exit Function
        Finally
            dsp.Dispose()
            dsR.Dispose()
        End Try
    End Function

    Public Sub UpdateLastVoteIDAndLastLegDay(ByVal LastVoteID As String)
        Try
            strSQL = "Update tblVotingParameters Set ParameterValue='" & LastVoteID & "' Where ucase(Parameter) ='LASTVOTEIDFORTHISSESSION'"
            V_DataSet(strSQL, "U")
          
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
                MoreThanOneLEGDay = False
                Application.Exit()
            ElseIf openDay > 1 Then
                MoreThanOneLEGDay = True
            ElseIf openDay = 1 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    gSessionID = dr("OID_SESSION")
                    gLegislativeDay = dr("LEGISLATIVE_DAY")
                    gLegislativeDayOID = dr("OID")
                    gCalendarDay = dr("CAL_DAY")
                    gSessionName = dr("LABEL")
                    gSessionAbbrev = dr("ABBREVIATION")
                    gCurrentSessionName = dr("LABEL")
                    MoreThanOneLEGDay = False
                Next
                V_DataSet("Update tblVotingParameters Set ParameterValue='" & gSessionName & "' WHERE UCASE(Parameter) = 'LASTSESSIONNAME'", "U")
                V_DataSet("Update tblVotingParameters Set ParameterValue='" & gSessionID & "' WHERE UCASE(Parameter) = 'LASTSESSIONID'", "U")
            End If
            LEGDay = True
            ds.Dispose()
        Catch ex As Exception
            ds.Dispose()
            LEGDay = False
            Exit Sub
        End Try
    End Sub

    Public Function SendVotesToALIS() As Integer
        Dim k As Integer
        Dim indVoteOID, VoteOID, SenatorOID, DistrictOID As Long
        Dim str, strAdd, WrkFld, Vote As String
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
            If (gTestMode And gWriteVotesToTest = False) Then
                GoTo ProcExit
            End If

            '--- check able to connecte ALIS
            '--- Check_ALIS_Database_Accessible()
            If (Not AlisProda_On And Not AlisProdb_On And gProduction) Then
                DisplayMessage("An error occurred while sending the votes to ALIS.  If the connection to ALIS is down, you should exit the system " & _
                "and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Vote Not Sent To ALIS", "S")
                GoTo ProcExit
            End If

            If gTestMode And gWriteVotesToTest Then
                If gReSendALIS = False Then
                    strSQL = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And AlisVoteOID=0 AND VoteID =" & gVoteID
                Else
                    strSQL = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & "  And AlisVoteOID=0 "
                End If
            Else
                If gReSendALIS = False Then
                    strSQL = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And  AlisVoteOID=0 AND VoteID =" & gVoteID
                Else
                    str = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And  AlisVoteOID=0 "
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
                    WrkFld = Mid$(WrkFld, InStr(WrkFld, "*") + 1)   '--- skip senator name
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
                V_DataSet("UPDATE tblRollCallVotes SET AlisVoteOID=" & VoteOID & "  WHERE SessionID =" & gSessionID & " AND VoteID =" & dr("VoteID"), "U")
            Next

            If gReSendALIS Then
                DisplayMessage("Process all of UNSEND votes to ALIS finished.", "Send Votes To ALIS", "S")
                Exit Function
            End If

ProcExit:

        Catch ex As Exception
            RetValue = DisplayMessage(ex.Message & vbNewLine & "An error occurred while Re-Send the votes to ALIS.  If the connection to ALIS is down, you should exit the system " & _
            "and then restart it again.  If the connection is OK, this vote will be sent to ALIS when the next one is sent.", "Execute: SendUnSendVotesToALIS()", "S")
            Exit Function
        Finally
            ds.Dispose()
            dsAdd.Dispose()
            dsDetail.Dispose()
            dsIndVoteOID.Dispose()
            dsVoteOID.Dispose()
        End Try
    End Function

    Public Sub DownloadSenatorsFromALIS()
        Dim i As Short
        Dim dsSenator, dsDistrict As New DataSet
        Dim drSenator As DataRow
        Dim dt As New DataTable
        dt.Clear()
        dt.Columns.Clear()
        dt.Columns.Add("SenatorName")
        dt.Columns.Add("SenatorOID")
        dt.Columns.Add("DistrictOID")

        If gNetwork Then
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
                strSQL = " SELECT A.NAME AS SenatorName, A.OID as SenatorOID, O.OID as DistrictOID" & _
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
            Finally
                dsDistrict.Dispose()
                dsSenator.Dispose()
            End Try
        Else
            DisplayMessage("Connect to ALIS failed! Unable download Senators. System Will Shutdown. Please Contact to Administrator.", "Download Senators From ALIS", "S")
            Application.Exit()
        End If
    End Sub

    Public Sub CopyAndClear()
        Try
            '--- if new session opened, backup previously session local database and vote.txt file
            If UCase(gDataComputerName) = UCase(SVOTE) Then
                If gLastSessionName <> gSessionName Then
                    If gTestMode = False Then
                        If File.Exists(gVotingSystemPath & "SenateVotes.mdb") Then
                            If Not File.Exists(gVotingSystemPath & "Archives\" & "SenateVotes-" & gSessionAbbrev & ".mdb") Then
                                File.Copy(gDatabasePath, gVotingSystemPath & "Archives\" & "SenateVotes-" & gSessionAbbrev & ".mdb")
                            End If
                        End If
                    Else
                        If File.Exists(gVotingSystemPath & "SenateVotesTest.mdb") Then
                            If Not File.Exists(gVotingSystemPath & "Archives\" & "SenateVotesTest-" & gSessionAbbrev & ".mdb") Then
                                File.Copy(gDatabasePath, gVotingSystemPath & "Archives\" & "SenateVotesTest-" & gSessionAbbrev & ".mdb")
                            End If
                        End If
                    End If

                    If gTestMode = False Then
                        If File.Exists(gVotingSystemPath & "Vote.txt") Then
                            If Not File.Exists(gVotingSystemPath & "Archives\" & "Vote-" & gSessionAbbrev & ".txt") Then
                                File.Copy(gVotingSystemPath & "Vote.txt", gVotingSystemPath & "Archives\" & "Vote-" & gSessionAbbrev & ".txt", True)
                            End If

                            '--- after backupped, delete and create new blank txt file
                            File.Delete(gVotingSystemPath & "Vote.txt")
                            File.Create(gVotingSystemPath & "Vote.txt")
                        End If
                    Else
                        If File.Exists(gVotingSystemPath & "VoteTest.txt") Then
                            If Not File.Exists(gVotingSystemPath & "Archives\" & "VoteTest-" & gSessionAbbrev & ".txt") Then
                                File.Copy(gVotingSystemPath & "VoteTest.txt", gVotingSystemPath & "Archives\" & "VoteTest-" & gSessionAbbrev & ".txt", True)
                            End If

                            '--- after backupped, delete and create new blank txt file
                            File.Delete(gVotingSystemPath & "VoteTest.txt")
                            File.Create(gVotingSystemPath & "VoteTest.txt")
                        End If
                    End If

                    '--- delete previously session vote records from tblRollCallVotes table
                    V_DataSet("Delete From tblRollCallVotes Where SessionID <> " & gSessionID, "D")
                End If
            ElseIf UCase(gDataComputerName) = UCase(SDIS) Then
                If gLastSessionName <> gSessionName Then
                    If gTestMode = False Then
                        If File.Exists(gVotingSystemPath & "SenateChamberDisplay.mdb") Then
                            If Not File.Exists(gVotingSystemPath & "Archives\" & "SenateChamberDisplay-" & gSessionAbbrev & ".mdb") Then
                                File.Copy(gDatabasePath, gVotingSystemPath & "Archives\" & "SenateChamberDisplay-" & gSessionAbbrev & ".mdb")
                            End If
                        End If
                    Else
                        If File.Exists(gVotingSystemPath & "SenateChamberDisplayTest.mdb") Then
                            If Not File.Exists(gVotingSystemPath & "Archives\" & "SenateChamberDisplayTest-" & gSessionAbbrev & ".mdb") Then
                                File.Copy(gDatabasePath, gVotingSystemPath & "Archives\" & "SenateChamberDisplayTest-" & gSessionAbbrev & ".mdb")
                            End If
                        End If
                    End If

                End If
            End If
        Catch ex As Exception
            DisplayMessage("Copy previously local database failed! Please contact to Administrator.", "Copy file", "S")
            Exit Sub
        End Try
    End Sub
End Module
