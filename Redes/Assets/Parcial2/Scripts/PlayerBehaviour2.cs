using Fusion;
using System;
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

    //[SerializeField] float _jumpForce;
    //public float JumpForce { get { return _jumpForce; } private set { _jumpForce = value; } }

    //[SerializeField] int _jumpsAmount;
    //public int JumpsMaxAmount { get { return _jumpsAmount; } private set { _jumpsAmount = value; } }

    //[SerializeField] int _jumpsLeft;
    //public int JumpsLeft { get { return _jumpsLeft; } private set { _jumpsLeft = value; } }

    //[SerializeField] int _jumpCost;
    //public int JumpCost { get { return _jumpCost; } private set { _jumpCost = value; } }

    //[SerializeField] bool _isGrounded = false;
    //public bool IsGrounded { get { return _isGrounded; } private set { _isGrounded = value; } }

    //[SerializeField] bool _isFalling = false;
    //public bool IsFalling { get { return _isFalling; } private set { _isFalling = value; } }

    //[SerializeField] float _poundForce;
    //public float PoundForce { get { return _poundForce; } private set { _poundForce = value; } }

    [SerializeField] SpriteRenderer _spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } private set { _spriteRenderer = value; } }

    [SerializeField] NetworkMecanimAnimator _anim;
    public NetworkMecanimAnimator Anim { get { return _anim; } private set { _anim = value; } }

    [SerializeField] PlayerTeam _team;
    public PlayerTeam Team { get { return _team; } private set { _team = value; } }

    [SerializeField] BulletBehaviour2 _bulletPrefab;
    public BulletBehaviour2 BulletPrefab { get { return _bulletPrefab; } private set { _bulletPrefab = value; } }

    //[SerializeField] bool _hasJustBeenTp = false;
    //public bool HasJustBeenTp { get { return _hasJustBeenTp; } private set { _hasJustBeenTp = value; } }

    //[SerializeField] float _requiredPoundVelocity;
    //public float RequiredPoundVelocity { get { return _requiredPoundVelocity; } private set { _requiredPoundVelocity = value; } }

    //[SerializeField] float _poundDamage;
    //public float PoundDamage { get { return _poundDamage; } private set { _poundDamage = value; } }

    //float _lastVelocityY;

    [Networked]
    public NetworkBool CanPlay { get; set; }

    public event Action OnDespawn;

    WeaponBehaviour _weaponBehaviour;

    [Networked]
    public NetworkBool IsReady { get; set; }

    public AudioSource _audioSourceJump, _audioSourceDano, _audioSourcePound;
    public AudioClip _audioClipJump, _audioClipDano, _audioClipPound;


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

        //_lastVelocityY = Mathf.Abs(_rb.velocity.y);

        //if (_rb.velocity.y < 0 && !Anim.Animator.GetBool("Cayendo") && !IsGrounded)
        //{
        //    SetAllAnimFalse();
        //    SetCayendoAnim();
        //}

        //Anim.Animator.SetBool("Grounded", _isGrounded);
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

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    _controller.FakeOnTriggerEnter2D(collision);
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    _controller.FakeOnTriggerExit2D(collision);
    //}

    public void ApplyTeam(PlayerTeam team, Material mat)
    {
        SelectedTeam = team;
        Team = team;
        SpriteRenderer.material = mat;
    }

    //public void GroundTouched()
    //{
    //    IsGrounded = true;
    //    Anim.Animator.SetBool("Cayendo", false);
    //    Anim.Animator.SetBool("Grounded", true);
    //    JumpsLeft = JumpsMaxAmount;
    //}

    //public void SetGroundedFalse()
    //{
    //    IsGrounded = false;
    //    Anim.Animator.SetBool("Grounded", false);
    //}

    void SetMaxVariable()
    {
        //JumpsLeft = JumpsMaxAmount;
        Hp = MaxHp;
    }

    //public void ReduceJump()
    //{
    //    JumpsLeft -= JumpCost;
    //}

    public void TPed()
    {
        _spriteRenderer.enabled = false;
        //HasJustBeenTp = true;
    }

    public void TPedOff()
    {
        //HasJustBeenTp = false;
        _spriteRenderer.enabled = true;
    }

    // Source = quien llama al metodo  |  Target = Quien ejecuta el contenido
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_GetDamage(float dmg)
    {
        Hp -= (int)dmg;
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerBehaviour2 enemy))
        {

            //GroundTouched();

            //if (_lastVelocityY >= RequiredPoundVelocity)
            //{
            //    enemy.RPC_GetDamage(PoundDamage);
            //    _audioSourcePound.PlayOneShot(_audioClipPound);
            //}
        }
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
