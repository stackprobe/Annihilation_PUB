using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HLTStudio.GameCommons;
using HLTStudio.GameCustoms;

namespace HLTStudio.Games.Gameplays.Enemies.Items
{
	public class OrdinaryItem : ItemBase
	{
		public OrdinaryItem(double x, double y)
		{
			this.Initialize_Item(x, y);
		}

		protected override APicture GetPicture()
		{
			return Pictures.OrdinaryItem;
		}

		protected override void PlayerCollected()
		{
			GEMain.I.Score += 100;
		}
	}
}
