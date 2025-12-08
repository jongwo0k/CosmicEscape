using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// 모든 Scene에 유지
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // UI로 씬을 넘길 경우
    public GameObject stageClearUI;
    public GameObject gameClearUI;
    public GameObject gameOverUI;

    public AudioSource bgmPlayer;

    private string[] projectileTags = { "Bullet", "Rock", "Bullet_ship" };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            if (bgmPlayer != null)
            {
                bgmPlayer.Stop();
                bgmPlayer.Play();
            }

            gameOverUI.SetActive(false);
            gameClearUI.SetActive(false);
            stageClearUI.SetActive(false);
        }
    }

    // 게임 시작
    void Start()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
        Debug.Log("Game Start");
    }

    // 스테이지 클리어 (보스 처치 성공)
    // Scene 자동 전환? or 클리어 UI 표시 후 Next 버튼으로 전환
    public void StageClear()
    {
        ClearProjectiles();

        StartCoroutine(MoveNextStage());
    }

    public void GameClear()
    {
        if(gameClearUI != null) // Clear UI 존재, 버튼으로 이동
        {
            gameClearUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else // 없는 경우
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void GameIsOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        if (gameOverUI != null) // Over UI 존재, 버튼으로 이동
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator MoveNextStage()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex >= totalScenes - 1)
        {
            GameClear();
            yield break;
        }

        if (stageClearUI != null)
        {
            stageClearUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    // Button
    public void ClearButton()
    {
        Time.timeScale = 1f;

        if (stageClearUI != null)
        {
            stageClearUI.SetActive(false);
        }

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else // 마지막 씬
        {
            GameClear();
        }
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;

        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (gameClearUI != null) gameClearUI.SetActive(false);
        if (stageClearUI != null) stageClearUI.SetActive(false);

        SceneManager.LoadScene("MainMenu");
    }

    // 남은 투사체 제거
    public void ClearProjectiles()
    {
        foreach (string tag in projectileTags)
        {
            GameObject[] projectiles = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject p in projectiles)
            {
                Destroy(p);
            }
        }
    }
}