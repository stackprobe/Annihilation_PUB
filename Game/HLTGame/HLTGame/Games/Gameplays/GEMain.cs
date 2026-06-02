using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;
using HLTStudio.Games.FrontEnds;
using HLTStudio.Games.Gameplays.Enemies;
using HLTStudio.Games.Gameplays.Enemies.Bullets;
using HLTStudio.Games.Gameplays.Scenarios;
using HLTStudio.Games.Gameplays.Walls;
using HLTStudio.Games.Gameplays.Weapons;

namespace HLTStudio.Games.Gameplays
{
	public class GEMain
	{
		public static GEMain I = null;

		public static void Run()
		{
			I = new GEMain();
			try
			{
				I.Run2();
			}
			finally
			{
				I = null;
			}
		}

		private GEMain()
		{ }

		public ScenarioBase Scenario = new OrdinaryScenario(); // TODO
		public WallBase Wall = new OrdinaryWall(); // TODO
		public Background Background = new Background();
		public BattleField BattleField = new BattleField();
		public PlayerActor PlayerActor = new PlayerActor();
		public EnemyController EnemyController = new EnemyController();
		public WeaponController WeaponController = new WeaponController();

		public int PlayerLife = GEConsts.INIT_PLAYER_LIFE;
		public int BombCount = GEConsts.INIT_BOMB_COUNT;
		public long Score = 0L;
		public int LearnedPermil = 0;
		public Type LearningWeapon = null;
		public Type LearnedWeapon = null;

		private void Run2()
		{
			Musics.BattleField.Play();

			this.PlayerActor.X = GEConsts.PLAYER_SPAWN_X;
			this.PlayerActor.Y = GEConsts.PLAYER_SPAWN_Y;

			for (; ; )
			{
				if (Inputs.START.GetInput() == 1) // 一時停止
				{
					DD.FreezeGameUntil(() => Inputs.START.GetInput() == 0);

					this.Pause();

					if (this.Pause_ReturnToTitle)
						break;
				}
				if (ProcMain.DEBUG && AKeyboard.GetInput(DX.KEY_INPUT_F11) == 1) // デバッグ用_処理落ち
				{
					DD.HeavyDelayForDebug ^= true;
				}
				if (/*ProcMain.DEBUG &&*/ AKeyboard.GetInput(DX.KEY_INPUT_F7) == 1) // デバッグ用_学習1
				{
					GEMain.I.LearnedWeapon = typeof(OrdinaryLearnableBullet);
				}
				if (/*ProcMain.DEBUG &&*/ AKeyboard.GetInput(DX.KEY_INPUT_F8) == 1) // デバッグ用_学習2
				{
					GEMain.I.LearnedWeapon = typeof(OrdinaryLearnableBullet2);
				}

				this.Background.Draw();

				using (BattleField.Screen.Section())
				{
					this.ClearCollisions();
					this.Scenario.EachFrame();
					this.Wall.EachFrame();
					this.PlayerActor.EachFrame();
					this.EnemyController.EachFrame();
					this.WeaponController.EachFrame();
					this.DetectCollisions();
					WeaponLarning.DetectAnnihilations();
					this.DrawAllCrash();
				}

				this.BattleField.DrawToMainScreen();

				DD.EachFrame();

				if (this.PlayerActor.Crashed)
				{
					SoundEffects.PlayerCrashed.Play();

					if (1 <= this.PlayerLife)
					{
						this.PlayerMiss();
						this.PlayerLife--;
					}
					else
					{
						this.Gameover();
						break;
					}
				}
			}

			DD.SetCurtain(-1.0);

			using (DD.FreeScreen.Section())
			{
				DD.Draw(DD.LastMainScreen.GetPicture(), new D4Rect(0, 0, GameConfig.ScreenSize.W, GameConfig.ScreenSize.H));
			}

			foreach (AScene scene in AScene.Create(30))
			{
				DD.Draw(DD.FreeScreen.GetPicture(), new D2Point(GameConfig.ScreenSize.W / 2, GameConfig.ScreenSize.H / 2));

				DD.EachFrame();
			}
		}

		private void PlayerMiss()
		{
			using (BattleField.FreeScreen.Section())
			{
				DD.Draw(BattleField.Screen.GetPicture(), new D4Rect(0, 0, BattleField.Screen.W, BattleField.Screen.H));
			}

			double redFogRate = 0.0;

			foreach (AScene scene in AScene.Create(30))
			{
				DD.Approach(ref redFogRate, 0.5, 0.8);

				this.Background.Draw();

				using (BattleField.Screen.Section())
				{
					DD.Draw(BattleField.FreeScreen.GetPicture(), new D4Rect(0, 0, BattleField.Screen.W, BattleField.Screen.H));

					DD.SetAlpha(redFogRate);
					DD.SetBright(new D3Color(1, 0, 0));
					DD.Draw(Pictures.WhiteBox, new D4Rect(0, 0, BattleField.Screen.W, BattleField.Screen.H));
				}

				this.BattleField.DrawToMainScreen();

				DD.EachFrame();
			}

			DD.TL.Add(SCommon.Supplier(this.PlayerActor.KilledEffect(this.PlayerActor.X, this.PlayerActor.Y)));

			this.PlayerActor.X = GEConsts.PLAYER_SPAWN_X;
			this.PlayerActor.Y = GEConsts.PLAYER_SPAWN_Y;

			this.PlayerActor.StartRespawnInvincible();

			this.BombCount = GEConsts.INIT_BOMB_COUNT;

			SoundEffects.Buy.Play();
		}

		private void Gameover()
		{
			using (BattleField.FreeScreen.Section())
			{
				DD.Draw(BattleField.Screen.GetPicture(), new D4Rect(0, 0, BattleField.Screen.W, BattleField.Screen.H));
			}

			double redFogRate = 0.0;

			foreach (AScene scene in AScene.Create(60))
			{
				if (scene.Numer == 30)
					AMusic.FadeOut();

				DD.Approach(ref redFogRate, 0.5, 0.8);

				this.Background.Draw();

				using (BattleField.Screen.Section())
				{
					DD.Draw(BattleField.FreeScreen.GetPicture(), new D4Rect(0, 0, BattleField.Screen.W, BattleField.Screen.H));

					DD.SetAlpha(redFogRate);
					DD.SetBright(new D3Color(1, 0, 0));
					DD.Draw(Pictures.WhiteBox, new D4Rect(0, 0, BattleField.Screen.W, BattleField.Screen.H));
				}

				this.BattleField.DrawToMainScreen();

				DD.EachFrame();
			}
		}

		private void ClearCollisions()
		{
			foreach (EnemyBase enemy in this.EnemyController.Iterate())
				enemy.Crash = ACrash.CreateNone(); // reset

			foreach (WeaponBase weapon in this.WeaponController.Iterate())
				weapon.Crash = ACrash.CreateNone(); // reset

			this.PlayerActor.Crash = ACrash.CreateNone(); // reset
		}

		private void DetectCollisions()
		{
			foreach (EnemyBase enemy in this.EnemyController.Iterate())
				enemy.Crashed = false; // reset

			foreach (WeaponBase weapon in this.WeaponController.Iterate())
				weapon.Crashed = false; // reset

			this.PlayerActor.Crashed = false; // reset

			foreach (EnemyBase enemy in this.EnemyController.Iterate())
			{
				foreach (WeaponBase weapon in this.WeaponController.Iterate())
				{
					if (ACrash.IsCrashed(enemy.Crash, weapon.Crash))
					{
						int damagePoint = Math.Min(enemy.HP, weapon.AttackPoint);

						if (1 <= damagePoint)
						{
							enemy.HP -= damagePoint;
							weapon.AttackPoint -= damagePoint;

							enemy.Crashed = true;
							weapon.Crashed = true;

							SoundEffects.EnemyDamaged.Play();
						}
					}
				}

				if (ACrash.IsCrashed(enemy.Crash, this.PlayerActor.Crash))
					this.PlayerActor.Crashed = true;
			}
		}

		private bool DrawAllCrashEnabled = false;

		private void DrawAllCrash()
		{
			if (AKeyboard.GetInput(DX.KEY_INPUT_F12) == 1)
				this.DrawAllCrashEnabled ^= true;

			if (!this.DrawAllCrashEnabled)
				return;

			// ====

			DD.DrawCurtain(-0.6);

			const double A = 0.6;

			foreach (EnemyBase enemy in this.EnemyController.Iterate())
				enemy.Crash.Draw(new D4Color(1.0, 1.0, 0.0, A), new D2Point(0.0, 0.0));

			foreach (WeaponBase weapon in this.WeaponController.Iterate())
				weapon.Crash.Draw(new D4Color(0.0, 1.0, 1.0, A), new D2Point(0.0, 0.0));

			this.PlayerActor.Crash.Draw(new D4Color(1.0, 0.0, 1.0, A), new D2Point(0.0, 0.0));
		}

		private static VScreen PauseWall = new VScreen(GameConfig.ScreenSize.W, GameConfig.ScreenSize.H);

		private bool Pause_ReturnToTitle = false;

		private void Pause()
		{
			this.Pause_ReturnToTitle = false; // reset

			using (PauseWall.Section())
			{
				DD.Draw(DD.LastMainScreen.GetPicture(), new I2Point(GameConfig.ScreenSize.W / 2, GameConfig.ScreenSize.H / 2).ToD2Point());
			}

			SimpleMenu menu = new SimpleMenu(24, 30, 16, 400, "PAUSE", new string[]
 			{
				"NOOP",
				"タイトルメニューに戻る",
				"ゲームに戻る",
			});

			menu.NoPound = true;
			menu.CancelByPause = true;

			double blurRate = 0.0;

			for (; ; )
			{
				for (; ; )
				{
					DD.Approach(ref blurRate, 0.5, 0.98);

					DD.Draw(PauseWall.GetPicture(), new I2Point(GameConfig.ScreenSize.W / 2, GameConfig.ScreenSize.H / 2).ToD2Point());
					DD.Blur(blurRate);

					if (menu.Draw())
						break;

					DD.EachFrame();
				}

				switch (menu.SelectedIndex)
				{
					case 0:
						// noop
						break;

					case 1:
						this.Pause_ReturnToTitle = true;
						goto endOfMenu;

					case 2:
						goto endOfMenu;

					default:
						throw null; // never
				}
			}
		endOfMenu:

			PauseWall.Unload();
		}
	}
}
