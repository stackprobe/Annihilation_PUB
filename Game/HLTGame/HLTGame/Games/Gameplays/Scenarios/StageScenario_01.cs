using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Games.Gameplays.Enemies.KEnemies;

namespace HLTStudio.Games.Gameplays.Scenarios
{
	public class StageScenario_01 : ScenarioBase
	{
		protected override IEnumerable<int> E_EachFrame()
		{
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

			yield return 180;

			// ====

			var subScenario = new OrdinaryScenario();

			while (subScenario.EachFrame())
				yield return 1;
		}
	}
}
