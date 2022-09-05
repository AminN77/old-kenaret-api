using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Contracts.Repository
{
    public interface IParticipantsRepository
    {
        void CreateParticipant(Participants participant);
        Task CreateParticipantAsync(Participants participant);
        Task<IEnumerable<Participants>> GetAllParticipantsForLiveStreamAsync(Guid liveStreamId, bool trackChanges);
    }
}