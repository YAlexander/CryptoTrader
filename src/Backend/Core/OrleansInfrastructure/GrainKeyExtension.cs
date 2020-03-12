using System;
using Contracts.Enums;

namespace Core.OrleansInfrastructure
{
    // For Grain identity we use Exchange code as primary key and GrainKeyExtension as key extension
    public class GrainKeyExtension
    {
        public Assets Asset1 { get; set; }
        public Assets Asset2 { get; set; }
        public DateTime? Time { get; set; }
    }
}