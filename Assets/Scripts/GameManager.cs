using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GAMESTEP // 준비 -> 시작! -> 시간흘러감 -> 게임 끝 
    {
        READY, GO, STARTGAME, GAMEOVER
    }
    public GAMESTEP gameStep = GAMESTEP.READY;

    // 외부에서 접근하기 위한 싱글턴
    public static GameManager instance;

    // 판정할 두 카드
    public Card fisrstCard;
    public Card secondCard;
    // 시간 UI
    public Text timeTxt;
    float time = 0.0f;
    // 준비 UI
    public Text readyTxt;
    // 끝 UI
    public GameObject endTxt;
    public int cardCnt = 0;
    public bool isOver = false;
    // 카드 파괴 사운드
    AudioSource audioSource;
    public AudioClip clip;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameStep) // 게임 단계에 따라 업데이트 달라지기
        {
            case GAMESTEP.READY:
                time += Time.deltaTime;
                if (time >= 2.0f)
                {
                    readyTxt.text = "Go!";
                    gameStep = GAMESTEP.GO;
                }
                break;
            case GAMESTEP.GO:
                time += Time.deltaTime;
                if (time >= 3.0f)
                {
                    readyTxt.gameObject.SetActive(false);
                    timeTxt.gameObject.SetActive(true);
                    time = 0.0f; // 다시 초기화
                    timeTxt.text = time.ToString("N2");
                    gameStep = GAMESTEP.STARTGAME;
                }
                break;
            case GAMESTEP.STARTGAME:
                time += Time.deltaTime;
                if (time >= 30f)
                {
                    GameOver();
                    time = 30.0f;
                }
                timeTxt.text = time.ToString("N2");
                break;
            default:
                break;
        }
    }

    public void Matched()
    {
        if (fisrstCard.idx == secondCard.idx) // 같다면 파괴
        {
            audioSource.PlayOneShot(clip);
            fisrstCard.DestoryCard();
            secondCard.DestoryCard();
            cardCnt -= 2;
            if (cardCnt == 0) GameOver();
        }
        else // 아니라면 닫기
        {
            fisrstCard.CloseCard();
            secondCard.CloseCard();
        }
        fisrstCard = null;
        secondCard = null;
    }

    void StopTime()
    {
        Invoke("StopTimeInvoke", 2.0f);
    }
    void StopTimeInvoke()
    { Time.timeScale = 1.0f; }

    void GameOver()
    {
        gameStep = GAMESTEP.GAMEOVER;
        StopTime();
        endTxt.SetActive(true);
    }
}
