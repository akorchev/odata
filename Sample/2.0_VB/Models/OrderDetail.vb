Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace MyApp.Models.Sample
    <Table("OrderDetails", Schema:="dbo")>
    Public Class OrderDetail
        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Id() As Integer

        Public Property OrderId() As System.Nullable(Of Integer)

        <ForeignKey("OrderId")>
        Public Property Order() As Order

        Public Property ProductId() As System.Nullable(Of Integer)

        <ForeignKey("ProductId")>
        Public Property Product() As Product

        Public Property Quantity() As Integer
    End Class
End Namespace
