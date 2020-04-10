using System;

namespace Common.Exceptions
{
	public class RiskManagementException : Exception
	{
		public RiskManagementException(string message) : base(message) { }
	}
}