using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.Games.Gameplays.Enemies.Bullets;

namespace HLTStudio.Games.Gameplays.Weapons
{
	public abstract class WeaponBase
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
		/// 残り攻撃力
		/// 1～：攻撃力
		/// 0：攻撃力を使い果たした || 攻撃能力無し
		/// </summary>
		public int AttackPoint;

		/// <summary>
		/// 学習した武器か
		/// </summary>
		public bool Learned;

		/// <summary>
		/// 当たり判定
		/// 毎フレーム設定すること。
		/// 攻撃能力無しの場合はセットしないこと。
		/// </summary>
		public ACrash Crash;

		/// <summary>
		/// 前回のフレームでプレイヤー弾に当たったか
		/// </summary>
		public bool Crashed;

		protected void Initialize(int attackPoint, bool learned)
		{
			D2Point pt = WeaponCommon.GetShotStartPosition();

			this.X = pt.X;
			this.Y = pt.Y;
			this.AttackPoint = attackPoint;
			this.Learned = learned;
		}

		protected void Initialize(double x, double y, int attackPoint, bool learned)
		{
			this.X = x;
			this.Y = y;
			this.AttackPoint = attackPoint;
			this.Learned = learned;
		}

		private Func<bool> S_EachFrame = null;

		public bool EachFrame()
		{
			if (this.S_EachFrame == null)
				this.S_EachFrame = SCommon.Supplier(this.E_EachFrame());

			return this.S_EachFrame();
		}

		protected abstract IEnumerable<bool> E_EachFrame();

		public abstract Type GetLearnedBulletType();
	}
}
