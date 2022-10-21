using System;
using UnityEngine;

namespace Mirror
{
    /// <summary>Start position for player spawning, automatically registers itself in the NetworkManager.</summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/Network Room Start Position")]
    public class NetworkRoomStartPosition : MonoBehaviour
    {
        public int index;
        
        #if UNITY_EDITOR
        private void OnValidate() => index = transform.GetSiblingIndex();
        #endif
    }
}
