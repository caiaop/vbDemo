Imports System.Configuration
Imports System.Data.SqlClient

Public Class Form1

    Public Shared ReadOnly myCatalogFacade As CatalogFacade = New CatalogFacade

    ' 定义一个列表来有序保存所有的目录
    Private catalogList As New CatalogCollection

    Private showRoot As String = False

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Reload(False)
    End Sub

    Private Sub Reload(inited As Boolean)
        'TODO: 这行代码将数据加载到表“DataSet1.NEWTABLE”中。您可以根据需要移动或删除它。
        Me.NEWTABLETableAdapter.Fill(Me.DataSet1.NEWTABLE)

        If inited Then
            myCatalogFacade.Clear()
            catalogList.Clear()
            TreeView1.Nodes.Clear()
        End If

        Dim rows As ICollection = Me.DataSet1.NEWTABLE.Rows
        For Each myDataRow As DataRow In rows
            CreateCatalog(myDataRow)
        Next

        Dim roots As CatalogCollection = myCatalogFacade.Items(CatalogFacade.Root)
        initTree(roots, showRoot)
    End Sub

    Private Sub CreateCatalog(myDataRow As DataRow)
        Dim newCatalog As Catalog = New Catalog
        newCatalog.CatalogId = myDataRow("ID")
        newCatalog.CatalogName = myDataRow("ITEM_NAME")
        If Not IsDBNull(myDataRow("PARENT_ID")) Then
            newCatalog.ParentCatalogId = myDataRow("PARENT_ID")
        End If
        If Not IsDBNull(myDataRow("SORT")) Then
            newCatalog.sort = CType(myDataRow("SORT"), Integer)
        End If

        catalogList.Add(newCatalog)
        myCatalogFacade.Add(newCatalog)
    End Sub

    Private Sub initTree(roots As CatalogCollection, showRootNode As Boolean)
        Dim cascadeLevel As Integer = 0

        If IsNothing(roots) Then
            Return
        End If

        Dim iter As IEnumerator = roots.GetEnumerator

        If Not IsNothing(iter) Then
            Do While (iter.MoveNext)
                Dim currentCatalog As Catalog = iter.Current
                If showRootNode Then
                    Dim rootTreeNode As TreeNode = TreeView1.Nodes.Add(currentCatalog.CatalogId, currentCatalog.CatalogName, cascadeLevel)
                    addChildTree(rootTreeNode, currentCatalog, cascadeLevel)
                Else
                    Dim nodes As CatalogCollection = myCatalogFacade.Items(currentCatalog.CatalogId)
                    initTree(nodes, True)
                End If
            Loop
        End If
    End Sub

    Private Sub addChildTree(rootTreeNode As TreeNode, currentCatalog As Catalog, cascadeLevel As Integer)
        If Not IsNothing(myCatalogFacade.Items(currentCatalog.CatalogId)) Then
            Dim nodes As IEnumerator = myCatalogFacade.Items(currentCatalog.CatalogId).GetEnumerator()

            If Not IsNothing(nodes) Then
                Dim childCascadeLevel As Integer = cascadeLevel + 1
                Do While (nodes.MoveNext)
                    Dim childCatalog As Catalog = nodes.Current
                    Dim childTreeNode As TreeNode = rootTreeNode.Nodes.Add(childCatalog.CatalogId, childCatalog.CatalogName, childCascadeLevel)

                    addChildTree(childTreeNode, childCatalog, childCascadeLevel)
                Loop
                rootTreeNode.ExpandAll()
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles addButton.Click
        Using myCatalogWin As New CatalogWin
            myCatalogWin.Text = CType(sender, Button).Text
            myCatalogWin.parentCatalog.Items.AddRange(AllCatalog().ToArray)
            myCatalogWin.ShowDialog(Me)
        End Using
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles delButton.Click
        delCatalog()
    End Sub

    Public Sub addCatalog(catalog As Catalog)
        catalog.CatalogId = Guid.NewGuid.ToString
        ' 数据库添加记录
        Me.NEWTABLETableAdapter.Insert(catalog.CatalogId， catalog.CatalogName, catalog.ParentCatalogId, catalog.Sort)

        Reload(True)

    End Sub

    Public Sub modCatalog(catalog As Catalog)
        catalog.CatalogId = SelectedCatalog.CatalogId

        Dim rows As ICollection = Me.DataSet1.NEWTABLE.Rows
        For Each myDataRow As DataRow In rows
            Dim catalogId As String = myDataRow("ID")
            If catalogId = catalog.CatalogId Then
                myDataRow("ITEM_NAME") = catalog.CatalogName

                Dim parentCatalogId As String = catalog.ParentCatalogId
                If IsNothing(parentCatalogId) Then
                    Dim roots As CatalogCollection = myCatalogFacade.Items(CatalogFacade.Root)
                    If Not IsNothing(roots) Then
                        parentCatalogId = roots.Item(0).CatalogId
                    End If
                End If
                myDataRow("PARENT_ID") = parentCatalogId
                myDataRow("SORT") = catalog.Sort
                Exit For
            End If
        Next

        ' 数据库添加记录
        Me.NEWTABLETableAdapter.Update(Me.DataSet1.NEWTABLE)

        Reload(True)
    End Sub

    Private Sub delCatalog()
        Dim catalog As Catalog = SelectedCatalog()

        If IsNothing(catalog.CatalogId) Then
            MessageBox.Show("请选择一个要删除的目录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        delCatalogCascade(catalog.CatalogId)

        If MessageBox.Show("您确定要删除【" & catalog.CatalogName & "】目录及其子目录吗？"， "提示"， MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Me.NEWTABLETableAdapter.Update(Me.DataSet1.NEWTABLE)

            Reload(True)

            MessageBox.Show("删除目录成功！", "提示")
        End If

    End Sub

    Private Sub delCatalogCascade(delCatalogId As String)
        If IsNothing(delCatalogId) Then
            Return
        End If

        Dim delCatalogs As HashSet(Of String) = New HashSet(Of String)
        AllDelCatalogs(delCatalogs, delCatalogId)

        Dim rows As ICollection = Me.DataSet1.NEWTABLE.Rows
        Dim iter As IEnumerator = rows.GetEnumerator

        Do While (iter.MoveNext)
            Dim myDataRow As DataRow = iter.Current
            Dim catalogId As String = myDataRow("ID")
            If delCatalogs.Contains(catalogId) Then
                myDataRow.Delete()
            End If
        Loop
    End Sub

    Private Sub AllDelCatalogs(delCatalogs As HashSet(Of String), delCatalogId As String)
        delCatalogs.Add(delCatalogId)

        Dim rows As ICollection = Me.DataSet1.NEWTABLE.Rows
        For Each myDataRow As DataRow In rows
            Dim catalogId As String = myDataRow("ID")
            Dim parentCatalogId As String = String.Empty
            If Not IsDBNull(myDataRow("PARENT_ID")) Then
                parentCatalogId = myDataRow("PARENT_ID")
            End If
            If delCatalogId = parentCatalogId Then
                AllDelCatalogs(delCatalogs, catalogId)
            End If
        Next
    End Sub

    Private Sub modButton_Click(sender As Object, e As EventArgs) Handles modButton.Click
        Dim catalog As Catalog = SelectedCatalog()

        If IsNothing(catalog.CatalogId) Then
            MessageBox.Show("请选择一个要修改的目录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using myCatalogWin As New CatalogWin
            myCatalogWin.Text = CType(sender, Button).Text
            myCatalogWin.catalogName.Text = catalog.CatalogName
            If Not IsNothing(catalog.Sort) Then
                myCatalogWin.sort.Text = catalog.Sort.ToString
            End If

            Dim catalogs As ArrayList = AllCatalog()
            myCatalogWin.parentCatalog.Items.AddRange(catalogs.ToArray)

            Dim parentCatalogId As String = myCatalogFacade.ParentKey(catalog)

            Dim count As Integer = catalogList.Count - 1
            For i As Integer = 0 To count
                Dim item As Catalog = catalogList.Item(i)
                If item.CatalogId = parentCatalogId Then
                    myCatalogWin.parentCatalog.SelectedIndex = i
                    Exit For
                End If
            Next

            myCatalogWin.ShowDialog(Me)
        End Using
    End Sub

    Public Function AllCatalog() As ArrayList
        Dim list As ArrayList = New ArrayList

        For Each catalog As Catalog In catalogList
            Dim item As Catalog = New Catalog
            item.CatalogId = catalog.CatalogId
            If IsNothing(catalog.ParentCatalogId) Then
                item.CatalogName = catalog.CatalogName
            Else
                Dim parentCatalog As Catalog = myCatalogFacade.Find(catalog.ParentCatalogId)
                item.CatalogName = parentCatalog.CatalogName & " / " & catalog.CatalogName
            End If
            item.ParentCatalogId = catalog.ParentCatalogId
            item.Sort = catalog.Sort
            list.Add(item)
        Next
        Return list
    End Function

    Private Function SelectedCatalog() As Catalog
        If Not IsNothing(TreeView1.SelectedNode) Then
            Dim catalogId As String = TreeView1.SelectedNode.Name
            Return myCatalogFacade.Find(catalogId)
        End If
        Return Nothing
    End Function

    Public Sub OpenScanner()
        Dim status As Integer
        Dim ErrorCode As Integer

        status = AxFiScn1.OpenScanner(Me.Handle.ToInt32)
        If status = RC_FAILURE Then
            ErrorCode = AxFiScn1.ErrorCode
            MsgBox("The ""OpenScanner"" function became an error." & Chr(13) & Chr(9) & "error code : " & HexString(ErrorCode))
            blnFjtwn = False
            blnOpenScanner = False
        ElseIf status = RC_NOT_DS_FJTWAIN Then
            MsgBox("It is not ""FUJITSU TWAIN32 Driver.""")
            blnFjtwn = False
        End If
    End Sub

    Public Function HexString(ByRef ErrorCode As Integer) As String
        Dim strWork As Object
        strWork = Hex(ErrorCode)
        If Len(strWork) = 1 Then
            strWork = "0x0000000" & strWork
        ElseIf Len(strWork) = 2 Then
            strWork = "0x000000" & strWork
        ElseIf Len(strWork) = 3 Then
            strWork = "0x00000" & strWork
        ElseIf Len(strWork) = 4 Then
            strWork = "0x0000" & strWork
        ElseIf Len(strWork) = 5 Then
            strWork = "0x000" & strWork
        ElseIf Len(strWork) = 6 Then
            strWork = "0x00" & strWork
        ElseIf Len(strWork) = 7 Then
            strWork = "0x0" & strWork
        Else
            strWork = "0x" & strWork
        End If
        HexString = strWork
    End Function

    Private Sub ButtonScan_Click_1(sender As Object, e As EventArgs) Handles ButtonScan.Click
        Call StartScan()
    End Sub

    Private Sub StartScan()
        Dim status As Integer
        Dim ErrorCode As Integer

        'A scanning parameter is set as scanner control.
        'Call ScanModeSet()

        'Dim frmCancelScan As New FormCancelScan
        'frmCancelScan.StartPosition = FormStartPosition.CenterScreen        '
        'frmCancelScan.Owner = Me
        'frmCancelScan.Show()

        Try

            AxFiScn1.ScanTo = TYPE_FILE
            AxFiScn1.FileType = FILE_TIF
            AxFiScn1.FileName = Application.StartupPath & "\image####"
            AxFiScn1.Overwrite = OVERSCAN_ON

            'Me.AxFiScn1.ScanCount = -1
            'Me.AxFiScn1.FileNumber2 = 5

            Call OpenScanner()

            'Scanning start
            status = AxFiScn1.StartScan(Me.Handle.ToInt32)
            'failure
            If status = RC_FAILURE Then
                ErrorCode = AxFiScn1.ErrorCode
                MsgBox("The scanning error occurred." & Chr(13) & Chr(9) & "error code : " & HexString(ErrorCode))
            ElseIf status = RC_CANCEL Then
                MsgBox("The user canceled scanning." & Chr(13) & "Or the error which cannot continue scanning was detected.")
            End If

            Me.AxFiScn1.CloseScanner(Me.Handle.ToInt32())
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

        'frmCancelScan.Close()

    End Sub


    Private Sub AxFiScn1_ScanToFile(sender As Object, e As AxFiScnLib._DFiScnEvents_ScanToFileEvent) Handles AxFiScn1.ScanToFile


        MessageBox.Show(e.fileName)
    End Sub

    '参考富士通例子中的ScanModeSet和InitialFileRead方法
    Private Sub ScanModeSet()
        Dim topWnd As Long
        Dim leftWnd As Long
        Dim widthWnd As Long
        Dim heightWnd As Long
        Dim strWork As String
        Dim rcl As Integer

        On Error GoTo ReadError
        If strFilePath = "" Then
            GoTo ReadError
        End If

        'Display position information on a dialog
        'leftWnd = GetPrivateProfileInt("Form", "Left", -1, strFilePath)
        'topWnd = GetPrivateProfileInt("Form", "Top", -1, strFilePath)
        'If topWnd <> -1 Then
        '    Me.Top = topWnd
        'End If
        'If leftWnd <> -1 Then
        '    Me.Left = leftWnd
        'End If

        'Read configuration parameter
        AxFiScn1.ScanTo = GetPrivateProfileInt("Scan", "ScanToType", TYPE_FILE, strFilePath)
        AxFiScn1.FileType = GetPrivateProfileInt("Scan", "FileType", FILE_TIF, strFilePath)
        strWork = New String(Chr(0), MAX_PATH)
        rcl = GetPrivateProfileString("Scan", "FileName", "", strWork, MAX_PATH, strFilePath)
        If rcl > 0 Then
            AxFiScn1.FileName = strWork
        Else
            AxFiScn1.FileName = Application.StartupPath & "\image#####"
        End If
        AxFiScn1.Overwrite = GetPrivateProfileInt("Scan", "Overwrite", OW_CONFIRM, strFilePath)
        AxFiScn1.FileCounter = GetPrivateProfileInt("Scan", "FileCounter", 1, strFilePath).ToString()
        AxFiScn1.CompressionType = GetPrivateProfileInt("Scan", "CompressionType", COMP_MMR, strFilePath)
        AxFiScn1.JpegQuality = GetPrivateProfileInt("Scan", "JpegQuality", COMP_JPEG4, strFilePath)
        AxFiScn1.ScanMode = GetPrivateProfileInt("Scan", "ScanMode", SM_NORMAL, strFilePath)
        rcl = GetPrivateProfileInt("Scan", "ScanContinue", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.ScanContinue = True
        Else
            AxFiScn1.ScanContinue = False
        End If
        AxFiScn1.ScanContinueMode = GetPrivateProfileInt("Scan", "ScanContinueMode", 0, strFilePath)
        AxFiScn1.PaperSupply = GetPrivateProfileInt("Scan", "PaperSupply", ADF, strFilePath)
        Dim iPaperSupply As Integer = 0
        If AxFiScn1.PaperSupply >= 8 And AxFiScn1.PaperSupply <= 47 Then
            iPaperSupply = 2
        End If
        AxFiScn1.PaperSupply = AxFiScn1.PaperSupply + iPaperSupply

        AxFiScn1.ScanCount = GetPrivateProfileInt("Scan", "ScanCount", -1, strFilePath).ToString()
        AxFiScn1.PaperSize = GetPrivateProfileInt("Scan", "PaperSize", PSIZE_A4, strFilePath)
        If AxFiScn1.PaperSize = PSIZE_INDEX_CUSTOM Then
            AxFiScn1.PaperSize = PSIZE_DATA_CUSTOM
        End If

        rcl = GetPrivateProfileInt("Scan", "LongPage", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.LongPage = True
        Else
            AxFiScn1.LongPage = False
        End If
        AxFiScn1.Orientation = GetPrivateProfileInt("Scan", "Orientation", PORTRAIT, strFilePath)
        AxFiScn1.Unit = GetPrivateProfileInt("Scan", "Unit", 0, strFilePath)
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "CustomPaperWidth", "8.268", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.CustomPaperWidth = Single.Parse(strWork)
        Else
            AxFiScn1.CustomPaperWidth = Single.Parse("8.268")
        End If
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "CustomPaperLength", "11.693", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.CustomPaperLength = Single.Parse(strWork)
        Else
            AxFiScn1.CustomPaperLength = Single.Parse("11.693")
        End If
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "RegionLeft", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.RegionLeft = Single.Parse(strWork)
        Else
            AxFiScn1.RegionLeft = SINGLE_DEFAULT
        End If
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "RegionTop", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.RegionTop = Single.Parse(strWork)
        Else
            AxFiScn1.RegionTop = SINGLE_DEFAULT
        End If
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "RegionWidth", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.RegionWidth = Single.Parse(strWork)
        Else
            AxFiScn1.RegionWidth = SINGLE_DEFAULT
        End If
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "RegionLength", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.RegionLength = Single.Parse(strWork)
        Else
            AxFiScn1.RegionLength = SINGLE_DEFAULT
        End If
        rcl = GetPrivateProfileInt("Scan", "UndefinedScanning", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.UndefinedScanning = True
        Else
            AxFiScn1.UndefinedScanning = False
        End If
        AxFiScn1.BackgroundColor = GetPrivateProfileInt("Scan", "BackgroundColor", 0, strFilePath)
        AxFiScn1.PixelType = GetPrivateProfileInt("Scan", "PixelType", PIXEL_BLACK_WHITE, strFilePath)
        AxFiScn1.AutomaticColorSensitivity = GetPrivateProfileInt("Scan", "AutomaticColorSensitivity", 0, strFilePath).ToString()
        AxFiScn1.AutomaticColorBackground = GetPrivateProfileInt("Scan", "AutomaticColorBackground", 0, strFilePath)
        AxFiScn1.Resolution = GetPrivateProfileInt("Scan", "Resolution", RS_300, strFilePath)
        If AxFiScn1.Resolution = 8 Then
            AxFiScn1.Resolution = RS_1200
        ElseIf AxFiScn1.Resolution = 9 Then
            AxFiScn1.Resolution = RS_CUSTM
        End If
        AxFiScn1.CustomResolution = GetPrivateProfileInt("Scan", "CustomResolution", 300, strFilePath).ToString()
        AxFiScn1.Brightness = GetPrivateProfileInt("Scan", "Brightness", 128, strFilePath).ToString()
        AxFiScn1.Contrast = GetPrivateProfileInt("Scan", "Contrast", 128, strFilePath).ToString()
        AxFiScn1.Threshold = GetPrivateProfileInt("Scan", "Threshold", 128, strFilePath).ToString()
        rcl = GetPrivateProfileInt("Scan", "Reverse", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.Reverse = True
        Else
            AxFiScn1.Reverse = False
        End If
        rcl = GetPrivateProfileInt("Scan", "Mirroring", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.Mirroring = True
        Else
            AxFiScn1.Mirroring = False
        End If
        AxFiScn1.Rotation = GetPrivateProfileInt("Scan", "Rotation", RT_NONE, strFilePath)
        AxFiScn1.Background = GetPrivateProfileInt("Scan", "Background", MODE_OFF, strFilePath)
        AxFiScn1.Outline = GetPrivateProfileInt("Scan", "Outline", NONE, strFilePath)
        If AxFiScn1.PixelType = PIXEL_RGB And AxFiScn1.Outline > 3 Then
            AxFiScn1.Outline = AxFiScn1.Outline + 1
        End If
        AxFiScn1.Halftone = GetPrivateProfileInt("Scan", "Halftone", NONE, strFilePath)
        strWork = New String(Chr(0), MAX_PATH)
        rcl = GetPrivateProfileString("Scan", "HalftoneFile", "", strWork, MAX_PATH, strFilePath)
        If rcl > 0 Then
            AxFiScn1.HalftoneFile = strWork
        End If
        AxFiScn1.Gamma = GetPrivateProfileInt("Scan", "Gamma", NONE, strFilePath)
        strWork = New String(Chr(0), MAX_PATH)
        rcl = GetPrivateProfileString("Scan", "GammaFile", "", strWork, MAX_PATH, strFilePath)
        If rcl > 0 Then
            AxFiScn1.GammaFile = strWork
        End If
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "CustomGamma", "2.2", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.CustomGamma = Single.Parse(strWork)
        Else
            AxFiScn1.CustomGamma = Single.Parse("2.2")
        End If
        AxFiScn1.AutoSeparation = GetPrivateProfileInt("Scan", "AutoSeparation", AS_OFF, strFilePath)
        AxFiScn1.SEE = GetPrivateProfileInt("Scan", "SEE", SEE_OFF, strFilePath)
        AxFiScn1.ThresholdCurve = GetPrivateProfileInt("Scan", "ThresholdCurve", TH_CURVE1, strFilePath)
        AxFiScn1.NoiseRemoval = GetPrivateProfileInt("Scan", "NoiseRemoval", NONE, strFilePath)
        rcl = GetPrivateProfileInt("Scan", "PreFiltering", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.PreFiltering = True
        Else
            AxFiScn1.PreFiltering = False
        End If
        rcl = GetPrivateProfileInt("Scan", "Smoothing", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.Smoothing = True
        Else
            AxFiScn1.Smoothing = False
        End If
        rcl = GetPrivateProfileInt("Scan", "Endorser", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.Endorser = True
        Else
            AxFiScn1.Endorser = False
        End If
        AxFiScn1.EndorserDialog = GetPrivateProfileInt("Scan", "EndorserDialog", EDD_OFF, strFilePath)
        AxFiScn1.EndorserCounter = GetPrivateProfileInt("Scan", "EndorserCounter", 0, strFilePath).ToString()
        strWork = New String(Chr(0), 50)
        rcl = GetPrivateProfileString("Scan", "EndorserString", "", strWork, 50, strFilePath)
        If rcl > 0 Then
            AxFiScn1.EndorserString = strWork
        End If
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "EndorserOffset", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.EndorserOffset = Single.Parse(strWork)
        Else
            AxFiScn1.EndorserOffset = SINGLE_DEFAULT
        End If
        AxFiScn1.EndorserDirection = GetPrivateProfileInt("Scan", "EndorserDirection", 0, strFilePath)
        If AxFiScn1.EndorserDirection = 1 Then
            AxFiScn1.EndorserDirection = 3
        Else
            AxFiScn1.EndorserDirection = 1
        End If
        AxFiScn1.EndorserCountStep = GetPrivateProfileInt("Scan", "EndorserCountStep", 1, strFilePath)
        AxFiScn1.EndorserCountDirection = GetPrivateProfileInt("Scan", "EndorserCountDirection", 0, strFilePath)
        AxFiScn1.EndorserFont = GetPrivateProfileInt("Scan", "EndorserFont", EDF_HORIZONTAL, strFilePath)
        rcl = GetPrivateProfileInt("Scan", "SynchronizationDigitalEndorser", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.SynchronizationDigitalEndorser = True
        Else
            AxFiScn1.SynchronizationDigitalEndorser = False
        End If
        rcl = GetPrivateProfileInt("Scan", "DigitalEndorser", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.DigitalEndorser = True
        Else
            AxFiScn1.DigitalEndorser = False
        End If
        AxFiScn1.DigitalEndorserCounter = GetPrivateProfileInt("Scan", "DigitalEndorserCounter", 0, strFilePath).ToString()
        strWork = New String(Chr(0), 50)
        rcl = GetPrivateProfileString("Scan", "DigitalEndorserString", "", strWork, 50, strFilePath)
        If rcl > 0 Then
            AxFiScn1.DigitalEndorserString = strWork
        End If
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "DigitalEndorserXOffset", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.DigitalEndorserXOffset = Single.Parse(strWork)
        Else
            AxFiScn1.DigitalEndorserXOffset = SINGLE_DEFAULT
        End If
        rcl = GetPrivateProfileString("Scan", "DigitalEndorserYOffset", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.DigitalEndorserYOffset = Single.Parse(strWork)
        Else
            AxFiScn1.DigitalEndorserYOffset = SINGLE_DEFAULT
        End If
        AxFiScn1.DigitalEndorserDirection = GetPrivateProfileInt("Scan", "DigitalEndorserDirection", 0, strFilePath)
        AxFiScn1.DigitalEndorserCountStep = GetPrivateProfileInt("Scan", "DigitalEndorserCountStep", 1, strFilePath)
        AxFiScn1.DigitalEndorserCountDirection = GetPrivateProfileInt("Scan", "DigitalEndorserCountDirection", 0, strFilePath)

        AxFiScn1.JobControl = GetPrivateProfileInt("Scan", "JobControl", 0, strFilePath)
        AxFiScn1.JobControlMode = GetPrivateProfileInt("Scan", "JobControlMode", JCM_SPECIAL_DOCUMENT, strFilePath)
        AxFiScn1.Binding = GetPrivateProfileInt("Scan", "Binding", 0, strFilePath)
        AxFiScn1.DoubleFeed = GetPrivateProfileInt("Scan", "DoubleFeed", 0, strFilePath)
        rcl = GetPrivateProfileInt("Scan", "MultiFeedNotice", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.MultiFeedNotice = True
        Else
            AxFiScn1.MultiFeedNotice = False
        End If
        AxFiScn1.Filter = GetPrivateProfileInt("Scan", "Filter", FILTER_GREEN, strFilePath)
        If AxFiScn1.Filter = 7 Then
            AxFiScn1.Filter = 99
        ElseIf AxFiScn1.Filter = 8 Then
            AxFiScn1.Filter = 100
        ElseIf AxFiScn1.Filter = 9 Then
            AxFiScn1.Filter = 101
        ElseIf AxFiScn1.Filter = 10 Then
            AxFiScn1.Filter = 102
        End If

        AxFiScn1.FilterSaturationSensitivity = GetPrivateProfileInt("Scan", "FilterSaturationSensitivity", 50, strFilePath)
        AxFiScn1.SkipWhitePage = GetPrivateProfileInt("Scan", "SkipWhitePage", 0, strFilePath)
        AxFiScn1.SkipBlackPage = GetPrivateProfileInt("Scan", "SkipBlackPage", 0, strFilePath)
        AxFiScn1.BlankPageSkip = GetPrivateProfileInt("Scan", "BlankPageSkip", 0, strFilePath)
        rcl = GetPrivateProfileInt("Scan", "AutoBorderDetection", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.AutoBorderDetection = True
        Else
            AxFiScn1.AutoBorderDetection = False
        End If
        rcl = GetPrivateProfileInt("Scan", "AIQCNotice", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.AIQCNotice = True
        Else
            AxFiScn1.AIQCNotice = False
        End If
        AxFiScn1.BackgroundSmoothness = GetPrivateProfileInt("Scan", "BackgroundSmoothness", 5, strFilePath)
        AxFiScn1.CropPriority = GetPrivateProfileInt("Scan", "CropPriority", 0, strFilePath)
        AxFiScn1.Deskew = GetPrivateProfileInt("Scan", "Deskew", 2, strFilePath)
        AxFiScn1.DeskewBackground = GetPrivateProfileInt("Scan", "DeskewBackground", 1, strFilePath)
        AxFiScn1.DeskewMode = GetPrivateProfileInt("Scan", "DeskewMode", 1, strFilePath)
        AxFiScn1.BlankPageSkipMode = GetPrivateProfileInt("Scan", "BlankPageSkipMode", 0, strFilePath)
        AxFiScn1.BlankPageSkipTabPage = GetPrivateProfileInt("Scan", "BlankPageSkipTabPage", 0, strFilePath)
        AxFiScn1.BlankPageIgnoreAreaSize = GetPrivateProfileInt("Scan", "BlankPageIgnoreAreaSize", 16, strFilePath)
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "CropMarginSize", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.CropMarginSize = Single.Parse(strWork)
        Else
            AxFiScn1.CropMarginSize = SINGLE_DEFAULT
        End If
        AxFiScn1.BlankPageNotice = GetPrivateProfileInt("Scan", "BlankPageNotice", 0, strFilePath)
        AxFiScn1.LengthDetection = GetPrivateProfileInt("Scan", "LengthDetection", 0, strFilePath)
        rcl = GetPrivateProfileInt("Scan", "HwCompression", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.HwCompression = True
        Else
            AxFiScn1.HwCompression = False
        End If
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "MultiFeedModeChangeSize", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.MultiFeedModeChangeSize = Single.Parse(strWork)
        Else
            AxFiScn1.MultiFeedModeChangeSize = SINGLE_DEFAULT
        End If
        rcl = GetPrivateProfileInt("Scan", "FrontBackMergingEnabled", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.FrontBackMergingEnabled = True
        Else
            AxFiScn1.FrontBackMergingEnabled = False
        End If
        AxFiScn1.FrontBackMergingLocation = GetPrivateProfileInt("Scan", "FrontBackMergingLocation", FBML_RIGHT, strFilePath)
        AxFiScn1.FrontBackMergingRotation = GetPrivateProfileInt("Scan", "FrontBackMergingRotation", FBMR_NONE, strFilePath)
        If AxFiScn1.FrontBackMergingRotation = FBMR_INDEX_R180 Then
            AxFiScn1.FrontBackMergingRotation = FBMR_R180
        End If

        AxFiScn1.FrontBackMergingTarget = GetPrivateProfileInt("Scan", "FrontBackMergingTarget", FBMT_ALL, strFilePath)
        AxFiScn1.FrontBackMergingTargetMode = GetPrivateProfileInt("Scan", "FrontBackMergingTargetMode", FBMTM_INDEX_CUSTOM, strFilePath)
        If AxFiScn1.FrontBackMergingTarget = FBMTM_INDEX_CUSTOM Then
            AxFiScn1.FrontBackMergingTargetMode = FBMTM_CUSTOM
        ElseIf AxFiScn1.FrontBackMergingTarget = FBMTM_INDEX_CARDSIZE Then
            AxFiScn1.FrontBackMergingTargetMode = FBMTM_CARDSIZE
        End If

        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "FrontBackMergingTargetSize", FBMTG_DEFAULT.ToString(), strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.FrontBackMergingTargetSize = Single.Parse(strWork)
        Else
            AxFiScn1.FrontBackMergingTargetSize = FBMTG_DEFAULT
        End If

        rcl = GetPrivateProfileInt("Scan", "ShowSourceUI", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.ShowSourceUI = True
        Else
            AxFiScn1.ShowSourceUI = False
        End If
        rcl = GetPrivateProfileInt("Scan", "CloseSourceUI", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.CloseSourceUI = True
        Else
            AxFiScn1.CloseSourceUI = False
        End If
        rcl = GetPrivateProfileInt("Scan", "SourceCurrentScan", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.SourceCurrentScan = True
        Else
            AxFiScn1.SourceCurrentScan = False
        End If
        rcl = GetPrivateProfileInt("Scan", "SilentMode", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.SilentMode = True
        Else
            AxFiScn1.SilentMode = False
        End If
        AxFiScn1.Report = GetPrivateProfileInt("Scan", "Report", RP_OFF, strFilePath)

        rcl = GetPrivateProfileInt("Scan", "Indicator", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.Indicator = True
        Else
            AxFiScn1.Indicator = False
        End If
        AxFiScn1.Highlight = GetPrivateProfileInt("Scan", "Highlight", 230, strFilePath)
        AxFiScn1.Shadow = GetPrivateProfileInt("Scan", "Shadow", 10, strFilePath)
        AxFiScn1.OverScan = GetPrivateProfileInt("Scan", "OverScan", 0, strFilePath)
        rcl = GetPrivateProfileInt("Scan", "AutomaticSenseMedium", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.AutomaticSenseMedium = True
        Else
            AxFiScn1.AutomaticSenseMedium = False
        End If
        AxFiScn1.BackgroundSmoothing = GetPrivateProfileInt("Scan", "BackgroundSmoothing", 0, ModuleScan.strFilePath)
        rcl = GetPrivateProfileInt("Scan", "AutoBright", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.AutoBright = True
        Else
            AxFiScn1.AutoBright = False
        End If
        AxFiScn1.DTCSensitivity = GetPrivateProfileInt("Scan", "DTCSensitivity", 50, strFilePath)
        AxFiScn1.BackgroundThreshold = GetPrivateProfileInt("Scan", "BackgroundThreshold", 50, strFilePath)
        AxFiScn1.CharacterThickness = GetPrivateProfileInt("Scan", "CharacterThickness", 5, strFilePath)
        AxFiScn1.SDTCSensitivity = GetPrivateProfileInt("Scan", "SDTCSensitivity", 2, strFilePath)
        AxFiScn1.NoiseRejection = GetPrivateProfileInt("Scan", "NoiseRejection", 0, strFilePath)
        AxFiScn1.ADTCThreshold = GetPrivateProfileInt("Scan", "ADTCThreshold", 83, strFilePath)
        AxFiScn1.FadingCompensation = GetPrivateProfileInt("Scan", "FadingCompensation", 0, ModuleScan.strFilePath)
        AxFiScn1.Sharpness = GetPrivateProfileInt("Scan", "Sharpness", SH_NONE, ModuleScan.strFilePath)
        AxFiScn1.PunchHoleRemoval = GetPrivateProfileInt("Scan", "PunchHoleRemoval", PHR_DO_NOT_REMOVE, ModuleScan.strFilePath)
        AxFiScn1.PunchHoleRemovalMode = GetPrivateProfileInt("Scan", "PunchHoleRemovalMode", PHRM_STANDARD, ModuleScan.strFilePath)
        rcl = GetPrivateProfileInt("Scan", "sRGB", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.sRGB = True
        Else
            AxFiScn1.sRGB = False
        End If
        AxFiScn1.PatternRemoval = GetPrivateProfileInt("Scan", "PatternRemoval", 1, ModuleScan.strFilePath)
        rcl = GetPrivateProfileInt("Scan", "VerticalLineReduction", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.VerticalLineReduction = True
        Else
            AxFiScn1.VerticalLineReduction = False
        End If

        rcl = GetPrivateProfileInt("Scan", "BarcodeDetection", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.BarcodeDetection = True
        Else
            AxFiScn1.BarcodeDetection = False
        End If
        AxFiScn1.BarcodeDirection = GetPrivateProfileInt("Scan", "BarcodeDirection", BD_HORIZONTAL_VERTICAL, strFilePath)
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "BarcodeRegionLeft", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.BarcodeRegionLeft = Single.Parse(strWork)
        Else
            AxFiScn1.BarcodeRegionLeft = SINGLE_DEFAULT
        End If
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "BarcodeRegionTop", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.BarcodeRegionTop = Single.Parse(strWork)
        Else
            AxFiScn1.BarcodeRegionTop = SINGLE_DEFAULT
        End If
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "BarcodeRegionLength", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.BarcodeRegionLength = Single.Parse(strWork)
        Else
            AxFiScn1.BarcodeRegionLength = SINGLE_DEFAULT
        End If
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "BarcodeRegionWidth", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.BarcodeRegionWidth = Single.Parse(strWork)
        Else
            AxFiScn1.BarcodeRegionWidth = SINGLE_DEFAULT
        End If

        Dim iBarcodeType As Integer = 0
        rcl = GetPrivateProfileInt("Scan", "BarcodeType_EAN8", 1, strFilePath)
        If rcl = 1 Then
            iBarcodeType += BA_EAN8
        End If
        rcl = GetPrivateProfileInt("Scan", "BarcodeType_EAN13", 1, strFilePath)
        If rcl = 1 Then
            iBarcodeType += BA_EAN13
        End If
        rcl = GetPrivateProfileInt("Scan", "BarcodeType_Code3of9", 1, strFilePath)
        If rcl = 1 Then
            iBarcodeType += BA_CODE3OF9
        End If
        rcl = GetPrivateProfileInt("Scan", "BarcodeType_Code128", 1, strFilePath)
        If rcl = 1 Then
            iBarcodeType += BA_CODE128
        End If
        rcl = GetPrivateProfileInt("Scan", "BarcodeType_ITF", 1, strFilePath)
        If rcl = 1 Then
            iBarcodeType += BA_ITF
        End If
        rcl = GetPrivateProfileInt("Scan", "BarcodeType_UPC-A", 1, strFilePath)
        If rcl = 1 Then
            iBarcodeType += BA_UPCA
        End If
        rcl = GetPrivateProfileInt("Scan", "BarcodeType_Codabar", 1, strFilePath)
        If rcl = 1 Then
            iBarcodeType += BA_CODABAR
        End If
        rcl = GetPrivateProfileInt("Scan", "BarcodeType_PDF417", 1, strFilePath)
        If rcl = 1 Then
            iBarcodeType += BA_PDF417
        End If
        rcl = GetPrivateProfileInt("Scan", "BarcodeType_QRCode", 1, strFilePath)
        If rcl = 1 Then
            iBarcodeType += BA_QRCODE
        End If
        rcl = GetPrivateProfileInt("Scan", "BarcodeType_DataMatrix", 1, strFilePath)
        If rcl = 1 Then
            iBarcodeType += BA_DATAMATRIX
        End If
        rcl = GetPrivateProfileInt("Scan", "BarcodeType_AztecCode", 1, strFilePath)
        If rcl = 1 Then
            iBarcodeType += BA_AZTECCODE
        End If
        AxFiScn1.BarcodeType = iBarcodeType

        AxFiScn1.BarcodeMaxSearchPriorities = GetPrivateProfileInt("Scan", "BarcodeMaxSearchPriorities", 1, strFilePath)

        rcl = GetPrivateProfileInt("Scan", "PatchCodeDetection", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.PatchCodeDetection = True
        Else
            AxFiScn1.PatchCodeDetection = False
        End If
        AxFiScn1.PatchCodeDirection = GetPrivateProfileInt("Scan", "PatchCodeDirection", PD_VERTICAL, strFilePath)

        Dim iPatchCodeType As Integer = 0
        rcl = GetPrivateProfileInt("Scan", "PatchCodeType_Patch1", 1, strFilePath)
        If rcl = 1 Then
            iPatchCodeType += PA_PATCH1
        End If
        rcl = GetPrivateProfileInt("Scan", "PatchCodeType_Patch2", 1, strFilePath)
        If rcl = 1 Then
            iPatchCodeType += PA_PATCH2
        End If
        rcl = GetPrivateProfileInt("Scan", "PatchCodeType_Patch3", 1, strFilePath)
        If rcl = 1 Then
            iPatchCodeType += PA_PATCH3
        End If
        rcl = GetPrivateProfileInt("Scan", "PatchCodeType_Patch4", 1, strFilePath)
        If rcl = 1 Then
            iPatchCodeType += PA_PATCH4
        End If
        rcl = GetPrivateProfileInt("Scan", "PatchCodeType_Patch6", 1, strFilePath)
        If rcl = 1 Then
            iPatchCodeType += PA_PATCH6
        End If
        rcl = GetPrivateProfileInt("Scan", "PatchCodeType_PatchT", 1, strFilePath)
        If rcl = 1 Then
            iPatchCodeType += PA_PATCHT
        End If
        AxFiScn1.PatchCodeType = iPatchCodeType

        AxFiScn1.EdgeFiller = GetPrivateProfileInt("Scan", "EdgeFiller", EF_OFF, strFilePath)
        strWork = New String(Chr(0), 10)
        rcl = GetPrivateProfileString("Scan", "EdgeFillerTop", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.EdgeFillerTop = Single.Parse(strWork)
        Else
            AxFiScn1.EdgeFillerTop = SINGLE_DEFAULT
        End If
        rcl = GetPrivateProfileString("Scan", "EdgeFillerBottom", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.EdgeFillerBottom = Single.Parse(strWork)
        Else
            AxFiScn1.EdgeFillerBottom = SINGLE_DEFAULT
        End If
        rcl = GetPrivateProfileString("Scan", "EdgeFillerLeft", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.EdgeFillerLeft = Single.Parse(strWork)
        Else
            AxFiScn1.EdgeFillerLeft = SINGLE_DEFAULT
        End If
        rcl = GetPrivateProfileString("Scan", "EdgeFillerRight", "0", strWork, 10, strFilePath)
        If rcl > 0 Then
            AxFiScn1.EdgeFillerRight = Single.Parse(strWork)
        Else
            AxFiScn1.EdgeFillerRight = SINGLE_DEFAULT
        End If
        rcl = GetPrivateProfileInt("Scan", "EdgeRepair", 0, strFilePath)
        If rcl = 1 Then
            AxFiScn1.EdgeRepair = True
        Else
            AxFiScn1.EdgeRepair = False
        End If

        bInitialFileRead = True
        Exit Sub
ReadError:
        Call DefaultSet()

    End Sub

    '参考富士通示例中的ScanModeSet和DefaultSet方法
    Private Sub DefaultSet()
        AxFiScn1.Outline = NONE
        AxFiScn1.ScanTo = TYPE_FILE
        AxFiScn1.FileType = FILE_TIF
        AxFiScn1.FileName = Application.StartupPath & "\image####"
        AxFiScn1.Overwrite = OW_OFF
        AxFiScn1.FileCounter = 1
        AxFiScn1.CompressionType = NO_COMP
        AxFiScn1.JpegQuality = COMP_JPEG7
        AxFiScn1.ScanMode = SM_NORMAL
        AxFiScn1.ScanContinue = False
        AxFiScn1.ScanContinueMode = 0

        AxFiScn1.PixelType = ADF_BACKSIDE
        AxFiScn1.ScanCount = -1
        AxFiScn1.PaperSize = PSIZE_A4
        AxFiScn1.LongPage = False
        AxFiScn1.Orientation = PORTRAIT
        AxFiScn1.CustomPaperWidth = Single.Parse("8.268")
        AxFiScn1.CustomPaperLength = Single.Parse("11.693")
        AxFiScn1.RegionLeft = SINGLE_DEFAULT
        AxFiScn1.RegionTop = SINGLE_DEFAULT
        AxFiScn1.RegionWidth = SINGLE_DEFAULT
        AxFiScn1.RegionLength = SINGLE_DEFAULT
        AxFiScn1.UndefinedScanning = False
        AxFiScn1.BackgroundColor = NONE

        AxFiScn1.PixelType = PIXEL_BLACK_WHITE
        AxFiScn1.AutomaticColorSensitivity = 0
        AxFiScn1.AutomaticColorBackground = 0
        AxFiScn1.Resolution = RS_300
        AxFiScn1.CustomResolution = 300

        AxFiScn1.Brightness = 128
        AxFiScn1.Contrast = 128
        AxFiScn1.Threshold = 128
        AxFiScn1.Reverse = False
        AxFiScn1.Mirroring = False
        AxFiScn1.Rotation = RT_R180
        AxFiScn1.Background = MODE_OFF
        AxFiScn1.Outline = NONE
        AxFiScn1.Halftone = NONE
        AxFiScn1.HalftoneFile = ""
        AxFiScn1.Gamma = NONE
        AxFiScn1.GammaFile = ""
        AxFiScn1.CustomGamma = Single.Parse("2.2")
        AxFiScn1.AutoSeparation = AS_OFF
        AxFiScn1.SEE = SEE_OFF

        AxFiScn1.ThresholdCurve = TH_CURVE1
        AxFiScn1.NoiseRemoval = 0
        AxFiScn1.PreFiltering = False
        AxFiScn1.Smoothing = False

        AxFiScn1.Endorser = False
        AxFiScn1.EndorserDialog = EDD_OFF
        AxFiScn1.EndorserCounter = 0
        AxFiScn1.EndorserString = ""
        AxFiScn1.EndorserOffset = SINGLE_DEFAULT
        AxFiScn1.EndorserDirection = 1
        AxFiScn1.EndorserCountStep = 1
        AxFiScn1.EndorserCountDirection = 0
        AxFiScn1.EndorserFont = EF_OFF
        AxFiScn1.SynchronizationDigitalEndorser = False

        AxFiScn1.DigitalEndorser = False
        AxFiScn1.DigitalEndorserCounter = 0
        AxFiScn1.DigitalEndorserString = ""
        AxFiScn1.DigitalEndorserXOffset = SINGLE_DEFAULT
        AxFiScn1.DigitalEndorserYOffset = SINGLE_DEFAULT
        AxFiScn1.DigitalEndorserDirection = 0
        AxFiScn1.DigitalEndorserCountStep = 1
        AxFiScn1.DigitalEndorserCountDirection = 0

        AxFiScn1.JobControl = NONE
        AxFiScn1.JobControlMode = JCM_SPECIAL_DOCUMENT
        AxFiScn1.Binding = 0
        AxFiScn1.MultiFeed = 0
        AxFiScn1.MultiFeedNotice = False
        AxFiScn1.Filter = FILTER_GREEN
        AxFiScn1.FilterSaturationSensitivity = 50
        AxFiScn1.SkipWhitePage = 0
        AxFiScn1.SkipBlackPage = 0
        AxFiScn1.BlankPageSkip = 0
        AxFiScn1.AutoBorderDetection = False

        AxFiScn1.ShowSourceUI = False
        AxFiScn1.CloseSourceUI = False
        AxFiScn1.SourceCurrentScan = False

        AxFiScn1.SilentMode = False
        AxFiScn1.Report = NONE

        AxFiScn1.Indicator = True

        AxFiScn1.Shadow = 10
        AxFiScn1.Highlight = 230

        AxFiScn1.OverScan = OVERSCAN_OFF
        AxFiScn1.Unit = UNIT_INCHES
        AxFiScn1.AutomaticSenseMedium = False
        AxFiScn1.BackgroundSmoothing = 0
        AxFiScn1.AutoBright = False
        AxFiScn1.DTCSensitivity = 50
        AxFiScn1.BackgroundThreshold = 50
        AxFiScn1.CharacterThickness = 5
        AxFiScn1.SDTCSensitivity = 2
        AxFiScn1.NoiseRejection = 0
        AxFiScn1.ADTCThreshold = 83
        AxFiScn1.Sharpness = SH_NONE
        AxFiScn1.FadingCompensation = 0
        AxFiScn1.sRGB = False
        AxFiScn1.PunchHoleRemoval = PHR_DO_NOT_REMOVE
        AxFiScn1.PunchHoleRemovalMode = PHRM_STANDARD
        AxFiScn1.PatternRemoval = 1
        AxFiScn1.VerticalLineReduction = False
        AxFiScn1.AIQCNotice = False
        AxFiScn1.BackgroundSmoothness = 5
        AxFiScn1.CropPriority = 0
        AxFiScn1.Deskew = 2
        AxFiScn1.DeskewBackground = 1
        AxFiScn1.DeskewMode = 1
        AxFiScn1.BlankPageSkipMode = 0
        AxFiScn1.BlankPageSkipTabPage = 0
        AxFiScn1.BlankPageIgnoreAreaSize = 16
        AxFiScn1.CropMarginSize = SINGLE_DEFAULT
        AxFiScn1.MultiFeedModeChangeSize = SINGLE_DEFAULT
        AxFiScn1.BlankPageNotice = 0
        AxFiScn1.LengthDetection = 0
        AxFiScn1.HwCompression = False
        AxFiScn1.FrontBackMergingEnabled = False
        AxFiScn1.FrontBackMergingLocation = FBML_RIGHT
        AxFiScn1.FrontBackMergingRotation = FBMR_NONE
        AxFiScn1.FrontBackMergingTarget = FBMT_ALL
        AxFiScn1.FrontBackMergingTargetMode = FBMTM_INDEX_CUSTOM
        AxFiScn1.FrontBackMergingTargetSize = FBMTG_DEFAULT.ToString()

        AxFiScn1.BarcodeDetection = False
        AxFiScn1.BarcodeDirection = BD_HORIZONTAL_VERTICAL
        AxFiScn1.BarcodeRegionLeft = SINGLE_DEFAULT
        AxFiScn1.BarcodeRegionTop = SINGLE_DEFAULT
        AxFiScn1.BarcodeRegionWidth = SINGLE_DEFAULT
        AxFiScn1.BarcodeRegionLength = SINGLE_DEFAULT

        AxFiScn1.BarcodeMaxSearchPriorities = 1

        AxFiScn1.PatchCodeDetection = False
        AxFiScn1.PatchCodeDirection = PD_VERTICAL

        AxFiScn1.EdgeFiller = EF_OFF
        AxFiScn1.EdgeFillerTop = SINGLE_DEFAULT
        AxFiScn1.EdgeFillerBottom = SINGLE_DEFAULT
        AxFiScn1.EdgeFillerLeft = SINGLE_DEFAULT
        AxFiScn1.EdgeFillerRight = SINGLE_DEFAULT

        AxFiScn1.EdgeRepair = False
    End Sub

End Class
