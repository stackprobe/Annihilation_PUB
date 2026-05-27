using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;

namespace HLTStudio.Games.Gameplays.Weapons
{
	public class AnnihilationExplosion : WeaponBase
	{
		private const int ATTACK_POINT_PER_FRAME = 3;

		public AnnihilationExplosion(double x, double y)
		{
			this.Initialize(x, y, ATTACK_POINT_PER_FRAME, false);
		}

		protected override IEnumerable<bool> E_EachFrame()
		{
			for (int frame = 0; frame < 30; frame++)
			{
				if (this.AttackPoint < ATTACK_POINT_PER_FRAME) // 消費した攻撃力を補充する。(最後まで消えない！)
					this.AttackPoint = ATTACK_POINT_PER_FRAME;

				double r = 80.0 + frame * 3.0;

				double xZure = SCommon.CRandom.GetDoubleRange(-20.0, 20.0);
				double yZure = SCommon.CRandom.GetDoubleRange(-20.0, 20.0);

				double x = this.X + xZure;
				double y = this.Y + yZure;

				DD.SetSize(new D2Size(r * 2.0, r * 2.0));
				DD.SetAlpha(0.3);
				DD.SetBright(new D3Color(0, 1, 0.5));
				DD.Draw(Pictures.WhiteCircle, new D2Point(x, y));

				this.Crash = ACrash.CreateCircle(new D2Point(x, y), r);

				yield return true;
			}
		}

		public override Type GetLearnedBulletType()
		{
			throw null;
		}
	}
}
