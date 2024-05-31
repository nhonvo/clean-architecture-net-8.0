using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Services;

public class CurrentTime : ICurrentTime
{
    public DateTime GetCurrentTime() => DateTime.UtcNow;
}
