using CSharpFunctionalExtensions;
using AuthService.Contracts.Demo;
using Shared;

namespace AuthService.Application.Features.Demo.GetDemo;

public sealed record GetDemoQuery;

public sealed class GetDemoQueryHandler
{
    public static Result<GetDemoResponse, Errors> Handle(GetDemoQuery query)
    {
        GetDemoResponse response = new(
            Service: "AuthService",
            Status: "OK");

        return Result.Success<GetDemoResponse, Errors>(response);
    }
}
