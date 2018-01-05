Imports Newtonsoft.Json.Linq
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.AspNet.OData
Imports Microsoft.AspNet.OData.Routing
Imports Microsoft.EntityFrameworkCore

Namespace MyApp.Controllers.Sample

    <EnableQuery>
    <ODataRoutePrefix("odata/Sample/Products")>
    Partial Public Class ProductsController
        Inherits Controller
        Private context As Data.SampleContext

        Public Sub New(context As Data.SampleContext)
            Me.context = context
        End Sub

        <HttpGet>
        Public Function GetProducts() As IEnumerable(Of Models.Sample.Product)
            Dim items = Me.context.Products.AsQueryable()

            Me.OnProductsRead(items)

            Return items
        End Function

        Partial Private Sub OnProductsRead(ByRef items As IQueryable(Of Models.Sample.Product))
        End Sub

        <HttpGet("{Id}")>
        Public Function GetProduct(key As Integer) As IActionResult
            Dim item = Me.context.Products.Where(Function(i) i.Id = key).SingleOrDefault()

            If item Is Nothing Then
                Return NotFound()
            End If

            Return New ObjectResult(item)
        End Function
        Partial Private Sub OnProductDeleted(item As Models.Sample.Product)
        End Sub

        <HttpDelete("{Id}")>
        Public Function DeleteProduct(key As Integer) As IActionResult
            Dim item = Me.context.Products.Where(Function(i) i.Id = key).Include(Function(i) i.OrderDetails).SingleOrDefault()

            If item Is Nothing Then
                Return NotFound()
            End If

            Me.OnProductDeleted(item)
            Me.context.Products.Remove(item)
            Me.context.SaveChanges()

            Return New NoContentResult()
        End Function

        Partial Private Sub OnProductUpdated(item As Models.Sample.Product)
        End Sub

        <HttpPut("{Id}")>
        Public Function PutProduct(key As Integer, <FromBody> newItem As Models.Sample.Product) As IActionResult
            If newItem Is Nothing OrElse newItem.Id <> key Then
                Return BadRequest()
            End If

            Me.OnProductUpdated(newItem)
            Me.context.Products.Update(newItem)
            Me.context.SaveChanges()

            Return New NoContentResult()
        End Function

        <HttpPatch("{Id}")>
        Public Function PatchProduct(key As Integer, <FromBody> patch As JObject) As IActionResult
            Dim item = Me.context.Products.Where(Function(i) i.Id = key).FirstOrDefault()

            If item Is Nothing Then
                Return BadRequest()
            End If

            Data.EntityPatch.Apply(item, patch)

            Me.OnProductUpdated(item)
            Me.context.Products.Update(item)
            Me.context.SaveChanges()

            Return New NoContentResult()
        End Function

        Partial Private Sub OnProductCreated(item As Models.Sample.Product)
        End Sub

        <HttpPost>
        Public Function Post(<FromBody> item As Models.Sample.Product) As IActionResult
            If item Is Nothing Then
                Return BadRequest()
            End If

            Me.OnProductCreated(item)
            Me.context.Products.Add(item)
            Me.context.SaveChanges()

            Return Created("odata/Sample/Products/{item.Id}", item)
        End Function

    End Class

End Namespace
