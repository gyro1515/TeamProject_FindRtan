using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.Burst.Intrinsics;

public class GameManager : MonoBehaviour
{
    public AudioSource bgmAudioSource;
    public AudioClip normalBGM;
    public AudioClip warningBGM;
    public Button Retry;
    public static GameManager instance;
    public RawImage videoDisplay;
    public VideoPlayer videoPlayer;
    public VideoClip[] endingVideos;

    private bool isWarningBGMPlaying = false;


    public enum GameProgress
    {
        EndGame, StartGame, Failed
    }

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
        }
    }

    void Start()
    {
        bgmAudioSource.clip = normalBGM;
        bgmAudioSource.loop = true;
        bgmAudioSource.volume = 0.3f;
        bgmAudioSource.Play();
    }


    void Update()
    {
        switch (progress)
        {
            case GameProgress.EndGame:
                time += Time.deltaTime;
                if (time >= 1.2f)
                {
                    SceneManager.LoadScene("GameOverScene");

                    if (!gameOverTriggered)
                    {
                        gameOverTriggered = true;
                        Invoke("GoToGameOver", 5.0f);
                    }
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
                timeTxt.text = "시간 초과!";
                break;
        }   
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
        if ((progress == GameProgress.EndGame || progress == GameProgress.Failed) && bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
        }
                
    }

    public void Matched()
    {
        if (firstCard.idx == secondCard.idx)
        {
            firstCard.PlayCorrectSound();
            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;

            Debug.Log($"남은 카드 수: {cardCount}");
            
            
                Combo++;
            ComboTxt.text = Combo.ToString();
            

            if (cardCount == 0)
            {
                progress = GameProgress.EndGame;
                //Time.timeScale = 0.0f;
                // 넘어가는 유예시간 주기
                time = 0.0f;
                //endTxt.SetActive(false);
                ChallengeManager.instance.OnGameClearedEarly(time);
                endTxt.SetActive(false);
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

    void GoToGameOver()
    {
        
        SceneManager.LoadScene("GameOverScene");
    }
}
