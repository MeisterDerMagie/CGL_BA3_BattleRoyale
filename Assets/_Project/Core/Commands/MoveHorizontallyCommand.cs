//(c) copyright by Martin M. Klöckener

namespace Doodlenite {
public class MoveHorizontallyCommand : Command<Player>
{
    public Axis axis;
    
    public MoveHorizontallyCommand(Player target, Axis axis) : base(target)
    {
        this.axis = axis;
    }

    public override void Execute()
    {
        target.playerMovement.MoveHorizontally(axis.axis);
        target.anim.PlayJumpAnimation();
    }
}

[System.Serializable]
public class Axis
{
    public float axis;
}
}