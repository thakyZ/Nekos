using System;
using RimWorld.BaseGen;
using Verse;

namespace CustomFactionBase
{
	// Token: 0x02000006 RID: 6
	public struct ResolverStruct
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000026F9 File Offset: 0x000008F9
		public string RuleDefName
		{
			get
			{
				return this.symbol;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002701 File Offset: 0x00000901
		private bool Chance
		{
			get
			{
				return Rand.Chance(this.chance / 100f);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002714 File Offset: 0x00000914
		public bool Active(ResolveParams rp)
		{
			return this.Chance && this.enabler(rp);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000272D File Offset: 0x0000092D
		public ResolverStruct(Predicate<ResolveParams> enabler, string symbol, float chance)
		{
			this.enabler = enabler;
			this.symbol = symbol;
			this.chance = chance;
		}

		// Token: 0x0400000A RID: 10
		private Predicate<ResolveParams> enabler;

		// Token: 0x0400000B RID: 11
		private string symbol;

		// Token: 0x0400000C RID: 12
		private float chance;
	}
}
