//(c) copyright by Martin M. Klöckener

using UnityEngine;
using Wichtel.StateMachine;

namespace Doodlenite {
public class PlayerAnimationState_WalkingLeft : IState
{
    public PlayerAnimations anims;
    public float walkingSpeed;
    
    public PlayerAnimationState_WalkingLeft(PlayerAnimations anims) => this.anims = anims;

    
    public void OnEnter()
    {
        anims.PlayWalkingLeftAnimation();
    }

    public void Tick()
    {
        anims.SetAnimationSpeed(walkingSpeed);
    }

    public void OnExit()
    {
        anims.SetAnimationSpeed(1f);
    }
}
}