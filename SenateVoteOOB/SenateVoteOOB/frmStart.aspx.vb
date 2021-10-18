Public Class frmStart
    Inherits System.Web.UI.Page

  

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        If cboMode.SelectedItem.Text = "Production" Then
            Response.Redirect("OrderOfBusiness.aspx")
        Else
            Response.Redirect("OrderOfBusinessTest.aspx")
        End If
    End Sub
End Class