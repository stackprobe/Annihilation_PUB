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
	public class OrdinaryLearnableBullet2 : BulletBase
	{
		private double Angle;
		private double AngleSpeed;

		public OrdinaryLearnableBullet2(double x, double y)
		{
			this.Initialize_Bullet(x, y, true);

			this.Angle = DD.GetAngle(new D2Point(GEMain.I.PlayerActor.X, GEMain.I.PlayerActor.Y), new D2Point(x, y));
			this.AngleSpeed = 0.01;

			if (x < BattleField.Screen.W / 2)
				this.AngleSpeed = -this.AngleSpeed;
		}

		protected override IEnumerable<bool> E_EachFrame()
		{
			for (int frame = 0; ; frame++)
			{
				if (DD.IsOutOf(
					new D4Rect(0, 0, BattleField.Screen.W, BattleField.Screen.H),
					new D2Point(this.X, this.Y),
					50.0
					))
					break; // 画面外に出たので消える。(自分で退場する方式！)

				this.Angle += this.AngleSpeed;

				D2Point d2Speed = DD.AngleToPoint(this.Angle, 2.0 + frame * 0.05);

				this.X += d2Speed.X;
				this.Y += d2Speed.Y;

				DD.Draw(Pictures.OrdinaryLearnableBullet2, new D2Point(this.X, this.Y));

				this.Crash = ACrash.CreateCircle(new D2Point(this.X, this.Y), 16.0);

				yield return true;
			}
		}

		public override int GetLearnPoint()
		{
			return 1000 / 23;
		}
	}
}
