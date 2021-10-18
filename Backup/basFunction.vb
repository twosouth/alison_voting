Option Strict Off
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
    ' Copyright @ 2000, Alabama State Senate with development assistance
    ' from International Roll-Call, Inc.  All rights reserved.

    ' Version 12.15.2000.1
    '*************************************************************************************************************

    ' comment out these next 2 lines and enable the following 2 to gTest
    ' on  a single PC; must also jump around DownloadFromALIS routine, change
    ' VotingSystem connection in the designer, and put an exit sub at the beginning
    ' of the CreateHTML sub in frmChamberDisplay; must also put in a statement in
    ' Main gSessionID=1015 and gLegislativeDayOID=111

    ' Public Const DataPath As String = "\\*\c$\VotingSystem\"
    'Public Const DatabasePath As String = "\\*\c$\VotingSystem\VoteSys.mdb"
    'Public Const HTMLFile As String = "\\Neverland\soawebroot\soa\SearchableInstruments\*\SenateOOB\CurrentMatter.htm"
    Public Const HTMLFile As String = "\\Neverland\soawebroot\soagTest\SearchableInstruments\*\SenateOOB\CurrentMatter.htm"
    'Public Const gDataPath = "C:\VotingSystem\", gDatabasePath = "C:\VotingSystem\VoteSys.mdb"
    'Global Const HTMLFile = "C:\VotingSystem\SenatePage.htm"

    Public Const gPhraseInsertionPoint As String = "*"
    Public Const gSenatorInsertionPoint As String = "^"
    Public Const gCommitteeInsertionPoint As String = "~"
    Public Const gTextInsertionPoint As String = "?"
    Public Const gBIR As String = "Budget Isolation Resolution"
    Public Const gSenateID As Short = 1753
    Public Const SD1 As String = "SENDISPLAY1"
    Public Const SV1 As String = "SENVOTE1"
    Public Const SD2 As String = "SENDISPLAY2"
    Public Const SV2 As String = "SENVOTE2"

    Public gCalendar As String
    Public gChamberTimer As Integer
    Public frm, Pfrm As New System.Windows.Forms.Form
    Public Sfrm, Wfrm As New System.Windows.Forms.Form
    Public gSessionID As Integer
    Public gBill, gApplication As String
    Public gSaveBillHeight As Integer
    Public gPutParams As Boolean
    Public gCalendarCode, gBillNbr As String
    Public gNbrPhrasesToDisplay As Short
    Public gSwitchBox As String
    Public gOK As Short
    Public gSenatorName() As String
    Public gCurrentPhrase, gMessage As New Object
    Public gVersionNbr As String
    Public gPhrases() As String
    Public gThePhrases() As String
    Public gCommittees() As String
    Public gCommitteeAbbrevs() As String
    Public gOOBHeadingAttributes, gNotSelectedOOBAttributes As String
    Public gSelectedOOBAttributes, gBillAttributes As String
    Public gIndex As Integer
    Public gPhraseAttributes, gSubjectAttributes As String
    Public gOOBLength, gStart, gLength, gSVoteID As Integer
    Public gSenator, gVoteID, gSubject As String
    Public gPhraseCodes() As Short
    Public gVotingStarted As Short
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
    Public gTestMode As Boolean
    Public gErrorCount As Short
    Public gPutBill, gPutCalendar, gPutCalendarCode, gPutBillNbr As String
    Public gPutPhrase As Object
    Public gPutChamberLight, gPutVotingLight As Integer
    Public gCheckingParams As Boolean
    Public gSessionName, gSessionAbbrev As String
    Public gLegislativeDay, gCalendarDay As String
    Public gChamberHelp As Boolean
    Public gVotingHelp, gPrintVoteRpt As Boolean
    Public gWorkData As Object
    Public gHTMLFile, gNbrSenator As String
    Public gNbrSenators, gNbrSenatorsNew As Integer
    Public gSenatorSplit As Short
    Public gSenatorOID() As Integer
    Public gDistrictOID() As Integer
    Public gWriteVogTestogTest As Boolean
    Public gLegislativeDayOID As Integer
    Public gDeletegTestVotesOnStartUp As Boolean
    Public gNbrVoteHistoryDays As Short
    Public gSessionIDTmp() As Integer
    Public gLegislativeDayTmp() As Short
    Public gLegislativeDayOIDTmp() As Integer
    Public gCalendarDayTmp() As Object
    Public gSessionNameTmp() As String
    Public gSessionAbbrevTmp() As String
    Public gDeleteTestVotesOnStartUp As Boolean
    Public gDataComputerName As String
    Public SD2Alive, SD1Alive, WoodyAlive As Boolean
    Public gLastVoteIDPerALIS, gLastVoteIDPerLocal As Integer
    Public gOnlyOnePC As Boolean
    Public frmMessage As New frmMessage



    Public strVoting As String                              '   connection string for VoteSys.mdb access database
    Public strSecVoting As String                           '   connection string for Secondary PC VoteSys.mdb access database
    Public strVoteTest As String
    Public strALIS As String                                '   connection string for ALIS Oracle Database
    Public cnVoting As OleDbConnection = Nothing
    Public cnSecVoting As OleDbConnection
    Public cnDisplay As OleDbConnection
    Public cnALIS As OleDbConnection
    Public v_Rows, a_Rows As Integer
    Public str As String
    Public cDateTime As String
    Public gStrVoting As String
    Public gStrALISProd As String
    Public gStrALISTest As String
    Public gDatabasePath As String
    Public gDataPath As String
    Public gVotingPath As String
    Public gLogFile, gDisplayTextFile As String
    Public msgText As String = "Alabama Senate Voting System"
    Public gSendQueueName, gSendQueueNameForSecondary, gSendVoteQueueToDisplay, gSendQueueToDisplay, gReceiveQueueName, gSendQueueTimer, gReceiverQueueTimer, gHTMLTimer, tmpWrkFld, Alis_VoteID, gVoteIDTest As String
    Public gProduction, gWriteVotesToTest As Boolean
    Public gPrimary_IPAddress, gSecondary_IPAddress, gDisplayPC_IPAddress, gOOB1_IPAddress, gOOB2_IPAddress, gPowerPointFilePath, gLastVoteIDForThisSessionTEST As String
    Public gPrimary_Conn, gSecondary_Conn, gOOB1_Conn, gOOB2_Conn, gALIS_Primary_Conn, gALIS_Secondary_Conn, gDisplay_Conn, gALIS_TEST_Conn, gWriteToAlisVoteTable, gWriteToAlisIndividualVoteTable As String
    Public SPrimary_On, SSecondary_On, SOOB1_On, SOOB2_On, SDisplay_On, SMembersVotePC_On, AlisProda_On, AlisProdb_On, AlisTest_On, Network_On, gNetwork, gWorkingGroup, tableExist As Boolean
    ' Public strSenator, strPhrase As String
    Public tPage0, tPage1, tPage2, bSenator, bPhrase, dataProcess, DisplayImage As Boolean


    ' pass all of values to frmChamberDisplay form for clicking Phrase or Senator from frmPhrase or frmSenator form
    Public pBill, pVoteID, pCSession, pLegDate, pSenator, pCommittee, pBusiness, pCCalendar, pPhrase, pOBusiness, pCalendar, pCBill, pInsertText, pWorkData, pWorkData2, pWorkData3 As String


    'Public gSendQueueName As String = "FormatName:DIRECT=OS:LEG13\PRIVATE$\senatevotequeue"
    'Public gReceiveQueueName As String = ".\PRIVATE$\senatevotequeue"
    'Public queueName As String = ".\PRIVATE$\SenateVoteQueue"

    Declare Function MoveWindow Lib "user32" (ByVal hwnd As Integer, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal bRepaint As Integer) As Integer
    Declare Function GetComputerName Lib "kernel32" Alias "GetComputerNameA" (ByVal lpBuffer As String, ByRef nSize As Integer) As Integer


    Public frmChamberDisplay As New frmChamberDisplay
    Public frmPhraseShow As New frmPhraseShow
    Public frmSenatorShow As New frmSenatorShow



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
        On Error GoTo FileExistsERH
        ERHPushStack("FileExists - " & FileName)

        FileExistes = True
        FileOpen(89, FileName, OpenMode.Input)
        FileClose(89)

        ERHPopStack()
        Exit Function

FileExistsERH:
        ' ---76 is path not found; 68 is device not available; 70 is permission denied implies file is found
        If Err.Number = 70 Then
            Exit Function
        ElseIf Err.Number = 53 Or Err.Number = 68 Or Err.Number = 76 Then

            FileExistes = False
            Exit Function
        End If

        ERHHandler()
        Exit Function
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
            gWriteVogTestogTest = False
            Network_On = False
            ' DisplayMessage("ALIS Oracle Databases is accessible", strText, "I")

            MsgBox("Network interruped. Please reset voting system computers to Local Working Group to continue the process.", MsgBoxStyle.Critical, "Has_Network_Connectivity()")
            gWorkingGroup = True
            Application.Exit()
        End Try
    End Sub

    Public Sub Check_ALIS_Database_Accessible()
        Dim connProda = New OleDbConnection(gALIS_Secondary_Conn)
        Dim connProdb = New OleDbConnection(gALIS_Primary_Conn)
        Dim connTest = New OleDbConnection(gALIS_TEST_Conn)
        Dim strText As String = "Main - Check_ALIS_Database_Accessible()"
        Dim RetValue As New Object

        ERHPushStack("Main - Check_Oracle_Accessible")

        Try
            '---Check ALIS_PRODB - alis primary
            connProdb.Open()
            If connProdb.State = ConnectionState.Open Then
                AlisProdb_On = True
            End If
        Catch
            AlisProdb_On = False
        End Try
        connProdb.Close()


        Try
            '---Check ALIS_PRODA - alis secondary
            connProda.Open()
            If connProda.State = ConnectionState.Open Then
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
                AlisTest_On = True
            End If
        Catch
            AlisTest_On = False
        End Try
        connTest.Close()


        '--- Initial oledbConnection to connect to local VotingSys.mdb database
        If gTestMode = False Then
            If AlisProdb_On And AlisProda_On Then
                strALIS = gALIS_Primary_Conn
                cnALIS = New OleDbConnection(strALIS)
                RetValue = DisplayMessage("ALIS Oracle Databases is accessible", strText, "I")
            ElseIf AlisProdb_On = False And AlisProda_On Then
                strALIS = gALIS_Secondary_Conn
                cnALIS = New OleDbConnection(strALIS)
                RetValue = DisplayMessage("Attention: Only able open the ALIS_PRODA connection to Woody.", strText, "I")
            ElseIf AlisProdb_On And AlisProda_On = False Then
                strALIS = gALIS_Primary_Conn
                cnALIS = New OleDbConnection(strALIS)
                RetValue = DisplayMessage("Attention: Only able open the ALIS_PRODB connection to Buzz.", strText, "I")
            ElseIf AlisProdb_On = False And AlisProda_On = False Then
                If MsgBox("Attention: unable write voting data to ALIS production database. Would you want continu process by local 'Working Group'? Otherwise system will shut down.", MsgBoxStyle.YesNo, strText) = MsgBoxResult.Yes Then
                    gWorkingGroup = True
                    cnALIS = Nothing
                Else
                    Application.Exit()
                    ' write log file here!
                End If
            End If
        Else
            If AlisTest_On Then
                strVoteTest = gALIS_TEST_Conn
                cnALIS = New OleDbConnection(strVoteTest)
                If gWriteVotesToTest Then
                    gWriteVotesToTest = True
                Else
                    gWriteVotesToTest = False
                End If
            Else
                gWriteVotesToTest = False
                strVoteTest = gALIS_TEST_Conn
                cnALIS = New OleDbConnection(strVoteTest)
                cnVoting = New OleDbConnection(strVoting)
                RetValue = DisplayMessage("Unable Write Test Vote Data To ALIS_TEST Oracle Database.", strText, "I")
            End If
        End If

        ERHPopStack()
        Exit Sub
    End Sub

    Public Sub Get_Voting_Parameters()
        Dim da As OleDbDataAdapter
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim dsP As New DataSet
        Dim parameters As New ArrayList()
        Dim RepValue As New Object
        Dim sqlStr As String = "Select * From tblVotingParameters Where Parameter="
        Dim strFlag As String
        On Error GoTo ProcERH
        ERHPushStack("Main - Get_Voting_Parameters")


        '---Builte power on PC's access database connection string to get parameters first
        If gComputerName <> "" Then
            gDataComputerName = gComputerName

            '--- Get Voting system parameters from power on PC          
            gPrimary_IPAddress = QueryParameters(sqlStr & "'PrimaryIPAddress'").Tables(0).Rows(0).Item("ParameterValue")
            gSecondary_IPAddress = QueryParameters(sqlStr & "'SecondaryIPAddress'").Tables(0).Rows(0).Item("ParameterValue")
            gOOB1_IPAddress = QueryParameters(sqlStr & "'OOB1IPAddress'").Tables(0).Rows(0).Item("ParameterValue")
            gOOB2_IPAddress = QueryParameters(sqlStr & "'OOB2IPAddress'").Tables(0).Rows(0).Item("ParameterValue")
            gDisplayPC_IPAddress = QueryParameters(sqlStr & "'DisplayPCIPAddress'").Tables(0).Rows(0).Item("ParameterValue")
            gPrimary_Conn = QueryParameters(sqlStr & "'PrimaryConnection'").Tables(0).Rows(0).Item("ParameterValue")
            gSecondary_Conn = QueryParameters(sqlStr & "'SecondaryConnection'").Tables(0).Rows(0).Item("ParameterValue")
            gOOB1_Conn = QueryParameters(sqlStr & "'OOB1Connection'").Tables(0).Rows(0).Item("ParameterValue")
            gOOB2_Conn = QueryParameters(sqlStr & "'OOB2Connection'").Tables(0).Rows(0).Item("ParameterValue")
            gALIS_Primary_Conn = QueryParameters(sqlStr & "'ALISPrimary'").Tables(0).Rows(0).Item("ParameterValue")
            gALIS_Secondary_Conn = QueryParameters(sqlStr & "'ALISSecondary'").Tables(0).Rows(0).Item("ParameterValue")
            gALIS_TEST_Conn = QueryParameters(sqlStr & "'ALISTEST'").Tables(0).Rows(0).Item("ParameterValue")
            gVotingPath = QueryParameters(sqlStr & "'VotingSystemPath'").Tables(0).Rows(0).Item("ParameterValue")
            gDatabasePath = QueryParameters(sqlStr & "'DatabasePath'").Tables(0).Rows(0).Item("ParameterValue")
            gSendQueueName = QueryParameters(sqlStr & "'SendQueueName'").Tables(0).Rows(0).Item("ParameterValue")
            gSendQueueNameForSecondary = QueryParameters(sqlStr & "'SendQueueNameForSecondary'").Tables(0).Rows(0).Item("ParameterValue")
            gSendVoteQueueToDisplay = QueryParameters(sqlStr & "'SendVoteQueueToDisplay'").Tables(0).Rows(0).Item("ParameterValue")
            gSendQueueToDisplay = QueryParameters(sqlStr & "'SendQueueToDisplay'").Tables(0).Rows(0).Item("ParameterValue")
            gReceiveQueueName = QueryParameters(sqlStr & "'ReceivequeueName'").Tables(0).Rows(0).Item("ParameterValue")
            gALISTimer = QueryParameters(sqlStr & "'ALISTimer'").Tables(0).Rows(0).Item("ParameterValue")
            gChamberTimer = QueryParameters(sqlStr & "'ChamberTimer'").Tables(0).Rows(0).Item("ParameterValue")
            gVotingTimer = QueryParameters(sqlStr & "'VotingTimer'").Tables(0).Rows(0).Item("ParameterValue")
            strFlag = QueryParameters(sqlStr & "'TestMode'").Tables(0).Rows(0).Item("ParameterValue")
            gDisplayTextFile = QueryParameters(sqlStr & "'DisplayTextFile'").Tables(0).Rows(0).Item("ParameterValue")
            gDisplay_Conn = QueryParameters(sqlStr & "'DisplayPCConnection'").Tables(0).Rows(0).Item("ParameterValue")
            gPowerPointFilePath = QueryParameters(sqlStr & "'PowerPointFilePath'").Tables(0).Rows(0).Item("ParameterValue")
            If strFlag = "Y" Then
                gTestMode = True
            Else
                gTestMode = False
            End If
            strFlag = QueryParameters(sqlStr & "'WriteVotesToAlisTest'").Tables(0).Rows(0).Item("ParameterValue")
            If strFlag = "Y" Then
                gWriteVotesToTest = True
            Else
                gWriteVotesToTest = False
            End If
            gNbrVoteHistoryDays = QueryParameters(sqlStr & "'VoteHistoryDays'").Tables(0).Rows(0).Item("ParameterValue")
            gNbrPhrasesToDisplay = QueryParameters(sqlStr & "'NumberOfPhrasesToDisplay'").Tables(0).Rows(0).Item("ParameterValue")
            gNbrSenator = QueryParameters(sqlStr & "'NumberOfSenator'").Tables(0).Rows(0).Item("ParameterValue").ToString
            gNbrSenators = CType(gNbrSenator, Integer)
            If gTestMode = False Then
                gSVoteID = QueryParameters(sqlStr & "'LastVoteIDForThisSession'").Tables(0).Rows(0).Item("ParameterValue")
                gVoteID = CType(gSVoteID, Integer)
            Else
                gSVoteID = QueryParameters(sqlStr & "'LastVoteIDForThisSessionTEST'").Tables(0).Rows(0).Item("ParameterValue")
                gVoteID = CType(gSVoteID, Integer)
            End If
            gHTMLFile = QueryParameters(sqlStr & "'HTMLFile'").Tables(0).Rows(0).Item("ParameterValue")
            gHTMLTimer = QueryParameters(sqlStr & "'HTMLTimer'").Tables(0).Rows(0).Item("ParameterValue")
            gLogFile = QueryParameters(sqlStr & "'LogFile'").Tables(0).Rows(0).Item("ParameterValue")
            gSendQueueTimer = QueryParameters(sqlStr & "'SendQueueTimer'").Tables(0).Rows(0).Item("ParameterValue")
            gReceiverQueueTimer = QueryParameters(sqlStr & "'GetQueueTimer'").Tables(0).Rows(0).Item("ParameterValue")
            gWriteToAlisVoteTable = QueryParameters(sqlStr & "'WriteToAlisVoteTableFile'").Tables(0).Rows(0).Item("ParameterValue")
            gWriteToAlisIndividualVoteTable = QueryParameters(sqlStr & "'WriteToAlisIndividualVoteTable'").Tables(0).Rows(0).Item("ParameterValue")
          
            strFlag = QueryParameters(sqlStr & "'DeleteTestVotesOnStart'").Tables(0).Rows(0).Item("ParameterValue")
            If strFlag = "Y" Then
                gDeletegTestVotesOnStartUp = True
            Else
                gDeletegTestVotesOnStartUp = False
            End If

            strFlag = QueryParameters(sqlStr & "'AutoPrintVoteReport'").Tables(0).Rows(0).Item("ParameterValue")
            If strFlag = "Y" Then
                gPrintVoteRpt = True
            Else
                gPrintVoteRpt = False
            End If

            strFlag = QueryParameters(sqlStr & "'ChamberHelp'").Tables(0).Rows(0).Item("ParameterValue")
            If strFlag = "Y" Then
                gChamberHelp = True
            Else
                gChamberHelp = False
            End If

            strFlag = QueryParameters(sqlStr & "'VotingHelp'").Tables(0).Rows(0).Item("ParameterValue")
            If strFlag = "Y" Then
                gVotingHelp = True
            Else
                gVotingHelp = False
            End If
        End If
        Exit Sub
ProcERH:
        ERHHandler()
        Exit Sub
    End Sub

    Public Sub Check_Voting_PCs_Status()
        Dim RetValue As New Object
        Dim Ping As Ping
        Dim pReply As PingReply
        Dim mes As String = "Main - Check_Voting_PCs_Status()"

        ERHPushStack(mes)

        Ping = New Ping

        '---ping Voting System Paramery Computer
        pReply = Ping.Send(gPrimary_IPAddress)
        If pReply.Status = IPStatus.Success Then
            SPrimary_On = True
            RetValue = DisplayMessage("Senate Vote Primary Computer Is On.", mes, "I")
        Else
            SPrimary_On = False
            RetValue = DisplayMessage("Senate Vote Primary Computer Is Off.", mes, "I")
        End If

        '---ping Voting System Secondary Computer
        pReply = Ping.Send(gSecondary_IPAddress)
        If pReply.Status = IPStatus.Success Then
            SSecondary_On = True
            RetValue = DisplayMessage("Senate Vote Secondary Computer Is On.", mes, "I")
        Else
            SSecondary_On = False
            RetValue = DisplayMessage("Senate Vote Secondary Computer Is Off.", mes, "I")
        End If

        '---ping OOB1 Computer
        pReply = Ping.Send(gOOB1_IPAddress)
        If pReply.Status = IPStatus.Success Then
            SOOB1_On = True
            RetValue = DisplayMessage("Order Of Business Primary Computer Is On.", mes, "I")
        Else
            SOOB1_On = False
            RetValue = DisplayMessage("Order Of Business Primary Computer Is Off.", mes, "I")
        End If

        ' ping OOB2 Computer
        pReply = Ping.Send(gOOB2_IPAddress)
        If pReply.Status = IPStatus.Success Then
            SOOB2_On = True
            RetValue = DisplayMessage("Order Of Business Secondary Computer Is On.", mes, "I")
        Else
            SOOB2_On = False
            RetValue = DisplayMessage("Order Of Business Secondary Computer Is Off.", mes, "I")
        End If

        '--- ping Display Computer
        pReply = Ping.Send(gDisplayPC_IPAddress)
        If pReply.Status = IPStatus.Success Then
            SDisplay_On = True
            RetValue = DisplayMessage("Display Computer Is On.", mes, "I")
        Else
            SDisplay_On = False
            RetValue = DisplayMessage("Display Computer Is Off. Unable Display Vote Informations.", mes, "I")
        End If

        '---initial OledbConnection for VoteSys.mdb access databases
        If SOOB1_On And SOOB2_On Then
            If gOOB1_IPAddress <> gOOB2_IPAddress Then         'Update priamary and secondary database same time
                strVoting = gOOB1_Conn
                strSecVoting = gOOB2_Conn
                cnVoting = New OleDbConnection(strVoting)
                cnSecVoting = New OleDbConnection(strSecVoting)
                gOnlyOnePC = False

                If SDisplay_On Then
                    cnDisplay = New OleDbConnection(gDisplay_Conn)
                End If
            Else
                gOnlyOnePC = True
                strVoting = gOOB1_Conn
                cnVoting = New OleDbConnection(strVoting)
            End If
        ElseIf SOOB1_On And SOOB2_On = False Then           'Only able update priamary database
            gOnlyOnePC = True
            strVoting = gOOB1_Conn
            cnVoting = New OleDbConnection(strVoting)
            cnSecVoting = Nothing
            RetValue = DisplayMessage("Unable Writes Data To Secondary PC.", mes, "I")

            If SDisplay_On Then
                cnDisplay = New OleDbConnection(gDisplay_Conn)
            End If
        ElseIf SOOB1_On = False And SOOB2_On Then           'Only able update seconday database
            gOnlyOnePC = True
            strVoting = gOOB2_Conn
            cnVoting = Nothing
            cnSecVoting = New OleDbConnection(strVoting)
            RetValue = DisplayMessage("Unable Writes Data To Primary PC.", mes, "I")

            If SDisplay_On Then
                cnDisplay = New OleDbConnection(gDisplay_Conn)
            End If
        End If
    End Sub

    Public Sub Main()
        Dim ds As New DataSet
        Dim dsP As New DataSet
        Dim str As String
        Dim RetValue As New Object, CheckDate As Object, x As Long, WrkFld As String

        gNetwork = True
        gWriteVogTestogTest = False
        gOnlyOnePC = False
        gWorkingGroup = False
        gTestMode = False

        On Error GoTo ProcERH
        ERHPushStack("Main")


        DisplayMessage("Welcome To The Alabama Senate Voting System!", msgText, "I")

        If FileExistes("C:\VotingSystem\VoteSys.mdb") Then
            '--- First - Built Voting System Access Database Connection String From Power On PC
            gComputerName = Trim(System.Environment.MachineName)
            gPrimary_Conn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\VotingSystem\VoteSys.mdb;User ID=admin;Password=;"
            ' gPrimary_Conn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\VotingSystem\VoteSys.accdb;Persist Security Info=False;"
            cnVoting = New OleDbConnection(gPrimary_Conn)

            '---get initial voting parameters from local VoteSys.mdb Access Database
            Get_Voting_Parameters()

            '---check Voting PCs Status
            Check_Voting_PCs_Status()

            '---check Network Connectivity 
            Has_Network_Connectivity()

            '---check Oracle database 
            Check_ALIS_Database_Accessible()

            '---for dendisplay1 and sendisplay2, set the paths to files on this PC; for senvote1
            '---and senvote2, set the paths to files on sendisplay1
            '---NOTE: KEEP COMPUTER NAMES IN CAPS
            x = 40
            WrkFld = Space(40)

            '---Before download Senator records, resize array. 
            gSenatorSplit = Int((gNbrSenators / 2) + 0.5)
            ReDim gSenatorName(gNbrSenators)
            ReDim gSalutation(gNbrSenators)
            ReDim gSenatorNameOrder(gNbrSenators)
            ReDim gSenatorOID(gNbrSenators)
            ReDim gDistrictOID(gNbrSenators)

            '---download Senator records from ALIS
            LoadSenatorsIntoArray()
            gNbrSenatorsNew = gNbrSenators

            ' ''---download Bills from ALIS
            DownloadBillsFromALIS()

            ' ''ReadDisplayBoardAttributes()

            '--- these test votes are different from session test votes deleted above;
            If gDeletegTestVotesOnStartUp Then
                V_DataSet("Delete From tblRollCallVotes Where Test= 1", "D")   ' delete any test votes
            End If

            '--- delete roll call votes per # of history days; if days=999, then no delete
            If gNbrVoteHistoryDays < 999 Then
                CheckDate = DateAdd(DateInterval.Day, -gNbrVoteHistoryDays, Date.Today)
                V_DataSet("Delete From tblRollCallVotes Where VoteDate<" & CheckDate, "D")
            End If

            ERHPopStack()
            Exit Sub
        Else
            MsgBox("Can not find " & gDatabasePath & " file. Please contact to Administator.", MsgBoxStyle.Critical, "Senate Voting System")
            ' write log file
            Application.Exit()
        End If
ProcERH:
        ERHHandler()
        Exit Sub
    End Sub

    Public Function NToB(ByRef var As Object) As Object
        '--- Accepts a variant argument and returns a blank if the argument is Null or the
        '---  argument itself if it is not Null.
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

        On Error GoTo ProcERH
        ERHPushStack(("ReplaceCharacter"))
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
ProcERH:
        ERHHandler()
        Exit Function
    End Function

    Private Sub ReadDisplayBoardAttributes()
        Dim ds As New DataSet

        On Error GoTo ProcERH
        ERHPushStack(("ReadDisplayBoardAttributes"))

        ds = V_DataSet("Select * From tblProjectionDisplayAttributes ", "R")
        If ds.Tables(0).Rows.Count <> 0 Then
            gOOBHeadingAttributes = ds.Tables(0).Rows(0).Item("OrderOfBusinessHeading")
            gNotSelectedOOBAttributes = ds.Tables(0).Rows(0).Item("NotSelectedOrdersOfBusiness")
            gSelectedOOBAttributes = ds.Tables(0).Rows(0).Item("SelectedOrderOfBusiness")
            gBillAttributes = ds.Tables(0).Rows(0).Item("Bill")
            gPhraseAttributes = ds.Tables(0).Rows(0).Item("Phrase")
            gSubjectAttributes = ds.Tables(0).Rows(0).Item("Subject")
        End If

        ERHPopStack()
        Exit Sub
ProcERH:
        ERHHandler()
        Exit Sub
    End Sub

    Public Sub LoadCommitteesIntoArray()
        Dim i As Short
        Dim ds As New DataSet
        Dim dr As DataRow

        Try
            ERHPushStack(("LoadCommitteesIntoArray"))

            '--- init to blanks
            ReDim gCommittees(0)
            ReDim gCommitteeAbbrevs(0)

            str = "Select Abbrev + '-' + Committee AS TheCommittee, Abbrev, Committee FROM tblCommittees Order By Abbrev"
            ds = V_DataSet(str, "R")

            ReDim Preserve gCommittees(ds.Tables(0).Rows.Count)
            ReDim Preserve gCommitteeAbbrevs(ds.Tables(0).Rows.Count)

            For Each dr In ds.Tables(0).Rows
                i += 1
                gCommittees(i) = dr("Committee")
                gCommitteeAbbrevs(i) = dr("Abbrev")
            Next
            gNbrCommittees = i

            ERHPopStack()
            Exit Sub
        Catch
            ERHHandler()
        End Try
    End Sub

    Public Sub LoadPhrasesIntoArray()
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim i As Short
        ReDim gPhrases(0)           '--- init to blanks
        Dim gPrhaseCodes(0) As Object

        Try
            ERHPushStack(("LoadPhrasesIntoArray"))

            str = "Select Cstr(Code)  + ' - ' + Phrase  AS ThePhrase, Code, Phrase From tblPhrases Order By Code"
            ds = V_DataSet(str, "R")

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

            ERHPopStack()
        Catch
            ERHHandler()
        End Try
    End Sub

    Function IsLoaded(ByRef ThisFormName As String) As Short
        '--- Accepts: a form name
        '--- Purpose: determines if a form is loaded
        '--- Returns: True if specified the form is loaded; False if the specified form is not loaded.

        Dim forms As New Collection
        Dim Ctr, FormFound As Short
        Dim i As Integer

        On Error GoTo IsLoadedERH
        ERHPushStack(("IsLoaded - " & ThisFormName))

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

        ERHPopStack()
        Exit Function

IsLoadedERH:
        ERHHandler()
        Exit Function
    End Function

    Sub WaitUntilFormCloses(ByRef FormName As String)
        Do Until Not IsLoaded(FormName)
            System.Windows.Forms.Application.DoEvents()
        Loop
    End Sub

    Public Function DownloadBillsFromALIS() As Integer
        Dim drV, drA, drA1 As DataRow
        Dim dsV As New DataSet
        Dim dsA As New DataSet
        Dim dsA1 As New DataSet
        Dim FieldValue(), FieldName() As Object, k As Integer
        Dim Title, WrkFld, str As String, RetValue As Object

        On Error GoTo ProcERH
        ERHPushStack("DownloadBillsFromALIS")

        RetValue = DisplayMessage("Please wait while " & IIf(gWriteVogTestogTest, "gTest ", "") & "bills are downloaded from ALIS.", "Main - DownloadBillsFromALIS()", "I")


        '--- get legislative day and session data; check for multiple open legislative days; if
        '--- found, then display and allow user to select or cancel; cancel
        '--- exits the system and assumes someone will fix on the ALIS end; only
        '--- trap 3 open days
        str = "SELECT LD.OID, LD.LEGISLATIVE_DAY, " & _
                "TO_CHAR(LD.CALENDAR_DATE,  'YYYY-MM-DD') AS CAL_DAY, " & _
                "UPPER(LD.STATUS_CODE) AS STATUS, LD.OID_SESSION, " & _
                "LS.LABEL, LS.ABBREVIATION " & _
                "FROM LEGISLATIVE_DAY LD, LEGISLATIVE_SESSION LS " & _
                "WHERE (LD.OID_SESSION = LS.OID) AND " & _
                "(NOT (LD.LEGISLATIVE_DAY IS NULL)) AND " & _
                "(LD.Status_code = 'O') " & _
                "ORDER BY LD.LEGISLATIVE_DAY DESC"
        dsA = A_DataSet(str, "R")

        k = -1
        For Each drA In dsA.Tables(0).Rows
            k = k + 1
            ReDim Preserve gSessionIDTmp(k)
            ReDim Preserve gLegislativeDayTmp(k)
            ReDim Preserve gLegislativeDayOIDTmp(k)
            ReDim Preserve gCalendarDayTmp(k)
            ReDim Preserve gSessionNameTmp(k)
            ReDim Preserve gSessionAbbrevTmp(k)
            gSessionIDTmp(k) = drA("OID_Session")
            gLegislativeDayTmp(k) = drA("Legislative_Day")
            gLegislativeDayOIDTmp(k) = drA("OID")
            gCalendarDayTmp(k) = FormatDateTime(drA("Cal_Day"), DateFormat.ShortDate)
            gSessionNameTmp(k) = drA("Label")
            gSessionAbbrevTmp(k) = drA("Abbreviation")
            If k = 2 Then
                Exit For
            End If
        Next

        '--- get current open session id, leg day, and calendar day too
        If k = 0 Then
            gSessionID = gSessionIDTmp(0)
            gLegislativeDay = gLegislativeDayTmp(0)
            gLegislativeDayOID = gLegislativeDayOIDTmp(0)
            gCalendarDay = gCalendarDayTmp(0)
            gSessionName = gSessionNameTmp(0)
            gSessionAbbrev = gSessionAbbrevTmp(0)
        Else
            Dim frmMOLD As New frmMultipleOpenLegislativeDays
            frmMOLD.Show()
            WaitUntilFormCloses("frmMultipleOpenLegislativeDays")
        End If

        dsA.Dispose()


        '--- get last voteid from ALIS for this session and update the
        '--- parameter table
        str = " SELECT MAX(TO_NUMBER(voteid)) AS LastVoteID  " & _
                " FROM LEGISLATIVE_DAY LD, VOTE " & _
                " WHERE LD.oid = VOTE.oid_legislative_day " & _
                " AND (LD.oid_session =" & gSessionID & ") " & _
                " AND (VOTE.oid_legislative_body = " & gSenateID & ")"
        dsA1 = A_DataSet(str, "R")

        If dsA1.Tables(0).Rows.Count = 0 Then
            RetValue = DisplayMessage("Cannot find last vote ID for session " & gSessionName & _
               " in ALIS.  The last vote ID will be set to 0.  If this is not correct, you must enter the last vote ID using the Parameter Maintenance form.", "No Vote ID In ALIS", "S")
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
        str = "SELECT MAX(VoteID) AS LastVoteID FROM tblRollCallVotes WHERE (SessionID = " & gSessionID & " ) AND (ALISVoteOID = 0)"
        dsV = V_DataSet(str, "R")
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
            RetValue = DisplayMessage("Attention: Local Last Vote ID is grate than ALIS production Last Vote ID.", "Main() - Check_ALIS_Database_Accessible", "I")
        ElseIf gLastVoteIDPerLocal < gLastVoteIDPerALIS Then
            RetValue = DisplayMessage("Attention: Local Last Vote ID is less than ALIS production Last Vote ID.", "Main() - Check_ALIS_Database_Accessible", "I")
        End If

        If gTestMode = False Then
            V_DataSet("Update tblVotingParameters SET ParameterValue='" & gVoteID & "' Where Parameter='LastVoteIDForThisSession'", "U")
        Else
            V_DataSet("Update tblVotingParameters SET ParameterValue='" & gVoteID & "' Where Parameter='LastVoteIDForThisSessionTEST'", "U")
        End If

        gHTMLFile = Mid$(gHTMLFile, 1, InStr(gHTMLFile, "*") - 1) & gSessionAbbrev & Mid$(gHTMLFile, InStr(gHTMLFile, "*") + 1)

        '--- always do this: Delete special order calendar from tblCalendar table
        V_DataSet("DELETE FROM tblCalendars WHERE LEFT(CalendarCode, 2)='SR'", "D")

        '--- save any bills that have something in the work area to a temporary table and
        '--- then restore the work areas if the same bills are downloaded again
        On Error Resume Next
        '' V_DataSet("Drop Table tblWork", "U")
        On Error GoTo ProcERH

        ' '' recreate tblWork table agina
        ''str = "SELECT CalendarCode, BillNbr, WorkData INTO tblWork FROM tblBills WHERE (WorkData <>'')"
        ''V_DataSet(str, "U")


        ' field 0 - calendar code
        ' field 1 - bill #
        ' field 2 - bill description - what will be displayed for the bil - composed of bill #, sponsor, , subject (includes desc for confirmation), and calendar page
        ' field 3 - sponsor
        ' field 4 - subject
        ' field 5 - work area data - always blank when added here - built by chamber display
        ' field 6 - calendar page


        '--- delete all other bills dynamically retrieved during the day
        V_DataSet("DELETE FROM tblBills WHERE (CalendarCode='ZZ')", "D")

        '--- get regular calendar, before get it Delete old first
        V_DataSet("DELETE FROM tblBills WHERE (CalendarCode='1')", "D")

        dsV = V_DataSet("Select * From tblBills", "R")

        ReDim FieldName(6)
        ReDim FieldValue(6)
        For k = 0 To 6
            FieldName(k) = dsV.Tables(0).Columns(k).ColumnName       '???
        Next k

        str = " SELECT label, sponsor, alis_object.index_word, calendar_page " & _
                " FROM alis_object, matter " & _
                " WHERE matter.oid_instrument = alis_object.oid AND matter.oid_session = " & gSessionID & " AND alis_object.oid_session = " & gSessionID & _
                " AND matter.matter_status_code = 'Pend' AND matter.oid_legislative_body = '1753' AND alis_object.legislative_body = 'S' " & _
                " AND matter.calendar_sequence_no > 0 ORDER BY matter.calendar_sequence_no"
        dsA = A_DataSet(str, "R")

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
                FieldValue(5) = ""
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
                V_DataSet("INSERT INTO tblBills VALUES ('1', '" & FieldValue(1) & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "')", "A")
            Next
        End If

        '--- get local bills
        V_DataSet("Delete From tblBills Where CalendarCode ='2'", "D")

        str = "SELECT label, calendar_page, senate_display_title, sponsor " & _
                " FROM alis_object, matter WHERE matter.oid_instrument = alis_object.oid " & _
                " AND matter.oid_session = " & gSessionID & " AND alis_object.oid_session = " & gSessionID & " AND matter.matter_status_code = 'Pend' " & _
                " AND matter.oid_legislative_body = '1753' AND alis_object.legislative_body = 'S' AND alis_object.local_bill = 'T' " & _
                " AND matter.calendar_sequence_no > 0 ORDER BY matter.calendar_sequence_no "
        dsA = A_DataSet(str, "R")

        If dsA.Tables(0).Rows.Count > 0 Then
            For Each drA In dsA.Tables(0).Rows
                FieldValue(0) = "2"
                FieldValue(1) = drA("Label")
                If Strings.Left(FieldValue(1), 1) = "S" Then
                    Title = " by Senator "
                Else
                    Title = " by Rep. "
                End If
                FieldValue(3) = NToB(drA("Sponsor"))
                FieldValue(4) = Mid$(NToB(drA("Senate_Display_Title")), InStr(NToB(drA("Senate_Display_Title")), " ") + 1)
                FieldValue(5) = ""
                FieldValue(6) = drA("Calendar_Page")
                If CType(FieldValue(6), String) <> "" Then
                    FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4) & " p." & FieldValue(6)
                Else
                    FieldValue(2) = FieldValue(1) & Title & FieldValue(3) & " - " & FieldValue(4)
                End If
                V_DataSet("INSERT INTO tblBills VALUES ('2', '" & drA("Label") & "', '" & Replace(FieldValue(2), "'", "") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "')", "A")
            Next
        End If

        '--- get confirmations
        V_DataSet("Delete From tblBills Where CalendarCode='3'", "D")

        str = "SELECT I.OID, I.LABEL, I.TYPE_CODE, PE.FIRSTNAME, PE.LASTNAME, PE.SUFFIX, O.NAME, CO.ABBREVIATION, TO_CHAR(R1DAY.CALENDAR_DATE, 'MM/DD/YYYY') AS R1DATE, TO_CHAR(MDAY.CALENDAR_DATE, 'MM/DD/YYYY') AS MDATE, " & _
                " M.TRANSACTION_TYPE_CODE AS MTRAN, M.MATTER_STATUS_CODE AS MSTAT, M.RECOMMENDATION_CODE AS MREC, AU.NAME AS AUTHORITY, CV.Value AS INSTR_VALUE " & _
            " FROM ALIS_OBJECT I, MATTER C1, MATTER M, ORGANIZATION CO, ORGANIZATION O, POSITION PO, POSITION AU, PERSON PE, LEGISLATIVE_DAY R1DAY, LEGISLATIVE_DAY MDAY, CODE_VALUES CV " & _
            " WHERE I.INSTRUMENT = 'T' AND I.TYPE_CODE = 'CF' AND I.STATUS_CODE = CV.CODE (+) AND CV.CODE_TYPE = 'InstrumentStatus' AND I.OID_SESSION = " & gSessionID & " AND I.OID_POSITION = PO.OID (+) " & _
                " AND O.OID (+) = PO.OID_ORGANIZATION AND AU.OID (+) = PO.OID_APPOINTING_AUTHORITY AND I.OID = C1.OID_INSTRUMENT AND M.OID_COMMITTEE = CO.OID (+) AND C1.OID_CANDIDACY = PE.OID (+) " & _
                " AND C1.OID_CONSIDERED_DAY = R1DAY.OID (+) AND M.OID_CONSIDERED_DAY = MDAY.OID (+) AND M.OID_INSTRUMENT = I.OID AND M.SEQUENCE = I.LAST_MATTER AND I.OID_SESSION = C1.OID_SESSION " & _
                " AND C1.SEQUENCE = (SELECT MIN(X.SEQUENCE) FROM MATTER X WHERE X.OID_SESSION = C1.OID_SESSION AND X.OID_INSTRUMENT = C1.OID_INSTRUMENT AND X.TRANSACTION_TYPE_CODE = 'R1') ORDER BY SUBSTR(I.LABEL, 1, 1), I.TYPE_CODE, I.ID"
        dsA = A_DataSet(str, "R")

        If dsA.Tables(0).Rows.Count > 0 Then
            For Each drA In dsA.Tables(0).Rows
                FieldValue(0) = "3"
                FieldValue(1) = Mid$(drA("Label"), 2)
                FieldValue(3) = Replace(drA("FirstName"), "'", " ") & " " & Replace(drA("LastName"), "'", " ")
                FieldValue(4) = ""

                '--- if boards and commissions exist, then search for the senate voting name and replace if found            
                If NToB(drA("Name")) = "" Then
                    FieldValue(2) = Replace(FieldValue(1) & " - " & FieldValue(3) & " -- ", "'", " ")
                Else
                    WrkFld = Replace(drA("Name"), "'", "''")
                    str = "SELECT SenateVotingName FROM tblBoardsAndCommissions WHERE AlisName = '" & Replace(WrkFld, "'", "''") & "'"
                    dsV = V_DataSet(str, "R")
                    If dsV.Tables(0).Rows.Count = 0 Then
                        FieldValue(2) = Replace(FieldValue(1) & " - " & FieldValue(3) & " -- " & NToB(drA("Name")), "'", "''")
                    Else
                        FieldValue(2) = Replace(FieldValue(1) & " - " & FieldValue(3) & " -- " & dsV.Tables(0).Rows(0).Item("SenateVotingName"), "'", "''")         '???
                    End If
                End If
                FieldValue(5) = ""
                FieldValue(6) = ""
                V_DataSet("INSERT INTO tblBills VALUES ('3', '" & drA("Label") & "', '" & Replace(FieldValue(2), "'", "''") & "', '" & FieldValue(3) & "', '" & FieldValue(4) & "', '" & FieldValue(5) & "', '" & FieldValue(6) & "')", "A")
            Next
        End If

        ' ''--- restore any saved work areas to the bills downloaded
        ''dsV = V_DataSet("Select * From tblWork", "R")
        ''For Each drV In dsV.Tables(0).Rows
        ''    V_DataSet("UPDATE tblBills SET WorkData ='" & Replace(drV("WorkData"), "'", "''") & "' WHERE CalendarCode ='" & drV("CalendarCode") & "' AND BillNbr ='" & drV("BillNbr") & "'", "U")
        ''Next
        ''V_DataSet("Drop TABLE tblWork", "U")   '???

        '' RetValue = DisplayMessage("Download of " & IIf(gWriteVogTestogTest, "TEST ", "") & "bills from ALIS completed.", "Alabama Senate Voting System", "I")

        ERHPopStack()
        Exit Function
ProcERH:
        ERHHandler()
        Exit Function
    End Function

    Public Sub LoadSenatorsIntoArray()
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim i As Short
        Dim RetValue As New Object

        '--- load the senators into an array
        Try
            ERHPushStack(("LoadSenatorsIntoArray"))

            i = 0

            str = "Select * From tblSenators Order By SenatorName"
            ds = V_DataSet(str, "R")

            ReDim Preserve gSenatorName(gNbrSenators)
            ReDim Preserve gSenatorOID(gNbrSenators)
            ReDim Preserve gDistrictOID(gNbrSenators)

            For Each dr In ds.Tables(0).Rows
                i = i + 1
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
                End If
            Next
            ds.Dispose()

            RetValue = DisplayMessage("Download Senators Records Finished.", "Main - LoadSenatorsIntoArray()", "I")

            ERHPopStack()
            Exit Sub
        Catch
            ERHHandler()
            Exit Sub
        End Try
    End Sub

    Public Function QueryParameters(ByVal strSql As String) As DataSet
        Dim da As New OleDbDataAdapter
        Dim ds As New DataSet

        If cnVoting.State = ConnectionState.Closed Then cnVoting.Open()

        Try
            If strSql <> "" Then
                da = New OleDbDataAdapter(strSql, cnVoting)
                da.SelectCommand = New OleDbCommand(strSql, cnVoting)
                da.Fill(ds, "Table")
                QueryParameters = ds

                Return ds
            End If

        Catch ex As OleDbException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Senate Voting System")
        Finally
            cnVoting.Close()
        End Try
    End Function

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

    '        If ((gTestMode) And (Not gWriteVogTestogTest)) Or (Not WoodyAlive) Then
    '            GoTo ProcExit
    '        End If

    '        'rmChamberDisplay.ALISTimer.Interval = 0    ' make so no entry while it is running

    '        If gWriteVogTestogTest Then                   '1 gTest votes
    '            str = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And gTest =1 And AlisVoteOID=0"
    '        Else                                        '0 gProduction votes
    '            str = "Select * From tblRollCallVotes Where SessionID=" & gSessionID & " And gTest =0 And AlisVoteOID=0"
    '        End If
    '        ds = V_DataSet(str, "R")

    '        For Each dr In ds.Tables(0).Rows
    '            ' add vote header and retrieve the generated OID
    '            strAdd = "INSERT INTO Vote (oid, oid_legislative_Body, oid_Legilative_day, voteId, nays, yeas, abstains, pass) VALUES (vote_s.nextval, " & ", " & gSenateID & "' " & _
    '            dr("LegislativeDayOID") & ", " & dr("VoteID") & ", " & dr("TotalNay") & ", " & dr("TotalYea") & ", " & dr("TotalAbstain") & ", " & dr("TotalPass") & ")"
    '            A_DataSet(strAdd, cnALIS, "A")

    '            dsVoteOID = A_DataSet("SELECT ALIS.VOTE_S.CURRVAL AS VOTEOID FROM DUAL", cnALIS, "S")

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
    '                A_DataSet("INSERT INTO Individual_Vote(oid, oid_legislator, oid_vote, oid_organization, vote) VALUES (individual_vote_s.NEXTVAL, " & SenatorOID & "' " & VoteOID & "' " & DistrictOID & ", " & Vote & ")", cnALIS, "A")

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
    '        ERHPushStack("Main - Get_Parameters")


    '        '--- Get parameters
    '        str = "Select * From tblParameters"
    '        ds = V_DataSet(str, "R")

    '        gChamberTimer = ds.Tables(0).Rows(0).Item("ChamberTimer")
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
    '        If gTestMode = False And gWriteVogTestogTest = True Then
    '            gWriteVogTestogTest = False
    '            gSwitchBox = Left(gSwitchBox, 3) & "N" & Mid(gSwitchBox, 5)
    '            V_DataSet("UPDATE tblParameters SET SwitchBox='" & gSwitchBox & "'", "U")
    '        ElseIf gTestMode = True And gWriteVogTestogTest = False Then
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


End Module
