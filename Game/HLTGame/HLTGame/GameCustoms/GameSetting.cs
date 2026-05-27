using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;

namespace HLTStudio.GameCustoms
{
	public static class GameSetting
	{
		public static I2Size UserScreenSize;
		public static bool FullScreen = false;
		public static bool MouseCursorShow = true;
		public static double MusicVolume;
		public static double SEVolume;

		public enum FieldActiveMode_e
		{
			TOGGLE = 1,
			HOLD,
		}

		public static FieldActiveMode_e FieldActiveMode = FieldActiveMode_e.TOGGLE;

		public static void Initialize()
		{
			UserScreenSize = GameConfig.ScreenSize;
			MusicVolume = GameConfig.DefaultMusicVolume;
			SEVolume = GameConfig.DefaultSEVolume;
		}

		#region Serialize / Deserialize

		public static byte[] Serialize()
		{
			List<object> dest = new List<object>();

			// ---- 保存データここから ----

			dest.Add(UserScreenSize.W);
			dest.Add(UserScreenSize.H);
			dest.Add(FullScreen);
			dest.Add(MouseCursorShow);
			dest.Add(DD.RateToPPB(MusicVolume));
			dest.Add(DD.RateToPPB(SEVolume));

			foreach (AInput input in DD.GetAllInput(true))
				dest.Add(input.Serialize());

			dest.Add((int)FieldActiveMode);

			// ---- 保存データここまで ----

			return SCommon.Serializer.I.BinJoin(dest.Select(v => v.ToString()).ToArray());
		}

		public static void Deserialize(byte[] serializedBytes)
		{
			string[] src = SCommon.Serializer.I.Split(serializedBytes);
			int c = 0;

			// ---- 保存データここから ----

			UserScreenSize.W = SCommon.ToRange(int.Parse(src[c++]), 1, SCommon.IMAX);
			UserScreenSize.H = SCommon.ToRange(int.Parse(src[c++]), 1, SCommon.IMAX);
			FullScreen = bool.Parse(src[c++]);
			MouseCursorShow = bool.Parse(src[c++]);
			MusicVolume = DD.PPBToRate(int.Parse(src[c++]));
			SEVolume = DD.PPBToRate(int.Parse(src[c++]));

			foreach (AInput input in DD.GetAllInput(true))
				input.Deserialize(src[c++]);

			FieldActiveMode = (FieldActiveMode_e)int.Parse(src[c++]);

			// ---- 保存データここまで ----

			src = null;
			c = default;
		}

		#endregion
	}
}
