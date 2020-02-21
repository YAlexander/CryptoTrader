﻿using Contracts.Trading;

namespace TechanCore.Indicators.Results
{
    public class MovingAverageEnvelopesResult : IResultSet
    {
        public decimal?[] MiddleLine { get; set; }
        
        public decimal?[] UpperLine { get; set; }
        
        public decimal?[] LowerLine { get; set; }
    }
}