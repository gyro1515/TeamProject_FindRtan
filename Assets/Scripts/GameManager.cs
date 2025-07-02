using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; //코루틴용 using문

public class GameManager : MonoBehaviour
{
    public Button Retry;
    public static GameManager instance;

    public enum GameProgress
    {
        EndGame, StartGame, Failed, NextStage
    }
    //스테이지 변수 추가
    public int currentStageIndex = 0; // 0번부터 시작, 빌드 세팅 순서에 맞게
    public int totalStageCount = 2; // 전체 스테이지 개수
    //panel 변수 추가
    public GameObject selectPanel; // 인스펙터에서 스테이지 판넬 오브젝트 할당

    public GameProgress progress = GameProgress.StartGame;
    public Card firstCard;
    public Card secondCard;
    public Text timeTxt;
    public GameObject endTxt;
    public int cardCount = 10;
    float time = 30.0f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    void Update()
    {
        switch (progress)
        {
            case GameProgress.EndGame:
                //마지막 스테이지 클리어>즉시 GameOverScene로드
                Time.timeScale = 1f;
                SceneManager.LoadScene("GameOverScene");
                //Debug.Log("End");
                break;
            case GameProgress.StartGame:
                time -= Time.deltaTime;
               
                if(time <= 0.0f)
                {
                    time = 0.0f;
                    progress = GameProgress.Failed;
                }
                timeTxt.text = time.ToString("N2");
                break;
            case GameProgress.Failed:   
                Retry.gameObject.SetActive(true);
                break;
            case GameProgress.NextStage:
                //다음 스테이지 이름이 "MainScene1", "MainScene2"
                //int nextStage = currentStageIndex + 1;
                //string nextSceneName = "MainScene"+ nextStage;
                //SceneManager.LoadScene(nextSceneName);
                break;
            default:
                break;
        }

    }

    public void Matched()
    {
        if (firstCard.idx == secondCard.idx)
        {
            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;
            //Debug.Log($"End{cardCount}");
            if (cardCount == 0)
            {
                // 마지막 스테이지
                if(currentStageIndex + 1 >= totalStageCount)
                {
                    //마지막 스테이지 완료 > 게임오버씬으로 전환
                    progress = GameProgress.EndGame;
                }
                else
                {
                    // 마지막 스테이지인지 체크
                    progress = GameProgress.NextStage;

                    // 다음 스테이지 해금
                    PlayerPrefs.SetInt("StageUnlocked_" + (currentStageIndex + 1), 1);
                    PlayerPrefs.Save();

                    //1초 딜레이 후 판넬 활성화(코루틴사용)
                    StartCoroutine(ShowSelectPanelWithDelay());
                }
                   

                //게임 진행을 멈추고 싶으면
                //Time.timeScale = 0.0f;
            }
            
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        firstCard = null;
        secondCard = null;

    }
    // 코루틴 함수 추가
    private IEnumerator ShowSelectPanelWithDelay()
    {
        //타임스케일이 0이라도 돌아가는 리얼타임 대기
        yield return new WaitForSeconds(1f); 
        if (selectPanel != null) selectPanel.SetActive(true);
        Time.timeScale = 0.0f; // 게임 멈춤

        //Scene 전환방식(예: StageScene으로)
       
    }


    public bool IsStageUnlocked(int stageIndex)
    {
        if (stageIndex == 0) return true;
        return PlayerPrefs.GetInt("StageUnlocked_"+ stageIndex, 0) == 1;
    }
    
    

}