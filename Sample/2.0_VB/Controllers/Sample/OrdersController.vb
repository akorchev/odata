Imports Newtonsoft.Json.Linq
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.AspNet.OData
Imports Microsoft.AspNet.OData.Routing
Imports Microsoft.EntityFrameworkCore

Namespace MyApp.Controllers.Sample

    <EnableQuery>
    <ODataRoutePrefix("odata/Sample/Orders")>
    Partial Public Class OrdersController
        Inherits ODataController
        Private context As Data.SampleContext

        Public Sub New(context As Data.SampleContext)
            Me.context = context
        End Sub

        <HttpGet>
        Public Function [Get]() As IEnumerable(Of Models.Sample.Order)
            Dim items = Me.context.Orders.AsQueryable()

            Me.OnOrdersRead(items)

            Return items
        End Function

        Partial Private Sub OnOrdersRead(ByRef items As IQueryable(Of Models.Sample.Order))
        End Sub

        <HttpGet("{Id}")>
        Public Function GetOrder(key As Integer) As IActionResult
            Dim item = Me.context.Orders.Where(Function(i) i.Id = key).SingleOrDefault()

            If item Is Nothing Then
                Return NotFound()
            End If

            Return New ObjectResult(item)
        End Function
        Partial Private Sub OnOrderDeleted(item As Models.Sample.Order)
        End Sub

        <HttpDelete("{Id}")>
        Public Function DeleteOrder(key As Integer) As IActionResult
            Dim item = Me.context.Orders.Where(Function(i) i.Id = key).Include(Function(i) i.OrderDetails).SingleOrDefault()

            If item Is Nothing Then
                Return NotFound()
            End If

            Me.OnOrderDeleted(item)
            Me.context.Orders.Remove(item)
            Me.context.SaveChanges()

            Return New NoContentResult()
        End Function

        Partial Private Sub OnOrderUpdated(item As Models.Sample.Order)
        End Sub

        <HttpPut("{Id}")>
        Public Function PutOrder(key As Integer, <FromBody> newItem As Models.Sample.Order) As IActionResult
            If newItem Is Nothing OrElse newItem.Id <> key Then
                Return BadRequest()
            End If

            Me.OnOrderUpdated(newItem)
            Me.context.Orders.Update(newItem)
            Me.context.SaveChanges()

            Return New NoContentResult()
        End Function

        <HttpPatch("{Id}")>
        Public Function PatchOrder(key As Integer, <FromBody> patch As JObject) As IActionResult
            Dim item = Me.context.Orders.Where(Function(i) i.Id = key).FirstOrDefault()

            If item Is Nothing Then
                Return BadRequest()
            End If

            Data.EntityPatch.Apply(item, patch)

            Me.OnOrderUpdated(item)
            Me.context.Orders.Update(item)
            Me.context.SaveChanges()

            Return New NoContentResult()
        End Function

        Partial Private Sub OnOrderCreated(item As Models.Sample.Order)
        End Sub

        <HttpPost>
        Public Function Post(<FromBody> item As Models.Sample.Order) As IActionResult
            If item Is Nothing Then
                Return BadRequest()
            End If

            Me.OnOrderCreated(item)
            Me.context.Orders.Add(item)
            Me.context.SaveChanges()

            Return Created("odata/Sample/Orders/{item.Id}", item)
        End Function

    End Class

End Namespace
