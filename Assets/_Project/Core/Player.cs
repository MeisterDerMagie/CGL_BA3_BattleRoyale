//(c) copyright by Martin M. Klöckener
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Doodlenite {
public class Player : NetworkBehaviour
{
    public string playerName;
    public Color playerColor;
    
    public PlayerAnimations anim;
    public PlayerMovement playerMovement;
}
}