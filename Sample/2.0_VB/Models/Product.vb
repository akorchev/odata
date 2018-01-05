Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace MyApp.Models.Sample
    <Table("Products", Schema:="dbo")>
    Public Class Product
        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Id() As Integer

        <InverseProperty("Product")>
        Public Property OrderDetails() As ICollection(Of OrderDetail)

        Public Property ProductName() As String

        Public Property ProductPicture() As String

        Public Property ProductPrice() As Decimal

    End Class
End Namespace
