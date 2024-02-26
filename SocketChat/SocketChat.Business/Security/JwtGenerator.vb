Imports System.IdentityModel.Tokens.Jwt
Imports System.Security.Claims
Imports System.Text
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.IdentityModel.Tokens

Public Class JwtGenerator

    Private ReadOnly _configuration As IConfiguration

    Private _options As New TokenOptions
    Private _expiration As DateTime


    Public Sub New(configuration As IConfiguration)
        _configuration = configuration
        Dim conf = _configuration.GetSection("TokenOptions")
        _options.Audience = conf.GetSection("Audience").Value
        _options.Issuer = conf.GetSection("Issuer").Value
        _options.SecurityKey = conf.GetSection("SecurityKey").Value
        _options.TokenExpiration = Integer.Parse(conf.GetSection("TokenExpiration").Value)
    End Sub

    Public Function CreateAccessToken(Optional claims As IEnumerable(Of Claim) = Nothing) As AccessToken
        _expiration = DateTime.Now.AddMinutes(_options.TokenExpiration)

        Dim securityKey = New SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecurityKey))
        Dim signingCredentials = New SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)

        Dim jwt = CreateJwtToken(_options, signingCredentials, claims)

        Dim jwtHandler = New JwtSecurityTokenHandler()
        Dim token = jwtHandler.WriteToken(jwt)

        Return New AccessToken With {
            .Expiration = _expiration,
            .Token = token
        }

    End Function

    Private Function CreateJwtToken(tokenOptions As TokenOptions, signingCredentials As SigningCredentials, Optional claims As IEnumerable(Of Claim) = Nothing) As JwtSecurityToken
        Dim jwtToken = New JwtSecurityToken(issuer:=tokenOptions.Issuer, audience:=tokenOptions.Audience, expires:=_expiration, notBefore:=DateTime.Now, signingCredentials:=signingCredentials, claims:=claims)
        Return jwtToken
    End Function
End Class
