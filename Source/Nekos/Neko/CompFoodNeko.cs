using RimWorld;
using UnityEngine;
using Verse;

namespace Neko
{
  internal class CompFoodNeko : ThingComp
  {
    public float NekoPrepared
    {
      get => nekoPrepared;
      set => nekoPrepared = Mathf.Clamp01(value);
    }

    public override void PostSpawnSetup(bool respawningAfterLoad)
    {
      base.PostSpawnSetup(respawningAfterLoad);
      Thing thing = GenClosest.ClosestThing_Global(parent.PositionHeld, parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.Pawn), 5f, null, null);
      NekoPrepared = ((thing != null && thing is Pawn && (thing as Pawn).kindDef.race.Equals(NekoDefOf.Alien_Neko)) ? 1f : 0f);
    }

    public override void PostExposeData()
    {
      base.PostExposeData();
      Scribe_Values.Look<float>(ref nekoPrepared, "NekoPrepared", 0f, false);
    }

    public override void PostSplitOff(Thing piece)
    {
      base.PostSplitOff(piece);
      CompFoodNeko compFoodNeko = ThingCompUtility.TryGetComp<CompFoodNeko>(piece);
      compFoodNeko.NekoPrepared = NekoPrepared;
    }

    public override void PreAbsorbStack(Thing otherStack, int count)
    {
      base.PreAbsorbStack(otherStack, count);
      CompFoodNeko compFoodNeko = ThingCompUtility.TryGetComp<CompFoodNeko>(otherStack);
      NekoPrepared = GenMath.WeightedAverage(NekoPrepared, parent.stackCount, compFoodNeko.NekoPrepared, count);
    }

    public override void PostIngested(Pawn ingester)
    {
      base.PostIngested(ingester);
      bool flag = ingester.RaceProps.Humanlike && !ingester.def.defName.Contains("Neko");
      if (flag)
      {
        bool flag2 = Rand.Value < NekoPrepared && !ingester.kindDef.race.Equals(NekoDefOf.Alien_Neko);
        if (flag2)
        {
          ingester.needs.mood.thoughts.memories.TryGainMemory(ThoughtDef.Named("NekoFoodOnHuman"), null);
        }
      }
    }

    private float nekoPrepared;
  }
}
