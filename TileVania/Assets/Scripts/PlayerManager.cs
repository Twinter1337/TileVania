using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public sealed class PlayerManager : MonoBehaviour
{
    private const int AfterDeathDelay = 3;
    private const float StandardLocalScale = 1f;
    private const float StandardGameGravityScale = 3f;
    private const float StandardPlayerVelocityValue = 0f;
    private const float StandardClimbingAnimationSpeed = 0.82f;
    private const string DyingTriggerNameForAnimation = "Dying";
    private const string IsRunningBoolNameForAnimation = "isRunning";
    private const string IsClimbingBoolNameForAnimation = "isClimbing";

    [SerializeField] private Rigidbody2D _playerRb2D;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D _playerBodyCollider;
    [SerializeField] private BoxCollider2D _playerFeetCollider;
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _gun;
    [SerializeField] private float _runSpeed = 10f;
    [SerializeField] private float _jumpSpeed = 5f;
    [SerializeField] private float _climbingSpeed = 5f;
    
    public static PlayerManager Instance;

    private Vector2 _moveInput;
    private bool _isAlive = true;

    private enum LayerMasksThatContactWithPlayer
    {
        Ground,
        Ladder,
        Enemies,
        Hazards
    };
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    void Update()
    {
        if (!_isAlive)
        {
            return;
        }

        Run();
        ClimbLadder();
        FlipSprite();
        CheckIfDie();
    }

    
    private void OnMove(InputValue value)
    {
        if (!_isAlive)
        {
            return;
        }

        _moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (!_isAlive)
        {
            return;
        }

        if (value.isPressed &&
            _playerFeetCollider.IsTouchingLayers(LayerMask.GetMask(LayerMasksThatContactWithPlayer.Ground.ToString())))
        {
            _playerRb2D.velocity += new Vector2(StandardPlayerVelocityValue, _jumpSpeed);
        }
    }

    private void OnFire(InputValue value)
    {
        if (!_isAlive)
        {
            return;
        }

        Instantiate(_bullet, _gun.position, transform.rotation);
    }

    private void Run()
    {
        _playerRb2D.velocity = new Vector2(_moveInput.x * _runSpeed, _playerRb2D.velocity.y);
        _animator.SetBool(IsRunningBoolNameForAnimation, (_moveInput.x != 0));
    }

    private void FlipSprite()
    {
        if (_playerRb2D.velocity.x == 0)
        {
            return;
        }

        transform.localScale = new Vector2(Mathf.Sign(_playerRb2D.velocity.x), StandardLocalScale);
    }

    private void ClimbLadder()
    {
        bool isClimbingLadderAnimationNeedsToPlay =
            _playerFeetCollider.IsTouchingLayers(LayerMask.GetMask(LayerMasksThatContactWithPlayer.Ladder.ToString()));
        _animator.SetBool(IsClimbingBoolNameForAnimation, isClimbingLadderAnimationNeedsToPlay);
        _animator.speed = StandardClimbingAnimationSpeed;

        if (!isClimbingLadderAnimationNeedsToPlay)
        {
            _playerRb2D.gravityScale = StandardGameGravityScale;
            return;
        }

        if (_moveInput.y == 0)
        {
            _animator.speed = 0;
        }

        _playerRb2D.velocity = new Vector2(_playerRb2D.velocity.x, _moveInput.y * _climbingSpeed);
        _playerRb2D.gravityScale = 0f;
    }
    
    private void CheckIfDie()
    {
        if (_playerBodyCollider.IsTouchingLayers(LayerMask.GetMask(LayerMasksThatContactWithPlayer.Enemies.ToString(),
                LayerMasksThatContactWithPlayer.Hazards.ToString())))
        {
            _isAlive = false;
            _animator.SetTrigger(DyingTriggerNameForAnimation);
            _playerSpriteRenderer.color = Color.red;
            _playerRb2D.velocity = new Vector2(StandardPlayerVelocityValue, StandardPlayerVelocityValue);
            Invoke(nameof(ReloadLevel), AfterDeathDelay);
        }
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}