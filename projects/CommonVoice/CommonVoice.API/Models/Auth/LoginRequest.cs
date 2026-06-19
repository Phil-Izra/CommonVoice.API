namespace CommonVoice.API.Models.Auth;

using System.ComponentModel.DataAnnotations;

public sealed record LoginRequest(
    [Required][EmailAddress] string Email,
    [Required][MinLength(8)] string Password
);
