using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;

namespace HLTStudio.Games.Gameplays.Enemies.KEnemies
{
	public class KEnemy_Sample : KEnemyBase
	{
		public KEnemy_Sample(double x, double y, int hp)
			: base(x, y, hp)
		{ }

		protected override IEnumerable<bool> E_EachFrame2()
		{
			for (; ; )
			{
				// TODO
				// TODO
				// TODO

				yield return true;
			}
		}

		protected override double GetCharacterRadius()
		{
			return 30.0;
		}

		protected override APicture GetPicture()
		{
			return Pictures.OrdinaryEnemy;
		}
	}
}
