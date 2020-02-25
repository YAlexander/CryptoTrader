namespace Contracts.Enums
{
	public enum FillPolitics
	{
		/// <summary>
		/// Good till cancelled
		/// </summary>
		GTC,

		/// <summary>
		/// Fill or Kill. Must be fully filled, else cancelled
		/// </summary>
		FOK,

		/// <summary>
		/// Immediate or Cancel. Fill max available, rest cancelled
		/// </summary>
		IOC
	}
}
