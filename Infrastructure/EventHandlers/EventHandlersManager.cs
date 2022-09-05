using System.Collections.Generic;
using BusinessLogic.Contracts;
using Contracts.LoggerManager;

namespace Infrastructure.EventHandlers
{
    public class EventHandlersManager
    {
        private readonly IList<IEventHandler> _eventHandlers;
        public EventHandlersManager(ILoggerManager loggerManager, ILiveStreamBusinessLogic liveStreamBusinessLogic, IUserBusinessLogic userBusinessLogic)
        {
            _eventHandlers = new List<IEventHandler>
            {
                new ElasticEventHandler(loggerManager, liveStreamBusinessLogic, userBusinessLogic)
            };
            
            SubscribeAll();
        }

        private void SubscribeAll()
        {
            foreach (var eventHandler in _eventHandlers)
            {
                eventHandler.Subscribe();
            }
        }
    }
}