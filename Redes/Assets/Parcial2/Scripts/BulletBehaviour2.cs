using Fusion;
using UnityEngine;

public class BulletBehaviour2 : NetworkBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _damage;
    [SerializeField] PlayerBehaviour2 _owner;
    [SerializeField] Rigidbody2D _rb;

    public override void Spawned()
    {
        base.Spawned();
    }

    public override void FixedUpdateNetwork()
    {
        Movement();
    }

    void Movement()
    {
        var dir = transform.up * _speed * Runner.DeltaTime;
        var newdir = new Vector2(dir.x, dir.y);
        _rb.position += newdir;     
    }

    public void SetDirection(Vector3 dir)
    {
        var newDir = dir - transform.position;

        newDir.z = 0;

        transform.up = newDir;
    }

    public void SetOwner(PlayerBehaviour2 owner)
    {
        _owner = owner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!HasStateAuthority) return;

        if (collision.GetComponent<PlayerBehaviour2>())
        {
            var hit = collision.GetComponent<PlayerBehaviour2>();

            if (hit == _owner)
            {
                //print("owner");
            }
            else
            {
                Runner.Despawn(Object);
                hit.RPC_GetDamage(_damage);
                print("hit player");
            }
        }
        else if (collision.GetComponent<Cudini>())
        {
            collision.GetComponent<Cudini>().DeActivate();
            Runner.Despawn(Object);
        }
        else
        {
            //print("choco con " +  collision.name);
            Runner.Despawn(Object);
        }

    }
}
