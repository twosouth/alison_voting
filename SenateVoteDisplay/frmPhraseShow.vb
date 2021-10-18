Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

Public Class frmPhraseShow

    Private dragBounds As Rectangle
    Private dragMethod As String
    Public strSenator, strPhrase As String
    Shadows Event SelectedIndexChanged()

    Private Sub frmPhraseShow_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dc As New DataColumn()

        Me.Top = 343
        Me.Left = 1090
        Me.MdiParent = frmMain
        Try

            LoadPhrasesIntoArray()
            For k As Integer = 1 To gThePhrases.Length - 1
                'lstPhrase.Items.Add(Mid(gThePhrases(k), InStr(gThePhrases(k), " - ") + 3))
                lstPhrase.Items.Add(gThePhrases(k))
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Load Phrase List")
        Finally
            'Connection.Close()
        End Try
    End Sub

    Private Sub lstPhrase_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstPhrase.Click

        bSenator = False
        bPhrase = True
        strPhrase = Mid(Me.lstPhrase.SelectedItem.ToString, InStr(Me.lstPhrase.SelectedItem.ToString, " - ") + 3)

        If tPage0 Then
            tPage0 = True
            tPage1 = False
            tPage2 = False
            DragDrop1 = True
            If Strings.Right(pWorkData1, 2) <> vbCrLf Then
                If pWorkData1 <> "" Then
                    frmChamberDisplay.WorkData.Text = pWorkData1 & vbCrLf & strPhrase
                Else
                    frmChamberDisplay.WorkData.Text = strPhrase
                End If
            Else
                frmChamberDisplay.WorkData.Text = pWorkData1 & strPhrase
            End If
        ElseIf tPage1 Then
            tPage1 = True
            tPage0 = False
            tPage2 = False
            DragDrop2 = True
            If Strings.Right(pWorkData2, 2) <> vbCrLf Then
                If pWorkData2 <> "" Then
                    frmChamberDisplay.WorkData1.Text = pWorkData2 & vbCrLf & strPhrase
                Else
                    frmChamberDisplay.WorkData1.Text = strPhrase
                End If
            Else
                frmChamberDisplay.WorkData1.Text = pWorkData2 & strPhrase
            End If
            'frmChamberDisplay.WorkData2.Text = pWorkData2 & vbCrLf & strPhrase
        ElseIf tPage2 Then
            tPage2 = True
            tPage0 = False
            tPage1 = False
            DragDrop2 = True
            If Strings.Right(pWorkData3, 2) <> vbCrLf Then
                If pWorkData3 <> "" Then
                    frmChamberDisplay.WorkData2.Text = pWorkData3 & vbCrLf & strPhrase
                Else
                    frmChamberDisplay.WorkData2.Text = strPhrase
                End If
            Else
                frmChamberDisplay.WorkData2.Text = pWorkData3 & strPhrase
            End If
        End If

        '  If the user enters something here, then show it in the
        '  first form
        RaiseEvent SelectedIndexChanged()
        'pDragDrop = False
    End Sub

    Private Sub lstPhrase_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstPhrase.MouseDown
        bSenator = False
        bPhrase = True

        ' ----- Prepare the draggable content.
        If (CType(sender, ListBox).SelectedItems.Count = 0) Then Return

        ' ----- Don't start the drag yet. Wait until we move a
        '       certain amount.
        dragBounds = New Rectangle(New Point(e.X - _
           (SystemInformation.DragSize.Width / 2), _
           e.Y - (SystemInformation.DragSize.Height / 2)), _
           SystemInformation.DragSize)
        If (sender Is lstPhrase) Then
            dragMethod = "1to2"
        Else
            dragMethod = "2to1"
        End If

    End Sub

    Private Sub lstPhrase_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstPhrase.MouseMove
        bSenator = False
        bPhrase = True
        'pDragDrop = True

        ' ----- Ignore if not dragging from ListBox1.
        If (dragMethod <> "1to2") Then Return

        ' ----- Have we left the drag boundary?
        If (dragBounds.Contains(e.X, e.Y) = False) Then
            ' ----- Start the drag-and-drop operation.
            If (lstPhrase.DoDragDrop(lstPhrase.SelectedItems, _
                  DragDropEffects.Move) = _
                  DragDropEffects.Move) Then
                ' ----- Successful move. Remove the items from
                '       this list.
                Do While lstPhrase.SelectedItems.Count > 0
                    lstPhrase.Items.Remove(lstPhrase.SelectedItems(0))
                Loop
            End If
            dragMethod = ""
        End If


    End Sub

    Private Sub lstPhrase_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstPhrase.MouseUp
        bSenator = False
        bPhrase = True
        ' ----- End of drag-and-drop.
        dragMethod = ""
    End Sub

  
End Class