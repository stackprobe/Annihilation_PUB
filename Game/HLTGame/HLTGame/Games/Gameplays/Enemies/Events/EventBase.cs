using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLTStudio.Games.Gameplays.Enemies.Events
{
	public abstract class EventBase : EnemyBase
	{
		protected void Initialize_Event()
		{
			this.Initialize(-100.0, -100.0, 0);
		}
	}
}
