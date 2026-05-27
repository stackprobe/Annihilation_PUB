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
	public class OrdinaryEnemy : EnemyBase
	{
		private double Speed;
		private bool Learnable;

		public OrdinaryEnemy(bool learnable)
		{
			double x;
			double y = (double)SCommon.CRandom.GetRange(100, 300);
			double speed = SCommon.CRandom.GetDoubleRange(3.0, 6.0);

			if (SCommon.CRandom.GetBoolean()) // 左から右(->)
			{
				x = -100.0;
			}
			else // 右から左(<-)
			{
				x = BattleField.Screen.W + 100.0;
				speed = -speed;
			}

			this.Initialize(x, y, 30);
			this.Speed = speed;
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

				this.X += this.Speed;

				if (
					this.X < -100.0 ||
					this.X > BattleField.Screen.W + 100.0
					)
					break; // 画面外に出たので消える。(自分で退場する方式！)

				if (!DD.IsOutOf(
					new D4Rect(0, 0, BattleField.Screen.W, BattleField.Screen.H),
					new D2Point(this.X, this.Y)
					))
				{
					if ((frame / 80) % 2 == 1)
					{
						if (frame % 8 == 0)
						{
							if (this.Learnable && SCommon.CRandom.GetRate() < 0.5)
								GEMain.I.EnemyController.Add(new OrdinaryLearnableBullet(this.X, this.Y));
							else
								GEMain.I.EnemyController.Add(new OrdinaryBullet(this.X, this.Y));
						}
					}
				}

				//DD.AddBright(this.Crashed ? 1.0 : 0.0);
				DD.Draw(Pictures.OrdinaryEnemy, new D2Point(this.X, this.Y));

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
