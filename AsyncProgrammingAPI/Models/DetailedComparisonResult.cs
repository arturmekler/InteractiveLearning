using System;
using System.Collections.Generic;

namespace AsyncProgrammingAPI.Models
{
    public class DetailedComparisonResult
    {
        public EnvironmentSnapshot EnvironmentSnapshot { get; set; } = new EnvironmentSnapshot();
        public ThreadPoolSnapshot ThreadPoolBefore { get; set; } = new ThreadPoolSnapshot();
        public ThreadPoolSnapshot ThreadPoolAfterSync { get; set; } = new ThreadPoolSnapshot();
        public ThreadPoolSnapshot ThreadPoolAfterAsync { get; set; } = new ThreadPoolSnapshot();
        public OperationGroupResult Synchronous { get; set; } = new OperationGroupResult();
        public OperationGroupResult Asynchronous { get; set; } = new OperationGroupResult();
        public IEnumerable<string> Summary { get; set; } = Array.Empty<string>();
    }
}