using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Commons;
using HLTStudio.Games.Gameplays.Enemies;

namespace HLTStudio.Games.Gameplays.Scenarios
{
	public class OrdinaryScenario : ScenarioBase
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			for (; ; )
			{
				if (SCommon.CRandom.GetRate() < 0.01)
				{
					GEMain.I.EnemyController.Add(new OrdinaryEnemy(SCommon.CRandom.GetRate() < 0.25));
				}

				if (SCommon.CRandom.GetRate() < 0.01)
				{
					GEMain.I.EnemyController.Add(new OrdinaryEnemy2(SCommon.CRandom.GetRate() < 0.25));
				}

				yield return true;
			}
		}
	}
}
