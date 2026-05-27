using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;
using HLTStudio.Games.Gameplays.Enemies.Bullets;
using HLTStudio.Games.Gameplays.Enemies.Items;

namespace HLTStudio.Games.Gameplays.Enemies
{
	public class OrdinaryEnemy2 : EnemyBase
	{
		private double Speed;
		private bool Learnable;

		public OrdinaryEnemy2(bool learnable)
		{
			this.Initialize(
				(double)SCommon.CRandom.GetRange(30, BattleField.Screen.W - 30),
				-50,
				15
				);

			this.Speed = SCommon.CRandom.GetDoubleRange(2.5, 3.5);
			this.Learnable = learnable;
		}

		protected override IEnumerable<bool> E_EachFrame()
		{
			for (int frame = 0; ; frame++)
			{
				if (this.HP <= 0) // 体力ゼロなので死亡する。(自分で消える方式！)
				{
					GEMain.I.EnemyController.Add(new OrdinaryItem(this.X, this.Y));
					DD.TL.Add(SCommon.Supplier(this.KilledEffect()));
					SoundEffects.EnemyKilled.Play();
					break;
				}

				this.Y += this.Speed;

				if (this.Y > BattleField.Screen.H + 50.0) // 画面外に出たので消える。(自分で退場する方式！)
					break;

				if (!DD.IsOutOf(
					new D4Rect(0, 0, BattleField.Screen.W, BattleField.Screen.H),
					new D2Point(this.X, this.Y)
					))
				{
					if ((frame / 60) % 2 == 1)
					{
						if (frame % 6 == 0)
						{
							if (this.Learnable)
								GEMain.I.EnemyController.Add(new OrdinaryLearnableBullet2(this.X, this.Y));
							else
								GEMain.I.EnemyController.Add(new OrdinaryBullet(this.X, this.Y));
						}
					}
				}

				//DD.AddBright(this.Crashed ? 1.0 : 0.0);
				DD.Draw(Pictures.OrdinaryEnemy2, new D2Point(this.X, this.Y));

				this.Crash = ACrash.CreateCircle(new D2Point(this.X, this.Y), 40.0);

				yield return true;
			}
		}

		private IEnumerable<bool> KilledEffect()
		{
			foreach (AScene scene in AScene.Create(5))
			{
				double size = 500 - 200 * scene.Rate;

				DD.SetAlpha(0.5);
				DD.SetBright(new D3Color(1.0, 1.0, 0.0));
				DD.SetSize(new D2Size(size, size));
				DD.Draw(Pictures.WhiteCircle, new D2Point(this.X, this.Y));

				yield return true;
			}
		}
	}
}
