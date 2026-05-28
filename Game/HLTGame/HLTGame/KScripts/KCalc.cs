using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HLTStudio.KScripts
{
	public static class KCalc
	{
		private const string OPERATOR_ASSIGNMENT = "=";
		private const string OPERATOR_ADD = "+";
		private const string OPERATOR_SUBTRACT = "-";
		private const string OPERATOR_MULTIPLY = "*";
		private const string OPERATOR_DIVIDE = "/";
		private const string OPERATOR_MODULO = "%";
		private const string TOKEN_OPEN_PARENTHESIS = "(";
		private const string TOKEN_CLOSE_PARENTHESIS = ")";

		public static void Run(KVariables variables, string[] arguments)
		{
			if (arguments.Length < 3)
				throw new Exception("計算式が短すぎます。");

			if (arguments[1] != OPERATOR_ASSIGNMENT)
				throw new Exception($"計算式の2番目のトークンは [[ {OPERATOR_ASSIGNMENT} ]] である必要があります。");

			double value = Calculate(variables, arguments, 2);

			variables.SetValue(arguments[0], value);
		}

		public static double Calculate(KVariables variables, string[] arguments)
		{
			return Calculate(variables, arguments, 0);
		}

		public static double Calculate(KVariables variables, string[] arguments, int startIndex)
		{
			int index = startIndex;
			double value = ParseExpression(variables, arguments, ref index);

			if (index != arguments.Length)
				throw new Exception("計算式に余分なトークンがあります。");

			return value;
		}

		private static double ParseExpression(KVariables variables, string[] arguments, ref int index)
		{
			double value = ParseTerm(variables, arguments, ref index);

			while (index < arguments.Length)
			{
				string token = arguments[index];

				if (token == OPERATOR_ADD)
				{
					index++;
					value += ParseTerm(variables, arguments, ref index);
				}
				else if (token == OPERATOR_SUBTRACT)
				{
					index++;
					value -= ParseTerm(variables, arguments, ref index);
				}
				else
				{
					break;
				}
			}
			return value;
		}

		private static double ParseTerm(KVariables variables, string[] arguments, ref int index)
		{
			double value = ParseFactor(variables, arguments, ref index);

			while (index < arguments.Length)
			{
				string token = arguments[index];

				if (token == OPERATOR_MULTIPLY)
				{
					index++;
					value *= ParseFactor(variables, arguments, ref index);
				}
				else if (token == OPERATOR_DIVIDE)
				{
					index++;
					value /= ParseFactor(variables, arguments, ref index);
				}
				else if (token == OPERATOR_MODULO)
				{
					index++;
					value %= ParseFactor(variables, arguments, ref index);
				}
				else
				{
					break;
				}
			}
			return value;
		}

		private static double ParseFactor(KVariables variables, string[] arguments, ref int index)
		{
			if (arguments.Length <= index)
				throw new Exception("計算式が途中で終了しました。");

			string token = arguments[index];
			index++;

			if (token == TOKEN_OPEN_PARENTHESIS)
			{
				double value = ParseExpression(variables, arguments, ref index);

				if (arguments.Length <= index || arguments[index] != TOKEN_CLOSE_PARENTHESIS)
					throw new Exception($"対応する [[ {TOKEN_CLOSE_PARENTHESIS} ]] がありません。");

				index++;
				return value;
			}

			if (
				token == OPERATOR_ADD ||
				token == OPERATOR_SUBTRACT ||
				token == OPERATOR_MULTIPLY ||
				token == OPERATOR_DIVIDE ||
				token == OPERATOR_MODULO ||
				token == TOKEN_CLOSE_PARENTHESIS
				)
				throw new Exception("計算式の値が必要な位置に演算子があります。");

			double number;

			if (double.TryParse(token, out number))
				return double.Parse(token);

			return variables.GetValue(token);
		}
	}
}
