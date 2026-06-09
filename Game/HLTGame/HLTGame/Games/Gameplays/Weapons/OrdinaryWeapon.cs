using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;

namespace HLTStudio.Games.Gameplays.Weapons
{
	public class OrdinaryWeapon : WeaponBase
	{
		public OrdinaryWeapon()
		{
			this.Initialize(1, false);
		}

		protected override IEnumerable<bool> E_EachFrame()
		{
			for (; ; )
			{
				if (this.AttackPoint <= 0) // 攻撃力を使い果たしたので消える。(自力で消える方式！)
					break;

				this.Y -= 30.0;

				// 画面外でもクラッシュ判定は行われるので、画面外に出たらすぐに消えること。

				if (this.Y < -30.0) // 画面外に出たので消える。(自力で退場する方式！)
					break;

				DD.SetSize(new D2Size(16.0, 16.0));
				DD.Draw(Pictures.WhiteCircle, new D2Point(this.X, this.Y));

				this.Crash = ACrash.CreateCircle(new D2Point(this.X, this.Y), 10.0);

				yield return true;
			}
		}

		public override Type GetLearnedBulletType()
		{
			throw null; // never
		}
	}
}
