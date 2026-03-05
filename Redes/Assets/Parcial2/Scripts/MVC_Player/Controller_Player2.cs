using UnityEngine;
using UnityEngine.Windows;
using static Unity.Collections.Unicode;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
            return;
        }

        if (inputs.YAxis != 0)
        {
            Movement(inputs.YAxis);
        }
        else
        {
            Still();
        }

        if (inputs.Buttons.IsSet(ButtonTypes.Jump) && !inputs.Buttons.IsSet(ButtonTypes.Pound))
        {
            ShieldUp();
        }

        if (inputs.Buttons.IsSet(ButtonTypes.Pound))
        {
            //Pound();
        }

        if (inputs.Buttons.IsSet(ButtonTypes.MouseButton0))
        {
            _playerScript.InstantiateBullet(inputs.MousePosition);
        }


        if (inputs.XAxis < 0)
        {
            _playerScript.SpriteRenderer.flipX = true;
        }
        else if (inputs.XAxis > 0)
        {
            _playerScript.SpriteRenderer.flipX = false;
        }

    }
    #endregion

    public void Movement(float inputY)
    {
        if (inputY == 0) return;
        _playerScript.SetCaminandoAnim();
        _playerScript.Rb.velocity = new Vector2(0, inputY * _playerScript.Speed);
    }

    public void ShieldUp()
    {
        _playerScript.SpawnShield();
    }

    public void Still()
    {
        if (!_playerScript.Anim.Animator.GetBool("Idle") && !_playerScript.Anim.Animator.GetBool("Cayendo"))
        {
            _playerScript.SetIdleAnim();
        }
        _playerScript.Rb.velocity = new Vector2(0, _playerScript.Rb.velocity.y);
    }

    //public void Pound()
    //{
    //    if (_playerScript.IsGrounded) return;

    //    _playerScript.SetCayendoAnim();
    //    _playerScript.Rb.velocity += (-Vector2.up * _playerScript.PoundForce * _playerScript.Runner.DeltaTime);
    //}

}
