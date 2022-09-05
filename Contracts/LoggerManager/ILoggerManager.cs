using System;

namespace Contracts.LoggerManager
{
    public interface ILoggerManager
    {
        void LogInfo(string message, Guid requestId, bool isLastLog = false);
        void LogDebug(string message, Guid requestId, bool isLastLog = false);
        void LogError(string message, Guid requestId, bool isLastLog = true);

    }
}