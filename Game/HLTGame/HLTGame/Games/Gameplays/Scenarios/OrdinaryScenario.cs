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
		protected override IEnumerable<int> E_EachFrame()
		{
			yield return 30;

			GEMain.I.EnemyController.Add(new KEnemy_LoiteringShooter(300.0, -30.0));

			yield return 60;

			for (; ; )
			{
				GEMain.I.EnemyController.Add(new KEnemy_ScriptRunner("ザコ敵", BattleField.Screen.H / 3, -100.0));

				yield return 120;

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

					if (SCommon.CRandom.GetRate() < 0.01)
					{
						GEMain.I.EnemyController.Add(new KEnemy_Ordinary(
							SCommon.CRandom.GetDoubleRange(10.0, BattleField.Screen.W - 20.0),
							-30.0
							));
					}

					yield return 1;
				}

				yield return 120;
			}
		}
	}
}
