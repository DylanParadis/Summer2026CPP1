using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int maxLives = 9;
    public int currentLives = 3;

    public System.Action<int> OnLivesChanged;

    public int Lives
    {
        get => currentLives;
        set
        {
            int previousLives = currentLives;

            currentLives = Mathf.Clamp(
                value,
                0,
                maxLives
            );

            if (currentLives == 0 && previousLives > 0)
            {
                GameOver();
            }
            else if (currentLives < previousLives)
            {
                Respawn();
            }

            OnLivesChanged?.Invoke(currentLives);

            Debug.Log(
                "Lives: " + currentLives +
                " Max Lives: " + maxLives
            );
        }
    }

    public void GameOver()
    {
        if (playerInstance != null)
        {
            Destroy(playerInstance);
        }

        SceneManager.LoadScene("3.GameOver");
    }

    public delegate void PlayerSpawned(GameObject player);
    public event PlayerSpawned OnPlayerSpawned;

    public Vector3 spawnPosition;

    public GameObject playerPrefab;
    public GameObject playerInstance;

    public void SpawnPlayer(Vector3 position)
    {
        spawnPosition = position;

        playerInstance = Instantiate(
            playerPrefab,
            position,
            Quaternion.identity
        );

        OnPlayerSpawned?.Invoke(playerInstance);
    }

    public void Respawn()
    {
        if (playerInstance != null)
        {
            Destroy(playerInstance);
        }

        playerInstance = Instantiate(
            playerPrefab,
            spawnPosition,
            Quaternion.identity
        );

        OnPlayerSpawned?.Invoke(playerInstance);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        currentLives = 3;
        OnLivesChanged?.Invoke(currentLives);

        SceneManager.LoadScene("2.Game");
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "1.Title" &&
            Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }

        if (SceneManager.GetActiveScene().name == "3.GameOver" &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("1.Title");
        }
    }
}