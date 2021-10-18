Imports Microsoft.VisualBasic
Imports System.Windows.Forms
Imports System.Text
Imports System.Data
Imports System.IO
Imports System.Data.OleDb
Imports System.ComponentModel

Public Class frmStart
    Dim RetValue As Object

    ' Reg Key Security Options...
    Const READ_CONTROL As Integer = &H20000
    Const KEY_QUERY_VALUE As Short = &H1S
    Const KEY_SET_VALUE As Short = &H2S
    Const KEY_CREATE_SUB_KEY As Short = &H4S
    Const KEY_ENUMERATE_SUB_KEYS As Short = &H8S
    Const KEY_NOTIFY As Short = &H10S
    Const KEY_CREATE_LINK As Short = &H20S
    Const KEY_ALL_ACCESS As Double = KEY_QUERY_VALUE + KEY_SET_VALUE + KEY_CREATE_SUB_KEY + KEY_ENUMERATE_SUB_KEYS + KEY_NOTIFY + KEY_CREATE_LINK + READ_CONTROL

    ' Reg Key ROOT Types...
    Const HKEY_LOCAL_MACHINE As Integer = &H80000002
    Const ERROR_SUCCESS As Short = 0
    Const REG_SZ As Short = 1 ' Unicode nul terminated string
    Const REG_DWORD As Short = 4 ' 32-bit number

    Const gREGKEYSYSINFOLOC As String = "SOFTWARE\Microsoft\Shared Tools Location"
    Const gREGVALSYSINFOLOC As String = "MSINFO"
    Const gREGKEYSYSINFO As String = "SOFTWARE\Microsoft\Shared Tools\MSINFO"
    Const gREGVALSYSINFO As String = "PATH"

    Private Declare Function RegOpenKeyEx Lib "advapi32" Alias "RegOpenKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
    Private Declare Function RegQueryValueEx Lib "advapi32" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
    Private Declare Function RegCloseKey Lib "advapi32" (ByVal hKey As Integer) As Integer



    Private Sub frmStart_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Main()
        'lblDay.Text = "Today is: " & Now.ToShortDateString & " " & Now.ToShortTimeString
        frmAboutNew.Show()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub frmStart_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Me.MaximizeBox = True
        Me.IsMdiContainer = True
        Me.BringToFront()
    End Sub

    Private Sub ChangeSize(ByVal frm As Windows.Forms.Form)
        Me.Width = 1024
        Me.Height = 745
        Me.Top = 0
        Me.Left = 0
        frm.MdiParent = Me
        frm.Show()
        frm.BringToFront()
    End Sub

    Private Sub uTScpm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTScpm.Click
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmPhraseList
        ChangeSize(frm)
        frmAboutNew.Close()
    End Sub

    Private Sub uTSsenator_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSsenator.Click
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmSenatorList
        ChangeSize(frm)
        frmAboutNew.Close()
    End Sub

    Private Sub uTSparameter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSparameter.Click
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmVotingParameterMaintenance
        ChangeSize(frm)
        frmAboutNew.Close()
    End Sub

    Private Sub uTSupdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSupdate.Click
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmDisplayBackGround
        ChangeSize(frm)
        frmAboutNew.Close()
    End Sub

    Private Sub uTSboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSboard.Click
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmBoardAndCommissionListNew
        ChangeSize(frm)
        frmAboutNew.Close()
    End Sub


    Private Sub uTSMotion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSMotion.Click
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmMotionList
        ChangeSize(frm)
        frmAboutNew.Close()
    End Sub

    Private Sub reportTSroll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles reportTSroll.Click
        Dim v_id As String
        v_id = Trim(Mid(reportTSvoteID.Text, 9))
        If v_id = "" Or Val(v_id) = 0 Then
            MsgBox("Invalid Vote ID", MsgBoxStyle.Critical, "Senate Voting System")
        Else

        End If
    End Sub

    Private Sub helpTSabout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles helpTSabout.Click
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmAbout
        ChangeSize(frm)
        frmAboutNew.Close()
    End Sub

    Private Sub helpTSsystem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles helpTSsystem.Click
        Me.WindowState = FormWindowState.Maximized
        StartSysInfo()
    End Sub


    Public Sub StartSysInfo()
        On Error GoTo SysInfoErr

        Dim rc As Integer
        Dim SysInfoPath As String

        ' Try To Get System Info Program Path\Name From Registry...
        If GetKeyValue(HKEY_LOCAL_MACHINE, gREGKEYSYSINFO, gREGVALSYSINFO, SysInfoPath) Then
            ' Try To Get System Info Program Path Only From Registry...
        ElseIf GetKeyValue(HKEY_LOCAL_MACHINE, gREGKEYSYSINFOLOC, gREGVALSYSINFOLOC, SysInfoPath) Then
            ' Validate Existance Of Known 32 Bit File Version
            If Directory.Exists(SysInfoPath & "\MSINFO32.EXE") = True Then
                SysInfoPath = SysInfoPath & "\MSINFO32.EXE"
                ' Error - File Can Not Be Found...
            Else
                GoTo SysInfoErr
            End If
            ' Error - Registry Entry Can Not Be Found...
        Else
            GoTo SysInfoErr
        End If

        Call Shell(SysInfoPath, AppWinStyle.NormalFocus)

        Exit Sub
SysInfoErr:
        MsgBox("System Information Is Unavailable At This Time", MsgBoxStyle.OkOnly)
    End Sub

    Public Function GetKeyValue(ByRef KeyRoot As Integer, ByRef KeyName As String, ByRef SubKeyRef As String, ByRef KeyVal As String) As Boolean
        Dim i As Integer                                        ' Loop Counter
        Dim rc As Integer                                       ' Return Code
        Dim hKey As Integer                                     ' Handle To An Open Registry Key
        Dim KeyValType As Integer                               ' Data Type Of A Registry Key
        Dim tmpVal As String                                    ' Tempory Storage For A Registry Key Value
        Dim KeyValSize As Integer                               ' Size Of Registry Key Variable

        Try
            '------------------------------------------------------------
            ' Open RegKey Under KeyRoot {HKEY_LOCAL_MACHINE...}
            '------------------------------------------------------------
            rc = RegOpenKeyEx(KeyRoot, KeyName, 0, KEY_ALL_ACCESS, hKey) ' Open Registry Key

            If (rc <> ERROR_SUCCESS) Then GoTo GetKeyError ' Handle Error...

            tmpVal = New String(Chr(0), 1024)                   ' Allocate Variable Space
            KeyValSize = 1024                                   ' Mark Variable Size

            '------------------------------------------------------------
            ' Retrieve Registry Key Value...
            '------------------------------------------------------------
            rc = RegQueryValueEx(hKey, SubKeyRef, 0, KeyValType, tmpVal, KeyValSize) ' Get/Create Key Value

            If (rc <> ERROR_SUCCESS) Then GoTo GetKeyError ' Handle Errors

            If (Asc(Mid(tmpVal, KeyValSize, 1)) = 0) Then       ' Win95 Adds Null Terminated String...
                tmpVal = Microsoft.VisualBasic.Left(tmpVal, KeyValSize - 1) ' Null Found, Extract From String
            Else                                                ' WinNT Does NOT Null Terminate String...
                tmpVal = Microsoft.VisualBasic.Left(tmpVal, KeyValSize) ' Null Not Found, Extract String Only
            End If
            '------------------------------------------------------------
            ' Determine Key Value Type For Conversion...
            '------------------------------------------------------------
            Select Case KeyValType                              ' Search Data Types...
                Case REG_SZ                                     ' String Registry Key Data Type
                    KeyVal = tmpVal                             ' Copy String Value
                Case REG_DWORD                                  ' Double Word Registry Key Data Type
                    For i = Len(tmpVal) To 1 Step -1            ' Convert Each Bit
                        KeyVal = KeyVal & Hex(Asc(Mid(tmpVal, i, 1))) ' Build Value Char. By Char.
                    Next
                    KeyVal = Format("&h" & KeyVal)              ' Convert Double Word To String
            End Select

            GetKeyValue = True                                  ' Return Success
            rc = RegCloseKey(hKey)                              ' Close Registry Key
            Exit Function                                       ' Exit

GetKeyError:
        Catch                                                   ' Cleanup After An Error Has Occured...
            KeyVal = ""                                         ' Set Return Val To Empty String
            GetKeyValue = False                                 ' Return Failure
            rc = RegCloseKey(hKey)                              ' Close Registry Key
        End Try

    End Function

    Private Sub ReportsTS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReportsTS.Click
        reportTSvoteID.Text = "Vote ID:"
    End Sub

    Private Sub StartTS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartTS.Click
        Me.WindowState = FormWindowState.Maximized
        frmPhraseShow.Show()
        frmSenatorShow.Show()
        Dim frm As New frmChamberDisplay
        ChangeSize(frmChamberDisplay)
        frm.TC.SelectedIndex = 0
        tPage0 = True
        tPage1 = False
        tPage2 = False
    End Sub

    Private Sub uTSdownloadS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSdownloadS.Click
        Dim i As Short
        Dim dsSenator As New DataSet
        Dim dsDistrict As New DataSet
        Dim drSenator As DataRow

        If Not DisplayMessage("You are about to download senators from ALIS.  " & "Do you want to continue?", "Download Senators From ALIS", "Y") Then
            Exit Sub
        End If

        'Clear tblSenators table first
        V_DataSet("Delete From tblSenators", "D")

     
        ' senators are read in by their distric #, so, the first one read is
        ' from district 1 and gets assigned as senator #1;
        ' also read another ALIS sql stmt to get the oid of the district which
        ' is used when the votes are recorded; oids are read in the same
        ' order such that the first oid is assigned to the now senator #1

        i = 0
        Dim strSql As String
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
        dsSenator = A_DataSet(strSql, "R")

        strSql = ""
        For Each drSenator In dsSenator.Tables(0).Rows
            strSql = "Insert Into tblSenators Values ('" & drSenator("SenatorName") & "', " & drSenator("SenatorOID") & ", '', " & i + 1 & ", " & drSenator("DistrictOID") & ")"
            V_DataSet(strSql, "A")
            i += 1
        Next

        LoadSenatorsIntoArray()

        RetValue = DisplayMessage("Senator download complete.  Don't forget to add the salutations.", "", "I")
    End Sub

    Private Sub uTSdownloadC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSdownloadC.Click
        Dim dsALIS As New DataSet
        Dim strSql As String
        Dim dr As DataRow
        Dim ds As New DataSet

        If Not DisplayMessage("You are about to download committees from ALIS.  " & "Do you want to continue?", "Download Committees From ALIS", "Y") Then
            Exit Sub
        End If

        V_DataSet("Delete From tblCommittees", "D")

        strSql = "SELECT COMMITTEE.NAME AS NAME, COMMITTEE.ABBREVIATION AS ABBREV " & _
                " FROM ORGANIZATION COMMITTEE, CODE_VALUES CV, ORGANIZATION HOUSE " & _
                " WHERE COMMITTEE.TYPE_CODE = CV.CODE AND COMMITTEE.OID_PARENT_ORGANIZATION = HOUSE.OID AND (COMMITTEE.COMMITTEE = 'Y') " & _
                " AND (CV.VALUE = 'Standing Committee') AND (HOUSE.NAME = 'Senate') AND (COMMITTEE.EXPIRATION_DATE IS NULL OR COMMITTEE.EXPIRATION_DATE > (SELECT MIN(D.CALENDAR_DATE) " & _
                " FROM LEGISLATIVE_DAY D WHERE D.OID_SESSION = " & gSessionID & ")) ORDER BY COMMITTEE.ABBREVIATION"
        dsALIS = A_DataSet(strSql, "R")

        For Each dr In dsALIS.Tables(0).Rows
            V_DataSet("Insert Into tblCommittees Values('" & dr("ABBREV") & "', '" & dr("NAME") & "')", "A")
        Next
    
        LoadCommitteesIntoArray()

        RetValue = DisplayMessage("Committee download complete", "", "I")
    End Sub

    Private Sub uTSdelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSdelete.Click
        Dim dsC As New DataSet
        Dim dsCG As New DataSet
       
        If DisplayMessage("You are about to delete all special order calendars.  Do you want to continue?", "Delete Special Order Calendars", "Y") Then

            V_DataSet("Delete From tblCalendars Where LEFT(CalendarCode, 2) ='SR'", "D")

            dsC = V_DataSet("Select * From tblCalendars Order By CalendarCode", "R")
           
            ''frmChamberDisplay.Calendar.DataSource = Nothing
            ''frmChamberDisplay.Calendar.Items.Clear()             
        End If
    End Sub

    Private Sub uTSdownloadB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uTSdownloadB.Click
        If DisplayMessage("You are about to download bills from ALIS. " & "Do you want to continue?", "Download Bills From ALIS", "Y") Then
            RetValue = DownloadBillsFromALIS()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As Windows.Forms.Form

        '   close all of the opening child windows
        For Each f In Me.MdiChildren
            Dim pcount As Integer = Me.MdiChildren.GetLength(0)
            f.Close()
            If pcount = Me.MdiChildren.GetLength(0) Then
                Exit For
            Else
                f.Dispose()
            End If
        Next

        '   set frmStar form back to centre of screen
        Me.Width = 628
        Me.Height = 451
        Me.Left = 200
        Me.Top = 150

    End Sub

   
    Private Sub uTSVPMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim frm As New frmVotingParameterMaintenance
        ChangeSize(frm)
        frmAboutNew.Close()
    End Sub

    Private Sub AppExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AppExit.Click
        Application.Exit()
    End Sub

    Private Sub SenatorList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SenatorList.Click
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmSenatorShow
        ChangeSize(frm)
        frmAboutNew.Close()
    End Sub

    Private Sub PhraseList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PhraseList.Click
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmPhraseShow
        ChangeSize(frm)
        frmAboutNew.Close()
    End Sub


    Private Sub SpecialOCalendar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SpecialOCalendar.Click
        Me.WindowState = FormWindowState.Maximized
        Dim frm As New frmPreSOC
        ChangeSize(frm)
        frmAboutNew.Close()
    End Sub
End Class