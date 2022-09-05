using System;
using Contracts.LoggerManager;
using Newtonsoft.Json;
using Serilog;

namespace Infrastructure.Logger
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILogger _logger = Log.Logger;
        private readonly LogStack _logStack;


        public LoggerManager(LogStack logStack)
        {
            _logStack = logStack;
        }

        public void LogInfo(string message, Guid requestId, bool isLastLog = false)
        {
            _logger.Information(message);
            if (isLastLog is true)
            {
                _logStack.PopAll(requestId);
            }
            else
            {
                _logStack.Push(message, requestId);
            }
        }

        public void LogDebug(string message, Guid requestId, bool isLastLog = false)
        {
            _logger.Debug(message);
            _logStack.Push(message, requestId);
            if (isLastLog is true)
            {
                _logStack.PopAll(requestId);
            }
            else
            {
                _logStack.Push(message, requestId);
            }
        }

        public void LogError(string message, Guid requestId, bool isLastLog = true)
        {
            var popStack = new
            {
                error = message,
                relatedLogs = JsonConvert.SerializeObject(_logStack.GetStack(requestId))
            };

            _logger.Error(JsonConvert.SerializeObject(popStack));
            if (isLastLog is true)
            {
                _logStack.PopAll(requestId);
            }
        }
    }
}