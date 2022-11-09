//(c) copyright by Martin M. Klöckener
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Wichtel.Extensions;

namespace Doodlenite {
public class CopyLobbyCodeToClipboard : MonoBehaviour
{
    [SerializeField] private DOTweenAnimation animation;
    
    public void Copy()
    {
        FindObjectOfType<NetworkRoomManagerExt>().lobbyCode.CopyToClipboard();
        animation.DORestart();
    }
}
}