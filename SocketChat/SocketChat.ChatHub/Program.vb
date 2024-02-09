Imports System
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Rewrite
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports SocketChat.Business
Imports SocketChat.DataAccess

Module Program
    Sub Main(args As String())
        Dim builder = WebApplication.CreateBuilder(args)



        builder.Services.AddControllers()
        builder.Services.AddMemoryCache()
        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen()


        builder.Services.AddAuthentication(JwtBearerDefaults)

        builder.Services.AddSingleton(Of ICacheModule, CacheModule)

        builder.Services.AddDataAccesServices()

        builder.Services.AddBusinessServices()


        Dim manager = ChatHubManagerFactory.Create()


        Dim app = builder.Build()

        app.UseRewriter(New RewriteOptions().AddRedirect("^$", "swagger"))


        If app.Environment.IsDevelopment() Then
            app.UseSwagger()
            app.UseSwaggerUI()
        End If

        app.UseSwagger()
        app.UseSwaggerUI()

        app.UseWebSockets()


        'app.UseHttpsRedirection()
        app.UseCors(Function(x) x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin())
        app.UseAuthorization()

        app.MapControllers()

        app.Run()
    End Sub
End Module
