using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLTStudio.Games.Gameplays.Enemies.Bullets
{
	public abstract class BulletBase : EnemyBase
	{
		public bool Learnable = false;

		protected void Initialize_Bullet(double x, double y, bool learnable)
		{
			this.Initialize(x, y, 0);

			this.Learnable = learnable;
		}

		public abstract int GetLearnPoint();
	}
}
