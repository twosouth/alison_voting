Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class frmReport
    Declare Function WriteProfileString Lib "kernel32" _
                 Alias "WriteProfileStringA" _
                 (ByVal lpszSection As String, _
                 ByVal lpszKeyName As String, _
                 ByVal lpszString As String) As Long

    Private Sub frmReport_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        LoadReport("RPTPASSVOTEID.RPT", v_id)
    End Sub

    Private Sub LoadReport(rptName As String, intVoteID As Integer)
        Try
            Dim reportPath As String = gVotingPath & rptName
            Dim reportDocument As New ReportDocument()
            Dim paramFields As New ParameterFields()
            Dim paramField1 As New ParameterField()
            Dim paramDiscreteValue1 As New ParameterDiscreteValue()
            Dim paramField2 As New ParameterField()
            Dim paramDiscreteValue2 As New ParameterDiscreteValue()
            Dim paramField3 As New ParameterField()
            Dim paramDiscreteValue3 As New ParameterDiscreteValue()

            paramField1.Name = "Vote ID"
            paramDiscreteValue1.Value = intVoteID
            paramField1.CurrentValues.Add(paramDiscreteValue1)
            paramFields.Add(paramField1)


            paramField2.Name = "Session Name"
            paramDiscreteValue2.Value = gSessionName
            paramField2.CurrentValues.Add(paramDiscreteValue2)
            paramFields.Add(paramField2)

            paramField3.Name = "sub Vote ID"
            paramDiscreteValue3.Value = intVoteID
            paramField3.CurrentValues.Add(paramDiscreteValue3)
            paramFields.Add(paramField3)

            CRView.ParameterFieldInfo = paramFields
            reportDocument.Load(reportPath)
            CRView.ReportSource = reportDocument

            CRView.ShowCloseButton = True
        Catch EX As Exception
            DisplayMessage("Failed Print Vote ID Report.", "Print Vote ID Report", "S")
            Me.Close()
        End Try
    End Sub
    Private Sub SetDefaultPrinter(PrinterName As String, DrivaerName As String, PrinterPort As String)
        Dim DeviceLine As String
        Dim r As Long

        DeviceLine = PrinterName & "," & DrivaerName & "," & PrinterPort

        '--- Store the new printer informationin the [WINDOWS] section of
        '--- the WIN.INI file for the DEVICE = item
        r = WriteProfileString("windows", "Device", DeviceLine)
        '--- Cause all applications to reload the INI file:
        '--- l = SendMessage(HWND_BROADCAST, WM_WININICHANGE, 0, "windows")
    End Sub
End Class