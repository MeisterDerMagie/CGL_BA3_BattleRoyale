//(c) copyright by Martin M. Klöckener

using UnityEngine;
using Wichtel.StateMachine;

namespace Doodlenite {
public class PlayerAnimationState_InAirRising : IState
{
    public PlayerAnimations anims;
    public PlayerAnimationState_InAirRising(PlayerAnimations anims) => this.anims = anims;

    public void OnEnter()
    {
        anims.PlayJumpAnimation();
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
    }
}
}