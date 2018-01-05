Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNet.OData.Builder
Imports Microsoft.AspNet.OData.Extensions
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.OData.Edm

Class Startup
    Sub ConfigureServices(Services As IServiceCollection)
        Services.AddOData()
        Services.AddODataQueryFilter()

        Services.AddDbContext(Of MyApp.Data.SampleContext)(Sub(options) options.UseSqlServer("Server=localhost;Initial Catalog=Sample;Persist Security Info=False;User ID=sa;Password=passw0rdMSSQL;MultipleActiveResultSets=False;Encrypt=false;TrustServerCertificate=true;Connection Timeout=30"))

    End Sub

    Sub Configure(app As IApplicationBuilder)
        Dim provider As IServiceProvider = app.ApplicationServices.GetRequiredService(Of IServiceProvider)

        app.UseMvc(Sub(builder)
                       builder.Count().Filter().OrderBy().Expand().[Select]().MaxTop(Nothing)

                       builder.MapODataServiceRoute("odata/Sample", "odata/Sample", GetSampleEdmModel(provider))

                   End Sub)
    End Sub

    Private Shared Function GetSampleEdmModel(ByVal provider As IServiceProvider) As IEdmModel
        Dim builder = New ODataConventionModelBuilder(provider)
        builder.ContainerName = "SampleContext"
        builder.EntitySet(Of MyApp.Models.Sample.Product)("Products")
        builder.EntitySet(Of MyApp.Models.Sample.Order)("Orders")
        builder.EntitySet(Of MyApp.Models.Sample.OrderDetail)("OrderDetails")
        Return builder.GetEdmModel()
    End Function
End Class