using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HLTStudio.Commons;

namespace HLTStudio.KScripts
{
	public class KCommand
	{
		private static KCommand[] EMPTRY_COMMANDS = new KCommand[0];

		public enum Kind_e
		{
			COMMAND,
			ASSIGNMENT,
			FUNCTION,
			IF,
			LOOP,
		}

		public Kind_e Kind;
		public string Name;
		public string[] Arguments;
		public KCommand[] UnderCommands;

		private KCommand(Kind_e kind, string name, string[] arguments)
		{
			Kind = kind;
			Name = name;
			Arguments = arguments;
			UnderCommands = EMPTRY_COMMANDS;
		}

		public static KCommand CreateFunction(string name)
		{
			return new KCommand(Kind_e.FUNCTION, name, SCommon.EMPTY_STRINGS);
		}

		public static KCommand CreateCommand(string name, string[] arguments)
		{
			return new KCommand(Kind_e.COMMAND, name, arguments);
		}

		public static KCommand CreateAssignment(string name, string[] expression)
		{
			return new KCommand(Kind_e.ASSIGNMENT, name, expression);
		}

		public static KCommand CreateIf(string[] condition)
		{
			return new KCommand(Kind_e.IF, KScriptConsts.DUMMY_NAME, condition);
		}

		public static KCommand CreateLoop(string[] condition)
		{
			return new KCommand(Kind_e.LOOP, KScriptConsts.DUMMY_NAME, condition);
		}
	}
}
