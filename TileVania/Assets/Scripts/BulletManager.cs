using System;
using UnityEngine;

public enum TagsNamesThatDestroysBulletOnTouch { Enemy, Ground, Hazards };

public class BulletManager : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _bulletRb2D;
    [SerializeField] private float _bulletSpeed = 20f;
    
    private float _xBulletSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DeleteBulletOnTouchTheCollisionSurface(collision);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
    
    private void Start()
    {
        SetBulletSpeedAndScale();
    }

    private void SetBulletSpeedAndScale()
    {
        _xBulletSpeed = PlayerManager.Instance.transform.localScale.x * _bulletSpeed;
        _bulletRb2D.velocity = new Vector2(_xBulletSpeed, 0f);
    }
    
    private void DeleteBulletOnTouchTheCollisionSurface(Collider2D collision)
    {
        Enum.TryParse(collision.tag, out TagsNamesThatDestroysBulletOnTouch value);

        if (value == TagsNamesThatDestroysBulletOnTouch.Enemy)
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
}
