using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HLTStudio.Commons;

namespace HLTStudio.KScripts
{
	public static class KCommandParser
	{
		private const string PTN_ST_COMMENT = "#";
		private const string COMMAND_IF_BGN = "もし";
		private const string COMMAND_IF_END = "もし終了";
		private const string COMMAND_LP_BGN = "繰り返し";
		private const string COMMAND_LP_END = "繰り返し終了";

		public static KCommand Parse(string functionName, string[] lines)
		{
			KCommand functionCommand = KCommand.CreateFunction(functionName);

			int index = 0;
			functionCommand.UnderCommands = ParseUnderCommands(lines, ref index, null);

			if (index < lines.Length)
				throw new Exception("関数ファイルは終端に達する前に終了しました。");

			return functionCommand;
		}

		private static KCommand[] ParseUnderCommands(
			string[] lines,
			ref int index,
			string endWord
			)
		{
			List<KCommand> commands = new List<KCommand>();

			while (index < lines.Length)
			{
				string line = lines[index].Trim();
				index++;

				if (line.StartsWith(PTN_ST_COMMENT))
					continue;

				if (endWord != null && line == endWord)
					return commands.ToArray();

				string[] tokens = SCommon.Tokenize(line, "\t\u0020\u3000", false, true);

				if (tokens.Length == 0)
					continue;

				if (tokens[0] == COMMAND_IF_END)
					throw new Exception($"対応する [[ {COMMAND_IF_BGN} ]] がありません。行番号：{index}");

				if (tokens[0] == COMMAND_LP_END)
					throw new Exception($"対応する [[ {COMMAND_LP_BGN} ]] がありません。行番号：{index}");

				if (tokens[0] == COMMAND_IF_BGN)
				{
					string[] condition = tokens.Skip(1).ToArray();

					KCommand ifCommand = KCommand.CreateIf(condition);
					ifCommand.UnderCommands = ParseUnderCommands(lines, ref index, COMMAND_IF_END);

					commands.Add(ifCommand);
					continue;
				}

				if (tokens[0] == COMMAND_LP_BGN)
				{
					string[] condition = tokens.Skip(1).ToArray();

					KCommand loopCommand = KCommand.CreateLoop(condition);
					loopCommand.UnderCommands = ParseUnderCommands(lines, ref index, COMMAND_LP_END);

					commands.Add(loopCommand);
					continue;
				}

				if (tokens.Length >= 2 && tokens[1] == "=")
				{
					commands.Add(KCommand.CreateAssignment(
						tokens[0],
						tokens.Skip(2).ToArray())
						);
				}
				else
				{
					commands.Add(KCommand.CreateCommand(
						tokens[0],
						tokens.Skip(1).ToArray())
						);
				}
			}

			if (endWord != null)
				throw new Exception($"[[ {endWord} ]] が見つかりません。");

			return commands.ToArray();
		}
	}
}
