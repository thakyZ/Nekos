using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Nekos
{
  [StaticConstructorOnStartup]
  public class Placeworker_Neko : PlaceWorker
  {
    static Placeworker_Neko()
    {
      Placeworker_Neko.acceptableTerrains.Add(TerrainDefOf.Soil);
      Placeworker_Neko.acceptableTerrains.Add(TerrainDefOf.Gravel);
      Placeworker_Neko.acceptableTerrains.Add(TerrainDefOf.Ice);
      Placeworker_Neko.acceptableTerrains.Add(TerrainDef.Named("MarshyTerrain"));
      Placeworker_Neko.acceptableTerrains.Add(TerrainDef.Named("MossyTerrain"));
      Placeworker_Neko.acceptableTerrains.Add(TerrainDef.Named("SoilRich"));
      Placeworker_Neko.acceptableTerrains.Add(TerrainDef.Named("NekoSoil"));
    }

    public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
    {
      TerrainDef item = map.terrainGrid.TerrainAt(loc);
      bool flag = Placeworker_Neko.acceptableTerrains.Contains(item);
      AcceptanceReport result;
      if (flag)
      {
        result = AcceptanceReport.WasAccepted;
      }
      else
      {
        result = AcceptanceReport.WasRejected;
      }
      return result;
    }

    public static List<TerrainDef> acceptableTerrains = new List<TerrainDef>();
  }
}
