using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;
using HLTStudio.Games.Gameplays;
using HLTStudio.Games.Gameplays.Scenarios;

namespace HLTStudio.Games.FrontEnds
{
	public static class TitleMenu
	{
		private static string FONT_NAME = "木漏れ日ゴシック";
		private static ASoundEffect[] TESTING_SOUND_EFFECTS = new ASoundEffect[]
		{
			SoundEffects.Save,
			SoundEffects.Load,
			SoundEffects.Buy,
		};

		private static Action DrawWall;

		public static void Run()
		{
			DD.SetCurtain(-1.0, 0);
			DD.SetCurtain(0.0);

			Musics.RemotestLibrary.Play();

			bool subMenuFlag = false;

			{
				double a = 0.0;
				double x = 50.0;
				double z = 1.3;

				DrawWall = () =>
				{
					DD.Approach(ref a, subMenuFlag ? 0.0 : 0.5, 0.97);
					DD.Approach(ref x, 0.0, 0.93);
					DD.Approach(ref z, 1.0, 0.999);

					DD.SetZoom(z);
					DD.Draw(Pictures.TitleWall, new D2Point(
						GameConfig.ScreenSize.W * 0.5,
						GameConfig.ScreenSize.H * 0.5
						));

					DD.SetAlpha(a);
					DD.Draw(Pictures.TitleLogo, new D2Point(700.0 - x, 250.0));
				};
			}

			SimpleMenu menu = new SimpleMenu(40, 40, 40, 440, null, new string[]
			{
				"スタート",
				"コンテニュー？",
				"設定",
				"終了",
			});

			for (; ; )
			{
				for (; ; )
				{
					DrawWall();

					if (menu.Draw())
						break;

					DD.EachFrame();
				}

				switch (menu.SelectedIndex)
				{
					case 0:
						StartTheGame();
						break;

					case 1:
						//ContinueTheGame();
						break;

					case 2:
						subMenuFlag = true;
						Setting();
						subMenuFlag = false;
						break;

					case 3:
						goto endOfMenu;

					default:
						throw null; // never
				}
			}
		endOfMenu:

			DD.SetCurtain(-1.0);
			AMusic.FadeOut(30);

			foreach (AScene scene in AScene.Create(40))
			{
				DrawWall();
				DD.EachFrame();
			}
		}

		private static void StartTheGame()
		{
			Leave();

			GEMaster.Run(0);

			Enter();
		}

		private static void Leave()
		{
			using (DD.FreeScreen.Section())
			{
				DD.Draw(DD.LastMainScreen.GetPicture(), new D2Point(GameConfig.ScreenSize.W / 2.0, GameConfig.ScreenSize.H / 2.0));
			}

			DD.SetCurtain(-1.0);

			foreach (AScene scene in AScene.Create(40))
			{
				DD.Draw(DD.FreeScreen.GetPicture(), new D2Point(GameConfig.ScreenSize.W / 2.0, GameConfig.ScreenSize.H / 2.0));
				DD.EachFrame();
			}
			DD.SetCurtain(0.0);
		}

		private static void Enter()
		{
			DD.SetCurtain(0.0);
			Musics.RemotestLibrary.Play();
		}

		private static void Setting()
		{
			SimpleMenu menu = new SimpleMenu(30, 40, 20, 540, "設定", new string[]
			{
				"ゲームパッドのボタン設定",
				"キーボードのキー設定",
				"ウィンドウサイズの変更",
				"ＢＭＧ音量",
				"ＳＥ音量",
				"マウスの表示／非表示",
				"フィールド展開ボタンの設定",
				"戻る",
			});

			for (; ; )
			{
				for (; ; )
				{
					DrawWall();

					if (menu.Draw())
						break;

					DD.EachFrame();
				}

				switch (menu.SelectedIndex)
				{
					case 0:
						CustomizePad();
						break;

					case 1:
						CustomizeKeyboard();
						break;

					case 2:
						ChangeWindowSize();
						break;

					case 3:
						ChangeMusicVolume();
						break;

					case 4:
						ChangeSEVolume();
						break;

					case 5:
						ChangeMouseEnabled();
						break;

					case 6:
						FieldActiveButtonSetting();
						break;

					case 7:
						goto endOfMenu;

					default:
						throw null; // never
				}
			}
		endOfMenu:

			DD.Save();
		}

		private static void CustomizePad()
		{
			AInput[] inputs = DD.GetAllInput(false);
			int[] dest = new int[inputs.Length];

			for (int index = 0; index < inputs.Length;)
			{
				if (AKeyboard.GetInput(DX.KEY_INPUT_SPACE) == 1)
					goto cancelled;

				int button = -1;

				for (int c = 0; c < APad.BUTTON_MAX; c++)
					if (APad.PrimaryPad != -1 && APad.GetInput(c, APad.PrimaryPad) == 1)
						button = c;

				for (int c = 0; c < index; c++)
					if (dest[c] == button)
						button = -1;

				if (button != -1)
				{
					dest[index] = button;
					index++;
				}

				// ここから描画

				DrawWall();
				DD.DrawCurtain(-0.5);

				DD.SetPrint(24, 20, 34, FONT_NAME, 20);
				DD.PrintLine("ゲームパッドのボタン設定");

				for (int c = 0; c < inputs.Length; c++)
				{
					DD.Print(c == index ? "[>] " : "[ ] ");
					DD.Print(inputs[c].NameOfFunction);

					if (c < index)
					{
						DD.Print("　>>>　(" + dest[c] + ")");
					}
					DD.PrintRet();
				}

				DD.SetPrint(428, 428, 34, FONT_NAME, 20);
				DD.PrintLine("/// ＴＩＰＳ ///");
				DD.PrintLine("カーソルの指す機能に割り当てるボタンを押して下さい。");
				DD.SetPrintColor(new I3Color(255, 255, 0));
				DD.PrintLine("スペースキーを押すとキャンセルします。");

				DD.EachFrame();
			}

			using (DD.FreeScreen.Section())
			{
				DD.Draw(DD.LastMainScreen.GetPicture(), new I2Point(GameConfig.ScreenSize.W / 2, GameConfig.ScreenSize.H / 2).ToD2Point());
			}

			foreach (AScene scene in AScene.Create(30))
			{
				DD.Draw(DD.FreeScreen.GetPicture(), new I2Point(GameConfig.ScreenSize.W / 2, GameConfig.ScreenSize.H / 2).ToD2Point());
				DD.EachFrame();
			}

			DD.FreeScreen.Unload();

			for (int index = 0; index < inputs.Length; index++)
			{
				inputs[index].Button = dest[index];
			}

		cancelled:
			DD.OBSOLETE_FreezeInputFrame = 1; // HACK
		}

		private static void CustomizeKeyboard()
		{
			DX.SetMouseDispFlag(1);

			AInput[] inputs = DD.GetAllInput(false);
			int[] dest = new int[inputs.Length];

			for (int index = 0; index < inputs.Length;)
			{
				if (AMouse.R.GetInput() == -1)
					goto cancelled;

				int key = -1;

				for (int c = 0; c < AKeyboard.KEY_MAX && key == -1; c++)
					if (AKeyboard.GetInput(c) == 1)
						key = c;

				// アサインできないキー
				int[] unassignableKeys = new int[]
				{
					DX.KEY_INPUT_ESCAPE, // ゲーム終了に使用

					// ----
					// シフト系キー(左から右)

					DX.KEY_INPUT_CAPSLOCK,
					//DX.KEY_INPUT_LSHIFT,
					//DX.KEY_INPUT_LCONTROL,
					DX.KEY_INPUT_LWIN,
					DX.KEY_INPUT_LALT,
					DX.KEY_INPUT_KANA, // [カタカナ/ひらがな/ローマ字]キー
					DX.KEY_INPUT_RALT,
					DX.KEY_INPUT_RWIN,
					DX.KEY_INPUT_APPS,
					//DX.KEY_INPUT_RCONTROL,
					//DX.KEY_INPUT_RSHIFT,

					// ----
					// ファンクションキー

					DX.KEY_INPUT_F1,
					DX.KEY_INPUT_F2,
					DX.KEY_INPUT_F3,
					DX.KEY_INPUT_F4,
					DX.KEY_INPUT_F5,
					DX.KEY_INPUT_F6,
					DX.KEY_INPUT_F7,
					DX.KEY_INPUT_F8,
					DX.KEY_INPUT_F9,
					DX.KEY_INPUT_F10,
					DX.KEY_INPUT_F11,
					DX.KEY_INPUT_F12,

					// ----

					DX.KEY_INPUT_SYSRQ, // PrintScreen/SysRq
					DX.KEY_INPUT_SCROLL, // ScrollLock
					DX.KEY_INPUT_PAUSE, // Pause/Break
					DX.KEY_INPUT_NUMLOCK, // NumLock
				};

				foreach (int unassignableKey in unassignableKeys) // アサインできないキーを却下する。
					if (key == unassignableKey)
						key = -1;

				for (int c = 0; c < index; c++) // 既にアサインされたキーを却下する。
					if (key == dest[c])
						key = -1;

				if (key != -1)
				{
					dest[index] = key;
					index++;
				}

				// ここから描画

				DrawWall();
				DD.DrawCurtain(-0.5);

				DD.SetPrint(24, 20, 34, FONT_NAME, 20);
				DD.PrintLine("キーボードのキー設定");

				for (int c = 0; c < inputs.Length; c++)
				{
					DD.Print(c == index ? "[>] " : "[ ] ");
					DD.Print(inputs[c].NameOfFunction);

					if (c < index)
					{
						DD.Print("　>>>　");
						DD.Print(AKeyboard.Keys.GetNames()[dest[c]]);
					}
					DD.PrintRet();
				}

				DD.SetPrint(448, 428, 34, FONT_NAME, 20);
				DD.PrintLine("/// ＴＩＰＳ ///");
				DD.PrintLine("カーソルの指す機能に割り当てるキーを押して下さい。");
				DD.SetPrintColor(new I3Color(255, 255, 0));
				DD.PrintLine("画面を右クリックするとキャンセルします。");

				DD.EachFrame();
			}

			using (DD.FreeScreen.Section())
			{
				DD.Draw(DD.LastMainScreen.GetPicture(), new I2Point(GameConfig.ScreenSize.W / 2, GameConfig.ScreenSize.H / 2).ToD2Point());
			}

			foreach (AScene scene in AScene.Create(30))
			{
				DD.Draw(DD.FreeScreen.GetPicture(), new I2Point(GameConfig.ScreenSize.W / 2, GameConfig.ScreenSize.H / 2).ToD2Point());
				DD.EachFrame();
			}

			DD.FreeScreen.Unload();

			for (int index = 0; index < inputs.Length; index++)
			{
				inputs[index].Keys = new int[] { dest[index] };
			}

		cancelled:
			DX.SetMouseDispFlag(GameSetting.MouseCursorShow ? 1 : 0);
			DD.OBSOLETE_FreezeInputFrame = 1; // HACK
		}

		private static void ChangeWindowSize()
		{
			I2Size[] sizes = new I2Size[]
			{
				GameConfig.ScreenSize,
				(GameConfig.ScreenSize.ToD2Size() * 1.1).ToI2Size(),
				(GameConfig.ScreenSize.ToD2Size() * 1.2).ToI2Size(),
				(GameConfig.ScreenSize.ToD2Size() * 1.3).ToI2Size(),
				(GameConfig.ScreenSize.ToD2Size() * 1.4).ToI2Size(),
				(GameConfig.ScreenSize.ToD2Size() * 1.5).ToI2Size(),
				(GameConfig.ScreenSize.ToD2Size() * 1.6).ToI2Size(),
				(GameConfig.ScreenSize.ToD2Size() * 1.7).ToI2Size(),
				(GameConfig.ScreenSize.ToD2Size() * 1.8).ToI2Size(),
			};

			string[] items = sizes.Select(size => size.W + " x " + size.H)
				.Concat(new string[] { "フルスクリーン", "戻る" })
				.ToArray();

			items[0] += " (デフォルト)";

			SimpleMenu menu = new SimpleMenu(24, 40, 16, 420, "ウィンドウサイズの変更", items);

			for (; ; )
			{
				for (; ; )
				{
					DrawWall();

					if (menu.Draw())
						break;

					DD.EachFrame();
				}

				if (menu.SelectedIndex < sizes.Length)
				{
					I2Size size = sizes[menu.SelectedIndex];

					GameSetting.UserScreenSize.W = size.W;
					GameSetting.UserScreenSize.H = size.H;
					GameSetting.FullScreen = false;

					DD.SetRealScreenSize(size.W, size.H);
				}
				else if (menu.SelectedIndex == sizes.Length)
				{
					GameSetting.FullScreen = true;

					DD.SetRealScreenSize(DD.TargetMonitor.W, DD.TargetMonitor.H);
				}
				else if (menu.SelectedIndex == sizes.Length + 1)
				{
					break;
				}
				else
				{
					throw null; // never
				}
			}
		}

		private static void ChangeMusicVolume()
		{
			ChangeVolume(
				"ＢＧＭ音量",
				GameSetting.MusicVolume,
				GameConfig.DefaultMusicVolume,
				value => GameSetting.MusicVolume = value,
				() =>
				{
					// none
				}
				);
		}

		private static void ChangeSEVolume()
		{
			ChangeVolume(
				"ＳＥ音量",
				GameSetting.SEVolume,
				GameConfig.DefaultSEVolume,
				value => GameSetting.SEVolume = value,
				() =>
				{
					SCommon.CRandom.ChooseOne(TESTING_SOUND_EFFECTS).Play();
				}
				);
		}

		private static void ChangeVolume(string title, double volume, double defaultVolume, Action<double> volumeSetter, Action pulse)
		{
			ChangeVolumePercent(
				title,
				SCommon.ToInt(volume * 100.0),
				SCommon.ToInt(defaultVolume * 100.0),
				value => volumeSetter(value / 100.0),
				pulse
				);
		}

		private static void ChangeVolumePercent(string title, int volume, int defaultVolume, Action<int> volumeSetter, Action pulse)
		{
			const int PULSE_FRAME = 60;

			for (; ; )
			{
				if (Inputs.DECIDE.GetInput() == 1)
				{
					DD.FreezeGameUntil(() => Inputs.DECIDE.GetInput() == 0);
					break;
				}
				if (Inputs.CANCEL.GetInput() == 1)
				{
					if (volume == defaultVolume)
					{
						DD.FreezeGameUntil(() => Inputs.CANCEL.GetInput() == 0);
						break;
					}
					volume = defaultVolume;
				}
				if (Inputs.DIR_8.IsPound())
				{
					volume += 3;
				}
				if (Inputs.DIR_2.IsPound())
				{
					volume -= 3;
				}
				if (Inputs.DIR_4.IsPound())
				{
					volume--;
				}
				if (Inputs.DIR_6.IsPound())
				{
					volume++;
				}
				volume = SCommon.ToRange(volume, 0, 100);
				volumeSetter(volume);

				if (DD.ProcFrame % PULSE_FRAME == 0)
				{
					pulse();
				}

				// ここから描画

				DrawWall();
				DD.DrawCurtain(-0.5);

				DD.SetPrint(40, 40, 40, FONT_NAME, 30);
				DD.PrintLine(title);
				DD.SetPrint(40, 170, 40, FONT_NAME, 20);
				DD.PrintLine(GetMeterString(volume / 100.0, 80));
				DD.SetPrint(450, 320, 40, FONT_NAME, 20); ;
				DD.PrintLine("/// ＴＩＰＳ ///");
				DD.PrintLine("右・上キー　⇒　上げる");
				DD.PrintLine("左・下キー　⇒　下げる");
				DD.PrintLine("調整が終わったら決定ボタンを押して下さい。");
				DD.PrintLine("キャンセルボタンを押すと変更をキャンセルします。");

				DD.EachFrame();
			}
		}

		private static string GetMeterString(double volume, int length)
		{
			int indicator = SCommon.ToInt(volume * (length - 1));

			StringBuilder buff = new StringBuilder();

			buff.Append("[");

			for (int index = 0; index < length; index++)
				buff.Append(index == indicator ? "■" : "-");

			buff.Append("] ");
			buff.Append(volume.ToString("F2"));

			return buff.ToString();
		}

		private static void ChangeMouseEnabled()
		{
			SimpleMenu menu = new SimpleMenu(30, 40, 30, 770, "マウスの表示／非表示の切り替え", new string[]
			{
				"マウスカーソルをゲーム画面上に表示する",
				"マウスカーソルをゲーム画面上に表示しない",
				"戻る",
			});

			menu.SelectedIndex = GameSetting.MouseCursorShow ? 0 : 1;

			for (; ; )
			{
				DrawWall();

				if (menu.Draw())
					break;

				DD.EachFrame();
			}

			switch (menu.SelectedIndex)
			{
				case 0:
					GameSetting.MouseCursorShow = true;
					break;

				case 1:
					GameSetting.MouseCursorShow = false;
					break;

				case 2:
					break;

				default:
					throw null; // never
			}

			DX.SetMouseDispFlag(GameSetting.MouseCursorShow ? 1 : 0);
		}

		private static void FieldActiveButtonSetting()
		{
			SimpleMenu menu = new SimpleMenu(30, 40, 30, 770, "フィールド展開ボタンの設定", new string[]
			{
				"トグル式 (押すたびにオン・オフ切り替わる)",
				"ホールド式 (押し下げている間、オンになる)",
				"戻る",
			});

			menu.SelectedIndex = GameSetting.FieldActiveMode == GameSetting.FieldActiveMode_e.TOGGLE ? 0 : 1;

			for (; ; )
			{
				DrawWall();

				if (menu.Draw())
					break;

				DD.EachFrame();
			}

			switch (menu.SelectedIndex)
			{
				case 0:
					GameSetting.FieldActiveMode = GameSetting.FieldActiveMode_e.TOGGLE;
					break;

				case 1:
					GameSetting.FieldActiveMode = GameSetting.FieldActiveMode_e.HOLD;
					break;

				case 2:
					break;

				default:
					throw null; // never
			}
		}
	}
}
