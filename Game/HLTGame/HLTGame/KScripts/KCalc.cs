using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HLTStudio.KScripts
{
	public static class KCalc
	{
		public static void Run(KVariables variables, string[] arguments)
		{
			if (arguments.Length < 3)
				throw new Exception("計算式が短すぎます。");

			if (arguments[1] != "=")
				throw new Exception("計算式の2番目のトークンは [[ = ]] である必要があります。");

			double value = Internal_Calculate(variables, arguments, 2);

			variables.SetValue(arguments[0], value);
		}

		public static double Internal_Calculate(KVariables variables, string[] arguments)
		{
			return Internal_Calculate(variables, arguments, 0);
		}

		public static double Internal_Calculate(KVariables variables, string[] arguments, int startIndex)
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

				if (token == "+")
				{
					index++;
					value += ParseTerm(variables, arguments, ref index);
				}
				else if (token == "-")
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

				if (token == "*")
				{
					index++;
					value *= ParseFactor(variables, arguments, ref index);
				}
				else if (token == "/")
				{
					index++;
					value /= ParseFactor(variables, arguments, ref index);
				}
				else if (token == "%")
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

			if (token == "(")
			{
				double value = ParseExpression(variables, arguments, ref index);

				if (arguments.Length <= index || arguments[index] != ")")
					throw new Exception("対応する [[ ) ]] がありません。");

				index++;
				return value;
			}

			if (token == "+" || token == "-" || token == "*" || token == "/" || token == "%" || token == ")")
				throw new Exception("計算式の値が必要な位置に演算子があります。");

			double number;

			if (double.TryParse(token, out number))
				return double.Parse(token);

			return variables.GetValue(token);
		}
	}
}
