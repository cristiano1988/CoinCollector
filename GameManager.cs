using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;

    [Header("Prefabs")]
    public GameObject explosionPrefab;
    public GameObject coinPrefab;

    [Header("Sound")]
    public AudioClip crashSound;
    
    private AudioSource audioSource;
    private GameObject spawnedCoin;
    private int score = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SpawnCoin();
        UpdateScore();
    }
    void SpawnCoin()
    {
        float x = Random.Range(GameBounds.minX, GameBounds.maxX);
        float y = Random.Range(GameBounds.minY, GameBounds.maxY);
        Vector3 spawnPos = new Vector3(x, y, 0);

        spawnedCoin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);
    }

    public void AddScore()
    {
        score++;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "" + score;
    }

    public void PlayerHit(GameObject player)
    {
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, player.transform.position, Quaternion.identity);
        if (audioSource != null && crashSound != null)
            audioSource.PlayOneShot(crashSound);
        Destroy(player);
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);
        Invoke(nameof(ReloadScene), 2f);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}