Public Class CatalogWin
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles closeButton.Click
        Close()
    End Sub

    Private Sub submitButton_Click(sender As Object, e As EventArgs) Handles submitButton.Click
        Dim catalog As Catalog
        Try
            catalog = GetCatalog()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        If Text.StartsWith("添加") Then
            Form1.addCatalog(catalog)
            MessageBox.Show("添加目录成功！", "提示")
            Close()
        Else
            Form1.modCatalog(catalog)
            MessageBox.Show("更改目录成功！", "提示")
            Close()
        End If
    End Sub

    Private Function GetCatalog() As Catalog
        Dim catalog As Catalog = New Catalog
        catalog.CatalogName = Trim(catalogName.Text)
        If catalog.CatalogName = String.Empty Then
            Throw New Exception("目录名称不能为空！")
        ElseIf catalog.CatalogName.Length > 20 Then
            Throw New Exception("目录名称不能超过20个字！")
        End If

        If Trim(sort.Text) <> String.Empty Then
            Try
                catalog.Sort = CType(sort.Text, Integer)
            Catch ex As Exception
                Throw New Exception("目录序号不是整数！", ex)
            End Try
        ElseIf sort.Text.Length > 3 Then
            Throw New Exception("目录序号不能超过3位整数！")
        End If

        If Not IsNothing(parentCatalog.SelectedItem) Then
            catalog.ParentCatalogId = CType(parentCatalog.SelectedItem, Catalog).CatalogId
        End If

        If Form1.myCatalogFacade.Exists(catalog) Then
            Throw New Exception("不能添加相同的目录！")
        End If

        Return catalog
    End Function
End Class