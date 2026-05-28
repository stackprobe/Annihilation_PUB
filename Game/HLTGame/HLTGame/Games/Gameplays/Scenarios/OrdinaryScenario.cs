using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Commons;
using HLTStudio.GameCommons;
using HLTStudio.Games.Gameplays.Enemies;
using HLTStudio.Games.Gameplays.Enemies.KEnemies;

namespace HLTStudio.Games.Gameplays.Scenarios
{
	public class OrdinaryScenario : ScenarioBase
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			foreach (AScene scene in AScene.Create(30))
			{
				yield return true;
			}

			for (; ; )
			{
				GEMain.I.EnemyController.Add(new KEnemy_ScriptRunner("ザコ敵", BattleField.Screen.H / 3, -100.0));

				foreach (AScene scene in AScene.Create(120))
				{
					yield return true;
				}

				foreach (AScene scene in AScene.Create(600))
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

				foreach (AScene scene in AScene.Create(120))
				{
					yield return true;
				}
			}
		}
	}
}
