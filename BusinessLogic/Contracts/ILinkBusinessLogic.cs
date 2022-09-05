using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.DataTransferObjects.Link;

namespace BusinessLogic.Contracts
{
    public interface ILinkBusinessLogic
    {
        Task<IBusinessLogicResult<LinkForShowDto>> CreateLinkAsync(LinkForCreationDto linkForCreationDto, Guid requestId);
        Task<IBusinessLogicResult> DeleteLink(Guid linkId, Guid requestId);
        Task<IBusinessLogicResult<IEnumerable<LinkForShowDto>>> GetAllLinksAsync(Guid requestId);
        Task<IBusinessLogicResult<LinkForShowDto>> GetLinkByIdAsync(Guid linkId, Guid requestId);
        Task<IBusinessLogicResult> UpdateLinkAsync(Guid linkId, LinkForUpdateDto linkForUpdateDto, Guid requestId);
    }
}