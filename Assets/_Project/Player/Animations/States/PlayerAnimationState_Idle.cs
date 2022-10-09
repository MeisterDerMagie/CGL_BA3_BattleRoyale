//(c) copyright by Martin M. Klöckener

using UnityEngine;
using Wichtel.StateMachine;

namespace Doodlenite {
public class PlayerAnimationState_Idle : IState
{
    public PlayerAnimations anims;
    public bool playLandingAnimation;
    public PlayerAnimationState_Idle(PlayerAnimations anims) => this.anims = anims;
    
    public void OnEnter()
    {
        if (playLandingAnimation)
            anims.PlayLandingAnimation();
        else
            anims.PlayIdleAnimation();

        playLandingAnimation = false;
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
    }
}
}