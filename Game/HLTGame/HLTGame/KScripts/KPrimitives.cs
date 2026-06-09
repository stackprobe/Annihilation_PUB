using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HLTStudio.GameCommons;
using HLTStudio.Games.Gameplays;
using HLTStudio.Games.Gameplays.Enemies.KEnemies;
using HLTStudio.Drawings;

namespace HLTStudio.KScripts
{
	public static class KPrimitives
	{
		public static IEnumerable<bool> Run(KVariables variables, string name, string[] arguments)
		{
			if (name == "発射")
			{
				return 発射(variables, arguments);
			}
			else if (name == "生成")
			{
				return 生成(variables, arguments);
			}
			else if (name == "アプローチ相対")
			{
				return アプローチ相対(variables, arguments);
			}
			else if (name == "アプローチ絶対")
			{
				return アプローチ絶対(variables, arguments);
			}
			else if (name == "移動相対")
			{
				return 移動相対(variables, arguments);
			}
			else if (name == "移動絶対")
			{
				return 移動絶対(variables, arguments);
			}
			else if (name == "移動角度")
			{
				return 移動角度(variables, arguments);
			}
			else if (name == "待つ")
			{
				return 待つ(variables, arguments);
			}
			else if (name == "画像")
			{
				return 画像(variables, arguments);
			}
			else if (name == "半径")
			{
				return 半径(variables, arguments);
			}
			else if (name == "矩形")
			{
				return 矩形(variables, arguments);
			}
			else if (name == "これは敵弾")
			{
				return これは敵弾(variables, arguments);
			}
			else if (name == "自機の角度")
			{
				return 自機の角度(variables, arguments);
			}
			else
			{
				return null;
			}
		}

		private static IEnumerable<bool> 発射(KVariables variables, string[] arguments)
		{
			GEMain.I.EnemyController.Add(new KEnemy_ScriptRunner(
				arguments[0],
				variables.Enemy.X,
				variables.Enemy.Y,
				SolveVarNames(variables, arguments, 1)
				));

			yield break;
		}

		private static IEnumerable<bool> 生成(KVariables variables, string[] arguments)
		{
			GEMain.I.EnemyController.Add(new KEnemy_ScriptRunner(
				arguments[0],
				Calculate(variables, arguments[1]),
				Calculate(variables, arguments[2]),
				SolveVarNames(variables, arguments, 3)
				));

			yield break;
		}

		private static string[] SolveVarNames(KVariables variables, string[] arguments, int startIndex)
		{
			return arguments.Skip(startIndex).Select(argument => Calculate(variables, argument).ToString("F9")).ToArray();
		}

		private static IEnumerable<bool> アプローチ相対(KVariables variables, string[] arguments)
		{
			double startX = variables.GetValue("X");
			double startY = variables.GetValue("Y");
			double targetX = startX + Calculate(variables, arguments[0]);
			double targetY = startY + Calculate(variables, arguments[1]);
			int frameMax = CalculateFrame(variables, arguments[2]);
			double rate = Calculate(variables, arguments[3]);

			foreach (var relay in ApproachTo(variables, startX, startY, targetX, targetY, frameMax, rate))
				yield return relay;
		}

		private static IEnumerable<bool> アプローチ絶対(KVariables variables, string[] arguments)
		{
			double startX = variables.GetValue("X");
			double startY = variables.GetValue("Y");
			double targetX = Calculate(variables, arguments[0]);
			double targetY = Calculate(variables, arguments[1]);
			int frameMax = CalculateFrame(variables, arguments[2]);
			double rate = Calculate(variables, arguments[3]);

			foreach (var relay in ApproachTo(variables, startX, startY, targetX, targetY, frameMax, rate))
				yield return relay;
		}

		private static IEnumerable<bool> 移動相対(KVariables variables, string[] arguments)
		{
			double startX = variables.GetValue("X");
			double startY = variables.GetValue("Y");
			double targetX = startX + Calculate(variables, arguments[0]);
			double targetY = startY + Calculate(variables, arguments[1]);
			int frameMax = CalculateFrame(variables, arguments[2]);

			foreach (var relay in MoveTo(variables, startX, startY, targetX, targetY, frameMax))
				yield return relay;
		}

		private static IEnumerable<bool> 移動絶対(KVariables variables, string[] arguments)
		{
			double startX = variables.GetValue("X");
			double startY = variables.GetValue("Y");
			double targetX = Calculate(variables, arguments[0]);
			double targetY = Calculate(variables, arguments[1]);
			int frameMax = CalculateFrame(variables, arguments[2]);

			foreach (var relay in MoveTo(variables, startX, startY, targetX, targetY, frameMax))
				yield return relay;
		}

		private static IEnumerable<bool> 移動角度(KVariables variables, string[] arguments)
		{
			double startX = variables.GetValue("X");
			double startY = variables.GetValue("Y");
			double angle = Calculate(variables, arguments[0]) * Math.PI / 180.0;
			double distance = Calculate(variables, arguments[1]);
			double targetX = startX + Math.Cos(angle) * distance;
			double targetY = startY + Math.Sin(angle) * distance;
			int frameMax = CalculateFrame(variables, arguments[2]);

			foreach (var relay in MoveTo(variables, startX, startY, targetX, targetY, frameMax))
				yield return relay;
		}

		private static IEnumerable<bool> 待つ(KVariables variables, string[] arguments)
		{
			int frameMax = CalculateFrame(variables, arguments[0]);

			for (int frame = 0; frame < frameMax; frame++)
				yield return true;
		}

		private static IEnumerable<bool> 画像(KVariables variables, string[] arguments)
		{
			variables.Enemy.Picture = KScriptPictures.GetPicture(arguments[0]);

			yield break;
		}

		private static IEnumerable<bool> 半径(KVariables variables, string[] arguments)
		{
			double r = Calculate(variables, arguments[0]);
			variables.Enemy.CrashGetter = pt => ACrash.CreateCircle(pt, r);

			yield break;
		}

		private static IEnumerable<bool> 矩形(KVariables variables, string[] arguments)
		{
			double w = Calculate(variables, arguments[0]);
			double h = Calculate(variables, arguments[1]);
			variables.Enemy.CrashGetter = pt => ACrash.CreateRect(new D4Rect(
				pt.X - w / 2,
				pt.Y - h / 2,
				w,
				h
				));

			yield break;
		}

		private static IEnumerable<bool> これは敵弾(KVariables variables, string[] arguments)
		{
			variables.Enemy.HP = 0;

			yield break;
		}

		private static IEnumerable<bool> 自機の角度(KVariables variables, string[] arguments)
		{
			string outVarName = arguments[0];

			double angle = DD.GetAngle(
				new D2Point(
					GEMain.I.PlayerActor.X,
					GEMain.I.PlayerActor.Y
					),
				new D2Point(
					variables.GetValue("X"),
					variables.GetValue("Y")
					)
				);

			double degree = angle * 180.0 / Math.PI;

			variables.SetValue(outVarName, degree);

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

		private static IEnumerable<bool> ApproachTo(KVariables variables, double startX, double startY, double targetX, double targetY, int frameMax, double rate)
		{
			if (frameMax <= 0)
			{
				variables.SetValue("X", targetX);
				variables.SetValue("Y", targetY);
				yield break;
			}

			double x = startX;
			double y = startY;

			for (int frame = 1; frame <= frameMax; frame++)
			{
				DD.Approach(ref x, targetX, rate);
				DD.Approach(ref y, targetY, rate);

				variables.SetValue("X", x);
				variables.SetValue("Y", y);

				yield return true;
			}
		}
	}
}
