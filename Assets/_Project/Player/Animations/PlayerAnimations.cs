using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SkeletonAnimation))]
public class PlayerAnimations : MonoBehaviour
{
    private float animationSpeed = 1f;
    
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
        TrackEntry trackEntry = anim.AnimationState.SetAnimation(0, "idle", true);
        //start idle animation at random time
        trackEntry.TrackTime = Random.Range(0f, trackEntry.AnimationEnd);
        
        //idle face animation
        trackEntry = anim.AnimationState.SetAnimation(1, "idleFace", true);
        //start face animation at random time
        trackEntry.TrackTime = Random.Range(0f, trackEntry.AnimationEnd);
        
        //blinking animation
        trackEntry = anim.AnimationState.SetAnimation(2, "blink", true);
        //start blinking animation at random time
        trackEntry.TrackTime = Random.Range(0f, trackEntry.AnimationEnd);
    }

    [Button]
    public void PlayJumpAnimation()
    {
        StopIdleFaceAnimations();
        //anim.AnimationState.SetAnimation(0, "jump", false);
        anim.AnimationState.SetAnimation(0, "inAirRising", true);
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
        //stop blinking
        anim.AnimationState.ClearTrack(2);
    }

    [Button]
    public void PlayDeadPokeAnimation()
    {
        StopIdleFaceAnimations();
        anim.AnimationState.SetAnimation(0, "deadPoking", false);
    }
    private void StopIdleFaceAnimations()
    {
        anim.AnimationState.ClearTrack(1);
    }

    public void SetAnimationSpeed(float speed)
    {
        animationSpeed = speed;
        anim.AnimationState.TimeScale = animationSpeed;
    }
    
    #if UNITY_EDITOR
    private void OnValidate() => anim = GetComponent<SkeletonAnimation>();
    #endif
}
