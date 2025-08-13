using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using myBookSolution.Communication.Responses;
using myBookSolution.Domain.Repositories.User;
using myBookSolution.Domain.Security.Tokens;
using myBookSolution.Exceptions;
using myBookSolution.Exceptions.ExceptionsBase;
using MyRecipeBook.Domain.Extensions;

namespace myBookSolution.API.Filters
{
    public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
    {
        private readonly IAccessTokenValidator _accessTokenValidator;
        private readonly IUserReadOnlyRepository _repository;

        public AuthenticatedUserFilter(IAccessTokenValidator accessTokenValidator,
            IUserReadOnlyRepository repository)
        {
            _accessTokenValidator = accessTokenValidator;
            _repository = repository;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var token = TokenOnRequest(context);

                var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);

                var exist = await _repository.ExistsActiveUserWithIdentifier(userIdentifier);

                if (exist.IsFalse())
                {
                    throw new UnauthorizedException(ResourceMessagesExceptions.USUARIO_SEM_PERMISSAO);
                }
            }
            catch (SecurityTokenExpiredException)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseError("Token expirado")
                {
                    TokenIsExpired = true
                });
            }
            catch (SolutionExceptions ex)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseError(ex.Message));
            }
            catch
            {
                context.Result = new UnauthorizedObjectResult(new ResponseError(ResourceMessagesExceptions.USUARIO_SEM_PERMISSAO));
            }
        }

        private static string TokenOnRequest(AuthorizationFilterContext context)
        {
            var authentication = context.HttpContext.Request.Headers.Authorization.ToString(); //O ERRO ACONTECE AQUI QUANDO TENTO REGISTRAR
            if (string.IsNullOrEmpty(authentication))
            {
                throw new UnauthorizedException(ResourceMessagesExceptions.SEM_TOKEN);

            }

            return authentication["Bearer ".Length..].Trim();
        }
    }
}
