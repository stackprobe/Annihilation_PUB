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
	public class Bomb : WeaponBase
	{
		private const int ATTACK_POINT_PER_FRAME = 10;

		public Bomb()
		{
			this.Initialize(GEMain.I.PlayerActor.X, GEMain.I.PlayerActor.Y, ATTACK_POINT_PER_FRAME, false);
		}

		protected override IEnumerable<bool> E_EachFrame()
		{
			foreach (AScene scene in AScene.Create(90))
			{
				GEMain.I.EnemyController.EraseBullet();

				if (this.AttackPoint < ATTACK_POINT_PER_FRAME) // 消費した攻撃力を補充する。(最後まで消えない！)
					this.AttackPoint = ATTACK_POINT_PER_FRAME;

				double r = 100.0 + scene.Rate * 400.0;

				DD.SetSize(new D2Size(r * 2.0, r * 2.0));
				DD.SetAlpha(0.3);
				DD.SetBright(new D3Color(1, 0, 0.5));
				DD.Draw(Pictures.WhiteCircle, new D2Point(this.X, this.Y));

				this.Crash = ACrash.CreateCircle(new D2Point(this.X, this.Y), r);

				yield return true;
			}

			GEMain.I.PlayerActor.BombActiveFlag = false;
		}

		public override Type GetLearnedBulletType()
		{
			throw null; // never
		}
	}
}
