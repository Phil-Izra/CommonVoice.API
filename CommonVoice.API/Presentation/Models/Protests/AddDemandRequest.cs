namespace CommonVoice.API.Presentation.Models.Protests;

using System.ComponentModel.DataAnnotations;

public sealed record AddDemandRequest([Required][MinLength(5)] string Description);
