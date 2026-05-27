using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.Commons;
using HLTStudio.Drawings;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;
using HLTStudio.Games.Gameplays.Enemies;
using HLTStudio.Games.Gameplays.Enemies.Bullets;

namespace HLTStudio.Games.Gameplays
{
	public class EnemyController
	{
		private List<EnemyBase> Enemies = new List<EnemyBase>();

		public void Add(EnemyBase enemy)
		{
			this.Enemies.Add(enemy);
		}

		public IEnumerable<EnemyBase> Iterate()
		{
			return this.Enemies;
		}

		public List<EnemyBase> GetEnemies()
		{
			return this.Enemies;
		}

		public void EachFrame()
		{
			for (int index = 0; index < this.Enemies.Count;)
			{
				if (this.Enemies[index].EachFrame())
				{
					index++;
				}
				else
				{
					SCommon.FastDesertElement(this.Enemies, index);
				}
			}
		}

		public void EraseBullet()
		{
			this.Enemies.RemoveAll(enemy => enemy is BulletBase);
		}
	}
}
