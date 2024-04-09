using UnityEngine;

public class Coin : MonoBehaviour
{
    private const string PlayerTagName = "Player";
    
    [SerializeField] private AudioClip _coinPickedIUpSfx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CoinPickedUp(collision);
    }

    private void CoinPickedUp(Collider2D collision)
    {
        if (collision.CompareTag(PlayerTagName))
        {
            GameSession.Instance.AddScore();
            AudioSource.PlayClipAtPoint(_coinPickedIUpSfx, Camera.main.transform.position);

            Destroy(gameObject);
        }
    }
}
