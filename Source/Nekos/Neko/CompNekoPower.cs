using RimWorld;
using System;
using System.Linq;
using Verse;

namespace Neko
{
  internal class CompNekoPower : CompPowerTrader
  {
    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
      base.PostSpawnSetup(respawningAfterLoad);
      base.PowerOn = true;
      Faction faction;
      if (!GenText.EqualsIgnoreCase(parent.Faction.def.defName, "NekoPlayerColony"))
      {
        faction = Find.FactionManager.AllFactionsListForReading.FirstOrDefault((Faction x) => x.def.defName.Contains("Neko"));
      }
      else
      {
        faction = parent.Faction;
      }
      this.faction = faction;
    }
    
    public override void CompTick()
    {
      base.CompTick();
      bool flag = faction == null;
      if (flag)
      {
        faction = Find.FactionManager.AllFactionsListForReading.FirstOrDefault((Faction x) => x.def.defName.Contains("Neko"));
      }
      bool flag2 = !BreakdownableUtility.IsBrokenDown(parent) && faction != null;
      if (flag2)
      {
        base.PowerOutput = Math.Max(0f, (faction.IsPlayer ? 100f : faction.PlayerGoodwill) * 100f);
      }
      else
      {
        base.PowerOutput = 0f;
      }
    }
    
    private Faction faction;
  }
}
