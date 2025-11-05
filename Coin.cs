using UnityEngine;

public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            FindObjectOfType<GameManager>().AddScore();
            TeleportCoin();
    }

    public void TeleportCoin()
    {
        float newX = Random.Range(GameBounds.minX, GameBounds.maxX);
        float newY = Random.Range(GameBounds.minY, GameBounds.maxY);
        transform.position = new Vector3(newX, newY, 0);
    }
}