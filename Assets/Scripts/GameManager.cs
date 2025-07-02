using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
//using System.Collections; //코루틴용 using문
using Unity.Burst.Intrinsics;
using UnityEditor.Experimental.GraphView;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //public AudioSource bgmAudioSource;
    AudioSource bgmAudioSource;
    public AudioClip normalBGM;
    public AudioClip warningBGM;
    public Button Retry;
    public GameObject fastImage;
    public GameObject normalImage;
    public GameObject slowImage;
    public RawImage videoDisplay;
    public VideoPlayer videoPlayer;
    public VideoClip[] endingVideos;
    public Board curBoard;

    //스테이지 선택 버튼 구조체
    [System.Serializable]
    public struct StageButton
    {
        public Button button;
        public int stageNumber;
    }
    // 스테이지 선택 버튼 배열
    //public StageButton[] stageButtons;
    public StageButton[] stageButtons = new StageButton[2];

    private bool isWarningBGMPlaying = false;

    public enum GameProgress
    {
        SettingCard, EndGame, StartGame, Failed, NextStage, SelectStage, Finish
    }
    //스테이지 변수 추가
    public int currentStageIndex = 0; // 0번부터 시작, 빌드 세팅 순서에 맞게
    public int totalStageCount = 2; // 전체 스테이지 개수
    //panel 변수 추가
    public GameObject selectPanel; // 인스펙터에서 스테이지 판넬 오브젝트 할당
    // 스테이지 버튼 활성화용 변수
    bool isFirstSetStageBtn = true;

    public GameProgress progress = GameProgress.SettingCard;
    public Card firstCard;
    public Card secondCard;
    public Text timeTxt;
    //public GameObject endTxt;
    public Text ComboTxt;
    public int Combo = 0;
    int cardCount = 12;
    // 시간 관리
    public float startTime = 30.0f;
    float time = 0.0f;
    bool gameOverTriggered = false;
    private bool Win = false;
    // 카드 배치 시간, CopyToGameInstace()에서 자동으로 설정 됨
    float setCardTime = 0.0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //Debug.Log("On" + " " + gameObject.GetInstanceID() + " " + instance.gameObject.GetInstanceID());
        }
        else
        {
            CopyToGameInstace(); // OnDestroy로 실행하면 한 프레임을 버리는 것과 같아 다시 Awake()에서 복사를 했습니다.
            Destroy(this.gameObject);
            if (instance.bgmAudioSource != null)
            {
                if (!instance.bgmAudioSource.isPlaying)
                {
                    instance.bgmAudioSource.clip = normalBGM;
                    instance.bgmAudioSource.loop = true;
                    instance.bgmAudioSource.volume = 0.3f;
                    instance.bgmAudioSource.Play();
                }
                else if (instance.bgmAudioSource.clip == warningBGM)
                {
                    instance.bgmAudioSource.Stop();
                    instance.bgmAudioSource.clip = normalBGM;
                    instance.bgmAudioSource.loop = true;
                    instance.bgmAudioSource.volume = 0.3f;
                    instance.bgmAudioSource.Play();
                }
            }
        }
    }

    void Start()
    {
        // Awake()에서 Destroy시 Start()실행 안됨
        
        bgmAudioSource = GetComponent<AudioSource>();
        //Debug.Log("Setting");

        if (bgmAudioSource != null)
        {
            bgmAudioSource.clip = normalBGM;
            bgmAudioSource.loop = true;
            bgmAudioSource.volume = 0.3f;
            bgmAudioSource.Play();
            //Debug.Log("PlaySound");
        }
        else
        {
            //Debug.Log("No");
        }
        progress = GameProgress.SettingCard;
        if (curBoard)
        {
            setCardTime = curBoard.cardTotalTime;
        }
    }

    void Update()
    {
        //스테이지 인덱스 초기화
        if (isFirstSetStageBtn) { RefreshButtonState(); isFirstSetStageBtn = false; }

        if (timeTxt == null) return;
        
        switch (progress)
        {
            case GameProgress.SettingCard:
                time += Time.deltaTime;
                if (time >= setCardTime)
                {
                    time = startTime;
                    progress = GameProgress.StartGame;
                    //Debug.Log(time);
                }
                break;
            case GameProgress.EndGame:
                Invoke("EndGame", 3.0f);
                progress = GameProgress.Finish;
                break;
            case GameProgress.StartGame:
                time -= Time.deltaTime;
                if (time <= 0.0f)
                {
                    time = 0.0f;
                    progress = GameProgress.Failed;
                   
                }
                if (timeTxt != null)
                {
                    timeTxt.text = time.ToString("N2");
                    if (time <= 5f)
                    {
                        timeTxt.color = Color.red;
                    }
                    else if (time < 10f)
                    {
                        timeTxt.color = new Color(1f, 0.5f, 0f);
                    }
                    else
                    {
                        timeTxt.color = Color.black;
                    }
                }
                if (time <= 10f && !isWarningBGMPlaying)
                {
                    bgmAudioSource.Stop();
                    bgmAudioSource.clip = warningBGM;
                    bgmAudioSource.volume = 0.2f;
                    bgmAudioSource.Play();
                    isWarningBGMPlaying = true;
                }
                else if (time > 10f && isWarningBGMPlaying)
                {
                    bgmAudioSource.Stop();
                    bgmAudioSource.clip = normalBGM;
                    bgmAudioSource.Play();
                    isWarningBGMPlaying = false;
                }
                break;

            case GameProgress.Failed:
                Retry.gameObject.SetActive(true);
                timeTxt.text = "시간 초과!";
                break;
            case GameProgress.SelectStage:
                Invoke("SelectStage", 4.0f);
                progress = GameProgress.Finish;
                break;
            case GameProgress.Finish:
                break;
            default:
                break;
        }
        
        if ((progress == GameProgress.EndGame || progress == GameProgress.Failed) && bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
        }
                
    }
    void SelectStage()
    {
        // 스테이지 선택 패널 활성화
        if (selectPanel != null)
        {
            selectPanel.SetActive(true);
        }
    }
    void EndGame()
    {
        //마지막 스테이지 클리어>즉시 GameOverScene로드
        /*Time.timeScale = 1f;
        if (!gameOverTriggered)
        // 타이머 누적
        {
            //Debug.Log(time);
            time += Time.deltaTime;
        }
        // 조건 만족시 한번만 실행
        if (time >= 1.2f)
        {
            if (!gameOverTriggered)
            {
                gameOverTriggered = true;
                Invoke("GoToGameOver", 2.0f);
            }
        }*/
        GoToGameOver();
    }

    public void Matched()
    {
        if (firstCard.idx == secondCard.idx)
        {
            firstCard.PlayCorrectSound();
            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;

            //Debug.Log($"남은 카드 수: {cardCount}");
            Combo++;
            ComboTxt.text = Combo.ToString();

            if (cardCount == 0)
            {
                // 업적 띄우기
                Invoke("ShowClearImageBasedOnTime", 1.5f);
                // 마지막 스테이지
                if (currentStageIndex + 1 >= totalStageCount)
                {
                                        //마지막 스테이지 완료 > 게임오버씬으로 전환
                    progress = GameProgress.EndGame;
                }

                else
                {
                                       // 다음 스테이지 해금
                    currentStageIndex++;
                    /*PlayerPrefs.SetInt("StageUnlocked_" + (currentStageIndex + 1), 1);
                    PlayerPrefs.Save();*/
                    progress = GameProgress.SelectStage;
                    
                }
                // 넘어가는 유예시간 주기
                time = 0.0f;
            }
        }
        else
        {
            firstCard.PlayErrorSount();
            firstCard.CloseCard();
            secondCard.CloseCard();

            Combo = 0;
            ComboTxt.text = Combo.ToString();
        }
        firstCard = null;
        secondCard = null;
    }


void ShowClearImageBasedOnTime()//클리어타임에 따라 보여지는 이미지
    {
        if (fastImage == null || normalImage == null || slowImage == null) return;
        fastImage.SetActive(false);
        normalImage.SetActive(false);
        slowImage.SetActive(false);

        if (time > 20f)
        {
            fastImage.SetActive(true);
            Debug.Log("빠른클리어 - photo");
        }
        else if (time > 10f)
        {
            normalImage.SetActive(true);
            Debug.Log("보통클리어 - photo");
        }
        else
        {
            slowImage.SetActive(true);
            Debug.Log("느린클리어 - photo");
        }
        Invoke("HideImage", 2.0f);
       
    }
    void HideImage()
    {
        if (fastImage == null || normalImage == null || slowImage == null) return;
        fastImage.SetActive(false);
        normalImage.SetActive(false);
        slowImage.SetActive(false);
    }
    // 코루틴 함수 추가
    private IEnumerator ShowSelectPanelWithDelay()
    {
        //타임스케일이 0이라도 돌아가는 리얼타임 대기
        yield return new WaitForSeconds(1f); 
        if (selectPanel != null) selectPanel.SetActive(true);
        //Time.timeScale = 0.0f; // 게임 멈춤


    //Scene 전환방식(예: StageScene으로)
       
    }


    public bool IsStageUnlocked(int stageIndex)
    {
        if (stageIndex == 0) return true;
        return PlayerPrefs.GetInt("StageUnlocked_"+ stageIndex, 0) == 1;
    }

    // 스테이지 로드 함수
    public void RefreshButtonState()
    {
        //Debug.Log("currentStageIndex: " + currentStageIndex);

        foreach (var sb in stageButtons)
        {
            bool unlocked = (sb.stageNumber <= 1 + instance.currentStageIndex);
            //|| PlayerPrefs.GetInt("StageUnlocked_" + (sb.stageNumber - 1), 0) == 1;

            //Debug.Log("Button: " + sb.stageNumber + " " + unlocked);
            // 보이기/숨기기
            sb.button.gameObject.SetActive(unlocked);
            sb.button.interactable = unlocked;

            if (unlocked)
            {
                // 클릭 리스너 등록 (중복 방지)
                sb.button.onClick.RemoveAllListeners();
                sb.button.onClick.AddListener(() => LoadStage(sb.stageNumber));
            }

        }
    }
    
    // 스테이지 로드 함수
    public void LoadStage(int stageNumber)
    {
        //Debug.Log(currentStageIndex + " "+ stageNumber);
        Time.timeScale = 1f;
        GameManager.instance.currentStageIndex = stageNumber - 1;
        SceneManager.LoadScene("MainScene" + stageNumber);
       
    }

    void GoToGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }
    void CopyToGameInstace()
    {
        instance.normalBGM = normalBGM;
        instance.warningBGM = warningBGM;
        instance.Retry = Retry;
        instance.fastImage = fastImage;
        instance.normalImage = normalImage;
        instance.slowImage = slowImage;
        instance.isWarningBGMPlaying = isWarningBGMPlaying;
        //instance.currentStageIndex = currentStageIndex;
        instance.totalStageCount = totalStageCount;
        instance.selectPanel = selectPanel;
        instance.progress = progress;
        instance.firstCard = firstCard;
        instance.secondCard = secondCard;
        instance.timeTxt = timeTxt;
        instance.ComboTxt = ComboTxt;
        instance.Combo = Combo;
        instance.cardCount = cardCount;
        instance.startTime = startTime;
        instance.time = time;
        instance.gameOverTriggered = gameOverTriggered;
        instance.Win = Win;
        if (curBoard != null)
        {
            instance.curBoard = curBoard;
            instance.setCardTime = instance.curBoard.cardTotalTime;
        }
        instance.stageButtons = stageButtons;
        instance.isFirstSetStageBtn = isFirstSetStageBtn;
    }
    /*private void OnDestroy()
    {
        instance.normalBGM = normalBGM;
        instance.warningBGM = warningBGM;
        instance.Retry = Retry;
        instance.fastImage = fastImage;
        instance.normalImage = normalImage; 
        instance.slowImage = slowImage;
        instance.isWarningBGMPlaying = isWarningBGMPlaying;
        //instance.currentStageIndex = currentStageIndex;
        instance.totalStageCount = totalStageCount;
        instance.selectPanel = selectPanel;
        instance.progress = progress;
        instance.firstCard = firstCard;
        instance.secondCard = secondCard;
        instance.timeTxt = timeTxt;
        instance.ComboTxt = ComboTxt;
        instance.Combo = Combo;
        instance.cardCount = cardCount;
        instance.startTime = startTime;
        instance.time = time;
        instance.gameOverTriggered = gameOverTriggered;
        instance.Win = Win;
        if(curBoard != null)
        {
            instance.curBoard = curBoard;
            instance.setCardTime = instance.curBoard.cardTotalTime;
        }
        instance.stageButtons = stageButtons;
        instance.isFirstSetStageBtn = isFirstSetStageBtn;
    }*/
}