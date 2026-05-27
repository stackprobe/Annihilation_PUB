using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HLTStudio.Commons;

namespace HLTStudio
{
	public static class Consts
	{
		public static string KEYS_DIR
		{
			get
			{
				string dir = Path.Combine(ProcMain.SelfDir, "Keys");

				if (!Directory.Exists(dir))
				{
					dir = SCommon.MakeFullPath(@"..\..\..\..\..\Keys");

					if (!Directory.Exists(dir))
						throw new Exception("no KEYS_DIR");
				}
				return dir;
			}
		}

		public static string LAST_REVISION_CODE_FILE => Path.Combine(ProcMain.SelfDir, "LastRevisionCode.txt");

		public static string GAME_DIR => Path.Combine(ProcMain.SelfDir, "Game");
		public static string GAME_PROGRAM => Path.Combine(GAME_DIR, "Game.exe");

		public static string URL_BASE => "http://chocomintice.ccsp.mydns.jp/HPStore/Annihilation";
		public static string URL_GAME => $"{URL_BASE}/Game.cmp-gz.enc";
		public static string URL_REVISION_CODE => $"{URL_BASE}/RevisionCode.txt";
	}
}
