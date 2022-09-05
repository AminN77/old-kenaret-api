using System;
using System.Threading.Tasks;
using BusinessLogic.Contracts;
using BusinessLogic.DataTransferObjects.Feedback;
using Contracts.LoggerManager;
using Contracts.Mapper;
using Contracts.Repository;
using Domain.Entities;

namespace BusinessLogic.Implementation
{
    public class FeedbackBusinessLogic : IFeedbackBusinessLogic
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        
        public FeedbackBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager loggerManager)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        

        public async Task<IBusinessLogicResult> CreateFeedbackAsync(FeedbackForCreationDto feedbackForCreationDto, Guid requestId)
        {
            var feedback = await _mapper.MapAsync(feedbackForCreationDto, new Feedback());
            feedback.CreateDateTime = DateTime.UtcNow;
            await _repositoryManager.Feedbacks.CreateFeedbackAsync(feedback);
            await _repositoryManager.SaveAsync();
            return new BusinessLogicResult{Success = true};
        }
    }
}