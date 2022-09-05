using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.DataTransferObjects.Extension;
using BusinessLogic.DataTransferObjects.Feedback;
using BusinessLogic.DataTransferObjects.LiveStream;
using Domain.RequestFeatures;

namespace BusinessLogic.Contracts
{
    public interface IExtensionBusinessLogic
    {
        Task<IBusinessLogicResult<PagedList<LiveStreamForShowDto>>> StreamDetectionAsync(StreamDetectionDto detectionDto, Guid requestId);

    }
}