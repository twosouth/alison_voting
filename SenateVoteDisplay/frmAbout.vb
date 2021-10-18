'Imports Microsoft.VisualBasic
Imports System.Text
Imports System.Data
Imports System.IO
Imports System.Messaging

Public Class frmAbout
    '' Reg Key Security Options...
    'Const READ_CONTROL As Integer = &H20000
    'Const KEY_QUERY_VALUE As Short = &H1S
    'Const KEY_SET_VALUE As Short = &H2S
    'Const KEY_CREATE_SUB_KEY As Short = &H4S
    'Const KEY_ENUMERATE_SUB_KEYS As Short = &H8S
    'Const KEY_NOTIFY As Short = &H10S
    'Const KEY_CREATE_LINK As Short = &H20S
    'Const KEY_ALL_ACCESS As Double = KEY_QUERY_VALUE + KEY_SET_VALUE + KEY_CREATE_SUB_KEY + KEY_ENUMERATE_SUB_KEYS + KEY_NOTIFY + KEY_CREATE_LINK + READ_CONTROL

    '' Reg Key ROOT Types...
    'Const HKEY_LOCAL_MACHINE As Integer = &H80000002
    'Const ERROR_SUCCESS As Short = 0
    'Const REG_SZ As Short = 1 ' Unicode nul terminated string
    'Const REG_DWORD As Short = 4 ' 32-bit number

    'Const gREGKEYSYSINFOLOC As String = "SOFTWARE\Microsoft\Shared Tools Location"
    'Const gREGVALSYSINFOLOC As String = "MSINFO"
    'Const gREGKEYSYSINFO As String = "SOFTWARE\Microsoft\Shared Tools\MSINFO"
    'Const gREGVALSYSINFO As String = "PATH"

    'Private Declare Function RegOpenKeyEx Lib "advapi32" Alias "RegOpenKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
    'Private Declare Function RegQueryValueEx Lib "advapi32" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
    'Private Declare Function RegCloseKey Lib "advapi32" (ByVal hKey As Integer) As Integer

    'Private Sub cmdSysInfo_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSysInfo.Click
    '    Call StartSysInfo()
    'End Sub

    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
        Me.cmdOK.Enabled = False
        Me.Hide()

        Dim p As New Process
        p = Process.Start(gCmdFile)
        p.WaitForExit()
        Me.cmdOK.Enabled = True


        Main()

   

        If UCase(System.Environment.MachineName) = UCase(SDIS) Then
            ''--- clear Chamber Displau PC Messages from Queue senatevotequeue
            Dim mq As New MessageQueue(gReceiveQueueName)
            Dim msg As New Message
            mq.Purge()
            mq.Close()
        ElseIf UCase(System.Environment.MachineName) = UCase(SVOTE) Then
            '-- clear Vote PC Messages from Queue senatevotequeue
            Dim mq1 As New MessageQueue(gReceiveQueueName)
            Dim msg1 As New Message
            mq1.Purge()
            mq1.Close()


            '-- clear myself Messages from Queue requestvoteidqueue
            Dim mq2 As New MessageQueue(gRequestvoteidqueue)
            Dim msg2 As New Message
            mq2.Purge()
            mq2.Close()
        End If
        frmMain.Show()
    End Sub


    '    Public Sub StartSysInfo()
    '        On Error GoTo SysInfoErr

    '        Dim rc As Integer
    '        Dim SysInfoPath As String = ""

    '        '--- Try To Get System Info Program Path\Name From Registry...
    '        If GetKeyValue(HKEY_LOCAL_MACHINE, gREGKEYSYSINFO, gREGVALSYSINFO, SysInfoPath) Then
    '            '--- Try To Get System Info Program Path Only From Registry...
    '        ElseIf GetKeyValue(HKEY_LOCAL_MACHINE, gREGKEYSYSINFOLOC, gREGVALSYSINFOLOC, SysInfoPath) Then
    '            ' ---Validate Existance Of Known 32 Bit File Version
    '            If Directory.Exists(SysInfoPath & "\MSINFO32.EXE") = True Then
    '                SysInfoPath = SysInfoPath & "\MSINFO32.EXE"
    '                ' Error - File Can Not Be Found...
    '            Else
    '                GoTo SysInfoErr
    '            End If
    '            ' Error - Registry Entry Can Not Be Found...
    '        Else
    '            GoTo SysInfoErr
    '        End If

    '            Call Shell(SysInfoPath, AppWinStyle.NormalFocus)

    '            Exit Sub
    'SysInfoErr:
    '        DisplayMessage("System Information Is Unavailable At This Time", "Senate Voting System", "S")
    '    End Sub

    '    Public Function GetKeyValue(ByRef KeyRoot As Integer, ByRef KeyName As String, ByRef SubKeyRef As String, ByRef KeyVal As String) As Boolean
    '        Dim i As Integer ' Loop Counter
    '        Dim rc As Integer ' Return Code
    '        Dim hKey As Integer ' Handle To An Open Registry Key
    '        Dim KeyValType As Integer ' Data Type Of A Registry Key
    '        Dim tmpVal As String ' Tempory Storage For A Registry Key Value
    '        Dim KeyValSize As Integer ' Size Of Registry Key Variable

    '        Try
    '            '------------------------------------------------------------
    '            ' Open RegKey Under KeyRoot {HKEY_LOCAL_MACHINE...}
    '            '------------------------------------------------------------
    '            rc = RegOpenKeyEx(KeyRoot, KeyName, 0, KEY_ALL_ACCESS, hKey) ' Open Registry Key

    '            If (rc <> ERROR_SUCCESS) Then GoTo GetKeyError ' Handle Error...

    '            tmpVal = New String(Chr(0), 1024) ' Allocate Variable Space
    '            KeyValSize = 1024 ' Mark Variable Size

    '            '------------------------------------------------------------
    '            ' Retrieve Registry Key Value...
    '            '------------------------------------------------------------
    '            rc = RegQueryValueEx(hKey, SubKeyRef, 0, KeyValType, tmpVal, KeyValSize) ' Get/Create Key Value

    '            If (rc <> ERROR_SUCCESS) Then GoTo GetKeyError ' Handle Errors

    '            If (Asc(Mid(tmpVal, KeyValSize, 1)) = 0) Then ' Win95 Adds Null Terminated String...
    '                tmpVal = Microsoft.VisualBasic.Left(tmpVal, KeyValSize - 1) ' Null Found, Extract From String
    '            Else ' WinNT Does NOT Null Terminate String...
    '                tmpVal = Microsoft.VisualBasic.Left(tmpVal, KeyValSize) ' Null Not Found, Extract String Only
    '            End If
    '            '------------------------------------------------------------
    '            ' Determine Key Value Type For Conversion...
    '            '------------------------------------------------------------
    '            Select Case KeyValType ' Search Data Types...
    '                Case REG_SZ ' String Registry Key Data Type
    '                    KeyVal = tmpVal ' Copy String Value
    '                Case REG_DWORD ' Double Word Registry Key Data Type
    '                    For i = Len(tmpVal) To 1 Step -1 ' Convert Each Bit
    '                        KeyVal = KeyVal & Hex(Asc(Mid(tmpVal, i, 1))) ' Build Value Char. By Char.
    '                    Next
    '                    KeyVal = Format("&h" & KeyVal) ' Convert Double Word To String
    '            End Select

    '            GetKeyValue = True ' Return Success
    '            rc = RegCloseKey(hKey) ' Close Registry Key
    '            Exit Function ' Exit

    'GetKeyError:
    '        Catch ' Cleanup After An Error Has Occured...
    '            KeyVal = "" ' Set Return Val To Empty String
    '            GetKeyValue = False ' Return Failure
    '            rc = RegCloseKey(hKey) ' Close Registry Key
    '        End Try

    '    End Function

    
    Private Sub frmAbout_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        lblDate.Text = Date.Now
    End Sub

    Private Sub btnExit_Click(sender As System.Object, e As System.EventArgs)
        End
    End Sub
End Class
