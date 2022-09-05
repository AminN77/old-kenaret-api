using System.Threading.Tasks;
using Contracts.Repository;
using Domain.Entities;
using Infrastructure.Data.DataBase;

namespace Infrastructure.Data.Repository
{
    public class FeedbackRepository : RepositoryBase<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(PgSqlDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateFeedback(Feedback feedback)
        {
            Create(feedback);
        }

        public async Task CreateFeedbackAsync(Feedback feedback)
        {
            await CreateAsync(feedback);
        }
    }
}