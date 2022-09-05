using System;
using System.Collections.Generic;

namespace BusinessLogic.Contracts
{
    public interface IBusinessLogicResult
    {
        bool Success { get; set; }
        string Error { get; set; }
    }

    public interface IBusinessLogicResult<T> : IBusinessLogicResult
    {
        T Result { get; set; }
    }
}