using System;

namespace AsyncProgrammingAPI.Models
{
    public class ThreadPoolSnapshot
    {
        public string Tag { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public int WorkerThreadsAvailable { get; set; }
        public int CompletionPortThreadsAvailable { get; set; }
        public int WorkerThreadsMax { get; set; }
        public int CompletionPortThreadsMax { get; set; }
        public int WorkerThreadsMin { get; set; }
        public int CompletionPortThreadsMin { get; set; }
    }
}
