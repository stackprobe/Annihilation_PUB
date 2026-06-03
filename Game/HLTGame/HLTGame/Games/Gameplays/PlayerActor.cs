using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;
using HLTStudio.Games.Gameplays.Enemies.Bullets;
using HLTStudio.Games.Gameplays.Weapons;

namespace HLTStudio.Games.Gameplays
{
	public class PlayerActor
	{
		/// <summary>
		/// カメラ上の位置(X)
		/// </summary>
		public double X = 0.0;

		/// <summary>
		/// カメラ上の位置(Y)
		/// </summary>
		public double Y = 0.0;

		/// <summary>
		/// 当たり判定
		/// </summary>
		public ACrash Crash;

		/// <summary>
		/// 前回のフレームで敵・敵弾に当たったか
		/// </summary>
		public bool Crashed;

		/// <summary>
		/// 攻撃中か
		/// 1～：攻撃継続フレーム数
		/// 0：攻撃していない
		/// </summary>
		public int AttackingFrame = 0;

		/// <summary>
		/// ボム炸裂中か
		/// </summary>
		public bool BombActiveFlag = false;

		/// <summary>
		/// フィールド展開中か
		/// </summary>
		public bool FieldActiveFlag = false;

		/// <summary>
		/// 移動速度
		/// </summary>
		public double MOVE_SPEED = 6.0;

		/// <summary>
		/// 移動速度(低速)
		/// </summary>
		public double MOVE_SPEED_SLOW = 3.0;

		/// <summary>
		/// フィールド半径
		/// </summary>
		public double FIELD_RADIUS = 170.0;

		// ====

		private const int RESPAWN_INVINCIBLE_FRAME_MAX = 120;

		private double CurrFieldRadius = 0.0;
		private int CurrRespawnInvincibleFrame = 0;
		private double RespawnEffectRate = 0.0;

		public void EachFrame()
		{
			bool slowFlag = Inputs.SLOW.GetInput() >= 1;
			double speed = MOVE_SPEED;

			if (slowFlag)
				speed = MOVE_SPEED_SLOW;

			if (Inputs.DIR_4.GetInput() >= 1)
			{
				this.X -= speed;
			}
			else if (Inputs.DIR_6.GetInput() >= 1)
			{
				this.X += speed;
			}
			if (Inputs.DIR_8.GetInput() >= 1)
			{
				this.Y -= speed;
			}
			else if (Inputs.DIR_2.GetInput() >= 1)
			{
				this.Y += speed;
			}

			this.X = SCommon.ToRange(this.X, 0.0, BattleField.Screen.W);
			this.Y = SCommon.ToRange(this.Y, 0.0, BattleField.Screen.H);

			if (Inputs.ATTACK.GetInput() >= 1)
			{
				this.AttackingFrame++;
				this.Attack();
			}
			else
			{
				this.AttackingFrame = 0;
			}

			if (Inputs.BOMB.GetInput() == 1 && GEMain.I.BombCount >= 1 && !this.BombActiveFlag)
			{
				DD.Countdown(ref GEMain.I.BombCount);
				this.BombActiveFlag = true;
				this.Bomb();
			}

			switch (GameSetting.FieldActiveMode)
			{
				case GameSetting.FieldActiveMode_e.TOGGLE:
					this.FieldActiveFlag ^= Inputs.FIELD_ACTIVE.GetInput() == 1;
					break;

				case GameSetting.FieldActiveMode_e.HOLD:
					this.FieldActiveFlag = Inputs.FIELD_ACTIVE.GetInput() >= 1;
					break;

				default:
					throw null; // never
			}

			if (this.CurrRespawnInvincibleFrame != 0)
			{
				if (this.CurrRespawnInvincibleFrame > RESPAWN_INVINCIBLE_FRAME_MAX)
					this.CurrRespawnInvincibleFrame = 0;
				else
					this.CurrRespawnInvincibleFrame++;

				DD.Approach(ref this.RespawnEffectRate, 0.0, 0.97);
			}

			bool respawnInvicibleFlag = this.CurrRespawnInvincibleFrame != 0;

			if (this.FieldActiveFlag)
			{
				WeaponLarning.AbsorbBullet();

				DD.Approach(ref this.CurrFieldRadius, FIELD_RADIUS, 0.7);
			}
			else
			{
				DD.Approach(ref this.CurrFieldRadius, 0.0, 0.9);
			}

			if (0.0001 < this.CurrFieldRadius)
			{
				DD.SetAlpha(0.3);
				DD.SetBright(new D3Color(0, 1, 1));
				DD.SetSize(new D2Size(this.CurrFieldRadius * 2, this.CurrFieldRadius * 2));
				DD.Draw(Pictures.WhiteCircle, new D2Point(this.X, this.Y));
			}

			if (respawnInvicibleFlag)
			{
				DD.SetAlpha(0.8 * this.RespawnEffectRate);
				DD.SetZoom(1.0 + this.RespawnEffectRate * 7.0);
				DD.Draw(Pictures.Player_自機, new D2Point(this.X, this.Y));

				DD.SetAlpha(0.5);
			}
			DD.Draw(Pictures.Player_自機, new D2Point(this.X, this.Y));

			if (slowFlag)
			{
				DD.SetBright(new D3Color(1, 0, 0));
				DD.SetSize(new D2Size(8, 8));
				DD.Draw(Pictures.WhiteCircle, new D2Point(this.X, this.Y));
			}

			if (!respawnInvicibleFlag)
				this.Crash = ACrash.CreatePoint(new D2Point(this.X, this.Y));
		}

		private void Attack()
		{
			GEMain.I.WeaponController.Add(new OrdinaryWeapon());

			if (DD.ProcFrame % 4 == 0)
			{
				SoundEffects.PlayerShot.Play();
			}

			if (GEMain.I.LearnedWeapon != null)
			{
				WeaponLarning.Attack();
			}
		}

		private void Bomb()
		{
			GEMain.I.WeaponController.Add(new Bomb());

			SoundEffects.Bomb.Play();
		}

		public IEnumerable<bool> KilledEffect(double x, double y)
		{
			foreach (AScene scene in AScene.Create(15))
			{
				double r = 1.0 - scene.Rate;
				r *= r;
				r = 1.0 - r;
				double size = r * 500.0;

				DD.SetAlpha(0.5);
				DD.SetBright(new D3Color(1.0, 0.5, 0.5));
				DD.SetSize(new D2Size(size, size));
				DD.Draw(Pictures.WhiteCircle, new D2Point(x, y));

				yield return true;
			}
		}

		public void StartRespawnInvincible()
		{
			this.CurrRespawnInvincibleFrame = 1;
			this.RespawnEffectRate = 1.0;
		}
	}
}
