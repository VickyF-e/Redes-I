using Fusion;
using System;
using System.Collections;
using UnityEngine;


public class PlayerBehaviour2 : NetworkBehaviour, IPlayerJoined
{
    [SerializeField] Material _orangeMaterial;
    [SerializeField] Material _blueMaterial;

    [Networked, OnChangedRender(nameof(SelectMaterial))] PlayerTeam SelectedTeam { get; set; }

    Controller_Player2 _controller;

    [SerializeField] int _hp;
    [Networked, OnChangedRender(nameof(LifeUpdated))] int Hp { get; set; }

    public event Action<float> OnLifeUpdate;

    [SerializeField] int _maxHp;
    public int MaxHp { get { return _maxHp; } private set { _maxHp = value; } }

    [SerializeField] float _speed;
    public float Speed { get { return _speed; } private set { _speed = value; } }

    [SerializeField] Rigidbody2D _rb;
    public Rigidbody2D Rb { get { return _rb; } private set { _rb = value; } }

    [NonSerialized] public float InputDirX, InputDirY;

    [SerializeField] SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } private set { _spriteRenderer = value; } }

    [SerializeField] NetworkMecanimAnimator _anim;
    public NetworkMecanimAnimator Anim { get { return _anim; } private set { _anim = value; } }

    [SerializeField] PlayerTeam _team;
    public PlayerTeam Team { get { return _team; } private set { _team = value; } }

    [SerializeField] BulletBehaviour2 _bulletPrefab;
    public BulletBehaviour2 BulletPrefab { get { return _bulletPrefab; } private set { _bulletPrefab = value; } }

    [Networked]
    public NetworkBool CanPlay { get; set; }

    public event Action OnDespawn;

    WeaponBehaviour _weaponBehaviour;

    [Networked]
    public NetworkBool IsReady { get; set; }

    public AudioSource _audioSourceJump, _audioSourceDano, _audioSourcePound;
    public AudioClip _audioClipJump, _audioClipDano, _audioClipPound;

    public NetworkPrefabRef shield; 


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPCSetBoolReady(bool mode)
    {
        IsReady = mode;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPCSetCanPlay(bool value)
    {
        CanPlay = value;
    }

    void SelectMaterial()
    {
        if (SelectedTeam == PlayerTeam.Orange)
        {
            SpriteRenderer.material = _orangeMaterial;
            SpriteRenderer.flipX = true;
            return;
        }

        SpriteRenderer.material = _blueMaterial;
    }

    public override void Spawned()
    {
        LifeBarManager2.Instance.CreateNewBar(this);

        Anim = GetComponent<NetworkMecanimAnimator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Rb = GetComponent<Rigidbody2D>();

        _controller = new(this);

        SetMaxVariable();

        GameManager2.Instance.AddToUsersList(this);

        GameManager2.Instance.OnGameEnded += PlayerCanNotStart;

        _weaponBehaviour = GetComponent<WeaponBehaviour>();

        SelectMaterial();

        if (HasInputAuthority)
        {
            var set = FindObjectOfType<ReadyOrNotScript>();
            if (set != null)
            {
                set.SetMyPlayer(this);
            }
        }

        LobbyManager.instance.JointTheList(this);
    }

    public void PlayerJoined(PlayerRef player)
    {
        //if (Runner.SessionInfo.PlayerCount >= GameManager2.Instance.MinPlayerRequiredToStart)
        //{
        //    _canPlay = true;
        //}
    }

    private void Update()
    {
        _hp = Hp;
    }

    public override void FixedUpdateNetwork()
    {

        if (!CanPlay) return;

        _controller.FakeFixedUpdate();

        if (HasStateAuthority)
        {

            if (GetInput(out NetworkInputData inputs)) { }


            float vx = Rb.velocity.x;
            float vy = Rb.velocity.y;
        }

    }

    public void ApplyTeam(PlayerTeam team, Material mat)
    {
        SelectedTeam = team;
        Team = team;
        SpriteRenderer.material = mat;
    }

    void SetMaxVariable()
    {
        Hp = MaxHp;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_GetDamage(float dmg)
    {
        Hp -= (int)dmg;
        _audioSourceDano.PlayOneShot(_audioClipDano);
        if (Hp <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        GameManager2.Instance.PlayerDeath(this);
        Runner.Despawn(Object);
    }

    void PlayerCanNotStart()
    {
        CanPlay = false;
        _rb.velocity = Vector2.zero;
    }

    public void InstantiateBullet(Vector3 DirBullet)
    {
        SetDisparoAnim();

        _weaponBehaviour.ShootBullet(this, DirBullet);
    }

    [SerializeField] bool canSpawnShield = true;
    [SerializeField] float shieldCooldown = 5;

    public void SpawnShield()
    {
        if (!HasStateAuthority) return;
        if (!canSpawnShield) return;

        var shieldSpawned = Runner.Spawn(shield, transform.position, Quaternion.identity);
        shieldSpawned.GetComponent<Cudini>().SetPadre(transform);
        StartCoroutine(SpawnShieldBool());
    }

    IEnumerator SpawnShieldBool()
    {
        canSpawnShield = false;
        yield return new WaitForSeconds(shieldCooldown);
        canSpawnShield = true;
    }

    void LifeUpdated()
    {
        OnLifeUpdate?.Invoke(Hp / (float)_maxHp);
    }

    #region Animator
    public void SetAllAnimFalse()
    {
        Anim.Animator.SetBool("Idle", false);
        Anim.Animator.SetBool("Saltando", false);
        Anim.Animator.SetBool("Cayendo", false);
        Anim.Animator.SetBool("Caminando", false);
        Anim.Animator.SetBool("Grounded", false);
    }

    public void SetIdleAnim()
    {
        Anim.Animator.SetBool("Saltando", false);
        Anim.Animator.SetBool("Cayendo", false);
        Anim.Animator.SetBool("Caminando", false);
        Anim.Animator.SetBool("Idle", true);
    }
    public void SetSaltandoAnim()
    {
        Anim.Animator.SetBool("Idle", false);
        Anim.Animator.SetBool("Caminando", false);
        Anim.Animator.SetBool("Cayendo", false);
        Anim.Animator.SetBool("Saltando", true);
    }
    public void SetCayendoAnim()
    {
        Anim.Animator.SetBool("Idle", false);
        Anim.Animator.SetBool("Caminando", false);
        Anim.Animator.SetBool("Cayendo", true);
        Anim.Animator.SetBool("Saltando", false);
    }
    public void SetCaminandoAnim()
    {
        Anim.Animator.SetBool("Idle", false);
        Anim.Animator.SetBool("Caminando", true);
        Anim.Animator.SetBool("Cayendo", false);
        Anim.Animator.SetBool("Saltando", false);
    }
    public void SetDisparoAnim()
    {
        Anim.Animator.SetTrigger("Disparo");
    }
    public void SetAplastadoAnim()
    {
        Anim.Animator.SetTrigger("Aplastado");
    }
    #endregion

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);

        GameManager2.Instance.OnGameEnded -= PlayerCanNotStart;
        OnDespawn?.Invoke();
    }

}
