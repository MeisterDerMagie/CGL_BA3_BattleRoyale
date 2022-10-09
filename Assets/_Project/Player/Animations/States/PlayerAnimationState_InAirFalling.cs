//(c) copyright by Martin M. Klöckener

using UnityEngine;
using Wichtel.StateMachine;

namespace Doodlenite {
public class PlayerAnimationState_InAirFalling : IState
{
    public PlayerAnimations anims;
    public PlayerAnimationState_InAirFalling(PlayerAnimations anims) => this.anims = anims;
    
    public void OnEnter()
    {
        anims.PlayFallingAnimation();
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
    }
}
}