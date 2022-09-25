using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _moveInput;
    private float _initialX;
    private float _initialY;
    private bool _facingRight = true;
    private bool _isGrounded;
    private int _jumpsInTheAir;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;

    [field: SerializeField] public Transform GroundCheck { get; set; }
    [field: SerializeField] public float CheckRadius { get; set; }
    [field: SerializeField] public LayerMask WhatIsGround { get; set; }
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }
    [field: SerializeField] public int JumpsInTheAir { get; set; }


    private void Start()
    {
        Vector3 position = transform.position;
        _initialX = position.x;
        _initialY = position.y;
        _jumpsInTheAir = JumpsInTheAir;
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        HandleJumps();
        if (!_renderer.isVisible)
            transform.position = new Vector3(_initialX, _initialY, 0);
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(GroundCheck.position, CheckRadius, WhatIsGround);
        HandleMovement();
    }

    private void HandleMovement()
    {
        _moveInput = Input.GetAxis("Horizontal");
        _rigidbody.velocity = new Vector2(_moveInput * Speed, _rigidbody.velocity.y);
        switch (_moveInput)
        {
            case > 0 when !_facingRight:
            case < 0 when _facingRight:
                Flip();
                break;
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Transform trans = transform;
        Vector3 scaler = trans.localScale;
        scaler.x *= -1;
        trans.localScale = scaler;
    }

    private void HandleJumps()
    {
        void DoJump() => _rigidbody.velocity = Vector2.up * JumpForce;

        if (_isGrounded)
            _jumpsInTheAir = JumpsInTheAir;

        if (!Input.GetKeyDown(KeyCode.Space)) return;

        switch (_jumpsInTheAir)
        {
            case > 0:
                DoJump();
                _jumpsInTheAir--;
                break;

            case 0 when _isGrounded:
                DoJump();
                break;
        }
    }
}