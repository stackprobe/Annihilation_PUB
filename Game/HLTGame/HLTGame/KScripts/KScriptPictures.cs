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
	public static class KScriptPictures
	{
		private static Dictionary<string, APicture> Cache = new Dictionary<string, APicture>();

		public static APicture GetPicture(string name)
		{
			if (!SCommon.IsFairLocalPath(name, -1))
				throw null; // never

			if (!Cache.ContainsKey(name))
				Cache.Add(name, new APicture(Path.Combine(KScriptConsts.RESDIR_PICTURE, name + ".png")));

			return Cache[name];
		}
	}
}
