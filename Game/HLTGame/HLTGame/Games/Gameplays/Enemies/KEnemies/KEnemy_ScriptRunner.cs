using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.GameCommons;

namespace HLTStudio.Games.Gameplays.Enemies.KEnemies
{
	public class KEnemy_ScriptRunner : KEnemyBase
	{
		//private double CharacterRadius = 30.0;
		private SortedDictionary<string, double> Variables = new SortedDictionary<string, double>();

		public KEnemy_ScriptRunner(string scriptName, double x, double y, params double[] scriptParameters)
			: base(x, y, 1)
		{
			throw null; // TODO
		}

		protected override IEnumerable<bool> E_EachFrame2()
		{
			throw null; // TODO
		}

		protected override double GetCharacterRadius()
		{
			throw null; // TODO
		}

		protected override APicture GetPicture()
		{
			throw null; // TODO
		}
	}
}
