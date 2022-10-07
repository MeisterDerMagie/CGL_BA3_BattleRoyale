using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerMainMenuInteraction : MonoBehaviour
{
    [SerializeField] private PlayerAnimations anim;
    [SerializeField] private AudioSourceRandomizer ouchSfx, deathSfx, hitSfx;
    
    [SerializeField] private int numberOfClicksUntilDeath;
    [SerializeField, ReadOnly] private int numberOfClicks;
    
    
    private void OnMouseDown()
    {
        numberOfClicks++;

        //if the player clicked less than four times: play a small animation
        if (numberOfClicks < numberOfClicksUntilDeath)
        {
            anim.PlayLandingAnimation();
            ouchSfx.Play();
        }
        //if the player clicks for the fourth time
        else if (numberOfClicks == numberOfClicksUntilDeath)
        {
            anim.PlayDeathAnimation();
            deathSfx.Play();
        }
        //if the player keeps klicking after death, play hit sound
        else
        {
            hitSfx.Play();
        }
    }
}
