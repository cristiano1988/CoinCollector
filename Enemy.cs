using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float rotationSpeed = 180f;

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().PlayerHit(collision.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Coin coin = other.GetComponent<Coin>();
            if (coin != null)
                coin.TeleportCoin();
        }

    }
}