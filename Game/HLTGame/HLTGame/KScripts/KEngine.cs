using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HLTStudio.KScripts
{
	public static class KEngine
	{
		public static IEnumerable<bool> Run(KVariables variables, string functionName, string[] arguments)
		{
			foreach (var relay in RunFunction(variables, functionName, arguments))
				yield return relay;
		}

		private static IEnumerable<bool> RunFunction(KVariables variables, string functionName, string[] arguments)
		{
			KVariables localVariables = variables.CreateLocal();
			SetParameters(localVariables, variables, arguments);

			KCommand functionCommand = KScriptCache.GetFunctionCommand(functionName);

			foreach (var relay in RunCommands(localVariables, functionCommand.UnderCommands))
				yield return relay;
		}

		private static void SetParameters(KVariables localVariables, KVariables callerVariables, string[] arguments)
		{
			for (int index = 0; index < arguments.Length; index++)
				localVariables.SetValue($"P{index + 1}", Calculate(callerVariables, arguments[index]));
		}

		private static IEnumerable<bool> RunCommands(KVariables variables, KCommand[] commands)
		{
			foreach (KCommand command in commands)
			{
				foreach (var relay in RunCommand(variables, command))
					yield return relay;
			}
		}

		private static IEnumerable<bool> RunCommand(KVariables variables, KCommand command)
		{
			switch (command.Kind)
			{
				case KCommand.Kind_e.ASSIGNMENT:
					{
						KCalc.Run(variables, command.Arguments);
					}
					break;

				case KCommand.Kind_e.COMMAND:
					{
						foreach (var relay in RunCommandOrFunction(variables, command.Name, command.Arguments))
							yield return relay;
					}
					break;

				case KCommand.Kind_e.IF:
					{
						if (CalculateCondition(variables, command.Arguments))
							foreach (var relay in RunCommands(variables, command.UnderCommands))
								yield return relay;
					}
					break;

				case KCommand.Kind_e.LOOP:
					{
						while (CalculateCondition(variables, command.Arguments))
							foreach (var relay in RunCommands(variables, command.UnderCommands))
								yield return relay;
					}
					break;

				default:
					throw null; // never
			}
		}

		private static IEnumerable<bool> RunCommandOrFunction(KVariables variables, string name, string[] arguments)
		{
			if (KPrimitives.IsPrimitiveCommand(name))
			{
				foreach (var relay in KPrimitives.Run(variables, name, arguments))
					yield return relay;
			}
			else
			{
				foreach (var relay in RunFunction(variables, name, arguments))
					yield return relay;
			}
		}

		private static bool CalculateCondition(KVariables variables, string[] arguments)
		{
			for (int index = 0; index < arguments.Length; index++)
			{
				string token = arguments[index];

				if (IsComparisonOperator(token))
				{
					double left = Calculate(variables, CopyRange(arguments, 0, index));
					double right = Calculate(variables, CopyRange(arguments, index + 1, arguments.Length - index - 1));

					if (token == "==")
						return left == right;
					if (token == "!=")
						return left != right;
					if (token == "<")
						return left < right;
					if (token == ">")
						return left > right;
					if (token == "<=")
						return left <= right;
					if (token == ">=")
						return left >= right;
				}
			}
			return Calculate(variables, arguments) != 0.0;
		}

		private static bool IsComparisonOperator(string token)
		{
			return
				token == "==" ||
				token == "!=" ||
				token == "<" ||
				token == ">" ||
				token == "<=" ||
				token == ">=";
		}

		private static string[] CopyRange(string[] arguments, int start, int length)
		{
			string[] ret = new string[length];

			Array.Copy(arguments, start, ret, 0, length);

			return ret;
		}

		private static double Calculate(KVariables variables, string argument)
		{
			return KCalc.Internal_Calculate(variables, new string[] { argument });
		}

		private static double Calculate(KVariables variables, string[] arguments)
		{
			return KCalc.Internal_Calculate(variables, arguments);
		}
	}
}
