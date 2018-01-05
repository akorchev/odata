Imports Newtonsoft.Json.Linq
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.AspNet.OData
Imports Microsoft.AspNet.OData.Routing

Namespace MyApp.Controllers.Sample

    <EnableQuery>
    <ODataRoutePrefix("odata/Sample/OrderDetails")>
    Partial Public Class OrderDetailsController
        Inherits Controller
        Private context As Data.SampleContext

        Public Sub New(context As Data.SampleContext)
            Me.context = context
        End Sub

        <HttpGet>
        Public Function GetOrderDetails() As IEnumerable(Of Models.Sample.OrderDetail)
            Dim items = Me.context.OrderDetails.AsQueryable()

            Me.OnOrderDetailsRead(items)

            Return items
        End Function

        Partial Private Sub OnOrderDetailsRead(ByRef items As IQueryable(Of Models.Sample.OrderDetail))
        End Sub

        <HttpGet("{Id}")>
        Public Function GetOrderDetail(key As Integer) As IActionResult
            Dim item = Me.context.OrderDetails.Where(Function(i) i.Id = key).SingleOrDefault()

            If item Is Nothing Then
                Return NotFound()
            End If

            Return New ObjectResult(item)
        End Function

        Partial Private Sub OnOrderDetailDeleted(item As Models.Sample.OrderDetail)
        End Sub

        <HttpDelete("{Id}")>
        Public Function DeleteOrderDetail(key As Integer) As IActionResult
            Dim item = Me.context.OrderDetails.Where(Function(i) i.Id = key).SingleOrDefault()

            If item Is Nothing Then
                Return NotFound()
            End If

            Me.OnOrderDetailDeleted(item)
            Me.context.OrderDetails.Remove(item)
            Me.context.SaveChanges()

            Return New NoContentResult()
        End Function

        Partial Private Sub OnOrderDetailUpdated(item As Models.Sample.OrderDetail)
        End Sub

        <HttpPut("{Id}")>
        Public Function PutOrderDetail(key As Integer, <FromBody> newItem As Models.Sample.OrderDetail) As IActionResult
            If newItem Is Nothing OrElse newItem.Id <> key Then
                Return BadRequest()
            End If

            Me.OnOrderDetailUpdated(newItem)
            Me.context.OrderDetails.Update(newItem)
            Me.context.SaveChanges()

            Return New NoContentResult()
        End Function

        <HttpPatch("{Id}")>
        Public Function PatchOrderDetail(key As Integer, <FromBody> patch As JObject) As IActionResult
            Dim item = Me.context.OrderDetails.Where(Function(i) i.Id = key).FirstOrDefault()

            If item Is Nothing Then
                Return BadRequest()
            End If

            Data.EntityPatch.Apply(item, patch)

            Me.OnOrderDetailUpdated(item)
            Me.context.OrderDetails.Update(item)
            Me.context.SaveChanges()

            Return New NoContentResult()
        End Function

        Partial Private Sub OnOrderDetailCreated(item As Models.Sample.OrderDetail)
        End Sub

        <HttpPost>
        Public Function Post(<FromBody> item As Models.Sample.OrderDetail) As IActionResult
            If item Is Nothing Then
                Return BadRequest()
            End If

            Me.OnOrderDetailCreated(item)
            Me.context.OrderDetails.Add(item)
            Me.context.SaveChanges()

            Return Created("odata/Sample/OrderDetails/{item.Id}", item)
        End Function

    End Class

End Namespace
