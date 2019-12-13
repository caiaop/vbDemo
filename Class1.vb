Public Class CatalogFacade

    ' 定义一个哈希表存储目录树结构，以方便与树组件的交互
    Private CatalogHashtable As New Hashtable

    ' 定义一个哈希表映射目录ID与目录的关联关系
    Private CatalogMap As New Hashtable

    Public Const Root As String = "ROOT"

    Public Sub Add(catalog As Catalog)
        CatalogMap.Item(catalog.CatalogId) = catalog

        Dim parentCatalogId As String = ParentKey(catalog)
        Dim childCatalogs As CatalogCollection = Items(parentCatalogId)

        If IsNothing(childCatalogs) Then
            childCatalogs = New CatalogCollection
            childCatalogs.Add(catalog)
            CatalogHashtable.Item(parentCatalogId) = childCatalogs
        Else
            childCatalogs.Add(catalog)
        End If

    End Sub

    Public Sub Remove(catalog As Catalog)
        Dim parentCatalogId As String = ParentKey(catalog)
        Dim childCatalogs As CatalogCollection = Items(parentCatalogId)

        If Not IsNothing(childCatalogs) Then
            childCatalogs.Remove(catalog)
        End If

        RemoveCatalogCascade(catalog)
    End Sub

    Public Sub Clear()
        CatalogHashtable.Clear()
        CatalogMap.Clear()
    End Sub

    Public Sub RemoveCatalogCascade(catalog As Catalog)
        Dim childCatalogs As CatalogCollection = Items(catalog.CatalogId)
        If Not IsNothing(childCatalogs) Then
            Dim nodes As IEnumerator = childCatalogs.GetEnumerator()
            If Not IsNothing(nodes) Then
                Do While (nodes.MoveNext)
                    Dim childCatalog As Catalog = nodes.Current
                    RemoveCatalogCascade(childCatalog)
                Loop
            End If
        End If
        CatalogHashtable.Remove(catalog.CatalogId)
        CatalogMap.Remove(catalog.CatalogId)
    End Sub

    Public Function Items(CatalogId As String) As CatalogCollection
        Return CType(CatalogHashtable.Item(CatalogId), CatalogCollection)
    End Function

    Public Function ParentKey(catalog As Catalog) As String
        Dim parentCatalogId As String = catalog.ParentCatalogId
        If IsNothing(catalog.ParentCatalogId) Then
            parentCatalogId = Root
        End If
        Return parentCatalogId
    End Function

    Public Function Find(CatalogId As String) As Catalog
        Return CType(CatalogMap.Item(CatalogId), Catalog)
    End Function

    Public Function Exists(catalog As Catalog) As Boolean
        Dim parentCatalogId As String = ParentKey(catalog)

        If Not IsNothing(Items(parentCatalogId)) Then
            Dim nodes As IEnumerator = Items(parentCatalogId).GetEnumerator()

            If Not IsNothing(nodes) Then
                Do While (nodes.MoveNext)
                    Dim childCatalog As Catalog = nodes.Current
                    If childCatalog.CatalogName = catalog.CatalogName And childCatalog.Sort = catalog.Sort Then
                        Return True
                    End If
                Loop
            End If
        End If

        Return False
    End Function

End Class

Public Class CatalogCollection
    Inherits CollectionBase

    Public Sub Add(catalog As Catalog)
        Me.List.Add(catalog)
    End Sub

    Public Sub Remove(catalog As Catalog)
        Me.List.Remove(catalog)
    End Sub

    Default Public Property Item(index As Integer) As Catalog
        Get
            Return CType(Me.List.Item(index), Catalog)
        End Get
        Set(value As Catalog)
            Me.List.Item(index) = value
        End Set
    End Property


End Class

Public Structure Catalog

    Public CatalogId As String
    Public CatalogName As String
    Public ParentCatalogId As String
    Public Sort As Integer

    Public Overrides Function ToString() As String
        Return CatalogName
    End Function

End Structure