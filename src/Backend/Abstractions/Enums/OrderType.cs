﻿namespace Abstractions.Enums
{
	public enum OrderType
	{
		/// <summary>
		/// Market
		/// </summary>
		MKT,

		/// <summary>
		/// Limit
		/// </summary>
		LMT,

		/// <summary>
		/// Stop limit
		/// </summary>
		LST,

		/// <summary>
		/// Trailing
		/// </summary>
		TRL,

		/// <summary>
		/// Stop market
		/// </summary>
		SMT,

		/// <summary>
		/// One cancel other
		/// </summary>
		OCO
	}
}