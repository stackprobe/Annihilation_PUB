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
	public class Bullet_LoiteringLearnable : BulletBase
	{
		private double Angle;
		private double AngleSpeed;

		public Bullet_LoiteringLearnable(double x, double y)
		{
			this.Initialize_Bullet(x, y, true);

			this.Angle = DD.GetAngle(new D2Point(GEMain.I.PlayerActor.X, GEMain.I.PlayerActor.Y), new D2Point(x, y));
			this.AngleSpeed = x < BattleField.Screen.W / 2 ? 0.018 : -0.018;
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
					break;

				this.Angle += this.AngleSpeed * (0.65 + Math.Sin(frame * 0.045) * 0.35);

				D2Point speed = DD.AngleToPoint(this.Angle, 2.2 + Math.Sin(frame * 0.055) * 0.4);

				this.X += speed.X;
				this.Y += speed.Y;

				DD.SetBright(new D3Color(0.35, 0.55, 1.0));
				DD.SetSize(new D2Size(24.0, 24.0));
				DD.Draw(Pictures.Dummy, new D2Point(this.X, this.Y));

				this.Crash = ACrash.CreateCircle(new D2Point(this.X, this.Y), 14.0);

				yield return true;
			}
		}

		public override int GetLearnPoint()
		{
			return 1000 / 14;
		}
	}
}
