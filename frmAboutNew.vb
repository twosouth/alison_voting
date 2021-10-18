

Public Class frmAboutNew
    Private Sub frmAboutNew_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.MdiParent = frmMain
        Me.lblDate.Text = Date.Now

        Dim intX As Integer = Screen.PrimaryScreen.Bounds.Width
        Dim intY As Integer = Screen.PrimaryScreen.Bounds.Height

        Me.Width = intX
        Me.Height = intY

        PictureBox1.Left = (Me.ClientSize.Width - PictureBox1.Width) / 2
        PictureBox1.Top = (Me.ClientSize.Height - PictureBox1.Height) / 2 - 100


        lblTitle.Left = (Me.ClientSize.Width - lblTitle.Width) / 2
        lblTitle.Top = (Me.ClientSize.Height - lblTitle.Height) / 2 + 150


        lblDate.Left = (Me.ClientSize.Width - lblDate.Width) / 2 + 200
        lblDate.Top = (Me.ClientSize.Height - lblDate.Height) / 2 + 350

    End Sub

    Private Sub ChangeSize()
        Me.Width = 1024
        Me.Height = 745
        Me.Top = 0
        Me.Left = 0
        Me.MdiParent = Me

        Me.BringToFront()

    End Sub
End Class
