Imports System
Imports System.Security.Claims
Imports System.Text
Imports Microsoft.AspNetCore.Authentication.JwtBearer
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Rewrite
Imports Microsoft.Extensions.Caching.Memory
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports Microsoft.IdentityModel.Tokens
Imports SocketChat.Business
Imports SocketChat.DataAccess

Module Program
    Sub Main(args As String())
        Dim builder = WebApplication.CreateBuilder(args)



        builder.Services.AddControllers()
        builder.Services.AddMemoryCache()
        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen()

        builder.Services.AddAuthorization()

        Dim tokenOptions = builder.Configuration.GetSection("TokenOptions").Get(Of TokenOptions)()

        builder.Services.AddAuthorization()

        builder.Services.AddAuthentication(Function(opt)
                                               opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme
                                               opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme
                                               opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme
                                           End Function).AddJwtBearer(Function(opt)
                                                                          opt.TokenValidationParameters = New Microsoft.IdentityModel.Tokens.TokenValidationParameters With
                                                                          {
                                                                            .ValidateIssuer = True,
                                                                            .ValidateAudience = True,
                                                                            .ValidateLifetime = True,
                                                                            .ValidateIssuerSigningKey = True,
                                                                            .ValidIssuer = tokenOptions.Issuer,
                                                                            .ValidAudience = tokenOptions.Audience,
                                                                            .IssuerSigningKey = New SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
                                                                            .ClockSkew = TimeSpan.FromDays(1)
                                                                          }
                                                                      End Function)

        builder.Services.AddAutoMapper(GetType(MainProfile))
        builder.Services.AddSingleton(Of ICacheModule, CacheModule)

        Dim provider As ModuleProvider = ModuleProvider.Instance




        builder.Services.AddAutoMapper(GetType(MainProfile))


        builder.Services.AddDataAccesServices()

        builder.Services.AddBusinessServices()


        Dim manager = ChatHubManagerFactory.Create()


        Dim app = builder.Build()
        app.UseCors(Function(cors)
                        cors.AllowAnyMethod() _
                    .AllowAnyHeader() _
                    .SetIsOriginAllowed(Function(origin) True) _
                    .AllowCredentials()
                    End Function)


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

        app.UseAuthentication()
        app.UseAuthorization()


        app.MapControllers()

        app.UseCustomException()
        app.Run()
    End Sub
End Module
