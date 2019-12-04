using core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Models.Mappers
{
	public static class StrategyPresetSelector
	{
		private static Dictionary<string, Type> _models = new Dictionary<string, Type>()
		{

		};

		//public  Get(string strategyName)
		//{
		//	return _models[strategyName];
		//}
	}
}
