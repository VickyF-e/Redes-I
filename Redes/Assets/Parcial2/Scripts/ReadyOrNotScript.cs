using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadyOrNotScript : MonoBehaviour
{

    public Canvas _readyCanvas;
    public TextMeshProUGUI _textReady;
    public Button _readyButton;
    public CursorManager2 _cursorManager;
    public PlayerBehaviour2 _player;

    public bool Ready = false;

    private void Start()
    {
        _readyButton.onClick.AddListener(ReadyClick);
    }


    public void ReadyClick()
    {
        if (!Ready)
        {
            _textReady.text = "Listoooos";
            Ready = true;
            _player.RPCSetBoolReady(true);
        }
        else
        {
            _textReady.text = "No Listos :(";
            Ready = false;
            _player.RPCSetBoolReady(false);
        }
    }

    public void SetMyPlayer(PlayerBehaviour2 player)
    {
        _player = player;
    }

}
