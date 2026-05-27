using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DxLibDLL;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.Games.Gameplays;
using HLTStudio.Games.FrontEnds;

namespace HLTStudio
{
	public class UProgram
	{
		public void Run()
		{
			if (ProcMain.DEBUG)
			{
				Main3();
			}
			else
			{
				Main4();
			}
		}

		private void Main3()
		{
#if DEBUG
			// -- choose one --

			TitleMenu.Run();
			//Main4();
			//Test01();

			// --
#endif
		}

#if DEBUG
		private void Test01()
		{
			GEMain.Run();
		}
#endif

		private void Main4()
		{
			Logo.Run();
		}
	}
}
