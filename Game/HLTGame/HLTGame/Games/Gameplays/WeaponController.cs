using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.Commons;
using HLTStudio.GameCommons;
using HLTStudio.Games.Gameplays.Weapons;

namespace HLTStudio.Games.Gameplays
{
	public class WeaponController
	{
		private List<WeaponBase> Weapons = new List<WeaponBase>();

		public void Add(WeaponBase weapon)
		{
			this.Weapons.Add(weapon);
		}

		public IEnumerable<WeaponBase> Iterate()
		{
			return this.Weapons;
		}

		public List<WeaponBase> GetWeapons()
		{
			return this.Weapons;
		}

		public void EachFrame()
		{
			for (int index = 0; index < this.Weapons.Count;)
			{
				if (this.Weapons[index].EachFrame())
				{
					index++;
				}
				else
				{
					SCommon.FastDesertElement(this.Weapons, index);
				}
			}
		}
	}
}
