using CSharpFunctionalExtensions;
using AuthService.Contracts.Demo;
using Shared.Core.Abstractions;
using Shared;

namespace AuthService.Application.Features.Demo.GetDemo;

public sealed record GetDemoQuery;

public sealed class GetDemoQueryHandler
    : IQueryHandler<GetDemoQuery, Result<GetDemoResponse, Errors>>
{
    public Task<Result<GetDemoResponse, Errors>> Handle(
        GetDemoQuery query,
        CancellationToken cancellationToken)
    {
        GetDemoResponse response = new(
            Service: "AuthService",
            Status: "demo feature works");

        return Task.FromResult(
            Result.Success<GetDemoResponse, Errors>(response));
    }
}
