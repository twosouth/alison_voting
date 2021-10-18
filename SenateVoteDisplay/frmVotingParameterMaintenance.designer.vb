<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVotingParameterMaintenance
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
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnFillGrid = New System.Windows.Forms.Button()
        Me.tbPageSize = New System.Windows.Forms.TextBox()
        Me.lblPageSize = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.userDataGridView = New System.Windows.Forms.DataGridView()
        Me.btnLast = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnPrevious = New System.Windows.Forms.Button()
        Me.btnFirst = New System.Windows.Forms.Button()
        Me.btndelete = New System.Windows.Forms.Button()
        CType(Me.userDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnClose.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnClose.Location = New System.Drawing.Point(568, 912)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(2)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(85, 33)
        Me.btnClose.TabIndex = 39
        Me.btnClose.Text = "C&lose"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnCancel.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnCancel.ForeColor = System.Drawing.Color.Black
        Me.btnCancel.Location = New System.Drawing.Point(392, 912)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(85, 33)
        Me.btnCancel.TabIndex = 36
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnUpdate
        '
        Me.btnUpdate.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnUpdate.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnUpdate.ForeColor = System.Drawing.Color.Black
        Me.btnUpdate.Location = New System.Drawing.Point(102, 912)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(85, 33)
        Me.btnUpdate.TabIndex = 41
        Me.btnUpdate.Text = "&Save"
        Me.btnUpdate.UseVisualStyleBackColor = False
        Me.btnUpdate.Visible = False
        '
        'btnPrint
        '
        Me.btnPrint.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnPrint.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnPrint.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnPrint.Location = New System.Drawing.Point(480, 912)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnPrint.Size = New System.Drawing.Size(85, 33)
        Me.btnPrint.TabIndex = 42
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'btnFillGrid
        '
        Me.btnFillGrid.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnFillGrid.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFillGrid.Location = New System.Drawing.Point(905, 12)
        Me.btnFillGrid.Margin = New System.Windows.Forms.Padding(2)
        Me.btnFillGrid.Name = "btnFillGrid"
        Me.btnFillGrid.Size = New System.Drawing.Size(63, 28)
        Me.btnFillGrid.TabIndex = 76
        Me.btnFillGrid.Text = "Fill Grid"
        Me.btnFillGrid.Visible = False
        '
        'tbPageSize
        '
        Me.tbPageSize.BackColor = System.Drawing.Color.Ivory
        Me.tbPageSize.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbPageSize.Location = New System.Drawing.Point(863, 13)
        Me.tbPageSize.Margin = New System.Windows.Forms.Padding(2)
        Me.tbPageSize.Name = "tbPageSize"
        Me.tbPageSize.Size = New System.Drawing.Size(44, 29)
        Me.tbPageSize.TabIndex = 77
        Me.tbPageSize.Text = "40"
        Me.tbPageSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.tbPageSize.Visible = False
        '
        'lblPageSize
        '
        Me.lblPageSize.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPageSize.ForeColor = System.Drawing.Color.Yellow
        Me.lblPageSize.Location = New System.Drawing.Point(769, 15)
        Me.lblPageSize.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblPageSize.Name = "lblPageSize"
        Me.lblPageSize.Size = New System.Drawing.Size(90, 20)
        Me.lblPageSize.TabIndex = 78
        Me.lblPageSize.Text = "&Page Size:"
        Me.lblPageSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblPageSize.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.Ivory
        Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblStatus.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.lblStatus.Location = New System.Drawing.Point(460, 15)
        Me.lblStatus.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(63, 25)
        Me.lblStatus.TabIndex = 75
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblStatus.Visible = False
        '
        'userDataGridView
        '
        Me.userDataGridView.AllowUserToOrderColumns = True
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Ivory
        Me.userDataGridView.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.userDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.userDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.userDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.userDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised
        Me.userDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.LightCyan
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.userDataGridView.DefaultCellStyle = DataGridViewCellStyle2
        Me.userDataGridView.Location = New System.Drawing.Point(16, 44)
        Me.userDataGridView.Margin = New System.Windows.Forms.Padding(2)
        Me.userDataGridView.MultiSelect = False
        Me.userDataGridView.Name = "userDataGridView"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Arial", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.userDataGridView.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.userDataGridView.RowHeadersWidth = 21
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Arial", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.userDataGridView.RowsDefaultCellStyle = DataGridViewCellStyle4
        Me.userDataGridView.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Arial Narrow", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.userDataGridView.RowTemplate.Height = 24
        Me.userDataGridView.Size = New System.Drawing.Size(950, 863)
        Me.userDataGridView.TabIndex = 74
        '
        'btnLast
        '
        Me.btnLast.BackColor = System.Drawing.Color.Ivory
        Me.btnLast.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.btnLast.Location = New System.Drawing.Point(568, 15)
        Me.btnLast.Name = "btnLast"
        Me.btnLast.Size = New System.Drawing.Size(33, 24)
        Me.btnLast.TabIndex = 73
        Me.btnLast.Text = ">|"
        Me.btnLast.UseVisualStyleBackColor = False
        Me.btnLast.Visible = False
        '
        'btnNext
        '
        Me.btnNext.BackColor = System.Drawing.Color.Ivory
        Me.btnNext.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.btnNext.Location = New System.Drawing.Point(529, 15)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(33, 24)
        Me.btnNext.TabIndex = 72
        Me.btnNext.Text = ">"
        Me.btnNext.UseVisualStyleBackColor = False
        Me.btnNext.Visible = False
        '
        'btnPrevious
        '
        Me.btnPrevious.BackColor = System.Drawing.Color.Ivory
        Me.btnPrevious.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.btnPrevious.Location = New System.Drawing.Point(422, 15)
        Me.btnPrevious.Name = "btnPrevious"
        Me.btnPrevious.Size = New System.Drawing.Size(33, 24)
        Me.btnPrevious.TabIndex = 71
        Me.btnPrevious.Text = "<"
        Me.btnPrevious.UseVisualStyleBackColor = False
        Me.btnPrevious.Visible = False
        '
        'btnFirst
        '
        Me.btnFirst.BackColor = System.Drawing.Color.Ivory
        Me.btnFirst.Font = New System.Drawing.Font("Arial", 9.0!)
        Me.btnFirst.Location = New System.Drawing.Point(383, 15)
        Me.btnFirst.Name = "btnFirst"
        Me.btnFirst.Size = New System.Drawing.Size(33, 24)
        Me.btnFirst.TabIndex = 70
        Me.btnFirst.Text = "|<"
        Me.btnFirst.UseVisualStyleBackColor = False
        Me.btnFirst.Visible = False
        '
        'btndelete
        '
        Me.btndelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btndelete.Font = New System.Drawing.Font("Arial", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btndelete.ForeColor = System.Drawing.Color.Black
        Me.btndelete.Location = New System.Drawing.Point(302, 912)
        Me.btndelete.Name = "btndelete"
        Me.btndelete.Size = New System.Drawing.Size(85, 33)
        Me.btndelete.TabIndex = 79
        Me.btndelete.Text = "&Delete"
        Me.btndelete.UseVisualStyleBackColor = False
        '
        'frmVotingParameterMaintenance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.ControlDark
        Me.ClientSize = New System.Drawing.Size(986, 745)
        Me.Controls.Add(Me.btndelete)
        Me.Controls.Add(Me.btnFillGrid)
        Me.Controls.Add(Me.tbPageSize)
        Me.Controls.Add(Me.lblPageSize)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.userDataGridView)
        Me.Controls.Add(Me.btnLast)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnPrevious)
        Me.Controls.Add(Me.btnFirst)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnCancel)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.Name = "frmVotingParameterMaintenance"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Voting Parameter Maintenance"
        CType(Me.userDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents btnClose As System.Windows.Forms.Button
    Private WithEvents btnCancel As System.Windows.Forms.Button
    Private WithEvents btnUpdate As System.Windows.Forms.Button
    Public WithEvents btnPrint As System.Windows.Forms.Button
    Private WithEvents btnFillGrid As System.Windows.Forms.Button
    Private WithEvents tbPageSize As System.Windows.Forms.TextBox
    Private WithEvents lblPageSize As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents userDataGridView As System.Windows.Forms.DataGridView
    Private WithEvents btnLast As System.Windows.Forms.Button
    Private WithEvents btnNext As System.Windows.Forms.Button
    Private WithEvents btnPrevious As System.Windows.Forms.Button
    Private WithEvents btnFirst As System.Windows.Forms.Button
    Private WithEvents btndelete As System.Windows.Forms.Button
End Class
