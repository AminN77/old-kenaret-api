using System.Threading.Tasks;

namespace Contracts.Repository
{
    public interface IRepositoryManager
    {
        IWorldRepository World { get; }
        IUserRepository User { get; }
        ILiveStreamRepository LiveStream { get; }
        ILinkRepository Link { get; }
        IOtpCodeRepository OtpCode { get; }
        IParticipantsRepository Participants { get; }
        IFeedbackRepository Feedbacks { get; }
        INodeRepository Nodes { get; }
        Task SaveAsync();
    }
}