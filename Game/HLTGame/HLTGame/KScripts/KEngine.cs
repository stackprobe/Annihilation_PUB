using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HLTStudio.KScripts
{
	public static class KEngine
	{
		private const string COMPARISON_EQUAL = "==";
		private const string COMPARISON_NOT_EQUAL = "!=";
		private const string COMPARISON_LESS_THAN = "<";
		private const string COMPARISON_GREATER_THAN = ">";
		private const string COMPARISON_LESS_THAN_OR_EQUAL = "<=";
		private const string COMPARISON_GREATER_THAN_OR_EQUAL = ">=";

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
			var primitive = KPrimitives.Run(variables, name, arguments);

			if (primitive != null)
			{
				foreach (var relay in primitive)
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
			if (arguments.Length == 3)
			{
				double value1 = Calculate(variables, arguments[0]);
				string strOperator = arguments[1];
				double value2 = Calculate(variables, arguments[2]);

				if (strOperator == COMPARISON_EQUAL)
					return value1 == value2;

				if (strOperator == COMPARISON_NOT_EQUAL)
					return value1 != value2;

				if (strOperator == COMPARISON_LESS_THAN)
					return value1 < value2;

				if (strOperator == COMPARISON_GREATER_THAN)
					return value1 > value2;

				if (strOperator == COMPARISON_LESS_THAN_OR_EQUAL)
					return value1 <= value2;

				if (strOperator == COMPARISON_GREATER_THAN_OR_EQUAL)
					return value1 >= value2;
			}
			return Calculate(variables, arguments) != 0.0;
		}

		private static double Calculate(KVariables variables, string argument)
		{
			return KCalc.Calculate(variables, new string[] { argument });
		}

		private static double Calculate(KVariables variables, string[] arguments)
		{
			return KCalc.Calculate(variables, arguments);
		}
	}
}
