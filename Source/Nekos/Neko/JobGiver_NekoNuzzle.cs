using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace Neko
{
  internal class JobGiver_NekoNuzzle : ThinkNode_JobGiver
  {
    protected override Job TryGiveJob(Pawn pawn)
    {
      bool flag = pawn.RaceProps.nuzzleMtbHours <= 0f;
      Job result;
      if (!flag)
      {
        List<Pawn> source = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
        bool flag2 = !GenCollection.TryRandomElement<Pawn>(from p in source
                                                           where p.RaceProps.Humanlike && p.Position.InHorDistOf(pawn.Position, 15f) && RegionAndRoomQuery.GetRoom(pawn, RegionType.Set_Passable) == RegionAndRoomQuery.GetRoom(p, RegionType.Set_Passable) && !ForbidUtility.IsForbidden(p.Position, pawn) && PawnUtility.CanCasuallyInteractNow(p, false) && p != pawn && p.relations.OpinionOf(pawn) > 30
                                                           select p, out Pawn pawn2);
        if (flag2)
        {
          result = null;
        }
        else
        {
          result = new Job(JobDefOf.Nuzzle, pawn2)
          {
            locomotionUrgency = LocomotionUrgency.Walk,
            expiryInterval = 3000
          };
        }
      }
      else
      {
        result = null;
      }
      return result;
    }
  }
}
