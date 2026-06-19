namespace CommonVoice.API.Models.Auth;

using System.ComponentModel.DataAnnotations;
using CommonVoice.API.Domain.Aggregates.Users;

public sealed record RegisterRequest(
    [Required] string   Name,
    [Required][EmailAddress] string Email,
    [Required][MinLength(8)] string Password,
    [Required] UserRole Role,
    string? Sector   = null,   // required when Role == User
    string? Province = null    // required when Role == User
);
