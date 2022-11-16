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
    
    private Dictionary<Player, (PlayerPointer upperPointer, PlayerPointer lowerPointer)> pointers = new Dictionary<Player, (PlayerPointer upperPointer, PlayerPointer lowerPointer)>();
    private int index = 0;
    
    private Camera cam;

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

        cam = Camera.main;
    }

    public void RegisterPlayer(Player player)
    {
        if (pointers.ContainsKey(player)) return;

        PlayerPointer topPointer = topPointers[index];
        PlayerPointer bottomPointer = bottomPointers[index];
        
        pointers.Add(player, (topPointer, bottomPointer));
        topPointer.SetPlayer(player);
        bottomPointer.SetPlayer(player);
        
        index += 1;
    }

    private void Update()
    {
        //This doesn't work. Disable it until the bug is solved
        return;
        
        if(cam == null) return;

        float cameraHeight = cam.orthographicSize * 2.0f;
        float cameraLowerBorderY = cam.transform.position.y - cameraHeight / 2f;
        float cameraUpperBorderY = cam.transform.position.y + cameraHeight / 2f;
        
        foreach (var pointer in pointers)
        {
            Player player = pointer.Key;
            PlayerPointer lowerPointer = pointer.Value.lowerPointer;
            PlayerPointer upperPointer = pointer.Value.upperPointer;
            
            //hide if player is null
            if (player == null)
            {
                pointer.Value.lowerPointer.gameObject.SetActive(false);
                pointer.Value.upperPointer.gameObject.SetActive(false);
                return;
            }
            
            float playerYPos = player.transform.position.y;
            
            float distanceToGoo = playerYPos - gooTop.position.y;

            Debug.Log($"playerYPos = {playerYPos}, cameraLowerBorderY = {cameraLowerBorderY}, cameraUpperBorder = {cameraUpperBorderY}");
            
            bool isBelowScreen = playerYPos < cameraLowerBorderY;
            bool isAboveScreen = playerYPos > cameraUpperBorderY;
            
            //hide both pointers if player is on the screen
            if (!isBelowScreen && !isAboveScreen)
            {
                lowerPointer.gameObject.SetActive(false);
                upperPointer.gameObject.SetActive(false);
                return;
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
