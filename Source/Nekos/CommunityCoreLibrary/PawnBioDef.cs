using System;
using AlienRace;
using RimWorld;
using UnityEngine;
using Verse;

namespace CommunityCoreLibrary
{
	// Token: 0x02000004 RID: 4
	public class PawnBioDef : Def
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000022E4 File Offset: 0x000004E4
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			PawnBio pawnBio = new PawnBio
			{
				gender = this.gender
			};
			bool flag = !GenText.NullOrEmpty(this.firstName) && !GenText.NullOrEmpty(this.lastName);
			if (flag)
			{
				pawnBio.name = new NameTriple(this.firstName, this.nickName, this.lastName);
				Backstory backstory;
				BackstoryDatabase.TryGetWithIdentifier(this.childhoodDef.defName, out backstory, true);
				bool flag2 = backstory != null;
				if (flag2)
				{
					pawnBio.childhood = backstory;
					pawnBio.childhood.shuffleable = false;
					pawnBio.childhood.slot = 0;
					Backstory backstory2;
					BackstoryDatabase.TryGetWithIdentifier(this.adulthoodDef.defName, out backstory2, true);
					bool flag3 = backstory2 != null;
					if (flag3)
					{
						pawnBio.adulthood = backstory2;
						pawnBio.adulthood.shuffleable = false;
						pawnBio.adulthood.slot = BackstorySlot.Adulthood;
						pawnBio.name.ResolveMissingPieces(null);
						bool flag4 = false;
						foreach (string str in pawnBio.ConfigErrors())
						{
							bool flag5 = !flag4;
							if (flag5)
							{
								flag4 = true;
								Debug.LogError("Config error(s) in PawnBioDef " + this.defName + ". Skipping...Backstories");
							}
							Debug.LogError(str + "Backstories");
						}
						bool flag6 = flag4;
						if (!flag6)
						{
							bool flag7 = !SolidBioDatabase.allBios.Contains(pawnBio);
							if (flag7)
							{
								SolidBioDatabase.allBios.Add(pawnBio);
							}
						}
					}
					else
					{
						Debug.LogError("PawnBio with defName: " + this.defName + " has null adulthood. It will not be added.Backstories");
					}
				}
				else
				{
					Debug.LogError("PawnBio with defName: " + this.defName + " has null childhood. It will not be added.Backstories");
				}
			}
			else
			{
				Debug.LogError("PawnBio with defName: " + this.defName + " has empty first or last name. It will not be added.Backstories");
			}
		}

		// Token: 0x04000003 RID: 3
		public BackstoryDef childhoodDef;

		// Token: 0x04000004 RID: 4
		public BackstoryDef adulthoodDef;

		// Token: 0x04000005 RID: 5
		public string firstName;

		// Token: 0x04000006 RID: 6
		public string nickName = "";

		// Token: 0x04000007 RID: 7
		public string lastName;

		// Token: 0x04000008 RID: 8
		public GenderPossibility gender = GenderPossibility.Either;
	}
}
