using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Abstractions.Enums;
using NUnit.Framework;
using Persistence.Entities;
using TechanCore.Enums;
using TechanCore.Helpers;
using TechanCore.Indicators;
using TechanCore.Indicators.Options;

namespace Tests
{
    public class Tests
    {
        private List<Candle> candles = new List<Candle>();
        
        [SetUp]
        public void Setup()
        {
            using (var file = new StreamReader("candles.csv"))
            {
                while (!file.EndOfStream)
                {
                    string[] line = file.ReadLine().Split(",");
                    var candle = new Candle();
                    candle.Exchange = Exchanges.BINANCE;
                    candle.Asset1 = Assets.BTC;
                    candle.Asset2 = Assets.USDT;
                    candle.Time = DateTime.Parse(line[0]);
                    candle.TimeFrame = Timeframes.MINUTE;
                    candle.High = decimal.Parse(line[3]);
                    candle.Low = decimal.Parse(line[4]);
                    candle.Open = decimal.Parse(line[1]);
                    candle.Close = decimal.Parse(line[2]);
                    candle.Volume = decimal.Parse(line[5]);

                    candles.Add(candle);
                }
            }
        }

        [Test]
        public void Test1()
        {
            RenkoOptions options = new RenkoOptions() { AtrPeriod = 14 };
            RenkoIndicator indicator = new RenkoIndicator();

            var source = candles.GroupCandles(Timeframes.FIVE_MINUTES);
            
            var bricks = indicator.Get(source.ToArray(), options).Bricks;

            var ab = bricks.Count(x => x.Type == RenkoBrickType.BLACK);
            var aw = bricks.Count(x => x.Type == RenkoBrickType.WHITE);

            Assert.Pass();
        }
    }
}