<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSenatorList
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Salutation = New System.Windows.Forms.ListBox()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.lblSalutation = New System.Windows.Forms.Label()
        Me.DGViewSenators = New System.Windows.Forms.DataGridView()
        Me.btnAll = New System.Windows.Forms.Button()
        CType(Me.DGViewSenators, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Salutation
        '
        Me.Salutation.BackColor = System.Drawing.Color.White
        Me.Salutation.Cursor = System.Windows.Forms.Cursors.Default
        Me.Salutation.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Salutation.ForeColor = System.Drawing.Color.Navy
        Me.Salutation.ItemHeight = 18
        Me.Salutation.Items.AddRange(New Object() {"Mr.", "Ms."})
        Me.Salutation.Location = New System.Drawing.Point(9, 34)
        Me.Salutation.Name = "Salutation"
        Me.Salutation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Salutation.Size = New System.Drawing.Size(79, 40)
        Me.Salutation.TabIndex = 7
        '
        'btnExit
        '
        Me.btnExit.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnExit.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnExit.Location = New System.Drawing.Point(184, 893)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnExit.Size = New System.Drawing.Size(77, 35)
        Me.btnExit.TabIndex = 5
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = False
        '
        'lblSalutation
        '
        Me.lblSalutation.BackColor = System.Drawing.Color.LightCyan
        Me.lblSalutation.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSalutation.Font = New System.Drawing.Font("Arial", 10.0!)
        Me.lblSalutation.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSalutation.Location = New System.Drawing.Point(10, 11)
        Me.lblSalutation.Name = "lblSalutation"
        Me.lblSalutation.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSalutation.Size = New System.Drawing.Size(76, 22)
        Me.lblSalutation.TabIndex = 8
        Me.lblSalutation.Text = "Salutation"
        Me.lblSalutation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DGViewSenators
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightYellow
        Me.DGViewSenators.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DGViewSenators.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGViewSenators.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DGViewSenators.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.LightCyan
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DGViewSenators.DefaultCellStyle = DataGridViewCellStyle3
        Me.DGViewSenators.Location = New System.Drawing.Point(92, 11)
        Me.DGViewSenators.Name = "DGViewSenators"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGViewSenators.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.DGViewSenators.RowHeadersWidth = 25
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DGViewSenators.RowsDefaultCellStyle = DataGridViewCellStyle5
        Me.DGViewSenators.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DGViewSenators.RowTemplate.Height = 24
        Me.DGViewSenators.Size = New System.Drawing.Size(292, 877)
        Me.DGViewSenators.TabIndex = 11
        '
        'btnAll
        '
        Me.btnAll.Location = New System.Drawing.Point(7, 86)
        Me.btnAll.Name = "btnAll"
        Me.btnAll.Size = New System.Drawing.Size(81, 30)
        Me.btnAll.TabIndex = 13
        Me.btnAll.Text = "Select All"
        Me.btnAll.UseVisualStyleBackColor = True
        Me.btnAll.Visible = False
        '
        'frmSenatorList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.ControlDark
        Me.ClientSize = New System.Drawing.Size(404, 932)
        Me.Controls.Add(Me.btnAll)
        Me.Controls.Add(Me.DGViewSenators)
        Me.Controls.Add(Me.Salutation)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.lblSalutation)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximizeBox = False
        Me.Name = "frmSenatorList"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Senator Salutations"
        CType(Me.DGViewSenators, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents Salutation As System.Windows.Forms.ListBox
    Public WithEvents btnExit As System.Windows.Forms.Button
    Public WithEvents lblSalutation As System.Windows.Forms.Label
    Friend WithEvents DGViewSenators As System.Windows.Forms.DataGridView
    Friend WithEvents btnAll As System.Windows.Forms.Button
End Class
