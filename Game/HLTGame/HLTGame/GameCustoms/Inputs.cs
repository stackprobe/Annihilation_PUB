using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DxLibDLL;
using HLTStudio.Commons;
using HLTStudio.GameCommons;

namespace HLTStudio.GameCustoms
{
	public static class Inputs
	{
		public static AInput DIR_8 = new AInput(3, new int[] { DX.KEY_INPUT_UP }, "上");
		public static AInput DIR_2 = new AInput(0, new int[] { DX.KEY_INPUT_DOWN }, "下");
		public static AInput DIR_4 = new AInput(1, new int[] { DX.KEY_INPUT_LEFT }, "左");
		public static AInput DIR_6 = new AInput(2, new int[] { DX.KEY_INPUT_RIGHT }, "右");
		public static AInput ATTACK = new AInput(7, new int[] { DX.KEY_INPUT_Z }, "攻撃 (決定)");
		public static AInput SLOW = new AInput(4, new int[] { DX.KEY_INPUT_LSHIFT }, "低速移動 (キャンセル)");
		public static AInput BOMB = new AInput(5, new int[] { DX.KEY_INPUT_X }, "ボム");
		public static AInput FIELD_ACTIVE = new AInput(8, new int[] { DX.KEY_INPUT_A }, "フィールド展開");
		public static AInput START = new AInput(13, new int[] { DX.KEY_INPUT_SPACE }, "START");

		public static AInput DECIDE => ATTACK;
		public static AInput CANCEL => SLOW;
	}
}
