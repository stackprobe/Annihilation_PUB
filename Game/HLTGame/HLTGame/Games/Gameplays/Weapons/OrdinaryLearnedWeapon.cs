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
	public class OrdinaryLearnedWeapon : WeaponBase
	{
		public OrdinaryLearnedWeapon()
		{
			this.Initialize(3, true);
		}

		protected override IEnumerable<bool> E_EachFrame()
		{
			for (; ; )
			{
				if (this.AttackPoint <= 0) // 攻撃力を使い果たしたので消える。(自力で消える方式！)
					break;

				this.Y -= 13.0;

				if (this.Y < -100.0) // 画面外に出たので消える。(自力で退場する方式！)
					break;

				DD.SetAlpha(0.5);
				DD.SetBright(new D3Color(0, 0.5, 1));
				DD.SetSize(new D2Size(48.0, 48.0));
				DD.Draw(Pictures.WhiteCircle, new D2Point(this.X, this.Y));

				this.Crash = ACrash.CreateCircle(new D2Point(this.X, this.Y), 26.0);

				yield return true;
			}
		}

		public override Type GetLearnedBulletType()
		{
			return typeof(OrdinaryLearnableBullet);
		}
	}
}
