namespace Todo.Infrastructure.Authentication;

internal sealed class TokenProvider(IConfiguration configuration) : ITokenProvider
{
    public string Create(User user)
    {
        string secretKey = configuration["Jwt:Secret"]!;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            ]),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes"))
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}
