using System;
using System.Text.Json;
using Contracts.Enums;

namespace Common
{
    // For Grain identity we use Exchange code as primary key and GrainKeyExtension as key extension
    public class GrainKeyExtension
    {
        public Assets Asset1 { get; set; }
        public Assets Asset2 { get; set; }
        public DateTime? Time { get; set; }
        
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}