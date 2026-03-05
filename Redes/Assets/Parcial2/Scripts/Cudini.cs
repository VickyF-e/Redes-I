using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cudini : NetworkBehaviour
{
    public Transform padre;
    public override void Spawned()
    {
        base.Spawned();
        //Object.gameObject.SetActive(false);
    }

    public void SetPadre(Transform t)
    {
        padre = t;
    }

    private void Update()
    {
        if (padre == null) return;
        transform.position = padre.position;
    }

    public void Activate()
    {
        //Object.gameObject.SetActive(true);
    }
    public void DeActivate()
    {
        Object.gameObject.SetActive(false);
        Runner.Despawn(Object);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerBehaviour2>())
        {
            padre = collision.GetComponent<PlayerBehaviour2>().transform;
        }
    }

}
