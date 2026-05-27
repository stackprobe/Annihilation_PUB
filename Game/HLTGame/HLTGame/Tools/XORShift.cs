using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HLTStudio.Commons;

namespace HLTStudio.Tools
{
	public class XORShift
	{
		private ulong C;

		public XORShift(ulong seed = 1UL)
		{
			this.C = seed;
		}

		public ulong Next64()
		{
			this.C ^= this.C << 13;
			this.C ^= this.C >> 7;
			this.C ^= this.C << 17;
			return this.C;
		}

		public uint Next32()
		{
			return (uint)(this.Next64() >> 32);
		}

		public uint GetUInt(uint modulo)
		{
			return this.Next32() % modulo;
		}

		public int GetInt(int modulo)
		{
			return (int)this.GetUInt((uint)modulo);
		}

		public int GetRange(int minval, int maxval)
		{
			return this.GetInt(maxval - minval + 1) + minval;
		}

		public bool GetBoolean()
		{
			return this.Next32() % 2 != 0u;
		}

		public void Shuffle<T>(IList<T> list)
		{
			for (int index = list.Count; 1 < index; index--)
			{
				SCommon.Swap(list, this.GetInt(index), index - 1);
			}
		}
	}
}
