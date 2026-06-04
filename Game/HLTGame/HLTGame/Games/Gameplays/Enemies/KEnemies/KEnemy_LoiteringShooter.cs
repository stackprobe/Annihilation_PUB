using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;
using HLTStudio.Games.Gameplays.Enemies.Bullets;

namespace HLTStudio.Games.Gameplays.Enemies.KEnemies
{
	public class KEnemy_LoiteringShooter : KEnemyBase
	{
		private const int ENTRY_FRAME = 55;
		private const int STAY_FRAME = 260;
		private const int FIRST_SHOT_FRAME = 50;
		private const int ORDINARY_SHOT_INTERVAL = 52;
		private const int LEARNABLE_SHOT_INTERVAL = 118;

		private double BaseX;
		private double StartY;
		private double CenterY;
		private double Phase;

		public KEnemy_LoiteringShooter(double x, double y)
			: base(x, y, 80)
		{
			this.BaseX = x;
			this.StartY = y;
			this.CenterY = Math.Max(80.0, Math.Min(BattleField.Screen.H * 0.35, y + 150.0));
			this.Phase = x * 0.017 + y * 0.013;
		}

		protected override IEnumerable<bool> E_EachFrame2()
		{
			for (int frame = 0; ; frame++)
			{
				this.Move(frame);

				if (this.IsInBattleField() && FIRST_SHOT_FRAME <= frame && frame < ENTRY_FRAME + STAY_FRAME)
				{
					if ((frame - FIRST_SHOT_FRAME) % ORDINARY_SHOT_INTERVAL == 0)
					{
						GEMain.I.EnemyController.Add(new OrdinaryBullet(this.X, this.Y));
					}
					if ((frame - FIRST_SHOT_FRAME + LEARNABLE_SHOT_INTERVAL / 2) % LEARNABLE_SHOT_INTERVAL == 0)
					{
						GEMain.I.EnemyController.Add(new Bullet_LoiteringLearnable(this.X, this.Y));
					}
				}

				DD.SetSize(new D2Size(64.0, 64.0));
				DD.Draw(this.GetPicture(), new D2Point(this.X, this.Y));
				this.Crash = ACrash.CreateCircle(new D2Point(this.X, this.Y), this.GetCharacterRadius());

				yield return true;
			}
		}

		private void Move(int frame)
		{
			if (frame < ENTRY_FRAME)
			{
				double rate = (double)frame / ENTRY_FRAME;
				double easedRate = 1.0 - (1.0 - rate) * (1.0 - rate);

				this.X = this.BaseX + Math.Sin(frame * 0.095 + this.Phase) * (12.0 + 22.0 * rate);
				this.Y = this.StartY + (this.CenterY - this.StartY) * easedRate + Math.Sin(frame * 0.07 + this.Phase) * 6.0;
			}
			else if (frame < ENTRY_FRAME + STAY_FRAME)
			{
				int stayFrame = frame - ENTRY_FRAME;

				this.X = this.BaseX + Math.Sin(stayFrame * 0.035 + this.Phase) * 42.0 + Math.Sin(stayFrame * 0.083) * 12.0;
				this.Y = this.CenterY + Math.Cos(stayFrame * 0.047 + this.Phase) * 22.0 + Math.Sin(stayFrame * 0.021) * 8.0;
			}
			else
			{
				int exitFrame = frame - ENTRY_FRAME - STAY_FRAME;

				this.X = this.BaseX + Math.Sin(frame * 0.041 + this.Phase) * 50.0;
				this.Y = this.CenterY + exitFrame * 1.8 + Math.Cos(frame * 0.053 + this.Phase) * 18.0;
			}
		}

		private bool IsInBattleField()
		{
			return !DD.IsOutOf(
				new D4Rect(0, 0, BattleField.Screen.W, BattleField.Screen.H),
				new D2Point(this.X, this.Y)
				);
		}

		protected override double GetCharacterRadius()
		{
			return 28.0;
		}

		protected override APicture GetPicture()
		{
			return Pictures.Dummy;
		}
	}
}
