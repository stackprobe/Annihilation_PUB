using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HLTStudio.GameCommons;
using HLTStudio.Games.Gameplays;
using HLTStudio.Games.Gameplays.Enemies.KEnemies;

namespace HLTStudio.KScripts
{
	public static class KPrimitives
	{
		private const string COMMAND_FIRE = "発射";
		private const string COMMAND_MOVE_RELATIVE = "移動相対";
		private const string COMMAND_MOVE_ABSOLUTE = "移動絶対";
		private const string COMMAND_MOVE_ANGLE = "移動角度";
		private const string COMMAND_WAIT = "待つ";
		private const string COMMAND_PICTURE = "画像";
		private const string COMMAND_RADIUS = "半径";
		private const string COMMAND_THIS_IS_BULLET = "これは敵弾";

		public static bool IsPrimitiveCommand(string name)
		{
			return
				name == COMMAND_FIRE ||
				name == COMMAND_MOVE_RELATIVE ||
				name == COMMAND_MOVE_ABSOLUTE ||
				name == COMMAND_MOVE_ANGLE ||
				name == COMMAND_WAIT ||
				name == COMMAND_PICTURE ||
				name == COMMAND_RADIUS ||
				name == COMMAND_THIS_IS_BULLET;
		}

		public static IEnumerable<bool> Run(KVariables variables, string name, string[] arguments)
		{
			if (name == COMMAND_FIRE)
			{
				foreach (var relay in 発射(variables, arguments))
					yield return relay;
			}
			else if (name == COMMAND_MOVE_RELATIVE)
			{
				foreach (var relay in 移動相対(variables, arguments))
					yield return relay;
			}
			else if (name == COMMAND_MOVE_ABSOLUTE)
			{
				foreach (var relay in 移動絶対(variables, arguments))
					yield return relay;
			}
			else if (name == COMMAND_MOVE_ANGLE)
			{
				foreach (var relay in 移動角度(variables, arguments))
					yield return relay;
			}
			else if (name == COMMAND_WAIT)
			{
				foreach (var relay in 待つ(variables, arguments))
					yield return relay;
			}
			else if (name == COMMAND_PICTURE)
			{
				foreach (var relay in 画像(variables, arguments))
					yield return relay;
			}
			else if (name == COMMAND_RADIUS)
			{
				foreach (var relay in 半径(variables, arguments))
					yield return relay;
			}
			else if (name == COMMAND_THIS_IS_BULLET)
			{
				foreach (var relay in これは敵弾(variables, arguments))
					yield return relay;
			}
			else
			{
				throw null; // never
			}
		}

		public static IEnumerable<bool> 発射(KVariables variables, string[] arguments)
		{
			GEMain.I.EnemyController.Add(new KEnemy_ScriptRunner(
				arguments[0],
				variables.Enemy.X,
				variables.Enemy.Y,
				SolveVarNames(variables, arguments, 1)
				));

			yield break;
		}

		private static string[] SolveVarNames(KVariables variables, string[] arguments, int startIndex)
		{
			return arguments.Skip(startIndex).Select(argument => Calculate(variables, argument).ToString("F9")).ToArray();
		}

		public static IEnumerable<bool> 移動相対(KVariables variables, string[] arguments)
		{
			double startX = variables.GetValue("X");
			double startY = variables.GetValue("Y");
			double targetX = startX + Calculate(variables, arguments[0]);
			double targetY = startY + Calculate(variables, arguments[1]);
			int frameMax = CalculateFrame(variables, arguments[2]);

			foreach (var relay in MoveTo(variables, startX, startY, targetX, targetY, frameMax))
				yield return relay;
		}

		public static IEnumerable<bool> 移動絶対(KVariables variables, string[] arguments)
		{
			double startX = variables.GetValue("X");
			double startY = variables.GetValue("Y");
			double targetX = Calculate(variables, arguments[0]);
			double targetY = Calculate(variables, arguments[1]);
			int frameMax = CalculateFrame(variables, arguments[2]);

			foreach (var relay in MoveTo(variables, startX, startY, targetX, targetY, frameMax))
				yield return relay;
		}

		public static IEnumerable<bool> 移動角度(KVariables variables, string[] arguments)
		{
			double startX = variables.GetValue("X");
			double startY = variables.GetValue("Y");
			double angle = Calculate(variables, arguments[0]) * Math.PI / 180.0;
			double distance = Calculate(variables, arguments[1]);
			double targetX = startX + Math.Cos(angle) * distance;
			double targetY = startY - Math.Sin(angle) * distance;
			int frameMax = CalculateFrame(variables, arguments[2]);

			foreach (var relay in MoveTo(variables, startX, startY, targetX, targetY, frameMax))
				yield return relay;
		}

		public static IEnumerable<bool> 待つ(KVariables variables, string[] arguments)
		{
			int frameMax = CalculateFrame(variables, arguments[0]);

			for (int frame = 0; frame < frameMax; frame++)
				yield return true;
		}

		public static IEnumerable<bool> 画像(KVariables variables, string[] arguments)
		{
			variables.Enemy.Picture = KScriptPictures.GetPicture(arguments[0]);

			yield break;
		}

		public static IEnumerable<bool> 半径(KVariables variables, string[] arguments)
		{
			variables.Enemy.CharacterRadius = double.Parse(arguments[0]);

			yield break;
		}

		public static IEnumerable<bool> これは敵弾(KVariables variables, string[] arguments)
		{
			variables.Enemy.HP = 0;

			yield break;
		}

		private static double Calculate(KVariables variables, string argument)
		{
			return KCalc.Calculate(variables, new string[] { argument });
		}

		private static int CalculateFrame(KVariables variables, string argument)
		{
			return Math.Max(0, (int)Calculate(variables, argument));
		}

		private static IEnumerable<bool> MoveTo(KVariables variables, double startX, double startY, double targetX, double targetY, int frameMax)
		{
			if (frameMax <= 0)
			{
				variables.SetValue("X", targetX);
				variables.SetValue("Y", targetY);
				yield break;
			}

			for (int frame = 1; frame <= frameMax; frame++)
			{
				double rate = (double)frame / frameMax;

				variables.SetValue("X", startX + (targetX - startX) * rate);
				variables.SetValue("Y", startY + (targetY - startY) * rate);

				yield return true;
			}
		}
	}
}
