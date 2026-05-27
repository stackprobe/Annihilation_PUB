using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.GameCommons;

namespace HLTStudio.GameCustoms
{
	public static class Musics
	{
		public static AMusic Dummy = new AMusic(@"General\muon10s.mp3");

		public static AMusic RemotestLibrary = new AMusic(@"Freebie\ユーフルカ\Remotest-Liblary_loop\Remotest-Liblary_loop.m4a", 192053, 4893464);
		public static AMusic WanderersCity = new AMusic(@"Freebie\ユーフルカ\Wanderers-City_loop\Wanderers-City_loop.m4a", 359706, 4498204);
		public static AMusic SunBeams = new AMusic(@"Freebie\ユーフルカ\sunbeams_loop\sunbeams_loop.m4a", 576250, 4908523);

		public static AMusic BattleField = new AMusic(@"Freebie\DOVA-SYNDROME\銀色都市の秘密.mp3");
	}
}
