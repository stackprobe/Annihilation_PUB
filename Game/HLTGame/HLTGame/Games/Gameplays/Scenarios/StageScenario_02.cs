using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLTStudio.Games.Gameplays.Scenarios
{
	public class StageScenario_02 : ScenarioBase
	{
		protected override IEnumerable<int> E_EachFrame()
		{
			for (; ; )
			{
				yield return 1;
			}
		}
	}
}
