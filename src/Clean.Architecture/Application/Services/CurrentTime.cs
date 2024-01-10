using Clean.Architecture.Application.Common.Interfaces;

namespace Clean.Architecture.Application.Services
{
    public class CurrentTime : ICurrentTime
    {
        public DateTime GetCurrentTime() => DateTime.UtcNow;
    }
}
