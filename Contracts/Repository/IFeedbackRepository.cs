using System.Threading.Tasks;
using Domain.Entities;

namespace Contracts.Repository
{
    public interface IFeedbackRepository
    {
        void CreateFeedback(Feedback feedback);
        Task CreateFeedbackAsync(Feedback feedback);
    }
}