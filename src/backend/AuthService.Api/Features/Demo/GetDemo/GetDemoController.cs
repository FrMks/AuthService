using AuthService.Application.Features.Demo.GetDemo;
using AuthService.Contracts.Demo;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Shared;
using Shared.Framework.EndpointResults;
using Wolverine;

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
        [FromServices] IMessageBus bus,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(bus);

        Result<GetDemoResponse, Errors> result =
            await bus.InvokeAsync<Result<GetDemoResponse, Errors>>(
                new GetDemoQuery(),
                cancellationToken).ConfigureAwait(false);

        if (result.IsFailure)
            return result.ConvertFailure<GetDemoResponse>();

        return result;
    }
}
