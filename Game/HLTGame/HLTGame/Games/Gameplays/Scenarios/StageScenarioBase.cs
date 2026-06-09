using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.KScripts;

namespace HLTStudio.Games.Gameplays.Scenarios
{
	public abstract class StageScenarioBase : ScenarioBase
	{
		protected abstract int GetStageNumber();

		protected override IEnumerable<int> E_EachFrame()
		{
			var scenarioName = $"ステージシナリオ_{this.GetStageNumber():D2}";

			foreach (var relay in KEngine.Run(new KVariables(null), scenarioName, new string[0]))
			{
				yield return 1;
			}
		}
	}
}
