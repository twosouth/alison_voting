

Public Class frmAboutNew
    Private Sub frmAbout_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.MdiParent = frmStart
        Me.lblDate.Text = Date.Now
    End Sub
End Class
