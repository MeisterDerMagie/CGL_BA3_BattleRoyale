//(c) copyright by Martin M. Klöckener
using System;
using UnityEngine;

namespace Doodlenite {
public class KeyboardInput : InputHandler
{
    [SerializeField] private KeyboardMap keyboardMap;
    [SerializeField] private Player player;
    [SerializeField] private Axis axis;
    private Command<Player> jumpCommand, moveHorizontallyCommand;
    
    private void Start()
    {
        axis = new Axis();
        
        jumpCommand = new JumpCommand(player);
        moveHorizontallyCommand = new MoveHorizontallyCommand(player, axis);
    }

    public override void HandleInput()
    {
        //movement
        axis.axis = keyboardMap.Axis;
        moveHorizontallyCommand.Execute();

        //jumping
        if(keyboardMap.JumpKey) jumpCommand.Execute();
    }
}
}