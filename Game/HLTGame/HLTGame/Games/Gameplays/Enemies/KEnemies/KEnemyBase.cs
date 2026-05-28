using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;
using HLTStudio.Games.Gameplays.Enemies.Items;

namespace HLTStudio.Games.Gameplays.Enemies.KEnemies
{
	public abstract class KEnemyBase : EnemyBase
	{
		protected KEnemyBase(double x, double y, int hp)
		{
			{
				const double MARGIN = 300.0;

				if (
					x < -MARGIN || BattleField.Screen.W + MARGIN < x ||
					y < -MARGIN || BattleField.Screen.H + MARGIN < y
					)
					throw new Exception("出現位置がバトルフィールドから離れすぎています。");
			}

			if (hp < 0 || SCommon.IMAX < hp)
				throw new Exception("不正な体力(0～)");

			this.Initialize(x, y, hp);
		}

		protected override IEnumerable<bool> E_EachFrame()
		{
			var f_eachFrame2 = SCommon.Supplier(this.E_EachFrame2());
			var f_eachFrame3 = SCommon.Supplier(this.E_EachFrame_Expair());

			for (; ; )
			{
				if (!f_eachFrame2())
					break;

				if (!f_eachFrame3())
					break;

				if (this.Crashed && this.HP < 1)
				{
					GEMain.I.EnemyController.Add(new OrdinaryItem(this.X, this.Y));
					DD.TL.Add(SCommon.Supplier(this.KilledEffect()));
					SoundEffects.EnemyKilled.Play();
					break;
				}
				yield return true;
			}
		}

		/// <summary>
		/// この敵キャラクタの大きさ(半径)を取得する。
		/// </summary>
		/// <returns>このキャラクタの半径</returns>
		protected abstract double GetCharacterRadius();

		/// <summary>
		/// この敵キャラクタの画像を取得する。
		/// </summary>
		/// <returns>この敵キャラクタの画像</returns>
		protected abstract APicture GetPicture();

		protected abstract IEnumerable<bool> E_EachFrame2();

		private IEnumerable<bool> E_EachFrame_Expair()
		{
			bool hasEnteredBF = false;
			int outOfBFFrame = 0;

			for (; ; )
			{
				bool outOfBF = DD.IsOutOf(
					new D4Rect(0, 0, BattleField.Screen.W, BattleField.Screen.H),
					new D2Point(this.X, this.Y)
					);

				if (outOfBF)
					outOfBFFrame++;
				else
					outOfBFFrame = 0;

				if (outOfBFFrame > (hasEnteredBF ? 10 : 60))
					break;

				yield return true;
			}
		}

		private IEnumerable<bool> KilledEffect()
		{
			foreach (AScene scene in AScene.Create(5))
			{
				double size = 500 - 200 * scene.Rate;

				DD.SetAlpha(0.5);
				DD.SetBright(new D3Color(1.0, 1.0, 0.0));
				DD.SetSize(new D2Size(size, size));
				DD.Draw(Pictures.WhiteCircle, new D2Point(this.X, this.Y));

				yield return true;
			}
		}
	}
}
