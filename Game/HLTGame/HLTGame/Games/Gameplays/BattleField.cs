using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;
using HLTStudio.Tools;

namespace HLTStudio.Games.Gameplays
{
	public class BattleField
	{
		private const int RECT_L = 4;
		private const int RECT_T = 4;
		private const int RECT_W = 456;
		private const int RECT_H = 532;

		public static VScreen Screen = new VScreen(RECT_W, RECT_H);
		public static VScreen FreeScreen = new VScreen(RECT_W, RECT_H);

		public void DrawToMainScreen()
		{
			DD.Draw(Screen.GetPicture(), new D4Rect(RECT_L, RECT_T, RECT_W, RECT_H));
		}
	}
}
