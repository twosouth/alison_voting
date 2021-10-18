
Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic.VBCodeProvider
Imports System
Imports System.Text

Module basErrorHandler
    ' Define the maximum number of procedure names held in the stack
    Const ERHMaxItems As Short = 20

    ' Array to simulate a stack of procedure names
    Public ERHStack(ERHMaxItems) As String

    ' Pointer to the current position in the stack
    Public ERHPointer As Short

    ' Set the following to True to included expanded system information in the log file
    Const ERHExpandedInfo As Boolean = False

    ' Set the following to True to display an error dialog in the event of an error
    Const ERHDisplayError As Boolean = True

    ' Set the following to true to log errors to a table
    Const ERHLogToTable As Boolean = False

    ' Set the following the the table to log errors to
    ' (if ERHLogToTable is True)
    Const ERHLogTable As String = "tblErrors"

    ' Set the following the path and name of the file to log errors to
    ' (if ERHLogToTable is False)
    Public ERHLogFile, UserDesc As String



    Sub ERHHandler()
        ' Comments  : the master error handler
        ' Parameters: none
        ' Returns   : nothing
        '
        Dim i As Short
        Dim strTemp As String
        Dim ErrorNbr As Integer
        Dim ErrorMsg As String

        Const MBStop As Short = 16
        Const AppErr As String = "Application-defined or object-defined error"

        ' capture a meaningful error
        ErrorNbr = Err.Number
        If (Err.Description <> AppErr) And (NToB(Err.Description) <> "") Then
            ErrorMsg = Err.Description
        Else
            ErrorMsg = ErrorToString(ErrorNbr)
        End If


        ' Since we don't know what state we are in, disable error trapping
        On Error Resume Next

        If ERHDisplayError Then
            Beep()
            strTemp = "The following error has occurred in procedure: " & Chr(13) & Chr(10)
            strTemp = strTemp & ERHStack(ERHPointer)
            strTemp = strTemp & " on line " & IIf(IsDBNull(Erl()), "<unknown>", Erl()) & Chr(13) & Chr(10)
            strTemp = strTemp & "Error (" & Format(ErrorNbr) & ") - " & ErrorMsg

            MsgBox(strTemp, MsgBoxStyle.Critical, "Database Error - Please Contact Support")
        End If

        ' what was the user doing
        UserDesc = ""

        Call ERHLogErrorToFile(ErrorMsg, ErrorNbr, Erl(), ERHLogFile, ERHExpandedInfo)

        gErrorCount = gErrorCount + 1
        If gErrorCount > 0 Then
            End
        Else
            Main()
        End If

    End Sub

    Function ERHInitialize() As Short
        ' Comments  : Initializes the error handler
        ' Parameters: None
        ' Returns   : True if successful, False otherwise
        '
        On Error GoTo ERHInitializeERR

        ' ERHLogFile = GetPath() & "\IRCError.txt"

        ' Initialize the stack
        System.Array.Clear(ERHStack, 0, ERHStack.Length)
        ERHPointer = LBound(ERHStack)

        ERHInitialize = True

ERHInitializeEXIT:
        Exit Function

ERHInitializeERR:
        ERHInitialize = False
        Resume ERHInitializeEXIT

    End Function


    Sub ERHPopStack()

        ERHStack(ERHPointer) = ERHStack(ERHPointer) & " - Completed"

    End Sub

    Sub ERHPushStack(ByRef strProc As String)
        ' Comments  : Pushes the supplied procedure name onto the error handling stack
        ' Parameters: strProc - name of the currently executing procedure
        ' Returns   : Nothing
        '

        Dim i As Short

        ERHPointer = ERHPointer + 1

        If ERHPointer <= ERHMaxItems Then
            ERHStack(ERHPointer) = strProc
        Else
            For i = 2 To ERHMaxItems
                ERHStack(i - 1) = ERHStack(i)
            Next i
            ERHPointer = ERHMaxItems
            ERHStack(ERHPointer) = strProc
        End If

    End Sub


    Sub ERHLogErrorToFile(ByRef strError As String, ByRef intError As Integer, ByRef intErl As Short, ByRef strFile As String, ByRef fExpandedStats As Short)
        ' Comments  : Logs the most recent error to a file
        ' Parameters: strError - error string
        '             intError - error number
        '             intErl - error line number
        '             strFile - name of the file to log the errors to
        '             fExpandedStats - True to include additional system information, False otherwise
        ' Returns   : nothing
        '
        Dim intFIle As Short
        Dim intCounter As Short
        Dim varTemp As Object

        intFIle = FreeFile()
        FileOpen(intFIle, strFile, OpenMode.Append)

        PrintLine(intFIle, "Access Application Error Information")
        PrintLine(intFIle, "====================================")
        PrintLine(intFIle, "Current Time    : " & Now)
        PrintLine(intFIle, "Error String    : " & Left(strError, 255))
        PrintLine(intFIle, "Error Number    : " & intError)
        PrintLine(intFIle, "Error Line      : " & intErl)
        PrintLine(intFIle, "Error Procedure : " & ERHStack(ERHPointer))
        PrintLine(intFIle, "User Description: " & UserDesc)
        PrintLine(intFIle)
        PrintLine(intFIle, "Procedure Stack")
        PrintLine(intFIle, "---------------")

        For intCounter = LBound(ERHStack) To UBound(ERHStack)
            If ERHStack(intCounter) <> "" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object varTemp. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                varTemp = varTemp & Format(intCounter, "00") & " - " & ERHStack(intCounter) & Chr(13) & Chr(10)
            End If
        Next intCounter

        PrintLine(intFIle, varTemp)


        ' Close the objects
        FileClose(intFIle)

    End Sub

End Module
