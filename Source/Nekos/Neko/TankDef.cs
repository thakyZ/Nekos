using RimWorld;
using Verse;

namespace Neko
{
  public class TankDef : ThingDef
  {
    public override void ResolveReferences()
    {
      inspectorTabs.Remove(typeof(ITab_Pawn_Needs));
      inspectorTabs.Remove(typeof(ITab_Pawn_Character));
      inspectorTabs.Remove(typeof(ITab_Pawn_Training));
      inspectorTabs.Remove(typeof(ITab_Pawn_Gear));
      inspectorTabs.Remove(typeof(ITab_Pawn_Guest));
      inspectorTabs.Remove(typeof(ITab_Pawn_Prisoner));
      inspectorTabs.Remove(typeof(ITab_Pawn_Social));
      base.ResolveReferences();
      Log.Message("Nya! Nya! Nya!", false);
    }
  }
}
