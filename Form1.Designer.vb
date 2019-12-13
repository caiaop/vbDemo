<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.DataSet1 = New MysqlWindowsApp1.DataSet1()
        Me.NEWTABLEBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.NEWTABLETableAdapter = New MysqlWindowsApp1.DataSet1TableAdapters.NEWTABLETableAdapter()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.ButtonScan = New System.Windows.Forms.Button()
        Me.delButton = New System.Windows.Forms.Button()
        Me.TreeView1 = New System.Windows.Forms.TreeView()
        Me.addButton = New System.Windows.Forms.Button()
        Me.modButton = New System.Windows.Forms.Button()
        Me.AxFiScn1 = New AxFiScnLib.AxFiScn()
        CType(Me.DataSet1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NEWTABLEBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.AxFiScn1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "wmlogo.gif")
        Me.ImageList1.Images.SetKeyName(1, "gswj2015.jpg")
        Me.ImageList1.Images.SetKeyName(2, "ind36.gif")
        '
        'DataSet1
        '
        Me.DataSet1.DataSetName = "DataSet1"
        Me.DataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'NEWTABLEBindingSource
        '
        Me.NEWTABLEBindingSource.DataMember = "NEWTABLE"
        Me.NEWTABLEBindingSource.DataSource = Me.DataSet1
        '
        'NEWTABLETableAdapter
        '
        Me.NEWTABLETableAdapter.ClearBeforeFill = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonScan, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.delButton, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TreeView1)
        Me.TableLayoutPanel1.Controls.Add(Me.addButton, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.modButton, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.AxFiScn1, 1, 4)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 6
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(820, 579)
        Me.TableLayoutPanel1.TabIndex = 6
        '
        'ButtonScan
        '
        Me.ButtonScan.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonScan.Location = New System.Drawing.Point(617, 281)
        Me.ButtonScan.Name = "ButtonScan"
        Me.ButtonScan.Size = New System.Drawing.Size(198, 84)
        Me.ButtonScan.TabIndex = 11
        Me.ButtonScan.Text = "扫描到目录"
        Me.ButtonScan.UseVisualStyleBackColor = True
        '
        'delButton
        '
        Me.delButton.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.delButton.Location = New System.Drawing.Point(617, 97)
        Me.delButton.Name = "delButton"
        Me.delButton.Size = New System.Drawing.Size(198, 84)
        Me.delButton.TabIndex = 9
        Me.delButton.Text = "删除目录"
        Me.delButton.UseVisualStyleBackColor = True
        '
        'TreeView1
        '
        Me.TreeView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TreeView1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.TreeView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeView1.HotTracking = True
        Me.TreeView1.ImageIndex = 0
        Me.TreeView1.ImageList = Me.ImageList1
        Me.TreeView1.Location = New System.Drawing.Point(5, 5)
        Me.TreeView1.Name = "TreeView1"
        Me.TableLayoutPanel1.SetRowSpan(Me.TreeView1, 7)
        Me.TreeView1.SelectedImageIndex = 0
        Me.TreeView1.Size = New System.Drawing.Size(604, 569)
        Me.TreeView1.TabIndex = 7
        '
        'addButton
        '
        Me.addButton.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.addButton.Location = New System.Drawing.Point(617, 5)
        Me.addButton.Name = "addButton"
        Me.addButton.Size = New System.Drawing.Size(198, 84)
        Me.addButton.TabIndex = 8
        Me.addButton.Text = "添加目录"
        Me.addButton.UseVisualStyleBackColor = True
        '
        'modButton
        '
        Me.modButton.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.modButton.Location = New System.Drawing.Point(617, 189)
        Me.modButton.Name = "modButton"
        Me.modButton.Size = New System.Drawing.Size(198, 84)
        Me.modButton.TabIndex = 10
        Me.modButton.Text = "更改目录"
        Me.modButton.UseVisualStyleBackColor = True
        '
        'AxFiScn1
        '
        Me.AxFiScn1.Enabled = True
        Me.AxFiScn1.Location = New System.Drawing.Point(617, 373)
        Me.AxFiScn1.Name = "AxFiScn1"
        Me.AxFiScn1.OcxState = CType(resources.GetObject("AxFiScn1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxFiScn1.Size = New System.Drawing.Size(48, 48)
        Me.AxFiScn1.TabIndex = 12
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(820, 579)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        CType(Me.DataSet1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NEWTABLEBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.AxFiScn1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DataSet1 As DataSet1
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents NEWTABLEBindingSource As BindingSource
    Friend WithEvents NEWTABLETableAdapter As DataSet1TableAdapters.NEWTABLETableAdapter
    Friend WithEvents TreeView1 As TreeView
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents addButton As Button
    Friend WithEvents delButton As Button
    Friend WithEvents modButton As Button
    Friend WithEvents ButtonScan As Button
    Friend WithEvents AxFiScn1 As AxFiScnLib.AxFiScn
End Class
