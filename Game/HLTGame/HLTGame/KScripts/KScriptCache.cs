using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using HLTStudio.Commons;
using HLTStudio.GameCommons;

namespace HLTStudio.KScripts
{
	public static class KScriptCache
	{
		private static Dictionary<string, KCommand> Cache = new Dictionary<string, KCommand>();

		public static KCommand GetFunctionCommand(string name)
		{
			if (!SCommon.IsFairLocalPath(name, -1))
				throw null; // never

			if (!Cache.ContainsKey(name))
			{
#if !true // 本来はこちら
				string resPath = Path.Combine(KScriptConsts.RESDIR_SCRIPT, name + ".txt");
				byte[] resData = DD.GetStorageFileData(resPath).Data.Value;
#else // zantei zantei zantei zantei zantei
				string resPath = Path.Combine(ProcMain.SelfDir, KScriptConsts.RESDIR_SCRIPT);
				if (!Directory.Exists(resPath)) resPath = Path.Combine(@"..\..\..\..", KScriptConsts.RESDIR_SCRIPT);
				resPath = Path.Combine(resPath, name + ".txt");
				byte[] resData = File.ReadAllBytes(resPath);
#endif
				string resText = BinaryToText(resData);
				string[] resLines = SCommon.TextToLines(resText);

				Cache.Add(name, KCommandParser.Run(name, resLines));
			}
			return Cache[name];
		}

		private static string BinaryToText(byte[] data)
		{
			Encoding encoding;

			if (HasUTF8_BOM(data))
				encoding = Encoding.UTF8;
			else
				encoding = SCommon.ENCODING_SJIS;

			return encoding.GetString(data);
		}

		private static bool HasUTF8_BOM(byte[] data)
		{
			return
				data.Length >= 3 &&
				data[0] == 0xEF &&
				data[1] == 0xBB &&
				data[2] == 0xBF;
		}
	}
}
