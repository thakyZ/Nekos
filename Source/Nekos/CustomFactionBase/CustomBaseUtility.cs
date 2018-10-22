using System;
using System.Collections.Generic;
using Harmony;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace CustomFactionBase
{
	// Token: 0x02000005 RID: 5
	[StaticConstructorOnStartup]
	public static class CustomBaseUtility
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002508 File Offset: 0x00000708
		static CustomBaseUtility()
		{
			HarmonyInstance.Create("rimworld.erdelf.customBaseUtility").Patch(AccessTools.Method(typeof(SymbolResolver_Settlement), "Resolve", null, null), new HarmonyMethod(typeof(CustomBaseUtility), "FactionBasePrefix", null), null, null);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000255C File Offset: 0x0000075C
		private static bool FactionBasePrefix(ref ResolveParams rp)
		{
			rp.faction = (rp.faction ?? Find.FactionManager.RandomEnemyFaction(false, false, true, 0));
			foreach (ResolverStruct resolverStruct in CustomBaseUtility.resolvers)
			{
				bool flag = resolverStruct.Active(rp);
				if (flag)
				{
					BaseGen.symbolStack.Push(resolverStruct.RuleDefName, rp);
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002600 File Offset: 0x00000800
		public static void AddMapResolver(ResolverStruct struc)
		{
			CustomBaseUtility.resolvers.Add(struc);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002610 File Offset: 0x00000810
		public static void ReplaceFloor(ResolveParams rp, TerrainDef floor)
		{
			Map map = BaseGen.globalSettings.map;
			LongEventHandler.QueueLongEvent(delegate()
			{
				TerrainGrid terrainGrid = map.terrainGrid;
				TerrainDef floor2 = floor;
				CellRect.CellRectIterator iterator = rp.rect.GetIterator();
				while (!iterator.Done())
				{
					terrainGrid.SetTerrain(iterator.Current, floor2);
					iterator.MoveNext();
				}
			}, "floor" + DateTime.Now.GetHashCode(), false, null);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002674 File Offset: 0x00000874
		public static void ReplaceEdgeWalls(ResolveParams rp, ThingDef wall, ThingDef door = null)
		{
			Map map = BaseGen.globalSettings.map;
			rp.rect = rp.rect.ExpandedBy(1);
			LongEventHandler.QueueLongEvent(delegate()
			{
				foreach (IntVec3 intVec in rp.rect.EdgeCells)
				{
					bool flag = GridsUtility.GetFirstThing(intVec, map, ThingDefOf.Wall) != null;
					if (flag)
					{
						GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Wall, wall), intVec, map, 0);
					}
					else
					{
						bool flag2 = GridsUtility.GetFirstThing(intVec, map, ThingDefOf.Door) != null;
						if (flag2)
						{
							GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Door, door ?? wall), intVec, map, 0);
						}
					}
				}
			}, "wall" + DateTime.Now.GetHashCode(), false, null);
		}

		// Token: 0x04000009 RID: 9
		internal static List<ResolverStruct> resolvers = new List<ResolverStruct>();
	}
}
