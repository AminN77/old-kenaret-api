using System;
using System.Collections;
using System.Collections.Generic;

namespace Infrastructure.Logger
{
    public class LogStack
    {
        private readonly Dictionary<Guid, List<string>> _stacks = new();
        
        public List<string> GetStack(Guid requestId)
        {
            try
            {
               return _stacks[requestId];
            }
            catch
            {
                _stacks[requestId] = new List<string>();
                return _stacks[requestId];
            }
        }

        public  void Push(string log, Guid requestId)
        {
            try
            {
                _stacks[requestId].Add(log);
            }
            catch
            {
                _stacks[requestId] = new List<string>();
                _stacks[requestId].Add(log);
            }
        }

        public  void PopAll(Guid requestId)
        {
            try
            {
                _stacks.Remove(requestId);
            }
            catch
            {
                return;
            }
        }
        
    }
}