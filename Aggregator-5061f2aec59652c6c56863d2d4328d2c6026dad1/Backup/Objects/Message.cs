using System.Collections.Generic;

namespace Aggregator.Objects
{
    public class Message
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Warnings { get; set; }

        public Message()
        {
            Errors = new List<string>();
            Warnings = new List<string>();
        }
    }
}