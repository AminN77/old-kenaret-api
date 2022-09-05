using System;
using BusinessLogic.Contracts;
using Contracts.LoggerManager;
using Domain.Events;
using Newtonsoft.Json;

namespace Infrastructure.EventHandlers
{
    public class ElasticEventHandler : IEventHandler
    {
        private readonly ILoggerManager _loggerManager;
        private readonly ILiveStreamBusinessLogic _liveStreamBusinessLogic;
        private readonly IUserBusinessLogic _userBusinessLogic;
        

        public ElasticEventHandler(ILoggerManager loggerManager, ILiveStreamBusinessLogic liveStreamBusinessLogic, IUserBusinessLogic userBusinessLogic)
        {
            _loggerManager = loggerManager;
            _liveStreamBusinessLogic = liveStreamBusinessLogic;
            _userBusinessLogic = userBusinessLogic;
        }

        public void Subscribe()
        {
            _liveStreamBusinessLogic.LiveStreamEventOccured += OnDomainEventOccured;
            _userBusinessLogic.UserEventOccured += OnDomainEventOccured;
        }
        
        // Domain-event-logs for log in elasticsearch 
        private void OnDomainEventOccured(object sender, EventArgs e, Guid requestId)
        {
            var eventToLog = new EventData
            {
                EventId = Guid.NewGuid(),
                Data = Serialize(e),
                Type = e.GetType().Name,
                MetaData = Serialize(new EventMetaData
                {
                    ClrType = e.GetType().AssemblyQualifiedName
                })
            };
            
            _loggerManager.LogInfo(Serialize(eventToLog), requestId);    
        }
        
        private static string Serialize(object data) => JsonConvert.SerializeObject(data);
    }
}