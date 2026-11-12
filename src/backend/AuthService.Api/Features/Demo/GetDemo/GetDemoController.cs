using AuthService.Application.Features.Demo.GetDemo;
using AuthService.Contracts.Demo;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Shared.Core.Abstractions;
using Shared;
using Shared.Framework.EndpointResults;

namespace AuthService.Api.Features.Demo.GetDemo;

[ApiController]
[Route("api/demo")]
[SuppressMessage(
    "Design",
    "CA1515",
    Justification = "ASP.NET Core MVC discovers controllers through public types.")]
public sealed class GetDemoController : ControllerBase
{
    [HttpGet]
    public async Task<EndpointResult<GetDemoResponse>> Get(
        [FromServices] IQueryHandler<GetDemoQuery, Result<GetDemoResponse, Errors>> handler,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(handler);

        Result<GetDemoResponse, Errors> result = await handler.Handle(
            new GetDemoQuery(),
            cancellationToken).ConfigureAwait(false);

        if (result.IsFailure)
            return result.ConvertFailure<GetDemoResponse>();

        return result;
    }
}
