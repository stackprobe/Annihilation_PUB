using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Commons;
using HLTStudio.Games.Gameplays.Enemies;
using HLTStudio.Games.Gameplays.Enemies.KEnemies;

namespace HLTStudio.Games.Gameplays.Scenarios
{
	public class OrdinaryScenario2 : ScenarioBase
	{
		protected override IEnumerable<int> E_EachFrame()
		{
			GEMain.I.EnemyController.Add(new KEnemy_Ordinary(BattleField.Screen.W * 0.25, -30.0));
			GEMain.I.EnemyController.Add(new KEnemy_Ordinary(BattleField.Screen.W * 0.50, -30.0));
			GEMain.I.EnemyController.Add(new KEnemy_Ordinary(BattleField.Screen.W * 0.75, -30.0));

			yield return 90;

			GEMain.I.EnemyController.Add(new OrdinaryEnemy(SCommon.CRandom.GetRate() < 0.25));
			GEMain.I.EnemyController.Add(new OrdinaryEnemy(SCommon.CRandom.GetRate() < 0.25));

			yield return 30;

			GEMain.I.EnemyController.Add(new OrdinaryEnemy2(SCommon.CRandom.GetRate() < 0.25));

			yield return 180;

			GEMain.I.EnemyController.Add(new KEnemy_ScriptRunner("クラゲ", 38, 0));
			yield return 20;
			GEMain.I.EnemyController.Add(new KEnemy_ScriptRunner("クラゲ", 76, 0));
			yield return 20;
			GEMain.I.EnemyController.Add(new KEnemy_ScriptRunner("クラゲ", 114, 0));

			yield return 120;

			GEMain.I.EnemyController.Add(new KEnemy_ScriptRunner("クラゲ", 418, 0));
			yield return 20;
			GEMain.I.EnemyController.Add(new KEnemy_ScriptRunner("クラゲ", 380, 0));
			yield return 20;
			GEMain.I.EnemyController.Add(new KEnemy_ScriptRunner("クラゲ", 342, 0));

			yield return 300;

			// ====

			var subScenario = new OrdinaryScenario();

			while (subScenario.EachFrame())
				yield return 1;
		}
	}
}
