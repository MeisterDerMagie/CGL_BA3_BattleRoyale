using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(SkeletonAnimation))]
public class PlayerAnimations : MonoBehaviour
{
    [SerializeField, HideInInspector] private SkeletonAnimation anim;
    
    void Start()
    {
        //start with idle animation
        PlayIdleAnimation();
    }

    [Button]
    public void PlayIdleAnimation()
    {
        //base idle animation
        anim.AnimationState.SetAnimation(0, "idle", true);
        //idle face animation
        anim.AnimationState.SetAnimation(1, "idleFace", true);
        //blinking animation
        anim.AnimationState.SetAnimation(2, "blink", true);
    }

    [Button]
    public void PlayJumpAnimation()
    {
        StopIdleFaceAnimations();
        anim.AnimationState.SetAnimation(0, "jump", false);
        anim.AnimationState.AddAnimation(0, "inAirRising", true, 0);
    }

    [Button]
    public void PlayFallingAnimation()
    {
        StopIdleFaceAnimations();
        anim.AnimationState.SetAnimation(0, "inAirFalling", true);
    }

    [Button]
    public void PlayLandingAnimation()
    {
        anim.AnimationState.SetAnimation(0, "landing", false);
        
        //start playing idle animation
        //base idle animation
        anim.AnimationState.AddAnimation(0, "idle", true, 0);
        //idle face animation
        anim.AnimationState.AddAnimation(1, "idleFace", true, 0);
        //blinking animation
        anim.AnimationState.AddAnimation(2, "blink", true, 0);
    }

    [Button]
    public void PlayWalkingLeftAnimation()
    {
        StopIdleFaceAnimations();
        anim.AnimationState.SetAnimation(0, "walking_l", true);
    }
    
    [Button]
    public void PlayWalkingRightAnimation()
    {
        StopIdleFaceAnimations();
        anim.AnimationState.SetAnimation(0, "walking_r", true);
    }
    
    [Button]
    public void PlayDeathAnimation()
    {
        StopIdleFaceAnimations();
        anim.AnimationState.SetAnimation(0, "dying", false);
        anim.AnimationState.AddAnimation(0, "dead", true, 0); 
        //stop blinking
        anim.AnimationState.ClearTrack(2);
    }

    private void StopIdleFaceAnimations()
    {
        anim.AnimationState.ClearTrack(1);
    }
    
    #if UNITY_EDITOR
    private void OnValidate() => anim = GetComponent<SkeletonAnimation>();
    #endif
}