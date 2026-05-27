using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;

namespace HLTStudio.Games.Gameplays.Enemies.Bullets
{
	public class OrdinaryLearnableBullet : BulletBase
	{
		private D2Point Speed;

		public OrdinaryLearnableBullet(double x, double y)
		{
			this.Initialize_Bullet(x, y, true);

			this.Speed = DD.MakeXYSpeed(new D2Point(x, y), new D2Point(GEMain.I.PlayerActor.X, GEMain.I.PlayerActor.Y), 2.0);
		}

		protected override IEnumerable<bool> E_EachFrame()
		{
			for (; ; )
			{
				if (DD.IsOutOf(
					new D4Rect(0, 0, BattleField.Screen.W, BattleField.Screen.H),
					new D2Point(this.X, this.Y),
					50.0
					))
					break; // 画面外に出たので消える。(自分で退場する方式！)

				this.X += this.Speed.X;
				this.Y += this.Speed.Y;

				DD.Draw(Pictures.OrdinaryLearnableBullet, new D2Point(this.X, this.Y));

				this.Crash = ACrash.CreateCircle(new D2Point(this.X, this.Y), 16.0);

				yield return true;
			}
		}

		public override int GetLearnPoint()
		{
			return 1000 / 7;
		}
	}
}
