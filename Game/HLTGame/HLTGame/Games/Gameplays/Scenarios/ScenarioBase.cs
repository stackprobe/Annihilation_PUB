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
				this.S_EachFrame = SCommon.Supplier(this.E_ExpandWaitFrames());

			return this.S_EachFrame();
		}

		private IEnumerable<bool> E_ExpandWaitFrames()
		{
			foreach (int waitFrame in this.E_EachFrame())
			{
				if (waitFrame < 1)
					break;

				for (int frame = 0; frame < waitFrame; frame++)
					yield return true;
			}
		}

		protected abstract IEnumerable<int> E_EachFrame();
	}
}
