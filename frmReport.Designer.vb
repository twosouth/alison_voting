<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.CRView = New CrystalDecisions.Windows.Forms.CrystalReportViewer()
        Me.SuspendLayout()
        '
        'CRView
        '
        Me.CRView.ActiveViewIndex = -1
        Me.CRView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.CRView.Cursor = System.Windows.Forms.Cursors.Default
        Me.CRView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CRView.Location = New System.Drawing.Point(0, 0)
        Me.CRView.Name = "CRView"
        Me.CRView.Size = New System.Drawing.Size(1002, 735)
        Me.CRView.TabIndex = 30
        Me.CRView.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
        '
        'frmReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1002, 735)
        Me.Controls.Add(Me.CRView)
        Me.MaximizeBox = False
        Me.Name = "frmReport"
        Me.Text = "Vote ID Report"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CRView As CrystalDecisions.Windows.Forms.CrystalReportViewer
End Class
