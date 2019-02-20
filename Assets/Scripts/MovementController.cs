using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MovementController : MonoBehaviour
{
    private const float SkinWidth = .02f;
    private const int TotalHorizontalRays = 6;
    private const int TotalVerticalRays = 4;

    [Header("Collision Masks")]
    [Tooltip("Layers to collide with vertically.")]
    public LayerMask VerticalMask;
    [Tooltip("Layers to collide with horizontally.")]
    public LayerMask HorizontalMask;
    [Header("Parameters")]
    public ControllerParameters DefaultParameters;

    public ControllerParameters Parameters { get { return DefaultParameters; } }
    public ControllerState State { get; private set; }
    public Vector2 Velocity { get { return _velocity; } }
    public bool HandleCollisions { get; set; }

    private Vector2 _velocity;
    private Transform _transform;
    private Vector3 _localScale;
    private BoxCollider2D _playerCollider;
    private Vector3
        _raycastTopLeft,
        _raycastBottomLeft,
        _raycastBottomRight;
    private float
        _verticalDistanceBetweenRays,
        _horizontalDistanceBetweenRays;

    public void Awake()
    {
        State = new ControllerState();
        _playerCollider = GetComponent<BoxCollider2D>();
        _transform = transform;
        _localScale = transform.localScale;
        HandleCollisions = true;

        _horizontalDistanceBetweenRays = (_playerCollider.size.x * Mathf.Abs(_localScale.x) - 2 * SkinWidth) / (TotalVerticalRays - 1);
        _verticalDistanceBetweenRays = (_playerCollider.size.y * Mathf.Abs(_localScale.y) - 2 * SkinWidth) / (TotalHorizontalRays - 1);
    }
    
    public void LateUpdate()
    {
		if (!Parameters.Flying)
			ApplyGravity();
		
        Move(Velocity * Time.deltaTime);
    }
    
    void ApplyGravity()
    {
		_velocity.y += Parameters.Gravity * Time.deltaTime;
    }

    public void AddForce(Vector2 force)
    {
        _velocity += force;
    }
    
    public void SetForce(Vector2 force)
    {
        _velocity = force;
    }

    public void AddHorizontalForce(float x)
    {
        _velocity.x += x;
    }

    public void SetHorizontalForce(float x)
    {
        _velocity.x = x;
    }

    public void AddVerticalForce(float y)
    {
        _velocity.y += y;
    }

    public void SetVerticalForce(float y)
    {
        _velocity.y = y;
    }

    void Move(Vector2 deltaMovement)
    {
        State.Reset();

        if (HandleCollisions)
        {
            CalculateRayOrigins();

			if (Mathf.Abs(deltaMovement.x) > .001f)
				MoveHorizontally(ref deltaMovement);

			MoveVertically(ref deltaMovement);
        }

		_transform.Translate(deltaMovement);

		if (Time.deltaTime > 0)
			_velocity = deltaMovement / Time.deltaTime;

		_velocity.x = Mathf.Min(_velocity.x, Parameters.MaxVelocity.x);
        _velocity.y = Mathf.Min(_velocity.y, Parameters.MaxVelocity.y);
    }

    void CalculateRayOrigins()
    {
        var size = new Vector2(_playerCollider.size.x * Mathf.Abs(_localScale.x), _playerCollider.size.y * Mathf.Abs(_localScale.y)) / 2;
        var center = new Vector2(_playerCollider.offset.x * _localScale.x, _playerCollider.offset.y * _localScale.y);

        _raycastTopLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth, center.y + size.y - SkinWidth);
        _raycastBottomLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth, center.y - size.y + SkinWidth);
        _raycastBottomRight = _transform.position + new Vector3(center.x + size.x - SkinWidth, center.y - size.y + SkinWidth);
    }

    void MoveHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : Vector2.left;
        var rayOrigin = isGoingRight ? _raycastBottomRight : _raycastBottomLeft;

        for (var i = 0; i < TotalHorizontalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + i * _verticalDistanceBetweenRays);
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, HorizontalMask);
			if (!raycastHit)
				continue;

			deltaMovement.x = raycastHit.point.x - rayVector.x;
            rayDistance = Mathf.Abs(deltaMovement.x);

            if (isGoingRight)
            {
                deltaMovement.x -= SkinWidth;
                State.IsCollidingRight = true;
            }
            else
            {
                deltaMovement.x += SkinWidth;
                State.IsCollidingLeft = true;
            }

			if (rayDistance < SkinWidth + .0001f)
				break;
		}
    }

    void MoveVertically(ref Vector2 deltaMovement)
    {
        var isGoingDown = deltaMovement.y < 0;
        var rayDistance = Mathf.Abs(deltaMovement.y) + SkinWidth;
        var rayDirection = isGoingDown ? Vector2.down : Vector2.up;
        var rayOrigin = isGoingDown ? _raycastBottomLeft : _raycastTopLeft;

        for (var i = 0; i < TotalVerticalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x + i * _horizontalDistanceBetweenRays, rayOrigin.y);
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, VerticalMask);
			if (!raycastHit)
				continue;

			if (raycastHit.collider.CompareTag("Platform") && (!isGoingDown || State.DropThroughPlatform))
				continue;

			deltaMovement.y = raycastHit.point.y - rayVector.y;
            rayDistance = Mathf.Abs(deltaMovement.y);

            if (isGoingDown)
            {
                deltaMovement.y += SkinWidth;
                State.IsCollidingBelow = true;
            }
            else
            {
                deltaMovement.y -= SkinWidth;
                State.IsCollidingAbove = true;
            }

			if (rayDistance < SkinWidth + .0001f)
				break;
		}
    }
}