using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;
using HLTStudio.Games.Gameplays.Enemies;
using HLTStudio.Games.Gameplays.Enemies.Bullets;
using HLTStudio.Games.Gameplays.Weapons;

namespace HLTStudio.Games.Gameplays
{
	public static class WeaponLarning
	{
		public static void Attack()
		{
			if (GEMain.I.LearnedWeapon == typeof(OrdinaryLearnableBullet))
			{
				if (DD.ProcFrame % 5 == 0)
				{
					GEMain.I.WeaponController.Add(new OrdinaryLearnedWeapon());
				}
			}
			else if (GEMain.I.LearnedWeapon == typeof(OrdinaryLearnableBullet2))
			{
				if (DD.ProcFrame % 3 == 0)
				{
					GEMain.I.WeaponController.Add(new OrdinaryLearnedWeapon2());
				}
			}
			else
			{
				throw null; // never
			}
		}

		private static void Annihilate(WeaponBase weapon)
		{
			if (weapon is OrdinaryLearnedWeapon)
			{
				GEMain.I.WeaponController.Add(new AnnihilateExplode(weapon.X, weapon.Y)); // 仮
			}
			else if (weapon is OrdinaryLearnedWeapon2)
			{
				GEMain.I.WeaponController.Add(new AnnihilateExplode(weapon.X, weapon.Y)); // 仮
			}
			else
			{
				throw null; // never
			}
		}

		public static void AbsorbBullet()
		{
			List<EnemyBase> enemies = GEMain.I.EnemyController.GetEnemies();

			D2Point absorbCenter = new D2Point(GEMain.I.PlayerActor.X, GEMain.I.PlayerActor.Y);
			double absorbRadius = GEMain.I.PlayerActor.FIELD_RADIUS;

			for (int index = 0; index < enemies.Count; index++)
			{
				EnemyBase enemy = enemies[index];

				if (!(enemy is BulletBase))
					continue;

				BulletBase bullet = (BulletBase)enemy;

				if (!bullet.Learnable)
					continue;

				D2Point bulletPt = new D2Point(bullet.X, bullet.Y);

				if (DD.GetDistance(bulletPt, absorbCenter) > absorbRadius)
					continue;

				DD.TL.Add(SCommon.Supplier(E_敵弾の吸収エフェクト(bullet.X, bullet.Y)));

				if (
					GEMain.I.LearningWeapon == null ||
					GEMain.I.LearningWeapon != bullet.GetType()
					)
				{
					SoundEffects.学習開始.Play();

					GEMain.I.LearnedPermil = 0;
					GEMain.I.LearningWeapon = bullet.GetType();
				}
				if (GEMain.I.LearnedPermil < 1000)
				{
					GEMain.I.LearnedPermil += bullet.GetLearnPoint();

					if (GEMain.I.LearnedPermil >= 1000)
					{
						SoundEffects.学習完了.Play();

						GEMain.I.LearnedPermil = 1000;
						GEMain.I.LearnedWeapon = GEMain.I.LearningWeapon;
					}
				}
				SCommon.FastDesertElement(enemies, index--);
			}
		}

		private static IEnumerable<bool> E_敵弾の吸収エフェクト(double x, double y)
		{
			SoundEffects.敵弾を吸収.Play();

			const double SPEED = 14.0;

			for (; ; )
			{
				if (DD.GetDistance(
					new D2Point(x, y),
					new D2Point(GEMain.I.PlayerActor.X, GEMain.I.PlayerActor.Y)
					) < SPEED
					)
					break;

				D2Point speed = DD.MakeXYSpeed(
					new D2Point(x, y),
					new D2Point(GEMain.I.PlayerActor.X, GEMain.I.PlayerActor.Y),
					SPEED
					);

				x += speed.X;
				y += speed.Y;

				DD.SetAlpha(0.7);
				DD.SetBright(new D3Color(0.5, 0.5, 1.0));
				DD.SetSize(new D2Size(60.0, 60.0));
				DD.Draw(Pictures.WhiteCircle, new D2Point(x, y));

				yield return true;
			}
		}

		public static void DetectAnnihilations()
		{
			List<EnemyBase> enemies = GEMain.I.EnemyController.GetEnemies();
			List<WeaponBase> weapons = GEMain.I.WeaponController.GetWeapons();

			for (int ei = 0; ei < enemies.Count; ei++)
			{
				EnemyBase enemy = enemies[ei];

				if (!(enemy is BulletBase))
					continue;

				BulletBase bullet = (BulletBase)enemy;

				if (!bullet.Learnable)
					continue;

				for (int wi = 0; wi < weapons.Count; wi++)
				{
					WeaponBase weapon = weapons[wi];

					if (!weapon.Learned)
						continue;

					if (weapon.GetLearnedBulletType() != enemy.GetType())
						continue;

					if (!ACrash.IsCrashed(weapon.Crash, enemy.Crash))
						continue;

					SoundEffects.対消滅.Play();

					Annihilate(weapon);

					SCommon.FastDesertElement(enemies, ei--);
					SCommon.FastDesertElement(weapons, wi--);

					break;
				}
			}
		}
	}
}
