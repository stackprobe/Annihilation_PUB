using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;

namespace HLTStudio.Games.Gameplays.Enemies
{
	public abstract class EnemyBase
	{
		/// <summary>
		/// 戦闘フィールド上の位置(X)
		/// </summary>
		public double X;

		/// <summary>
		/// 戦闘フィールド上の位置(Y)
		/// </summary>
		public double Y;

		/// <summary>
		/// 残り体力
		/// 1～：生きている
		/// 0：死亡した || 無敵
		/// </summary>
		public int HP;

		/// <summary>
		/// 当たり判定
		/// 毎フレーム設定すること。
		/// 無敵である場合はセットしないこと。
		/// </summary>
		public ACrash Crash;

		/// <summary>
		/// 前回のフレームでプレイヤー弾に当たったか
		/// </summary>
		public bool Crashed;

		// 実装ヒント：
		// 敵
		// -- HP：1～, 当たり判定を設置する, 描画する
		// 敵弾
		// -- HP：0, 当たり判定を設置する, 描画する
		// アイテム
		// -- HP：0, 当たり判定を設置しない, 描画する
		// イベント
		// -- HP：0, 当たり判定を設置しない, 描画しない

		protected void Initialize(double x, double y, int hp)
		{
			this.X = x;
			this.Y = y;
			this.HP = hp;
			this.Crash = ACrash.CreateNone();
			this.Crashed = false;
		}

		private Func<bool> S_EachFrame = null;

		public bool EachFrame()
		{
			if (this.S_EachFrame == null)
				this.S_EachFrame = SCommon.Supplier(this.E_EachFrame());

			return this.S_EachFrame();
		}

		protected abstract IEnumerable<bool> E_EachFrame();
	}
}
