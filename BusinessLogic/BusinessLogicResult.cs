using System;
using System.Collections.Generic;
using BusinessLogic.Contracts;

namespace BusinessLogic
{   public class BusinessLogicResult : IBusinessLogicResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class BusinessLogicResult<T> : BusinessLogicResult , IBusinessLogicResult<T>
    {
        
        public T Result { get; set; }
    }
}