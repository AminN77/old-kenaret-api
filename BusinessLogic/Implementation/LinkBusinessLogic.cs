using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Contracts;
using BusinessLogic.DataTransferObjects.Link;
using Contracts.LoggerManager;
using Contracts.Mapper;
using Contracts.Repository;
using Domain.Entities;

namespace BusinessLogic.Implementation
{
    public class LinkBusinessLogic : ILinkBusinessLogic
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILoggerManager _loggerManager;
        private readonly IMapper _mapper;

        public LinkBusinessLogic(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager loggerManager)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }
        
        public async Task<IBusinessLogicResult<LinkForShowDto>> CreateLinkAsync(LinkForCreationDto linkForCreationDto, Guid requestId)
        {
            //validation
            var link = await _mapper.MapAsync(linkForCreationDto, new Link());
            link.Id = Guid.NewGuid();
            await _repositoryManager.Link.CreateLinkAsync(link);
            await _repositoryManager.SaveAsync();
            var linkForShow = await _mapper.MapAsync(link, new LinkForShowDto());
            return new BusinessLogicResult<LinkForShowDto>() {Success = true, Result = linkForShow};
        }

        public async Task<IBusinessLogicResult> DeleteLink(Guid linkId, Guid requestId)
        {
            var link = await _repositoryManager.Link.GetLinkAsync(linkId, trackChanges: false);
            if (link is null)
                return new BusinessLogicResult() {Success = false, Error = BusinessLogicErrors.Link.LinkDoesNotExist};
            
            _repositoryManager.Link.DeleteLink(link);
            await _repositoryManager.SaveAsync();
            return new BusinessLogicResult() {Success = true};
        }

        public async Task<IBusinessLogicResult<IEnumerable<LinkForShowDto>>> GetAllLinksAsync(Guid requestId)
        {
            var links =await _repositoryManager.Link.GetAllLinksAsync(trackChanges: false);
            var linksForShow = new List<LinkForShowDto>();
            foreach (var link in links)
            {
                linksForShow.Add(await _mapper.MapAsync(link, new LinkForShowDto()));
            }
            
            return new BusinessLogicResult<IEnumerable<LinkForShowDto>>() {Success = true, Result = linksForShow};
        }

        public async Task<IBusinessLogicResult<LinkForShowDto>> GetLinkByIdAsync(Guid linkId, Guid requestId)
        {
            var link = await _repositoryManager.Link.GetLinkAsync(linkId, trackChanges: false);
            if (link is null)
                return new BusinessLogicResult<LinkForShowDto>() {Success = false, Error = BusinessLogicErrors.Link.LinkDoesNotExist};
            
            var linkForShow = await _mapper.MapAsync(link, new LinkForShowDto());
            return new BusinessLogicResult<LinkForShowDto>() {Success = true, Result = linkForShow};
        }

        public async Task<IBusinessLogicResult> UpdateLinkAsync(Guid linkId, LinkForUpdateDto linkForUpdateDto, Guid requestId)
        {
            var link = await _repositoryManager.Link.GetLinkAsync(linkId, trackChanges: true);
            if (link is null)
                return new BusinessLogicResult() {Success = false, Error =BusinessLogicErrors.Link.LinkDoesNotExist};

            await _mapper.MapAsync(linkForUpdateDto, link);
            await _repositoryManager.SaveAsync();
            return new BusinessLogicResult() {Success = true};
        }
    }
}