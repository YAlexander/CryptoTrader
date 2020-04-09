﻿namespace Persistence.Entities
{
    public class StrategyOptionsEntity : BaseEntity<long>
    {
        /// <summary>
        /// JSON-based Strategy options
        /// </summary>
        private string Options { get; set; }
    }
}