Imports System
Imports System.Windows.Forms
Imports System.Data
Imports System.Data.OleDb
Imports System.Exception

Public Class frmPreSOC
    Private frmOpen As Boolean

    Private Sub frmPreSOC_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.MdiParent = frmStart
      
        LoadCalendar()

        lstCalendar.Items.Add("SB1")
        lstCalendar.Items.Add("SB12")
    End Sub

    Private Sub LoadCalendar()
        Dim ds As New DataSet
        Dim strSql As String

        Try
            strSql = "SELECT label, sponsor, alis_object.index_word, calendar_page " & _
                     " FROM alis_object, matter " & _
                     " WHERE matter.oid_instrument = alis_object.oid AND matter.oid_session = " & gSessionID & " AND alis_object.oid_session = " & gSessionID & " AND matter.matter_status_code = 'Pend' " & _
                     " AND matter.oid_legislative_body = '1753' AND alis_object.legislative_body = 'S' AND matter.calendar_sequence_no > 0 " & _
                     " ORDER BY matter.calendar_sequence_no"
            ds = A_DataSet(strSql, "R")

            For Each dr In ds.Tables(0).Rows
                lstCalendar.Items.Add(dr("label"))
            Next

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, msgText)
        End Try
    End Sub

    Private Sub btnRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRight.Click
        If lstCalendar.SelectedIndex <> -1 Then
            If SOCBillExist(Me.lstCalendar.Items(lstCalendar.SelectedIndex).ToString) = False Then
                lstSCalendar.Items.Add(lstCalendar.SelectedItem.ToString)
                If lstCalendar.SelectedIndex < lstCalendar.Items.Count - 1 Then
                    lstCalendar.SelectedIndex = lstCalendar.SelectedIndex + 1
                End If
            End If
        End If
    End Sub

    Private Sub btnLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeft.Click
        'lstCalendar.Items.RemoveAt(lstSCalendar.SelectedIndex)
        If lstSCalendar.SelectedIndex <> -1 Then
            lstSCalendar.Items.Remove(lstSCalendar.SelectedItem.ToString)
        End If
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Me.lstSCalendar.Items.Clear()
        txtSearch.Text = ""
        txtSearch.Focus()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnPUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPUp.Click
        Dim item1, item2 As String
        Dim index1, index2 As Integer

        If lstSCalendar.SelectedIndex <> -1 And lstSCalendar.SelectedIndex <> 0 Then
            'Highlined item
            item1 = Me.lstSCalendar.SelectedItem.ToString
            index1 = Me.lstSCalendar.SelectedIndex

            index2 = index1 - 1
            item2 = Me.lstSCalendar.Items(index2).ToString

            Me.lstSCalendar.Items.Item(index2) = item1
            Me.lstSCalendar.Items.Item(index1) = item2

            index1 = 0
            index2 = 0

            Me.lstSCalendar.SelectedIndex = Me.lstSCalendar.SelectedIndex - 1
        End If
    End Sub

    Private Sub btnPDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPDown.Click
        Dim item1, item2 As String
        Dim index1, index2 As Integer

        If lstSCalendar.SelectedIndex <> -1 And lstSCalendar.SelectedIndex < lstSCalendar.Items.Count Then
            'Highlined item
            item1 = Me.lstSCalendar.SelectedItem.ToString
            index1 = Me.lstSCalendar.SelectedIndex

            index2 = index1 + 1
            If index2 < Me.lstSCalendar.Items.Count Then
                item2 = Me.lstSCalendar.Items(index2).ToString

                Me.lstSCalendar.Items.Item(index2) = item1
                Me.lstSCalendar.Items.Item(index1) = item2

                index1 = 0
                index2 = 0

                ' Highline 
                Me.lstSCalendar.SelectedIndex = Me.lstSCalendar.SelectedIndex + 1
            End If
        End If
    End Sub

    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click
        If lstCalendar.SelectedIndex <> -1 And lstCalendar.SelectedIndex <= lstCalendar.Items.Count - 1 Then
            Me.lstCalendar.SelectedIndex = Me.lstCalendar.SelectedIndex + 1
        End If
    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        If lstCalendar.SelectedIndex <> -1 And lstCalendar.SelectedIndex <> 0 Then
            Me.lstCalendar.SelectedIndex = Me.lstCalendar.SelectedIndex - 1
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim sw As New Stopwatch

        If lstSCalendar.Items.Count <> 0 Then
            ' Finding out 'Order Of Business' window does open or close 
            sw.Start()
            For i As Int16 = 0 To My.Application.OpenForms.Count - 1
                If My.Application.OpenForms.Item(i).Text = "Senate Voting System - Order Of Business" Then
                    frmOpen = True
                End If
            Next
            sw.Stop()


            ' if 'Order Of Business' window is close, open it first
            If frmOpen = False Then
                frmChamberDisplay.Show()
                ' Me.BringToFront()
            End If

            ' paste Special Order Calendar List Bills to 'Order Of Business' window
            frmChamberDisplay.Bill.Items.Clear()
            For j As Integer = 0 To Me.lstSCalendar.Items.Count - 1
                frmChamberDisplay.Bill.Items.Add(Me.lstSCalendar.Items(j).ToString)
            Next
            frmChamberDisplay.Calendar.Items.Add("Special Order Calendar")

            Me.lstSCalendar.Items.Clear()
        End If
    End Sub

    Private Sub txtSearch_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.MouseLeave
        Dim ds, dsA As New DataSet
        Dim dr As DataRow
        Dim strSql As String
        Dim i As Integer
        SOCBillExist(txtSearch.Text)
        Try
            If lstCalendar.Items.Count <> 0 Then
                strSql = "SELECT label, sponsor, alis_object.index_word, calendar_page " & _
                             " FROM alis_object, matter " & _
                             " WHERE matter.oid_instrument = alis_object.oid AND matter.oid_session = " & gSessionID & " AND alis_object.oid_session = " & gSessionID & " AND matter.matter_status_code = 'Pend' " & _
                             " AND matter.oid_legislative_body = '1753' AND alis_object.legislative_body = 'S' AND matter.calendar_sequence_no > 0 " & _
                             " ORDER BY matter.calendar_sequence_no"
                ds = A_DataSet(strSql, "R")

                For Each dr In ds.Tables(0).Rows
                    If Trim(UCase(txtSearch.Text)) = UCase(dr("label")) Then
                        ' if bill found, highline it
                        If UCase(Me.lstCalendar.Items(i).ToString) = dr("label") Then
                            Me.lstCalendar.SelectedIndex = i
                            ' add bill to right list box
                            Me.lstSCalendar.Items.Add(dr("label"))
                        Else
                            txtSearch.Text = ""
                            txtSearch.Focus()
                        End If


                    End If
                    i += 1
                Next

                txtSearch.Text = ""
                txtSearch.Focus()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, msgText)
        End Try
    End Sub

    Private Function SOCBillExist(ByVal SOCBill As String) As Boolean
        If Me.lstSCalendar.Items.Count > 0 Then
            For j As Integer = 0 To Me.lstSCalendar.Items.Count - 1
                If UCase(SOCBill) = Me.lstSCalendar.Items(j).ToString Then
                    MsgBox(SOCBill & " is existed.", MsgBoxStyle.Information)
                    SOCBillExist = True
                    Exit Function
                End If
            Next
        Else
            SOCBillExist = False
        End If
    End Function

    Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged

    End Sub
End Class