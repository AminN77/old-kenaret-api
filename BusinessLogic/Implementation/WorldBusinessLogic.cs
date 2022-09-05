using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Contracts;
using BusinessLogic.DataTransferObjects.World;
using Contracts.LoggerManager;
using Contracts.Mapper;
using Contracts.Repository;

namespace BusinessLogic.Implementation
{
    public class WorldBusinessLogic : IWorldBusinessLogic
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        
        public WorldBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager loggerManager)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }
        
        public async Task<IBusinessLogicResult<IEnumerable<WorldForShowDto>>> GetAllWorldsAsync(Guid requestId)
        {
            var worlds = await _repositoryManager.World.GetAllWorldsAsync(trackChanges: false);
            var worldsForShow = new List<WorldForShowDto>();
            foreach (var world in worlds)
            {
                worldsForShow.Add(await _mapper.MapAsync(world, new WorldForShowDto()));
            }

            return new BusinessLogicResult<IEnumerable<WorldForShowDto>>() {Success = true, Result = worldsForShow};
        }
    }
}