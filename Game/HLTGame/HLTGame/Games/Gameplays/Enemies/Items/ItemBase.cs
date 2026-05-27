using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;

namespace HLTStudio.Games.Gameplays.Enemies.Items
{
	public abstract class ItemBase : EnemyBase
	{
		private double YSpeed = -3.0;
		private bool Vacuuming = false;

		protected void Initialize_Item(double x, double y)
		{
			this.Initialize(x, y, 0);
		}

		protected override IEnumerable<bool> E_EachFrame()
		{
			for (; ; )
			{
				if (this.Y > BattleField.Screen.H + 100.0) // ? 画面の下に行き過ぎた。-> 消滅
					break;

				if (this.Vacuuming)
				{
					D2Point speed = DD.MakeXYSpeed(
						new D2Point(this.X, this.Y),
						new D2Point(GEMain.I.PlayerActor.X, GEMain.I.PlayerActor.Y),
						8.0
						);

					this.X += speed.X;
					this.Y += speed.Y;
				}
				else
				{
					this.Y += this.YSpeed;
					this.YSpeed += 0.2;
				}

				double d = DD.GetDistance(
					new D2Point(this.X, this.Y),
					new D2Point(GEMain.I.PlayerActor.X, GEMain.I.PlayerActor.Y)
					);

				if (d < 10.0)
				{
					this.PlayerCollected();
					SoundEffects.アイテム取得.Play();
					break;
				}
				if (d < 100.0)
					this.Vacuuming = true;

				DD.Draw(this.GetPicture(), new D2Point(this.X, this.Y));

				yield return true;
			}
		}

		protected abstract APicture GetPicture();

		protected abstract void PlayerCollected();
	}
}
