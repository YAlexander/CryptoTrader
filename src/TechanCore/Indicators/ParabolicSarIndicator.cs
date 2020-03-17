using System;
using System.Collections.Generic;
using System.Linq;
using TechanCore.Helpers;
using TechanCore.Indicators.Extensions;
using TechanCore.Indicators.Options;
using TechanCore.Indicators.Results;

// TODO: Make Refactoring
namespace TechanCore.Indicators
{
    public class ParabolicSarIndicator : BaseIndicator<ParabolicSarOptions, SeriesIndicatorResult>
    {
        public override string Name { get; }
        
        public override SeriesIndicatorResult Get(ICandle[] source, ParabolicSarOptions options)
        {
            // Difference of High and Low
            List<decimal> differences = new List<decimal>();
            for (int i = 0; i < source.Length; i++)
            {
                decimal difference = source[i].High - source[i].Low;
                differences.Add(difference);
            }

            // STDEV of differences
            decimal?[] stDev = differences.StDev(options.Period).Result;

            decimal?[] sarArr = new decimal?[source.Length];

            decimal[] highList = source.High();
            decimal[] lowList = source.Low();

            /* Find first non-NA value */
            int beg = 1;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i].High == 0 || source[i].Low == 0)
                {
                    sarArr[i] = 0;
                    beg++;
                }
                else
                {
                    break;
                }
            }
            
            /* Initialize values needed by the routine */
            int sig0 = 1, sig1 = 0;
            decimal xpt0 = highList[beg - 1], xpt1 = 0;
            double af0 = options.AccelerationFactor, af1 = 0;
            decimal lmin, lmax;
            sarArr[beg - 1] = lowList[beg - 1] - stDev[beg - 1];

            for (int i = beg; i < source.Length; i++)
            {
                /* Increment signal, extreme point, and acceleration factor */
                sig1 = sig0;
                xpt1 = xpt0;
                af1 = af0;

                /* Local extrema */
                lmin = (lowList[i - 1] > lowList[i]) ? lowList[i] : lowList[i - 1];
                lmax = (highList[i - 1] > highList[i]) ? highList[i - 1] : highList[i];
                /* Create signal and extreme price vectors */
                if (sig1 == 1)
                {  /* Previous buy signal */
                    sig0 = (lowList[i] > sarArr[i - 1]) ? 1 : -1;  /* New signal */
                    xpt0 = (lmax > xpt1) ? lmax : xpt1;             /* New extreme price */
                }
                else
                {           /* Previous sell signal */
                    sig0 = (highList[i] < sarArr[i - 1]) ? -1 : 1;  /* New signal */
                    xpt0 = (lmin > xpt1) ? xpt1 : lmin;             /* New extreme price */
                }

                /*
                    * Calculate acceleration factor (af)
                    * and stop-and-reverse (sar) vector
                */

                /* No signal change */
                if (sig0 == sig1)
                {
                    /* Initial calculations */
                    sarArr[i] = sarArr[i - 1] + (xpt1 - sarArr[i - 1]) * (decimal)af1;
                    af0 = (af1 == options.MaximumAccelerationFactor) ? options.MaximumAccelerationFactor : (options.AccelerationFactor + af1);
                    /* Current buy signal */
                    if (sig0 == 1)
                    {
                        af0 = (xpt0 > xpt1) ? af0 : af1;  /* Update acceleration factor */
                        sarArr[i] = (sarArr[i] > lmin) ? lmin : sarArr[i];  /* Determine sar value */
                    }
                    /* Current sell signal */
                    else
                    {
                        af0 = (xpt0 < xpt1) ? af0 : af1;  /* Update acceleration factor */
                        sarArr[i] = (sarArr[i] > lmax) ? sarArr[i] : lmax;   /* Determine sar value */
                    }
                }
                else /* New signal */
                {
                    af0 = options.AccelerationFactor;    /* reset acceleration factor */
                    sarArr[i] = xpt0;  /* set sar value */
                }
            }

            return new SeriesIndicatorResult {Result = sarArr.ToArray()};
        }

        public override SeriesIndicatorResult Get(decimal[] source, ParabolicSarOptions options)
        {
            throw new NotImplementedException();
        }

        public override SeriesIndicatorResult Get(decimal?[] source, ParabolicSarOptions options)
        {
            throw new NotImplementedException();
        }
    }
}