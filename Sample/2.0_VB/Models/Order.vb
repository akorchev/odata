Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace MyApp.Models.Sample
    <Table("Orders", Schema:="dbo")>
    Public Class Order
        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Id() As Integer

        <InverseProperty("Order")>
        Public Property OrderDetails() As ICollection(Of OrderDetail)

        Public Property OrderDate() As DateTime

        Public Property UserName() As String

    End Class
End Namespace
