using System;
using System.Threading.Tasks;
using BusinessLogic.DataTransferObjects.Feedback;

namespace BusinessLogic.Contracts
{
    public interface IFeedbackBusinessLogic
    {
        Task<IBusinessLogicResult> CreateFeedbackAsync(FeedbackForCreationDto feedbackForCreationDto, Guid requestId);
    }
}