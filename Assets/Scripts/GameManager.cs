using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections; //코루틴용 using문
using Unity.Burst.Intrinsics;


public class GameManager : MonoBehaviour
{
    public Button Retry;
    public static GameManager instance;
    public RawImage videoDisplay;
    public VideoPlayer videoPlayer;
    public VideoClip[] endingVideos;

    public enum GameProgress
    {
        EndGame, StartGame, Failed
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
    public Text ComboTxt;
    public int Combo = 0;
    public int cardCount = 10;

    float time = 30.0f;
    bool gameOverTriggered = false;
    private bool Win = false;

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
                if (!gameOverTriggered)
                {
                    gameOverTriggered = true;
                    Invoke("GoToGameOver", 5.0f);
                }
                break;

            case GameProgress.StartGame:
                time -= Time.deltaTime;
                if (time <= 0.0f)
                {
                    time = 0.0f;
                    progress = GameProgress.Failed;
                    ChallengeManager.instance.OnGameFailed();
                }
                timeTxt.text = time.ToString("N2");
                break;

            case GameProgress.Failed:
                Retry.gameObject.SetActive(true);
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

            Combo++;
            ComboTxt.text = Combo.ToString();

            if (cardCount == 0)
            {
                // 마지막 스테이지
                if (currentStageIndex + 1 >= totalStageCount)
                {
                    //마지막 스테이지 완료 > 게임오버씬으로 전환
                    progress = GameProgress.EndGame;
                    ChallengeManager.instance.OnGameClearedEarly(time);
                    endTxt.SetActive(false);
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

            Combo = 0;
            ComboTxt.text = Combo.ToString();
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
    
    



    void GoToGameOver()
    {
        
        SceneManager.LoadScene("GameOverScene");
    }
}