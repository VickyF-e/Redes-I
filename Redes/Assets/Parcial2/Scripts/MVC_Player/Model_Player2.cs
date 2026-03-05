using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Player2
{
    PlayerBehaviour2 _playerScript;

    public Model_Player2(PlayerBehaviour2 playerScript)
    {
        _playerScript = playerScript;
    }

    public void Movement()
    {
        _playerScript.SetCaminandoAnim();

        _playerScript.Rb.velocity = new Vector2(_playerScript.InputDirX * _playerScript.Speed, _playerScript.Rb.velocity.y);

    }

    public void Jump()
    {

    }

    public void Still()
    {

    }

    public void Pound()
    {

    }

    public void FlipX()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _playerScript.SpriteRenderer.flipX = true;
        }
        else
        {
            _playerScript.SpriteRenderer.flipX = false;
        }
    }

}
