using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;

namespace HLTStudio.Tools
{
	public static class GameTools
	{
		public static void Approach(ref D2Point value, D2Point target, double rate)
		{
			DD.Approach(ref value.X, target.X, rate);
			DD.Approach(ref value.Y, target.Y, rate);
		}
	}
}
