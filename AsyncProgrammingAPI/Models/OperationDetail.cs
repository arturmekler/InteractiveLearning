using System;

namespace AsyncProgrammingAPI.Models
{
    public class OperationDetail
    {
        public int Index { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public TimeSpan Elapsed { get; set; }
        public int StartThreadId { get; set; }
        public int EndThreadId { get; set; }
        public string Notes { get; set; } = "";
    }
}
