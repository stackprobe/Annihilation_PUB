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
	public static class KScriptPictures
	{
		private static Dictionary<string, APicture> Cache = new Dictionary<string, APicture>();

		public static APicture GetPicture(string name)
		{
			if (!SCommon.IsFairLocalPath(name, -1))
				throw null; // never

			if (!Cache.ContainsKey(name))
			{
#if !true // 本来はこちら
				Cache.Add(name, new APicture(() => DD.GetStorageFileData(Path.Combine(KScriptConsts.RESDIR_PICTURE, name + ".png"))));
#else // zantei zantei zantei zantei zantei
				string resPath = Path.Combine(ProcMain.SelfDir, KScriptConsts.RESDIR_PICTURE);
				if (!Directory.Exists(resPath)) resPath = Path.Combine(@"..\..\..\..", KScriptConsts.RESDIR_PICTURE);
				resPath = Path.Combine(resPath, name + ".png");
				byte[] resData = File.ReadAllBytes(resPath);

				Cache.Add(name, new APicture(() => new DD.LzData(resData.Length, () => resData)));
#endif
			}
			return Cache[name];
		}
	}
}
