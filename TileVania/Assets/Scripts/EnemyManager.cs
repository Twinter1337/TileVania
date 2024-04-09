using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private const float StandartLocalScale = 1f;
    private const float StandartVelocityValue = 0f;
    private const string GroundLayerMaskName = "Ground";

    [SerializeField] private Rigidbody2D _enemyRb2D;
    [SerializeField] private BoxCollider2D _enemyBodyCollider2D;
    [SerializeField] private BoxCollider2D _enemyFlipSignCollider2D;
    [SerializeField] private float _enemyMoveSpeed = 1f;

    void Update()
    {
        Move();
        FlipSprite();
    }

    private void Move()
    {
        _enemyRb2D.velocity = new Vector2(_enemyMoveSpeed, StandartVelocityValue);
    }

    private void FlipSprite()
    {
        if (_enemyFlipSignCollider2D.IsTouchingLayers(LayerMask.GetMask(GroundLayerMaskName)))
        {
            _enemyMoveSpeed = -_enemyMoveSpeed;
            transform.localScale = new Vector2(-Mathf.Sign(_enemyRb2D.velocity.x), StandartLocalScale);
        }
    }
}