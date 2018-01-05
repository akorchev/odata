Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.AspNetCore
Imports Microsoft.AspNetCore.Builder

Module Program
    Sub Main(args As String())
        BuildWebHost(args).Run()
    End Sub

    Function BuildWebHost(args As String()) As IWebHost
        Return WebHost.CreateDefaultBuilder(args).UseUrls("http://localhost:5000").UseStartup(Of Startup)().Build()
    End Function
End Module