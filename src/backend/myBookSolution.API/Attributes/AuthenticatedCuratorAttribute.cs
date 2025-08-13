using Microsoft.AspNetCore.Mvc;
using myBookSolution.API.Filters;

namespace myBookSolution.API.Attributes;

public class AuthenticatedCuratorAttribute : TypeFilterAttribute
{
    public AuthenticatedCuratorAttribute() : base(typeof(AuthenticatedCuratorFilter))
    {
    }
}
