using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.Commons;

namespace HLTStudio.Games.Gameplays.Scenarios
{
	public abstract class ScenarioBase
	{
		private Func<bool> S_EachFrame = null;

		public bool EachFrame()
		{
			if (this.S_EachFrame == null)
				this.S_EachFrame = SCommon.Supplier(this.E_EachFrame());

			return this.S_EachFrame();
		}

		protected abstract IEnumerable<bool> E_EachFrame();
	}
}
