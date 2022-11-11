using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Ground))]
public abstract class CharacterController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;
    [SerializeField] private bool _canAirStrafe = true;

    [Header("Jumping")]
    [SerializeField, Range(0f, 10f)] protected float _jumpHeight = 3f;
    [SerializeField, Range(0, 5)] protected int _maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] protected float _downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] protected float _upwardMovementMultiplier = 1.7f;

    protected Vector2 _direction, _desiredVelocity, _velocity;
    protected Rigidbody2D _body;
    protected Ground _ground;

    protected float _maxSpeedChange, _acceleration, _defaultGravityScale, _jumpSpeed;
    protected int _jumpPhase;

    protected bool _desiredJump, _onGround;

    protected abstract void OnAwake();
    protected abstract float MoveX();
    protected abstract float MoveY();

    private void Awake()
    {
        OnAwake();
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();

        _defaultGravityScale = 1f;
    }

    private void Update()
    {
        _direction.x = MoveX();
        _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - _ground.Friction, 0f);

        _desiredJump |= MoveY() > 0;
    }

    private void FixedUpdate()
    {
        _onGround = _ground.OnGround;
        _velocity = _body.velocity;

        HandleMove();
        HandleJump();

        _body.velocity = _velocity;
    }

    private void HandleMove()
    {
        if (!_canAirStrafe && !_onGround)
        {
            return;
        }

        _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
    }

    private void HandleJump()
    {
        if (_onGround)
        {
            _jumpPhase = 0;
        }

        if (_desiredJump)
        {
            _desiredJump = false;
            JumpAction();
        }

        if (_body.velocity.y > 0)
        {
            _body.gravityScale = _upwardMovementMultiplier;
        }
        else if (_body.velocity.y < 0)
        {
            _body.gravityScale = _downwardMovementMultiplier;
        }
        else if (_body.velocity.y == 0)
        {
            _body.gravityScale = _defaultGravityScale;
        }
    }

    private void JumpAction()
    {
        if (_onGround || _jumpPhase < _maxAirJumps)
        {
            _jumpPhase += 1;

            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight);

            if (_velocity.y > 0f)
            {
                _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
            }
            else if (_velocity.y < 0f)
            {
                _jumpSpeed += Mathf.Abs(_body.velocity.y);
            }
            _velocity.y += _jumpSpeed;
        }
    }
}
