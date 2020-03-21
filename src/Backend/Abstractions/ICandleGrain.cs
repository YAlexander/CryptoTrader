﻿using System.Threading.Tasks;
using Orleans;
using TechanCore;

namespace Abstractions
{
    public interface ICandleGrain : IGrainWithIntegerCompoundKey
    {
        Task Set(ICandle candle);
        Task<ICandle> Get();
    }
}