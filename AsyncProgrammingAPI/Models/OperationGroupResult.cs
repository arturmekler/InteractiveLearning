using System;
using System.Collections.Generic;

namespace AsyncProgrammingAPI.Models
{
    public class OperationGroupResult
    {
        public IList<OperationDetail> Operations { get; set; } = new List<OperationDetail>();
        public TimeSpan TotalElapsed { get; set; }
        public IList<int> DistinctThreadIds { get; set; } = new List<int>();
    }
}
