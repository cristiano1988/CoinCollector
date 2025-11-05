using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject coinPrefab;
    public GameObject explosionPrefab;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;

    [Header("Audio")]
    public AudioClip crashSound;
    private AudioSource audioSource;

    [Header("Settings")]
    public float playerSpeed = 7.2f;
    public float rotationSpeed = 180f;
    public float enemyRotationSpeed = 180f;
    public float enemySpawnY = 12f;

    private GameObject player;
    private GameObject coin;
    private int score = 0;

    private float minX, maxX, minY, maxY;

    void Awake()
    {
        // Postavi granice scene
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        float margin = 1f;

        minX = -camWidth + margin;
        maxX = camWidth - margin;
        minY = -camHeight + margin;
        maxY = camHeight - margin;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Stvori igrača
        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        // Stvori početni coin
        SpawnCoin();

        // Pokreni spawn neprijatelja
        StartCoroutine(SpawnEnemies());

        // Postavi početni score
        UpdateScore();
    }

    void Update()
    {
        if (player != null)
            HandlePlayerMovement();

        RotateEnemies();
        CheckCoinCollision();
        CheckEnemyCollision();
    }

    // ---------------- PLAYER ----------------
    void HandlePlayerMovement()
    {
        float moveInput = Input.GetAxisRaw("Vertical");
        player.transform.Translate(Vector3.up * moveInput * playerSpeed * Time.deltaTime, Space.Self);

        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.A)) rotationInput = 1f;
        else if (Input.GetKey(KeyCode.D)) rotationInput = -1f;

        player.transform.Rotate(0f, 0f, rotationInput * rotationSpeed * Time.deltaTime);

        float clampedX = Mathf.Clamp(player.transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(player.transform.position.y, minY, maxY);
        player.transform.position = new Vector3(clampedX, clampedY, player.transform.position.z);
    }

    // ---------------- COIN ----------------
    void SpawnCoin()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        coin = Instantiate(coinPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }

    void TeleportCoin()
    {
        if (coin == null) return;
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        coin.transform.position = new Vector3(x, y, 0);
    }

    void CheckCoinCollision()
    {
        if (player == null || coin == null) return;
        if (Vector2.Distance(player.transform.position, coin.transform.position) < 0.7f)
        {
            AddScore();
            TeleportCoin();
        }
    }

    void AddScore()
    {
        score++;
        UpdateScore();
    }

    void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    // ---------------- ENEMY ----------------
    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            float wait = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(wait);

            Vector3 pos = new Vector3(Random.Range(minX, maxX), enemySpawnY, 0);
            Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
    }

    void RotateEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy == null) continue;
            enemy.transform.Rotate(0, 0, enemyRotationSpeed * Time.deltaTime);
        }
    }

    void CheckEnemyCollision()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy == null || player == null) continue;

            // Sudar s igračem
            if (Vector2.Distance(enemy.transform.position, player.transform.position) < 0.8f)
            {
                PlayerHit();
                break;
            }

            // Sudar s novčićem
            if (coin != null && Vector2.Distance(enemy.transform.position, coin.transform.position) < 0.8f)
            {
                TeleportCoin();
            }
        }
    }

    // ---------------- PLAYER HIT ----------------
    void PlayerHit()
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
