using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.GameCommons;

namespace HLTStudio.Games.Gameplays.Enemies.Events
{
	public class DummyEvent : EventBase
	{
		public DummyEvent()
		{
			this.Initialize_Event();
		}

		protected override IEnumerable<bool> E_EachFrame()
		{
			foreach (AScene scene in AScene.Create(60))
			{
				// none

				yield return true;
			}
		}
	}
}
