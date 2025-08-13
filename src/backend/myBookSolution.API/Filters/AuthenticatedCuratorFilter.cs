using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using myBookSolution.Domain.Repositories.Curator;
using myBookSolution.Domain.Security.Tokens;
using myBookSolution.Exceptions;
using myBookSolution.Exceptions.ExceptionsBase;
using MyRecipeBook.Domain.Extensions;

namespace myBookSolution.API.Filters;

public class AuthenticatedCuratorFilter : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _accessTokenValidator;
    private readonly ICuratorReadOnlyRepository _repository;

    public AuthenticatedCuratorFilter(IAccessTokenValidator accessTokenValidator,
        ICuratorReadOnlyRepository repository)
    {
        _accessTokenValidator = accessTokenValidator;
        _repository = repository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);

            var curatorIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);

            var exist = await _repository.ExistsActiveCuratorWithIdentifier(curatorIdentifier);

            if (exist.IsFalse())
            {
                throw new UnauthorizedException(ResourceMessagesExceptions.USUARIO_SEM_PERMISSAO); ;
            }
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new Communication.Responses.ResponseError("Token expirado")
            {
                TokenIsExpired = true
            });
        }
        catch (SolutionExceptions ex)
        {
            context.Result = new UnauthorizedObjectResult(new Communication.Responses.ResponseError(ex.Message));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(new Communication.Responses.ResponseError(ResourceMessagesExceptions.USUARIO_SEM_PERMISSAO));
        }
    }

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authentication))
        {
            throw new UnauthorizedException(ResourceMessagesExceptions.SEM_TOKEN);

        }

        return authentication["Bearer ".Length..].Trim();
    }
}

