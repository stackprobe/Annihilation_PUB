using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using HLTStudio.Drawings;

namespace HLTStudio.GameCustoms
{
	public static class GameConfig
	{
		public static string GameTitle = "LEARN & ANNIHILATE";
		public static string Version = "1.00";

		public static I2Size ScreenSize = new I2Size(960, 540);

		public static double DefaultMusicVolume = 0.45;
		public static double DefaultSEVolume = 0.45;

		public static Color LibbonBackColor = Color.FromArgb(96, 24, 48);
		public static Color LibbonForeColor = Color.White;
	}
}
