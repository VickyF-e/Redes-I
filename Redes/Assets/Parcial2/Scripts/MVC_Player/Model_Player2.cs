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
        //_playerScript.SetGroundedFalse();

        _playerScript.SetSaltandoAnim();

        //if (_playerScript.JumpsLeft > 0)
        //{
        //    _playerScript.ReduceJump();
        //    _playerScript.Rb.velocity = new Vector2(_playerScript.Rb.velocity.x, 0);
        //    _playerScript.Rb.velocity += (Vector2.up * _playerScript.JumpForce);
        //}
    }

    public void Still()
    {
        if (!_playerScript.Anim.Animator.GetBool("Idle") && !_playerScript.Anim.Animator.GetBool("Cayendo"))
        {
            _playerScript.SetIdleAnim();
        }
        _playerScript.Rb.velocity = new Vector2(0, _playerScript.Rb.velocity.y);
    }

    public void Pound()
    {
        //if (_playerScript.IsGrounded) return;

        //_playerScript.SetCayendoAnim();
        //_playerScript.Rb.velocity += (-Vector2.up * _playerScript.PoundForce * _playerScript.Runner.DeltaTime);
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
