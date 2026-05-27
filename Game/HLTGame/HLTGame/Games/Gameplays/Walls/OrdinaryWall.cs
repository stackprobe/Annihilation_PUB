using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;

namespace HLTStudio.Games.Gameplays.Walls
{
	public class OrdinaryWall : WallBase
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			for (int frame = 0; ; frame++)
			{
				DD.SetBright(new D3Color(0, 0, 0));
				DD.Draw(Pictures.WhiteBox, new D4Rect(0, 0, BattleField.Screen.W, BattleField.Screen.H));

				using (_3DScreen.Section())
				{
					DX.ClearDrawScreen();

					/*
					DD.SetAlpha(0.9);
					DD.SetBright(new D3Color(0, 0, 0));
					DD.Draw(Pictures.WhiteBox, new D4Rect(0, 0, _3DScreen.W, _3DScreen.H));
					*/

					DrawBackGround3D(frame);
				}

				// 縮小して本画面へ描画
				DX.DrawExtendGraph(
					0, 0, _3D_SCREEN_W, _3D_SCREEN_H,
					_3DScreen.GetHandle(),
					DX.FALSE
					);

				yield return true;
			}
		}

		private static int _3D_SCREEN_W => BattleField.Screen.W;
		private static int _3D_SCREEN_H => BattleField.Screen.H;

		private const int _3D_RESOLUTION_SCALE = 2;

		private static VScreen _3DScreen = new VScreen(
			_3D_SCREEN_W * _3D_RESOLUTION_SCALE,
			_3D_SCREEN_H * _3D_RESOLUTION_SCALE
			);

		private static void DrawBackGround3D(int frame)
		{
			// 3D描画設定
			DX.SetUseZBuffer3D(DX.TRUE);
			DX.SetWriteZBuffer3D(DX.TRUE);
			DX.SetUseLighting(DX.FALSE);

			// カメラ
			// 注視点を少し上にすることで「画面上方へ進む」感じにする
			DX.SetCameraPositionAndTarget_UpVecY(
				DX.VGet(0.0f, 230.0f, -100.0f), // カメラ位置
				DX.VGet(0.0f, -330.0f, 800.0f)  // 注視点
				);

			uint lineColor = DX.GetColor(0, 160, 255);

			float scroll = frame * 12.0f;

			const float GRID_W = 700.0f;
			const float GRID_Z_MAX = 5000.0f;
			const float GRID_STEP = 200.0f;

			// 横線：奥から手前に流れてくる
			for (int i = 0; i < 40; i++)
			{
				float z = (i * GRID_STEP + scroll) % GRID_Z_MAX;
				z = GRID_Z_MAX - z;

				DX.DrawLine3D(
					DX.VGet(-GRID_W, 0.0f, z),
					DX.VGet(GRID_W, 0.0f, z),
					lineColor
					);
			}

			// 縦線：奥行き方向
			for (int i = -7; i <= 7; i++)
			{
				float x = i * 100.0f;

				DX.DrawLine3D(
					DX.VGet(x, 0.0f, 0.0f),
					DX.VGet(x, 0.0f, GRID_Z_MAX),
					lineColor
					);
			}

			// 遠くに流れる点・デジタル粒子
			for (int i = 0; i < 80; i++)
			{
				float x = ((i * 97) % 1200) - 600.0f;
				float y = -80.0f - ((i * 53) % 300);
				float z = ((i * 211) + scroll * 2.0f) % GRID_Z_MAX;
				z = GRID_Z_MAX - z;

				DX.DrawSphere3D(
					DX.VGet(x, y, z),
					4.0f,
					8,
					DX.GetColor(80, 220, 255),
					DX.GetColor(80, 220, 255),
					DX.FALSE
					);
			}

			// 2D描画に戻す
			DX.SetUseZBuffer3D(DX.FALSE);
			DX.SetWriteZBuffer3D(DX.FALSE);

			//frame++;
		}
	}
}
