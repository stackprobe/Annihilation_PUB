using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;

namespace HLTStudio.Games.Gameplays.Weapons
{
	public static class WeaponCommon
	{
		/// <summary>
		/// 自弾の発射位置を取得する。
		/// </summary>
		/// <returns>自弾の発射位置</returns>
		public static D2Point GetShotStartPosition()
		{
			double x = GEMain.I.PlayerActor.X;
			double y = GEMain.I.PlayerActor.Y;

			x += 0.0;
			y -= 20.0;

			return new D2Point(x, y);
		}
	}
}
