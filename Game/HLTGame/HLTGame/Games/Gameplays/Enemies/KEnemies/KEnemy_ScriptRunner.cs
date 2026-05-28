using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;
using HLTStudio.KScripts;

namespace HLTStudio.Games.Gameplays.Enemies.KEnemies
{
	public class KEnemy_ScriptRunner : KEnemyBase
	{
		public double CharacterRadius = 30.0;
		public APicture Picture = Pictures.Transparent;

		private Func<bool> F_Script;

		public KEnemy_ScriptRunner(string scriptName, double x, double y, params string[] scriptParameters)
			: base(x, y, 1)
		{
			this.F_Script = SCommon.Supplier(KEngine.Run(new KVariables(this), scriptName, scriptParameters));

			// 最初の1フレーム分をここで実行する。
			// -- 敵の状態の初期化をここで完了させるため。
			this.F_Script();
		}

		protected override IEnumerable<bool> E_EachFrame2()
		{
			for (; ; )
			{
				if (!this.F_Script())
					break;

				DD.Draw(this.GetPicture(), new D2Point(this.X, this.Y));

				this.Crash = ACrash.CreateCircle(new D2Point(this.X, this.Y), this.CharacterRadius);

				yield return true;
			}
		}

		protected override double GetCharacterRadius()
		{
			return this.CharacterRadius;
		}

		protected override APicture GetPicture()
		{
			return this.Picture;
		}
	}
}
