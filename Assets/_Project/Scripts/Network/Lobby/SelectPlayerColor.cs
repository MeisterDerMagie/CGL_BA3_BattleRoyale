//(c) copyright by Martin M. Klöckener
using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Doodlenite {
[RequireComponent(typeof(Image))]
public class SelectPlayerColor: MonoBehaviour
{
    [SerializeField] private Image colorPickerImage;
    [SerializeField] private NetworkRoomPlayer roomPlayer;
    [SerializeField] private SpriteRenderer playerColorSprite;
    
    public void SetPlayerColor()
    {
        EventSystem.Instance.Fire("LOBBY", "playerColorChanged", new PlayerColorLink(roomPlayer.index, colorPickerImage.color));
    }

    private void Start()
    {
        EventSystem.Instance.AddEventListener("LOBBY", OnPlayerColorChanged);
    }

    private void OnPlayerColorChanged(string eventName, object param = null)
    {
        if(eventName == "playerColorChanged")
            if (((PlayerColorLink)param).playerIndex == roomPlayer.index)
                playerColorSprite.color = ((PlayerColorLink)param).playerColor;
    }

    private void OnValidate()
    {
        colorPickerImage = GetComponent<Image>();
    }

    public struct PlayerColorLink
    {
        public int playerIndex;
        public Color playerColor;

        public PlayerColorLink(int _playerIndex, Color _playerColor)
        {
            playerIndex = _playerIndex;
            playerColor = _playerColor;
        }
    }
}
}