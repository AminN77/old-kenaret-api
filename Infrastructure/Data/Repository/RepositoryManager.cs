using System.Threading.Tasks;
using Contracts.Repository;
using Infrastructure.Data.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly PgSqlDbContext _repositoryContext;
        private IWorldRepository _worldRepository;
        private IUserRepository _userRepository;
        private ILiveStreamRepository _liveStreamRepository;
        private ILinkRepository _linkRepository;
        private IOtpCodeRepository _otpCodeRepository;
        private IParticipantsRepository _participantsRepository;
        private IFeedbackRepository _feedbackRepository;
        private INodeRepository _nodeRepository;

        public RepositoryManager(PgSqlDbContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _repositoryContext.Database.Migrate();
        }

        public IWorldRepository World
        {
            get { return _worldRepository ??= new WorldRepository(_repositoryContext); }
        }

        public ILiveStreamRepository LiveStream
        {
            get { return _liveStreamRepository ??= new LiveStreamRepository(_repositoryContext); }
        }

        public ILinkRepository Link
        {
            get { return _linkRepository ??= new LinkRepository(_repositoryContext); }
        }

        public IOtpCodeRepository OtpCode
        {
            get { return _otpCodeRepository ??= new OtpCodeRepository(_repositoryContext); }
        }

        public IParticipantsRepository Participants
        {
            get { return _participantsRepository ??= new ParticipantsRepository(_repositoryContext); }
        }

        public IFeedbackRepository Feedbacks
        {
            get { return _feedbackRepository ??= new FeedbackRepository(_repositoryContext); }
        }
        public INodeRepository Nodes
        {
            get { return _nodeRepository ??= new NodeRepository(_repositoryContext); }
        }

        public IUserRepository User
        {
            get { return _userRepository ??= new UserRepository(_repositoryContext); }
        }

        public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
    }
}