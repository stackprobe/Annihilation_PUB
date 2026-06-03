using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Games.Gameplays.Scenarios;

namespace HLTStudio.Games.Gameplays
{
	public static class GEMaster
	{
		private static Func<ScenarioBase>[] STAGES = new Func<ScenarioBase>[]
		{
			() => new StageScenario_01(),
			() => new StageScenario_02(),
			() => new StageScenario_03(),
			() => new StageScenario_04(),
			() => new StageScenario_05(),
			() => new StageScenario_06(),
		};

		public static void Run(int firstStageIndex)
		{
			for (int si = firstStageIndex; si <= STAGES.Length; si++)
			{
				GEMain.Run(STAGES[si]());
			}
		}
	}
}
