using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using Nekos;
using RimWorld;
using UnityEngine;
using Verse;

namespace Neko
{
	// Token: 0x0200000C RID: 12
	[StaticConstructorOnStartup]
	internal static class Injector
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002E08 File Offset: 0x00001008
		/*static Injector()
		{
			HarmonyInstance harmonyInstance = HarmonyInstance.Create("chjees.Neko");
			Type typeFromHandle = typeof(Plant);
			PropertyInfo propertyInfo = AccessTools.Property(typeFromHandle, "GrowthRateFactor_Temperature");
			MethodInfo getMethod = propertyInfo.GetGetMethod();
			HarmonyMethod prefix = new HarmonyMethod(typeof(Injector), "Patch_Plant_GrowthRateFactor_Temperature_get", null);
			harmonyInstance.Patch(getMethod, prefix, null, null);
			Type typeFromHandle2 = typeof(Zone_Growing);
			MethodInfo method = typeFromHandle2.GetMethod("GetInspectString");
			HarmonyMethod prefix2 = new HarmonyMethod(typeof(Injector), "Patch_Zone_Growing_GetInspectString", null);
			harmonyInstance.Patch(method, prefix2, null, null);
			Type typeFromHandle3 = typeof(PlantUtility);
			MethodInfo method2 = typeFromHandle3.GetMethod("GrowthSeasonNow");
			HarmonyMethod prefix3 = new HarmonyMethod(typeof(Injector), "Patch_GrowthSeasonNow", null);
			harmonyInstance.Patch(method2, prefix3, null, null);
		}*/

		// Token: 0x06000027 RID: 39 RVA: 0x00002EE4 File Offset: 0x000010E4
		/*public static bool Patch_GrowthSeasonNow(ref bool __result, ref IntVec3 c, Map map, bool forSowing)
		{
			ThingDef thingDef = WorkGiver_Grower.CalculateWantedPlantDef(c, map);
			bool flag = thingDef != null && thingDef.thingClass == typeof(FrostPlant);
			bool result;
			if (flag)
			{
				__result = Injector.GrowthSeasonNowFrostPlant(c, map);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}*/

		// Token: 0x06000028 RID: 40 RVA: 0x00002F33 File Offset: 0x00001133
		/*public static IEnumerable<CodeInstruction> JobOnCellTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
		{
			MethodInfo growthSeason = AccessTools.Method(typeof(PlantUtility), "GrowthSeasonNow", null, null);
			foreach (CodeInstruction instruction in instructions)
			{
				bool flag = instruction.opcode == OpCodes.Call && instruction.operand == growthSeason;
				if (flag)
				{
					instruction.operand = AccessTools.Method(typeof(Injector), "FrostPlantSeason", null, null);
					yield return instruction;
				}
				else
				{
					yield return instruction;
				}
				//instruction = null;
			}
			//IEnumerator<CodeInstruction> enumerator = null;
			yield break;
		}*/

		// Token: 0x06000029 RID: 41 RVA: 0x00002F4C File Offset: 0x0000114C
		/*public static bool FrostPlantSeason(IntVec3 c, Map map)
		{
			ThingDef thingDef = WorkGiver_Grower.CalculateWantedPlantDef(c, map);
			bool flag = thingDef != null && thingDef.thingClass == typeof(FrostPlant);
			bool result;
			if (flag)
			{
				bool flag2 = Injector.GrowthSeasonNowFrostPlant(c, map);
				result = flag2;
			}
			else
			{
				result = false;
			}
			return result;
		}*/

		// Token: 0x0600002A RID: 42 RVA: 0x00002F90 File Offset: 0x00001190
		/*public static bool Patch_Zone_Growing_GetInspectString(ref Zone_Growing __instance, ref string __result)
		{
			ThingDef plantDefToGrow = __instance.GetPlantDefToGrow();
			bool flag = plantDefToGrow.thingClass != typeof(FrostPlant);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				string text = "";
				bool flag2 = !GenList.NullOrEmpty<IntVec3>(__instance.Cells);
				if (flag2)
				{
					IntVec3 intVec = __instance.Cells.First<IntVec3>();
					bool flag3 = GridsUtility.UsesOutdoorTemperature(intVec, __instance.Map);
					if (flag3)
					{
						string text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							Translator.Translate("OutdoorGrowingPeriod"),
							": ",
							Zone_Growing.GrowingQuadrumsDescription(__instance.Map.Tile),
							" (" + Translator.Translate("NekosColdImmunePlants") + ")\n"
						});
					}
					bool flag4 = Injector.GrowthSeasonNowFrostPlant(intVec, __instance.Map);
					if (flag4)
					{
						text += Translator.Translate("GrowSeasonHereNow");
					}
					else
					{
						text += Translator.Translate("CannotGrowBadSeasonTemperature");
					}
					__result = text;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000030AC File Offset: 0x000012AC
		public static bool GrowthSeasonNowFrostPlant(IntVec3 c, Map map)
		{
			Room roomOrAdjacent = GridsUtility.GetRoomOrAdjacent(c, map, 7);
			bool flag = roomOrAdjacent == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				float temperature = GridsUtility.GetTemperature(c, map);
				result = (temperature < 58f);
			}
			return result;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000030EC File Offset: 0x000012EC
		public static bool Patch_GenPlant_GrowthSeasonNow(ref bool __result, ref IntVec3 c, ref Map map)
		{
			Thing thing = map.thingGrid.ThingAt(c, 4);
			bool flag = thing == null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = thing.GetType() != typeof(FrostPlant);
				if (flag2)
				{
					result = true;
				}
				else
				{
					__result = Injector.GrowthSeasonNowFrostPlant(c, map);
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00003150 File Offset: 0x00001350
		public static bool Patch_Plant_GrowthRateFactor_Temperature_get(ref Plant __instance, ref float __result)
		{
			bool flag = __instance.GetType() != typeof(FrostPlant);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				float num;
				bool flag2 = !GenTemperature.TryGetTemperatureForCell(__instance.Position, __instance.Map, ref num);
				float num2;
				if (flag2)
				{
					num2 = 1f;
				}
				else
				{
					bool flag3 = num > 42f;
					if (flag3)
					{
						num2 = Mathf.InverseLerp(58f, 42f, num);
					}
					else
					{
						num2 = 1f;
					}
				}
				__result = num2;
				result = false;
			}
			return result;
		}*/
	}
}
