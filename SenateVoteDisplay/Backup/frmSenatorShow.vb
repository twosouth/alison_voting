Public Class frmSenatorShow
    Private dragBounds As Rectangle
    Private dragMethod As String
    Public strSenator As String

    Shadows Event SelectedIndexChanged()

    Private Sub frmSenatorShow_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Top = 3
        Me.Left = 1090
        Me.MdiParent = frmStart

        'LoadSenatorsIntoArray()
        For i As Integer = 1 To gSenatorName.Length - 1
            lstSenator.Items.Add("Senator " & gSenatorName(i))
        Next
    End Sub

    Private Sub lstSenator_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstSenator.Click
        strSenator = Me.lstSenator.SelectedItem.ToString

        bSenator = True
        bPhrase = False

        If tPage0 Then
            tPage0 = True
            tPage1 = False
            tPage2 = False

            frmChamberDisplay.InsertIntoWorkData("" & strSenator, gSenatorInsertionPoint)

            'frmChamberDisplay.WorkData.Text = strSenator
        ElseIf tPage1 Then
            tPage1 = True
            tPage0 = False
            tPage2 = False
            frmChamberDisplay.InsertIntoWorkData("" & strSenator, gSenatorInsertionPoint)
            'frmChamberDisplay.WorkData2.Text = strSenator
        ElseIf tPage2 Then
            tPage2 = True
            tPage0 = False
            tPage1 = False
            frmChamberDisplay.InsertIntoWorkData("" & strSenator, gSenatorInsertionPoint)
            'frmChamberDisplay.WorkData3.Text = strSenator
        End If

        '  If the user enters something here, then show it in the
        '  first form
        RaiseEvent SelectedIndexChanged()
    End Sub

    Private Sub lstSenator_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstSenator.MouseDown
        bSenator = True
        bPhrase = False

        ' ----- Prepare the draggable content.
        If (CType(sender, ListBox).SelectedItems.Count = 0) Then Return

        ' ----- Don't start the drag yet. Wait until we move a
        '       certain amount.
        dragBounds = New Rectangle(New Point(e.X - _
           (SystemInformation.DragSize.Width / 2), _
           e.Y - (SystemInformation.DragSize.Height / 2)), _
           SystemInformation.DragSize)
        If (sender Is lstSenator) Then
            dragMethod = "1to2"
        Else
            dragMethod = "2to1"
        End If
    End Sub

    Private Sub lstSenator_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstSenator.MouseMove
        bSenator = True
        bPhrase = False

        ' ----- Ignore if not dragging from ListBox1.
        If (dragMethod <> "1to2") Then Return

        ' ----- Have we left the drag boundary?
        If (dragBounds.Contains(e.X, e.Y) = False) Then
            ' ----- Start the drag-and-drop operation.
            If (lstSenator.DoDragDrop(lstSenator.SelectedItems, _
                  DragDropEffects.Move) = _
                  DragDropEffects.Move) Then
                ' ----- Successful move. Remove the items from
                '       this list.
                Do While lstSenator.SelectedItems.Count > 0
                    lstSenator.Items.Remove(lstSenator.SelectedItems(0))
                Loop
            End If
            dragMethod = ""
        End If
    End Sub

    Private Sub lstSenator_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstSenator.MouseUp
        bSenator = True
        bPhrase = False
        ' ----- End of drag-and-drop.
        dragMethod = ""
    End Sub

End Class