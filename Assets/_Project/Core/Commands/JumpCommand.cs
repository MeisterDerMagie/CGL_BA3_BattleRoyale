//(c) copyright by Martin M. Klöckener

namespace Doodlenite {
public class JumpCommand : Command<Player>
{
    public JumpCommand(Player target) : base(target) { }

    public override void Execute()
    {
        target.playerMovement.Jump();
        //target.anim.PlayJumpAnimation();
    }
}
}