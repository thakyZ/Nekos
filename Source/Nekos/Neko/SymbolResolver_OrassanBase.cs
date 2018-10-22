using System;
using System.Collections.Generic;
using System.Linq;
using CustomFactionBase;
using Harmony;
using RimWorld;
using RimWorld.BaseGen;
using Verse;

namespace Neko
{
	// Token: 0x0200000F RID: 15
	[StaticConstructorOnStartup]
	internal class SymbolResolver_NekoBase : SymbolResolver
	{
		// Token: 0x06000032 RID: 50 RVA: 0x000033D5 File Offset: 0x000015D5
		static SymbolResolver_NekoBase()
		{
			CustomBaseUtility.AddMapResolver(new ResolverStruct((ResolveParams rp) => rp.faction.def == NekoDefOf.Neko, "NekoBase", 100f));
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000033FC File Offset: 0x000015FC
		public override void Resolve(ResolveParams rp)
		{
			this.rp = rp;
			this.map = BaseGen.globalSettings.map;
			this.rp.rect.Width = 68;
			this.rp.rect.Height = 60;
			this.GenerateBaseHex(new IntVec3(this.rp.rect.CenterCell.x + 12, this.rp.rect.CenterCell.y, this.rp.rect.CenterCell.z + 7));
			this.GenerateBaseHex(new IntVec3(this.rp.rect.CenterCell.x + 12, this.rp.rect.CenterCell.y, this.rp.rect.CenterCell.z - 7));
			this.GenerateBaseHex(new IntVec3(this.rp.rect.CenterCell.x - 12, this.rp.rect.CenterCell.y, this.rp.rect.CenterCell.z + 7));
			this.GenerateBaseHex(new IntVec3(this.rp.rect.CenterCell.x - 12, this.rp.rect.CenterCell.y, this.rp.rect.CenterCell.z - 7));
			this.GenerateBaseHex(new IntVec3(this.rp.rect.CenterCell.x, this.rp.rect.CenterCell.y, this.rp.rect.CenterCell.z - 15 + 1));
			this.GenerateBaseHex(new IntVec3(this.rp.rect.CenterCell.x, this.rp.rect.CenterCell.y, this.rp.rect.CenterCell.z + 15 - 1));
			this.GenerateCenterHex();
			LongEventHandler.QueueLongEvent(delegate()
			{
				Thing.allowDestroyNonDestroyable = true;
				Traverse traverse = Traverse.Create(this.map.terrainGrid);
				Traverse traverse2 = traverse.Field("underGrid");
				TerrainDef[] underGrid = traverse2.GetValue<TerrainDef[]>();
				this.rp.rect.Cells.ToList<IntVec3>().ForEach(delegate(IntVec3 c)
				{
					GridsUtility.GetThingList(c, this.map).ForEach(delegate(Thing t)
					{
						bool flag = !this.thingList.Contains(t);
						if (flag)
						{
							t.Destroy(0);
						}
						underGrid[this.map.cellIndices.CellToIndex(c)] = TerrainDefOf.Soil;
						this.map.terrainGrid.SetTerrain(c, NekoDefOf.TileGranite);
					});
				});
				traverse2.SetValue(underGrid);
				Thing.allowDestroyNonDestroyable = false;
			}, "clean up", false, null);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003648 File Offset: 0x00001848
		private void GenerateCenterHex()
		{
			IntVec3 centerCell = this.rp.rect.CenterCell;
			this.GenerateBaseHex(centerCell);
			int num = centerCell.x - 8;
			int z = centerCell.z;
			GenSpawn.Spawn(this.GenerateThing(ThingDefOf.OrbitalTradeBeacon, null, this.rp.faction), centerCell, this.map, 0);
			bool flag = false;
			for (int i = 1; i <= 2; i++)
			{
				bool flag2 = false;
				for (int j = 1; j <= 2; j++)
				{
					IntVec3 intVec;
					intVec..ctor(num + (flag ? -1 : 1), centerCell.y, z + (flag2 ? -1 : 1) * 2);
					GridsUtility.GetFirstThing(intVec, this.map, ThingDefOf.Wall).Destroy(0);
					GenSpawn.Spawn(this.GenerateThing(ThingDefOf.Door, ThingDefOf.BlocksGranite, this.rp.faction), intVec, this.map, 0);
					GenSpawn.Spawn(this.GenerateThing(NekoDefOf.SculptureLarge, null, this.rp.faction), new IntVec3(num + (flag ? -1 : 1) * 2, centerCell.y, z + (flag2 ? -1 : 1)), this.map, 0);
					GenSpawn.Spawn(this.GenerateThing(NekoDefOf.SculptureLarge, null, this.rp.faction), new IntVec3(num + (flag ? -1 : 1) * 3, centerCell.y, z + (flag2 ? -1 : 1) * 3), this.map, 0);
					GenSpawn.Spawn(this.GenerateWorkbench(this.rp.faction), new IntVec3(num + (flag ? -1 : 1) * 6, centerCell.y, z + (flag2 ? -1 : 1) * 6), this.map, flag2 ? Rot4.South : Rot4.North, 0, false);
					z = centerCell.z;
					flag2 = true;
				}
				GenSpawn.Spawn(this.GenerateThing(ThingDefOf.StandingLamp, ThingDefOf.BlocksGranite, this.rp.faction), new IntVec3(num + (flag ? -1 : 1) * -1, centerCell.y, centerCell.z), this.map, 0);
				num = centerCell.x + 8;
				flag = true;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003888 File Offset: 0x00001A88
		public void GenerateBaseHex(IntVec3 center)
		{
			int num = center.x - 8;
			int num2 = center.z - 7;
			int num3 = center.x + 8;
			int num4 = center.z + 7;
			GenSpawn.Spawn(this.GenerateThing(ThingDefOf.Door, ThingDefOf.BlocksGranite, this.rp.faction), new IntVec3(center.x, center.y, num4), this.map, 0);
			GenSpawn.Spawn(this.GenerateThing(ThingDefOf.Door, ThingDefOf.BlocksGranite, this.rp.faction), new IntVec3(center.x, center.y, num2), this.map, 0);
			for (int i = 1; i <= 3; i++)
			{
				GenSpawn.Spawn(this.GenerateThing(ThingDefOf.Wall, ThingDefOf.BlocksGranite, this.rp.faction), new IntVec3(center.x + i, center.y, num4), this.map, 0);
				GenSpawn.Spawn(this.GenerateThing(ThingDefOf.Wall, ThingDefOf.BlocksGranite, this.rp.faction), new IntVec3(center.x + i, center.y, num2), this.map, 0);
				GenSpawn.Spawn(this.GenerateThing(ThingDefOf.Wall, ThingDefOf.BlocksGranite, this.rp.faction), new IntVec3(center.x - i, center.y, num4), this.map, 0);
				GenSpawn.Spawn(this.GenerateThing(ThingDefOf.Wall, ThingDefOf.BlocksGranite, this.rp.faction), new IntVec3(center.x - i, center.y, num2), this.map, 0);
			}
			GenSpawn.Spawn(this.GenerateThing(ThingDefOf.Wall, ThingDefOf.BlocksGranite, this.rp.faction), new IntVec3(num, center.y, center.z), this.map, 0);
			GenSpawn.Spawn(this.GenerateThing(ThingDefOf.Wall, ThingDefOf.BlocksGranite, this.rp.faction), new IntVec3(num3, center.y, center.z), this.map, 0);
			int num5 = num;
			int z = center.z;
			bool flag = false;
			for (int j = 1; j <= 2; j++)
			{
				bool flag2 = false;
				for (int k = 1; k <= 2; k++)
				{
					int l = 0;
					int num6 = 0;
					int num7 = 0;
					while (l <= 6)
					{
						switch (num7 % 5)
						{
						case 0:
						case 2:
						case 3:
							l++;
							break;
						case 1:
						case 4:
							num6++;
							break;
						}
						GenSpawn.Spawn(this.GenerateThing(ThingDefOf.Wall, ThingDefOf.BlocksGranite, this.rp.faction), new IntVec3(num5 + (flag ? -1 : 1) * num6, center.y, z + (flag2 ? -1 : 1) * l), this.map, 0);
						num7++;
					}
					z = center.z;
					flag2 = true;
				}
				num5 = num3;
				flag = true;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003BC7 File Offset: 0x00001DC7
		private Thing GenerateWorkbench(Faction faction)
		{
			return this.GenerateThing(GenCollection.RandomElement<ThingDef>(from td in DefDatabase<ThingDef>.AllDefs
			where td.building != null && !GenList.NullOrEmpty<RecipeDef>(td.AllRecipes) && td.Size.x == 3 && td.Size.z == 1
			select td), null, faction);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003C00 File Offset: 0x00001E00
		private Thing GenerateThing(ThingDef def, ThingDef stuff, Faction fac)
		{
			Thing thing = ThingMaker.MakeThing(def, def.MadeFromStuff ? (stuff ?? GenStuff.RandomStuffFor(def)) : null);
			thing.SetFactionDirect(fac);
			this.thingList.Add(thing);
			return thing;
		}

		// Token: 0x04000016 RID: 22
		private const int width = 17;

		// Token: 0x04000017 RID: 23
		private const int height = 15;

		// Token: 0x04000018 RID: 24
		private ResolveParams rp;

		// Token: 0x04000019 RID: 25
		private Map map;

		// Token: 0x0400001A RID: 26
		private List<Thing> thingList = new List<Thing>();
	}
}
