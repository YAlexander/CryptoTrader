namespace core.Trading.Models
{
	public class TdSeqItem
	{
		public int? Value { get; set; }
		public bool IsGreen { get; set; }

		public override string ToString ()
		{
			return $"{Value}";
		}
	}
}
