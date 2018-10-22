using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace Neko
{
	// Token: 0x0200000B RID: 11
	public class IncidentWorker_PortalRaid : IncidentWorker_RaidEnemy
	{
		// Token: 0x06000022 RID: 34 RVA: 0x00002A74 File Offset: 0x00000C74
		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return base.FactionCanBeGroupSource(f, map, desperate) && f.def == NekoDefOf.Neko;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002A94 File Offset: 0x00000C94
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			bool result;
			if (base.CanFireNowSub(parms))
			{
				result = Find.FactionManager.AllFactionsVisible.Any((Faction x) => x.def.defName.Equals("Neko") && FactionUtility.HostileTo(x, Faction.OfPlayer));
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002AE0 File Offset: 0x00000CE0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			this.ResolveRaidPoints(parms);
			bool flag = !this.TryResolveRaidFaction(parms);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.ResolveRaidStrategy(parms, PawnGroupKindDefOf.Combat);
				parms.raidArrivalMode = PawnsArrivalModeDefOf.CenterDrop;
				this.ResolveRaidPoints(parms);
				PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, parms, false);
				List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
				bool flag2 = list.Count == 0;
				if (flag2)
				{
					Log.Error("Got no pawns spawning raid from parms " + parms, false);
					result = false;
				}
				else
				{
					TargetInfo targetInfo;
					targetInfo..ctor(parms.spawnCenter, map, false);
					Thing thing = GenSpawn.Spawn(NekoDefOf.NekoPortal, targetInfo.Cell, map, 0);
					MoteMaker.ThrowHeatGlow(targetInfo.Cell, map, 5f);
					MoteMaker.ThrowHeatGlow(targetInfo.Cell, map, 5f);
					MoteMaker.ThrowFireGlow(targetInfo.Cell, map, 5f);
					MoteMaker.ThrowFireGlow(targetInfo.Cell, map, 5f);
					MoteMaker.ThrowMetaPuffs(targetInfo);
					MoteMaker.ThrowMetaPuffs(targetInfo);
					foreach (Pawn pawn in list)
					{
						GenSpawn.Spawn(pawn, targetInfo.Cell, map, 0);
						targetInfo = pawn;
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Points = " + parms.points.ToString("F0"));
					foreach (Pawn pawn2 in list)
					{
						string str = (pawn2.equipment == null || pawn2.equipment.Primary == null) ? "unarmed" : pawn2.equipment.Primary.LabelCap;
						stringBuilder.AppendLine(pawn2.KindLabel + " - " + str);
					}
					string letterLabel = this.GetLetterLabel(parms);
					string letterText = this.GetLetterText(parms, list);
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref letterLabel, ref letterText, this.GetRelatedPawnsInfoLetterText(parms), true, true);
					parms.raidStrategy.Worker.MakeLords(parms, list);
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.EquippingWeapons, 2);
					bool flag3 = !PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.ShieldBelts);
					if (flag3)
					{
						for (int i = 0; i < list.Count; i++)
						{
							Pawn pawn3 = list[i];
							bool flag4 = GenCollection.Any<Apparel>(pawn3.apparel.WornApparel, (Apparel ap) => ap is ShieldBelt);
							if (flag4)
							{
								LessonAutoActivator.TeachOpportunity(ConceptDefOf.ShieldBelts, 2);
								break;
							}
						}
					}
					Find.TickManager.slower.SignalForceNormalSpeedShort();
					Find.StoryWatcher.statsRecord.numRaidsEnemy++;
					result = true;
				}
			}
			return result;
		}
	}
}
