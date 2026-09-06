using Azure.Core;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Endpoints;

public class Users : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        //groupBuilder.MapIdentityApi<ApplicationUser>();
        groupBuilder.MapPost(Logout, "logout").RequireAuthorization();
        groupBuilder.MapPost(SignIn, "SignIn");

    }

    [EndpointSummary("Log out")]
    public static async Task<Results<Ok, UnauthorizedHttpResult>> Logout(SignInManager<ApplicationUser> signInManager, [FromBody] object empty)
    {
        if (empty != null)
        {
            await signInManager.SignOutAsync();
            return TypedResults.Ok();
        }

        return TypedResults.Unauthorized();
    }

    public static async Task<AuthenticateResponse> SignIn([FromBody] AuthenticateRequest request, SignInManager<ApplicationUser> signInManager,
        ITokenClaimsService tokenClaimsService)
    {
        var response = new AuthenticateResponse(Guid.NewGuid());

        var result = await signInManager.PasswordSignInAsync(request.Username, request.Password, false, true);

        response.Result = result.Succeeded;
        response.IsLockedOut = result.IsLockedOut;
        response.IsNotAllowed = result.IsNotAllowed;
        response.RequiresTwoFactor = result.RequiresTwoFactor;
        response.Username = request.Username;

        if (result.Succeeded)
        {
            response.Token = await tokenClaimsService.GetTokenAsync(request.Username);
        }

        return response;
    }

    public class AuthenticateRequest 
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class AuthenticateResponse 
    {
        public AuthenticateResponse(Guid correlationId) // base(correlationId)
        {
        }

        public AuthenticateResponse()
        {
        }
        public bool Result { get; set; } = false;
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsLockedOut { get; set; } = false;
        public bool IsNotAllowed { get; set; } = false;
        public bool RequiresTwoFactor { get; set; } = false;
    }
}
