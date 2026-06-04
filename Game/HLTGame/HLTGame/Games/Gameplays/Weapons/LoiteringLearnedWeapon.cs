using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;
using HLTStudio.Games.Gameplays.Enemies.Bullets;

namespace HLTStudio.Games.Gameplays.Weapons
{
	public class LoiteringLearnedWeapon : WeaponBase
	{
		private double BaseX;
		private double Angle;

		public LoiteringLearnedWeapon()
		{
			this.Initialize(2, true);

			this.BaseX = this.X;
			this.Angle = DD.GetAngle(new D2Point(BattleField.Screen.W / 2.0, -60.0), new D2Point(this.X, this.Y));
		}

		protected override IEnumerable<bool> E_EachFrame()
		{
			for (int frame = 0; ; frame++)
			{
				if (this.AttackPoint <= 0)
					break;

				if (this.Y < -80.0)
					break;

				this.Angle += Math.Sin(frame * 0.065) * 0.0025;

				D2Point speed = DD.AngleToPoint(this.Angle, 10.0);

				this.X += speed.X;
				this.Y += speed.Y;
				this.X = this.BaseX + (this.X - this.BaseX) * 0.97 + Math.Sin(frame * 0.22) * 3.0;

				DD.SetAlpha(0.65);
				DD.SetBright(new D3Color(0.25, 0.55, 1.0));
				DD.SetSize(new D2Size(30.0, 30.0));
				DD.Draw(Pictures.WhiteCircle, new D2Point(this.X, this.Y));

				this.Crash = ACrash.CreateCircle(new D2Point(this.X, this.Y), 16.0);

				yield return true;
			}
		}

		public override Type GetLearnedBulletType()
		{
			return typeof(Bullet_LoiteringLearnable);
		}
	}
}
