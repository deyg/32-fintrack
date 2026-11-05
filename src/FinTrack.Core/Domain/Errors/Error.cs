namespace FinTrack.Core.Domain.Errors;

public sealed record Error(string Code, string Message, ErrorType Type);
