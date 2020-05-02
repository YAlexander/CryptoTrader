using System;
using System.Collections.Generic;
using System.Text;

namespace Abstractions
{
	public interface IMessage
	{
		string Receiver { get; set; }
		string Sender { get; set; }

		DateTime Created { get; set; }

		DateTime? Expired { get; set; }

		string Message { get; set; }
	}
}
