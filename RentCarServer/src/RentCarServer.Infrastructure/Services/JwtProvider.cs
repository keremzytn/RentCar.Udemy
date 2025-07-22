using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Users;
using RentCarServer.Infrastructure.Options;

namespace RentCarServer.Infrastructure.Services;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public string CreateToken(User user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrEmpty(_options.SecretKey))
            throw new InvalidOperationException("JWT SecretKey is not configured");

        if (string.IsNullOrEmpty(_options.Issuer))
            throw new InvalidOperationException("JWT Issuer is not configured");

        if (string.IsNullOrEmpty(_options.Audience))
            throw new InvalidOperationException("JWT Audience is not configured");

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.Value.ToString()),
            new Claim("fullName", user.FullName?.Value ?? $"{user.FirstName.Value} {user.LastName.Value}"),
            new Claim("email", user.Email?.Value ?? string.Empty)
        };

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_options.SecretKey));
        SigningCredentials signingCredentials = new(securityKey,
        SecurityAlgorithms.HmacSha256);

        JwtSecurityToken securityToken = new(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: signingCredentials
        );

        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(securityToken);
        return token;
    }
}