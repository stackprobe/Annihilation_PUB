using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;

namespace HLTStudio.Games.Gameplays
{
	public class Background
	{
		public void Draw()
		{
			DD.SetBright(new D3Color(0, 0, 0.5));
			DD.Draw(Pictures.WhiteBox, new D4Rect(0.0, 0.0, GameConfig.ScreenSize.W, GameConfig.ScreenSize.H));

			DD.Draw(Pictures.TitleLogo2, new D2Point(
				630.0,
				110.0
				));

			DD.Draw(Pictures.Gemina_立ち絵, new D2Point(
				790.0,
				SCommon.ToInt(272.0 + 10.0 * Math.Sin(DD.ProcFrame * 0.03))
				));

			DD.SetPrint(480, 310, 30);
			DD.PrintLine($"スコア : [ {GEMain.I.Score} ]");
			DD.PrintLine($"学習度 : [ {GEMain.I.LearnedPermil} / 1000 ]");
			DD.PrintLine($"学習中 : [ {GEMain.I.LearningWeapon?.Name ?? "-"} ]");
			DD.PrintLine($"学習済 : [ {GEMain.I.LearnedWeapon?.Name ?? "-"} ]");
			DD.PrintLine($"ライフ : [ {string.Join("＝", Enumerable.Repeat("★", GEMain.I.PlayerLife))} ]");
			DD.PrintLine($"ボム残 : [ {string.Join("＝", Enumerable.Repeat("◎", GEMain.I.BombCount))} ]");
		}
	}
}
