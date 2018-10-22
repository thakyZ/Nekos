using Verse;
using Verse.AI;

namespace Neko
{
  public class ThinkNode_ConditionalIsNeko : ThinkNode_Conditional
  {
    protected override bool Satisfied(Pawn pawn) => pawn.def == NekoDefOf.Alien_Neko;
  }
}
