using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLTStudio.Games.Gameplays
{
	public static class GEConsts
	{
		public const int INIT_PLAYER_LIFE = 3;
		public const int INIT_BOMB_COUNT = 3;

		public static double PLAYER_SPAWN_X = BattleField.Screen.W * 0.5;
		public static double PLAYER_SPAWN_Y = BattleField.Screen.H * 0.9;
	}
}
