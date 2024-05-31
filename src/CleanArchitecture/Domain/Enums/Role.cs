using System.Text.Json.Serialization;

namespace CleanArchitecture.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    Admin,
    User
}
