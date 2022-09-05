using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.DataTransferObjects.Link;
using BusinessLogic.DataTransferObjects.World;

namespace BusinessLogic.Contracts
{
    public interface IWorldBusinessLogic
    {
        Task<IBusinessLogicResult<IEnumerable<WorldForShowDto>>> GetAllWorldsAsync(Guid requestId);
    }
}