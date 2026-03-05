using UnityEngine;

public class Controller_Player2
{
    PlayerBehaviour2 _playerScript;

    public Controller_Player2(PlayerBehaviour2 playerScript)
    {
        _playerScript = playerScript;
    }

    #region Fakes

    public void FakeFixedUpdate()
    {
        if (!_playerScript.GetInput(out NetworkInputData inputs))
        {
            Still();
            Debug.Log("still");
            return;
        }

        //if (_playerScript.GetInput(out NetworkInputData inputs))
        //{
        //    if (inputs.YAxis == 0 && inputs.XAxis == 0 && !inputs.Buttons.IsSet(ButtonTypes.Jump) && !inputs.Buttons.IsSet(ButtonTypes.Pound) && !inputs.Buttons.IsSet(ButtonTypes.MouseButton0))
        //    {
        //        Still();
        //        Debug.Log("still");
        //        return;
        //    }
        //}

        Movement(inputs.XAxis);

        if (inputs.Buttons.IsSet(ButtonTypes.Jump) && !inputs.Buttons.IsSet(ButtonTypes.Pound))
        {
            Jump();
        }

        if (inputs.Buttons.IsSet(ButtonTypes.Pound))
        {
            Pound();
        }

        if (inputs.Buttons.IsSet(ButtonTypes.MouseButton0))
        {
            _playerScript.InstantiateBullet(inputs.MousePosition);
            //_playerScript.SetDisparoAnim();
        }


        if (inputs.XAxis < 0)
        {
            _playerScript.SpriteRenderer.flipX = true;
        }
        else if (inputs.XAxis > 0)
        {
            _playerScript.SpriteRenderer.flipX = false;
        }

        //if (_playerScript.Runner.LocalPlayer == _playerScript.Object.InputAuthority)
        //{
        //    if (Mathf.Abs(inputs.XAxis) > 0.01f)
        //        _playerScript.Anim.SetTrigger("WalkLocal");
        //    else
        //        _playerScript.Anim.SetTrigger("IdleLocal");
        //}

    }

    public void FakeOnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            _playerScript.GroundTouched();
        }
    }

    public void FakeOnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            _playerScript.SetGroundedFalse();
        }
    }
    #endregion

    public void Movement(float inputX)
    {
        if (inputX <= 0) return;
        _playerScript.SetCaminandoAnim();
        _playerScript.Rb.velocity = new Vector2(inputX * _playerScript.Speed, _playerScript.Rb.velocity.y);

        //if (_playerScript.Runner.LocalPlayer == _playerScript.Object.InputAuthority)
        //{
        //    if (Mathf.Abs(inputX) > 0.01f)
        //        _playerScript.Anim.SetTrigger("WalkLocal");
        //    else
        //        _playerScript.Anim.SetTrigger("IdleLocal");
        //}
    }

    public void Jump()
    {
        _playerScript.SetGroundedFalse();

        _playerScript.SetSaltandoAnim();

        if (_playerScript.JumpsLeft > 0)
        {
            _playerScript.ReduceJump();
            _playerScript.Rb.velocity = new Vector2(_playerScript.Rb.velocity.x, 0);
            _playerScript.Rb.velocity += (Vector2.up * _playerScript.JumpForce);
            //----------------------------------------------------------------------------------------------------------------------------------
            _playerScript._audioSourceJump.PlayOneShot(_playerScript._audioClipJump);
        }
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
        if (_playerScript.IsGrounded) return;

        _playerScript.SetCayendoAnim();
        _playerScript.Rb.velocity += (-Vector2.up * _playerScript.PoundForce * _playerScript.Runner.DeltaTime);
    }

}
