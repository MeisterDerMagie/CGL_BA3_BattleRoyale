using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Doodlenite {
public class PlayerPointerManager : MonoBehaviour
{
    [SerializeField] private PlayerPointer[] topPointers, bottomPointers;
    [SerializeField] private Transform gooTop;
    [SerializeField] private float distanceForDanger1, distanceForDanger2;
    
    private Dictionary<Player, (PlayerPointer upperPointer, PlayerPointer lowerPointer)> activePointers = new Dictionary<Player, (PlayerPointer upperPointer, PlayerPointer lowerPointer)>();
    private int index = 0;
    
    private static PlayerPointerManager instance;
    public static PlayerPointerManager Instance => instance;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        
        foreach (PlayerPointer topPointer in topPointers)
        {
            topPointer.gameObject.SetActive(false);
        }

        foreach (PlayerPointer bottomPointer in bottomPointers)
        {
            bottomPointer.gameObject.SetActive(false);
        }
    }

    public void RegisterPlayer(Player player)
    {
        if (activePointers.ContainsKey(player)) return;

        PlayerPointer topPointer = topPointers[index];
        PlayerPointer bottomPointer = bottomPointers[index];

        activePointers.Add(player, (topPointer, bottomPointer));
        topPointer.SetPlayer(player);
        bottomPointer.SetPlayer(player);
        
        index += 1;
    }

    private void Update()
    {
        if (Camera.main == null) return;

        float cameraHeight = Camera.main.orthographicSize * 2.0f;
        float cameraLowerBorderY = Camera.main.transform.position.y - cameraHeight / 2f;
        float cameraUpperBorderY = Camera.main.transform.position.y + cameraHeight / 2f;

        foreach ((Player player, (PlayerPointer upperPointer, PlayerPointer lowerPointer) pointers) in activePointers)
        {
            PlayerPointer lowerPointer = pointers.lowerPointer;
            PlayerPointer upperPointer = pointers.upperPointer;
            
            //hide if player is null
            if (player == null)
            {
                lowerPointer.gameObject.SetActive(false);
                upperPointer.gameObject.SetActive(false);
                continue;
            }
            
            float playerYPos = player.transform.position.y;
            
            float distanceToGoo = playerYPos - gooTop.position.y;

            bool isBelowScreen = playerYPos + 3 < cameraLowerBorderY; //+3 -> hardcoded player height
            bool isAboveScreen = playerYPos > cameraUpperBorderY;

            //hide both pointers if player is on the screen
            if (!isBelowScreen && !isAboveScreen)
            {
                lowerPointer.gameObject.SetActive(false);
                upperPointer.gameObject.SetActive(false);
                continue;
            }
            
            //show lower pointer if below screen
            if (isBelowScreen)
            {
                upperPointer.gameObject.SetActive(false);
                
                if(!lowerPointer.gameObject.activeSelf)
                    lowerPointer.gameObject.SetActive(true);
                
                //set values
                if(!player.isAlive)
                    lowerPointer.SetDead();
                
                else if(distanceToGoo <= distanceForDanger2)
                    lowerPointer.SetDanger2();
                
                else if (distanceToGoo <= distanceForDanger1)
                    lowerPointer.SetDanger1();
                else
                    lowerPointer.SetNoDangerAndAlive();
            }
            
            //show upper pointer if above screen
            if (isAboveScreen)
            {
                lowerPointer.gameObject.SetActive(false);
                
                if(!upperPointer.gameObject.activeSelf)
                    upperPointer.gameObject.SetActive(true);
                
                //set values
                if(!player.isAlive)
                    upperPointer.SetDead();
                
                else if(distanceToGoo <= distanceForDanger2)
                    upperPointer.SetDanger2();
                
                else if (distanceToGoo <= distanceForDanger1)
                    upperPointer.SetDanger1();
                else
                    upperPointer.SetNoDangerAndAlive();
            }
            
            //update xPos
            lowerPointer.UpdateXPos();
            upperPointer.UpdateXPos();
        }
    }
}
}
