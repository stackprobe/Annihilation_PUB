using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;
using HLTStudio.Games.Gameplays.Enemies.Bullets;

namespace HLTStudio.Games.Gameplays.Weapons
{
	public class OrdinaryLearnedWeapon2 : WeaponBase
	{
		private double Angle;
		private double AngleSpeed;

		public OrdinaryLearnedWeapon2()
		{
			this.Initialize(2, true);

			this.Angle = DD.GetAngle(new D2Point(BattleField.Screen.W / 2.0, -50.0), new D2Point(GEMain.I.PlayerActor.X, GEMain.I.PlayerActor.Y));
			this.AngleSpeed = 0.02;

			if (this.X > BattleField.Screen.W / 2)
				this.AngleSpeed = -this.AngleSpeed;
		}

		protected override IEnumerable<bool> E_EachFrame()
		{
			for (int frame = 0; ; frame++)
			{
				if (this.AttackPoint <= 0) // 攻撃力を使い果たしたので消える。(自力で消える方式！)
					break;

				if (DD.IsOutOf(
					new D4Rect(0, 0, BattleField.Screen.W, BattleField.Screen.H),
					new D2Point(this.X, this.Y),
					50.0
					))
					break; // 画面外に出たので消える。(自分で退場する方式！)

				this.Angle += this.AngleSpeed;

				D2Point d2Speed = DD.AngleToPoint(this.Angle, 9.0 + frame * 0.05);

				this.X += d2Speed.X;
				this.Y += d2Speed.Y;

				DD.SetSize(new D2Size(24.0, 24.0));
				DD.SetBright(new D3Color(0, 0.5, 1));
				DD.Draw(Pictures.WhiteCircle, new D2Point(this.X, this.Y));

				this.Crash = ACrash.CreateCircle(new D2Point(this.X, this.Y), 13.0);

				yield return true;
			}
		}

		public override Type GetLearnedBulletType()
		{
			return typeof(OrdinaryLearnableBullet2);
		}
	}
}
