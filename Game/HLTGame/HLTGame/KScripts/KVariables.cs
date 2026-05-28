using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using HLTStudio.Games.Gameplays.Enemies.KEnemies;

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
				this.Enemy.X = value;
			else if (name == VAR_NAME_Y)
				this.Enemy.Y = value;
			else if (name == VAR_NAME_HP)
				this.Enemy.HP = (int)value;
			else if (this.Variables.ContainsKey(name))
				this.Variables[name] = value;
			else
				this.Variables.Add(name, value);
		}
	}
}
