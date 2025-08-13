using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using myBookSolution.Communication.Responses;
using myBookSolution.Exceptions;
using myBookSolution.Exceptions.ExceptionsBase;
using System.Net;

namespace myBookSolution.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is SolutionExceptions solutionExceptions)
            HandleProjectException(solutionExceptions, context);
        else
            ThrowUnknownException(context);
    }

    private void HandleProjectException(SolutionExceptions solutionExceptions, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)solutionExceptions.GetStatusCode();
        context.Result = new ObjectResult(new ResponseError(solutionExceptions.GetErrorMessages()));
    }

    private void ThrowUnknownException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new Communication.Responses.ResponseError(ResourceMessagesExceptions.ERRO_DESCONHECIDO));
        
    }

}