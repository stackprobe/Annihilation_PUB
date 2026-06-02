using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using HLTStudio.Games.Gameplays.Enemies.KEnemies;
using HLTStudio.Commons;
using HLTStudio.Games.Gameplays;

namespace HLTStudio.KScripts
{
	public class KVariables
	{
		private const string VAR_NAME_X = "X";
		private const string VAR_NAME_Y = "Y";
		private const string VAR_NAME_HP = "HP";

		public KEnemy_ScriptRunner Enemy;

		private Dictionary<string, double> Variables = new Dictionary<string, double>();

		public KVariables(KEnemy_ScriptRunner enemy)
		{
			this.Enemy = enemy;
		}

		public KVariables CreateLocal()
		{
			return new KVariables(this.Enemy);
		}

		public double GetValue(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw null; // never

			if (name == VAR_NAME_X)
				return this.Enemy.X;

			if (name == VAR_NAME_Y)
				return this.Enemy.Y;

			if (name == VAR_NAME_HP)
				return (double)this.Enemy.HP;

			if (this.Variables.ContainsKey(name))
				return this.Variables[name];

			return 0.0;
		}

		public void SetValue(string name, double value)
		{
			if (string.IsNullOrEmpty(name))
				throw null; // never

			if (name == VAR_NAME_X)
			{
				double x = value;

				if (x < -KEnemyConsts.BATTLE_FIELD_EXTENT || BattleField.Screen.W + KEnemyConsts.BATTLE_FIELD_EXTENT < x)
					throw new Exception("不正な位置(X)が設定されました。" + x);

				this.Enemy.X = x;
			}
			else if (name == VAR_NAME_Y)
			{
				double y = value;

				if (y < -KEnemyConsts.BATTLE_FIELD_EXTENT || BattleField.Screen.H + KEnemyConsts.BATTLE_FIELD_EXTENT < y)
					throw new Exception("不正な位置(Y)が設定されました。" + y);

				this.Enemy.Y = y;
			}
			else if (name == VAR_NAME_HP)
			{
				int hp = (int)value;

				if (hp < 1 || SCommon.IMAX < hp)
					throw new Exception("不正な体力が設定されました。" + hp);

				this.Enemy.HP = hp;
			}
			else if (this.Variables.ContainsKey(name))
			{
				this.Variables[name] = value;
			}
			else
			{
				this.Variables.Add(name, value);
			}
		}
	}
}
