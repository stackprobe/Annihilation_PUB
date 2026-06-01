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
	public class KEnemy_Ordinary : KEnemyBase
	{
		private const double MOVE_SPEED_Y = 2.0;
		private const double SWAY_WIDTH = 24.0;
		private const double SWAY_SPEED = 0.035;
		private const int FIRST_SHOT_FRAME = 45;
		private const int SHOT_INTERVAL = 70;

		private double BaseX;
		private double SwayPhase;

		public KEnemy_Ordinary(double x, double y)
			: base(x, y, 50)
		{
			// 出現位置を基準点として持っておき、普通の敵は「指定された位置から来る」ことを優先する。
			// ランダム生成やルート選択は呼び出し側に任せると、KEnemies 配下の敵を配置部品として使いやすい。
			this.BaseX = x;
			this.SwayPhase = x * 0.01;
		}

		protected override IEnumerable<bool> E_EachFrame2()
		{
			for (int frame = 0; ; frame++)
			{
				// 最初の実装なので、複雑な状態遷移は持たせず「ゆっくり降下しながら少し揺れる」敵にする。
				// 完全な直線移動より画面上で個体として見つけやすく、後続の KEnemy 実装にも移動処理の置き場を示せる。
				this.Y += MOVE_SPEED_Y;
				this.X = this.BaseX + Math.Sin(frame * SWAY_SPEED + this.SwayPhase) * SWAY_WIDTH;

				// 画面内に入ってから弾を撃つ。画面外発射を避けることで、出現直後の理不尽さを抑える。
				if (this.IsInBattleField())
				{
					if (FIRST_SHOT_FRAME <= frame && (frame - FIRST_SHOT_FRAME) % SHOT_INTERVAL == 0)
					{
						// Ordinary は学習弾や特殊弾ではなく、現在のプレイヤー位置へ向かう標準弾だけを撃つ。
						// まずは KEnemies の素朴な敵サンプルとして、弾種の分岐を増やさない。
						GEMain.I.EnemyController.Add(new OrdinaryBullet(this.X, this.Y));
					}
				}

				// KEnemyBase は死亡演出と退場判定を担当するため、個別敵側では毎フレームの見た目と当たり判定だけを更新する。
				DD.Draw(this.GetPicture(), new D2Point(this.X, this.Y));
				this.Crash = ACrash.CreateCircle(new D2Point(this.X, this.Y), this.GetCharacterRadius());

				yield return true;
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
			return 30.0;
		}

		protected override APicture GetPicture()
		{
			return Pictures.KOrdinaryEnemy;
		}
	}
}
