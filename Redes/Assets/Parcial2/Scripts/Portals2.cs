using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portals2 : MonoBehaviour
{
    [SerializeField] Transform _counterTp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.TryGetComponent(out PlayerBehaviour2 _playerScript) && !_playerScript.HasJustBeenTp)
        //{
        //    _playerScript.TPed();
        //    _playerScript.transform.position = _counterTp.position;
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.TryGetComponent(out PlayerBehaviour2 _playerScript) && _playerScript.HasJustBeenTp)
        //{
        //    _playerScript.TPedOff();
        //}
    }
}
