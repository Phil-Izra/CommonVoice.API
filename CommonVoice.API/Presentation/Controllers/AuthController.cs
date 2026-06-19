namespace CommonVoice.API.Presentation.Controllers;

using CommonVoice.API.Application.UseCases.Users;
using CommonVoice.API.Domain.Aggregates.Users;
using CommonVoice.API.Models.Auth;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    RegisterUserUseCase     registerUser,
    LoginUserUseCase        loginUser,
    ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req, CancellationToken ct)
    {
        logger.LogDebug("Register request for {Email}", req.Email);

        var result = await registerUser.ExecuteAsync(
            new RegisterUserCommand(req.Name, req.Email, req.Password, req.Role, req.Sector, req.Province), ct);

        return CreatedAtAction(nameof(Register), new { result.UserId },
            new { result.UserId, result.Email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken ct)
    {
        logger.LogDebug("Login attempt for {Email}", req.Email);

        var result = await loginUser.ExecuteAsync(new LoginUserCommand(req.Email, req.Password), ct);

        var role = Enum.Parse<UserRole>(result.Role);
        return Ok(new AuthResponse(result.Token, result.ExpiresAt, result.UserId, result.Name, role));
    }
}
