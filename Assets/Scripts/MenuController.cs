using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{

    private StartButtonEffect startButtonEffect; //버튼 이펙트 오브젝트 참조
    public float delayBeforeLoad = 0.15f;
    public void OnClickStart()
    {
        if (startButtonEffect != null)
            startButtonEffect.StopAndReset(); //버튼 클릭 시 멈춤


        startButtonEffect?.PlayClickSound();

        // 씬 전환 딜레이 코루틴 실행
        StartCoroutine(LoadSceneWithDelay());

        IEnumerator LoadSceneWithDelay()
        {
            yield return new WaitForSeconds(delayBeforeLoad);
            SceneManager.LoadScene("HumanoidBoss"); // 나중에 FirstScene으로 교체
        }
    }
}
