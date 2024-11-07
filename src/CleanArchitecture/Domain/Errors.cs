using System.Text.Json;

namespace CleanArchitecture.Domain;
public record Error(string? Code, string Message, Guid ErrorId)
{
    public static implicit operator string(Error error) => JsonSerializer.Serialize(error);
};
