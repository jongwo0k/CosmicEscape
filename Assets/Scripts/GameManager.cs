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

    // 게임 시작
    void Start()
    {
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
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator MoveNextStage()
    {
        if(stageClearUI != null) // Clear UI 존재, 버튼으로 이동
        {
            stageClearUI.SetActive(true);
        }
        else // 없는 경우
        {
            yield return new WaitForSeconds(3.0f); // 사망 애니메이션 재생 대기

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1) // 다음 씬 (Build Settings)
            {
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
            else
            {
                GameClear();
            }
        }
    }

    // Button
    public void ClearButton()
    {
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
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

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