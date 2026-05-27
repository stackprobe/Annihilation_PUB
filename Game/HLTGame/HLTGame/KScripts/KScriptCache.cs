using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Commons;
using HLTStudio.GameCommons;

namespace HLTStudio.KScripts
{
	public static class KScriptCache
	{
		private static Dictionary<string, KCommand> Cache = new Dictionary<string, KCommand>();

		public static KCommand GetPicture(string name)
		{
			if (!SCommon.IsFairLocalPath(name, -1))
				throw null; // never

			if (!Cache.ContainsKey(name))
			{
				string resPath = Path.Combine(KScriptConsts.RESDIR_SCRIPT, name + ".txt");
				byte[] resData = DD.GetStorageFileData(resPath).Data.Value;
				string resText = Encoding.UTF8.GetString(resData);
				string[] resLines = SCommon.TextToLines(resText);

				Cache.Add(name, KCommandParser.Parse(name, resLines));
			}
			return Cache[name];
		}
	}
}
