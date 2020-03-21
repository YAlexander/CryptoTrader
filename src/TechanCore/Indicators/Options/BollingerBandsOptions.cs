namespace TechanCore.Indicators.Options
{
    public class BollingerBandsOptions : IOptionsSet
    {
        public int Period { get; set; }
        
        public double DeviationUp { get; set; }
        
        public double DeviationDown { get; set; }
    }
}