using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Repository;
using Domain.Entities;
using Infrastructure.Data.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository
{
    public class ParticipantsRepository : RepositoryBase<Participants>, IParticipantsRepository
    {
        public ParticipantsRepository(PgSqlDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateParticipant(Participants participant)
        {
            Create(participant);
        }

        public async Task CreateParticipantAsync(Participants participant)
        {
            await CreateAsync(participant);
        }

        public async Task<IEnumerable<Participants>> GetAllParticipantsForLiveStreamAsync(Guid liveStreamId,
            bool trackChanges)
        {
            return await FindByCondition(l => l.LiveStreamId.Equals(liveStreamId), trackChanges: trackChanges)
                .Where(p => p.IsConnected).ToListAsync();
        }
    }
}