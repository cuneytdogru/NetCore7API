using Microsoft.AspNetCore.Authorization;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Services;
using System.IdentityModel.Tokens.Jwt;

namespace NetCore7API.Authorization
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;

        public JwtSecurityToken? Token { get; private set; }

        public Guid? UserId { get; private set; }

        public TokenService(IHttpContextAccessor contextAccessor, ILogger<TokenService> logger)
        {
            ArgumentNullException.ThrowIfNull(contextAccessor, nameof(contextAccessor));
            ArgumentNullException.ThrowIfNull(contextAccessor.HttpContext, nameof(contextAccessor.HttpContext));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            _logger = logger;

            var context = contextAccessor.HttpContext;

            if (!context.GetEndpoint().Metadata.Any(x => x is AuthorizeAttribute))
            {
                return;
            }

            if (!context.Items.TryGetValue("Authorization", out var authorization))
                throw new TokenException("Failed to get Authorization!");

            if (authorization == null)
                throw new TokenException("Authorization is null!");

            var token = authorization.ToString();

            if (string.IsNullOrWhiteSpace(token))
                throw new TokenException("Token is empty!");

            this.Token = new JwtSecurityToken(jwtEncodedString: token);
            if (this.Token == null)
                throw new TokenException("Token is invalid!");

            this.UserId = Guid.Parse(Token.Id);
        }
    }
}