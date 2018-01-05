Imports Microsoft.EntityFrameworkCore
Imports Sample20VB.MyApp.Models.Sample

Namespace MyApp.Data
    Partial Public Class SampleContext
        Inherits DbContext
        Public Sub New(options As DbContextOptions(Of SampleContext))
            MyBase.New(options)
        End Sub

        Public Sub New()
        End Sub

        Public Property Orders() As DbSet(Of Order)

        Public Property OrderDetails() As DbSet(Of OrderDetail)

        Public Property Products() As DbSet(Of Product)

    End Class
End Namespace
