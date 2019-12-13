<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CatalogWin
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
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

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.catalogName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.sort = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.parentCatalog = New System.Windows.Forms.ComboBox()
        Me.submitButton = New System.Windows.Forms.Button()
        Me.closeButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(28, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "目录名称："
        '
        'catalogName
        '
        Me.catalogName.Location = New System.Drawing.Point(134, 30)
        Me.catalogName.Name = "catalogName"
        Me.catalogName.Size = New System.Drawing.Size(429, 26)
        Me.catalogName.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(28, 91)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(88, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "目录序号："
        '
        'sort
        '
        Me.sort.Location = New System.Drawing.Point(134, 86)
        Me.sort.Name = "sort"
        Me.sort.Size = New System.Drawing.Size(429, 26)
        Me.sort.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(28, 145)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "上级目录："
        '
        'parentCatalog
        '
        Me.parentCatalog.DisplayMember = "CatalogName"
        Me.parentCatalog.FormattingEnabled = True
        Me.parentCatalog.Location = New System.Drawing.Point(134, 141)
        Me.parentCatalog.Name = "parentCatalog"
        Me.parentCatalog.Size = New System.Drawing.Size(429, 24)
        Me.parentCatalog.TabIndex = 4
        Me.parentCatalog.ValueMember = "CatalogId"
        '
        'submitButton
        '
        Me.submitButton.Location = New System.Drawing.Point(179, 183)
        Me.submitButton.Name = "submitButton"
        Me.submitButton.Size = New System.Drawing.Size(85, 32)
        Me.submitButton.TabIndex = 6
        Me.submitButton.Text = "确定"
        Me.submitButton.UseVisualStyleBackColor = True
        '
        'closeButton
        '
        Me.closeButton.Location = New System.Drawing.Point(287, 183)
        Me.closeButton.Name = "closeButton"
        Me.closeButton.Size = New System.Drawing.Size(85, 32)
        Me.closeButton.TabIndex = 7
        Me.closeButton.Text = "取消"
        Me.closeButton.UseVisualStyleBackColor = True
        '
        'CatalogWin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(599, 241)
        Me.Controls.Add(Me.closeButton)
        Me.Controls.Add(Me.submitButton)
        Me.Controls.Add(Me.parentCatalog)
        Me.Controls.Add(Me.sort)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.catalogName)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("宋体", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "CatalogWin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CatalogWin"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents catalogName As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents sort As TextBox
    Friend WithEvents parentCatalog As ComboBox
    Friend WithEvents submitButton As Button
    Friend WithEvents closeButton As Button
End Class
