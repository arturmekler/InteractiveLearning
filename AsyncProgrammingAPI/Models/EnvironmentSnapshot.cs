using System;

namespace AsyncProgrammingAPI.Models
{
    public class EnvironmentSnapshot
    {
        public int ProcessorCount { get; set; }
        public int ProcessId { get; set; }
        public int ThreadCount { get; set; }
    }
}
