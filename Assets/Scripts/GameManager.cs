using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Button Retry;
    public static GameManager instance;

    public enum GameProgress
    {
        EndGame, StartGame, Failed
    }

    public GameProgress progress = GameProgress.StartGame;
    public Card firstCard;
    public Card secondCard;
    public Text timeTxt;
    public GameObject endTxt;
    public int cardCount = 10;
    float time = 30.0f;
    public int stage = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        switch (stage)
        {
            case 1:
                time = 25.0f;
                break;
            case 2:
                time = 20.0f;
                break;
            case 3:
                time = 15.0f;
                break;
            default:
                time = 30.0f;
                break;
        }
    }


    void Update()
    {
        switch (progress)
        {
            case GameProgress.EndGame:
                SceneManager.LoadScene("GameOverScene");
                Debug.Log("End");
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
            Debug.Log($"End{cardCount}");
            if (cardCount == 0)
            {
                progress = GameProgress.EndGame;
                //Time.timeScale = 0.0f;
                endTxt.SetActive(false);
            }

        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
        switch (stage)
        {
            case 1:
                time -= 0.5f;
                break;
            case 2:
                time -= 1f;
                break;
            case 3:
                time -= 1.5f;
                break;
            default:
                time -= 0.1f;
                break;
        }


        }

        firstCard = null;
        secondCard = null;

    }

    
    
}